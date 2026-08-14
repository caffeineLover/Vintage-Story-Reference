using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000046 RID: 70
	[Flags]
	internal enum ImplicitUseKindFlags
	{
		// Token: 0x040000C2 RID: 194
		Default = 7,
		// Token: 0x040000C3 RID: 195
		Access = 1,
		// Token: 0x040000C4 RID: 196
		Assign = 2,
		// Token: 0x040000C5 RID: 197
		InstantiatedWithFixedConstructorSignature = 4,
		// Token: 0x040000C6 RID: 198
		InstantiatedNoFixedConstructorSignature = 8
	}
}
