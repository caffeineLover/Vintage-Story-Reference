using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200003F RID: 63
	[AttributeUsage(64)]
	internal sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
	{
		// Token: 0x0600016A RID: 362 RVA: 0x0000DFFD File Offset: 0x0000C1FD
		public NotifyPropertyChangedInvocatorAttribute()
		{
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000E005 File Offset: 0x0000C205
		public NotifyPropertyChangedInvocatorAttribute([NotNull] string parameterName)
		{
			this.ParameterName = parameterName;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000E014 File Offset: 0x0000C214
		// (set) Token: 0x0600016D RID: 365 RVA: 0x0000E01C File Offset: 0x0000C21C
		[CanBeNull]
		public string ParameterName { get; private set; }
	}
}
