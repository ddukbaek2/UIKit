using UIKit;
using UnityEngine;


/// <summary>
/// 유니티 애플리케이션.
/// </summary>
public static class UnityApplication
{
	/// <summary>
	/// 유니티 진입점.
	/// </summary>
	[RuntimeInitializeOnLoadMethod]
	private static void RuntimeInitializeOnLoad()
	{
		Debug.Log("UnityApplication.RuntimeInitializeOnLoad()");
		UIApplication.LaunchApplication(new UIExampleState());
	}
}