//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;


//namespace UIKit
//{
//	/// <summary>
//	/// 자체적인 컨텐트 사이즈 피터.
//	/// </summary>
//	public class ContentSizeFitter : UIBehaviour
//	{
//		/// <summary>
//		/// 모드.
//		/// </summary>
//		public enum FitMode
//		{
//			Unconstrained,
//			MinSize,
//			PreferredSize
//		}


//		/// <summary>
//		/// 
//		/// </summary>
//		public class Size : Disposable
//		{
//			public Vector2 Location;
//			public Vector2 SizeDelta;

//			public Size() : base()
//			{
//			}

//			protected override void OnDispose(bool explicitDisposing)
//			{
//			}
//		}


//		#region INSPECTOR
//		[SerializeField] private RectTransform m_RectTransform;
//		[SerializeField] private FitMode m_HorizontalFit = FitMode.PreferredSize;
//		[SerializeField] private FitMode m_VerticalFit = FitMode.PreferredSize;
//		#endregion

//		/// <summary>
//		/// UI 트랜스폼 프로퍼티.
//		/// </summary>
//		public RectTransform RectTransform => m_RectTransform;

//		/// <summary>
//		/// 생성됨.
//		/// </summary>
//		protected override void Awake()
//		{
//			base.Awake();

//			m_RectTransform = GetComponent<RectTransform>();
//		}

//		/// <summary>
//		/// 파괴됨.
//		/// </summary>
//		protected override void OnDestroy()
//		{
//			base.OnDestroy();
//		}

//		/// <summary>
//		/// 업데이트.
//		/// </summary>
//		protected virtual void LateUpdate()
//		{
			
//		}

//		/// <summary>
//		/// 트리 생성.
//		/// </summary>
//		public void BuildTree()
//		{
//			Transform GetRootTransform()
//			{
//				var root = transform;
//				while (root != null)
//					root = transform.parent;
//				return root; // transform.root;
//			}

//			var root = GetRootTransform();
//			var contentSizeFitters = root.GetComponentsInChildren<ContentSizeFitter>();
//			var map = new Dictionary<Transform, ContentSizeFitter>();
//			for (var i = 0; i < contentSizeFitters.Length; ++i)
//			{
//				var contentSizeFitter = contentSizeFitters[i];
//				map.Add(contentSizeFitter.transform, contentSizeFitter);
//			}

//			void Recursive(Transform current)
//			{
//				for (var i = 0; i < current.childCount; ++i)
//				{
//					var child = current.GetChild(i);
//					Recursive(child);
//				}

//				if (!map.TryGetValue(current, out var contentSizeFitter))
//					return;

//				contentSizeFitter.SizeFit();
//			}
//		}

//		/// <summary>
//		/// 사이즈 변경.
//		/// </summary>
//		public void SizeFit()
//		{
//			//if (m_HorizontalFit != FitMode.Unconstrained)
//			//{
//			//	RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
//			//}
//		}
//	}
//}