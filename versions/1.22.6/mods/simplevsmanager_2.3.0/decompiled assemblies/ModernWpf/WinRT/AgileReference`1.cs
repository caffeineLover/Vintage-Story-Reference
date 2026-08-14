using System;

namespace WinRT
{
	// Token: 0x02000184 RID: 388
	internal sealed class AgileReference<T> : AgileReference where T : class
	{
		// Token: 0x06000C3B RID: 3131 RVA: 0x00021B59 File Offset: 0x0001FD59
		public AgileReference(IObjectReference instance) : base(instance)
		{
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00021B62 File Offset: 0x0001FD62
		internal AgileReference(in ObjectReferenceValue instance) : base(instance)
		{
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00021B6C File Offset: 0x0001FD6C
		public new T Get()
		{
			T result;
			using (IObjectReference objectReference = base.Get())
			{
				result = ComWrappersSupport.CreateRcwForComObject<T>((objectReference != null) ? objectReference.ThisPtr : IntPtr.Zero);
			}
			return result;
		}
	}
}
