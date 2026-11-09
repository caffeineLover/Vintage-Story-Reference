using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017C RID: 380
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CallbackFrameJson
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x0005B837 File Offset: 0x00059A37
		// (set) Token: 0x060012F9 RID: 4857 RVA: 0x0005B83F File Offset: 0x00059A3F
		public string Code { get; set; } = "";

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x0005B848 File Offset: 0x00059A48
		// (set) Token: 0x060012FB RID: 4859 RVA: 0x0005B850 File Offset: 0x00059A50
		public float DurationFraction { get; set; }

		// Token: 0x060012FC RID: 4860 RVA: 0x0005B859 File Offset: 0x00059A59
		public CallbackFrame ToCallbackFrame()
		{
			return new CallbackFrame(this.Code, this.DurationFraction);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0005B86C File Offset: 0x00059A6C
		public static CallbackFrameJson FromCallbackFrame(CallbackFrame frame)
		{
			return new CallbackFrameJson
			{
				Code = frame.Code,
				DurationFraction = frame.DurationFraction
			};
		}
	}
}
