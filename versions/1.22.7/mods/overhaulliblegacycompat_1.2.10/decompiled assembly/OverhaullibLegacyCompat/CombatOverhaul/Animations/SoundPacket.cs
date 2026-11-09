using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001AB RID: 427
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class SoundPacket
	{
		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x0600147F RID: 5247 RVA: 0x000654C7 File Offset: 0x000636C7
		// (set) Token: 0x06001480 RID: 5248 RVA: 0x000654CF File Offset: 0x000636CF
		public string Code { get; set; } = "";

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x000654D8 File Offset: 0x000636D8
		// (set) Token: 0x06001482 RID: 5250 RVA: 0x000654E0 File Offset: 0x000636E0
		public bool RandomizePitch { get; set; }

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x000654E9 File Offset: 0x000636E9
		// (set) Token: 0x06001484 RID: 5252 RVA: 0x000654F1 File Offset: 0x000636F1
		public float Range { get; set; }

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x000654FA File Offset: 0x000636FA
		// (set) Token: 0x06001486 RID: 5254 RVA: 0x00065502 File Offset: 0x00063702
		public float Volume { get; set; }

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0006550B File Offset: 0x0006370B
		// (set) Token: 0x06001488 RID: 5256 RVA: 0x00065513 File Offset: 0x00063713
		public float Pitch { get; set; } = 1f;

		// Token: 0x06001489 RID: 5257 RVA: 0x0006551C File Offset: 0x0006371C
		public SoundPacket()
		{
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0006553A File Offset: 0x0006373A
		public SoundPacket(SoundFrame frame, int index) : this(frame, index, frame.GetPitch(1f))
		{
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00065550 File Offset: 0x00063750
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
