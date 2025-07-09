using System.Collections.Generic;


namespace UIKit
{
	/// <summary>
	/// 제너릭 노드 유틸리티.
	/// <para>INode 제네릭 인터페이스 관련 기능 함수 모음.</para>
	/// </summary>
	public static class NodeUtility<TNode> where TNode : INode<TNode>
	{
		/// <summary>
		/// 루트 노드 반환.
		/// </summary>
		public static TNode GetRoot(TNode target)
		{
			if (target == null)
				return default;
			
			var node = target;
			while (node.Parent != null)
				node = node.Parent;
			return node;
		}

		/// <summary>
		/// 형제 노드 목록 반환.
		/// </summary>
		public static List<TNode> ToSiblings(TNode target)
		{
			var siblings = new List<TNode>();
			if (target == null)
				return siblings;
			if (target.Parent == null)
				return siblings;

			using var enumerator = target.Parent.Children.GetEnumerator();
			while (enumerator.MoveNext())
				siblings.Add(enumerator.Current);

			return siblings;
		}

		/// <summary>
		/// 리프 노드 목록 반환.
		/// </summary>
		public static List<TNode> ToLeaves(TNode target)
		{
			void Recursive(TNode node, List<TNode> leaves)
			{
				using var enumerator = target.Children.GetEnumerator();
				while (enumerator.MoveNext())
					Recursive(enumerator.Current, leaves);
				if (!node.IsLeaf)
					return;
				leaves.Add(node);
			}

			var leaves = new List<TNode>();
			if (target == null)
				return leaves;

			Recursive(target, leaves);
			return leaves;
		}
	}
}