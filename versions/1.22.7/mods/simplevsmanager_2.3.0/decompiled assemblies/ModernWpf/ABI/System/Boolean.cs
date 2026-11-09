using System;

namespace ABI.System
{
	// Token: 0x0200011A RID: 282
	internal struct Boolean
	{
		// Token: 0x060009CA RID: 2506 RVA: 0x0001B898 File Offset: 0x00019A98
		public static bool CreateMarshaler(bool value)
		{
			return value;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0001B89C File Offset: 0x00019A9C
		public static Boolean GetAbi(bool value)
		{
			return new Boolean
			{
				value = (value ? 1 : 0)
			};
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0001B8C1 File Offset: 0x00019AC1
		public static bool FromAbi(Boolean abi)
		{
			return abi.value > 0;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0001B8CC File Offset: 0x00019ACC
		public unsafe static void CopyAbi(bool value, IntPtr dest)
		{
			*(byte*)dest.ToPointer() = Boolean.GetAbi(value).value;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0001B8E1 File Offset: 0x00019AE1
		public static Boolean FromManaged(bool value)
		{
			return Boolean.GetAbi(value);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0001B8E9 File Offset: 0x00019AE9
		public unsafe static void CopyManaged(bool arg, IntPtr dest)
		{
			*(byte*)dest.ToPointer() = Boolean.FromManaged(arg).value;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0001B8FE File Offset: 0x00019AFE
		public static void DisposeMarshaler(bool m)
		{
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0001B900 File Offset: 0x00019B00
		public static void DisposeAbi(Boolean abi)
		{
		}

		// Token: 0x040001C1 RID: 449
		private byte value;
	}
}
