using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UIKit
{
	/// <summary>
	/// UIKitFrameworks에서 사용하는 기본 컴포넌트.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class UIBase : UIBehaviour
	{
		#region INSPECTOR
		[SerializeField] private RectTransform m_RectTransform;
		#endregion

		/// <summary>
		/// UI 트랜스폼 프로퍼티.
		/// </summary>
		public RectTransform RectTransform
		{
			get
			{
				if (m_RectTransform == null)
				{
					m_RectTransform = GetComponent<RectTransform>();
					if (m_RectTransform == null)
					{
						throw new UnityException("m_RectTransform is null.");
					}
					else
					{
						return m_RectTransform;
					}
				}
				else
				{
					return m_RectTransform;
				}
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			var type = GetType();
			Debug.Log($"UIBase.Awake(): Class: \"{type.Name}\"");

			m_RectTransform = GetComponent<RectTransform>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		/// <summary>
		/// 부모 설정.
		/// </summary>
		public void SetParentRectTransform(Transform parentTransform)
		{
			var offsetMin = RectTransform.offsetMin;
			var offsetMax = RectTransform.offsetMax;
			var pivot = RectTransform.pivot;
			var anchorMin = RectTransform.anchorMin;
			var anchorMax = RectTransform.anchorMax;
			var anchoredPosition = RectTransform.anchoredPosition;
			var anchoredPosition3D = RectTransform.anchoredPosition3D;
			var translation = RectTransform.position;
			var rotation = RectTransform.rotation;
			var localScale = RectTransform.localScale;

			RectTransform.SetParent(parentTransform, false);
			//RectTransform.offsetMin = new Vector2(0f, 0f);
			//RectTransform.offsetMax = new Vector2(1f, 1f);
			RectTransform.offsetMin = offsetMin;
			RectTransform.offsetMax = offsetMax;
		}

		///// <summary>
		///// 컴포넌트 반환.
		///// </summary>
		//public TComponent FindComponent<TComponent>(string path) where TComponent : Component
		//{
		//	var transformRoute = new TransformRoute(RectTransform);
		//	var transform = transformRoute.FindRelativeTransform(path);
		//	return transform.GetComponent<TComponent>();
		//}

		///// <summary>
		///// 버튼 눌림 이벤트를 외부로 연결.
		///// </summary>
		//public void AddButtonClickEvent(string path, UnityAction onClick)
		//{
		//	var button = FindComponent<Button>(path);
		//	button.onClick.AddListener(onClick);
		//}
	}
}