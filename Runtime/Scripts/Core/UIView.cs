using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIKit
{
	/// <summary>
	/// 물리적 UI 단위 처리.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class UIView : UIBase
	{
		#region INSPECTOR
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
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			m_Parent = null;
			m_Children = new List<UIView>();
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
		/// 크기가 변경됨.
		/// </summary>
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
		}

		/// <summary>
		/// 부모가 변경됨.
		/// </summary>
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();

			// 직계 부모가 변경 되었을 때, 직계 부모의 변경을 통지한다.
			var parentView = RectTransform.parent.GetComponent<UIView>();
			if (parentView != null)
			{
				OnParentViewChanged(parentView);
			}

			// 직계 부모가 변경되었을 때, 직계 부모에게 직계 자식의 변경을 통지한다.
			if (m_Parent != null)
				m_Parent.OnChildViewChanged(this);
		}

		/// <summary>
		/// 직계 부모가 변경됨.
		/// </summary>
		protected virtual void OnParentViewChanged(UIView parent)
		{
		}

		/// <summary>
		/// 직계 자식이 변경됨.
		/// </summary>
		protected virtual void OnChildViewChanged(UIView view)
		{
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
		public void SetParent(UIView view)
		{
			m_Parent = view;
			if (m_Parent != null)
			{
				SetParentRectTransform(view.RectTransform);
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
		public bool HasView(UIView view)
		{
			return m_Children.Contains(view);
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
				var component = AssetLoader.InstantiateWithComponent(instanceType, typeof(UIView));
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