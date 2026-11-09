using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001BA RID: 442
	internal class BitStream
	{
		// Token: 0x06000A9E RID: 2718 RVA: 0x000351B4 File Offset: 0x000333B4
		public BitStream(IReadOnlyList<byte> data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.data = data;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x000351D4 File Offset: 0x000333D4
		public int Get(int numberOfBits)
		{
			int num = (numberOfBits + this.currentWithinByteBitOffset) % 8;
			int num2 = (numberOfBits + this.currentWithinByteBitOffset) / 8;
			if (num != 0)
			{
				num2++;
			}
			int num3 = 0;
			for (int i = 0; i < num2; i++)
			{
				if (i > 0)
				{
					this.currentByteIndex++;
				}
				if (this.currentByteIndex >= this.data.Count)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(61, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Reached the end of the bit stream while trying to read ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" bits.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				num3 <<= 8;
				num3 |= (int)this.data[this.currentByteIndex];
			}
			if (num > 0)
			{
				num3 >>= 8 - num;
			}
			else
			{
				this.currentByteIndex++;
			}
			int num4 = 32 - numberOfBits;
			num3 &= (int)(uint.MaxValue >> num4);
			this.currentWithinByteBitOffset = num;
			return num3;
		}

		// Token: 0x04000554 RID: 1364
		private readonly IReadOnlyList<byte> data;

		// Token: 0x04000555 RID: 1365
		private int currentWithinByteBitOffset;

		// Token: 0x04000556 RID: 1366
		private int currentByteIndex;
	}
}
