using UnityEngine;
using UnityEngine.EventSystems;


namespace UIKit
{
	/// <summary>
	/// 물리적 UI 동적 항목 처리 단위.
	/// </summary>
	//[RequireComponent(typeof(RectTransform))]
	public class UIItem : UIView
	{
		#region INSPECTOR
		#endregion

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		/// <summary>
		/// 풀에 집어넣음.
		/// </summary>
		protected virtual void OnEnqueued()
		{
		}

		/// <summary>
		/// 풀에서 꺼내옴.
		/// </summary>
		protected virtual void OnDequeued()
		{
		}
	}
}