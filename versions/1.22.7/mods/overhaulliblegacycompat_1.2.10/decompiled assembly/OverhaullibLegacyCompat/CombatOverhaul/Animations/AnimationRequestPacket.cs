using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000174 RID: 372
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AnimationRequestPacket
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x0005A05A File Offset: 0x0005825A
		// (set) Token: 0x0600127F RID: 4735 RVA: 0x0005A062 File Offset: 0x00058262
		public bool MainHand { get; set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x0005A06B File Offset: 0x0005826B
		// (set) Token: 0x06001281 RID: 4737 RVA: 0x0005A073 File Offset: 0x00058273
		public string Animation { get; set; } = "";

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x0005A07C File Offset: 0x0005827C
		// (set) Token: 0x06001283 RID: 4739 RVA: 0x0005A084 File Offset: 0x00058284
		public float AnimationSpeed { get; set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x0005A08D File Offset: 0x0005828D
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x0005A095 File Offset: 0x00058295
		public float Weight { get; set; }

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x0005A09E File Offset: 0x0005829E
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x0005A0A6 File Offset: 0x000582A6
		public string Category { get; set; } = "";

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0005A0AF File Offset: 0x000582AF
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x0005A0B7 File Offset: 0x000582B7
		public double EaseOutDurationMs { get; set; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x0005A0C0 File Offset: 0x000582C0
		// (set) Token: 0x0600128B RID: 4747 RVA: 0x0005A0C8 File Offset: 0x000582C8
		public double EaseInDurationMs { get; set; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x0005A0D1 File Offset: 0x000582D1
		// (set) Token: 0x0600128D RID: 4749 RVA: 0x0005A0D9 File Offset: 0x000582D9
		public bool EaseOut { get; set; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x0005A0E2 File Offset: 0x000582E2
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x0005A0EA File Offset: 0x000582EA
		public long EntityId { get; set; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x0005A0F3 File Offset: 0x000582F3
		// (set) Token: 0x06001291 RID: 4753 RVA: 0x0005A0FB File Offset: 0x000582FB
		public int ItemId { get; set; }

		// Token: 0x06001292 RID: 4754 RVA: 0x0005A104 File Offset: 0x00058304
		public AnimationRequestPacket()
		{
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0005A124 File Offset: 0x00058324
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

		// Token: 0x06001294 RID: 4756 RVA: 0x0005A1C4 File Offset: 0x000583C4
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
