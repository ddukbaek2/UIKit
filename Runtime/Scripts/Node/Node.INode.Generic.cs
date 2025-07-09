using System.Collections.Generic;


namespace UIKit
{
	/// <summary>
	/// 노드.
	/// <para>INode 제네릭 인터페이스 구현체.</para>
	/// </summary>
	public partial class Node : Disposable, INode<Node>
	{
		/// <summary>
		/// 루트 노드 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public Node Root => NodeUtility<Node>.GetRoot(this);

		/// <summary>
		/// 리프 노드 목록 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public List<Node> Leaves => NodeUtility<Node>.GetLeaves(this);

		/// <summary>
		/// 부모 노드 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public Node Parent { set => m_Parent = value; get => (Node)m_Parent; }
		
		/// <summary>
		/// 형제 노드 목록 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public List<Node> Siblings => NodeUtility<Node>.GetSiblings(this);
		
		/// <summary>
		/// 자식 노드 목록 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public List<Node> Children => m_Children;

		/// <summary>
		/// 자식 노드 추가.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public void AddChild(Node node)
		{
			NodeUtility<Node>.AddChild(this, node);
		}

		/// <summary>
		/// 자식 노드 제거.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public void RemoveChild(Node node)
		{
			NodeUtility<Node>.RemoveChild(this, node);
		}

		/// <summary>
		/// 대상 노드가 자식 노드에 포함되는지 여부.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public bool IsChild(Node node)
		{
			return NodeUtility.IsChild(this, node);
		}
	}
}