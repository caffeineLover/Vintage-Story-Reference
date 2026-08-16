using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200019D RID: 413
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class SoundPacket
	{
		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x00063A47 File Offset: 0x00061C47
		// (set) Token: 0x06001408 RID: 5128 RVA: 0x00063A4F File Offset: 0x00061C4F
		public string Code { get; set; } = "";

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x00063A58 File Offset: 0x00061C58
		// (set) Token: 0x0600140A RID: 5130 RVA: 0x00063A60 File Offset: 0x00061C60
		public bool RandomizePitch { get; set; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x00063A69 File Offset: 0x00061C69
		// (set) Token: 0x0600140C RID: 5132 RVA: 0x00063A71 File Offset: 0x00061C71
		public float Range { get; set; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x00063A7A File Offset: 0x00061C7A
		// (set) Token: 0x0600140E RID: 5134 RVA: 0x00063A82 File Offset: 0x00061C82
		public float Volume { get; set; }

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x00063A8B File Offset: 0x00061C8B
		// (set) Token: 0x06001410 RID: 5136 RVA: 0x00063A93 File Offset: 0x00061C93
		public float Pitch { get; set; } = 1f;

		// Token: 0x06001411 RID: 5137 RVA: 0x00063A9C File Offset: 0x00061C9C
		public SoundPacket()
		{
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x00063ABA File Offset: 0x00061CBA
		public SoundPacket(SoundFrame frame, int index) : this(frame, index, frame.GetPitch(1f))
		{
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00063AD0 File Offset: 0x00061CD0
		public SoundPacket(SoundFrame frame, int index, float pitch)
		{
			this.Code = frame.Code[index];
			this.RandomizePitch = frame.RandomizePitch;
			this.Range = frame.Range;
			this.Volume = frame.Volume;
			this.Pitch = pitch;
		}
	}
}
