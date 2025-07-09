using System.Collections.Generic;


namespace UIKit
{
	/// <summary>
	/// 제너릭 노드 인터페이스.
	/// </summary>
	public interface INode<TNode> : INode where TNode : INode<TNode>
	{
		/// <summary>
		/// 루트 노드 프로퍼티.
		/// </summary>
		new TNode Root { get; }

		/// <summary>
		/// 리프 노드 목록 프로퍼티.
		/// </summary>
		new IEnumerable<TNode> Leaves { get; }

		/// <summary>
		/// 부모 노드 프로퍼티.
		/// </summary>
		new TNode Parent { set; get; }

		/// <summary>
		/// 형제 노드 목록 프로퍼티.
		/// </summary>
		new IEnumerable<TNode> Siblings { get; }

		/// <summary>
		/// 자식 노드 목록 프로퍼티.
		/// </summary>
		new IEnumerable<TNode> Children { get; }
		
		/// <summary>
		/// 자식 노드 추가.
		/// </summary>
		void AddChild(TNode node);

		/// <summary>
		/// 자식 노드 제거.
		/// </summary>
		void RemoveChild(TNode node);
		
		/// <summary>
		/// 대상 노드가 자식 노드에 포함되는지 여부.
		/// </summary>
		bool IsChild(TNode node);
	}
}