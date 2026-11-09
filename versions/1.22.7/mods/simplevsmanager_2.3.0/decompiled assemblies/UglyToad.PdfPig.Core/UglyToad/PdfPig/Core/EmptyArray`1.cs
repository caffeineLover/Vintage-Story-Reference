using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000004 RID: 4
	public static class EmptyArray<T>
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000022C9 File Offset: 0x000004C9
		public static T[] Instance
		{
			[CompilerGenerated]
			get
			{
				return EmptyArray<T>.<Instance>k__BackingField;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022D0 File Offset: 0x000004D0
		// Note: this type is marked as 'beforefieldinit'.
		static EmptyArray()
		{
			EmptyArray<T>.<Instance>k__BackingField = new T[0];
		}
	}
}
