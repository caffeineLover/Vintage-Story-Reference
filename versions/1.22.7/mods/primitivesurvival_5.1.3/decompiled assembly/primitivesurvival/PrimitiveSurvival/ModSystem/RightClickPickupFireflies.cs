using System;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000003 RID: 3
	public class RightClickPickupFireflies : BlockBehavior
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000021E1 File Offset: 0x000003E1
		public RightClickPickupFireflies(Block block) : base(block)
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021EC File Offset: 0x000003EC
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

		// Token: 0x06000008 RID: 8 RVA: 0x00002280 File Offset: 0x00000480
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
					Block thisBlock = world.BlockAccessor.GetBlock(blockSel.Position, 0);
					string newBlock = "primitivesurvival:" + thisBlock.Code.Path.Replace("angled", "straight");
					Block dropBlock = world.GetBlock(new AssetLocation(newBlock));
					if (dropBlock == null)
					{
						return false;
					}
					ItemStack newStack = new ItemStack(dropBlock, 1);
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

		// Token: 0x06000009 RID: 9 RVA: 0x000023C8 File Offset: 0x000005C8
		public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos, ref EnumHandling handling)
		{
			return base.OnPickBlock(world, pos, ref handling);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023D3 File Offset: 0x000005D3
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

		// Token: 0x04000005 RID: 5
		[DocumentAsJson("Optional", "None", false)]
		private SoundAttributes pickupSound;
	}
}
