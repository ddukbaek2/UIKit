using System;


namespace UIKit
{
	/// <summary>
	/// 애셋 경로 어트리뷰트.
	/// </summary>
	public class AssetPathAttribute : Attribute
	{
		/// <summary>
		/// 애셋 경로.
		/// </summary>
		private string m_AssetPath;

		/// <summary>
		/// 애셋 경로 프로퍼티.
		/// </summary>
		public string AssetPath => m_AssetPath;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssetPathAttribute(string assetPath) : base()
		{
			m_AssetPath = assetPath;
			//m_AssetPath = m_AssetPath.Replace("Assets/Resources/", string.Empty);
			//m_AssetPath = m_AssetPath.Replace(".prefab", string.Empty);
		}
	}
}