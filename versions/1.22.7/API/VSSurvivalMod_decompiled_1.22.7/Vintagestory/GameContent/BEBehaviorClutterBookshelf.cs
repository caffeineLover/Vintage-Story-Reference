using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class BEBehaviorClutterBookshelf : BEBehaviorShapeFromAttributes
{
	public string Variant;

	public string Type2;

	private BookShelfVariantGroup vgroup
	{
		get
		{
			(base.Block as BlockClutterBookshelf).variantGroupsByCode.TryGetValue(Variant, out var vgroup);
			return vgroup;
		}
	}

	public BEBehaviorClutterBookshelf(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void OnBlockPlaced(ItemStack byItemStack = null)
	{
		Variant = byItemStack?.Attributes.GetString("variant") ?? Variant;
		if (Variant != null)
		{
			BookShelfVariantGroup bookShelfVariantGroup = vgroup;
			if (bookShelfVariantGroup != null && bookShelfVariantGroup.DoubleSided)
			{
				Type = (base.Block as BlockClutterBookshelf).RandomType(Variant);
				Type2 = (base.Block as BlockClutterBookshelf).RandomType(Variant);
				loadMesh();
				Blockentity.MarkDirty(redrawOnClient: true);
				return;
			}
		}
		base.OnBlockPlaced(byItemStack);
	}

	public override void loadMesh()
	{
		if (Type == null || Api.Side == EnumAppSide.Server || Variant == null)
		{
			return;
		}
		IShapeTypeProps cprops = clutterBlock.GetTypeProps(Type, null, this);
		if (cprops != null)
		{
			mesh = createMesh(cprops);
			if (cprops.ShapeLOD2Resolved != null)
			{
				lod2mesh = createMesh(cprops, forLOD2: true);
			}
		}
	}

	public override MeshData createMesh(IShapeTypeProps cprops, bool forLOD2 = false)
	{
		bool noOffset = offsetX == 0f && offsetY == 0f && offsetZ == 0f;
		float angleY = base.rotateY + cprops.Rotation.Y * ((float)Math.PI / 180f);
		MeshData baseMesh = clutterBlock.GetOrCreateMesh(cprops, null, null, forLOD2);
		MeshData mesh = ((!(rotateX == 0f && angleY == 0f && rotateZ == 0f && noOffset)) ? baseMesh.Clone().Rotate(BEBehaviorShapeFromAttributes.Origin, rotateX, angleY, rotateZ) : baseMesh);
		if (!noOffset)
		{
			mesh.Translate(offsetX, offsetY, offsetZ);
		}
		return mesh;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		string prevType2 = Type2;
		Variant = tree.GetString("variant");
		Type2 = tree.GetString("type2");
		if (worldAccessForResolve.Side == EnumAppSide.Client && Api != null && (mesh == null || prevType2 != Type2))
		{
			MaybeInitialiseMesh_OnMainThread();
			Blockentity.MarkDirty(redrawOnClient: true);
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetString("variant", Variant);
		tree.SetString("type2", Type2);
	}
}
