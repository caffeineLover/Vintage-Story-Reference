using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace Allclasses
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AllclassesDropStatsSystem : ModSystem
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000020CE File Offset: 0x000002CE
		public override bool ShouldLoad(EnumAppSide side)
		{
			return side == 1;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020D4 File Offset: 0x000002D4
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.characterSystem = api.ModLoader.GetModSystem<CharacterSystem>(true);
			api.Event.RegisterGameTickListener(delegate(float _)
			{
				foreach (IPlayer player in api.World.AllOnlinePlayers)
				{
					this.ApplyDropTraitStats(player);
				}
			}, 5000, 0);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002130 File Offset: 0x00000330
		private void ApplyDropTraitStats(IPlayer player)
		{
			EntityPlayer entity = player.Entity;
			EntityStats stats = (entity != null) ? entity.Stats : null;
			if (entity == null || stats == null || this.characterSystem == null)
			{
				return;
			}
			string classCode = entity.WatchedAttributes.GetString("characterClass", null);
			CharacterClass characterClass;
			if (classCode == null || !this.characterSystem.characterClassesByCode.TryGetValue(classCode, ref characterClass))
			{
				AllclassesDropStatsSystem.ClearDropTraitStats(stats);
				return;
			}
			Dictionary<string, float> totals = new Dictionary<string, float>();
			foreach (string stat in AllclassesDropStatsSystem.DropStats)
			{
				totals[stat] = 0f;
				stats.Remove(stat, "allclasses-dropstat-init");
				stats.Remove(stat, "trait");
			}
			this.AddTraitStats(characterClass.Traits, totals);
			string[] extraTraits = entity.WatchedAttributes.GetStringArray("extraTraits", null);
			if (extraTraits != null)
			{
				this.AddTraitStats(extraTraits, totals);
			}
			foreach (KeyValuePair<string, float> keyValuePair in totals)
			{
				string text;
				float num;
				keyValuePair.Deconstruct(ref text, ref num);
				string stat2 = text;
				float value = num;
				if (value != 0f)
				{
					stats.Set(stat2, "trait", value, true);
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000227C File Offset: 0x0000047C
		private void AddTraitStats(IEnumerable<string> traitCodes, Dictionary<string, float> totals)
		{
			foreach (string traitCode in traitCodes)
			{
				Trait trait;
				if (this.characterSystem.TraitsByCode.TryGetValue(traitCode, ref trait) && trait.Attributes != null)
				{
					foreach (string stat in AllclassesDropStatsSystem.DropStats)
					{
						double value;
						if (trait.Attributes.TryGetValue(stat, ref value))
						{
							string text = stat;
							totals[text] += (float)value;
						}
					}
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002328 File Offset: 0x00000528
		private static void ClearDropTraitStats(EntityStats stats)
		{
			foreach (string stat in AllclassesDropStatsSystem.DropStats)
			{
				stats.Remove(stat, "allclasses-dropstat-init");
				stats.Remove(stat, "trait");
			}
		}

		// Token: 0x04000001 RID: 1
		private const string TraitStatSource = "trait";

		// Token: 0x04000002 RID: 2
		private const string OldInitSource = "allclasses-dropstat-init";

		// Token: 0x04000003 RID: 3
		[Nullable(2)]
		private CharacterSystem characterSystem;

		// Token: 0x04000004 RID: 4
		private static readonly string[] DropStats = new string[]
		{
			"produceDropRate",
			"woodDropRate",
			"fruittreeDropRate",
			"treeseedDropRate",
			"stickDropRate",
			"charcoalPileDropRate",
			"temporalGearDropRate"
		};
	}
}
