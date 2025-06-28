namespace UIKit
{
	/// <summary>
	/// 윈도우를 가진 씬.
	/// </summary>
	public class UIWindowScene : UIScene
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIWindowScene(UIState state = null, UIApplication application = null) : base(null, null)
		{
			// 애플리케이션 설정.
			SetApplication(application);

			// 기본 UIWindow 로드.
			var window = AssetLoader.InstantiateWithComponent<UIWindow>();

			// 윈도우를 소유한 씬을 설정.
			window.SetScene(this);
			
			// 윈도우에서 처리할 상태를 설정.
			window.SetState(state);

			// 윈도우를 씬에 추가.
			AddWindow(window);
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