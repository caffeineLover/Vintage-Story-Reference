using System;
using System.Runtime.CompilerServices;
using RPVoiceChat;
using RPVoiceChat.Config;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

// Token: 0x02000003 RID: 3
[NullableContext(1)]
[Nullable(0)]
internal class BehaviorRingable : BlockBehavior
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000008 RID: 8 RVA: 0x000022DF File Offset: 0x000004DF
	private double BellRingCooldownSeconds
	{
		get
		{
			return ServerConfigManager.BellRingCooldownSeconds;
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000022E6 File Offset: 0x000004E6
	public BehaviorRingable(Block block) : base(block)
	{
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000022F0 File Offset: 0x000004F0
	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		if (byPlayer == null)
		{
			return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position);
		if (blockEntity == null)
		{
			return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
		}
		BEBehaviorRingable ringable = blockEntity.GetBehavior<BEBehaviorRingable>();
		if (ringable == null)
		{
			return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
		}
		ItemStack itemstack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;
		Item item = (itemstack != null) ? itemstack.Item : null;
		if (item != null && string.IsNullOrWhiteSpace(ringable.BellPartCode))
		{
			if (item.Code.Path.StartsWith("smallbellparts"))
			{
				ringable.BellPartCode = item.Code.Path;
				if (byPlayer.WorldData.CurrentGameMode != 2)
				{
					byPlayer.InventoryManager.ActiveHotbarSlot.TakeOut(1);
					byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
				}
				BlockEntity blockentity = ringable.Blockentity;
				if (blockentity != null)
				{
					blockentity.MarkDirty(true, null);
				}
				return true;
			}
		}
		else if (!string.IsNullOrWhiteSpace(ringable.BellPartCode) && (ringable.LastRung == null || ringable.LastRung < DateTime.Now.AddSeconds(-this.BellRingCooldownSeconds)))
		{
			ringable.LastRung = new DateTime?(DateTime.Now);
			int rand = new Random().Next(1, 3);
			float volume = (world.Side == 2 && ModConfig.ClientConfig.OutputBlock != 0f) ? ModConfig.ClientConfig.OutputBlock : 0.6f;
			string modID = RPVoiceChatMod.modID;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(35, 1);
			defaultInterpolatedStringHandler.AppendLiteral("sounds/block/callbell/callbell_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(rand);
			defaultInterpolatedStringHandler.AppendLiteral(".ogg");
			world.PlaySoundAt(new AssetLocation(modID, defaultInterpolatedStringHandler.ToStringAndClear()), (double)blockSel.Position.X + 0.5, (double)blockSel.Position.Y + 0.5, (double)blockSel.Position.Z + 0.5, byPlayer, false, 16f, volume);
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002520 File Offset: 0x00000720
	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier, ref EnumHandling handling)
	{
		if (byPlayer == null)
		{
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier, ref handling);
			return;
		}
		if (byPlayer.WorldData.CurrentGameMode != 2)
		{
			BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
			if (blockEntity == null)
			{
				base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier, ref handling);
				return;
			}
			BEBehaviorRingable ringable = blockEntity.GetBehavior<BEBehaviorRingable>();
			if (ringable == null || string.IsNullOrWhiteSpace(ringable.BellPartCode))
			{
				base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier, ref handling);
				return;
			}
			Item item = world.GetItem(new AssetLocation(RPVoiceChatMod.modID, ringable.BellPartCode));
			if (item == null)
			{
				base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier, ref handling);
				return;
			}
			ItemStack stack = new ItemStack(item, 1)
			{
				StackSize = 1
			};
			world.SpawnItemEntity(stack, pos.ToVec3d(), null);
		}
		base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier, ref handling);
	}
}
