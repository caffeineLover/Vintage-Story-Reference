using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class ItemSnowshovel : Item
{
	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		if (handling == EnumHandHandling.NotHandled && blockSel != null)
		{
			byEntity.Stats.Set("walkspeed", "snowshovelingmod", -0.4f, persistent: true);
			if (byEntity is EntityPlayer byPlayer)
			{
				byPlayer.walkSpeed = byEntity.Stats.GetBlended("walkspeed");
				api.ModLoader.GetModSystem<ModSystemSnowShoveling>().BeginSnowShoveling(byPlayer.PlayerUID, blockSel.FullPosition);
			}
			handling = EnumHandHandling.PreventDefault;
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		return true;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		byEntity.Stats.Remove("walkspeed", "snowshovelingmod");
		if (byEntity is EntityPlayer byPlayer)
		{
			byPlayer.walkSpeed = byEntity.Stats.GetBlended("walkspeed");
			api.ModLoader.GetModSystem<ModSystemSnowShoveling>().StopSnowShoveling(byPlayer.PlayerUID);
		}
	}

	public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
	{
		byEntity.Stats.Remove("walkspeed", "snowshovelingmod");
		if (byEntity is EntityPlayer byPlayer)
		{
			byPlayer.walkSpeed = byEntity.Stats.GetBlended("walkspeed");
			api.ModLoader.GetModSystem<ModSystemSnowShoveling>().StopSnowShoveling(byPlayer.PlayerUID);
		}
		return true;
	}
}
