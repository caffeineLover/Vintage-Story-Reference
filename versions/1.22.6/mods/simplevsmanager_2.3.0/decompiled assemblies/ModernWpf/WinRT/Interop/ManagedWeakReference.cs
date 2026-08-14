using System;

namespace WinRT.Interop
{
	// Token: 0x020001D5 RID: 469
	internal sealed class ManagedWeakReference : IWeakReference
	{
		// Token: 0x06000E78 RID: 3704 RVA: 0x0002B3AF File Offset: 0x000295AF
		public ManagedWeakReference(object obj)
		{
			this._ref = new WeakReference<object>(obj);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0002B3C4 File Offset: 0x000295C4
		public IObjectReference Resolve(Guid riid)
		{
			object obj;
			if (!this._ref.TryGetTarget(ref obj))
			{
				return null;
			}
			return ComWrappersSupport.CreateCCWForObject<IUnknownVftbl>(obj, riid);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0002B3EC File Offset: 0x000295EC
		public IntPtr ResolveForABI(Guid riid)
		{
			object obj;
			if (!this._ref.TryGetTarget(ref obj))
			{
				return IntPtr.Zero;
			}
			return ComWrappersSupport.CreateCCWForObjectForABI(obj, riid);
		}

		// Token: 0x040002E5 RID: 741
		private readonly WeakReference<object> _ref;
	}
}
