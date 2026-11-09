using System;

namespace Concentus.Common.CPlusPlus
{
	// Token: 0x0200007E RID: 126
	internal class BoxedValueShort
	{
		// Token: 0x0600046E RID: 1134 RVA: 0x00027C51 File Offset: 0x00025E51
		internal BoxedValueShort(short v = 0)
		{
			this.Val = v;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00027C60 File Offset: 0x00025E60
		public override string ToString()
		{
			return this.Val.ToString();
		}

		// Token: 0x0400042B RID: 1067
		internal short Val;
	}
}
