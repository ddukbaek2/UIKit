using UnityEngine;

namespace UIKit
{
	/// <summary>
	/// 컴포넌트 관련 도구 모음.
	/// </summary>
	public static class ComponentExtensions
	{
		/// <summary>
		/// 컴포넌트를 반환. (없으면 추가)
		/// </summary>
		public static TComponent GetOrAddComponent<TComponent>(this GameObject obj) where TComponent : Component
		{
			var component = obj.GetComponent<TComponent>();
			if (component == null)
				component = obj.AddComponent<TComponent>();
			return component;
		}
	
		/// <summary>
		/// 컴포넌트를 반환. (없으면 추가)
		/// </summary>
		public static TComponent GetOrAddComponent<TComponent>(this MonoBehaviour target) where TComponent : Component
		{
			return target.gameObject.GetOrAddComponent<TComponent>();
		}
	}
}