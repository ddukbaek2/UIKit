using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UIKit
{
	/// <summary>
	/// UI를 위한 컴포넌트.
	/// <para>INode 인터페이스 구현체.</para>
	/// </summary>
	// [RequireComponent(typeof(RectTransform))]
	public class UINode : UIBehaviour, INode
	{
		/// <summary>
		/// 렉트 트랜스폼.
		/// </summary>
		private RectTransform m_RectTransform;
		
		/// <summary>
		/// 부모 노드.
		/// </summary>
		private INode m_Parent;

		/// <summary>
		/// 자식 목록 노드.
		/// </summary>
		private List<INode> m_Children;
		
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
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public bool IsRoot => NodeUtility.IsRoot(this);

		/// <summary>
		/// 리프 노드 여부 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public bool IsLeaf => NodeUtility.IsLeaf(this);

		/// <summary>
		/// 루트 노드 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public INode Root => NodeUtility.GetRoot<UINode>(this);

		/// <summary>
		/// 리프 노드 목록 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public List<INode> Leaves => NodeUtility.GetLeaves<INode>(this);

		/// <summary>
		/// 부모 노드 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public INode Parent { set => SetParent(value); get => m_Parent; }

		/// <summary>
		/// 자식 목록 노드 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public List<INode> Children => m_Children;

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
			m_Children = new List<INode>();
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
		public void UpdateViewHierarchy(Action<INode> onParentChanged = null, Action<INode> onChildAdded = null, Action<INode> onChildRemoved = null, Action<int, int, INode> onChildChanged = null)
		{
			// 부모 변경 감지.
			var parentTransform = RectTransform.parent;
			if (parentTransform != null)
			{
				var parent = parentTransform.GetComponent<INode>();
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
		public virtual void SetParent(INode node)
		{
			m_Parent = node;
			if (m_Parent != null)
			{
				if (node is UINode)
				{
					SetParentRectTransform(((UINode)node).RectTransform);
				}
			}
		}

		/// <summary>
		/// 자식 노드 추가.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public void AddChild(INode node)
		{
			NodeUtility.AddChild(this, node);
		}

		/// <summary>
		/// 자식 노드 제거.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public void RemoveChild(INode node)
		{
			NodeUtility.RemoveChild(this, node);
		}

		/// <summary>
		/// 모든 자식 노드 제거.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public void RemoveAllChildren()
		{
			NodeUtility.RemoveAllChildren(this);
		}
		
		/// <summary>
		/// 대상 노드가 자식 노드에 포함되는지 여부.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public bool IsChild(INode node)
		{
			return NodeUtility.IsChild(this, node);
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
		
		
		/// <summary>
		/// 대상 오브젝트로부터 노드를 가져오고, 없으면 추가.
		/// </summary>
		public UINode CreateNodeFromChildName(string childName)
		{
			var view = CreateNodeFromChildName<UINode>(childName);
			return view;
		}

		/// <summary>
		/// 대상 오브젝트로부터 노드를 가져오고, 없으면 추가.
		/// </summary>
		public TUINode CreateNodeFromChildName<TUINode>(string childName) where TUINode : UINode
		{
			var target = RectTransform.Find(childName);
			if (target == null)
				return null;

			var view = target.GetComponent<TUINode>();
			if (view == null)
				view = target.gameObject.AddComponent<TUINode>();
			return view;
		}

		/// <summary>
		/// 프리팹으로부터 노드 생성.
		/// </summary>
		public static UINode CreateNodeFromAsset(Type instanceType)
		{
			try
			{
				var node = AssetLoader.InstantiateWithComponentFromAssetPath(instanceType, typeof(UINode));
				return node as UINode;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 프리팹으로부터 노드 생성.
		/// </summary>
		public static TUINode CreateNodeFromAsset<TUINode>() where TUINode : UINode
		{
			var instanceType = typeof(TUINode);
			try
			{
				var node = UINode.CreateNodeFromAsset(instanceType);
				return node as TUINode;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
	}
}