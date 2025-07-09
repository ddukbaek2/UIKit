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
		/// 루트 노드 여부 반환.
		/// </summary>
		public static bool IsRoot(TNode target)
		{
			if (target == null)
				return false;

			return target.Parent == null;
		}

		/// <summary>
        /// 리프 노드 여부 반환.
        /// </summary>
        public static bool IsLeaf(TNode target)
        {
	        if (target == null)
		        return false;
	        
        	return NodeUtility.IsLeaf((target);
        }

		/// <summary>
		/// 지정 노드의 자식 노드인지 여부.
		/// </summary>
		public static bool IsChild(TNode target, TNode node)
		{
			if (target == null)
				return false;
			if (node == null)
				return false;
			
			return target.Children.Contains(node);
		}

		/// <summary>
		/// 자식 노드 갯수 반환.
		/// </summary>
		public static int GetChildCount(TNode target)
		{
			if (target == null)
				return 0;

			return target.Children.Count;
		}
		
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
		public static List<TNode> GetSiblings(TNode target)
		{
			var siblings = new List<TNode>();
			if (target == null)
				return siblings;
			if (target.Parent == null)
				return siblings;

			var childCount = target.Parent.Children.Count;
			for (var i = 0; i < childCount; ++i)
			{
				var child = target.Parent.Children[i];
				siblings.Add((TNode)child);
			}
			return siblings;
		}

		/// <summary>
		/// 리프 노드 목록 반환.
		/// </summary>
		public static List<TNode> GetLeaves(TNode target)
		{
			void Recursive(TNode node, List<TNode> leaves)
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

			var leaves = new List<TNode>();
			if (target == null)
				return leaves;

			Recursive(target, leaves);
			return leaves;
		}

		/// <summary>
		/// 자식 노드 추가.
		/// </summary>
		public static void AddChild(TNode target, TNode child)
		{
			if (target == null || child == null)
				return;
			if (NodeUtility<TNode>.IsChild(target, child))
				return;

			child.Parent = target;
			target.Children.Add(child);
		}

		/// <summary>
		/// 자식 노드 제거.
		/// </summary>
		public static void RemoveChild(TNode target, TNode child)
		{
			if (target == null || child == null)
				return;
			if (!NodeUtility<TNode>.IsChild(target, child))
				return;

			child.Parent = default;
			target.Children.Remove(child);		
		}

		/// <summary>
		/// 모든 자식 노드 제거.
		/// </summary>
		public static void RemoveAllChildren(TNode target)
		{
			if (target == null)
				return;
			
			var children = new List<TNode>(target.Children);
			var childCount = children.Count;
			for (var i = 0; i < childCount; ++i)
			{
				NodeUtility<TNode>.RemoveChild(target, children[i]);
			}		
		}
	}
}