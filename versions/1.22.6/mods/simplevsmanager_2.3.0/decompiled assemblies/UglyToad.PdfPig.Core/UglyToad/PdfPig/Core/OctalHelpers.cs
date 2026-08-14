using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000009 RID: 9
	public static class OctalHelpers
	{
		// Token: 0x0600001F RID: 31 RVA: 0x000023B8 File Offset: 0x000005B8
		public static short CharacterToShort(this char c)
		{
			switch (c)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not convert the character ");
				defaultInterpolatedStringHandler.AppendFormatted<char>(c);
				defaultInterpolatedStringHandler.AppendLiteral(" to a short.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002444 File Offset: 0x00000644
		public static int FromOctalDigits(short[] octal)
		{
			int num = 0;
			for (int i = octal.Length - 1; i >= 0; i--)
			{
				int pow = i;
				num += (int)octal[i] * OctalHelpers.QuickPower(8, pow);
			}
			return num;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002474 File Offset: 0x00000674
		public static int FromOctalInt(int input)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(input);
			return Convert.ToInt32(defaultInterpolatedStringHandler.ToStringAndClear(), 8);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000024A0 File Offset: 0x000006A0
		private static int QuickPower(int x, int pow)
		{
			int num = 1;
			while (pow != 0)
			{
				if ((pow & 1) == 1)
				{
					num *= x;
				}
				x *= x;
				pow >>= 1;
			}
			return num;
		}
	}
}
