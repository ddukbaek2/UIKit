//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;
//using UnityEngine.SceneManagement;


//namespace UIKit
//{
//	/// <summary>
//	/// 하이어라키상의 트랜스폼 경로를 만들거나 반환.
//	/// <para>기준이 되는 트랜스폼이 지정되지 않으면 루트를 기준으로 삼게 됨.</para>
//	/// <para>맨앞의 /이 붙으면 루트를 기준으로 한 절대경로, 안붙거나 ../이면 현재 트랜스폼을 기준으로 한 상대경로.</para>
//	/// <para>/{name}/{name}/ ... 방식으로 경로를 만들어줌.</para>
//	/// <para>/{name}/{name}:{index}/ ... 동일이름일 경우 다음과 같이 : 순서에 대한 색인 번호를 포함하여 지정 가능.(0 - siblingCount - 1)</para>
//	/// </summary>
//	public class TransformRoute : Disposable
//	{
//		/// <summary>
//		/// 경로의 기준점이 되는 트랜스폼.
//		/// </summary>
//		private Transform m_AnchoredTransform;

//		/// <summary>
//		/// 경로의 기준점이 되는 트랜스폼 프로퍼티.
//		/// </summary>
//		public Transform AnchoredTransform => m_AnchoredTransform;

//		/// <summary>
//		/// 절대 경로 프로퍼티.
//		/// </summary>
//		public string AbsolutePath => TransformRoute.CreateAbsolutePath(m_AnchoredTransform);

//		/// <summary>
//		/// 생성됨.
//		/// </summary>
//		public TransformRoute() : base()
//		{
//			m_AnchoredTransform = null;
//		}

//		/// <summary>
//		/// 생성됨.
//		/// </summary>
//		public TransformRoute(string path) : base()
//		{
//			m_AnchoredTransform = TransformRoute.FindAbsoluteTransform(path);
//		}

//		/// <summary>
//		/// 생성됨.
//		/// </summary>
//		public TransformRoute(Transform anchoredTransform) : base()
//		{
//			m_AnchoredTransform = anchoredTransform;
//		}

//		/// <summary>
//		/// 해제됨.
//		/// </summary>
//		protected override void OnDispose(bool explicitDisposing)
//		{
//		}

//		/// <summary>
//		/// 상대적인 트랜스폼을 찾음.
//		/// </summary>
//		public Transform FindRelativeTransform(string path)
//		{
//			return FindAbsoluteTransform($"{AbsolutePath}/{path}");
//		}

//		///// <summary>
//		///// 절대경로로 찾기.
//		///// </summary>
//		//public static Transform FindTransform(string absolutePath, Transform anchoredTransform = null)
//		//{
//		//	// 경로 문자열이 존재하지 않음.
//		//	if (string.IsNullOrWhiteSpace(absolutePath))
//		//		return null;

//		//	var transform = default(Transform);


//		//	// 기준 트랜스폼이 없다면 절대 경로.
//		//	// 절대경로의 경로 문자열은 반드시 / 로 시작하는 룰이 존재.
//		//	if (anchoredTransform == null)
//		//	{
//		//		// 맨 앞에 슬래시를 붙지 않아서 절대 경로로 볼 수 없음.
//		//		if (!absolutePath.StartsWith("/"))
//		//			return null;

//		//		var names = absolutePath.Split("/", System.StringSplitOptions.RemoveEmptyEntries);
//		//		var nameCount = names.Length;

//		//		// 경로 문법이 맞지 않음. (/{이름}/{이름}/ ...)
//		//		if (nameCount == 0)
//		//			return null;

//		//		// 루트 오브젝트를 찾지 못함.
//		//		var obj = GameObject.FindTransform(names[0]);
//		//		if (obj == null)
//		//			return null;

//		//		transform = obj.transform;
//		//		for (var i = 1; i < nameCount; ++i)
//		//		{
//		//			var name = names[i];
//		//			transform = transform.FindTransform(name);

