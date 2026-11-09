using System;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000137 RID: 311
	internal struct Exception
	{
		// Token: 0x06000A3D RID: 2621 RVA: 0x0001CC14 File Offset: 0x0001AE14
		public static Exception.Marshaler CreateMarshaler(Exception value)
		{
			return new Exception.Marshaler
			{
				__abi = new Exception
				{
					hr = ((value != null) ? ExceptionHelpers.GetHRForException(value) : 0)
				}
			};
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0001CC4D File Offset: 0x0001AE4D
		public static Exception GetAbi(Exception.Marshaler m)
		{
			return m.__abi;
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0001CC55 File Offset: 0x0001AE55
		public static Exception FromAbi(Exception value)
		{
			return ExceptionHelpers.GetExceptionForHR(value.hr);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0001CC62 File Offset: 0x0001AE62
		public unsafe static void CopyAbi(Exception.Marshaler arg, IntPtr dest)
		{
			*(Exception*)dest.ToPointer() = Exception.GetAbi(arg);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0001CC78 File Offset: 0x0001AE78
		public static Exception FromManaged(Exception value)
		{
			return new Exception
			{
				hr = ((value != null) ? ExceptionHelpers.GetHRForException(value) : 0)
			};
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0001CCA1 File Offset: 0x0001AEA1
		public unsafe static void CopyManaged(Exception arg, IntPtr dest)
		{
			*(Exception*)dest.ToPointer() = Exception.FromManaged(arg);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001CCB5 File Offset: 0x0001AEB5
		public static void DisposeMarshaler(Exception.Marshaler m)
		{
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0001CCB7 File Offset: 0x0001AEB7
		public static void DisposeAbi(Exception abi)
		{
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0001CCB9 File Offset: 0x0001AEB9
		public static string GetGuidSignature()
		{
			return "struct(Windows.Foundation.HResult;i4)";
		}

		// Token: 0x040001DC RID: 476
		public int hr;

		// Token: 0x0200034D RID: 845
		public struct Marshaler
		{
			// Token: 0x04000880 RID: 2176
			public Exception __abi;
		}
	}
}
