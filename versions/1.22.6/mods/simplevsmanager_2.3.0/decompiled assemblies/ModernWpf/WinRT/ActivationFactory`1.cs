using System;

namespace WinRT
{
	// Token: 0x020001B0 RID: 432
	internal sealed class ActivationFactory<T> : BaseActivationFactory
	{
		// Token: 0x06000DEE RID: 3566 RVA: 0x0002A5A3 File Offset: 0x000287A3
		public ActivationFactory() : base(typeof(T).Namespace, typeof(T).FullName)
		{
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0002A5C9 File Offset: 0x000287C9
		public static I AsInterface<I>()
		{
			return ActivationFactory<T>._factory.Value.AsInterface<I>();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0002A5DA File Offset: 0x000287DA
		public static ObjectReference<I> As<I>()
		{
			return ActivationFactory<T>._factory._As<I>();
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0002A5E6 File Offset: 0x000287E6
		public static IObjectReference As(Guid iid)
		{
			return ActivationFactory<T>._factory._As(iid);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0002A5F3 File Offset: 0x000287F3
		public static ObjectReference<I> ActivateInstance<I>()
		{
			return ActivationFactory<T>._factory._ActivateInstance<I>();
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0002A5FF File Offset: 0x000287FF
		// Note: this type is marked as 'beforefieldinit'.
		static ActivationFactory()
		{
			ActivationFactory<T>._factory = new ActivationFactory<T>();
		}

		// Token: 0x040002AE RID: 686
		private static readonly ActivationFactory<T> _factory;
	}
}
