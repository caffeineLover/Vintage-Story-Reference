using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010E RID: 270
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DamageResistData
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x00040C50 File Offset: 0x0003EE50
		public Dictionary<EnumDamageType, float> Resists { get; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00040C58 File Offset: 0x0003EE58
		public Dictionary<EnumDamageType, float> FlatDamageReduction { get; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00040C60 File Offset: 0x0003EE60
		public IEnumerable<DirectionConstrain> DirectionsCoverage { get; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00040C68 File Offset: 0x0003EE68
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x00040C6F File Offset: 0x0003EE6F
		public static int MaxAttackTier { get; set; } = 9;

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x00040C77 File Offset: 0x0003EE77
		// (set) Token: 0x06000D4E RID: 3406 RVA: 0x00040C7E File Offset: 0x0003EE7E
		public static int MaxArmorTier { get; set; } = 24;

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00040C86 File Offset: 0x0003EE86
		// (set) Token: 0x06000D50 RID: 3408 RVA: 0x00040C8D File Offset: 0x0003EE8D
		public static float[][] DamageReduction { get; set; } = new float[][]
		{
			new float[]
			{
				0.75f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.5f,
				0.75f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.25f,
				0.5f,
				0.75f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.1f,
				0.25f,
				0.5f,
				0.75f,
				1f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.05f,
				0.15f,
				0.33f,
				0.5f,
				0.75f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.03f,
				0.1f,
				0.25f,
				0.4f,
				0.5f,
				0.75f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.02f,
				0.05f,
				0.2f,
				0.33f,
				0.4f,
				0.5f,
				0.75f,
				1f,
				1f
			},
			new float[]
			{
				0.01f,
				0.03f,
				0.15f,
				0.25f,
				0.35f,
				0.45f,
				0.5f,
				0.75f,
				1f
			},
			new float[]
			{
				0.01f,
				0.02f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.45f,
				0.5f,
				0.75f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.05f,
				0.15f,
				0.25f,
				0.35f,
				0.4f,
				0.45f,
				0.5f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.03f,
				0.1f,
				0.2f,
				0.3f,
				0.35f,
				0.41f,
				0.46f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.02f,
				0.07f,
				0.15f,
				0.25f,
				0.3f,
				0.37f,
				0.42f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.05f,
				0.1f,
				0.2f,
				0.25f,
				0.33f,
				0.39f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.03f,
				0.07f,
				0.15f,
				0.2f,
				0.29f,
				0.36f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.02f,
				0.06f,
				0.1f,
				0.17f,
				0.25f,
				0.33f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.05f,
				0.08f,
				0.15f,
				0.21f,
				0.3f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.03f,
				0.07f,
				0.12f,
				0.18f,
				0.27f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.02f,
				0.06f,
				0.1f,
				0.15f,
				0.24f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.05f,
				0.08f,
				0.12f,
				0.21f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.03f,
				0.07f,
				0.1f,
				0.18f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.02f,
				0.06f,
				0.08f,
				0.15f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.05f,
				0.07f,
				0.12f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.03f,
				0.06f,
				0.1f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.02f,
				0.05f,
				0.08f
			}
		};

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00040C95 File Offset: 0x0003EE95
		// (set) Token: 0x06000D52 RID: 3410 RVA: 0x00040C9C File Offset: 0x0003EE9C
		public static float EntityProtectionFactor { get; set; } = 0.5f;

		// Token: 0x06000D53 RID: 3411 RVA: 0x00040CA4 File Offset: 0x0003EEA4
		public DamageResistData(Dictionary<EnumDamageType, float> resists, Dictionary<EnumDamageType, float> flatDamageReduction, IEnumerable<DirectionConstrain> directionsCoverage)
		{
			this.Resists = resists;
			this.FlatDamageReduction = flatDamageReduction;
			this.DirectionsCoverage = directionsCoverage;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00040CC1 File Offset: 0x0003EEC1
		public DamageResistData(Dictionary<EnumDamageType, float> resists, Dictionary<EnumDamageType, float> flatDamageReduction)
		{
			this.Resists = resists;
			this.FlatDamageReduction = flatDamageReduction;
			this.DirectionsCoverage = Array.Empty<DirectionConstrain>();
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00040CE2 File Offset: 0x0003EEE2
		public DamageResistData(Dictionary<EnumDamageType, float> resists)
		{
			this.Resists = resists;
			this.FlatDamageReduction = new Dictionary<EnumDamageType, float>();
			this.DirectionsCoverage = Array.Empty<DirectionConstrain>();
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00040D07 File Offset: 0x0003EF07
		public DamageResistData()
		{
			this.Resists = new Dictionary<EnumDamageType, float>();
			this.FlatDamageReduction = new Dictionary<EnumDamageType, float>();
			this.DirectionsCoverage = Array.Empty<DirectionConstrain>();
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x00040D30 File Offset: 0x0003EF30
		public static DamageResistData Empty
		{
			get
			{
				return new DamageResistData();
			}
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00040D38 File Offset: 0x0003EF38
		public bool CheckDirection(DirectionOffset direction)
		{
			return !Enumerable.Any<DirectionConstrain>(this.DirectionsCoverage) || Enumerable.Any<DirectionConstrain>(this.DirectionsCoverage, (DirectionConstrain constraint) => constraint.Check(direction));
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00040D78 File Offset: 0x0003EF78
		public DamageData ApplyResist(DamageData damageData, ref float damage)
		{
			float protectionLevel = 0f;
			float value;
			if (this.Resists.TryGetValue(damageData.DamageType, ref value))
			{
				protectionLevel = value;
				damage *= DamageResistData.DamageMultiplier(protectionLevel, damageData);
			}
			float flatReduction;
			if (this.FlatDamageReduction.TryGetValue(damageData.DamageType, ref flatReduction))
			{
				damage = Math.Clamp(damage - flatReduction, 0f, damage);
			}
			return new DamageData(damageData.DamageType, (int)((float)damageData.Tier - protectionLevel), damageData.ArmorPiercingTier);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00040DF0 File Offset: 0x0003EFF0
		public DamageData ApplyPlayerResist(DamageData damageData, ref float damage, out int durabilityDamage)
		{
			float protectionLevel = 0f;
			float initialDamage = damage;
			float flatReduction;
			if (this.FlatDamageReduction.TryGetValue(damageData.DamageType, ref flatReduction))
			{
				damage = Math.Clamp(damage - flatReduction, 0f, damage);
			}
			float value;
			if (this.Resists.TryGetValue(damageData.DamageType, ref value))
			{
				protectionLevel = value;
				damage *= DamageResistData.DamageMultiplierPlayer(protectionLevel, damageData);
			}
			durabilityDamage = (int)Math.Clamp(initialDamage - flatReduction, 0f, initialDamage);
			return new DamageData(damageData.DamageType, (int)((float)damageData.Tier - protectionLevel), damageData.ArmorPiercingTier);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00040E7C File Offset: 0x0003F07C
		public DamageData ApplyNonPlayerResist(DamageData damageData, ref float damage)
		{
			float protectionLevel = 0f;
			float flatReduction;
			if (this.FlatDamageReduction.TryGetValue(damageData.DamageType, ref flatReduction))
			{
				damage = Math.Clamp(damage - flatReduction, 0f, damage);
			}
			float value;
			if (this.Resists.TryGetValue(damageData.DamageType, ref value))
			{
				protectionLevel = value;
				damage *= DamageResistData.DamageMultiplierNonPlayer(protectionLevel, damageData);
			}
			return new DamageData(damageData.DamageType, (int)((float)damageData.Tier - protectionLevel), damageData.ArmorPiercingTier);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00040EF4 File Offset: 0x0003F0F4
		public static DamageResistData Combine(IEnumerable<DamageResistData> resists)
		{
			Dictionary<EnumDamageType, float> combinedResists = new Dictionary<EnumDamageType, float>();
			EnumDamageType enumDamageType;
			float num;
			foreach (KeyValuePair<EnumDamageType, float> keyValuePair in Enumerable.SelectMany<DamageResistData, KeyValuePair<EnumDamageType, float>>(resists, (DamageResistData element) => element.Resists))
			{
				keyValuePair.Deconstruct(ref enumDamageType, ref num);
				EnumDamageType damageType = enumDamageType;
				float protectionLevel = num;
				if (!combinedResists.ContainsKey(damageType))
				{
					combinedResists[damageType] = protectionLevel;
				}
				else
				{
					Dictionary<EnumDamageType, float> dictionary = combinedResists;
					enumDamageType = damageType;
					dictionary[enumDamageType] += protectionLevel;
				}
			}
			Dictionary<EnumDamageType, float> combinedFlat = new Dictionary<EnumDamageType, float>();
			foreach (KeyValuePair<EnumDamageType, float> keyValuePair in Enumerable.SelectMany<DamageResistData, KeyValuePair<EnumDamageType, float>>(resists, (DamageResistData element) => element.FlatDamageReduction))
			{
				keyValuePair.Deconstruct(ref enumDamageType, ref num);
				EnumDamageType damageType2 = enumDamageType;
				float protectionLevel2 = num;
				if (!combinedFlat.ContainsKey(damageType2))
				{
					combinedFlat[damageType2] = protectionLevel2;
				}
				else
				{
					Dictionary<EnumDamageType, float> dictionary = combinedFlat;
					enumDamageType = damageType2;
					dictionary[enumDamageType] += protectionLevel2;
				}
			}
			IEnumerable<DirectionConstrain> directionsCoverage = Enumerable.SelectMany<DamageResistData, DirectionConstrain>(resists, (DamageResistData element) => element.DirectionsCoverage);
			return new DamageResistData(combinedResists, combinedFlat, directionsCoverage);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00041074 File Offset: 0x0003F274
		private static float DamageMultiplier(float protectionLevel, DamageData damageData)
		{
			float result;
			switch (damageData.DamageType)
			{
			case 0:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 1:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 2:
				result = DamageResistData.LookupTableMultiplier(protectionLevel, (float)damageData.Tier);
				break;
			case 3:
				result = DamageResistData.LookupTableMultiplier(protectionLevel, (float)damageData.Tier);
				break;
			case 4:
				result = DamageResistData.LookupTableMultiplier(protectionLevel, (float)damageData.Tier);
				break;
			case 5:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 6:
				result = (float)(1 + damageData.Tier) + protectionLevel;
				break;
			case 7:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 8:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 9:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 10:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 11:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 12:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 13:
				result = 1f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x000411B8 File Offset: 0x0003F3B8
		private static float DamageMultiplierPlayer(float protectionLevel, DamageData damageData)
		{
			float result;
			switch (damageData.DamageType)
			{
			case 0:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 1:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 2:
				result = DamageResistData.LookupTableMultiplier(protectionLevel, (float)(damageData.Tier + damageData.ArmorPiercingTier));
				break;
			case 3:
				result = DamageResistData.LookupTableMultiplier(protectionLevel, (float)(damageData.Tier + damageData.ArmorPiercingTier));
				break;
			case 4:
				result = DamageResistData.LookupTableMultiplier(protectionLevel, (float)(damageData.Tier + damageData.ArmorPiercingTier));
				break;
			case 5:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 6:
				result = (float)(1 + damageData.Tier) + protectionLevel;
				break;
			case 7:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 8:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 9:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 10:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 11:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 12:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 13:
				result = 1f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00041310 File Offset: 0x0003F510
		private static float DamageMultiplierNonPlayer(float protectionLevel, DamageData damageData)
		{
			float result;
			switch (damageData.DamageType)
			{
			case 0:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 1:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 2:
				result = DamageResistData.FlatMultiplier(protectionLevel, (float)damageData.Tier);
				break;
			case 3:
				result = DamageResistData.FlatMultiplier(protectionLevel, (float)damageData.Tier);
				break;
			case 4:
				result = DamageResistData.FlatMultiplier(protectionLevel, (float)damageData.Tier);
				break;
			case 5:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 6:
				result = (float)(1 + damageData.Tier) + protectionLevel;
				break;
			case 7:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 8:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 9:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 10:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 11:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 12:
				result = DamageResistData.Percentage(protectionLevel, (float)damageData.Tier);
				break;
			case 13:
				result = 1f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00041453 File Offset: 0x0003F653
		private static float Percentage(float protection, float strength)
		{
			return Math.Clamp(1f + strength - protection, 0f, 1f);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0004146D File Offset: 0x0003F66D
		private static float LookupTableMultiplier(float protection, float attackTier)
		{
			return DamageResistData.LookupTableMultiplier((int)protection, (int)attackTier);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00041478 File Offset: 0x0003F678
		private static float LookupTableMultiplier(int protection, int attackTier)
		{
			if (protection == 0)
			{
				return 1f;
			}
			if (attackTier == 0 && protection > 0)
			{
				return 0f;
			}
			return DamageResistData.DamageReduction[GameMath.Clamp(protection - 1, 0, DamageResistData.MaxArmorTier - 1)][GameMath.Clamp(attackTier - 1, 0, DamageResistData.MaxAttackTier - 1)];
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x000414B7 File Offset: 0x0003F6B7
		private static float FlatMultiplier(float protection, float attackTier)
		{
			if (attackTier < protection)
			{
				return DamageResistData.EntityProtectionFactor;
			}
			return 1f;
		}
	}
}
