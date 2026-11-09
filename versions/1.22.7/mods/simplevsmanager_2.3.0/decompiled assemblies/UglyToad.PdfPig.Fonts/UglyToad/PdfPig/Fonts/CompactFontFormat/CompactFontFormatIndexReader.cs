using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x0200008D RID: 141
	internal static class CompactFontFormatIndexReader
	{
		// Token: 0x0600033A RID: 826 RVA: 0x00016A2C File Offset: 0x00014C2C
		public static CompactFontFormatIndex ReadDictionaryData(CompactFontFormatData data)
		{
			int[] array = CompactFontFormatIndexReader.ReadIndex(data);
			if (array.Length == 0)
			{
				return new CompactFontFormatIndex(null);
			}
			int num = array.Length - 1;
			byte[][] array2 = new byte[num][];
			for (int i = 0; i < num; i++)
			{
				int num2 = array[i + 1] - array[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (num2 < 0)
				{
					defaultInterpolatedStringHandler..ctor(48, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Negative object length ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler.AppendLiteral(" at ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(". Current position: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(data.Position);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (num2 > data.Length)
				{
					defaultInterpolatedStringHandler..ctor(59, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Attempted to read data of length ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler.AppendLiteral(" in data array of length ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(data.Length);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				array2[i] = data.ReadBytes(num2);
			}
			return new CompactFontFormatIndex(array2);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00016B4C File Offset: 0x00014D4C
		public static int[] ReadIndex(CompactFontFormatData data)
		{
			ushort num = data.ReadCard16();
			if (num == 0)
			{
				return EmptyArray<int>.Instance;
			}
			byte offsetSize = data.ReadOffsize();
			int[] array = new int[(int)(num + 1)];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = data.ReadOffset((int)offsetSize);
			}
			return array;
		}
	}
}
