using System;
using NVorbis.Contracts.Ogg;

namespace NVorbis.Ogg
{
	// Token: 0x02000019 RID: 25
	internal class Crc : ICrc
	{
		// Token: 0x06000114 RID: 276 RVA: 0x000065D4 File Offset: 0x000047D4
		static Crc()
		{
			for (uint num = 0U; num < 256U; num += 1U)
			{
				uint num2 = num << 24;
				for (int i = 0; i < 8; i++)
				{
					num2 = (num2 << 1 ^ ((num2 >= 2147483648U) ? 79764919U : 0U));
				}
				Crc.s_crcTable[(int)num] = num2;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000662E File Offset: 0x0000482E
		public Crc()
		{
			this.Reset();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000663C File Offset: 0x0000483C
		public void Reset()
		{
			this._crc = 0U;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006645 File Offset: 0x00004845
		public void Update(int nextVal)
		{
			this._crc = (this._crc << 8 ^ Crc.s_crcTable[(int)(checked((IntPtr)(unchecked((long)nextVal ^ (long)((ulong)(this._crc >> 24))))))]);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000666A File Offset: 0x0000486A
		public bool Test(uint checkCrc)
		{
			return this._crc == checkCrc;
		}

		// Token: 0x04000086 RID: 134
		private const uint CRC32_POLY = 79764919U;

		// Token: 0x04000087 RID: 135
		private static readonly uint[] s_crcTable = new uint[256];

		// Token: 0x04000088 RID: 136
		private uint _crc;
	}
}
