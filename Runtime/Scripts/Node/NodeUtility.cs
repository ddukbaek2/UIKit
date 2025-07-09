using System.Collections.Generic;


namespace UIKit
{
	/// <summary>
	/// 노드 유틸리티.
	/// <para>INode 인터페이스 관련 기능 함수 모음.</para>
	/// </summary>
	public static class NodeUtility
	{
		/// <summary>
		/// 루트 노드 반환.
		/// </summary>
		public static INode GetRoot(INode target)
		{
			if (target == null)
				return null;
			var node = target;
			while (node.Parent != null)
				node = node.Parent;
			return node;
		}

		/// <summary>
		/// 형제 노드 목록 반환.
		/// </summary>
		public static List<INode> ToSiblings(INode target)
		{
			var siblings = new List<INode>();
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
		public static List<INode> ToLeaves(INode target)
		{
			void Recursive(INode node, List<INode> leaves)
			{
				using var enumerator = node.Children.GetEnumerator();
				while (enumerator.MoveNext())
					Recursive(enumerator.Current, leaves);
				if (!node.IsLeaf)
					return;
				leaves.Add(node);
			}

			var leaves = new List<INode>();
			if (target == null)
				return leaves;

			Recursive(target, leaves);
			return leaves;
		}
		
		/// <summary>
		/// 모든 자식 노드 제거.
		/// </summary>
		public static void RemoveAllChildren(INode target)
		{
			if (target == null)
				return;
			
			var children = EnumerableUtility<INode>.ToList(target.Children);
			var childCount = children.Count;
			for (var i = 0; i < childCount; ++i)
			{
				target.RemoveChild(children[i]);
			}		
		}
	}
}