using System.Collections.Generic;


namespace UIKit
{
	/// <summary>
	/// 노드 인터페이스.
	/// </summary>
	public interface INode
	{
		/// <summary>
		/// 루트 노드 여부 프로퍼티.
		/// </summary>
		bool IsRoot { get; }
	
		/// <summary>
		/// 리프 노드 여부 프로퍼티.
		/// </summary>
		bool IsLeaf { get; }

		/// <summary>
		/// 루트 노드 프로퍼티.
		/// </summary>
		INode Root { get; }
		
		/// <summary>
		/// 리프 노드 목록 프로퍼티.
		/// </summary>
		IEnumerable<INode> Leaves { get; }
		
		/// <summary>
		/// 부모 노드 프로퍼티.
		/// </summary>
		INode Parent { set; get; }
		
		/// <summary>
		/// 형제 노드 목록 프로퍼티.
		/// </summary>
		IEnumerable<INode> Siblings { get; }
		
		/// <summary>
		/// 자식 노드 목록 프로퍼티.
		/// </summary>
		IEnumerable<INode> Children { get; }

		/// <summary>
		/// 자식 노드 추가.
		/// </summary>
		void AddChild(INode node);

		/// <summary>
		/// 자식 노드 제거.
		/// </summary>
		void RemoveChild(INode node);

		/// <summary>
		/// 모든 자식 노드 제거.
		/// </summary>
		void RemoveAllChildren();

		/// <summary>
		/// 대상 노드가 자식 노드에 포함되는지 여부.
		/// </summary>
		bool IsChild(INode node);
	}
}