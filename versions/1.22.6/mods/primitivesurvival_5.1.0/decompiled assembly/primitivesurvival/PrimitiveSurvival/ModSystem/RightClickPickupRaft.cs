using System;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000005 RID: 5
	public class RightClickPickupRaft : BlockBehavior
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002679 File Offset: 0x00000879
		public RightClickPickupRaft(Block block) : base(block)
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002684 File Offset: 0x00000884
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			JsonObject jsonSound = properties["sound"];
			if (!jsonSound.Exists)
			{
				JsonObject attributes = this.block.Attributes;
				jsonSound = ((attributes != null) ? attributes["placeSound"] : null);
			}
			this.pickupSound = (((jsonSound != null) ? jsonSound.AsObject<SoundAttributes?>(default(SoundAttributes?), this.block.Code.Domain, true) : default(SoundAttributes?)) ?? new SoundAttributes());
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002718 File Offset: 0x00000918
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
		{
			(new ItemStack[1])[0] = this.block.OnPickBlock(world, blockSel.Position);
			if (!world.Claims.TryAccess(byPlayer, blockSel.Position, 1))
			{
				return false;
			}
			if (!byPlayer.Entity.Controls.Sneak && byPlayer.InventoryManager.ActiveHotbarSlot.Empty)
			{
				if (world.Side == 1)
				{
					string curType = world.BlockAccessor.GetBlock(blockSel.Position, 0).Code.FirstCodePart();
					Block raftBlock = world.GetBlock(new AssetLocation("primitivesurvival:" + curType + "-north"));
					if (raftBlock == null)
					{
						return false;
					}
					ItemStack newStack = new ItemStack(raftBlock, 1);
					if (byPlayer.InventoryManager.TryGiveItemstack(newStack, true))
					{
						world.BlockAccessor.SetBlock(0, blockSel.Position);
						world.BlockAccessor.TriggerNeighbourBlockUpdate(blockSel.Position);
						world.PlaySoundAt((this.pickupSound.Location != null) ? this.pickupSound : this.block.GetSounds(world.BlockAccessor, blockSel, null).Place, byPlayer, null, 1f);
						handling = 2;
						return true;
					}
				}
				handling = 2;
				return true;
			}
			return false;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002854 File Offset: 0x00000A54
		public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos, ref EnumHandling handling)
		{
			return base.OnPickBlock(world, pos, ref handling);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000285F File Offset: 0x00000A5F
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

		// Token: 0x04000008 RID: 8
		[DocumentAsJson("Optional", "None", false)]
		private SoundAttributes pickupSound;
	}
}
