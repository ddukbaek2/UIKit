using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace UIKit
{
	/// <summary>
	/// UI 애플리케이션.
	/// <para>애플리케이션에 단 하나만 존재함.</para>
	/// </summary>
	public class UIApplication : Disposable
	{
		/// <summary>
		/// 공유 인스턴스 프로퍼티.
		/// </summary>
		public static UIApplication Instance => Repository.Get<UIApplication>(false);

		/// <summary>
		/// 루트 트랜스폼
		/// </summary>
		private Transform m_UIKitTransform;

		/// <summary>
		/// 이벤트 시스템.
		/// </summary>
		private EventSystem m_EventSystem;

		/// <summary>
		/// 연결된 씬 목록.
		/// </summary>
		private List<UIScene> m_Scenes;

		/// <summary>
		/// 루트 트랜스폼 프로퍼티.
		/// </summary>
		public Transform UIKitTransform => m_UIKitTransform;

		/// <summary>
		/// 이벤트 시스템 프로퍼티.
		/// </summary>
		public EventSystem EventSystem => m_EventSystem;

		/// <summary>
		/// 씬 목록 프로퍼티.
		/// </summary>
		public List<UIScene> Scenes { set => SetScenes(value); get => m_Scenes; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIApplication() : base()
		{
			// 객체 등록.
			Repository.Register<UIApplication>(this);

			// 씬 추가.
			m_Scenes = new List<UIScene>();

			// // 루트 트랜스폼 생성.
			// CreateUIKitTransform();
			//
			// // 기본 이벤트 시스템 생성.
			// CreateEventSystem();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 루트 트랜스폼 생성.
		/// </summary>
		public Transform CreateUIKitTransform(string defaultName = "UIKit")
		{
			if (m_UIKitTransform != null)
				return m_UIKitTransform;
				
			var uikit = GameObject.Find(defaultName);
			if (uikit == null)
				uikit = new GameObject(defaultName);
			uikit.layer = LayerMask.NameToLayer("UI");
			GameObject.DontDestroyOnLoad(uikit);
			m_UIKitTransform = uikit.GetComponent<Transform>();		
			return m_UIKitTransform;
		}

		/// <summary>
		/// 이벤트 시스템 생성.
		/// </summary>
		public EventSystem CreateEventSystem(string defaultAssetPath = "EventSystem")
		{
			if (m_EventSystem != null)
				return m_EventSystem;
			
			var obj = AssetLoader.Instantiate(defaultAssetPath);
			obj.name = "EventSystem";
			obj.transform.SetParent(m_UIKitTransform);
			m_EventSystem = obj.GetComponent<EventSystem>();
			return m_EventSystem;
		}
		
		/// <summary>
		/// 씬 설정.
		/// </summary>
		public void SetScene(UIScene scene)
		{
			if (scene == null)
				return;

			RemoveAllScenes();
			AddScene(scene);
		}

		/// <summary>
		/// 씬 설정.
		/// </summary>
		public void SetScenes(List<UIScene> scenes)
		{
			if (scenes == null)
				return;

			var count = scenes.Count;
			if (count == 0)
				return;

			RemoveAllScenes();
			AddScenes(scenes);
		}

		/// <summary>
		/// 씬 추가.
		/// </summary>
		public bool AddScene(UIScene scene)
		{
			if (scene == null || scene.IsDisposed)
				return false;
			if (m_Scenes.Contains(scene))
				return false;

			m_Scenes.Add(scene);
			return true;
		}

		/// <summary>
		/// 씬 추가.
		/// </summary>
		public void AddScenes(List<UIScene> scenes)
		{
			if (scenes == null)
				return;

			var count = scenes.Count;
			if (count == 0)
				return;

			for (var i = 0; i < count; ++i)
			{
				var scene = scenes[i];
				AddScene(scene);
			}
		}

		/// <summary>
		/// 씬 제거.
		/// </summary>
		public bool RemoveScene(UIScene scene)
		{
			if (scene == null || scene.IsDisposed)
				return false;
			if (!m_Scenes.Contains(scene))
				return false;

			m_Scenes.Remove(scene);
			scene.Dispose();
			return true;
		}

		/// <summary>
		/// 모든 씬 제거.
		/// </summary>
		public void RemoveAllScenes()
		{
			var scenes = new List<UIScene>(m_Scenes);
			var count = scenes.Count;
			for (var i = 0; i < count; ++i)
			{
				var scene = scenes[i];
				RemoveScene(scene);
			}
		}
		
		/// <summary>
		/// 실행.
		/// </summary>
		public void Launch()
		{
			var count = m_Scenes.Count;
			for (var i = 0; i < count; ++i)
			{
				var scene = m_Scenes[i];
				//scene.
			}
		}

		/// <summary>
		/// 단순하게 실행.
		/// </summary>
		public static void LaunchSimpleApplication<TUIApplication>(UIState state) where TUIApplication : UIApplication, new()
		{
			var application = new TUIApplication();
			application.CreateUIKitTransform();
			application.CreateEventSystem();

			var scene = new UIWindowScene(application);
			application.AddScene(scene);
			scene.Window.SetRootState(state);
			scene.Window.Present();
			application.Launch();
		}

		/// <summary>
		/// 종료.
		/// </summary>
		public static void ExitApplication()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}
	}
}