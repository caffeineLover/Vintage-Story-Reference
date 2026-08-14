using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class BEBehaviorShapeFromAttributes : BlockEntityBehavior, IRotatable, IExtraWrenchModes
{
	public string Type;

	public BlockShapeFromAttributes clutterBlock;

	protected MeshData mesh;

	protected MeshData lod2mesh;

	public float rotateX;

	public float rotateZ;

	public bool Collected;

	public string overrideTextureCode;

	public float repairState;

	public int reparability;

	protected static Vec3f Origin = new Vec3f(0.5f, 0.5f, 0.5f);

	public float offsetX;

	public float offsetY;

	public float offsetZ;

	protected bool loadMeshDuringTesselation;

	public float rotateY { get; internal set; }

	public BEBehaviorShapeFromAttributes(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public SkillItem[] GetExtraWrenchModes(IPlayer byPlayer, BlockSelection blockSelection)
	{
		return clutterBlock?.extraWrenchModes;
	}

	public void OnWrenchInteract(IPlayer player, BlockSelection blockSel, int mode, int rightmouseBtn)
	{
		switch (mode)
		{
		case 0:
			offsetZ += (float)(1 - rightmouseBtn * 2) / 16f;
			break;
		case 1:
			offsetX += (float)(1 - rightmouseBtn * 2) / 16f;
			break;
		case 2:
			offsetY += (float)(1 - rightmouseBtn * 2) / 16f;
			break;
		}
		loadMesh();
		Blockentity.MarkDirty(redrawOnClient: true);
		Api.World.PlaySoundAt(base.Block.Sounds.Place, blockSel.Position, 0.0, player);
		(Api.World as IClientWorldAccessor)?.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		clutterBlock = base.Block as BlockShapeFromAttributes;
		if (Type != null)
		{
			MaybeInitialiseMesh_OnMainThread();
			clutterBlock.GetBehavior<BlockBehaviorReparable>()?.Initialize(Type, this);
		}
	}

	public virtual void loadMesh()
	{
		if (Type == null || Api == null || Api.Side == EnumAppSide.Server)
		{
			return;
		}
		IShapeTypeProps cprops = clutterBlock?.GetTypeProps(Type, null, this);
		if (cprops != null)
		{
			mesh = createMesh(cprops);
			if (clutterBlock.Lod2Shape != null)
			{
				lod2mesh = createMesh(cprops, forLOD2: true);
			}
		}
	}

	public virtual MeshData createMesh(IShapeTypeProps cprops, bool forLOD2 = false)
	{
		bool noOffset = offsetX == 0f && offsetY == 0f && offsetZ == 0f;
		float angleY = rotateY + cprops.Rotation.Y * ((float)Math.PI / 180f);
		MeshData baseMesh = clutterBlock.GetOrCreateMesh(cprops, null, overrideTextureCode, forLOD2);
		MeshData mesh;
		if (cprops.RandomizeYSize)
		{
			BlockShapeFromAttributes blockShapeFromAttributes = clutterBlock;
			if (blockShapeFromAttributes == null || blockShapeFromAttributes.AllowRandomizeDims)
			{
				mesh = baseMesh.Clone().Rotate(Origin, rotateX, angleY, rotateZ).Scale(Vec3f.Zero, 1f, 0.98f + (float)GameMath.MurmurHash3Mod(base.Pos.X, base.Pos.Y, base.Pos.Z, 1000) / 1000f * 0.04f, 1f);
				goto IL_0135;
			}
		}
		mesh = ((!(rotateX == 0f && angleY == 0f && rotateZ == 0f && noOffset)) ? baseMesh.Clone().Rotate(Origin, rotateX, angleY, rotateZ) : baseMesh);
		goto IL_0135;
		IL_0135:
		if (!noOffset)
		{
			mesh.Translate(offsetX, offsetY, offsetZ);
		}
		return mesh;
	}

	public override void OnBlockPlaced(ItemStack byItemStack = null)
	{
		base.OnBlockPlaced(byItemStack);
		if (byItemStack != null)
		{
			Type = byItemStack.Attributes.GetString("type");
			Collected = byItemStack.Attributes.GetBool("collected");
		}
		loadMesh();
		clutterBlock.GetBehavior<BlockBehaviorReparable>()?.Initialize(Type, this);
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		IShapeTypeProps cprops = clutterBlock?.GetTypeProps(Type, null, this);
		if (cprops?.LightHsv != null)
		{
			Api.World.BlockAccessor.RemoveBlockLight(cprops.LightHsv, base.Pos);
		}
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		string prevType = Type;
		string prevOverrideTextureCode = overrideTextureCode;
		float prevRotateX = rotateX;
		float prevRotateY = rotateY;
		float prevRotateZ = rotateZ;
		float prevOffsetX = offsetX;
		float prevOffsetY = offsetY;
		float prevOffsetZ = offsetZ;
		Type = tree.GetString("type");
		if (Type != null)
		{
			Type = BlockClutter.Remap(worldAccessForResolve, Type);
		}
		rotateX = tree.GetFloat("rotateX");
		rotateY = tree.GetFloat("meshAngle");
		rotateZ = tree.GetFloat("rotateZ");
		overrideTextureCode = tree.GetString("overrideTextureCode");
		Collected = tree.GetBool("collected");
		repairState = tree.GetFloat("repairState");
		offsetX = tree.GetFloat("offsetX");
		offsetY = tree.GetFloat("offsetY");
		offsetZ = tree.GetFloat("offsetZ");
		if (worldAccessForResolve.Side == EnumAppSide.Client && Api != null && (mesh == null || prevType != Type || prevOverrideTextureCode != overrideTextureCode || rotateX != prevRotateX || rotateY != prevRotateY || rotateZ != prevRotateZ || offsetX != prevOffsetX || offsetY != prevOffsetY || offsetZ != prevOffsetZ))
		{
			MaybeInitialiseMesh_OnMainThread();
			relight(prevType);
			Blockentity.MarkDirty(redrawOnClient: true);
		}
	}

	protected void relight(string oldType)
	{
		IShapeTypeProps cprops = clutterBlock?.GetTypeProps(oldType, null, this);
		if (cprops?.LightHsv != null)
		{
			Api.World.BlockAccessor.RemoveBlockLight(cprops.LightHsv, base.Pos);
		}
		if ((clutterBlock?.GetTypeProps(Type, null, this))?.LightHsv != null)
		{
			Api.World.BlockAccessor.ExchangeBlock(base.Block.Id, base.Pos);
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		tree.SetString("type", Type);
		tree.SetFloat("rotateX", rotateX);
		tree.SetFloat("meshAngle", rotateY);
		tree.SetFloat("rotateZ", rotateZ);
		tree.SetBool("collected", Collected);
		tree.SetFloat("repairState", repairState);
		tree.SetFloat("offsetX", offsetX);
		tree.SetFloat("offsetY", offsetY);
		tree.SetFloat("offsetZ", offsetZ);
		if (overrideTextureCode != null)
		{
			tree.SetString("overrideTextureCode", overrideTextureCode);
		}
		base.ToTreeAttributes(tree);
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		MaybeInitialiseMesh_OffThread();
		if (lod2mesh == null)
		{
			mesher.AddMeshData(mesh);
		}
		else
		{
			mesher.AddMeshData(mesh, 2);
			mesher.AddMeshData(lod2mesh, 3);
		}
		return true;
	}

	protected void MaybeInitialiseMesh_OnMainThread()
	{
		if (Api.Side != EnumAppSide.Server)
		{
			if (RequiresTextureUploads())
			{
				loadMesh();
			}
			else
			{
				loadMeshDuringTesselation = true;
			}
		}
	}

	protected void MaybeInitialiseMesh_OffThread()
	{
		if (loadMeshDuringTesselation)
		{
			loadMeshDuringTesselation = false;
			loadMesh();
		}
	}

	private bool RequiresTextureUploads()
	{
		IShapeTypeProps cprops = clutterBlock?.GetTypeProps(Type, null, this);
		if (cprops == null)
		{
			return false;
		}
		if (cprops?.Textures == null && overrideTextureCode == null)
		{
			return false;
		}
		return !MSShapeFromAttrCacheHelper.IsInCache(Api as ICoreClientAPI, base.Block, cprops, overrideTextureCode);
	}

	public void OnTransformed(IWorldAccessor worldAccessor, ITreeAttribute tree, int degreeRotation, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, EnumAxis? flipAxis)
	{
		float thetaX = tree.GetFloat("rotateX");
		float thetaY = tree.GetFloat("meshAngle");
		float thetaZ = tree.GetFloat("rotateZ");
		IShapeTypeProps cprops = clutterBlock?.GetTypeProps(Type, null, this);
		if (cprops != null)
		{
			thetaY += cprops.Rotation.Y * ((float)Math.PI / 180f);
		}
		Span<float> m = stackalloc float[12];
		m[0] = 1f;
		m[5] = 1f;
		m[10] = 1f;
		Mat4f.RotateY(m, (float)(-degreeRotation) * ((float)Math.PI / 180f));
		Mat4f.RotateByXYZ(m, thetaX, thetaY, thetaZ);
		Mat4f.ExtractEulerAngles(m, ref thetaX, ref thetaY, ref thetaZ);
		if (cprops != null)
		{
			thetaY -= cprops.Rotation.Y * ((float)Math.PI / 180f);
		}
		tree.SetFloat("rotateX", thetaX);
		tree.SetFloat("meshAngle", thetaY);
		tree.SetFloat("rotateZ", thetaZ);
		rotateX = thetaX;
		rotateY = thetaY;
		rotateZ = thetaZ;
		float tmpOffsetX = tree.GetFloat("offsetX");
		offsetY = tree.GetFloat("offsetY");
		float tmpOffsetZ = tree.GetFloat("offsetZ");
		switch (degreeRotation)
		{
		case 90:
			offsetX = 0f - tmpOffsetZ;
			offsetZ = tmpOffsetX;
			break;
		case 180:
			offsetX = 0f - tmpOffsetX;
			offsetZ = 0f - tmpOffsetZ;
			break;
		case 270:
			offsetX = tmpOffsetZ;
			offsetZ = 0f - tmpOffsetX;
			break;
		}
		tree.SetFloat("offsetX", offsetX);
		tree.SetFloat("offsetY", offsetY);
		tree.SetFloat("offsetZ", offsetZ);
	}

	public void Rotate(EntityAgent byEntity, BlockSelection blockSel, int dir)
	{
		if (byEntity.Controls.ShiftKey)
		{
			if (blockSel.Face.Axis == EnumAxis.X)
			{
				rotateX += (float)Math.PI / 2f * (float)dir;
			}
			if (blockSel.Face.Axis == EnumAxis.Y)
			{
				rotateY += (float)Math.PI / 2f * (float)dir;
			}
			if (blockSel.Face.Axis == EnumAxis.Z)
			{
				rotateZ += (float)Math.PI / 2f * (float)dir;
			}
		}
		else
		{
			float deg22dot5rad = (float)Math.PI / 8f;
			rotateY += deg22dot5rad * (float)dir;
		}
		loadMesh();
		Blockentity.MarkDirty(redrawOnClient: true);
	}

	public override void OnPlacementBySchematic(ICoreServerAPI api, IBlockAccessor blockAccessor, BlockPos pos, Dictionary<int, Dictionary<int, int>> replaceBlocks, int centerrockblockid, Block layerBlock, bool resolveImports)
	{
		IShapeTypeProps obj = (base.Block as BlockShapeFromAttributes)?.GetTypeProps(Type, null, this);
		byte? obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			byte[] lightHsv = obj.LightHsv;
			obj2 = ((lightHsv != null) ? new byte?(lightHsv[2]) : ((byte?)null));
		}
		if (obj2 > 0 && blockAccessor is IWorldGenBlockAccessor wgen)
		{
			wgen.ScheduleBlockLightUpdate(pos, 0, base.Block.BlockId);
		}
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		base.GetBlockInfo(forPlayer, dsc);
		if (Api is ICoreClientAPI capi && capi.Settings.Bool["extendedDebugInfo"])
		{
			dsc.AppendLine("<font color=\"#bbbbbb\">Type:" + Type + "</font>");
		}
	}

	public string GetFullCode()
	{
		return clutterBlock.BaseCodeForName() + Type?.Replace("/", "-");
	}
}
