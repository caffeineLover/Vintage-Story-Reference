using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;

namespace Vintagestory.GameContent;

public class BlockMaterialFromAttributes : Block
{
	public Dictionary<string, CompositeTexture> TexturesBMFA;

	public virtual string MeshKey => "BMA";

	public virtual string MeshKeyInventory => MeshKey + "Inventory";

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		LoadTypes();
	}

	public virtual void LoadTypes()
	{
		RegistryObjectVariantGroup grp = Attributes["materials"].AsObject<RegistryObjectVariantGroup>();
		ArgumentNullException.ThrowIfNull(grp, "grp");
		TexturesBMFA = Attributes["textures"].AsObject<Dictionary<string, CompositeTexture>>();
		ArgumentNullException.ThrowIfNull(TexturesBMFA, "TexturesBMFA");
		string[] materials = grp.States;
		if (grp.LoadFromProperties != null)
		{
			materials = api.Assets.TryGet(grp.LoadFromProperties.WithPathPrefixOnce("worldproperties/").WithPathAppendixOnce(".json")).ToObject<StandardWorldProperty>().Variants.Select((WorldPropertyVariant p) => p.Code.Path).ToArray().Append(materials);
		}
		List<JsonItemStack> stacks = new List<JsonItemStack>();
		string[] array = materials;
		foreach (string material in array)
		{
			JsonItemStack jstack = new JsonItemStack
			{
				Code = Code,
				Type = EnumItemClass.Block,
				Attributes = new JsonObject(JToken.Parse("{ \"material\": \"" + material + "\" }"))
			};
			jstack.Resolve(api.World, string.Concat(Code, " type"));
			stacks.Add(jstack);
		}
		CreativeInventoryStacks = new CreativeTabAndStackList[1]
		{
			new CreativeTabAndStackList
			{
				Stacks = stacks.ToArray(),
				Tabs = new string[2] { "general", "decorative" }
			}
		};
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		Dictionary<string, MeshData> meshes = ObjectCacheUtil.TryGet<Dictionary<string, MeshData>>(api, MeshKey);
		string key;
		if (meshes != null && meshes.Count > 0)
		{
			foreach (KeyValuePair<string, MeshData> item in meshes)
			{
				item.Deconstruct(out key, out var value);
				value.Dispose();
			}
			ObjectCacheUtil.Delete(api, MeshKey);
		}
		Dictionary<string, MultiTextureMeshRef> invMeshes = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, MeshKeyInventory);
		if (invMeshes != null && invMeshes.Count > 0)
		{
			foreach (KeyValuePair<string, MultiTextureMeshRef> item2 in invMeshes)
			{
				item2.Deconstruct(out key, out var value2);
				value2.Dispose();
			}
			ObjectCacheUtil.Delete(api, MeshKeyInventory);
		}
		base.OnUnloaded(api);
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		Dictionary<string, MultiTextureMeshRef> meshRefs = ObjectCacheUtil.GetOrCreate(capi, MeshKeyInventory, () => new Dictionary<string, MultiTextureMeshRef>());
		string material = itemstack.Attributes.GetString("material", "");
		string key = Variant["type"] + material;
		if (!meshRefs.TryGetValue(key, out var meshref))
		{
			MeshData mesh = GetOrCreateMesh(material);
			meshref = (meshRefs[key] = capi.Render.UploadMultiTextureMesh(mesh));
		}
		renderinfo.ModelRef = meshref;
	}

	public virtual MeshData GetOrCreateMesh(string material, ITexPositionSource? overrideTexturesource = null)
	{
		Dictionary<string, MeshData> cMeshes = ObjectCacheUtil.GetOrCreate(api, MeshKey, () => new Dictionary<string, MeshData>());
		ICoreClientAPI capi = (ICoreClientAPI)api;
		string key = Variant["type"] + material;
		if (overrideTexturesource != null || !cMeshes.TryGetValue(key, out var mesh))
		{
			CompositeShape rcshape = Shape.Clone();
			mesh = capi.TesselatorManager.CreateMesh(string.Concat(Code, " block"), rcshape, (Shape shape, string name) => new ShapeTextureSource(capi, shape, name, TexturesBMFA, (string p) => p.Replace("{material}", material)), overrideTexturesource);
			if (overrideTexturesource == null)
			{
				cMeshes[key] = mesh;
			}
		}
		return mesh;
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
	{
		bool num = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
		if (num)
		{
			BEBehaviorMaterialFromAttributes bect = world.BlockAccessor.GetBlockEntity(blockSel.Position).GetBehavior<BEBehaviorMaterialFromAttributes>();
			if (bect != null)
			{
				BlockPos targetPos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
				double y = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
				double dz = (double)(float)byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
				float num2 = (float)Math.Atan2(y, dz);
				float intervalRad = (float)Math.PI / 2f;
				float roundRad = (float)(int)Math.Round(num2 / intervalRad) * intervalRad;
				bect.MeshAngleY = roundRad;
				bect.Init();
			}
		}
		return num;
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		return new ItemStack[1] { OnPickBlock(world, pos) };
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack stack = base.OnPickBlock(world, pos);
		BEBehaviorMaterialFromAttributes beshelf = world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorMaterialFromAttributes>();
		if (beshelf != null)
		{
			stack.Attributes.SetString("material", beshelf.Material);
		}
		return stack;
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		return "block-" + Code.Path + "-" + itemStack.Attributes.GetString("material");
	}
}
