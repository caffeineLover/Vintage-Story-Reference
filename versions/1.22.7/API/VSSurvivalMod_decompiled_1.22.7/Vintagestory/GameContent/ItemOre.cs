using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemOre : ItemPileable, IContainedInteractable
{
	public bool IsCoal
	{
		get
		{
			if (!(Variant["ore"] == "lignite") && !(Variant["ore"] == "bituminouscoal"))
			{
				return Variant["ore"] == "anthracite";
			}
			return true;
		}
	}

	public override bool IsPileable => IsCoal;

	protected override AssetLocation PileBlockCode => new AssetLocation("coalpile");

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		CombustibleProperties combustibleProps = inSlot.Itemstack.Collectible.GetCombustibleProperties(world, inSlot.Itemstack, null);
		if (combustibleProps?.SmeltedStack?.ResolvedItemstack == null)
		{
			JsonObject attributes = Attributes;
			if (attributes != null && attributes["metalUnits"].Exists)
			{
				float units = Attributes["metalUnits"].AsInt();
				string orename = LastCodePart(1);
				if (orename.Contains("_"))
				{
					orename = orename.Split('_')[1];
				}
				AssetLocation loc = new AssetLocation("nugget-" + orename);
				Item item = api.World.GetItem(loc);
				if (item != null)
				{
					ItemStack nuggetStack = new ItemStack(item);
					CombustibleProperties nuggetCombustibleProps = nuggetStack.Collectible.GetCombustibleProperties(world, nuggetStack, null);
					if (nuggetCombustibleProps?.SmeltedStack?.ResolvedItemstack != null)
					{
						string smelttype = nuggetCombustibleProps.SmeltingType.ToString().ToLowerInvariant();
						string metal = nuggetCombustibleProps.SmeltedStack.ResolvedItemstack.Collectible?.Variant?["metal"];
						string metalname = Lang.Get("material-" + metal);
						if (metal == null)
						{
							metalname = nuggetCombustibleProps.SmeltedStack.ResolvedItemstack.GetName();
						}
						dsc.AppendLine(Lang.Get("game:smeltdesc-" + smelttype + "ore-plural", units.ToString("0.#"), metalname));
					}
				}
				dsc.AppendLine(Lang.Get("Parent Material: {0}", Lang.Get("rock-" + LastCodePart())));
				dsc.AppendLine();
				dsc.AppendLine(Lang.Get("Crush with hammer to extract nuggets"));
			}
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			return;
		}
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		if (combustibleProps.SmeltedStack.ResolvedItemstack.Collectible.FirstCodePart() == "ingot")
		{
			string smelttype2 = combustibleProps.SmeltingType.ToString().ToLowerInvariant();
			int instacksize = combustibleProps.SmeltedRatio;
			float units2 = (float)combustibleProps.SmeltedStack.ResolvedItemstack.StackSize * 100f / (float)instacksize;
			string metal2 = combustibleProps.SmeltedStack.ResolvedItemstack.Collectible?.Variant?["metal"];
			string metalname2 = Lang.Get("material-" + metal2);
			if (metal2 == null)
			{
				metalname2 = combustibleProps.SmeltedStack.ResolvedItemstack.GetName();
			}
			string str = Lang.Get("game:smeltdesc-" + smelttype2 + "ore-plural", units2.ToString("0.#"), metalname2);
			dsc.AppendLine(str);
		}
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		JsonObject attributes = Attributes;
		if (attributes != null && attributes["metalUnits"].Exists)
		{
			string orename = LastCodePart(1);
			LastCodePart();
			if (FirstCodePart() == "crystalizedore")
			{
				return Lang.Get(LastCodePart(2) + "-crystallizedore-chunk", Lang.Get("ore-" + orename));
			}
			return Lang.Get(LastCodePart(2) + "-ore-chunk", Lang.Get("ore-" + orename));
		}
		return base.GetHeldItemName(itemStack);
	}

	private bool CanBreakOnMaterial(EnumBlockMaterial blockMaterial)
	{
		if ((uint)(blockMaterial - 6) <= 1u || (uint)(blockMaterial - 11) <= 1u || blockMaterial == EnumBlockMaterial.Brick)
		{
			return true;
		}
		return false;
	}

	public bool OnContainedInteractStart(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		JsonObject attributes = Attributes;
		if (attributes == null || !attributes["metalUnits"].Exists || byPlayer.InventoryManager.ActiveTool != EnumTool.Hammer || !byPlayer.Entity.Controls.ShiftKey || !be.Api.World.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use))
		{
			return false;
		}
		if (!CanBreakOnMaterial(be.Api.World.BlockAccessor.GetBlock(be.Pos.DownCopy()).BlockMaterial))
		{
			(be.Api as ICoreClientAPI)?.TriggerIngameError(this, "needssolidsurface", Lang.Get("itemore-needssolid-error"));
			return false;
		}
		return true;
	}

	public bool OnContainedInteractStep(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		JsonObject attributes = Attributes;
		if (attributes == null || !attributes["metalUnits"].Exists || !CanBreakOnMaterial(be.Api.World.BlockAccessor.GetBlock(be.Pos.DownCopy()).BlockMaterial) || byPlayer.InventoryManager.ActiveTool != EnumTool.Hammer || !byPlayer.Entity.Controls.ShiftKey || blockSel == null)
		{
			return false;
		}
		if (byPlayer is IClientPlayer)
		{
			byPlayer.Entity.StartAnimation("hammerhit-fp");
		}
		if (be.Api.World.Rand.NextDouble() < 0.1)
		{
			be.Api.World.PlaySoundAt("sounds/block/rock-hit-pickaxe", blockSel.Position, 0.0);
		}
		if (be.Api.World.Side == EnumAppSide.Client && be.Api.World.Rand.NextDouble() < 0.25)
		{
			be.Api.World.SpawnCubeParticles(blockSel.Position.ToVec3d().Add(blockSel.HitPosition), slot.Itemstack, 0.25f, 1, 0.5f, byPlayer, new Vec3f(0f, 1f, 0f));
		}
		return secondsUsed < 2f;
	}

	public void OnContainedInteractStop(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		JsonObject attributes = Attributes;
		if (attributes == null || !attributes["metalUnits"].Exists || !CanBreakOnMaterial(be.Api.World.BlockAccessor.GetBlock(be.Pos.DownCopy()).BlockMaterial) || byPlayer.InventoryManager.ActiveTool != EnumTool.Hammer || !(secondsUsed > 1.95f) || be.Api.World.Side != EnumAppSide.Server)
		{
			return;
		}
		byPlayer.Entity.StopAnimation("hammerhit-fp");
		ItemSlot toolSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		int units = slot.Itemstack.ItemAttributes["metalUnits"].AsInt(5);
		string type = slot.Itemstack.Collectible.Variant["ore"].Replace("quartz_", "").Replace("galena_", "");
		ItemStack outStack = new ItemStack(api.World.GetItem("nugget-" + type), Math.Max(1, units / 5));
		int smashed;
		for (smashed = 0; smashed < slot.StackSize; smashed++)
		{
			if (toolSlot.Empty)
			{
				break;
			}
			if (smashed >= 4)
			{
				break;
			}
			for (int k = 0; k < outStack.StackSize; k++)
			{
				ItemStack stack = outStack.Clone();
				stack.StackSize = 1;
				be.Api.World.SpawnItemEntity(stack, blockSel.Position);
			}
			toolSlot.Itemstack.Collectible.DamageItem(be.Api.World, byPlayer.Entity, toolSlot);
		}
		if (slot.StackSize <= smashed)
		{
			slot.Itemstack = null;
		}
		else
		{
			slot.Itemstack.StackSize -= smashed;
		}
		be.MarkDirty(redrawOnClient: true);
		if (be.Inventory.Empty)
		{
			be.Api.World.BlockAccessor.SetBlock(0, blockSel.Position);
		}
		be.Api.World.PlaySoundAt("sounds/block/rock-break-pickaxe", blockSel.Position, 0.0);
	}

	public bool OnContainedInteractCancel(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel, EnumItemUseCancelReason cancelReason)
	{
		byPlayer.Entity.StopAnimation("hammerhit-fp");
		return true;
	}

	public WorldInteraction[] GetContainedInteractionHelp(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		JsonObject attributes = Attributes;
		if (attributes != null && attributes["metalUnits"].Exists && CanBreakOnMaterial(be.Api.World.BlockAccessor.GetBlock(be.Pos.DownCopy()).BlockMaterial))
		{
			bool notProtected = true;
			if (be.Api.World.Claims != null && be.Api.World is IClientWorldAccessor clientWorld)
			{
				IClientPlayer player = clientWorld.Player;
				if (player != null && player.WorldData.CurrentGameMode == EnumGameMode.Survival && clientWorld.Claims.TestAccess(clientWorld.Player, blockSel.Position, EnumBlockAccessFlags.Use) != EnumWorldAccessResponse.Granted)
				{
					notProtected = false;
				}
			}
			if (notProtected)
			{
				return new WorldInteraction[1]
				{
					new WorldInteraction
					{
						ActionLangCode = "itemhelp-ore-smash",
						MouseButton = EnumMouseButton.Right,
						HotKeyCode = "shift",
						Itemstacks = ObjectCacheUtil.GetToolStacks(api, EnumTool.Hammer)
					}
				};
			}
		}
		return Array.Empty<WorldInteraction>();
	}
}
