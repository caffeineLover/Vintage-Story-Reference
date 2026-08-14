using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x02000104 RID: 260
	[EditorBrowsable(1)]
	internal struct StringMap
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x0001A273 File Offset: 0x00018473
		public static IObjectReference CreateMarshaler(Windows.Foundation.Collections.StringMap obj)
		{
			if (obj != null)
			{
				return MarshalInterface<IDictionary<string, string>>.CreateMarshaler(obj);
			}
			return null;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001A280 File Offset: 0x00018480
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.Collections.StringMap obj)
		{
			return MarshalInterface<IDictionary<string, string>>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(IDictionary<string, string>).GetHelperType()));
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001A29C File Offset: 0x0001849C
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001A2AD File Offset: 0x000184AD
		public static Windows.Foundation.Collections.StringMap FromAbi(IntPtr thisPtr)
		{
			return Windows.Foundation.Collections.StringMap.FromAbi(thisPtr);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001A2B8 File Offset: 0x000184B8
		public static IntPtr FromManaged(Windows.Foundation.Collections.StringMap obj)
		{
			if (obj != null)
			{
				return StringMap.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001A2DC File Offset: 0x000184DC
		public static MarshalInterfaceHelper<Windows.Foundation.Collections.StringMap>.MarshalerArray CreateMarshalerArray(Windows.Foundation.Collections.StringMap[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.StringMap>.CreateMarshalerArray2(array, (Windows.Foundation.Collections.StringMap o) => StringMap.CreateMarshaler2(o));
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001A303 File Offset: 0x00018503
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.StringMap>.GetAbiArray(box);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001A30B File Offset: 0x0001850B
		public static Windows.Foundation.Collections.StringMap[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.StringMap>.FromAbiArray(box, new Func<IntPtr, Windows.Foundation.Collections.StringMap>(StringMap.FromAbi));
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001A31F File Offset: 0x0001851F
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.Foundation.Collections.StringMap[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.StringMap>.FromManagedArray(array, (Windows.Foundation.Collections.StringMap o) => StringMap.FromManaged(o));
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001A346 File Offset: 0x00018546
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001A34E File Offset: 0x0001854E
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.Foundation.Collections.StringMap>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.Foundation.Collections.StringMap>.DisposeMarshalerArray(array);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001A35B File Offset: 0x0001855B
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001A363 File Offset: 0x00018563
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
