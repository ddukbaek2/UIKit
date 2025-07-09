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
		/// 루트 노드 여부 반환.
		/// </summary>
		public static bool IsRoot(INode target)
		{
			if (target == null)
				return false;

			return target.Parent == null;
		}

		/// <summary>
        /// 리프 노드 여부 반환.
        /// </summary>
        public static bool IsLeaf(INode target)
        {
	        if (target == null)
		        return false;

        	return NodeUtility.GetChildCount(target) == 0;
        }

		/// <summary>
		/// 지정 노드의 자식 노드인지 여부.
		/// </summary>
		public static bool IsChild(INode target, INode node)
		{
			if (target == null)
				return false;
			if (node == null)
				return false;
			
			return target.Children.Contains(node);
		}
		
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
		public static List<INode> GetSiblings(INode target)
		{
			var siblings = new List<INode>();
			if (target == null)
				return siblings;
			if (target.Parent == null)
				return siblings;

			var childCount = target.Parent.Children.Count;
			for (var i = 0; i < childCount; ++i)
			{
				var child = target.Parent.Children[i];
				siblings.Add(child);
			}
			return siblings;
		}

		/// <summary>
		/// 리프 노드 목록 반환.
		/// </summary>
		public static List<INode> GetLeaves(INode target)
		{
			void Recursive(INode node, List<INode> leaves)
			{
				for (var i = 0; i < node.Children.Count; ++i)
				{
					var child = node.Children[i];
					Recursive(child, leaves);
				}
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
		/// 자식 노드 추가.
		/// </summary>
		public static void AddChild(INode target, INode child)
		{
			if (target == null || child == null)
				return;
			if (NodeUtility.IsChild(target, child))
				return;

			child.Parent = target;
			target.Children.Add(child);
		}

		/// <summary>
		/// 자식 노드 제거.
		/// </summary>
		public static void RemoveChild(INode target, INode child)
		{
			if (target == null || child == null)
				return;
			
			if (!NodeUtility.IsChild(target, child))
				return;

			child.Parent = null;
			target.Children.Remove(child);		
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
				NodeUtility.RemoveChild(target, children[i]);
			}		
		}

		/// <summary>
		/// 자식 갯수 반환.
		/// </summary>
		public static int GetChildCount(INode target)
		{
			if (target == null)
				return 0;

			return EnumerableUtility<INode>.GetCount(target.Children);
		}
	}
}