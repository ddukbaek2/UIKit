using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UIKit
{
	/// <summary>
	/// 기본 컴포넌트.
	/// </summary>
	// [RequireComponent(typeof(RectTransform))]
	public class UINode : UIBehaviour
	{
		/// <summary>
		/// 렉트 트랜스폼.
		/// </summary>
		private RectTransform m_RectTransform;
		
		/// <summary>
		/// 부모 노드.
		/// </summary>
		private UINode m_Parent;

		/// <summary>
		/// 자식 목록 노드.
		/// </summary>
		private List<UINode> m_Children;
		
		/// <summary>
		/// UI 트랜스폼 프로퍼티.
		/// </summary>
		public RectTransform RectTransform
		{
			get
			{
				if (m_RectTransform == null)
				{
					m_RectTransform = GetComponent<RectTransform>();
					if (m_RectTransform == null)
					{
						throw new UnityException("m_RectTransform is null.");
					}
					else
					{
						return m_RectTransform;
					}
				}
				else
				{
					return m_RectTransform;
				}
			}
		}

		/// <summary>
		/// 루트 노드 여부 프로퍼티.
		/// </summary>
		public bool IsRoot => m_Parent == null;

		/// <summary>
		/// 리프 노드 여부 프로퍼티.
		/// </summary>
		public bool IsLeaf => m_Children.Count == 0;

		/// <summary>
		/// 루트 노드 프로퍼티.
		/// </summary>
		public UINode Root
		{
			get
			{
				var view = this;
				while (view.m_Parent != null)
					view = view.m_Parent;
				return view;
			}
		}

		
		/// <summary>
		/// 리프 노드 목록 프로퍼티.
		/// </summary>
		public List<UINode> Leaves
		{
			get
			{
				var nodes = new List<UINode>();
				void Recursive(UINode node)
				{
					for (var i = 0; i < node.m_Children.Count; ++i)
					{
						var child = m_Children[i];
						Recursive(child);
					}
					
					if (node.IsLeaf)
						nodes.Add(node);
				}

				Recursive(this);
				
				return nodes;
			}
		}

		/// <summary>
		/// 부모 노드 프로퍼티.
		/// </summary>
		public UINode Parent { set => SetParent(value); get => m_Parent; }

		/// <summary>
		/// 자식 목록 노드 프로퍼티.
		/// </summary>
		public List<UINode> Children => m_Children;

		/// <summary>
		/// 형제의 갯수 설정 프로퍼티.
		/// </summary>
		public int SiblingCount => m_RectTransform.parent != null ? m_RectTransform.parent.childCount : 0;
		
		/// <summary>
		/// 형제 순서 설정 프로퍼티.
		/// </summary>
		public int SiblingIndex { set => m_RectTransform.SetSiblingIndex(value); get => m_RectTransform.GetSiblingIndex(); }

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			Print("UINode", "Awake");

			if (m_RectTransform == null)
				m_RectTransform = GetComponent<RectTransform>();

			m_Parent = null;
			m_Children = new List<UINode>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		/// <summary>
		/// 부모가 변경됨.
		/// </summary>
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
		}

		/// <summary>
		/// 자식이 변경됨.
		/// </summary>
		protected virtual void OnTransformChildrenChanged()
		{
		}
		
		/// <summary>
		/// 크기가 변경됨.
		/// </summary>
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
		}
		
		/// <summary>
		/// UIView 계층구조 갱신.
		/// </summary>
		public void UpdateViewHierarchy(Action<UINode> onParentChanged = null, Action<UINode> onChildAdded = null, Action<UINode> onChildRemoved = null, Action<int, int, UINode> onChildChanged = null)
		{
			// 부모 변경 감지.
			var parentTransform = RectTransform.parent;
			if (parentTransform != null)
			{
				var parent = parentTransform.GetComponent<UINode>();
				if (m_Parent == null)
				{
					if (parent != null)
					{
						m_Parent = parent;
						onParentChanged?.Invoke(m_Parent);
					}
				}
				else if (parent == null)
				{
					m_Parent = null;
					onParentChanged?.Invoke(m_Parent);
				}
				else if (m_Parent != parent)
				{
					m_Parent = parent;
					onParentChanged?.Invoke(m_Parent);
				}
			}
			else
			{
				if (m_Parent != null)
				{
					m_Parent = null;
					onParentChanged?.Invoke(m_Parent);
				}
			}

			// 자식 변경 감지.
			var children = new List<UINode>();
			var childCount = RectTransform.childCount;
			for (var i = 0; i < childCount; ++i)
			{
				var childTransform = m_RectTransform.GetChild(i);
				var child = childTransform.GetComponent<UINode>();
				if (child == null)
					continue;
				
				children.Add(child);
			}
			
			// 추가됨.
			childCount = children.Count;
			for (var i = 0; i < childCount; ++i)
			{
				var child = children[i];
				if (m_Children.Contains(child))
					continue;

				onChildAdded?.Invoke(child);
			}
			
			// 제거됨.
			childCount = m_Children.Count;
			for (var i = 0; i < childCount; ++i)
			{
				var child = m_Children[i];
				if (child == null || children.Contains(child))
					continue;
				
				onChildRemoved?.Invoke(child);
			}

			// 순서 변경됨.
			childCount = m_Children.Count;
			for (var i = 0; i < childCount; ++i)
			{
				var child = m_Children[i];
				var nextIndex = children.IndexOf(child);

				// 제거된 것 제외.
				if (nextIndex == -1)
					continue;

				// 이전과 인덱스가 같은 것 제외.
				if (i == nextIndex)
					continue;
				
				onChildChanged?.Invoke(i, nextIndex, child);
			}

			// 덮어쓰기.
			m_Children.Clear();
			m_Children.AddRange(children);
		}
	
		/// <summary>
		/// 부모 노드 설정.
		/// </summary>
		public virtual void SetParent(UINode node)
		{
			m_Parent = node;
			if (m_Parent != null)
			{
				SetParentRectTransform(node.RectTransform);
			}
		}

		/// <summary>
		/// 자식 노드 추가.
		/// </summary>
		public virtual void AddChild(UINode node)
		{
			if (node == null)
				return;

			node.SetParent(this);
			m_Children.Add(node);
		}

		/// <summary>
		/// 자식 노드 제거.
		/// </summary>
		public virtual void RemoveChild(UINode node)
		{
			if (node == null)
				return;

			node.SetParent(null);
			m_Children.Remove(node);
		}

		/// <summary>
		/// 모든 자식 노드 제거.
		/// </summary>
		public virtual void RemoveAllChildren()
		{
			var children = new List<UINode>(m_Children);
			foreach (var child in children)
			{
				RemoveChild(child);
			}
		}
		
		/// <summary>
		/// 자식 노드에 포함되어있는지 여부.
		/// </summary>
		public virtual bool HasChild(UINode node)
		{
			return m_Children.Contains(node);
		}
		
		/// <summary>
		/// 좌상 기준의 오브젝트 설정.
		/// </summary>
		public void ResetLocation()
		{
			// 좌측상단 기준 셋업.
			m_RectTransform.pivot = new Vector2(0f, 1f);
			m_RectTransform.anchorMin = new Vector2(0f, 1f);
			m_RectTransform.anchorMax = new Vector2(0f, 1f);
			m_RectTransform.anchoredPosition = new Vector2(0f, 0f);
		}
		
		/// <summary>
		/// 부모 설정.
		/// </summary>
		public void SetParentRectTransform(Transform parentTransform)
		{
			var offsetMin = m_RectTransform.offsetMin;
			var offsetMax = m_RectTransform.offsetMax;
			// var pivot = m_RectTransform.pivot;
			// var anchorMin = m_RectTransform.anchorMin;
			// var anchorMax = m_RectTransform.anchorMax;
			// var anchoredPosition = m_RectTransform.anchoredPosition;
			// var anchoredPosition3D = m_RectTransform.anchoredPosition3D;
			// var translation = m_RectTransform.position;
			// var rotation = m_RectTransform.rotation;
			// var localScale = m_RectTransform.localScale;

			m_RectTransform.SetParent(parentTransform, false);
			//m_RectTransform.offsetMin = new Vector2(0f, 0f);
			//m_RectTransform.offsetMax = new Vector2(1f, 1f);
			m_RectTransform.offsetMin = offsetMin;
			m_RectTransform.offsetMax = offsetMax;
		}

		///// <summary>
		///// 컴포넌트 반환.
		///// </summary>
		//public TComponent FindComponent<TComponent>(string path) where TComponent : Component
		//{
		//	var transformRoute = new TransformRoute(RectTransform);
		//	var transform = transformRoute.FindRelativeTransform(path);
		//	return transform.GetComponent<TComponent>();
		//}

		///// <summary>
		///// 버튼 눌림 이벤트를 외부로 연결.
		///// </summary>
		//public void AddButtonClickEvent(string path, UnityAction onClick)
		//{
		//	var button = FindComponent<Button>(path);
		//	button.onClick.AddListener(onClick);
		//}

		/// <summary>
		/// 로그 출력.
		/// </summary>
		public void Print(string category, string methodName)
		{
			var type = GetType();
			Debug.Log($"[{category}] {type.Name}.{methodName}()");
		}
	}
}