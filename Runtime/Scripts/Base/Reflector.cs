using System;
using System.Reflection;


namespace UIKit
{
	/// <summary>
	/// 리플렉션 처리기.
	/// </summary>
	public static class Reflector
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		static Reflector()
		{
		}

		/// <summary>
		/// 대상 인스턴스가 대상 타입을 상속받거나 대상 타입인지 여부 반환.
		/// </summary>
		public static bool IsBaseClass(object instance, Type parentType)
		{
			if (instance == null || parentType == null)
				return false;

			var instanceType = instance.GetType();
			return IsBaseClass(instanceType, parentType);
		}

		/// <summary>
		/// 대상 인스턴스가 대상 타입을 상속받거나 대상 타입인지 여부 반환.
		/// </summary>
		public static bool IsBaseClass(Type instanceType, Type parentType)
		{
			if (instanceType == null || parentType == null)
				return false;

			return parentType.IsAssignableFrom(instanceType);
		}

		/// <summary>
		/// 인스턴스 생성.
		/// </summary>
		public static object CreateInstance(Type instanceType, params object[] arguments)
		{
			if (instanceType == null || !instanceType.IsClass)
				return null;

			try
			{
				var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
				var returnValue = Activator.CreateInstance(instanceType, bindingFlags, null, arguments, null);
				return returnValue;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 인스턴스 메소드 대리 호출.
		/// </summary>
		public static object InvokeInstanceMethod(object instance, string methodName, params object[] parameters)
		{
			if (instance == null)
				return null;

			try
			{
				var instanceType = instance.GetType();
				var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance;
				var methodInfo = instanceType.GetMethod(methodName, bindingFlags);
				if (methodInfo == null)
					return null;

				var returnValue = methodInfo.Invoke(instance, parameters);
				return returnValue;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 스태틱 메소드 대리 호출.
		/// </summary>
		public static object InvokeStaticMethod(Type instanceType, string methodName, params object[] parameters)
		{
			if (instanceType == null)
				return null;

			try
			{
				var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Static;
				var method = instanceType.GetMethod(methodName, bindingFlags);
				if (method == null)
					return null;

				var returnValue = method.Invoke(instanceType, parameters);
				return returnValue;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}


		/// <summary>
		/// 인스턴스 생성.
		/// </summary>
		public static T CreateInstance<T>(params object[] arguments) where T : class
		{
			try
			{
				var instanceType = typeof(T);
				var returnValue = CreateInstance(instanceType, arguments);
				if (returnValue == null)
					return default;
				return (T)returnValue;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 인스턴스 메소드 대리 호출.
		/// </summary>
		public static TReturnValue InvokeInstanceMethod<TReturnValue>(object instance, string methodName, params object[] parameters)
		{
			try
			{
				var returnValue = InvokeInstanceMethod(instance, methodName, parameters);
				if (returnValue == null)
					return default;
				return (TReturnValue)returnValue;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 스태틱 메소드 대리 호출.
		/// </summary>
		public static TReturnValue InvokeStaticMethod<TReturnValue>(Type instanceType, string methodName, params object[] parameters)
		{
			try
			{
				var returnValue = InvokeStaticMethod(instanceType, methodName, parameters);
				if (returnValue == null)
					return default;
				return (TReturnValue)returnValue;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 어트리뷰트 가져오기.
		/// </summary>
		public static bool TryGetAttribute(Type instanceType, Type attributeType, out Attribute attribute)
		{
			attribute = null;
			if (instanceType == null || attributeType == null)
				return false;

			try
			{
				attribute = instanceType.GetCustomAttribute(attributeType);
				if (attribute == null)
					return false;
				return true;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 어트리뷰트 가져오기.
		/// </summary>
		public static bool TryGetAttribute<TAttribute>(Type instanceType, out TAttribute attribute) where TAttribute : Attribute
		{
			attribute = default;
			try
			{
				if (!TryGetAttribute(instanceType, typeof(TAttribute), out var attrib))
					return false;
				attribute = (TAttribute)attrib;
				return true;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
	}
}