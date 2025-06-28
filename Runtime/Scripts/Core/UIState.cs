using System;


namespace UIKit
{
	/// <summary>
	/// UI 상태.
	/// <para>논리적 UI 단위 처리.</para>
	/// </summary>
	public class UIState : Disposable
	{
		/// <summary>
		/// 윈도우.
		/// <para>현재 컨트롤러를 소유한 렌더링 레이어 단위 객체.</para>
		/// </summary>
		private UIWindow m_Window;

		/// <summary>
		/// 현재 상태를 실행한 이전 상태.
		/// </summary>
		private UIState m_PreviousState;

		/// <summary>
		/// 뷰.
		/// <para>실제 UI 게임오브젝트</para>
		/// </summary>
		private UIView m_View;

		/// <summary>
		/// 현재 상태를 실행한 이전 상태 프로퍼티.
		/// </summary>
		public UIState PreviousState => m_PreviousState;

		/// <summary>
		/// 윈도우 프로퍼티.
		/// </summary>
		public UIWindow Window { set => SetWindow(value); get => m_Window; }

		/// <summary>
		/// 뷰 프로퍼티.
		/// </summary>
		public UIView View { set => SetView(value); get => m_View; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIState(UIView view = null, UIWindow window = null) : base()
		{
			m_Window = null;
			m_PreviousState = null;
			m_View = null;

			SetWindow(window);
			SetView(view);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 뷰 로드 시작됨.
		/// </summary>
		protected virtual UIView OnViewLoadStart()
		{
			return null;
		}

		/// <summary>
		/// 뷰 로드 완료됨.
		/// </summary>
		protected virtual void OnViewLoadComplete()
		{
		}

		/// <summary>
		/// 상태 진입 시작됨.
		/// </summary>
		protected virtual void OnEnterStart(UIState previousState, bool animated)
		{
			m_PreviousState = previousState;
		}

		/// <summary>
		/// 상태 진입 완료됨.
		/// </summary>
		protected virtual void OnEnterComplete()
		{
		}

		/// <summary>
		/// 상태 탈출 시작됨.
		/// </summary>
		protected virtual void OnExitStart(bool animated)
		{
		}

		/// <summary>
		/// 상태 탈출 완료됨.
		/// </summary>
		protected virtual void OnExitComplete()
		{
		}

		/// <summary>
		/// 소유 윈도우 설정.
		/// </summary>
		public void SetWindow(UIWindow window)
		{
			m_Window = window;
		}

		/// <summary>
		/// 루트 뷰 설정.
		/// </summary>
		public void SetView(UIView view)
		{
			// 전처리.
			//if (m_View != null)
			//{
			//	if (m_View.Parent != null)
			//	{
			//		m_View.Parent.RemoveChild(m_View);
			//		//GameObject.Destroy(m_View);
			//	}
			//}

			// 설정.
			m_View = view;
			
			// 후처리.
			if (m_View != null)
			{
				// 뷰의 소유자를 설정.
				m_View.SetState(this);
				//m_View.SetParentRectTransform(Window.RectTransform);
			}
		}

		/// <summary>
		/// 다음 상태로 진입.
		/// </summary>
		public void Present(UIState nextState, bool animated)
		{
			try
			{
				UIState.EnterStateProcess(Window, this, nextState, animated);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 현재 상태 탈출.
		/// </summary>
		public void Dismiss(bool animated)
		{
			try
			{
				UIState.ExitStateProcess(this, animated);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 상태 진입 프로세스.
		/// </summary>
		internal static void EnterStateProcess(UIWindow window, UIState previousState, UIState nextState, bool animated)
		{
			// 없는 상태로 진입 할 수 없음.
			if (nextState == null)
			{
				throw new ArgumentNullException();
			}

			// 이전과 동일한 상태로 진입 할 수 없음.
			if (previousState == nextState)
			{
				throw new ArgumentNullException();
			}

			nextState.SetWindow(window);

			// 뷰가 없으면 루트 뷰 최초 1회 생성.
			if (nextState.View == null)
			{
				var view = nextState.OnViewLoadStart();
				nextState.SetView(view);
				nextState.OnViewLoadComplete();
			}

			// 윈도우의 자식으로 추가.
			nextState.View.SetParentRectTransform(window.RectTransform);

			// 진입.
			nextState.OnEnterStart(previousState, animated);
			var panel = nextState.View as UIPanel;
			panel.SetVisible(true, animated);
			nextState.OnEnterComplete();
		}

		/// <summary>
		/// 상태 탈출 프로세스.
		/// </summary>
		internal static void ExitStateProcess(UIState state, bool animated)
		{
			if (state == null)
				throw new ArgumentNullException();

			// 탈출.
			state.OnExitStart(animated);
			var panel = state.View as UIPanel;
			panel.SetVisible(false, animated);
			state.OnExitComplete();
		}
	}
}