namespace UIKit
{
	/// <summary>
	/// 모달 상태.
	/// </summary>
	public class UIModalState : UIState
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIModalState(UIView view = null, UIWindow window = null) : base(view, window)
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			base.OnDispose(explicitDisposing);
		}
	}
}