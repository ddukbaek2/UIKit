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
	public class UIWindow : UIBase
	{
		#region INSPECTOR
		[SerializeField] private Canvas m_Canvas;
		[SerializeField] private CanvasScaler m_CanvasScaler;
		[SerializeField] private GraphicRaycaster m_GraphicRaycaster;
		#endregion

		/// <summary>
		/// 씬.
		/// </summary>
		private UIScene m_Scene;

		/// <summary>
		/// 상태.
		/// </summary>
		private UIState m_State;

		/// <summary>
		/// 씬 프로퍼티.
		/// </summary>
		public UIScene Scene { set => SetScene(value); get => m_Scene; }

		/// <summary>
		/// 상태 프로퍼티.
		/// </summary>
		public UIState State { set => SetState(value); get => m_State; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			var type = GetType();
			Debug.Log($"UIWindow.Awake(): Class: \"{type.Name}\"");

			m_Scene = null;
			m_State = null;

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
		public void SetState(UIState state)
		{
			// 이전 처리.
			if (m_State != null)
			{
			}

			m_State = state;

			// 이후 처리.
			if (m_State != null)
			{
				// 윈도우 설정.
				m_State.SetWindow(this);

				// 상태 진입 프로세스.
				UIState.EnterStateProcess(this, null, m_State, false);
			}
		}
	}
}