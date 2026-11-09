using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;

namespace Vintagestory.GameContent;

public class BlockShapeMaterialFromAttributes : Block, IHandbookGrouping
{
	private string[] types;

	private string[] materials;

	public Dictionary<string, CompositeTexture> TexturesBSMFA;

	public CompositeShape Cshape;

	public virtual string MeshKey { get; } = "BSMFA";

	public virtual string MeshKeyInventory => MeshKey + "Inventory";

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		LoadTypes();
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

	public virtual void LoadTypes()
	{
		types = Attributes["types"].AsArray<string>();
		ArgumentNullException.ThrowIfNull(types, "types");
		Cshape = Attributes["shape"].AsObject<CompositeShape>();
		ArgumentNullException.ThrowIfNull(Cshape, "Cshape");
		TexturesBSMFA = Attributes["textures"].AsObject<Dictionary<string, CompositeTexture>>();
		ArgumentNullException.ThrowIfNull(TexturesBSMFA, "TexturesBSMFA");
		RegistryObjectVariantGroup grp = Attributes["materials"].AsObject<RegistryObjectVariantGroup>();
		ArgumentNullException.ThrowIfNull(grp, "grp");
		materials = grp.States;
		if (grp.LoadFromProperties != null)
		{
			StandardWorldProperty prop = api.Assets.TryGet(grp.LoadFromProperties.WithPathPrefixOnce("worldproperties/").WithPathAppendixOnce(".json")).ToObject<StandardWorldProperty>();
			materials = prop.Variants.Select((WorldPropertyVariant p) => p.Code.Path).ToArray().Append<string>(materials);
		}
		List<JsonItemStack> stacks = new List<JsonItemStack>();
		string[] array = types;
		foreach (string type in array)
		{
			string[] array2 = materials;
			foreach (string material in array2)
			{
				JsonItemStack jsonItemStack = new JsonItemStack();
				jsonItemStack.Code = Code;
				jsonItemStack.Type = EnumItemClass.Block;
				jsonItemStack.Attributes = new JsonObject(JToken.Parse("{ \"type\": \"" + type + "\", \"material\": \"" + material + "\" }"));
				JsonItemStack jstack = jsonItemStack;
				jstack.Resolve(api.World, string.Concat(Code, " type"));
				stacks.Add(jstack);
			}
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

	public virtual MeshData GetOrCreateMesh(string? type, string? material, string? cachekeyextra = null, ITexPositionSource? overrideTexturesource = null)
	{
		Dictionary<string, MeshData> cMeshes = ObjectCacheUtil.GetOrCreate(api, MeshKey, () => new Dictionary<string, MeshData>());
		ICoreClientAPI capi = (ICoreClientAPI)api;
		string key = type + "-" + material + cachekeyextra;
		if (overrideTexturesource != null || !cMeshes.TryGetValue(key, out var mesh))
		{
			CompositeShape rcshape = Cshape.Clone();
			rcshape.Base.Path = rcshape.Base.Path.Replace("{type}", type).Replace("{material}", material);
			mesh = capi.TesselatorManager.CreateMesh(string.Concat(Code, " block"), rcshape, (Shape shape, string name) => new ShapeTextureSource(capi, shape, name, TexturesBSMFA, (string p) => p.Replace("{type}", type).Replace("{material}", material)), overrideTexturesource);
			if (overrideTexturesource == null)
			{
				cMeshes[key] = mesh;
			}
		}
		return mesh;
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
		Dictionary<string, MultiTextureMeshRef> meshRefs = ObjectCacheUtil.GetOrCreate(capi, MeshKeyInventory, () => new Dictionary<string, MultiTextureMeshRef>());
		string type = itemstack.Attributes.GetString("type", "");
		string material = itemstack.Attributes.GetString("material", "");
		string key = type + "-" + material;
		if (!meshRefs.TryGetValue(key, out var meshref))
		{
			MeshData mesh = GetOrCreateMesh(type, material);
			meshref = (meshRefs[key] = capi.Render.UploadMultiTextureMesh(mesh));
		}
		renderinfo.ModelRef = meshref;
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
	{
		bool num = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
		if (num)
		{
			BEBehaviorShapeMaterialFromAttributes bect = world.BlockAccessor.GetBlockEntity(blockSel.Position).GetBehavior<BEBehaviorShapeMaterialFromAttributes>();
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

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		string wood = inSlot.Itemstack.Attributes.GetString("material", "oak");
		dsc.AppendLine(Lang.Get("Material: {0}", Lang.Get("material-" + wood)));
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		return new ItemStack[1] { OnPickBlock(world, pos) };
	}

	public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
	{
		BlockDropItemStack[] drops = base.GetDropsForHandbook(handbookStack, forPlayer);
		drops[0] = drops[0].Clone();
		ArgumentNullException.ThrowIfNull(drops[0].ResolvedItemstack, "drops[0].ResolvedItemstack");
		drops[0].ResolvedItemstack.SetFrom(handbookStack);
		return drops;
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack stack = base.OnPickBlock(world, pos);
		BEBehaviorShapeMaterialFromAttributes beshelf = world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorShapeMaterialFromAttributes>();
		if (beshelf != null)
		{
			stack.Attributes.SetString("type", beshelf.Type);
			stack.Attributes.SetString("material", beshelf.Material);
		}
		return stack;
	}

	public void Rotate(EntityAgent byEntity, BlockSelection blockSel, int dir)
	{
		(GetBlockEntity<BlockEntityGeneric>(blockSel.Position)?.GetBehavior<BEBehaviorShapeMaterialFromAttributes>())?.Rotate(byEntity, blockSel, dir);
	}

	public AssetLocation GetCodeForHandbookGrouping(ItemStack stack)
	{
		return AssetLocation.Create(Code.Path + "-" + stack.Attributes.GetAsString("type") + "-" + stack.Attributes.GetAsString("material"), Code.Domain);
	}

	public string GetWildcardForHandbookGrouping(string wildcard, ItemStack stack)
	{
		return wildcard.Replace("{type}", stack.Attributes.GetAsString("type")).Replace("{material}", stack.Attributes.GetAsString("material"));
	}
}
