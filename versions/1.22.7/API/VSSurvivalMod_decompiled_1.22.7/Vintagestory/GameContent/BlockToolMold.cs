using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockToolMold : Block
{
	private WorldInteraction[] interactions;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		if (api.Side != EnumAppSide.Client)
		{
			return;
		}
		ICoreClientAPI capi = api as ICoreClientAPI;
		string createdByText = (createdByText = Attributes["createdByText"]?.AsString());
		JsonObject attributes = Attributes;
		if (attributes != null && attributes["drop"].Exists && createdByText != null)
		{
			JsonItemStack ojstack = Attributes["drop"].AsObject<JsonItemStack>();
			if (ojstack != null)
			{
				MetalPropertyVariant[] metals = api.ModLoader.GetModSystem<SurvivalCoreSystem>().metalsByCode.Values.ToArray();
				for (int i = 0; i < metals.Length; i++)
				{
					string metaltype = metals[i].Code.Path;
					JsonItemStack jstack = ojstack.Clone();
					jstack.Code.Path = jstack.Code.Path.Replace("{metal}", metaltype);
					CollectibleObject collObj = ((jstack.Type != EnumItemClass.Block) ? ((CollectibleObject)capi.World.GetItem(jstack.Code)) : ((CollectibleObject)capi.World.GetBlock(jstack.Code)));
					if (collObj == null)
					{
						continue;
					}
					JsonObject attributes2 = collObj.Attributes;
					if (attributes2 == null || !attributes2["handbook"].Exists)
					{
						if (collObj.Attributes == null)
						{
							collObj.Attributes = new JsonObject(JToken.Parse("{ handbook: {} }"));
						}
						else
						{
							collObj.Attributes.Token["handbook"] = JToken.Parse("{ }");
						}
					}
					collObj.Attributes["handbook"].Token["createdBy"] = JToken.FromObject(createdByText);
				}
			}
		}
		interactions = ObjectCacheUtil.GetOrCreate(api, Variant["tooltype"] + "moldBlockInteractions", delegate
		{
			ItemStack[] itemstacks = (from block in api.World.Blocks
				where block is BlockSmeltedContainer
				select new ItemStack(block)).ToArray();
			return new WorldInteraction[5]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-toolmold-pour",
					HotKeyCode = "shift",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = itemstacks,
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (!(api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityToolMold { IsFull: false, Shattered: false })) ? null : wi.Itemstacks
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-toolmold-takeworkitem",
					HotKeyCode = null,
					MouseButton = EnumMouseButton.Right,
					ShouldApply = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityToolMold { IsFull: not false, IsHardened: not false, Shattered: false } blockEntityToolMold && !blockEntityToolMold.BreaksWhenFilled
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-toolmold-breakmoldforitem",
					HotKeyCode = null,
					MouseButton = EnumMouseButton.Left,
					ShouldApply = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityToolMold { IsFull: not false, IsHardened: not false, Shattered: false } blockEntityToolMold && blockEntityToolMold.BreaksWhenFilled
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-toolmold-chiselmoldforbits",
					HotKeyCode = null,
					MouseButton = EnumMouseButton.Left,
					Itemstacks = ObjectCacheUtil.GetToolStacks(api, EnumTool.Chisel),
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (!(api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityToolMold { FillLevel: >0, IsHardened: not false, Shattered: false })) ? null : wi.Itemstacks
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-toolmold-pickup",
					HotKeyCode = null,
					RequireFreeHand = true,
					MouseButton = EnumMouseButton.Right,
					ShouldApply = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityToolMold { MetalContent: null } blockEntityToolMold && !blockEntityToolMold.Shattered
				}
			};
		});
	}

	public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
	{
		if (world.Rand.NextDouble() < 0.05)
		{
			BlockEntityToolMold blockEntity = GetBlockEntity<BlockEntityToolMold>(pos);
			if (blockEntity != null && blockEntity.Temperature > 300f)
			{
				entity.ReceiveDamage(new DamageSource
				{
					Source = EnumDamageSource.Block,
					SourceBlock = this,
					Type = EnumDamageType.Fire,
					SourcePos = pos.ToVec3d()
				}, 0.5f);
			}
		}
		base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);
	}

	public override float GetTraversalCost(BlockPos pos, EnumAICreatureType creatureType)
	{
		if (creatureType == EnumAICreatureType.LandCreature || creatureType == EnumAICreatureType.Humanoid)
		{
			BlockEntityToolMold blockEntity = GetBlockEntity<BlockEntityToolMold>(pos);
			if (blockEntity != null && blockEntity.Temperature > 300f)
			{
				return 10000f;
			}
		}
		return 0f;
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor ba, BlockPos pos)
	{
		Cuboidf[] boxesRotated = getColSelRotatedBoxes(SelectionBoxes, (ba.GetBlockEntity(pos) as BlockEntityToolMold)?.MeshAngle ?? 0f);
		if (RandomDrawOffset != 0 && boxesRotated != null && boxesRotated.Length >= 1)
		{
			float x = (float)(GameMath.oaatHash(pos.X, 0, pos.Z) % 12) / (24f + 12f * (float)RandomDrawOffset);
			float z = (float)(GameMath.oaatHash(pos.X, 1, pos.Z) % 12) / (24f + 12f * (float)RandomDrawOffset);
			return new Cuboidf[1] { boxesRotated[0].OffsetCopy(x, 0f, z) };
		}
		if (boxesRotated == null || boxesRotated.Length != 1)
		{
			return boxesRotated;
		}
		IWorldChunk chunk = ba.GetChunkAtBlockPos(pos);
		if (chunk == null)
		{
			return boxesRotated;
		}
		return chunk.AdjustSelectionBoxForDecor(ba, pos, boxesRotated);
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		return getColSelRotatedBoxes(CollisionBoxes, (blockAccessor.GetBlockEntity(pos) as BlockEntityToolMold)?.MeshAngle ?? 0f);
	}

	private Cuboidf[] getColSelRotatedBoxes(Cuboidf[] boxes, float meshAngle)
	{
		if (meshAngle == 0f)
		{
			return boxes;
		}
		Cuboidf[] boxesRotated = new Cuboidf[boxes.Length];
		for (int i = 0; i < boxesRotated.Length; i++)
		{
			boxesRotated[i] = boxes[i].RotatedCopy(0f, meshAngle * (180f / (float)Math.PI), 0f, new Vec3d(0.5, 0.5, 0.5));
		}
		return boxesRotated;
	}

	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
	{
		BlockEntityToolMold be = GetBlockEntity<BlockEntityToolMold>(pos);
		if (be != null && be.MeshAngle != 0f)
		{
			blockModelData = be.GetCurrentDecalMesh(be).Rotate(Vec3f.Half, 0f, be.MeshAngle, 0f);
			decalModelData = be.GetCurrentDecalMesh(decalTexSource).Rotate(Vec3f.Half, 0f, be.MeshAngle, 0f);
		}
		else
		{
			base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
		}
	}

	public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
	{
		if (blockSel == null)
		{
			base.OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
		}
		else if (byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position.AddCopy(blockSel.Face.Opposite)) is BlockEntityToolMold betm)
		{
			IPlayer byPlayer = byEntity.World.PlayerByUid((byEntity as EntityPlayer).PlayerUID);
			if (byPlayer != null && betm.OnPlayerInteract(byPlayer, blockSel.Face, blockSel.HitPosition))
			{
				handHandling = EnumHandHandling.PreventDefault;
			}
		}
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel?.Position) is BlockEntityToolMold betm)
		{
			return betm.OnPlayerInteract(byPlayer, blockSel.Face, blockSel.HitPosition);
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		if (!byPlayer.Entity.Controls.ShiftKey)
		{
			failureCode = "onlywhensneaking";
			return false;
		}
		if (!world.BlockAccessor.GetBlock(blockSel.Position.DownCopy()).CanAttachBlockAt(world.BlockAccessor, this, blockSel.Position.DownCopy(), BlockFacing.UP))
		{
			failureCode = "requiresolidground";
			return false;
		}
		return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
	{
		bool num = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
		if (num && world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityToolMold betm)
		{
			BlockPos targetPos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
			double y = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
			double dz = byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
			float roundRad = (float)(int)Math.Round((float)Math.Atan2(y, dz) / ((float)Math.PI / 2f)) * ((float)Math.PI / 2f);
			betm.MeshAngle = roundRad;
			betm.MarkDirty();
		}
		return num;
	}

	public override float OnGettingBroken(IPlayer player, BlockSelection blockSel, ItemSlot itemslot, float remainingResistance, float dt, int counter)
	{
		BlockEntityToolMold blockEntity = GetBlockEntity<BlockEntityToolMold>(blockSel.Position);
		IPlayerInventoryManager invMan = player?.InventoryManager;
		int beingChiseled;
		if (invMan != null)
		{
			EnumTool? offhandTool = invMan.OffhandTool;
			if (offhandTool.HasValue && offhandTool == EnumTool.Hammer)
			{
				offhandTool = invMan.ActiveTool;
				beingChiseled = ((offhandTool.HasValue && offhandTool == EnumTool.Chisel) ? 1 : 0);
				goto IL_0057;
			}
		}
		beingChiseled = 0;
		goto IL_0057;
		IL_0057:
		blockEntity.BeingChiseled = (byte)beingChiseled != 0;
		return base.OnGettingBroken(player, blockSel, itemslot, remainingResistance, dt, counter);
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityToolMold { FillLevel: >0 } betm && (byPlayer == null || byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative))
		{
			IPlayerInventoryManager invMan = byPlayer?.InventoryManager;
			if (invMan != null)
			{
				EnumTool? offhandTool = invMan.OffhandTool;
				if (offhandTool.HasValue && offhandTool == EnumTool.Hammer)
				{
					offhandTool = invMan.ActiveTool;
					if (offhandTool.HasValue && offhandTool == EnumTool.Chisel)
					{
						ItemStack drop = betm.GetChiseledStack();
						if (drop != null)
						{
							if (SplitDropStacks)
							{
								for (int k = 0; k < drop.StackSize; k++)
								{
									ItemStack stack = drop.Clone();
									stack.StackSize = 1;
									world.SpawnItemEntity(stack, pos);
								}
							}
							else
							{
								world.SpawnItemEntity(drop, pos);
							}
							world.PlaySoundAt(new AssetLocation("sounds/block/ingot"), pos, 0.0, byPlayer);
							betm.MetalContent = null;
							betm.FillLevel = 0;
							DamageItem(world, byPlayer.Entity, invMan.ActiveHotbarSlot);
							DamageItem(world, byPlayer.Entity, byPlayer.Entity?.LeftHandItemSlot);
							return;
						}
					}
				}
			}
			if (betm.BreaksWhenFilled)
			{
				world.PlaySoundAt(new AssetLocation("sounds/block/ceramicbreak"), pos, -0.4);
				SpawnBlockBrokenParticles(pos);
			}
		}
		base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
	}

	public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
	{
		return GetHandbookDropsFromBreakDrops(handbookStack, forPlayer);
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		List<ItemStack> stacks = new List<ItemStack>();
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityToolMold betm)
		{
			ItemStack[] mold = betm.GetStateAwareMold();
			if (mold != null)
			{
				stacks.AddRange(mold);
			}
			ItemStack[] contents = betm.GetStateAwareMoldedStacks();
			if (contents != null)
			{
				stacks.AddRange(contents);
			}
		}
		else
		{
			stacks.Add(new ItemStack(this));
		}
		return stacks.ToArray();
	}

	public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityToolMold { Shattered: not false })
		{
			return Lang.Get("ceramicblock-blockname-shattered", base.GetPlacedBlockName(world, pos));
		}
		return base.GetPlacedBlockName(world, pos);
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return interactions.Append(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
	}
}
