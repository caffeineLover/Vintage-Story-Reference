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

public class BlockAnimalTrap : Block
{
	protected float rotInterval = (float)Math.PI / 8f;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		if (api is ICoreClientAPI capi)
		{
			AddTrappableHandbookInfo(capi);
		}
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
	{
		bool num = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
		if (num)
		{
			BlockEntityAnimalTrap be = GetBlockEntity<BlockEntityAnimalTrap>(blockSel.Position);
			if (be != null)
			{
				BlockPos targetPos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
				double y = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
				double dz = (double)(float)byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
				float roundRad = (float)(int)Math.Round((float)Math.Atan2(y, dz) / rotInterval) * rotInterval;
				be.RotationYDeg = roundRad * (180f / (float)Math.PI);
				be.MarkDirty(redrawOnClient: true);
			}
		}
		return num;
	}

	public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ItemStack? byItemStack = null)
	{
		base.OnBlockPlaced(world, blockPos, byItemStack);
		if (byItemStack != null && byItemStack.Attributes.GetBool("destroyed"))
		{
			GetBlockEntity<BlockEntityAnimalTrap>(blockPos).TrapState = EnumTrapState.Destroyed;
		}
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		return GetBlockEntity<BlockEntityAnimalTrap>(blockSel.Position)?.Interact(byPlayer, blockSel) ?? base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
	{
		ItemStack[] stacks = (handbookStack.Attributes.GetBool("destroyed") ? getDestroyedDrops(api.World, forPlayer.Entity.Pos.XYZ.AsBlockPos, forPlayer) : GetDrops(api.World, forPlayer.Entity.Pos.XYZ.AsBlockPos, forPlayer));
		if (stacks == null)
		{
			return Array.Empty<BlockDropItemStack>();
		}
		return stacks.Select((ItemStack stack) => new BlockDropItemStack(stack)).ToArray();
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		BlockEntityAnimalTrap be = GetBlockEntity<BlockEntityAnimalTrap>(pos);
		if (be != null && be.TrapState == EnumTrapState.Trapped)
		{
			JsonItemStack jstack = Attributes["creatureContainer"].AsObject<JsonItemStack>();
			if (be.Inventory[0].Empty && jstack != null)
			{
				jstack.Resolve(world, "creature container of " + Code);
				return new ItemStack[1] { jstack.ResolvedItemstack };
			}
			return Array.Empty<ItemStack>();
		}
		if (be != null && be.TrapState == EnumTrapState.Destroyed)
		{
			return getDestroyedDrops(world, pos, byPlayer, dropQuantityMultiplier);
		}
		return base.GetDrops(world, pos, byPlayer, dropQuantityMultiplier);
	}

	private ItemStack[] getDestroyedDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		BlockDropItemStack[] destroyedDrops = Attributes?["destroyedDrops"]?.AsObject<BlockDropItemStack[]>();
		if (destroyedDrops == null)
		{
			return Array.Empty<ItemStack>();
		}
		List<ItemStack> todrop = new List<ItemStack>();
		foreach (BlockDropItemStack obj in destroyedDrops)
		{
			obj.Resolve(world, "Block ", Code);
			ItemStack stack = obj.ToRandomItemstackForPlayer(byPlayer, world, dropQuantityMultiplier);
			if (stack != null)
			{
				todrop.Add(stack);
			}
			if (obj.LastDrop)
			{
				break;
			}
		}
		return todrop.ToArray();
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		if (itemStack.Attributes.GetBool("destroyed"))
		{
			return Lang.GetMatching(Code.Domain + ":block-" + Code.Path + "-destroyed");
		}
		return base.GetHeldItemName(itemStack);
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack stack = base.OnPickBlock(world, pos);
		BlockEntityAnimalTrap blockEntity = GetBlockEntity<BlockEntityAnimalTrap>(pos);
		if (blockEntity != null && blockEntity.TrapState == EnumTrapState.Destroyed)
		{
			stack.Attributes.SetBool("destroyed", value: true);
		}
		return stack;
	}

	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
	{
		BlockEntityAnimalTrap be = GetBlockEntity<BlockEntityAnimalTrap>(pos);
		if (be != null)
		{
			blockModelData = be.GetCurrentMesh(null).Clone().Rotate(Vec3f.Half, 0f, (be.RotationYDeg - 90f) * ((float)Math.PI / 180f), 0f);
			decalModelData = be.GetCurrentMesh(decalTexSource).Clone().Rotate(Vec3f.Half, 0f, (be.RotationYDeg - 90f) * ((float)Math.PI / 180f), 0f);
		}
		else
		{
			base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
		}
	}

	public bool IsAppetizingBait(ICoreAPI api, ItemStack baitStack)
	{
		CollectibleObject collobj = baitStack.Collectible;
		if (collobj.GetNutritionProperties(api.World, baitStack, null) == null)
		{
			JsonObject attributes = collobj.Attributes;
			if (attributes == null || !attributes["foodTags"].Exists)
			{
				return false;
			}
		}
		return api.World.EntityTypes.Any(delegate(EntityProperties type)
		{
			JsonObject attributes2 = type.Attributes;
			return attributes2 != null && attributes2["creatureDiet"].AsObject<CreatureDiet>()?.Matches(baitStack, checkCategory: true, 0.5f) == true;
		});
	}

	public bool CanFitBait(ICoreAPI api, ItemStack baitStack)
	{
		CollectibleObject collobj = baitStack.Collectible;
		JsonObject attributes = Attributes;
		if (attributes == null)
		{
			return true;
		}
		return attributes["excludeFoodTags"].AsArray<string>()?.Any(delegate(string tag)
		{
			JsonObject attributes2 = collobj.Attributes;
			return attributes2 != null && attributes2["foodTags"].AsArray<string>()?.Contains<string>(tag) == true;
		}) != true;
	}

	protected virtual void AddTrappableHandbookInfo(ICoreClientAPI capi)
	{
		ExtraHandbookSection[] extraHandbookSections = Attributes?["handbook"]?["extraSections"]?.AsObject<ExtraHandbookSection[]>();
		if (extraHandbookSections?.FirstOrDefault((ExtraHandbookSection s) => s?.Title == "handbook-trappableanimals-title") != null)
		{
			return;
		}
		JsonObject attributes = Attributes;
		if (attributes == null || !attributes["handbook"].Exists)
		{
			if (Attributes == null)
			{
				Attributes = new JsonObject(JToken.Parse("{ handbook: {} }"));
			}
			else
			{
				Attributes.Token["handbook"] = JToken.Parse("{ }");
			}
		}
		string trapType = Attributes["traptype"].AsString("small");
		HashSet<string> creatureNames = new HashSet<string>();
		foreach (EntityProperties entityType in capi.World.EntityTypes)
		{
			JsonObject attributes2 = entityType.Attributes;
			string code = attributes2?["handbook"]?["trappableGroupCode"]?.AsString() ?? (entityType.Code.Domain + ":item-creature-" + entityType.Code.Path);
			if (attributes2 != null && attributes2["trappable"]?[trapType]?["trapChance"]?.AsFloat() > 0f)
			{
				creatureNames.Add(Lang.GetMatching(code));
			}
		}
		ExtraHandbookSection section = new ExtraHandbookSection
		{
			Title = "handbook-trappableanimals-title",
			Text = string.Join(", ", creatureNames)
		};
		if (extraHandbookSections != null)
		{
			extraHandbookSections.Append(section);
		}
		else
		{
			extraHandbookSections = new ExtraHandbookSection[1] { section };
		}
		Attributes["handbook"].Token["extraSections"] = JToken.FromObject(extraHandbookSections);
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		if (itemstack.Attributes.GetBool("destroyed"))
		{
			CompositeShape cshape = Attributes["destroyedShape"].AsObject<CompositeShape>(null, Code.Domain);
			ArgumentNullException.ThrowIfNull(cshape, "cshape");
			string key = Variant["material"] + "BasketTrap-" + cshape.ToString();
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate(capi, "animalTrapDestroyedMeshRefs", () => new Dictionary<string, MultiTextureMeshRef>());
			if (!meshrefs.TryGetValue(key, out renderinfo.ModelRef))
			{
				MeshData mesh = ObjectCacheUtil.GetOrCreate(capi, key, () => capi.TesselatorManager.CreateMesh("basket trap decal", cshape, (Shape shape, string name) => new ShapeTextureSource(capi, shape, name), capi.Tesselator.GetTextureSource(this)));
				meshrefs[key] = (renderinfo.ModelRef = capi.Render.UploadMultiTextureMesh(mesh));
			}
		}
		base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
	}
}
