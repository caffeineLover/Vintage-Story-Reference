using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200001B RID: 27
	public class BlockFirework : Block, IIgnitable
	{
		// Token: 0x06000180 RID: 384 RVA: 0x00012D14 File Offset: 0x00010F14
		public override void OnLoaded(ICoreAPI api)
		{
			if (api.Side != 2)
			{
				return;
			}
			ICoreClientAPI capi = api as ICoreClientAPI;
			InteractionStacksDelegate <>9__1;
			this.interactions = ObjectCacheUtil.GetOrCreate<WorldInteraction[]>(api, "fireworkInteractions", delegate()
			{
				List<ItemStack> canIgniteStacks = new List<ItemStack>();
				foreach (CollectibleObject obj in api.World.Collectibles)
				{
					if (obj is Block && (obj as Block).HasBehavior<BlockBehaviorCanIgnite>(false))
					{
						List<ItemStack> stacks = obj.GetHandBookStacks(capi);
						if (stacks != null)
						{
							canIgniteStacks.AddRange(stacks);
						}
					}
				}
				WorldInteraction[] array = new WorldInteraction[1];
				int num = 0;
				WorldInteraction worldInteraction = new WorldInteraction();
				worldInteraction.MouseButton = 2;
				worldInteraction.ActionLangCode = "primitivesurvival:blockhelp-firework-ignite";
				worldInteraction.Itemstacks = canIgniteStacks.ToArray();
				InteractionStacksDelegate getMatchingStacks;
				if ((getMatchingStacks = <>9__1) == null)
				{
					getMatchingStacks = (<>9__1 = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BEFirework befirework = api.World.BlockAccessor.GetBlockEntity(bs.Position) as BEFirework;
						if (befirework != null && !befirework.IsLit)
						{
							return wi.Itemstacks;
						}
						return null;
					});
				}
				worldInteraction.GetMatchingStacks = getMatchingStacks;
				array[num] = worldInteraction;
				return array;
			});
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00012D70 File Offset: 0x00010F70
		EnumIgniteState IIgnitable.OnTryIgniteStack(EntityAgent byEntity, BlockPos pos, ItemSlot slot, float secondsIgniting)
		{
			if (secondsIgniting <= 2f)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00012D80 File Offset: 0x00010F80
		public EnumIgniteState OnTryIgniteBlock(EntityAgent byEntity, BlockPos pos, float secondsIgniting)
		{
			BEFirework befirework = byEntity.World.BlockAccessor.GetBlockEntity(pos) as BEFirework;
			if (befirework == null || befirework.IsLit)
			{
				return 1;
			}
			if (secondsIgniting > 0.75f)
			{
				return 3;
			}
			return 2;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00012DBC File Offset: 0x00010FBC
		public void OnTryIgniteBlockOver(EntityAgent byEntity, BlockPos pos, float secondsIgniting, ref EnumHandling handling)
		{
			if (secondsIgniting < 0.7f)
			{
				return;
			}
			handling = 2;
			IPlayer byPlayer = null;
			EntityPlayer player = byEntity as EntityPlayer;
			if (player != null)
			{
				byPlayer = byEntity.World.PlayerByUid(player.PlayerUID);
			}
			if (byPlayer == null)
			{
				return;
			}
			BEFirework befirework = byPlayer.Entity.World.BlockAccessor.GetBlockEntity(pos) as BEFirework;
			if (befirework == null)
			{
				return;
			}
			befirework.OnIgnite(byPlayer);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00012E1E File Offset: 0x0001101E
		public override void OnBlockExploded(IWorldAccessor world, BlockPos pos, BlockPos explosionCenter, EnumBlastType blastType, string ignitedByPlayerUid)
		{
			BEFirework befirework = world.BlockAccessor.GetBlockEntity(pos) as BEFirework;
			if (befirework == null)
			{
				return;
			}
			befirework.OnBlockExploded(pos, ignitedByPlayerUid);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00012E3E File Offset: 0x0001103E
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return ArrayExtensions.Append<WorldInteraction>(this.interactions, base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00012E54 File Offset: 0x00011054
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			BlockPos pos = blockSel.Position;
			bool inwater = world.BlockAccessor.GetBlock(pos, 0).LiquidCode == "water";
			BlockSelection blockSelBelow = blockSel.Clone();
			blockSelBelow.Position.Y--;
			if (world.BlockAccessor.GetBlock(blockSelBelow.Position, 0).Fertility <= 0 || inwater)
			{
				failureCode = Lang.Get("softer-ground-needed", Array.Empty<object>());
				return false;
			}
			return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
		}

		// Token: 0x040000C4 RID: 196
		private WorldInteraction[] interactions;
	}
}
