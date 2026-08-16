using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000102 RID: 258
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DamageBlockJson
	{
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00040511 File Offset: 0x0003E711
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x00040519 File Offset: 0x0003E719
		public string[] Zones { get; set; } = Array.Empty<string>();

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00040522 File Offset: 0x0003E722
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x0004052A File Offset: 0x0003E72A
		public float[] Directions { get; set; } = Array.Empty<float>();

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00040533 File Offset: 0x0003E733
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x0004053B File Offset: 0x0003E73B
		[Nullable(2)]
		public string Sound { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00040544 File Offset: 0x0003E744
		// (set) Token: 0x06000D05 RID: 3333 RVA: 0x0004054C File Offset: 0x0003E74C
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public Dictionary<string, int> BlockTier { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00040555 File Offset: 0x0003E755
		// (set) Token: 0x06000D07 RID: 3335 RVA: 0x0004055D File Offset: 0x0003E75D
		public bool CanBlockProjectiles { get; set; } = true;

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00040566 File Offset: 0x0003E766
		// (set) Token: 0x06000D09 RID: 3337 RVA: 0x0004056E File Offset: 0x0003E76E
		public int StaggerTimeMs { get; set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00040577 File Offset: 0x0003E777
		// (set) Token: 0x06000D0B RID: 3339 RVA: 0x0004057F File Offset: 0x0003E77F
		public int StaggerTier { get; set; } = 1;

		// Token: 0x06000D0C RID: 3340 RVA: 0x00040588 File Offset: 0x0003E788
		public DamageBlockPacket ToPacket()
		{
			DamageBlockPacket damageBlockPacket = new DamageBlockPacket();
			IEnumerable<string> zones = this.Zones;
			Func<string, PlayerBodyPart> func;
			if ((func = DamageBlockJson.<>O.<0>__Parse) == null)
			{
				func = (DamageBlockJson.<>O.<0>__Parse = new Func<string, PlayerBodyPart>(Enum.Parse<PlayerBodyPart>));
			}
			damageBlockPacket.Zones = (int)Enumerable.Aggregate<PlayerBodyPart>(Enumerable.Select<string, PlayerBodyPart>(zones, func), (PlayerBodyPart first, PlayerBodyPart second) => first | second);
			damageBlockPacket.Directions = this.Directions;
			damageBlockPacket.Sound = this.Sound;
			Dictionary<string, int> blockTier = this.BlockTier;
			Dictionary<EnumDamageType, int> blockTier2;
			if (blockTier == null)
			{
				blockTier2 = null;
			}
			else
			{
				blockTier2 = Enumerable.ToDictionary<KeyValuePair<string, int>, EnumDamageType, int>(blockTier, (KeyValuePair<string, int> entry) => Enum.Parse<EnumDamageType>(entry.Key), (KeyValuePair<string, int> entry) => entry.Value);
			}
			damageBlockPacket.BlockTier = blockTier2;
			damageBlockPacket.CanBlockProjectiles = this.CanBlockProjectiles;
			damageBlockPacket.StaggerTimeMs = this.StaggerTimeMs;
			damageBlockPacket.StaggerTier = this.StaggerTier;
			return damageBlockPacket;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0004067C File Offset: 0x0003E87C
		public DamageBlockJson Clone()
		{
			DamageBlockJson damageBlockJson = new DamageBlockJson();
			damageBlockJson.Zones = this.Zones;
			damageBlockJson.Directions = this.Directions;
			damageBlockJson.Sound = this.Sound;
			Dictionary<string, int> blockTier = this.BlockTier;
			Dictionary<string, int> blockTier2;
			if (blockTier == null)
			{
				blockTier2 = null;
			}
			else
			{
				blockTier2 = Enumerable.ToDictionary<KeyValuePair<string, int>, string, int>(blockTier, (KeyValuePair<string, int> entry) => entry.Key, (KeyValuePair<string, int> entry) => entry.Value);
			}
			damageBlockJson.BlockTier = blockTier2;
			damageBlockJson.CanBlockProjectiles = this.CanBlockProjectiles;
			damageBlockJson.StaggerTimeMs = this.StaggerTimeMs;
			damageBlockJson.StaggerTier = this.StaggerTier;
			return damageBlockJson;
		}

		// Token: 0x0200025B RID: 603
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000C95 RID: 3221
			[Nullable(0)]
			public static Func<string, PlayerBodyPart> <0>__Parse;
		}
	}
}
