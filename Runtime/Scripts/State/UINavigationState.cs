using System.Collections.Generic;


namespace UIKit
{
	/// <summary>
	/// 네비게이션 상태.
	/// </summary>
	public class UINavigationState : UIState
	{
		/// <summary>
		/// 상태 스택.
		/// </summary>
		private Stack<UIState> m_Stack;

		/// <summary>
		/// 최신 상태 프로퍼티.
		/// </summary>
		private UIState Top
		{
			get
			{
				if (m_Stack.Count == 0)
					return null;

				return m_Stack.Peek();
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UINavigationState(UIView view = null, UIWindow window = null) : base(view, window)
		{
			m_Stack = new Stack<UIState>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			base.OnDispose(explicitDisposing);
		}
		
		/// <summary>
		/// 현재 상태 안에서 다음 상태 쌓고 진입.
		/// </summary>
		public void Push(UIState pushState, bool animated)
		{
			m_Stack.Push(pushState);
			
			UIState.EnterStateProcess(Window, this, pushState, animated);
		}

		/// <summary>
		/// 현재 상태 안에서 최신 상태 탈출.
		/// </summary>
		public void Pop(bool animated)
		{
			if (m_Stack.Count <= 1)
				return;

			var currentState = m_Stack.Pop();
			var previousState = default(UIState);
			if (m_Stack.Count > 0)
			{
				previousState = m_Stack.Peek();
			}
			
			UIState.ExitStateProcess(currentState, previousState, animated);
		}
	}
}