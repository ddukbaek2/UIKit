using System.Collections;
using UnityEngine;


namespace UIKit
{
	/// <summary>
	/// 물리적 UI 단위 처리.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class UIPanel : UIView
	{
		#region INSPECTOR
		[SerializeField] private CanvasGroup m_CanvasGroup;
		#endregion

		/// <summary>
		/// 캔버스 그룹 프로퍼티.
		/// </summary>
		public CanvasGroup CanvasGroup => m_CanvasGroup;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			m_CanvasGroup = GetComponent<CanvasGroup>();
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected override void Start()
		{
			base.Start();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		/// <summary>
		/// 보이기/감추기 설정.
		/// </summary>
		public virtual void SetVisible(bool visible, bool animated)
		{
			if (animated)
			{
				State.Window.StartCoroutine(VisibleAnimationProcess(visible));
			}
			else
			{
				gameObject.SetActive(visible);
			}
		}

		/// <summary>
		/// 보이기/감추기에 대한 부드러운 처리.
		/// </summary>
		protected virtual IEnumerator VisibleAnimationProcess(bool visible)
		{
			if (m_CanvasGroup == null)
			{
				SetVisible(visible, false);
				yield break;
			}

			if (visible)
			{
				SetVisible(true, false);
			}

			var time = 0f;
			var duration = 0.5f;
			m_CanvasGroup.alpha = visible ? 0f : 1f;
			do
			{
				var normalizedTime = Mathf.Clamp01(time / duration);
				m_CanvasGroup.alpha = visible ? normalizedTime : 1f - normalizedTime;
				time += Time.deltaTime;
			}
			while (time <= duration);
			m_CanvasGroup.alpha = visible ? 1f : 0f;
			if (!visible)
			{
				SetVisible(false, false);
			}
		}

	}
}