using System;

namespace Concentus.Common.CPlusPlus
{
	// Token: 0x0200007D RID: 125
	internal class BoxedValueInt
	{
		// Token: 0x0600046C RID: 1132 RVA: 0x00027C35 File Offset: 0x00025E35
		internal BoxedValueInt(int v = 0)
		{
			this.Val = v;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00027C44 File Offset: 0x00025E44
		public override string ToString()
		{
			return this.Val.ToString();
		}

		// Token: 0x0400042A RID: 1066
		internal int Val;
	}
}
