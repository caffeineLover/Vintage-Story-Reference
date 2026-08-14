namespace Vintagestory.API.Common;

public interface IWearable
{
	void ChangeCondition(ItemSlot slot, float changeVal);

	float GetWarmth(ItemSlot slot);
}
