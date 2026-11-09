using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockBunchOCandles : Block
{
	private WorldInteraction[] interactions;

	internal int QuantityCandles;

	internal Vec3f[] candleWickPositions = new Vec3f[9]
	{
		new Vec3f(3.8f, 4f, 3.8f),
		new Vec3f(7.8f, 7f, 4.8f),
		new Vec3f(12.8f, 2f, 1.8f),
		new Vec3f(4.8f, 5f, 9.8f),
		new Vec3f(7.8f, 2f, 8.8f),
		new Vec3f(12.8f, 6f, 12.8f),
		new Vec3f(11.8f, 4f, 6.8f),
		new Vec3f(1.8f, 1f, 12.8f),
		new Vec3f(6.8f, 4f, 13.8f)
	};

	private Vec3f[][] candleWickPositionsByRot = new Vec3f[4][];

	internal void initRotations()
	{
		for (int i = 0; i < 4; i++)
		{
			Matrixf m = new Matrixf();
			m.Translate(0.5f, 0.5f, 0.5f);
			m.RotateYDeg(i * 90);
			m.Translate(-0.5f, -0.5f, -0.5f);
			Vec3f[] poses = (candleWickPositionsByRot[i] = new Vec3f[candleWickPositions.Length]);
			for (int j = 0; j < poses.Length; j++)
			{
				Vec4f rotated = m.TransformVector(new Vec4f(candleWickPositions[j].X / 16f, candleWickPositions[j].Y / 16f, candleWickPositions[j].Z / 16f, 1f));
				poses[j] = new Vec3f(rotated.X, rotated.Y, rotated.Z);
			}
		}
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		initRotations();
		QuantityCandles = Variant["quantity"].ToInt();
		interactions = ObjectCacheUtil.GetOrCreate(api, "candleInteractions", delegate
		{
			Item item = api.World.GetItem("candle");
			if (item != null)
			{
				ItemStack[] itemstacks = new ItemStack[1]
				{
					new ItemStack(item)
				};
				return new WorldInteraction[2]
				{
					new WorldInteraction
					{
						ActionLangCode = "blockhelp-groundstorage-addone",
						MouseButton = EnumMouseButton.Right,
						HotKeyCode = "shift",
						Itemstacks = itemstacks
					},
					new WorldInteraction
					{
						ActionLangCode = "blockhelp-groundstorage-removeone",
						MouseButton = EnumMouseButton.Right,
						RequireFreeHand = true
					}
				};
			}
			return Array.Empty<WorldInteraction>();
		});
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (!byPlayer.Entity.ActiveHandItemSlot.Empty && !(byPlayer.Entity.ActiveHandItemSlot?.Itemstack?.Collectible is ItemCandle))
		{
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}
		string curQuantity = Variant["quantity"];
		if (curQuantity == null)
		{
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}
		int.TryParse(curQuantity, out var stage);
		Block nextblock = world.GetBlock(CodeWithVariant("quantity", (stage - 1).ToString() ?? ""));
		if (nextblock == null)
		{
			world.BlockAccessor.SetBlock(0, blockSel.Position);
			world.BlockAccessor.TriggerNeighbourBlockUpdate(blockSel.Position);
		}
		else
		{
			world.BlockAccessor.SetBlock(nextblock.BlockId, blockSel.Position);
		}
		world.PlaySoundAt(Sounds.Place, blockSel.Position, -0.4, byPlayer);
		ItemStack outstack = new ItemStack(world.GetItem("candle"));
		if (outstack != null && !byPlayer.InventoryManager.TryGiveItemstack(outstack, slotNotifyEffect: true))
		{
			world.SpawnItemEntity(outstack, blockSel.Position);
		}
		return true;
	}

	public override void OnAsyncClientParticleTick(IAsyncParticleManager manager, BlockPos pos, float windAffectednessAtPos, float secondsTicking)
	{
		if (ParticleProperties == null || ParticleProperties.Length == 0)
		{
			return;
		}
		int rnd = GameMath.MurmurHash3Mod(pos.X, pos.Y, pos.Z, 4);
		Vec3f[] poses = candleWickPositionsByRot[rnd];
		for (int i = 0; i < ParticleProperties.Length; i++)
		{
			AdvancedParticleProperties bps = ParticleProperties[i];
			bps.WindAffectednesAtPos = windAffectednessAtPos;
			for (int j = 0; j < QuantityCandles; j++)
			{
				Vec3f dp = poses[j];
				bps.basePos.X = (float)pos.X + dp.X - 1f / 64f;
				bps.basePos.Y = (float)pos.InternalY + dp.Y;
				bps.basePos.Z = (float)pos.Z + dp.Z;
				manager.Spawn(bps);
			}
		}
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return interactions.Append(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
	}
}
