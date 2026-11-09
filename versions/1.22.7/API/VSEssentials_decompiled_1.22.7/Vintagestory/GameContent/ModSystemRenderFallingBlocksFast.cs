using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ModSystemRenderFallingBlocksFast : ModSystem, IRenderer, IDisposable, ITerrainMeshPool
{
	public Dictionary<long, EntityBlockFalling> fallingBlocks = new Dictionary<long, EntityBlockFalling>();

	protected EntityPartitioning ep;

	protected ICoreClientAPI capi;

	protected int plrDim;

	protected Vec3d plrPos;

	protected float viewDistSq = 22500f;

	protected Matrixf ModelMat = new Matrixf();

	protected double rotaccum;

	protected MeshData mesh = new MeshData(4, 3);

	public double RenderOrder => 0.4;

	public int RenderRange => 150;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Client;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
		api.Event.RegisterRenderer(this, EnumRenderStage.Opaque, "renderfallingblocks");
		api.Event.LeaveWorld += Event_LeaveWorld;
		ep = api.ModLoader.GetModSystem<EntityPartitioning>();
	}

	private void Event_LeaveWorld()
	{
		Dictionary<string, MultiTextureMeshRef> dict = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(capi, "fallingblockmeshrefs");
		if (dict == null)
		{
			return;
		}
		foreach (KeyValuePair<string, MultiTextureMeshRef> item in dict)
		{
			item.Value.Dispose();
		}
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		if (fallingBlocks.Count == 0)
		{
			return;
		}
		rotaccum += deltaTime;
		IRenderAPI rapi = capi.Render;
		rapi.GlDisableCullFace();
		rapi.GlToggleBlend(blend: true);
		IStandardShaderProgram prog = rapi.PreparedStandardShader(0, 0, 0);
		prog.ViewMatrix = rapi.CameraMatrixOriginf;
		prog.ProjectionMatrix = rapi.CurrentProjectionMatrix;
		Vec3d camPos = capi.World.Player.Entity.CameraPos;
		plrDim = capi.World.Player.Entity.Pos.Dimension;
		plrPos = capi.World.Player.Entity.Pos.XYZ;
		Vec3d curPos = new Vec3d();
		foreach (EntityBlockFalling entity in fallingBlocks.Values)
		{
			if (IsRendered(entity))
			{
				if (entity.meshRef == null)
				{
					genMesh(entity);
				}
				Vec4f lightrgbs = capi.World.BlockAccessor.GetLightRGBs((int)entity.Pos.X, (int)entity.Pos.Y, (int)entity.Pos.Z);
				prog.RgbaLightIn = lightrgbs;
				curPos.Set(entity.Pos.X + (double)entity.SelectionBox.X1, entity.Pos.Y + (double)entity.SelectionBox.Y1, entity.Pos.Z + (double)entity.SelectionBox.Z1);
				float div = (entity.Collided ? 4f : 1.5f);
				double rotaccumThis = rotaccum + (double)((float)(entity.EntityId.GetHashCode() % 1000) / 1000f * ((float)Math.PI * 2f));
				prog.ModelMatrix = ModelMat.Identity().Translate(curPos.X - camPos.X + (double)(GameMath.Sin((float)capi.InWorldEllapsedMilliseconds / 120f + 30f) / 20f / div), curPos.Y - camPos.Y, curPos.Z - camPos.Z + (double)(GameMath.Cos((float)capi.InWorldEllapsedMilliseconds / 110f + 20f) / 20f / div)).RotateX((float)(Math.Sin(rotaccumThis * 10.0) / 10.0 / (double)div))
					.RotateZ((float)(Math.Cos(10.0 + rotaccumThis * 9.0) / 10.0 / (double)div))
					.Values;
				rapi.RenderMultiTextureMesh(entity.meshRef, "tex");
			}
		}
		prog.Stop();
	}

	private void genMesh(EntityBlockFalling entity)
	{
		int posx = entity.blockEntityAttributes?.GetInt("posx", entity.initialPos.X) ?? entity.initialPos.X;
		int posy = entity.blockEntityAttributes?.GetInt("posy", entity.initialPos.Y) ?? entity.initialPos.Y;
		int posz = entity.blockEntityAttributes?.GetInt("posz", entity.initialPos.Z) ?? entity.initialPos.Z;
		BlockEntity be = capi.World.BlockAccessor.GetBlockEntity(new BlockPos(posx, posy, posz));
		if (!entity.InitialBlockRemoved)
		{
			be?.OnTesselation(this, capi.Tesselator);
			if (mesh.VerticesCount > 0)
			{
				mesh.CustomBytes = null;
				mesh.CustomFloats = null;
				mesh.CustomInts = null;
				entity.meshRef = capi.Render.UploadMultiTextureMesh(mesh);
			}
		}
		if (entity.meshRef != null)
		{
			return;
		}
		Dictionary<string, MultiTextureMeshRef> dict = ObjectCacheUtil.GetOrCreate(capi, "fallingblockmeshrefs", () => new Dictionary<string, MultiTextureMeshRef>());
		string cacheKey = entity.Block.Code;
		IFallingBlockMeshSource ifbms = be as IFallingBlockMeshSource;
		if (ifbms != null)
		{
			cacheKey = ifbms.GetCacheKey(entity);
		}
		entity.reusedMeshRef = true;
		if (dict.TryGetValue(cacheKey, out entity.meshRef))
		{
			return;
		}
		if (ifbms != null)
		{
			ifbms.OnFallingBlockTesselation(entity, this, capi.Tesselator);
			if (mesh.VerticesCount > 0)
			{
				mesh.CustomBytes = null;
				mesh.CustomFloats = null;
				mesh.CustomInts = null;
				entity.meshRef = capi.Render.UploadMultiTextureMesh(mesh);
			}
		}
		else
		{
			mesh = capi.TesselatorManager.GetDefaultBlockMesh(entity.Block);
		}
		dict[cacheKey] = (entity.meshRef = capi.Render.UploadMultiTextureMesh(mesh));
	}

	private bool IsRendered(EntityBlockFalling entity)
	{
		if (!entity.DoRender || (!entity.InitialBlockRemoved && entity.World.BlockAccessor.GetBlock(entity.initialPos).Id != 0))
		{
			return false;
		}
		EntityPos entityPos = entity.Pos;
		if (!capi.Render.DefaultFrustumCuller.SphereInFrustum((float)entityPos.X, (float)entityPos.InternalY, (float)entityPos.Z, entity.FrustumSphereRadius))
		{
			return false;
		}
		if (entityPos.Dimension != plrDim)
		{
			return false;
		}
		if (entity.AllowOutsideLoadedRange)
		{
			return true;
		}
		if (plrPos.HorizontalSquareDistanceTo(entityPos.X, entityPos.Z) >= viewDistSq)
		{
			return false;
		}
		return capi.IsChunkRendered(entityPos);
	}

	public void AddMeshData(MeshData data, int lodlevel = 1)
	{
		if (data != null)
		{
			mesh.AddMeshData(data);
		}
	}

	public void AddMeshData(MeshData data, ColorMapData colormapdata, int lodlevel = 1)
	{
		if (data != null)
		{
			mesh.AddMeshData(data);
		}
	}

	public void AddMeshData(MeshData data, float[] tfMatrix, int lodLevel = 1)
	{
		if (data != null)
		{
			mesh.AddMeshData(data);
		}
	}
}
