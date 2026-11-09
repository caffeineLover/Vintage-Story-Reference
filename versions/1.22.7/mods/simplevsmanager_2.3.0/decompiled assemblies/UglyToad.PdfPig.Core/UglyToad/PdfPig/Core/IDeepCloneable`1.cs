using System;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000005 RID: 5
	public interface IDeepCloneable<out T>
	{
		// Token: 0x0600000F RID: 15
		T DeepClone();
	}
}
