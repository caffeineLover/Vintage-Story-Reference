using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using WinRT;
using WinRT.Interop;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000EC RID: 236
	[EditorBrowsable(1)]
	internal struct WwwFormUrlDecoder
	{
		// Token: 0x06000883 RID: 2179 RVA: 0x000184B0 File Offset: 0x000166B0
		public static IObjectReference CreateMarshaler(Windows.Foundation.WwwFormUrlDecoder obj)
		{
			if (obj != null)
			{
				return MarshalInspectable<Windows.Foundation.WwwFormUrlDecoder>.CreateMarshaler<IUnknownVftbl>(obj, GuidGenerator.GetIID(typeof(Windows.Foundation.IWwwFormUrlDecoderRuntimeClass).GetHelperType()), true);
			}
			return null;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000184D2 File Offset: 0x000166D2
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.WwwFormUrlDecoder obj)
		{
			return MarshalInspectable<object>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(Windows.Foundation.IWwwFormUrlDecoderRuntimeClass).GetHelperType()), true);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x000184EF File Offset: 0x000166EF
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00018500 File Offset: 0x00016700
		public static Windows.Foundation.WwwFormUrlDecoder FromAbi(IntPtr thisPtr)
		{
			return Windows.Foundation.WwwFormUrlDecoder.FromAbi(thisPtr);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00018508 File Offset: 0x00016708
		public static IntPtr FromManaged(Windows.Foundation.WwwFormUrlDecoder obj)
		{
			if (obj != null)
			{
				return WwwFormUrlDecoder.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001852C File Offset: 0x0001672C
		public static MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoder>.MarshalerArray CreateMarshalerArray(Windows.Foundation.WwwFormUrlDecoder[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoder>.CreateMarshalerArray2(array, (Windows.Foundation.WwwFormUrlDecoder o) => WwwFormUrlDecoder.CreateMarshaler2(o));
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00018553 File Offset: 0x00016753
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoder>.GetAbiArray(box);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0001855B File Offset: 0x0001675B
		public static Windows.Foundation.WwwFormUrlDecoder[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoder>.FromAbiArray(box, new Func<IntPtr, Windows.Foundation.WwwFormUrlDecoder>(WwwFormUrlDecoder.FromAbi));
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001856F File Offset: 0x0001676F
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.Foundation.WwwFormUrlDecoder[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoder>.FromManagedArray(array, (Windows.Foundation.WwwFormUrlDecoder o) => WwwFormUrlDecoder.FromManaged(o));
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00018596 File Offset: 0x00016796
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001859E File Offset: 0x0001679E
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoder>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.Foundation.WwwFormUrlDecoder>.DisposeMarshalerArray(array);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000185AB File Offset: 0x000167AB
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x000185B3 File Offset: 0x000167B3
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
