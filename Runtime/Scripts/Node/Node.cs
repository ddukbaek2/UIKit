using System.Collections.Generic;


namespace UIKit
{
	/// <summary>
	/// 노드.
	/// <para>INode 인터페이스 구현체.</para>
	/// </summary>
	public partial class Node : Disposable, INode<Node>
	{
		/// <summary>
		/// 부모 노드.
		/// </summary>
		private Node m_Parent;

		/// <summary>
		/// 자식 노드 목록.
		/// </summary>
		private List<Node> m_Children;
	
		/// <summary>
		/// 생성됨.
		/// </summary>
		public Node() : base()
		{
			m_Parent = null;
			m_Children = new List<Node>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			// m_Parent = null;
			// m_Children.Clear();
		}

		/// <summary>
		/// 부모 노드 설정.
		/// </summary>
		public void SetParent(Node parent)
		{
			m_Parent = parent;
		}
	}
}