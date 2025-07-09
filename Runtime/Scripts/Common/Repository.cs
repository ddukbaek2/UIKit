//#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL || UNITY_WSA || UNITY_XBOXONE || UNITY_PS4
//#define UNITY_ENGINE
//#endif

using System;
using System.Collections.Generic;
using UnityEngine;
//#if UNITY_ENGINE
//using UnityEngine;
//#endif


namespace UIKit
{
	/// <summary>
	/// 인스턴스 캐시 보관.
	/// </summary>
	public static class Repository
	{
		/// <summary>
		/// 인스턴스 목록.
		/// </summary>
		private static Dictionary<Type, object> s_Cache;

		/// <summary>
		/// 생성됨.
		/// </summary>
		static Repository()
		{
			s_Cache = new Dictionary<Type, object>();
		}

		/// <summary>
		/// 수동 인스턴스 등록.
		/// </summary>
		public static void Register(object instance, Type instanceType)
		{
			if (s_Cache.ContainsKey(instanceType))
			{
				s_Cache[instanceType] = instance;
			}
			else
			{
				s_Cache.Add(instanceType, instance);
			}
		}

		/// <summary>
		/// 수동 인스턴스 등록.
		/// </summary>
		public static void Register<T>(T instance) where T : class
		{
			var instanceType = typeof(T);
			Register(instance, instanceType);
		}

		/// <summary>
		/// 수동 인스턴스 등록 해제.
		/// </summary>
		public static void Unregister(object instance, Type instanceType)
		{
			if (s_Cache.ContainsKey(instanceType))
			{
				s_Cache[instanceType] = instance;
			}
			else
			{
				s_Cache.Add(instanceType, instance);
			}
		}

		/// <summary>
		/// 수동 인스턴스 등록 해제.
		/// </summary>
		public static void Unregister<T>(T instance) where T : class
		{
			var instanceType = typeof(T);
			Unregister(instance, instanceType);
		}

		/// <summary>
		/// 인스턴스 등록되었는지 여부.
		/// </summary>
		public static bool IsRegistered(Type instanceType)
		{
			return s_Cache.ContainsKey(instanceType);
		}

		/// <summary>
		/// 인스턴스 등록되었는지 여부.
		/// </summary>
		public static bool IsRegistered<T>() where T : class
		{
			var instanceType = typeof(T);
			return IsRegistered(instanceType);
		}

		/// <summary>
		/// 등록된 인스턴스 반환. 없다면 신규 생성.
		/// </summary>
		public static object Get(Type instanceType, bool autoCreate = true)
		{
			if (s_Cache.TryGetValue(instanceType, out var instance))
				return instance;

			if (!autoCreate)
				return null;

			// 컴포넌트 계통인지 확인.
			if (Reflector.IsBaseClass(instanceType, typeof(Component)))
			{
				// 컴포넌트는 이미 존재하는 것에 대한 확인 가능.
				var component = GameObject.FindAnyObjectByType(instanceType);
				if (component == null)
				{
					try
					{
						component = AssetLoader.InstantiateWithComponentFromAssetPath(instanceType);
						instance = component;
					}
					catch// (Exception exception)
					{
						//throw exception;
						var obj = new GameObject(instanceType.Name);
						component = obj.AddComponent(instanceType);
						instance = component;
					}
				}
				else
				{
					instance = component;
				}
			}
			else
			{
				instance = Reflector.CreateInstance(instanceType);
			}

			Register(instance, instanceType);
			return instance;
		}

		/// <summary>
		/// 등록된 인스턴스 반환. 없다면 신규 생성.
		/// </summary>
		public static T Get<T>(bool autoCreate = true) where T : class
		{
			var instanceType = typeof(T);
			var instance = Get(instanceType, autoCreate);
			if (instance == null)
				return default;

			return (T)instance;
		}
	}
}