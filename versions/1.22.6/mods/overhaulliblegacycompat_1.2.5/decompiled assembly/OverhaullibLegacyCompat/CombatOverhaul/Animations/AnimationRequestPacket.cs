using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000168 RID: 360
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AnimationRequestPacket
	{
		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0005873A File Offset: 0x0005693A
		// (set) Token: 0x0600120B RID: 4619 RVA: 0x00058742 File Offset: 0x00056942
		public bool MainHand { get; set; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x0005874B File Offset: 0x0005694B
		// (set) Token: 0x0600120D RID: 4621 RVA: 0x00058753 File Offset: 0x00056953
		public string Animation { get; set; } = "";

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x0005875C File Offset: 0x0005695C
		// (set) Token: 0x0600120F RID: 4623 RVA: 0x00058764 File Offset: 0x00056964
		public float AnimationSpeed { get; set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x0005876D File Offset: 0x0005696D
		// (set) Token: 0x06001211 RID: 4625 RVA: 0x00058775 File Offset: 0x00056975
		public float Weight { get; set; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x0005877E File Offset: 0x0005697E
		// (set) Token: 0x06001213 RID: 4627 RVA: 0x00058786 File Offset: 0x00056986
		public string Category { get; set; } = "";

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001214 RID: 4628 RVA: 0x0005878F File Offset: 0x0005698F
		// (set) Token: 0x06001215 RID: 4629 RVA: 0x00058797 File Offset: 0x00056997
		public double EaseOutDurationMs { get; set; }

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x000587A0 File Offset: 0x000569A0
		// (set) Token: 0x06001217 RID: 4631 RVA: 0x000587A8 File Offset: 0x000569A8
		public double EaseInDurationMs { get; set; }

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x000587B1 File Offset: 0x000569B1
		// (set) Token: 0x06001219 RID: 4633 RVA: 0x000587B9 File Offset: 0x000569B9
		public bool EaseOut { get; set; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600121A RID: 4634 RVA: 0x000587C2 File Offset: 0x000569C2
		// (set) Token: 0x0600121B RID: 4635 RVA: 0x000587CA File Offset: 0x000569CA
		public long EntityId { get; set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x000587D3 File Offset: 0x000569D3
		// (set) Token: 0x0600121D RID: 4637 RVA: 0x000587DB File Offset: 0x000569DB
		public int ItemId { get; set; }

		// Token: 0x0600121E RID: 4638 RVA: 0x000587E4 File Offset: 0x000569E4
		public AnimationRequestPacket()
		{
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00058804 File Offset: 0x00056A04
		public AnimationRequestPacket(AnimationRequestByCode request, bool mainHand, long entityId, int itemId)
		{
			this.MainHand = mainHand;
			this.Animation = request.Animation;
			this.AnimationSpeed = request.AnimationSpeed;
			this.Weight = request.Weight;
			this.Category = request.Category;
			this.EaseOutDurationMs = request.EaseOutDuration.TotalMilliseconds;
			this.EaseInDurationMs = request.EaseInDuration.TotalMilliseconds;
			this.EaseOut = request.EaseOut;
			this.EntityId = entityId;
			this.ItemId = itemId;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x000588A4 File Offset: 0x00056AA4
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"request",
			"mainHand"
		})]
		public ValueTuple<AnimationRequestByCode, bool> ToRequest()
		{
			return new ValueTuple<AnimationRequestByCode, bool>(new AnimationRequestByCode(this.Animation, this.AnimationSpeed, this.Weight, this.Category, TimeSpan.FromMilliseconds(this.EaseOutDurationMs), TimeSpan.FromMilliseconds(this.EaseInDurationMs), this.EaseOut, null, null), this.MainHand);
		}
	}
}
