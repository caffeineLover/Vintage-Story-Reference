using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class CollectibleBuffDebug : ModSystem
{
	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Server;
	}

	public override double ExecuteOrder()
	{
		return 1.0;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		CommandArgumentParsers parsers = api.ChatCommands.Parsers;
		api.ChatCommands.GetOrCreate("debug").BeginSub("cbuff").BeginSub("add")
			.WithArgs(parsers.Word("code"), parsers.Word("statcode"), parsers.Float("multiplier"), parsers.OptionalFloat("flatchange"))
			.HandleWith(cmdAddBuff)
			.EndSub()
			.BeginSub("clear")
			.HandleWith(cmdClearBuffs)
			.EndSub()
			.EndSub();
	}

	private TextCommandResult cmdAddBuff(TextCommandCallingArgs args)
	{
		ItemSlot heldItemSlot = args.Caller?.Player?.InventoryManager.ActiveHotbarSlot;
		if (heldItemSlot == null || heldItemSlot.Empty)
		{
			return TextCommandResult.Error("Hold something in your hands");
		}
		CollectibleBehaviorBuffable bh = heldItemSlot.Itemstack.Collectible.GetBehavior<CollectibleBehaviorBuffable>();
		if (bh == null)
		{
			return TextCommandResult.Error("Held item is not buffable");
		}
		bh.AddBuff(heldItemSlot.Itemstack, new AppliedCollectibleBuff
		{
			Code = (string)args[0],
			StatCode = (string)args[1],
			Multiplier = (float)args[2],
			FlatChange = (float)args[3]
		});
		heldItemSlot.MarkDirty();
		return TextCommandResult.Success("Buff added");
	}

	private TextCommandResult cmdClearBuffs(TextCommandCallingArgs args)
	{
		ItemSlot heldItemSlot = args.Caller?.Player?.InventoryManager.ActiveHotbarSlot;
		if (heldItemSlot == null || heldItemSlot.Empty)
		{
			return TextCommandResult.Error("Hold something in your hands");
		}
		CollectibleBehaviorBuffable bh = heldItemSlot.Itemstack.Collectible.GetBehavior<CollectibleBehaviorBuffable>();
		if (bh == null)
		{
			return TextCommandResult.Error("Held item is not buffable");
		}
		bh.StoreItemBuffs(heldItemSlot.Itemstack, new List<AppliedCollectibleBuff>());
		heldItemSlot.MarkDirty();
		return TextCommandResult.Success("Buffs cleared");
	}
}
