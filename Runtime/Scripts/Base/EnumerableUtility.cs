using System.Collections.Generic;


namespace UIKit
{
	/// <summary>
	/// 반복기 유틸리티.
	/// </summary>
	public static class EnumerableUtility<T>
	{
		/// <summary>
		/// 리스트로 변환.
		/// </summary>
		public static List<T> ToList(IEnumerable<T> enumerable)
		{
			var list = new List<T>();
			if (enumerable == null)
				return list;
			
			using var enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				list.Add(enumerator.Current);
			}

			return list;
		}
		
		/// <summary>
		/// 반복기 안에 대상이 포함되어있는지 여부.
		/// </summary>
		public static bool IsChild(IEnumerable<T> enumerable, T target)
		{
			if (enumerable == null)
				return false;
			if (target == null)
				return false;
			
			using var enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Equals(target))
					return true;
			}

			return false;
		}
		
		/// <summary>
		/// 요소 갯수 반환.
		/// </summary>
		public static int GetCount(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
				return 0;

			var childCount = 0;
			using var enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
				++childCount;
			return childCount;
		}
	}
}