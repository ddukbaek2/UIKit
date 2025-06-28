using UIKit;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 빈 화면.
/// </summary>
public class UIEmptyState : UIModalState
{
	/// <summary>
	/// 생성됨.
	/// </summary>
	public UIEmptyState() : base()
	{
	}

	/// <summary>
	/// 해제됨.
	/// </summary>
	protected override void OnDispose(bool explicitDisposing)
	{
		base.OnDispose(explicitDisposing);
	}

	/// <summary>
	/// 뷰 로드 시작됨.
	/// </summary>
	protected override UIView OnViewLoadStart()
	{
		// 타이틀 패널 생성.
		var view = UIView.CreateUIViewFromAsset<UIEmptyPanel>();
		// var verticalLayoutGroupView = view.GetView("VerticalLayoutGroup");

		// // 종료 버튼 등록.
		// var exitButtonView = verticalLayoutGroupView.GetView("ExitButton");
		// var button = exitButtonView.GetComponent<Button>();
		// button.onClick.AddListener(OnClickExit);
		return view;
	}

	/// <summary>
	/// 종료 버튼 눌림.
	/// </summary>
	private void OnClickExit()
	{
		Debug.Log("UIDesktopState.OnClickExit()");
#if UNITY_EDITOR
		UnityEditor.EditorApplication.ExitPlaymode();
#else
		Application.Quit();
#endif
	}
}