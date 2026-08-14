using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x02000103 RID: 259
	[EditorBrowsable(1)]
	internal struct PropertySet
	{
		// Token: 0x0600091A RID: 2330 RVA: 0x0001A17C File Offset: 0x0001837C
		public static IObjectReference CreateMarshaler(Windows.Foundation.Collections.PropertySet obj)
		{
			if (obj != null)
			{
				return MarshalInterface<Windows.Foundation.Collections.IPropertySet>.CreateMarshaler(obj);
			}
			return null;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001A189 File Offset: 0x00018389
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.Collections.PropertySet obj)
		{
			return MarshalInterface<Windows.Foundation.Collections.IPropertySet>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(Windows.Foundation.Collections.IPropertySet).GetHelperType()));
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001A1A5 File Offset: 0x000183A5
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001A1B6 File Offset: 0x000183B6
		public static Windows.Foundation.Collections.PropertySet FromAbi(IntPtr thisPtr)
		{
			return Windows.Foundation.Collections.PropertySet.FromAbi(thisPtr);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001A1C0 File Offset: 0x000183C0
		public static IntPtr FromManaged(Windows.Foundation.Collections.PropertySet obj)
		{
			if (obj != null)
			{
				return PropertySet.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001A1E4 File Offset: 0x000183E4
		public static MarshalInterfaceHelper<Windows.Foundation.Collections.PropertySet>.MarshalerArray CreateMarshalerArray(Windows.Foundation.Collections.PropertySet[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.PropertySet>.CreateMarshalerArray2(array, (Windows.Foundation.Collections.PropertySet o) => PropertySet.CreateMarshaler2(o));
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001A20B File Offset: 0x0001840B
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.PropertySet>.GetAbiArray(box);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001A213 File Offset: 0x00018413
		public static Windows.Foundation.Collections.PropertySet[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.PropertySet>.FromAbiArray(box, new Func<IntPtr, Windows.Foundation.Collections.PropertySet>(PropertySet.FromAbi));
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001A227 File Offset: 0x00018427
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.Foundation.Collections.PropertySet[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.PropertySet>.FromManagedArray(array, (Windows.Foundation.Collections.PropertySet o) => PropertySet.FromManaged(o));
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001A24E File Offset: 0x0001844E
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001A256 File Offset: 0x00018456
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.Foundation.Collections.PropertySet>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.Foundation.Collections.PropertySet>.DisposeMarshalerArray(array);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001A263 File Offset: 0x00018463
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001A26B File Offset: 0x0001846B
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
