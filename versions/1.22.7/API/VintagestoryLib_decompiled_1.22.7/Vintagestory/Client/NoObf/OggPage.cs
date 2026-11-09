using System;
using csogg;

namespace Vintagestory.Client.NoObf;

public class OggPage : Page
{
	[ThreadStatic]
	private static uint[] crc_lookup;

	private static uint crc_entry(uint index)
	{
		uint r = index << 24;
		for (int i = 0; i < 8; i++)
		{
			r = (((r & 0x80000000u) == 0) ? (r << 1) : ((r << 1) ^ 0x4C11DB7));
		}
		return r & 0xFFFFFFFFu;
	}

	internal int version()
	{
		return base.header_base[base.header + 4] & 0xFF;
	}

	internal int continued()
	{
		return base.header_base[base.header + 5] & 1;
	}

	public int bos()
	{
		return base.header_base[base.header + 5] & 2;
	}

	public int eos()
	{
		return base.header_base[base.header + 5] & 4;
	}

	public long granulepos()
	{
		return ((((((((((((((long)(base.header_base[base.header + 13] & 0xFF) << 8) | (uint)(base.header_base[base.header + 12] & 0xFF)) << 8) | (uint)(base.header_base[base.header + 11] & 0xFF)) << 8) | (uint)(base.header_base[base.header + 10] & 0xFF)) << 8) | (uint)(base.header_base[base.header + 9] & 0xFF)) << 8) | (uint)(base.header_base[base.header + 8] & 0xFF)) << 8) | (uint)(base.header_base[base.header + 7] & 0xFF)) << 8) | (uint)(base.header_base[base.header + 6] & 0xFF);
	}

	public int serialno()
	{
		return (base.header_base[base.header + 14] & 0xFF) | ((base.header_base[base.header + 15] & 0xFF) << 8) | ((base.header_base[base.header + 16] & 0xFF) << 16) | ((base.header_base[base.header + 17] & 0xFF) << 24);
	}

	internal int pageno()
	{
		return (base.header_base[base.header + 18] & 0xFF) | ((base.header_base[base.header + 19] & 0xFF) << 8) | ((base.header_base[base.header + 20] & 0xFF) << 16) | ((base.header_base[base.header + 21] & 0xFF) << 24);
	}

	internal void checksum()
	{
		uint crc_reg = 0u;
		for (int i = 0; i < base.header_len; i++)
		{
			uint a = (uint)(base.header_base[base.header + i] & 0xFF);
			uint b = (crc_reg >> 24) & 0xFF;
			crc_reg = (crc_reg << 8) ^ crc_lookup[a ^ b];
		}
		for (int j = 0; j < base.body_len; j++)
		{
			uint a = (uint)(base.body_base[base.body + j] & 0xFF);
			uint b = (crc_reg >> 24) & 0xFF;
			crc_reg = (crc_reg << 8) ^ crc_lookup[a ^ b];
		}
		base.header_base[base.header + 22] = (byte)crc_reg;
		base.header_base[base.header + 23] = (byte)(crc_reg >> 8);
		base.header_base[base.header + 24] = (byte)(crc_reg >> 16);
		base.header_base[base.header + 25] = (byte)(crc_reg >> 24);
	}

	public OggPage()
	{
		if (crc_lookup == null)
		{
			crc_lookup = new uint[256];
			for (uint i = 0u; i < crc_lookup.Length; i++)
			{
				crc_lookup[i] = crc_entry(i);
			}
		}
	}
}