//		//			// 찾기 실패.
//		//			if (transform == null)
//		//				return null;
//		//		}
//		//	}
//		//	// 상대경로.
//		//	// 상대경로 경로 문자열은 규칙이 복잡하다.
//		//	// 1. "./" 로 시작: 현재 기준점으로 부터 시작.
//		//	// 2. "../"로 시작: 현재 기준점의 부모로부터 시작.
//		//	// 3. "트랜스폼이름" 으로 시작: "./"이 생략된 결과.
//		//	// 4. "//" 
//		//	else
//		//	{
//		//		// 절대경로는 상대경로가 아님.
//		//		if (absolutePath.StartsWith("/"))
//		//			return null;
//		//	}


//		//	return transform;
//		//}

//		/// <summary>
//		/// 모든 루트 트랜스폼 반환.
//		/// </summary>
//		public static List<Transform> GetAllRootTransforms()
//		{
//			var rootTransforms = new List<Transform>();
//			var loadedScenes = new List<Scene>();
//			var sceneCount = SceneManager.sceneCount;
//			for (var i = 0; i < sceneCount; ++i)
//			{
//				var scene = SceneManager.GetSceneAt(i);
//				if (!scene.isLoaded)
//					continue;
//				loadedScenes.Add(scene);
//			}

//			var rootGameObjects = new List<GameObject>();
//			var loadedSceneCount = loadedScenes.Count;
//			for (var i = 0; i < loadedSceneCount; ++i)
//			{
//				var loadedScene = loadedScenes[i];
//				loadedScene.GetRootGameObjects(rootGameObjects);
//			}

//			var rootGameObjectCount = rootGameObjects.Count;
//			for (var i = 0; i < rootGameObjectCount; ++i)
//			{
//				var rootGameObject = rootGameObjects[i];
//				if (rootGameObject.transform.parent != null)
//					continue;
//				rootTransforms.Add(rootGameObject.transform);
//			}

//			return rootTransforms;
//		}

//		/// <summary>
//		/// 절대경로로 트랜스폼 찾기.
//		/// </summary>
//		public static Transform FindAbsoluteTransform(string absolutePath)
//		{
//			Debug.Log($"absolutePath: {absolutePath}");

//			// 경로 문자열이 존재하지 않음.
//			if (string.IsNullOrWhiteSpace(absolutePath))
//				return null;

//			// 맨 앞에 슬래시를 붙지 않아서 절대 경로로 볼 수 없음.
//			if (!absolutePath.StartsWith("/"))
//				return null;

//			var names = absolutePath.Split("/", System.StringSplitOptions.RemoveEmptyEntries);
//			var nameCount = names.Length;

//			// 경로 문법이 맞지 않음. (/{이름}/{이름}/ ...)
//			if (nameCount == 0)
//				return null;

//			// 루트 오브젝트를 찾지 못함.
//			var obj = GameObject.Find(names[0]);
//			if (obj == null)
//				return null;

//			var transform = obj.transform;
//			for (var i = 1; i < nameCount; ++i)
//			{
//				var name = names[i];
//				var childCount = transform.childCount;
//				var target = default(Transform);
//				for (var j = 0; j < childCount; ++j)
//				{
//					var child = transform.GetChild(j);
//					if (child.name.CompareTo(name) == 0)
//					{
//						target = child;
//						break;
//					}
//				}

//				// 찾기 실패.
//				if (target == null)
//					break;
				
//				// 찾기 성공.
//				transform = target;
//			}

//			return transform;
//		}

//		/// <summary>
//		/// 대상 트랜스폼의 절대 경로를 반환.
//		/// <para>/{이름/{이름}/{이름} 과 같은 형태이다.</para>
//		/// </summary>
//		public static string CreateAbsolutePath(Transform transform)
//		{
//			if (transform == null)
//				return string.Empty; 

//			var stringBuilder = new StringBuilder();
//			do
//			{			
//				stringBuilder.Insert(0, $"/{transform.name}");
//				transform = transform.parent;
//			}
//			while (transform != null);

//			var path = stringBuilder.ToString();
//			return path;
//		}

//		/// <summary>
//		/// 경로의 절대 경로를 변환.
//		/// </summary>
//		public static explicit operator string(TransformRoute path)
//		{
//			return path.AbsolutePath;
//		}
//	}
//}