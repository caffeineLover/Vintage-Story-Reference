using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000045 RID: 69
	[AttributeUsage(16388)]
	internal sealed class MeansImplicitUseAttribute : Attribute
	{
		// Token: 0x06000184 RID: 388 RVA: 0x0000E10E File Offset: 0x0000C30E
		public MeansImplicitUseAttribute() : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000E118 File Offset: 0x0000C318
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags) : this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000E122 File Offset: 0x0000C322
		public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags) : this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000E12C File Offset: 0x0000C32C
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000E142 File Offset: 0x0000C342
		// (set) Token: 0x06000189 RID: 393 RVA: 0x0000E14A File Offset: 0x0000C34A
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000E153 File Offset: 0x0000C353
		// (set) Token: 0x0600018B RID: 395 RVA: 0x0000E15B File Offset: 0x0000C35B
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; private set; }
	}
}
