//namespace UIKit
//{
//	/// <summary>
//	/// 공유 클래스.
//	/// </summary>
//	public class SharedClass<T> where T : SharedClass<T>
//	{
//		/// <summary>
//		/// 공유 클래스 인스턴스 프로퍼티.
//		/// </summary>
//		public static T Instnace => Repository.Get<T>();

//		/// <summary>
//		/// 공유 클래스 인스턴스 생성 여부 프로퍼티.
//		/// </summary>
//		public static bool IsCreated => Repository.IsRegistered<T>();

//		/// <summary>
//		/// 공유 클래스 인스턴스 생성.
//		/// </summary>
//		public static T Create()
//		{
//			return Repository.Get<T>();
//		}
//	}
//}