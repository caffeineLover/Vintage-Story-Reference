using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000017 RID: 23
	public class BlockBombFuse : Block, IIgnitable
	{
		// Token: 0x0600016B RID: 363 RVA: 0x000120C0 File Offset: 0x000102C0
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			if (api.Side != 2)
			{
				return;
			}
			ICoreClientAPI capi = api as ICoreClientAPI;
			InteractionStacksDelegate <>9__1;
			this.interactions = ObjectCacheUtil.GetOrCreate<WorldInteraction[]>(api, "bombInteractions", delegate()
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
				worldInteraction.ActionLangCode = "blockhelp-bomb-ignite";
				worldInteraction.Itemstacks = canIgniteStacks.ToArray();
				InteractionStacksDelegate getMatchingStacks;
				if ((getMatchingStacks = <>9__1) == null)
				{
					getMatchingStacks = (<>9__1 = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BEBombFuse bebomb = api.World.BlockAccessor.GetBlockEntity(bs.Position) as BEBombFuse;
						if (bebomb != null && !bebomb.IsLit)
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

		// Token: 0x0600016C RID: 364 RVA: 0x00012128 File Offset: 0x00010328
		EnumIgniteState IIgnitable.OnTryIgniteStack(EntityAgent byEntity, BlockPos pos, ItemSlot slot, float secondsIgniting)
		{
			if (secondsIgniting <= 2f)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00012138 File Offset: 0x00010338
		public EnumIgniteState OnTryIgniteBlock(EntityAgent byEntity, BlockPos pos, float secondsIgniting)
		{
			BEBombFuse bebomb = byEntity.World.BlockAccessor.GetBlockEntity(pos) as BEBombFuse;
			if (bebomb == null || bebomb.IsLit)
			{
				return 1;
			}
			if (secondsIgniting > 0.75f)
			{
				return 3;
			}
			return 2;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00012174 File Offset: 0x00010374
		public void OnTryIgniteBlockOver(EntityAgent byEntity, BlockPos pos, float secondsIgniting, ref EnumHandling handling)
		{
			if (secondsIgniting < 0.7f)
			{
				return;
			}
			handling = 2;
			IPlayer byPlayer = null;
			if (byEntity is EntityPlayer)
			{
				byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
			}
			if (byPlayer == null)
			{
				return;
			}
			BEBombFuse bebombFuse = byPlayer.Entity.World.BlockAccessor.GetBlockEntity(pos) as BEBombFuse;
			if (bebombFuse == null)
			{
				return;
			}
			bebombFuse.OnIgnite(byPlayer);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000121D9 File Offset: 0x000103D9
		public override void OnBlockExploded(IWorldAccessor world, BlockPos pos, BlockPos explosionCenter, EnumBlastType blastType, string ignitedByPlayerUid)
		{
			BEBombFuse bebombFuse = world.BlockAccessor.GetBlockEntity(pos) as BEBombFuse;
			if (bebombFuse == null)
			{
				return;
			}
			bebombFuse.OnBlockExploded(pos, ignitedByPlayerUid);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000121F9 File Offset: 0x000103F9
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return ArrayExtensions.Append<WorldInteraction>(this.interactions, base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
		}

		// Token: 0x040000C1 RID: 193
		private WorldInteraction[] interactions;
	}
}
