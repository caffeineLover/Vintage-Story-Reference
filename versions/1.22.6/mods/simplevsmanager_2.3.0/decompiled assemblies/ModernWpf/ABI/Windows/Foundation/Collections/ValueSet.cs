using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x02000105 RID: 261
	[EditorBrowsable(1)]
	internal struct ValueSet
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x0001A36B File Offset: 0x0001856B
		public static IObjectReference CreateMarshaler(Windows.Foundation.Collections.ValueSet obj)
		{
			if (obj != null)
			{
				return MarshalInterface<Windows.Foundation.Collections.IPropertySet>.CreateMarshaler(obj);
			}
			return null;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001A378 File Offset: 0x00018578
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.Collections.ValueSet obj)
		{
			return MarshalInterface<Windows.Foundation.Collections.IPropertySet>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(Windows.Foundation.Collections.IPropertySet).GetHelperType()));
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001A394 File Offset: 0x00018594
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001A3A5 File Offset: 0x000185A5
		public static Windows.Foundation.Collections.ValueSet FromAbi(IntPtr thisPtr)
		{
			return Windows.Foundation.Collections.ValueSet.FromAbi(thisPtr);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0001A3B0 File Offset: 0x000185B0
		public static IntPtr FromManaged(Windows.Foundation.Collections.ValueSet obj)
		{
			if (obj != null)
			{
				return ValueSet.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0001A3D4 File Offset: 0x000185D4
		public static MarshalInterfaceHelper<Windows.Foundation.Collections.ValueSet>.MarshalerArray CreateMarshalerArray(Windows.Foundation.Collections.ValueSet[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.ValueSet>.CreateMarshalerArray2(array, (Windows.Foundation.Collections.ValueSet o) => ValueSet.CreateMarshaler2(o));
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0001A3FB File Offset: 0x000185FB
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.ValueSet>.GetAbiArray(box);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001A403 File Offset: 0x00018603
		public static Windows.Foundation.Collections.ValueSet[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.ValueSet>.FromAbiArray(box, new Func<IntPtr, Windows.Foundation.Collections.ValueSet>(ValueSet.FromAbi));
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0001A417 File Offset: 0x00018617
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.Foundation.Collections.ValueSet[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.ValueSet>.FromManagedArray(array, (Windows.Foundation.Collections.ValueSet o) => ValueSet.FromManaged(o));
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0001A43E File Offset: 0x0001863E
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0001A446 File Offset: 0x00018646
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.Foundation.Collections.ValueSet>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.Foundation.Collections.ValueSet>.DisposeMarshalerArray(array);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0001A453 File Offset: 0x00018653
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0001A45B File Offset: 0x0001865B
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
