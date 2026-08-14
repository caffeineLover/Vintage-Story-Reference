using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000048 RID: 72
	[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
	internal sealed class PublicAPIAttribute : Attribute
	{
		// Token: 0x0600018C RID: 396 RVA: 0x0000E164 File Offset: 0x0000C364
		public PublicAPIAttribute()
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000E16C File Offset: 0x0000C36C
		public PublicAPIAttribute([NotNull] string comment)
		{
			this.Comment = comment;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0000E17B File Offset: 0x0000C37B
		// (set) Token: 0x0600018F RID: 399 RVA: 0x0000E183 File Offset: 0x0000C383
		[CanBeNull]
		public string Comment { get; private set; }
	}
}
