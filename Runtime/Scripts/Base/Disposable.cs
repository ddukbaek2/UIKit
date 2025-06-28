using System;
//using System.Collections.Concurrent;


namespace UIKit
{
	/// <summary>
	/// 명시적으로 내부 자원의 사용 해제 구간이 존재하는 오브젝트.
	/// <para>단, 객체의 제거를 의미하진 않으므로 Dispose() 후에 실제 객체가 null이 되려면 모든 참조를 끊어야 한다.</para>
	/// </summary>
	public abstract class Disposable : IDisposable
	{
		///// <summary>
		///// 실제 인스턴스를 단일 참조로 구현.
		///// <para>Dispose()에 의한 제거시 완벽히 제거됨.</para>
		///// </summary>
		//private static ConcurrentDictionary<Disposable, int> s_SingleReferences = new ConcurrentDictionary<Disposable, int>();

		/// <summary>
		/// 해제되었는지 여부.
		/// </summary>
		private bool m_IsDisposed;

		/// <summary>
		/// 해제되었는지 여부 프로퍼티.
		/// </summary>
		public bool IsDisposed => m_IsDisposed;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Disposable()
		{
			m_IsDisposed = false;
			//s_SingleReferences.TryAdd(this, 0);
		}

		/// <summary>
		/// 소멸됨.
		/// </summary>
		~Disposable()
		{
			if (m_IsDisposed)
				return;

			try
			{
				m_IsDisposed = true;
				OnDispose(false);
				//s_SingleReferences.TryRemove(this, out var value);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected abstract void OnDispose(bool explicitDisposing);

		/// <summary>
		/// 해제.
		/// </summary>
		public void Dispose()
		{
			if (m_IsDisposed)
				return;

			try
			{
				m_IsDisposed = true;
				GC.SuppressFinalize(this); // 소멸자 실행 중단.
				OnDispose(true);
				//s_SingleReferences.TryRemove(this, out var value);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 객체 생성.
		/// <para>명시적으로 함수를 제공하기 위해 작성. 실제로는 new TView()로 생성해도 동일한 결과.</para>
		/// </summary>
		public static T Create<T>(params object[] arguments) where T : Disposable
		{
			var obj = Reflector.CreateInstance<T>(arguments);
			return obj;
		}

		/// <summary>
		/// 객체 생성.
		/// </summary>
		public static WeakReference<T> CreateAsWeakReference<T>(params object[] arguments) where T : Disposable
		{
			var obj = Create<T>(arguments);
			return new WeakReference<T>(obj);
		}

		/// <summary>
		/// 안전한 해제.
		/// </summary>
		public static void SafeDispose(ref object obj)
		{
			if (obj is IDisposable)
			{
				if (obj is Disposable)
				{
					var disposable = obj as Disposable;
					if (disposable.IsDisposed)
						return;

					disposable.Dispose();
					obj = null;
				}
			}
		}

		/// <summary>
		/// 안전한 해제 여부 반환.
		/// </summary>
		public static bool IsSafeDisposed(object obj)
		{
			if (obj is IDisposable)
			{
				if (obj is Disposable)
				{
					var disposable = obj as Disposable;
					if (disposable.IsDisposed)
						return false;

					return true;
				}
			}

			return false;
		}
	}
}