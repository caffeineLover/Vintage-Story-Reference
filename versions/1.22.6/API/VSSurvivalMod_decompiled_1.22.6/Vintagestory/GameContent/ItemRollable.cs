using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class ItemRollable : Item, IContainedMeshSource
{
	private string rolledShape;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		rolledShape = Attributes["rolledShape"].AsString();
	}

	public MeshData GenMesh(ItemSlot slot, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos = null)
	{
		if (!Attributes.KeyExists("rolledShape"))
		{
			return null;
		}
		ICoreClientAPI obj = api as ICoreClientAPI;
		AssetLocation loc = AssetLocation.Create(Attributes["rolledShape"].AsString(), Code.Domain).WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
		Shape shape = obj.Assets.TryGet(loc).ToObject<Shape>();
		ContainedTextureSource cnts = new ContainedTextureSource(obj, targetAtlas, shape.Textures, $"For displayed item {Code}");
		obj.Tesselator.TesselateShape(new TesselationMetaData
		{
			TexSource = cnts
		}, shape, out var meshdata);
		return meshdata;
	}

	public string GetMeshCacheKey(ItemSlot slot)
	{
		return string.Concat(slot.Itemstack.Collectible.Code, "-", rolledShape);
	}
}
