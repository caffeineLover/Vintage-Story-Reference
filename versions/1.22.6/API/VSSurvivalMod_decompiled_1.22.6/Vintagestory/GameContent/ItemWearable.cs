using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[Obsolete("Use CollectibleBehaviorWearable instead")]
public class ItemWearable : ItemWearableAttachment
{
	[Obsolete("Use collectible.GetCollectibleInterface<IWearableStatsSupplier>().GetStatModifiers instead")]
	public StatModifiers StatModifers;

	[Obsolete("Use collectible.GetCollectibleInterface<IWearableStatsSupplier>().GetProtectionModifiers instead")]
	public ProtectionModifiers ProtectionModifiers;

	[Obsolete("Use collectible.GetCollectibleInterface<IWearableStatsSupplier>().GetFootStepSounds instead")]
	public AssetLocation[] FootStepSounds;

	[Obsolete("Use collectible.GetCollectibleInterface<IWearableStatsSupplier>().GetDressType instead")]
	public virtual EnumCharacterDressType DressType { get; private set; }

	[Obsolete("Use collectible.GetCollectibleInterface<IWearableStatsSupplier>().IsArmorType instead")]
	public virtual bool IsArmor
	{
		get
		{
			if (DressType != EnumCharacterDressType.ArmorBody && DressType != EnumCharacterDressType.ArmorHead)
			{
				return DressType == EnumCharacterDressType.ArmorLegs;
			}
			return true;
		}
	}

	public override void OnLoaded(ICoreAPI api)
	{
		if (!CollectibleBehaviors.Any((CollectibleBehavior x) => x is CollectibleBehaviorWearable))
		{
			CollectibleBehaviors = CollectibleBehaviors.Append(new CollectibleBehaviorWearable(this));
		}
		base.OnLoaded(api);
		api.World.Logger.Warning("'ItemWearable' class is obsolete. Please, replace it with 'Wearable' behavior for {0} instead", Code);
		Enum.TryParse<EnumCharacterDressType>(Attributes["clothescategory"].AsString(), ignoreCase: true, out var dt);
		DressType = dt;
		JsonObject jsonObj = Attributes?["footStepSound"];
		if (jsonObj != null && jsonObj.Exists)
		{
			string soundloc = jsonObj.AsString();
			if (soundloc != null)
			{
				AssetLocation loc = AssetLocation.Create(soundloc, Code.Domain).WithPathPrefixOnce("sounds/");
				if (soundloc.EndsWith('*'))
				{
					loc.Path = loc.Path.TrimEnd('*');
					FootStepSounds = api.Assets.GetLocations(loc.Path, loc.Domain).ToArray();
				}
				else
				{
					FootStepSounds = new AssetLocation[1] { loc };
				}
			}
		}
		jsonObj = Attributes?["statModifiers"];
		if (jsonObj != null && jsonObj.Exists)
		{
			try
			{
				StatModifers = jsonObj.AsObject<StatModifiers>();
			}
			catch (Exception e)
			{
				api.World.Logger.Error("Failed loading statModifiers for item/block {0}. Will ignore.", Code);
				api.World.Logger.Error(e);
				StatModifers = null;
			}
		}
		ProtectionModifiers defMods = null;
		jsonObj = Attributes?["defaultProtLoss"];
		if (jsonObj != null && jsonObj.Exists)
		{
			try
			{
				defMods = jsonObj.AsObject<ProtectionModifiers>();
			}
			catch (Exception e2)
			{
				api.World.Logger.Error("Failed loading defaultProtLoss for item/block {0}. Will ignore.", Code);
				api.World.Logger.Error(e2);
			}
		}
		jsonObj = Attributes?["protectionModifiers"];
		if (jsonObj != null && jsonObj.Exists)
		{
			try
			{
				ProtectionModifiers = jsonObj.AsObject<ProtectionModifiers>();
			}
			catch (Exception e3)
			{
				api.World.Logger.Error("Failed loading protectionModifiers for item/block {0}. Will ignore.", Code);
				api.World.Logger.Error(e3);
				ProtectionModifiers = null;
			}
		}
		if (ProtectionModifiers != null && ProtectionModifiers.PerTierFlatDamageReductionLoss == null)
		{
			ProtectionModifiers.PerTierFlatDamageReductionLoss = defMods?.PerTierFlatDamageReductionLoss;
		}
		if (ProtectionModifiers != null && ProtectionModifiers.PerTierRelativeProtectionLoss == null)
		{
			ProtectionModifiers.PerTierRelativeProtectionLoss = defMods?.PerTierRelativeProtectionLoss;
		}
	}
}
