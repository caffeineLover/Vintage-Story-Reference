using System;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000A9 RID: 169
	public class LucerneBlock : Block
	{
		// Token: 0x0600051E RID: 1310 RVA: 0x0001B878 File Offset: 0x00019A78
		public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode)
		{
			if (!base.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
			{
				return false;
			}
			RelaxedReadOnlyDictionary<string, string> variant = this.Variant;
			if (((variant != null) ? variant["materialtype"] : null) != "fired")
			{
				return true;
			}
			BlockPos below = ((world.BlockAccessor.GetBlock(blockSel.Position).Replaceable >= 6000) ? blockSel.Position : blockSel.Position.AddCopy(blockSel.Face)).DownCopy(1);
			if (!WarningBeaconStructure.IsValidConstructionBlock(world.BlockAccessor.GetBlock(below)))
			{
				failureCode = RPVoiceChatMod.modID + ":Lucerne.NeedStoneGround";
				return false;
			}
			return true;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001B920 File Offset: 0x00019B20
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			RelaxedReadOnlyDictionary<string, string> variant = this.Variant;
			if (((variant != null) ? variant["materialtype"] : null) != "fired")
			{
				return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
			}
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = RPVoiceChatMod.modID + ":WarningBeacon.Interaction.LoadFuel",
					MouseButton = 2,
					ShouldApply = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BlockEntityLucerne be = world.BlockAccessor.GetBlockEntity(bs.Position) as BlockEntityLucerne;
						return be != null && be.StructureComplete;
					}
				},
				new WorldInteraction
				{
					ActionLangCode = RPVoiceChatMod.modID + ":WarningBeacon.Interaction.ShowGuide",
					HotKeyCode = "sneak",
					MouseButton = 2,
					ShouldApply = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BlockEntityLucerne be = world.BlockAccessor.GetBlockEntity(bs.Position) as BlockEntityLucerne;
						return be != null && !be.StructureComplete && !be.ShowStructureGuide;
					}
				},
				new WorldInteraction
				{
					ActionLangCode = RPVoiceChatMod.modID + ":WarningBeacon.Interaction.HideGuide",
					HotKeyCodes = new string[]
					{
						"ctrl",
						"shift"
					},
					MouseButton = 2,
					ShouldApply = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BlockEntityLucerne be = world.BlockAccessor.GetBlockEntity(bs.Position) as BlockEntityLucerne;
						return be != null && !be.StructureComplete && be.ShowStructureGuide;
					}
				}
			};
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001BA40 File Offset: 0x00019C40
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			RelaxedReadOnlyDictionary<string, string> variant = this.Variant;
			if (((variant != null) ? variant["materialtype"] : null) != "fired")
			{
				return base.OnBlockInteractStart(world, byPlayer, blockSel);
			}
			BlockEntityLucerne be = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityLucerne;
			if (be == null)
			{
				return false;
			}
			EntityControls ctrls = byPlayer.Entity.Controls;
			if (be.ShowStructureGuide && ctrls.CtrlKey && ctrls.ShiftKey)
			{
				be.ToggleStructureGuide(byPlayer);
				return true;
			}
			if (!be.ShowStructureGuide && ctrls.Sneak)
			{
				be.ToggleStructureGuide(byPlayer);
				return true;
			}
			return (be.ShowStructureGuide && ctrls.Sneak) || !be.StructureComplete || be.OnPlayerRightClick(byPlayer, blockSel);
		}
	}
}
