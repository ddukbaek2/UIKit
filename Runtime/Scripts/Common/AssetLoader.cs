using System;
using UnityEngine;


namespace UIKit
{
	/// <summary>
	/// 애셋 로더.
	/// </summary>
	public static class AssetLoader
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		static AssetLoader()
		{
		}

		/// <summary>
		/// 대상 클래스 타입의 애셋 경로 어트리뷰트에서 찾아낸 실제 애셋 경로를 반환.
		/// </summary>
		public static string GetAssetPath(Type instanceType)
		{
			if (!Reflector.TryGetAttribute<AssetPathAttribute>(instanceType, out var attribute))
			{
				throw new Exception("Create Failed. Not Found AssetPath Attribute.");
			}

			return attribute.AssetPath;
		}

		/// <summary>
		/// 대상 클래스의 애셋 경로 어트리뷰트에서 찾아낸 실제 애셋 경로를 반환.
		/// </summary>
		public static string GetAssetPath<TClass>() where TClass : class
		{
			try
			{
				var instanceType = typeof(TClass);
				return GetAssetPath(instanceType);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 애셋 로드.
		/// </summary>
		public static UnityEngine.Object LoadAsset(string assetPath)
		{
			try
			{
				var asset = Resources.Load(assetPath, assetType);
				return asset;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 애셋 로드.
		/// </summary>
		public static TAsset LoadAsset<TAsset>(string assetPath) where TAsset : UnityEngine.Object
		{
			try
			{
				var asset = Resources.Load<TAsset>(assetPath, assetType);
				return asset;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 대상 클래스 타입의 애셋 경로 어트리뷰트에서 찾아낸 실제 애셋을 로드.
		/// </summary>
		public static UnityEngine.Object LoadAssetFromAssetPath(Type instanceType, Type assetType)
		{
			try
			{
				var assetPath = AssetLoader.GetAssetPath(instanceType);
				var asset = AssetLoader.LoadAsset(assetPath, assetType);
				return asset;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 대상 클래스의 애셋 경로 어트리뷰트에서 찾아낸 실제 애셋을 로드.
		/// </summary>
		public static TObject LoadAssetFromAssetPath<TClass, TObject>() where TClass : class where TObject : UnityEngine.Object
		{
			try
			{
				var assetPath = AssetLoader.GetAssetPath<TClass>();
				var asset = AssetLoader.LoadAsset<TObject>(assetPath);
				return asset;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 애셋 경로를 통해 게임오브젝트 로드하여 생성.
		/// </summary>
		public static GameObject Instantiate(string assetPath)
		{
			try
			{
				var asset = AssetLoader.LoadAsset<GameObject>(asetPath);
				var obj = GameObject.Instantiate<GameObject>(asset);
				return obj;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 대상 클래스 타입의 애셋 경로 어트리뷰트에 설정된 게임 오브젝트 로드하여 생성.
		/// </summary>
		public static GameObject InstantiateFromAssetPath(Type instanceType)
		{
			try
			{
				var asset = AssetLoader.Instantiate(instanceType, typeof(GameObject));
				var obj = GameObject.Instantiate(asset);
				return (GameObject)obj;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 대상 클래스의 애셋 경로 어트리뷰트에 설정된 게임 오브젝트 로드하여 생성.
		/// </summary>
		public static GameObject InstantiateWithComponentFromAssetPath<TClass>() where TClass : class
		{
			try
			{
				var asset = AssetLoader.LoadAssetFromAssetPath<TClass, GameObject>();
				var obj = GameObject.Instantiate<GameObject>(asset);
				return obj;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 대상 컴포넌트 타입의 애셋 경로 어트리뷰트에 설정된 게임 오브젝트 로드하여 생성.
		/// </summary>
		public static Component InstantiateWithComponent(Type instanceType, Type parentType = null)
		{
			var component = default(Component);
			try
			{
				var obj = AssetLoader.InstantiateFromAssetPath(instanceType);
				obj.name = instanceType.Name;
				component = obj.GetComponent(instanceType);
				if (component == null)
					component = obj.AddComponent(instanceType);
			}
			catch (Exception exception)
			{
				throw exception;
			}

			if (parentType != null && !Reflector.IsBaseClass(component, parentType))
			{
				throw new Exception($"Create Failed. This Instance Not Inherit {parentType.Name} Class.");
			}

			return component;
		}

		/// <summary>
		/// 대상 컴포넌트의 애셋 경로 어트리뷰트에 설정된 게임 오브젝트 로드하여 생성.
		/// </summary>
		public static TComponent InstantiateWithComponentFromAssetPath<TComponent>(Type parentType = null) where TComponent : MonoBehaviour
		{
			var instanceType = typeof(TComponent);
			var component = AssetLoader.InstantiateWithComponent(instanceType, parentType);
			return component as TComponent;
		}
	}
}