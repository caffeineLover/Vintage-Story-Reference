using System;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000D5 RID: 213
	public class LucerneBlock : Block
	{
		// Token: 0x0600065C RID: 1628 RVA: 0x00022740 File Offset: 0x00020940
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

		// Token: 0x0600065D RID: 1629 RVA: 0x000227E8 File Offset: 0x000209E8
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

		// Token: 0x0600065E RID: 1630 RVA: 0x00022908 File Offset: 0x00020B08
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
