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
	// [RequireComponent(typeof(RectTransform))]
	public class UIView : UINode
	{
		#region INSPECTOR
		[SerializeField] private Image m_BackgroundImage;
		[SerializeField] private Mask m_Mask;
		#endregion

		/// <summary>
		/// 상태.
		/// </summary>
		private UIState m_State;
		
		/// <summary>
		/// 상태 프로퍼티.
		/// </summary>
		public UIState State { set => SetState(value); get => m_State; }

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
		public bool ClipsToBounds { set => SetMask(value); get => m_Mask.enabled; }
		
		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			
			// // 좌측상단 기준 셋업.
			// m_RectTransform.pivot = new Vector2(0f, 1f);
			// m_RectTransform.anchorMin = new Vector2(0f, 1f);
			// m_RectTransform.anchorMax = new Vector2(0f, 1f);
			// m_RectTransform.anchoredPosition = new Vector2(0f, 0f);
			
			Print("UIView", "Awake");

			// if (m_BackgroundImage == null)
			// 	m_BackgroundImage = gameObject.GetOrAddComponent<Image>();
			// if (m_Mask == null)
			// 	m_Mask = gameObject.GetOrAddComponent<Mask>();

			// m_BackgroundImage.color = Color.clear;
			// ClipsToBounds = false;
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
			RemoveAllChildren();
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
		/// 현재 뷰의 상태 설정.
		/// </summary>
		public void SetState(UIState state)
		{
			// 뷰 설정.
			m_State = state;
		}

		/// <summary>
		/// 배경 이미지 색상 조정.
		/// </summary>
		public void SetBackgroundImageColor(Color color)
		{
			if (color == Color.clear)
			{
				if (m_BackgroundImage != null)
					GameObject.Destroy(m_BackgroundImage);
				m_BackgroundImage = null;
			}
			else
			{
				if (m_BackgroundImage == null)
					m_BackgroundImage = gameObject.GetOrAddComponent<Image>();
				m_BackgroundImage.color = color;
			}
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
		/// 마스크 설정.
		/// </summary>
		public void SetMask(bool active)
		{
			if (active)
			{
				if (m_Mask == null)
					m_Mask = gameObject.GetOrAddComponent<Mask>();
			}
			else
			{
				if (m_Mask != null)
					GameObject.Destroy(m_Mask);
				m_Mask = null;		
			}
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
			var view = CreateNodeFromChildName<UIView>(childName);
			return view;
		}

		/// <summary>
		/// 대상 오브젝트로부터 뷰를 가져오고, 없으면 추가.
		/// </summary>
		public TUIView GetView<TUIView>(string childName) where TUIView : UIView
		{
			var view = CreateNodeFromChildName<TUIView>(childName);
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