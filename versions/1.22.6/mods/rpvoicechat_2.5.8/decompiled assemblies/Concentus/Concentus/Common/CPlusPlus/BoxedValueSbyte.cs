using System;

namespace Concentus.Common.CPlusPlus
{
	// Token: 0x0200007F RID: 127
	internal class BoxedValueSbyte
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x00027C6D File Offset: 0x00025E6D
		internal BoxedValueSbyte(sbyte v = 0)
		{
			this.Val = v;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00027C7C File Offset: 0x00025E7C
		public override string ToString()
		{
			return this.Val.ToString();
		}

		// Token: 0x0400042C RID: 1068
		internal sbyte Val;
	}
}
