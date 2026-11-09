using System;

namespace WinRT
{
	// Token: 0x0200019E RID: 414
	internal static class Marshaler
	{
		// Token: 0x04000263 RID: 611
		internal static Action<object> EmptyFunc = delegate(object box)
		{
		};

		// Token: 0x04000264 RID: 612
		internal static Func<object, object> ReturnParameterFunc = (object box) => box;

		// Token: 0x04000265 RID: 613
		internal static Action<object, IntPtr> CopyIntEnumFunc = delegate(object value, IntPtr dest)
		{
			*(int*)dest.ToPointer() = (int)Convert.ChangeType(value, typeof(int));
		};

		// Token: 0x04000266 RID: 614
		internal static Action<object, IntPtr> CopyUIntEnumFunc = delegate(object value, IntPtr dest)
		{
			*(int*)dest.ToPointer() = (int)((uint)Convert.ChangeType(value, typeof(uint)));
		};

		// Token: 0x04000267 RID: 615
		internal static Action<object, Lazy<Action<object>>> DisposeMarshaler = delegate(object arg, Lazy<Action<object>> genericDisposeMarshaler)
		{
			if (arg is ObjectReferenceValue)
			{
				((ObjectReferenceValue)arg).Dispose();
				return;
			}
			genericDisposeMarshaler.Value.Invoke(arg);
		};

		// Token: 0x04000268 RID: 616
		internal static Func<object, Lazy<Func<object, object>>, object> GetAbi = delegate(object arg, Lazy<Func<object, object>> genericGetAbi)
		{
			if (arg is ObjectReferenceValue)
			{
				return ((ObjectReferenceValue)arg).GetAbi();
			}
			return genericGetAbi.Value.Invoke(arg);
		};
	}
}
