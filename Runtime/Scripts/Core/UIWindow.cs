using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UIKit
{
	/// <summary>
	/// UI 윈도우.
	/// <para>실제 시각적 출력을 위한 뷰 ㅎ 계층의 시작점.</para>.
	/// <para>독립된 UI 출력 단위.</para>
	/// </summary>
	[AssetPath("UIWindow"), RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
	public class UIWindow : UINode
	{
		#region INSPECTOR
		[SerializeField] private Canvas m_Canvas;
		[SerializeField] private CanvasScaler m_CanvasScaler;
		[SerializeField] private GraphicRaycaster m_GraphicRaycaster;
		#endregion

		/// <summary>
		/// 키 윈도우 여부.
		/// </summary>
		private bool m_IsKeyWindow;

		/// <summary>
		/// 씬.
		/// </summary>
		private UIScene m_Scene;

		/// <summary>
		/// 상태.
		/// </summary>
		private UIState m_RootState;

		/// <summary>
		/// 키 윈도우 여부 프로퍼티.
		/// </summary>
		public bool IsKeyWindow => m_IsKeyWindow;

		/// <summary>
		/// 씬 프로퍼티.
		/// </summary>
		public UIScene Scene { set => SetScene(value); get => m_Scene; }

		/// <summary>
		/// 상태 프로퍼티.
		/// </summary>
		public UIState RootState { set => SetRootState(value); get => m_RootState; }

		/// <summary>
		/// 출력 순서 설정.
		/// </summary>
		public int Order { set => m_Canvas.sortingOrder = value; get => m_Canvas.sortingOrder; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			Print("UIWindow", "Awake");

			m_IsKeyWindow = false;
			m_Scene = null;
			m_RootState = null;

			// 컴포넌트 연결.
			m_Canvas = GetComponent<Canvas>();
			m_CanvasScaler = GetComponent<CanvasScaler>();
			m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		/// <summary>
		/// 씬 설정.
		/// </summary>
		public void SetScene(UIScene scene)
		{
			// 이전 처리.
			if (m_Scene != null)
			{
			}

			m_Scene = scene;

			// 이후 처리.
			if (m_Scene != null)
			{
				// 부모 설정.
				//SetParentRectTransform(m_Scene.Application.UIKitTransform);
			}
		}

		/// <summary>
		/// 컨트롤러 설정.
		/// </summary>
		public void SetRootState(UIState rootState)
		{
			// 이전 처리.
			if (m_RootState != null)
			{
			}

			m_RootState = rootState;

			// 이후 처리.
			if (m_RootState != null)
			{
				// 윈도우 설정.
				m_RootState.SetWindow(this);

				// 키 윈도우로 만들고 화면에 표시.
				// Present();
			}
		}

		/// <summary>
		/// 현재 윈도우를 키 윈도우로 만듬 + 현재 윈도우의 상태를 화면에 표시.
		/// <para>makeKeyAndVisible()의 암시적 설계 이슈 따라가지 않음.</para>
		/// </summary>
		[Obsolete("")]
		public void MakeKeyAndVisible()
		{
			Present(true);
		}
		
		/// <summary>
		/// 현재 윈도우를 키 윈도우로 만듬 + 현재 윈도우의 상태를 화면에 표시.
		/// <para>makeKeyAndVisible()의 암시적 설계 이슈 따라가지 않음.</para>
		/// </summary>
		public void Present(bool useKeyWindow = false)
		{
			if (m_RootState == null)
				throw new NullReferenceException();

			// 현재 윈도우를 키 윈도우로 변경.
			if (useKeyWindow)
			{
				var windowCount = m_Scene.Windows.Count;
				for (var i = 0; i < windowCount; ++i)
				{
					var window = m_Scene.Windows[i];
					window.m_IsKeyWindow = false;
				}
				m_IsKeyWindow = true;
			}
			
			UIState.EnterStateProcess(this, null, m_RootState, false);
		}

		/// <summary>
		/// 보이기/감추기 설정.
		/// </summary>
		public virtual void SetVisible(bool visible)
		{
			// gameObject.SetActive(visible);
			m_Canvas.enabled = visible;
		}
	}
}