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
		/// 루트 노드 여부 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public bool IsRoot => NodeUtility.IsRoot(this);

		/// <summary>
		/// 리프 노드 여부 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public bool IsLeaf => NodeUtility.IsLeaf(this);
		
		/// <summary>
		/// 루트 노드 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		INode INode.Root => NodeUtility.GetRoot(this);

		/// <summary>
		/// 리프 노드 목록 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		IEnumerable<INode> INode.Leaves => NodeUtility.GetLeaves(this);

		/// <summary>
		/// 부모 노드 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		INode INode.Parent { set => m_Parent = (Node)value; get => m_Parent; }
		
		/// <summary>
		/// 형제 노드 목록 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		IEnumerable<INode> INode.Siblings => NodeUtility.GetSiblings(this);
		
		/// <summary>
		/// 자식 노드 목록 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		IEnumerable<INode> INode.Children => m_Children;
		
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
			m_Parent = null;
			m_Children.Clear();
		}

		/// <summary>
		/// 자식 노드 추가.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		void INode.AddChild(INode node)
		{
			NodeUtility.AddChild(this, node);
		}

		/// <summary>
		/// 자식 노드 제거.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		void INode.RemoveChild(INode node)
		{
			NodeUtility.RemoveChild(this, node);
		}

		/// <summary>
		/// 모든 자식 노드 제거.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		void INode.RemoveAllChildren()
		{
			NodeUtility.RemoveAllChildren(this);
		}

		/// <summary>
		/// 대상 노드가 자식 노드에 포함되는지 여부.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		bool INode.IsChild(INode node)
		{
			return NodeUtility.IsChild(this, node);
		}
	}
}