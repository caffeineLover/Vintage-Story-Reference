using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000170 RID: 368
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CallbackFrameJson
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x00059F17 File Offset: 0x00058117
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x00059F1F File Offset: 0x0005811F
		public string Code { get; set; } = "";

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x00059F28 File Offset: 0x00058128
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x00059F30 File Offset: 0x00058130
		public float DurationFraction { get; set; }

		// Token: 0x06001288 RID: 4744 RVA: 0x00059F39 File Offset: 0x00058139
		public CallbackFrame ToCallbackFrame()
		{
			return new CallbackFrame(this.Code, this.DurationFraction);
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00059F4C File Offset: 0x0005814C
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
