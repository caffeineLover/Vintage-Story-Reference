using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000120 RID: 288
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ArmorFootStepSoundUtil
	{
		// Token: 0x06000E07 RID: 3591 RVA: 0x0004461C File Offset: 0x0004281C
		public static AssetLocation[] GetFootStepSounds([Nullable(2)] ICoreAPI api, ItemSlot slot, CollectibleObject collectible, [Nullable(2)] CollectibleBehaviorWearable wearable)
		{
			AssetLocation[] sounds = ArmorFootStepSoundUtil.ResolveAvailableFootStepSounds(api, slot, collectible, wearable);
			if (sounds.Length == 0)
			{
				return Array.Empty<AssetLocation>();
			}
			if (!ArmorFootStepSoundUtil.IsSelectedFootStepArmor(api, slot))
			{
				return Array.Empty<AssetLocation>();
			}
			return sounds;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00044650 File Offset: 0x00042850
		private static bool IsSelectedFootStepArmor([Nullable(2)] ICoreAPI api, ItemSlot slot)
		{
			if (slot.Inventory == null || slot.Itemstack == null)
			{
				return true;
			}
			ValueTuple<int, int, int> current = ArmorFootStepSoundUtil.GetFootStepArmorPriority(api, slot);
			if (current.Item1 <= 0)
			{
				return true;
			}
			foreach (ItemSlot candidate in slot.Inventory)
			{
				if (candidate != slot && !candidate.Empty)
				{
					ValueTuple<int, int, int> other = ArmorFootStepSoundUtil.GetFootStepArmorPriority(api, candidate);
					if (other.Item1 > 0)
					{
						if (other.Item1 > current.Item1)
						{
							return false;
						}
						if (other.Item1 >= current.Item1)
						{
							if (other.Item2 > current.Item2)
							{
								return false;
							}
							if (other.Item2 >= current.Item2 && other.Item3 >= 0 && (current.Item3 < 0 || other.Item3 < current.Item3))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0004474C File Offset: 0x0004294C
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"soundPriority",
			"zonePriority",
			"slotId"
		})]
		private static ValueTuple<int, int, int> GetFootStepArmorPriority([Nullable(2)] ICoreAPI api, [Nullable(1)] ItemSlot slot)
		{
			ItemStack itemstack = slot.Itemstack;
			CollectibleObject collectible = (itemstack != null) ? itemstack.Collectible : null;
			if (collectible == null)
			{
				return new ValueTuple<int, int, int>(0, 0, -1);
			}
			if (collectible.GetCollectibleBehavior<ArmorBehavior>(true) == null)
			{
				return new ValueTuple<int, int, int>(0, 0, -1);
			}
			CollectibleBehaviorWearable wearable = collectible.GetBehavior<CollectibleBehaviorWearable>();
			AssetLocation[] sounds = ArmorFootStepSoundUtil.ResolveAvailableFootStepSounds(api, slot, collectible, wearable);
			if (sounds.Length == 0)
			{
				return new ValueTuple<int, int, int>(0, 0, -1);
			}
			InventoryBase inventory = slot.Inventory;
			int slotId = (inventory != null) ? inventory.GetSlotId(slot) : -1;
			return new ValueTuple<int, int, int>(ArmorFootStepSoundUtil.GetSoundPriority(collectible, sounds), ArmorFootStepSoundUtil.GetZonePriority(collectible), slotId);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x000447D0 File Offset: 0x000429D0
		private static AssetLocation[] ResolveAvailableFootStepSounds([Nullable(2)] ICoreAPI api, ItemSlot slot, CollectibleObject collectible, [Nullable(2)] CollectibleBehaviorWearable wearable)
		{
			AssetLocation[] sounds = (wearable != null) ? wearable.GetFootStepSounds(slot) : null;
			if (sounds != null && sounds.Length > 0)
			{
				return sounds;
			}
			sounds = ArmorFootStepSoundUtil.ResolveConfiguredFootStepSounds(api, slot, collectible);
			if (sounds.Length != 0)
			{
				return sounds;
			}
			return ArmorFootStepSoundUtil.InferFootStepSounds(api, collectible);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0004480C File Offset: 0x00042A0C
		private static AssetLocation[] ResolveConfiguredFootStepSounds([Nullable(2)] ICoreAPI api, ItemSlot slot, CollectibleObject collectible)
		{
			ItemStack itemstack = slot.Itemstack;
			string text;
			if (itemstack == null)
			{
				text = null;
			}
			else
			{
				JsonObject itemAttributes = itemstack.ItemAttributes;
				text = ((itemAttributes != null) ? itemAttributes["footStepSound"].AsString(null) : null);
			}
			string soundCode = text;
			if (soundCode == null)
			{
				JsonObject attributes = collectible.Attributes;
				soundCode = ((attributes != null) ? attributes["footStepSound"].AsString(null) : null);
			}
			if (string.IsNullOrWhiteSpace(soundCode))
			{
				return Array.Empty<AssetLocation>();
			}
			string defaultDomain = (soundCode.Contains(':') || soundCode.StartsWith("wearable/", 5)) ? "game" : collectible.Code.Domain;
			return ArmorFootStepSoundUtil.ExpandSound(api, soundCode, defaultDomain);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000448A8 File Offset: 0x00042AA8
		private static AssetLocation[] InferFootStepSounds([Nullable(2)] ICoreAPI api, CollectibleObject collectible)
		{
			AssetLocation code2 = collectible.Code;
			string text;
			if (code2 == null)
			{
				text = null;
			}
			else
			{
				string path = code2.Path;
				text = ((path != null) ? path.ToLowerInvariant() : null);
			}
			string code = text ?? "";
			if (code.Contains("plate"))
			{
				return ArmorFootStepSoundUtil.ExpandSound(api, "wearable/plate*", "game");
			}
			if (code.Contains("scale"))
			{
				return ArmorFootStepSoundUtil.ExpandSound(api, "wearable/scale*", "game");
			}
			if (code.Contains("chain"))
			{
				return ArmorFootStepSoundUtil.ExpandSound(api, "wearable/chain*", "game");
			}
			if (code.Contains("brigandine"))
			{
				return ArmorFootStepSoundUtil.ExpandSound(api, "wearable/brigandine*", "game");
			}
			if (code.Contains("lamellar-wood"))
			{
				return ArmorFootStepSoundUtil.ExpandSound(api, "wearable/leather*", "game");
			}
			if (code.Contains("lamellar"))
			{
				return ArmorFootStepSoundUtil.ExpandSound(api, "wearable/brigandine*", "game");
			}
			if (code.Contains("leather") || code.Contains("jerkin") || code.Contains("hide") || code.Contains("sewn"))
			{
				return ArmorFootStepSoundUtil.ExpandSound(api, "wearable/leather*", "game");
			}
			return Array.Empty<AssetLocation>();
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x000449DC File Offset: 0x00042BDC
		private static AssetLocation[] ExpandSound([Nullable(2)] ICoreAPI api, string soundCode, string defaultDomain)
		{
			AssetLocation soundLocation = AssetLocation.Create(soundCode, defaultDomain).WithPathPrefixOnce("sounds/");
			if (soundCode.EndsWith('*') && api != null)
			{
				soundLocation.Path = soundLocation.Path.TrimEnd('*');
				return api.Assets.GetLocations(soundLocation.Path, soundLocation.Domain).ToArray();
			}
			return new AssetLocation[]
			{
				soundLocation
			};
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00044A44 File Offset: 0x00042C44
		private static int GetSoundPriority(CollectibleObject collectible, AssetLocation[] sounds)
		{
			AssetLocation code = collectible.Code;
			string text;
			if (code == null)
			{
				text = null;
			}
			else
			{
				string path = code.Path;
				text = ((path != null) ? path.ToLowerInvariant() : null);
			}
			string text2 = text ?? "";
			string sound2 = string.Join<string>(' ', Enumerable.Select<AssetLocation, string>(sounds, (AssetLocation sound) => sound.Path.ToLowerInvariant()));
			string value = text2 + " " + sound2;
			if (value.Contains("plate"))
			{
				return 60;
			}
			if (value.Contains("scale"))
			{
				return 50;
			}
			if (value.Contains("chain"))
			{
				return 45;
			}
			if (value.Contains("brigandine"))
			{
				return 40;
			}
			if (value.Contains("lamellar"))
			{
				return 30;
			}
			if (value.Contains("leather") || value.Contains("jerkin") || value.Contains("hide") || value.Contains("sewn"))
			{
				return 20;
			}
			return 10;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00044B3C File Offset: 0x00042D3C
		private static int GetZonePriority(CollectibleObject collectible)
		{
			ArmorBehavior collectibleBehavior = collectible.GetCollectibleBehavior<ArmorBehavior>(true);
			ArmorType armorType = (collectibleBehavior != null) ? collectibleBehavior.ArmorType : ArmorType.Empty;
			if ((armorType.Slots & DamageZone.Torso) != DamageZone.None)
			{
				return 700;
			}
			if ((armorType.Slots & DamageZone.Legs) != DamageZone.None)
			{
				return 600;
			}
			if ((armorType.Slots & DamageZone.Feet) != DamageZone.None)
			{
				return 500;
			}
			if ((armorType.Slots & DamageZone.Arms) != DamageZone.None)
			{
				return 400;
			}
			if ((armorType.Slots & DamageZone.Hands) != DamageZone.None)
			{
				return 300;
			}
			if ((armorType.Slots & DamageZone.Head) != DamageZone.None)
			{
				return 200;
			}
			if ((armorType.Slots & DamageZone.Neck) != DamageZone.None)
			{
				return 100;
			}
			if ((armorType.Slots & DamageZone.Face) != DamageZone.None)
			{
				return 90;
			}
			return 0;
		}
	}
}
