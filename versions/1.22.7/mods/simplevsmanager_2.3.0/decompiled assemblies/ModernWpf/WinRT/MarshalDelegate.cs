using System;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x0200019D RID: 413
	internal static class MarshalDelegate
	{
		// Token: 0x06000D46 RID: 3398 RVA: 0x00026C8C File Offset: 0x00024E8C
		public static IObjectReference CreateMarshaler(object o, Guid delegateIID, bool unwrapObject = true)
		{
			if (o == null)
			{
				return null;
			}
			IObjectReference objectReference;
			if (unwrapObject && ComWrappersSupport.TryUnwrapObject(o, out objectReference))
			{
				return objectReference.As<IDelegateVftbl>(delegateIID);
			}
			return ComWrappersSupport.CreateCCWForObject<IDelegateVftbl>(o, delegateIID);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00026CBC File Offset: 0x00024EBC
		public static ObjectReferenceValue CreateMarshaler2(object o, Guid delegateIID, bool unwrapObject = true)
		{
			if (o == null)
			{
				return default(ObjectReferenceValue);
			}
			IObjectReference objectReference;
			if (unwrapObject && ComWrappersSupport.TryUnwrapObject(o, out objectReference))
			{
				return objectReference.AsValue(delegateIID);
			}
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(o, delegateIID);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00026CF4 File Offset: 0x00024EF4
		public static T FromAbi<T>(IntPtr nativeDelegate) where T : Delegate
		{
			if (nativeDelegate == IntPtr.Zero)
			{
				return default(T);
			}
			if (IUnknownVftbl.IsReferenceToManagedObject(nativeDelegate))
			{
				return ComWrappersSupport.FindObject<T>(nativeDelegate);
			}
			return ComWrappersSupport.CreateRcwForComObject<T>(nativeDelegate);
		}
	}
}
