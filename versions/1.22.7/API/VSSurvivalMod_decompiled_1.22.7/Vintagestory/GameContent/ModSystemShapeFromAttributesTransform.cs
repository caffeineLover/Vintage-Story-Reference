using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class ModSystemShapeFromAttributesTransform : ModSystem
{
	private ICoreClientAPI capi;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Client;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
		api.Event.RegisterEventBusListener(OnEventBusEvent);
	}

	private void OnEventBusEvent(string eventName, ref EnumHandling handling, IAttribute data)
	{
		TreeAttribute tree = data as TreeAttribute;
		if (eventName != "ongettransform" && eventName != "onsettransform")
		{
			return;
		}
		ItemSlot slot = capi.World.Player.InventoryManager.ActiveHotbarSlot;
		if (slot.Empty)
		{
			return;
		}
		string type = slot.Itemstack.Attributes.GetString("type");
		if (!(slot.Itemstack.Block is BlockShapeFromAttributes block))
		{
			return;
		}
		IShapeTypeProps cprops = block.GetTypeProps(type, slot.Itemstack, null);
		if (cprops == null)
		{
			return;
		}
		bool isSet = eventName == "onsettransform";
		switch (tree.GetString("target"))
		{
		case "Gui":
			tree.SetBool("preventDefault", value: true);
			if (isSet)
			{
				cprops.GuiTransform = ModelTransform.CreateFromTreeAttribute(tree);
			}
			else
			{
				(cprops.GuiTransform ?? ModelTransform.BlockDefaultGui()).ToTreeAttribute(tree);
			}
			break;
		case "HandTp":
			tree.SetBool("preventDefault", value: true);
			if (isSet)
			{
				cprops.TpTransform = ModelTransform.CreateFromTreeAttribute(tree);
			}
			else
			{
				(cprops.TpTransform ?? ModelTransform.BlockDefaultTp()).ToTreeAttribute(tree);
			}
			break;
		case "Ground":
			tree.SetBool("preventDefault", value: true);
			if (isSet)
			{
				cprops.GroundTransform = ModelTransform.CreateFromTreeAttribute(tree);
			}
			else
			{
				(cprops.GroundTransform ?? ModelTransform.BlockDefaultGround()).ToTreeAttribute(tree);
			}
			break;
		}
	}
}
