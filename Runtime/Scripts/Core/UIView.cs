using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UIKit
{
	/// <summary>
	/// 물리적 UI 단위 처리.
	/// <para>규칙1 : 모든 UIView가 부착된 루트 오브젝트에는 Image + Mask를 둔다.</para>
	/// </summary>
	[RequireComponent(typeof(RectTransform), typeof(Image))]
	public class UIView : UIBase
	{
		#region INSPECTOR
		[SerializeField] private Image m_BackgroundImage;
		[SerializeField] private Mask m_Mask;
		// [SerializeField] private bool m_ClipsToBounds;
		#endregion

		/// <summary>
		/// 상태.
		/// </summary>
		private UIState m_State;

		/// <summary>
		/// 부모 뷰.
		/// </summary>
		private UIView m_Parent;

		/// <summary>
		/// 자식 뷰 목록.
		/// </summary>
		private List<UIView> m_Children;

		/// <summary>
		/// 상태 프로퍼티.
		/// </summary>
		public UIState State { set => SetState(value); get => m_State; }

		/// <summary>
		/// 루트 뷰 프로퍼티.
		/// </summary>
		public UIView Root
		{
			get
			{
				var view = this;
				while (view.Parent != null)
					view = view.Parent;
				return view;
			}
		}

		/// <summary>
		/// 부모 뷰 프로퍼티.
		/// </summary>
		public UIView Parent { set => SetParent(value); get => m_Parent; }

		/// <summary>
		/// 자식 뷰 목록 프로퍼티.
		/// </summary>
		public List<UIView> Children => m_Children;

		/// <summary>
		/// 배경색 프로퍼티.
		/// </summary>
		public Color BackgroundColor { set => SetBackgroundImageColor(value); get => m_BackgroundImage?.color ?? Color.clear; }

		// /// <summary>
		// /// 부모의 위치 + 크기가 고려된 위치 + 크기 프로퍼티.
		// /// </summary>
		// public Rect Frame
		// {
		// 	
		// }
		
		// /// <summary>
		// /// 자신의 위치 + 크기 프로퍼티.
		// /// </summary>
		// public Rect Bounds
		// {
		// 	set
		// 	{
		// 		
		// 	}
		// 	get
		// 	{
		// 		
		// 	}
		// }
		
		/// <summary>
		/// 마스킹 처리 프로퍼티.
		/// </summary>
		public bool ClipsToBounds
		{
			set
			{
				if (m_Mask == null)
					return;
				m_Mask.enabled = value;
			}
			get
			{
				if (m_Mask == null)
					return false;
				return m_Mask.enabled;
			}
		}
		
		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			
			// 좌측상단 기준 셋업.
			m_RectTransform.pivot = new Vector2(0f, 1f);
			m_RectTransform.anchorMin = new Vector2(0f, 1f);
			m_RectTransform.anchorMax = new Vector2(0f, 1f);
			m_RectTransform.anchoredPosition = new Vector2(0f, 0f);
			
			var type = GetType();
			Debug.Log($"UIView.Awake(): Class: \"{type.Name}\"");

			if (m_BackgroundImage == null)
				m_BackgroundImage = GetComponent<Image>();
			if (m_Mask == null)
				m_Mask = GetComponent<Mask>();
			
			m_Parent = null;
			m_Children = new List<UIView>();
			ClipsToBounds = false;
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			//if (m_Parent != null)
			//{
			//	m_Parent.RemoveChild((IView)this);
			//}
				
			base.OnDestroy();
		}

		/// <summary>
		/// 부모가 변경됨.
		/// </summary>
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			UpdateViewHierarchy();
		}

		/// <summary>
		/// 자식이 변경됨.
		/// </summary>
		protected override void OnTransformChildrenChanged()
		{
			base.OnTransformChildrenChanged();
			UpdateViewHierarchy();
		}
		
		/// <summary>
		/// UIView 계층구조 갱신.
		/// </summary>
		public void UpdateViewHierarchy(Action<UIView> onParentChanged = null, Action<UIView> onChildAdded = null, Action<UIView> onChildRemoved = null, Action<int, int, UIView> onChildChanged = null)
		{
			// 부모 변경 감지.
			var parentTransform = RectTransform.parent;
			if (parentTransform != null)
			{
				var parent = parentTransform.GetComponent<UIView>();
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
			var children = new List<UIView>();
			var childCount = RectTransform.childCount;
			for (var i = 0; i < childCount; ++i)
			{
				var childTransform = m_RectTransform.GetChild(i);
				var child = childTransform.GetComponent<UIView>();
				if (child == null)
					continue;
				
				children.Add(child);
			}
			
			// 추가됨.
			for (var i = 0; i < children.Count; ++i)
			{
				var child = children[i];
				if (m_Children.Contains(child))
					continue;

				onChildAdded?.Invoke(child);
			}
			
			// 제거됨.
			for (var i = 0; i < m_Children.Count; ++i)
			{
				var child = m_Children[i];
				if (child == null || children.Contains(child))
					continue;
				
				onChildRemoved?.Invoke(child);
			}

			// 순서 변경됨.
			for (var i = 0; i < m_Children.Count; ++i)
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
		/// 현재 뷰의 상태 설정.
		/// </summary>
		public void SetState(UIState state)
		{
			// 뷰 설정.
			m_State = state;
		}

		/// <summary>
		/// 직계 부모 뷰 설정.
		/// </summary>
		public void SetParent(UIView parentView)
		{
			m_Parent = parentView;
			if (m_Parent != null)
			{
				SetParentRectTransform(parentView.RectTransform);
			}
		}

		/// <summary>
		/// 직계 자식 뷰 추가.
		/// </summary>
		public void AddChild(UIView view)
		{
			if (view == null)
				return;

			if (view.State.View == view)
				return;

			view.SetParent(this);
			view.SetState(State);
			m_Children.Add(view);
		}

		/// <summary>
		/// 직계 자식 뷰 제거.
		/// </summary>
		public void RemoveChild(UIView view)
		{
			if (view == null)
				return;

			view.SetParent(null);
			m_Children.Remove(view);
		}

		/// <summary>
		/// 모든 직계 자식 뷰 제거.
		/// </summary>
		public void RemoveAllChildren()
		{
			var children = new List<UIView>(m_Children);
			foreach (var view in children)
			{
				RemoveChild(view);
			}
		}

		/// <summary>
		/// 직계 자식으로 해당 뷰가 포함되어있는지 여부.
		/// </summary>
		public bool HasChild(UIView view)
		{
			return m_Children.Contains(view);
		}

		/// <summary>
		/// 배경 이미지 색상 조정.
		/// </summary>
		public void SetBackgroundImageColor(Color color)
		{
			if (m_BackgroundImage == null)
				return;
			
			m_BackgroundImage.color = color;
		}
		
		/// <summary>
		/// 보이기/감추기 설정.
		/// <para>UIView는 투명도 처리를 지원하지 않음.</para>
		/// </summary>
		public virtual void SetVisible(bool visible, bool animated)
		{
			if (animated)
			{
				Debug.LogWarning("[UIView] SetVisible() is not supported animation.");
			}
			
			gameObject.SetActive(visible);
		}
		
		/// <summary>
		/// 새로운 UIView 생성.
		/// </summary>
		public static TUIView CreateUIView<TUIView>() where TUIView : UIView
		{
			// 객체 및 컴포넌트 생성.
			var instanceType = typeof(TUIView);
			var obj = new GameObject(instanceType.Name);
			var view = obj.AddComponent<TUIView>();

			// 임시 위치 생성.
			view.SetParentRectTransform(UIApplication.Instance.UIKitTransform);
			return view;
		}

		/// <summary>
		/// 대상 오브젝트로부터 뷰를 가져오고, 없으면 추가.
		/// </summary>
		public UIView GetView(string childName)
		{
			return GetView<UIView>(childName);
		}

		/// <summary>
		/// 대상 오브젝트로부터 뷰를 가져오고, 없으면 추가.
		/// </summary>
		public TUIView GetView<TUIView>(string childName) where TUIView : UIView
		{
			var target = RectTransform.Find(childName);
			if (target == null)
				return null;

			var view = target.GetComponent<TUIView>();
			if (view == null)
				view = target.gameObject.AddComponent<TUIView>();
			return view;
		}

		/// <summary>
		/// 애셋으로부터 UIView 생성.
		/// </summary>
		public static UIView CreateUIViewFromAsset(Type instanceType)
		{
			try
			{
				var component = AssetLoader.InstantiateWithComponentFromAssetPath(instanceType, typeof(UIView));
				return component as UIView;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 애셋으로부터 UIView 생성.
		/// </summary>
		public static TUIView CreateUIViewFromAsset<TUIView>() where TUIView : UIView
		{
			var instanceType = typeof(TUIView);
			try
			{
				var view = CreateUIViewFromAsset(instanceType);
				return view as TUIView;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
	}
}