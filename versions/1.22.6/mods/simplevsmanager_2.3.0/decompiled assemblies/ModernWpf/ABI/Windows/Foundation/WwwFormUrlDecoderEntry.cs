using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000ED RID: 237
	[EditorBrowsable(1)]
	internal struct WwwFormUrlDecoderEntry
	{
		// Token: 0x06000890 RID: 2192 RVA: 0x000185BB File Offset: 0x000167BB
		public static IObjectReference CreateMarshaler(Windows.Foundation.WwwFormUrlDecoderEntry obj)
		{
			if (obj != null)
			{
				return MarshalInterface<Windows.Foundation.IWwwFormUrlDecoderEntry>.CreateMarshaler(obj);
			}
			return null;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x000185C8 File Offset: 0x000167C8
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.WwwFormUrlDecoderEntry obj)
		{
			return MarshalInterface<Windows.Foundation.IWwwFormUrlDecoderEntry>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(Windows.Foundation.IWwwFormUrlDecoderEntry).GetHelperType()));
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x000185E4 File Offset: 0x000167E4
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000185F5 File Offset: 0x000167F5
		public static Windows.Foundation.WwwFormUrlDecoderEntry FromAbi(IntPtr thisPtr)
		{
			return Windows.Foundation.WwwFormUrlDecoderEntry.FromAbi(thisPtr);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00018600 File Offset: 0x00016800
		public static IntPtr FromManaged(Windows.Foundation.WwwFormUrlDecoderEntry obj)
		{
			if (obj != null)
			{
				return WwwFormUrlDecoderEntry.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00018624 File Offset: 0x00016824
		public static MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoderEntry>.MarshalerArray CreateMarshalerArray(Windows.Foundation.WwwFormUrlDecoderEntry[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoderEntry>.CreateMarshalerArray2(array, (Windows.Foundation.WwwFormUrlDecoderEntry o) => WwwFormUrlDecoderEntry.CreateMarshaler2(o));
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001864B File Offset: 0x0001684B
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoderEntry>.GetAbiArray(box);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00018653 File Offset: 0x00016853
		public static Windows.Foundation.WwwFormUrlDecoderEntry[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoderEntry>.FromAbiArray(box, new Func<IntPtr, Windows.Foundation.WwwFormUrlDecoderEntry>(WwwFormUrlDecoderEntry.FromAbi));
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00018667 File Offset: 0x00016867
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.Foundation.WwwFormUrlDecoderEntry[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoderEntry>.FromManagedArray(array, (Windows.Foundation.WwwFormUrlDecoderEntry o) => WwwFormUrlDecoderEntry.FromManaged(o));
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001868E File Offset: 0x0001688E
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00018696 File Offset: 0x00016896
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoderEntry>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoderEntry>.DisposeMarshalerArray(array);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x000186A3 File Offset: 0x000168A3
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x000186AB File Offset: 0x000168AB
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
