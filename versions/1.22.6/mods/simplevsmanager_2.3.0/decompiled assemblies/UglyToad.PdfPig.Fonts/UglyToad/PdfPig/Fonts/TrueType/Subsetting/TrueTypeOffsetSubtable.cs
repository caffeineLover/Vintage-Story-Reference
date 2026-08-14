using System;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Subsetting
{
	// Token: 0x02000051 RID: 81
	internal class TrueTypeOffsetSubtable : IWriteable
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0000B70C File Offset: 0x0000990C
		public TrueTypeOffsetSubtable(byte numberOfTables)
		{
			this.numberOfTables = numberOfTables;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000B71C File Offset: 0x0000991C
		public void Write(Stream stream)
		{
			stream.Write(TrueTypeOffsetSubtable.VersionHeader, 0, TrueTypeOffsetSubtable.VersionHeader.Length);
			WritingExtensions.WriteUShort(stream, (ushort)this.numberOfTables);
			ushort highestPowerOf = TrueTypeOffsetSubtable.GetHighestPowerOf2((int)this.numberOfTables);
			ushort num = highestPowerOf * 16;
			ushort num2 = (ushort)Math.Log((double)highestPowerOf, 2.0);
			ushort num3 = (ushort)(this.numberOfTables * 16) - num;
			WritingExtensions.WriteUShort(stream, num);
			WritingExtensions.WriteUShort(stream, num2);
			WritingExtensions.WriteUShort(stream, num3);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000B78C File Offset: 0x0000998C
		private static ushort GetHighestPowerOf2(int numberOfTables)
		{
			ushort result = 1;
			for (int i = 0; i < 16; i++)
			{
				ushort num = (ushort)(1 << i);
				if ((int)num > numberOfTables)
				{
					break;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000B7B7 File Offset: 0x000099B7
		// Note: this type is marked as 'beforefieldinit'.
		static TrueTypeOffsetSubtable()
		{
			byte[] array = new byte[4];
			array[1] = 1;
			TrueTypeOffsetSubtable.VersionHeader = array;
		}

		// Token: 0x040001D3 RID: 467
		private static readonly byte[] VersionHeader;

		// Token: 0x040001D4 RID: 468
		private readonly byte numberOfTables;
	}
}
