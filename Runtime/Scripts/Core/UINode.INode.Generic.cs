using System.Collections.Generic;


namespace UIKit
{	/// <summary>
	/// UI를 위한 컴포넌트.
	/// <para>INode 제네릭 인터페이스 구현체.</para>
	/// </summary>
	public partial class UINode
	{
		/// <summary>
		/// 루트 노드 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public UINode Root => NodeUtility<UINode>.GetRoot(this);

		/// <summary>
		/// 리프 노드 목록 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public IEnumerable<UINode> Leaves => NodeUtility<UINode>.ToLeaves(this);

		/// <summary>
		/// 부모 노드 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public UINode Parent { set => m_Parent = value; get => m_Parent; }
		
		/// <summary>
		/// 형제 노드 목록 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public IEnumerable<UINode> Siblings => NodeUtility<UINode>.ToSiblings(this);
		
		/// <summary>
		/// 자식 노드 목록 프로퍼티.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public IEnumerable<UINode> Children => m_Children;

		/// <summary>
		/// 자식 노드 추가.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public void AddChild(UINode node)
		{
			if (node == null)
				return;
			if (m_Children.Contains(node))
				return;
			
			node.Parent = this;
			m_Children.Add(node);
		}

		/// <summary>
		/// 자식 노드 제거.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public void RemoveChild(UINode node)
		{
			if (node == null)
				return;
			if (!m_Children.Contains(node))
				return;
			
			node.Parent = null;
			m_Children.Remove(node);
		}

		/// <summary>
		/// 대상 노드가 자식 노드에 포함되는지 여부.
		/// <para>INode 제네릭 인터페이스 구현.</para>
		/// </summary>
		public bool IsChild(UINode node)
		{
			if (node == null)
				return false;
			if (!m_Children.Contains(node))
				return false;
			return true;
		}
	}
}