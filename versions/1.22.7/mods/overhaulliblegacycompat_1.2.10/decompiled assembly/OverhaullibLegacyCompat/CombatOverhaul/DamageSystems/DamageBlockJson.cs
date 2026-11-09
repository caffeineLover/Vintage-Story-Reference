using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010D RID: 269
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DamageBlockJson
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00041501 File Offset: 0x0003F701
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x00041509 File Offset: 0x0003F709
		public string[] Zones { get; set; } = Array.Empty<string>();

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00041512 File Offset: 0x0003F712
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x0004151A File Offset: 0x0003F71A
		public float[] Directions { get; set; } = Array.Empty<float>();

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00041523 File Offset: 0x0003F723
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x0004152B File Offset: 0x0003F72B
		[Nullable(2)]
		public string Sound { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00041534 File Offset: 0x0003F734
		// (set) Token: 0x06000D61 RID: 3425 RVA: 0x0004153C File Offset: 0x0003F73C
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

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00041545 File Offset: 0x0003F745
		// (set) Token: 0x06000D63 RID: 3427 RVA: 0x0004154D File Offset: 0x0003F74D
		public bool CanBlockProjectiles { get; set; } = true;

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00041556 File Offset: 0x0003F756
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x0004155E File Offset: 0x0003F75E
		public int StaggerTimeMs { get; set; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00041567 File Offset: 0x0003F767
		// (set) Token: 0x06000D67 RID: 3431 RVA: 0x0004156F File Offset: 0x0003F76F
		public int StaggerTier { get; set; } = 1;

		// Token: 0x06000D68 RID: 3432 RVA: 0x00041578 File Offset: 0x0003F778
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

		// Token: 0x06000D69 RID: 3433 RVA: 0x0004166C File Offset: 0x0003F86C
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

		// Token: 0x0200026A RID: 618
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000CBF RID: 3263
			[Nullable(0)]
			public static Func<string, PlayerBodyPart> <0>__Parse;
		}
	}
}
