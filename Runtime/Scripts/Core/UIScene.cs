using System.Collections.Generic;
using UnityEngine;


namespace UIKit
{
	/// <summary>
	/// UI 씬.
	/// <para>애플리케이션 안에 실제 UI의 논리적 세션.</para>
	/// <para>하나의 실제 애플리케이션과 연결되는 이벤트 핸들러이자 애플리케이션 윈도우 같은 프로세스 개념.</para>
	/// <para>애플리케이션 내 생명주기 등 이벤트 처리.</para>
	/// </summary>
	public class UIScene : Disposable
	{
		/// <summary>
		/// 애플리케이션.
		/// </summary>
		private UIApplication m_Application;

		/// <summary>
		/// 윈도우 목록.
		/// </summary>
		private List<UIWindow> m_Windows;

		/// <summary>
		/// 애플리케이션 프로퍼티.
		/// </summary>
		public UIApplication Application { set => SetApplication(value); get => m_Application; }

		/// <summary>
		/// 윈도우 목록 프로퍼티.
		/// </summary>
		public List<UIWindow> Windows { set => SetWindows(value); get => m_Windows; }

		/// <summary>
		/// 키 윈도우 프로퍼티.
		/// </summary>
		public UIWindow KeyWindow
		{
			get
			{
				var windowCount = m_Windows.Count;
				for (var i = 0; i < windowCount; ++i)
				{
					var window = m_Windows[i];
					if (window.IsKeyWindow)
						return window;
				}

				return null;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIScene(UIWindow window = null, UIApplication application = null) : base()
		{
			m_Application = null;
			m_Windows = new List<UIWindow>();

			SetApplication(application);
			AddWindow(window);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 애플리케이션 설정.
		/// </summary>
		public void SetApplication(UIApplication application)
		{
			m_Application = application;
		}

		/// <summary>
		/// 윈도우 설정.
		/// </summary>
		public void SetWindow(UIWindow window)
		{
			if (window == null)
				return;

			RemoveAllWindows();
			AddWindow(window);
		}

		/// <summary>
		/// 윈도우 설정.
		/// </summary>
		public void SetWindows(List<UIWindow> windows)
		{
			if (windows == null)
				return;

			var count = windows.Count;
			if (count == 0)
				return;

			RemoveAllWindows();
			AddWindows(windows);
		}

		/// <summary>
		/// 윈도우 추가.
		/// </summary>
		public bool AddWindow(UIWindow window)
		{
			if (window == null)
				return false;
			if (m_Windows.Contains(window))
				return false;

			m_Windows.Add(window);
			window.SetScene(this);
			window.SetParentRectTransform(Application.UIKitTransform);
			return true;
		}

		/// <summary>
		/// 윈도우 추가.
		/// </summary>
		public void AddWindows(List<UIWindow> windows)
		{
			if (windows == null)
				return;

			var count = windows.Count;
			if (count == 0)
				return;

			for (var i = 0; i < count; ++i)
			{
				var window = windows[i];
				AddWindow(window);
			}
		}

		/// <summary>
		/// 윈도우 제거.
		/// </summary>
		public bool RemoveWindow(UIWindow window)
		{
			if (window == null)
				return false;
			if (!m_Windows.Contains(window))
				return false;

			window.SetScene(null);
			m_Windows.Remove(window);
			GameObject.Destroy(window);
			return true;
		}

		/// <summary>
		/// 모든 윈도우 제거.
		/// </summary>
		public void RemoveAllWindows()
		{
			var windows = new List<UIWindow>(m_Windows);
			var count = windows.Count;
			for (var i = 0; i < count; ++i)
			{
				var window = windows[i];
				RemoveWindow(window);
			}
		}
	}
}