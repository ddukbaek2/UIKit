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
		public EventSystem EventSystem { set => SetEventSystem(value); get => m_EventSystem; }

		/// <summary>
		/// 씬 목록 프로퍼티.
		/// </summary>
		public List<UIScene> Scenes { set => SetScenes(value); get => m_Scenes; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIApplication(UIScene scene = null, bool useDefaultEventSystem = true) : base()
		{
			// 루트 트랜스폼 생성.
			var obj = new GameObject("UIKit");
			obj.layer = LayerMask.NameToLayer("UI");
			GameObject.DontDestroyOnLoad(obj);
			m_UIKitTransform = obj.GetComponent<Transform>();

			// 기본 벤트 시스템 생성.
			if (useDefaultEventSystem)
			{
				var asset = Resources.Load<GameObject>("EventSystem");
				obj = GameObject.Instantiate<GameObject>(asset);
				obj.name = "EventSystem";
				obj.transform.SetParent(m_UIKitTransform);
				var eventSystem = obj.GetComponent<EventSystem>();
				SetEventSystem(eventSystem);
			}

			// 객체 등록.
			Repository.Register<UIApplication>(this);

			// 씬 추가.
			m_Scenes = new List<UIScene>();
			AddScene(scene);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			SetEventSystem(null);
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
		/// 이벤트시스템 설정.
		/// </summary>
		public void SetEventSystem(EventSystem eventSystem)
		{
			if (m_EventSystem != null)
			{
				GameObject.Destroy(m_EventSystem.gameObject);
			}

			m_EventSystem = eventSystem;
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
		/// 실행.
		/// </summary>
		public static void LaunchApplication(UIState state)
		{
			var application = new UIApplication();
			application.AddScene(new UIWindowScene(state, application));
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