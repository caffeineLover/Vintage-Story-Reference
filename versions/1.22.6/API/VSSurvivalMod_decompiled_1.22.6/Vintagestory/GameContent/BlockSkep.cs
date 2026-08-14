using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockSkep : Block, ICreatureDietFoodTags
{
	private float beemobSpawnChance = 0.4f;

	public bool IsEmpty()
	{
		return Variant["type"] == "empty";
	}

	public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ItemStack? byItemStack = null)
	{
		base.OnBlockPlaced(world, blockPos, byItemStack);
		if (byItemStack != null && byItemStack.Attributes.GetBool("harvestable"))
		{
			GetBlockEntity<BlockEntityBeehive>(blockPos).Harvestable = true;
		}
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		CollectibleObject collObj = byPlayer.InventoryManager.ActiveHotbarSlot?.Itemstack?.Collectible;
		if ((collObj is ItemClosedBeenade || collObj is ItemOpenedBeenade) ? true : false)
		{
			return false;
		}
		if (byPlayer.WorldData.CurrentGameMode == EnumGameMode.Creative && collObj?.FirstCodePart() == "honeycomb")
		{
			if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityBeehive { Harvestable: false } beh)
			{
				beh.Harvestable = true;
				beh.MarkDirty(redrawOnClient: true);
			}
			return true;
		}
		if (byPlayer.InventoryManager.TryGiveItemstack(new ItemStack(world.BlockAccessor.GetBlock(CodeWithVariant("side", "east")))))
		{
			world.BlockAccessor.SetBlock(0, blockSel.Position);
			world.PlaySoundAt(new AssetLocation("sounds/block/planks"), blockSel.Position, -0.5, byPlayer, randomizePitch: false);
			return true;
		}
		return false;
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		beemobSpawnChance = Attributes?["beemobSpawnChance"].AsFloat(0.4f) ?? 0.4f;
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
		if (world.Side == EnumAppSide.Server && !IsEmpty() && world.Rand.NextDouble() < (double)beemobSpawnChance)
		{
			Entity entity = world.ClassRegistry.CreateEntity(world.GetEntityType("beemob"));
			if (entity != null)
			{
				entity.Pos.X = (float)pos.X + 0.5f;
				entity.Pos.Y = (float)pos.Y + 0.5f;
				entity.Pos.Z = (float)pos.Z + 0.5f;
				entity.Pos.Yaw = (float)world.Rand.NextDouble() * 2f * (float)Math.PI;
				entity.Attributes.SetString("origin", "brokenbeehive");
				world.SpawnEntity(entity);
			}
		}
	}

	public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
	{
		ItemStack[] stacks = (handbookStack.Attributes.GetBool("harvestable") ? getHarvestableDrops(api.World, forPlayer.Entity.Pos.XYZ.AsBlockPos, forPlayer) : GetDrops(api.World, forPlayer.Entity.Pos.XYZ.AsBlockPos, forPlayer));
		if (stacks == null)
		{
			return Array.Empty<BlockDropItemStack>();
		}
		return stacks.Select((ItemStack stack) => new BlockDropItemStack(stack)).ToArray();
	}

	public override ItemStack[]? GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		if (!IsEmpty())
		{
			BlockEntityBeehive blockEntity = GetBlockEntity<BlockEntityBeehive>(pos);
			if (blockEntity != null && blockEntity.Harvestable)
			{
				return getHarvestableDrops(world, pos, byPlayer, dropQuantityMultiplier);
			}
		}
		return new ItemStack[1]
		{
			new ItemStack(world.BlockAccessor.GetBlock(CodeWithVariant("side", "east")))
		};
	}

	private ItemStack[]? getHarvestableDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		if (Drops == null)
		{
			return null;
		}
		List<ItemStack> todrop = new List<ItemStack>();
		for (int i = 0; i < Drops.Length; i++)
		{
			if (Drops[i].Tool.HasValue && (byPlayer == null || Drops[i].Tool != byPlayer.InventoryManager.ActiveTool))
			{
				continue;
			}
			ItemStack stack = Drops[i].GetNextItemStack(dropQuantityMultiplier);
			if (stack != null)
			{
				todrop.Add(stack);
				if (Drops[i].LastDrop)
				{
					break;
				}
			}
		}
		return todrop.ToArray();
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		if (itemStack.Attributes.GetBool("harvestable"))
		{
			return Lang.GetMatching(Code.Domain + ":block-" + CodeWithVariant("type", "harvestable").Path);
		}
		return base.GetHeldItemName(itemStack);
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack stack = base.OnPickBlock(world, pos);
		BlockEntityBeehive blockEntity = GetBlockEntity<BlockEntityBeehive>(pos);
		if (blockEntity != null && blockEntity.Harvestable)
		{
			stack.Attributes.SetBool("harvestable", value: true);
		}
		return stack;
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		WorldInteraction[] wi = new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = ((Variant["type"] == "populated") ? "blockhelp-skep-putinbagslot" : "blockhelp-skep-pickup"),
				MouseButton = EnumMouseButton.Right
			}
		};
		BlockEntityBeehive blockEntity = GetBlockEntity<BlockEntityBeehive>(selection.Position);
		WorldInteraction[] array2;
		WorldInteraction[] array;
		ReadOnlySpan<WorldInteraction> readOnlySpan;
		int num;
		if (blockEntity != null && blockEntity.Harvestable)
		{
			array = wi;
			num = 0;
			array2 = new WorldInteraction[1 + array.Length];
			readOnlySpan = new ReadOnlySpan<WorldInteraction>(array);
			readOnlySpan.CopyTo(new Span<WorldInteraction>(array2).Slice(num, readOnlySpan.Length));
			num += readOnlySpan.Length;
			array2[num] = new WorldInteraction
			{
				ActionLangCode = "blockhelp-skep-harvest",
				MouseButton = EnumMouseButton.Left
			};
			wi = array2;
		}
		array2 = wi;
		array = base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
		num = 0;
		WorldInteraction[] array3 = new WorldInteraction[array2.Length + array.Length];
		readOnlySpan = new ReadOnlySpan<WorldInteraction>(array2);
		readOnlySpan.CopyTo(new Span<WorldInteraction>(array3).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		ReadOnlySpan<WorldInteraction> readOnlySpan2 = new ReadOnlySpan<WorldInteraction>(array);
		readOnlySpan2.CopyTo(new Span<WorldInteraction>(array3).Slice(num, readOnlySpan2.Length));
		num += readOnlySpan2.Length;
		return array3;
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		if (itemstack.Attributes.GetBool("harvestable"))
		{
			string orientation = Variant["side"];
			string key = "beehive-" + Variant["material"] + "-harvestablemesh-" + orientation;
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate(capi, "skepHarvestableMeshRefs", () => new Dictionary<string, MultiTextureMeshRef>());
			if (!meshrefs.TryGetValue(key, out renderinfo.ModelRef))
			{
				MeshData mesh = ObjectCacheUtil.GetOrCreate(capi, key, delegate
				{
					Block block = capi.World.GetBlock(CodeWithVariant("type", "populated"));
					ArgumentNullException.ThrowIfNull(block, "fullSkep");
					AssetLocation assetLocation = AssetLocation.Create(block.Attributes?["harvestableShape"].AsString() ?? "game:shapes/block/beehive/skep-harvestable.json", Code.Domain);
					capi.Tesselator.TesselateShape(block, Vintagestory.API.Common.Shape.TryGet(api, assetLocation.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json")), out mesh, new Vec3f(0f, BlockFacing.FromCode(orientation).HorizontalAngleIndex * 90 - 90, 0f));
					return mesh;
				});
				meshrefs[key] = (renderinfo.ModelRef = capi.Render.UploadMultiTextureMesh(mesh));
			}
		}
		base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
	}

	public string[] GetFoodTags(ItemStack itemstack)
	{
		if (itemstack.Attributes.GetBool("harvestable"))
		{
			return Attributes?["harvestableFoodTags"].AsArray<string>() ?? new string[1] { "lootableSweet" };
		}
		return Array.Empty<string>();
	}
}
