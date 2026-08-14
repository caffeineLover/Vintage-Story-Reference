using System;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x02000083 RID: 131
	public struct NullTerminatedString
	{
		// Token: 0x0600091D RID: 2333 RVA: 0x000165B2 File Offset: 0x000147B2
		public unsafe NullTerminatedString(byte* data)
		{
			this.Data = data;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x000165BC File Offset: 0x000147BC
		public unsafe override string ToString()
		{
			int num = 0;
			byte* ptr = this.Data;
			while (*ptr != 0)
			{
				num++;
				ptr++;
			}
			return Encoding.ASCII.GetString(this.Data, num);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x000165F1 File Offset: 0x000147F1
		public static implicit operator string(NullTerminatedString nts)
		{
			return nts.ToString();
		}

		// Token: 0x040007DE RID: 2014
		public unsafe readonly byte* Data;
	}
}
