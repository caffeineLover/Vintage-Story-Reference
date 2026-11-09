using System;
using WinRT;

namespace ABI.System
{
	// Token: 0x0200013B RID: 315
	internal struct Uri
	{
		// Token: 0x06000A5D RID: 2653 RVA: 0x0001D046 File Offset: 0x0001B246
		public static IObjectReference CreateMarshaler(Uri value)
		{
			if (value == null)
			{
				return null;
			}
			return Uri.ActivationFactory.Instance.CreateUri(value.OriginalString);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0001D060 File Offset: 0x0001B260
		public static ObjectReferenceValue CreateMarshaler2(Uri value)
		{
			if (value == null)
			{
				return default(ObjectReferenceValue);
			}
			return Uri.ActivationFactory.Instance.CreateUriForMarshaling(value.OriginalString);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0001D08A File Offset: 0x0001B28A
		public static IntPtr GetAbi(IObjectReference m)
		{
			if (m == null)
			{
				return IntPtr.Zero;
			}
			return m.ThisPtr;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0001D09C File Offset: 0x0001B29C
		public unsafe static Uri FromAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			Uri result;
			try
			{
				method get_RawUri_ = ((IntPtr*)((void*)ptr))->get_RawUri_10;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), ptr, &zero, get_RawUri_));
				result = new Uri(MarshalString.FromAbi(zero));
			}
			finally
			{
				MarshalString.DisposeAbi(zero);
			}
			return result;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0001D100 File Offset: 0x0001B300
		public unsafe static void CopyManaged(Uri o, IntPtr dest)
		{
			*(IntPtr*)dest.ToPointer() = Uri.CreateMarshaler2(o).Detach();
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0001D124 File Offset: 0x0001B324
		public static IntPtr FromManaged(Uri value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			return Uri.CreateMarshaler2(value).Detach();
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0001D148 File Offset: 0x0001B348
		public static void DisposeMarshaler(IObjectReference m)
		{
			if (m != null)
			{
				m.Dispose();
			}
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0001D153 File Offset: 0x0001B353
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0001D15B File Offset: 0x0001B35B
		public static string GetGuidSignature()
		{
			return "rc(Windows.Foundation.Uri;{9e365e57-48b2-4160-956f-c7385120bbfc})";
		}

		// Token: 0x02000352 RID: 850
		private sealed class ActivationFactory : BaseActivationFactory
		{
			// Token: 0x06001829 RID: 6185 RVA: 0x000439B3 File Offset: 0x00041BB3
			public ActivationFactory() : base("Windows.Foundation", "Windows.Foundation.Uri")
			{
			}

			// Token: 0x0400088B RID: 2187
			internal static WinRTUriRuntimeClassFactory Instance = new Uri.ActivationFactory()._As<WinRTUriRuntimeClassFactory.Vftbl>();
		}
	}
}
