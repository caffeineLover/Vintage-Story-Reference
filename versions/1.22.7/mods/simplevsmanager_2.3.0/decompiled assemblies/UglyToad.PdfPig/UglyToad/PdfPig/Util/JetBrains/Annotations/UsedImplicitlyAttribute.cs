using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000044 RID: 68
	[AttributeUsage(32767)]
	internal sealed class UsedImplicitlyAttribute : Attribute
	{
		// Token: 0x0600017C RID: 380 RVA: 0x0000E0B8 File Offset: 0x0000C2B8
		public UsedImplicitlyAttribute() : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000E0C2 File Offset: 0x0000C2C2
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags) : this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000E0CC File Offset: 0x0000C2CC
		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags) : this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000E0D6 File Offset: 0x0000C2D6
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000E0EC File Offset: 0x0000C2EC
		// (set) Token: 0x06000181 RID: 385 RVA: 0x0000E0F4 File Offset: 0x0000C2F4
		public ImplicitUseKindFlags UseKindFlags { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000E0FD File Offset: 0x0000C2FD
		// (set) Token: 0x06000183 RID: 387 RVA: 0x0000E105 File Offset: 0x0000C305
		public ImplicitUseTargetFlags TargetFlags { get; private set; }
	}
}
