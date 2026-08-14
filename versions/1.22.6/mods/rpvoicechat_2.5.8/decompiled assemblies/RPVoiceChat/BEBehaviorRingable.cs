using System;
using System.Text;
using RPVoiceChat.Util;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

// Token: 0x02000006 RID: 6
public class BEBehaviorRingable : BlockEntityBehavior
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000020 RID: 32 RVA: 0x00002BE5 File Offset: 0x00000DE5
	// (set) Token: 0x06000021 RID: 33 RVA: 0x00002BED File Offset: 0x00000DED
	public string BellPartCode { get; set; } = string.Empty;

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000022 RID: 34 RVA: 0x00002BF6 File Offset: 0x00000DF6
	// (set) Token: 0x06000023 RID: 35 RVA: 0x00002BFE File Offset: 0x00000DFE
	public DateTime? LastRung { get; set; }

	// Token: 0x06000024 RID: 36 RVA: 0x00002C07 File Offset: 0x00000E07
	public BEBehaviorRingable(BlockEntity blockEntity) : base(blockEntity)
	{
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002C1C File Offset: 0x00000E1C
	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		this.BellPartCode = (tree.GetString("bellPartCode", null) ?? string.Empty);
		string lastRungStr = tree.GetString("lastRung", null);
		if (!string.IsNullOrEmpty(lastRungStr))
		{
			DateTime parsed;
			if (DateTime.TryParseExact(lastRungStr, "o", null, 128, ref parsed))
			{
				this.LastRung = new DateTime?(parsed);
			}
			else
			{
				this.LastRung = default(DateTime?);
			}
		}
		else
		{
			this.LastRung = default(DateTime?);
		}
		base.FromTreeAttributes(tree, worldAccessForResolve);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002CA4 File Offset: 0x00000EA4
	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		tree.SetString("bellPartCode", this.BellPartCode);
		DateTime? lastRung = this.LastRung;
		if (lastRung != null)
		{
			string text = "lastRung";
			lastRung = this.LastRung;
			tree.SetString(text, (lastRung != null) ? lastRung.GetValueOrDefault().ToString("o") : null);
		}
		else if (tree.HasAttribute("lastRung"))
		{
			tree.RemoveAttribute("lastRung");
		}
		base.ToTreeAttributes(tree);
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002D25 File Offset: 0x00000F25
	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		if (!string.IsNullOrWhiteSpace(this.BellPartCode))
		{
			dsc.AppendLine(UIUtils.I18n("General.BellAttached"));
		}
	}
}
