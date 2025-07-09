namespace UIKit
{
	/// <summary>
	/// 윈도우를 가진 씬.
	/// </summary>
	public class UIWindowScene : UIScene
	{
		/// <summary>
		/// 윈도우 프로퍼티.
		/// </summary>
		public UIWindow Window { private set; get; }
		
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIWindowScene(UIApplication application) : base()
		{
			// 애플리케이션 설정.
			SetApplication(application);

			// 기본 UIWindow 로드.
			Window = AssetLoader.InstantiateWithComponentFromAssetPath<UIWindow>();

			// 윈도우를 소유한 씬을 설정.
			Window.SetScene(this);
		
			// 윈도우를 씬에 추가.
			AddWindow(Window);

			// 윈도우에서 처리할 상태를 설정.
			// window.SetRootState(state);
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