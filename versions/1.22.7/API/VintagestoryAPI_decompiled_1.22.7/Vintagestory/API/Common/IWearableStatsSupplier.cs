namespace Vintagestory.API.Common;

public interface IWearableStatsSupplier
{
	bool IsArmorType(ItemSlot slot);

	EnumCharacterDressType GetDressType(ItemSlot slot);

	StatModifiers GetStatModifiers(ItemSlot slot);

	ProtectionModifiers GetProtectionModifiers(ItemSlot slot);

	AssetLocation[] GetFootStepSounds(ItemSlot slot);

	float GetMaxWarmth(ItemSlot slot);
}
