using System;

namespace ABI.System
{
	// Token: 0x0200011B RID: 283
	internal struct Char
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x0001B902 File Offset: 0x00019B02
		public static char CreateMarshaler(char value)
		{
			return value;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0001B908 File Offset: 0x00019B08
		public static Char GetAbi(char value)
		{
			return new Char
			{
				value = (ushort)value
			};
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0001B926 File Offset: 0x00019B26
		public static char FromAbi(Char abi)
		{
			return (char)abi.value;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0001B92E File Offset: 0x00019B2E
		public unsafe static void CopyAbi(char value, IntPtr dest)
		{
			*(short*)dest.ToPointer() = (short)Char.GetAbi(value).value;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0001B943 File Offset: 0x00019B43
		public static Char FromManaged(char value)
		{
			return Char.GetAbi(value);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0001B94B File Offset: 0x00019B4B
		public unsafe static void CopyManaged(char arg, IntPtr dest)
		{
			*(short*)dest.ToPointer() = (short)Char.FromManaged(arg).value;
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0001B960 File Offset: 0x00019B60
		public static void DisposeMarshaler(char m)
		{
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0001B962 File Offset: 0x00019B62
		public static void DisposeAbi(Char abi)
		{
		}

		// Token: 0x040001C2 RID: 450
		private ushort value;
	}
}
