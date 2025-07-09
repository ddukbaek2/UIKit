using System.Collections.Generic;


namespace UIKit
{
	/// <summary>
	/// 노드.
	/// <para>INode 인터페이스 구현체.</para>
	/// </summary>
	public partial class Node
	{
		/// <summary>
		/// 루트 노드 여부 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public bool IsRoot => m_Parent == null;

		/// <summary>
		/// 리프 노드 여부 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		public bool IsLeaf => m_Children.Count == 0;
		
		/// <summary>
		/// 루트 노드 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		INode INode.Root => NodeUtility.GetRoot(this);

		/// <summary>
		/// 리프 노드 목록 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		IEnumerable<INode> INode.Leaves => NodeUtility.ToLeaves(this);

		/// <summary>
		/// 부모 노드 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		INode INode.Parent { set => SetParent((Node)value); get => m_Parent; }
		
		/// <summary>
		/// 형제 노드 목록 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		IEnumerable<INode> INode.Siblings => NodeUtility.ToSiblings(this);
		
		/// <summary>
		/// 자식 노드 목록 프로퍼티.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		IEnumerable<INode> INode.Children => m_Children;

		/// <summary>
		/// 자식 노드 추가.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		void INode.AddChild(INode node)
		{
			if (node == null)
				return;
			if (!(node is Node))
				return;
			if (m_Children.Contains((Node)node))
				return;

			node.Parent = this;
			m_Children.Add((Node)node);
		}

		/// <summary>
		/// 자식 노드 제거.
		/// <para>INode 인터페이스 구현.</para>
		/// </summary>
		void INode.RemoveChild(INode node)
		{
			if (node == null)
				return;
			if (!(node is Node))
				return;
			if (!m_Children.Contains((Node)node))
				return;

			node.Parent = null;
			m_Children.Remove((Node)node);
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
			if (node == null)
				return false;
			if (!m_Children.Contains((Node)node))
				return false;
			return true;
		}
	}
}