using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType
{
	// Token: 0x02000031 RID: 49
	public static class TrueTypeChecksumCalculator
	{
		// Token: 0x0600012D RID: 301 RVA: 0x0000758C File Offset: 0x0000578C
		public static uint CalculateWholeFontChecksum(IInputBytes bytes, TrueTypeHeaderTable headerTable)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (!TrueTypeChecksumCalculator.IsHeadTable(headerTable))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(86, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Can only calculate checksum for the whole font when the head table is provided. Got: ");
				defaultInterpolatedStringHandler.AppendFormatted<TrueTypeHeaderTable>(headerTable);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bytes.Seek(0L);
			return TrueTypeChecksumCalculator.Calculate(TrueTypeChecksumCalculator.ToChecksumSkippedEnumerable(bytes, headerTable));
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000075FC File Offset: 0x000057FC
		public static uint Calculate(IInputBytes bytes, TrueTypeHeaderTable table)
		{
			bytes.Seek((long)((ulong)table.Offset));
			if (!TrueTypeChecksumCalculator.IsHeadTable(table))
			{
				uint num = 0U;
				uint num2;
				while (TrueTypeChecksumCalculator.TryReadUInt(bytes, (long)((ulong)(table.Offset + table.Length)), out num2))
				{
					num += num2;
				}
				return num;
			}
			byte[] array = new byte[table.Length];
			if ((long)bytes.Read(array, default(int?)) != (long)((ulong)table.Length))
			{
				throw new InvalidOperationException();
			}
			array[8] = 0;
			array[9] = 0;
			array[10] = 0;
			array[11] = 0;
			return TrueTypeChecksumCalculator.Calculate(array);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000768C File Offset: 0x0000588C
		public static uint Calculate(IEnumerable<byte> bytes)
		{
			uint num = 0U;
			using (IEnumerator<byte> enumerator = bytes.GetEnumerator())
			{
				uint num2;
				while (TrueTypeChecksumCalculator.TryReadUInt(enumerator, out num2))
				{
					num += num2;
				}
			}
			return num;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000076D0 File Offset: 0x000058D0
		private static bool IsHeadTable(TrueTypeHeaderTable table)
		{
			return string.Equals("head", table.Tag, 5);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000076E4 File Offset: 0x000058E4
		private static bool TryReadUInt(IEnumerator<byte> enumerator, out uint result)
		{
			result = 0U;
			if (!enumerator.MoveNext())
			{
				return false;
			}
			byte b = enumerator.Current;
			int num = (int)(enumerator.MoveNext() ? enumerator.Current : 0);
			int num2 = (int)(enumerator.MoveNext() ? enumerator.Current : 0);
			int num3 = (int)(enumerator.MoveNext() ? enumerator.Current : 0);
			result = (uint)(((ulong)b << 24) + (ulong)((ulong)((long)num) << 16) + (ulong)((long)((long)num2 << 8)) + (ulong)((long)num3));
			return true;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007754 File Offset: 0x00005954
		private static bool TryReadUInt(IInputBytes input, long endAt, out uint result)
		{
			TrueTypeChecksumCalculator.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.input = input;
			CS$<>8__locals1.endAt = endAt;
			result = 0U;
			if (CS$<>8__locals1.input.CurrentOffset >= CS$<>8__locals1.endAt)
			{
				return false;
			}
			byte b = TrueTypeChecksumCalculator.<TryReadUInt>g__ReadNext|7_0(ref CS$<>8__locals1);
			byte b2 = TrueTypeChecksumCalculator.<TryReadUInt>g__ReadNext|7_0(ref CS$<>8__locals1);
			byte b3 = TrueTypeChecksumCalculator.<TryReadUInt>g__ReadNext|7_0(ref CS$<>8__locals1);
			byte b4 = TrueTypeChecksumCalculator.<TryReadUInt>g__ReadNext|7_0(ref CS$<>8__locals1);
			result = (uint)(((ulong)b << 24) + ((ulong)b2 << 16) + (ulong)((long)((long)b3 << 8)) + (ulong)((long)b4));
			return true;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000077C2 File Offset: 0x000059C2
		private static IEnumerable<byte> ToChecksumSkippedEnumerable(IInputBytes bytes, TrueTypeHeaderTable table)
		{
			while (bytes.MoveNext())
			{
				if (bytes.CurrentOffset <= (long)((ulong)(table.Offset + 8U)) || bytes.CurrentOffset > (long)((ulong)(table.Offset + 8U + 4U)))
				{
					yield return bytes.CurrentByte;
				}
			}
			yield break;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000077D9 File Offset: 0x000059D9
		[CompilerGenerated]
		internal static byte <TryReadUInt>g__ReadNext|7_0(ref TrueTypeChecksumCalculator.<>c__DisplayClass7_0 A_0)
		{
			if (A_0.input.CurrentOffset == A_0.endAt || !A_0.input.MoveNext())
			{
				return 0;
			}
			return A_0.input.CurrentByte;
		}

		// Token: 0x0400010E RID: 270
		private const string HeaderTableTag = "head";

		// Token: 0x0400010F RID: 271
		private const int ChecksumAdjustmentPosition = 8;
	}
}
