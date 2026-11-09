using System;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000004 RID: 4
	public class RightClickPickupFloatingDock : BlockBehavior
	{
		// Token: 0x0600000B RID: 11 RVA: 0x0000240C File Offset: 0x0000060C
		public RightClickPickupFloatingDock(Block block) : base(block)
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002418 File Offset: 0x00000618
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this.delayTime = 1f;
			JsonObject jsonSound = properties["sound"];
			if (!jsonSound.Exists)
			{
				JsonObject attributes = this.block.Attributes;
				jsonSound = ((attributes != null) ? attributes["placeSound"] : null);
			}
			this.pickupSound = (((jsonSound != null) ? jsonSound.AsObject<SoundAttributes?>(default(SoundAttributes?), this.block.Code.Domain, true) : default(SoundAttributes?)) ?? new SoundAttributes());
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024B4 File Offset: 0x000006B4
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
		{
			(new ItemStack[1])[0] = this.block.OnPickBlock(world, blockSel.Position);
			if (!world.Claims.TryAccess(byPlayer, blockSel.Position, 1))
			{
				return false;
			}
			if (byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack != null)
			{
				return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
			}
			handling = 2;
			return true;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002517 File Offset: 0x00000717
		public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
		{
			handling = 2;
			return secondsUsed < this.delayTime;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002528 File Offset: 0x00000728
		public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
		{
			if (secondsUsed < this.delayTime - 0.7f)
			{
				return;
			}
			if (!byPlayer.Entity.Controls.Sneak)
			{
				if (world.Side == 1)
				{
					string curType = world.BlockAccessor.GetBlock(blockSel.Position, 0).FirstCodePart(1);
					Block tempBlock = world.GetBlock(new AssetLocation("primitivesurvival:floatingdock-" + curType + "-ns"));
					if (tempBlock == null)
					{
						return;
					}
					ItemStack newStack = new ItemStack(tempBlock, 1);
					if (byPlayer.InventoryManager.TryGiveItemstack(newStack, true))
					{
						world.BlockAccessor.SetBlock(0, blockSel.Position);
						world.BlockAccessor.TriggerNeighbourBlockUpdate(blockSel.Position);
						world.PlaySoundAt((this.pickupSound.Location != null) ? this.pickupSound : this.block.GetSounds(world.BlockAccessor, blockSel, null).Place, byPlayer, null, 1f);
						handling = 2;
						return;
					}
				}
				handling = 2;
				return;
			}
			base.OnBlockInteractStop(secondsUsed, world, byPlayer, blockSel, ref handling);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002635 File Offset: 0x00000835
		public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos, ref EnumHandling handling)
		{
			return base.OnPickBlock(world, pos, ref handling);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002640 File Offset: 0x00000840
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handled)
		{
			return ArrayExtensions.Append<WorldInteraction>(new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-behavior-rightclickpickup",
					MouseButton = 2,
					RequireFreeHand = true
				}
			}, base.GetPlacedBlockInteractionHelp(world, selection, forPlayer, ref handled));
		}

		// Token: 0x04000006 RID: 6
		public float delayTime;

		// Token: 0x04000007 RID: 7
		[DocumentAsJson("Optional", "None", false)]
		private SoundAttributes pickupSound;
	}
}
