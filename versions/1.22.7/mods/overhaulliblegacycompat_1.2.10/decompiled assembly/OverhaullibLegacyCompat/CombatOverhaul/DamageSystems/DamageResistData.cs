using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000119 RID: 281
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DamageResistData
	{
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x00041C74 File Offset: 0x0003FE74
		public Dictionary<EnumDamageType, float> Resists { get; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x00041C7C File Offset: 0x0003FE7C
		public Dictionary<EnumDamageType, float> FlatDamageReduction { get; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00041C84 File Offset: 0x0003FE84
		public IEnumerable<DirectionConstrain> DirectionsCoverage { get; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x00041C8C File Offset: 0x0003FE8C
		// (set) Token: 0x06000DA8 RID: 3496 RVA: 0x00041C93 File Offset: 0x0003FE93
		public static int MaxAttackTier { get; set; } = 9;

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x00041C9B File Offset: 0x0003FE9B
		// (set) Token: 0x06000DAA RID: 3498 RVA: 0x00041CA2 File Offset: 0x0003FEA2
		public static int MaxArmorTier { get; set; } = 24;

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x00041CAA File Offset: 0x0003FEAA
		// (set) Token: 0x06000DAC RID: 3500 RVA: 0x00041CB1 File Offset: 0x0003FEB1
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

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x00041CB9 File Offset: 0x0003FEB9
		// (set) Token: 0x06000DAE RID: 3502 RVA: 0x00041CC0 File Offset: 0x0003FEC0
		public static float EntityProtectionFactor { get; set; } = 0.5f;

		// Token: 0x06000DAF RID: 3503 RVA: 0x00041CC8 File Offset: 0x0003FEC8
		public DamageResistData(Dictionary<EnumDamageType, float> resists, Dictionary<EnumDamageType, float> flatDamageReduction, IEnumerable<DirectionConstrain> directionsCoverage)
		{
			this.Resists = resists;
			this.FlatDamageReduction = flatDamageReduction;
			this.DirectionsCoverage = directionsCoverage;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00041CE5 File Offset: 0x0003FEE5
		public DamageResistData(Dictionary<EnumDamageType, float> resists, Dictionary<EnumDamageType, float> flatDamageReduction)
		{
			this.Resists = resists;
			this.FlatDamageReduction = flatDamageReduction;
			this.DirectionsCoverage = Array.Empty<DirectionConstrain>();
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00041D06 File Offset: 0x0003FF06
		public DamageResistData(Dictionary<EnumDamageType, float> resists)
		{
			this.Resists = resists;
			this.FlatDamageReduction = new Dictionary<EnumDamageType, float>();
			this.DirectionsCoverage = Array.Empty<DirectionConstrain>();
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00041D2B File Offset: 0x0003FF2B
		public DamageResistData()
		{
			this.Resists = new Dictionary<EnumDamageType, float>();
			this.FlatDamageReduction = new Dictionary<EnumDamageType, float>();
			this.DirectionsCoverage = Array.Empty<DirectionConstrain>();
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x00041D54 File Offset: 0x0003FF54
		public static DamageResistData Empty
		{
			get
			{
				return new DamageResistData();
			}
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00041D5C File Offset: 0x0003FF5C
		public bool CheckDirection(DirectionOffset direction)
		{
			return !Enumerable.Any<DirectionConstrain>(this.DirectionsCoverage) || Enumerable.Any<DirectionConstrain>(this.DirectionsCoverage, (DirectionConstrain constraint) => constraint.Check(direction));
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00041D9C File Offset: 0x0003FF9C
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

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00041E14 File Offset: 0x00040014
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

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00041EA0 File Offset: 0x000400A0
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

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00041F18 File Offset: 0x00040118
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

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00042098 File Offset: 0x00040298
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

		// Token: 0x06000DBA RID: 3514 RVA: 0x000421DC File Offset: 0x000403DC
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

		// Token: 0x06000DBB RID: 3515 RVA: 0x00042334 File Offset: 0x00040534
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

		// Token: 0x06000DBC RID: 3516 RVA: 0x00042477 File Offset: 0x00040677
		private static float Percentage(float protection, float strength)
		{
			return Math.Clamp(1f + strength - protection, 0f, 1f);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00042491 File Offset: 0x00040691
		private static float LookupTableMultiplier(float protection, float attackTier)
		{
			return DamageResistData.LookupTableMultiplier((int)protection, (int)attackTier);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0004249C File Offset: 0x0004069C
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

		// Token: 0x06000DBF RID: 3519 RVA: 0x000424DB File Offset: 0x000406DB
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
