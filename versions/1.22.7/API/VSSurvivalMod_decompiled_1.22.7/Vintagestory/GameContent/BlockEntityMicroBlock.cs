using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Vintagestory.API.Client;
using Vintagestory.API.Client.Tesselation;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockEntityMicroBlock : BlockEntity, IRotatable, IAcceptsDecor, IMaterialExchangeable
{
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct VoxelInfo
	{
		public int Material;

		public ushort MainIndex;

		public byte Size;

		public bool CullFace;
	}

	public ref struct GenFaceInfo
	{
		public ICoreClientAPI capi;

		public MeshData targetMesh;

		public SizeConverter converter;

		public unsafe int* originalBounds;

		public int posPacked;

		public int width;

		public int length;

		public int face;

		public BlockFacing facing;

		public bool AnyFrostable;

		public float subPixelPaddingx;

		public float subPixelPaddingy;

		public int flags;

		private VoxelMaterial decorMat;

		private float uSize;

		private float vSize;

		private float uOffset;

		private float vOffset;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetInfo(SizeConverter converter, int face, VoxelMaterial decorMat)
		{
			this.decorMat = decorMat;
			this.face = face;
			facing = BlockFacing.ALLFACES[face];
			this.converter = converter;
		}

		public unsafe void GenFace(in VoxelMaterial blockMat)
		{
			MeshData targetMesh = this.targetMesh;
			XYZ xyz = default(XYZ);
			xyz.Z = posPacked / 324;
			posPacked -= xyz.Z * 324;
			xyz.Y = posPacked / 18;
			posPacked -= xyz.Y * 18;
			xyz.Z--;
			xyz.Y--;
			xyz.X = posPacked - 1;
			converter(width, length, out var halfSizeX, out var halfSizeY, out var halfSizeZ);
			float posX = (float)xyz.X * 0.0625f;
			float posY = (float)xyz.Y * 0.0625f;
			float posZ = (float)xyz.Z * 0.0625f;
			float centerX = posX + halfSizeX;
			float centerY = posY + halfSizeY;
			float centerZ = posZ + halfSizeZ;
			int axis = (int)facing.Axis;
			TextureAtlasPosition tpos = ((originalBounds[face] == xyz[axis] + shiftOffsetByFace[face]) ? blockMat.Texture[face] : blockMat.TextureInside[face]);
			TextureAtlasPosition topsoiltpos = blockMat.TextureTopSoil;
			TextureAtlasPosition[] texture = decorMat.Texture;
			TextureAtlasPosition decortpos = ((texture != null) ? texture[face] : null) ?? null;
			uSize = 2f * ((axis == 0) ? halfSizeZ : halfSizeX);
			vSize = 2f * ((axis == 1) ? halfSizeZ : halfSizeY);
			uOffset = ((axis == 0) ? posZ : posX);
			vOffset = ((axis == 1) ? posZ : posY);
			float texWidth = tpos.x2 - tpos.x1;
			float texHeight = tpos.y2 - tpos.y1;
			float uBase = tpos.x1 - subPixelPaddingx;
			float vBase = tpos.y1 - subPixelPaddingy;
			int zoff1 = 256;
			int[] CubeVertices = CubeMeshUtil.CubeVertices;
			int start = ((targetMesh.IndicesCount > 0) ? (targetMesh.Indices[targetMesh.IndicesCount - 1] + 1) : 0);
			int vertOffset = face * 4;
			for (int i = 0; i < 4; i++)
			{
				int ind = vertOffset + i;
				GetRelativeUV(ind * 2, out var uRel, out var vRel);
				targetMesh.AddVertexWithFlagsSkipColor((float)CubeVertices[ind * 3] * halfSizeX + centerX, (float)CubeVertices[ind * 3 + 1] * halfSizeY + centerY, (float)CubeVertices[ind * 3 + 2] * halfSizeZ + centerZ, uBase + uRel * texWidth, vBase + vRel * texHeight, flags);
				if (targetMesh.CustomShorts != null)
				{
					if (topsoiltpos == null)
					{
						targetMesh.CustomShorts.Add(default(short), default(short));
					}
					else
					{
						targetMesh.CustomShorts.AddPackedUV(topsoiltpos.x1 + uRel * texWidth, topsoiltpos.y1 + vRel * texHeight, uRel > 0f, vRel > 0f);
					}
				}
			}
			int faceOffset = face * 6;
			for (int j = 0; j < 6; j++)
			{
				targetMesh.AddIndex(start + CubeMeshUtil.CubeVertexIndices[faceOffset + j] - vertOffset);
			}
			targetMesh.AddXyzFace(facing.MeshDataIndex);
			targetMesh.AddTextureId(tpos.atlasTextureId);
			if (AnyFrostable)
			{
				targetMesh.AddColorMapIndex(blockMat.ClimateMapIndex, blockMat.SeasonMapIndex, blockMat.Frostable);
			}
			else
			{
				targetMesh.AddColorMapIndex(blockMat.ClimateMapIndex, blockMat.SeasonMapIndex);
			}
			targetMesh.AddRenderPass((short)blockMat.RenderPass);
			if (decortpos == null)
			{
				return;
			}
			start = ((targetMesh.IndicesCount > 0) ? (targetMesh.Indices[targetMesh.IndicesCount - 1] + 1) : 0);
			vertOffset = face * 4;
			texWidth = decortpos.x2 - decortpos.x1;
			texHeight = decortpos.y2 - decortpos.y1;
			uBase = decortpos.x1 - subPixelPaddingx;
			vBase = decortpos.y1 - subPixelPaddingy;
			Vec3i faceNormal = BlockFacing.ALLNORMALI[face];
			float xg = (1f + Math.Abs((float)faceNormal.X * 0.01f)) * halfSizeX;
			float yg = (1f + Math.Abs((float)faceNormal.Y * 0.01f)) * halfSizeY;
			float zg = (1f + Math.Abs((float)faceNormal.Z * 0.01f)) * halfSizeZ;
			int decorRotation = decorMat.TextureRotation;
			for (int k = 0; k < 4; k++)
			{
				int ind2 = vertOffset + k;
				GetRelativeUV(ind2 * 2, out var u, out var v);
				if ((decorRotation & 4) == 0)
				{
					u = 1f - u;
				}
				switch (decorRotation % 8)
				{
				case 3:
				case 5:
				{
					float vtmp = v;
					v = 1f - u;
					u = vtmp;
					break;
				}
				case 2:
				case 6:
					u = 1f - u;
					v = 1f - v;
					break;
				case 1:
				case 7:
				{
					float vtmp = v;
					v = u;
					u = 1f - vtmp;
					break;
				}
				}
				targetMesh.AddVertexWithFlagsSkipColor((float)CubeVertices[ind2 * 3] * xg + centerX, (float)CubeVertices[ind2 * 3 + 1] * yg + centerY, (float)CubeVertices[ind2 * 3 + 2] * zg + centerZ, uBase + u * texWidth, vBase + v * texHeight, flags | zoff1);
			}
			if (targetMesh.CustomShorts != null)
			{
				targetMesh.CustomShorts.Add(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short));
			}
			faceOffset = face * 6;
			for (int l = 0; l < 6; l++)
			{
				targetMesh.AddIndex(start + CubeMeshUtil.CubeVertexIndices[faceOffset + l] - vertOffset);
			}
			targetMesh.AddXyzFace(facing.MeshDataIndex);
			targetMesh.AddTextureId(decortpos.atlasTextureId);
			targetMesh.AddColorMapIndex(decorMat.ClimateMapIndex, decorMat.SeasonMapIndex);
			targetMesh.AddRenderPass((short)decorMat.RenderPass);
		}

		private void GetRelativeUV(int vIndex, out float u, out float v)
		{
			float uc = CubeMeshUtil.CubeUvCoords[vIndex];
			float vc = CubeMeshUtil.CubeUvCoords[vIndex + 1];
			switch (facing.Index)
			{
			case 0:
				u = (uc - 1f) * uSize + 1f - uOffset;
				v = (0f - vc) * vSize + 1f - vOffset;
				break;
			case 1:
				u = (uc - 1f) * uSize + 1f - uOffset;
				v = (0f - vc) * vSize + 1f - vOffset;
				break;
			case 2:
				u = uc * uSize + uOffset;
				v = (0f - vc) * vSize + 1f - vOffset;
				break;
			case 3:
				u = uc * uSize + uOffset;
				v = (0f - vc) * vSize + 1f - vOffset;
				break;
			case 4:
				u = (0f - uc) * uSize + 1f - uOffset;
				v = (vc - 1f) * vSize + 1f - vOffset;
				break;
			case 5:
				u = (uc - 1f) * uSize + 1f - uOffset;
				v = (1f - vc) * vSize + vOffset;
				break;
			default:
				throw new Exception();
			}
		}
	}

	public ref struct GenPlaneInfo
	{
		public RefList<VoxelMaterial> blockMaterials;

		public RefList<VoxelMaterial> decorMaterials;

		public unsafe VoxelInfo* voxels;

		public int materialIndex;

		public int fromA;

		public int toA;

		public int fromB;

		public int toB;

		public int c;

		public int stepA;

		public int stepB;

		public int faceOffsetZ;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetCoords(int fromA, int toA, int stepA, int fromB, int toY, int stepY, int c, int faceOffsetZ)
		{
			this.fromA = fromA;
			this.toA = toA;
			this.stepA = stepA;
			this.fromB = fromB;
			toB = toY;
			stepB = stepY;
			this.c = c;
			this.faceOffsetZ = faceOffsetZ;
		}

		public unsafe void GenPlaneMesh(ref GenFaceInfo faceGenInfo)
		{
			for (int a = fromA; a < toA; a += stepA)
			{
				int sizeB = 1;
				int index = a + fromB + c;
				bool mergable = isMergableMaterial(materialIndex, voxels[a + fromB + faceOffsetZ].Material, blockMaterials);
				for (int b = fromB + stepB; b < toB; b += stepB)
				{
					if (isMergableMaterial(materialIndex, voxels[a + b + faceOffsetZ].Material, blockMaterials) == mergable)
					{
						sizeB++;
						voxels[a + b + c].MainIndex = (ushort)index;
						continue;
					}
					voxels[index].Size = (byte)sizeB;
					voxels[index].CullFace = mergable;
					voxels[index].MainIndex = (ushort)index;
					sizeB = 1;
					index = a + b + c;
					mergable = !mergable;
				}
				voxels[index].Size = (byte)sizeB;
				voxels[index].CullFace = mergable;
				voxels[index].MainIndex = (ushort)index;
			}
			ref VoxelMaterial blockMat = ref blockMaterials[materialIndex];
			if (blockMat.BlockId == 0 && !blockMat.ForDecal)
			{
				return;
			}
			faceGenInfo.flags = faceGenInfo.facing.NormalPackedFlags | blockMat.Flags;
			for (int b = fromB; b < toB; b += stepB)
			{
				int sizeB = 1;
				int index = fromA + b + c;
				ref VoxelInfo curVoxel = ref voxels[index];
				int sizeA = curVoxel.Size;
				int prevIndex = index;
				bool mergable = curVoxel.CullFace;
				bool skip = curVoxel.MainIndex != index;
				for (int a = fromA + stepA; a < toA; a += stepA)
				{
					index = a + b + c;
					curVoxel = ref voxels[index];
					if (curVoxel.MainIndex != index)
					{
						if (skip)
						{
							continue;
						}
						skip = true;
					}
					else
					{
						if (skip)
						{
							skip = false;
							sizeB = 1;
							prevIndex = index;
							sizeA = curVoxel.Size;
							mergable = curVoxel.CullFace;
							continue;
						}
						if (mergable == curVoxel.CullFace && sizeA == curVoxel.Size)
						{
							sizeB++;
							continue;
						}
					}
					if (!mergable)
					{
						faceGenInfo.posPacked = prevIndex;
						faceGenInfo.width = sizeB;
						faceGenInfo.length = sizeA;
						faceGenInfo.GenFace(in blockMat);
					}
					sizeB = 1;
					prevIndex = index;
					sizeA = curVoxel.Size;
					mergable = curVoxel.CullFace;
				}
				if (!mergable && !skip)
				{
					faceGenInfo.posPacked = prevIndex;
					faceGenInfo.width = sizeB;
					faceGenInfo.length = sizeA;
					faceGenInfo.GenFace(in blockMat);
				}
			}
		}
	}

	public delegate void SizeConverter(int width, int height, out float sx, out float sy, out float sz);

	public readonly struct VoxelMaterial(int blockId, TextureAtlasPosition[] texture, TextureAtlasPosition[] textureInside, TextureAtlasPosition textureTopSoil, EnumChunkRenderPass renderPass, int flags, byte climateMapIndex, byte seasonMapIndex, bool frostable, bool cullBetweenTransparents, int textureRotation, bool forDecal)
	{
		public readonly int BlockId = blockId;

		public readonly bool ForDecal = forDecal;

		public readonly TextureAtlasPosition[] Texture = texture;

		public readonly TextureAtlasPosition[] TextureInside = textureInside;

		public readonly TextureAtlasPosition TextureTopSoil = textureTopSoil;

		public readonly EnumChunkRenderPass RenderPass = renderPass;

		public readonly int Flags = flags;

		public readonly bool CullBetweenTransparents = cullBetweenTransparents;

		public readonly byte ClimateMapIndex = climateMapIndex;

		public readonly bool Frostable = frostable;

		public readonly byte SeasonMapIndex = seasonMapIndex;

		public readonly int TextureRotation = textureRotation;

		public VoxelMaterial(int blockId, TextureAtlasPosition[] texture, TextureAtlasPosition[] textureInside, TextureAtlasPosition textureTopSoil, EnumChunkRenderPass renderPass, int flags, byte climateMapIndex, byte seasonMapIndex, bool frostable, bool cullBetweenTransparents)
			: this(blockId, texture, textureInside, textureTopSoil, renderPass, flags, climateMapIndex, seasonMapIndex, frostable, cullBetweenTransparents, 0, forDecal: false)
		{
		}

		public VoxelMaterial(int blockId, TextureAtlasPosition[] texture, TextureAtlasPosition[] textureInside, TextureAtlasPosition textureTopSoil, EnumChunkRenderPass renderPass, int flags, byte climateMapIndex, byte seasonMapIndex, bool frostable, bool cullBetweenTransparents, int textureRotation)
			: this(blockId, texture, textureInside, textureTopSoil, renderPass, flags, climateMapIndex, seasonMapIndex, frostable, cullBetweenTransparents, textureRotation, forDecal: false)
		{
		}

		public static VoxelMaterial FromBlock(ICoreClientAPI capi, Block block, BlockPos posForRnd = null, bool cullBetweenTransparents = false, int decorRotation = 0)
		{
			int altNum = 0;
			if (block.HasAlternates && posForRnd != null)
			{
				int altcount = 0;
				foreach (KeyValuePair<string, CompositeTexture> texture2 in block.Textures)
				{
					BakedCompositeTexture bct = texture2.Value.Baked;
					if (bct.BakedVariants != null)
					{
						altcount = Math.Max(altcount, bct.BakedVariants.Length);
					}
				}
				if (altcount > 0)
				{
					altNum = ((block.RandomizeAxes == EnumRandomizeAxes.XYZ) ? GameMath.MurmurHash3Mod(posForRnd.X, posForRnd.Y, posForRnd.Z, altcount) : GameMath.MurmurHash3Mod(posForRnd.X, 0, posForRnd.Z, altcount));
				}
			}
			TextureAtlasPosition[] texture = new TextureAtlasPosition[6];
			TextureAtlasPosition[] textureInside = new TextureAtlasPosition[6];
			TextureAtlasPosition fallbackTexture = null;
			ITexPositionSource texSource = capi.Tesselator.GetTextureSource(block, altNum, returnNullWhenMissing: true);
			BlockFacing facing;
			for (int i = 0; i < 6; textureInside[i] = texSource["inside-" + facing.Code] ?? texture[i], i++)
			{
				facing = BlockFacing.ALLFACES[i];
				if (block.HasTiles)
				{
					BakedCompositeTexture[] tiles = block.FastTextureVariants[i];
					if (tiles != null && posForRnd != null)
					{
						int positionSelector = BakedCompositeTexture.GetTiledTexturesSelector(tiles, i, posForRnd.X, posForRnd.Y, posForRnd.Z);
						int textureSubId = tiles[GameMath.Mod(positionSelector, tiles.Length)].TextureSubId;
						texture[i] = ((textureSubId >= 0) ? capi.BlockTextureAtlas.Positions[textureSubId] : capi.BlockTextureAtlas.UnknownTexturePosition);
						continue;
					}
				}
				if ((texSource[facing.Code] == null || texSource["inside-" + facing.Code] == null) && fallbackTexture == null)
				{
					fallbackTexture = capi.BlockTextureAtlas.UnknownTexturePosition;
					if (block.Textures.Count > 0)
					{
						fallbackTexture = texSource[block.Textures.First().Key] ?? capi.BlockTextureAtlas.UnknownTexturePosition;
					}
				}
				texture[i] = texSource[facing.Code] ?? fallbackTexture;
			}
			byte climateColorMapId = (byte)((block.ClimateColorMapResolved != null) ? ((byte)(block.ClimateColorMapResolved.RectIndex + 1)) : 0);
			byte seasonColorMapId = (byte)((block.SeasonColorMapResolved != null) ? ((byte)(block.SeasonColorMapResolved.RectIndex + 1)) : 0);
			TextureAtlasPosition grasscoverexPos = null;
			if (block.RenderPass == EnumChunkRenderPass.TopSoil)
			{
				grasscoverexPos = capi.BlockTextureAtlas[block.Textures["specialSecondTexture"].Baked.BakedName];
			}
			return new VoxelMaterial(block.Id, texture, textureInside, grasscoverexPos, block.RenderPass, block.VertexFlags.All, climateColorMapId, seasonColorMapId, block.Frostable, cullBetweenTransparents, decorRotation, forDecal: false);
		}

		public static VoxelMaterial FromTexSource(ICoreClientAPI capi, ITexPositionSource texSource, bool cullBetweenTransparents = false, bool forDecal = false)
		{
			TextureAtlasPosition[] texture = new TextureAtlasPosition[6];
			TextureAtlasPosition[] textureInside = new TextureAtlasPosition[6];
			for (int i = 0; i < 6; i++)
			{
				BlockFacing facing = BlockFacing.ALLFACES[i];
				texture[i] = texSource[facing.Code] ?? capi.BlockTextureAtlas.UnknownTexturePosition;
				textureInside[i] = texSource["inside-" + facing.Code] ?? texSource[facing.Code] ?? capi.BlockTextureAtlas.UnknownTexturePosition;
			}
			return new VoxelMaterial(0, texture, textureInside, null, EnumChunkRenderPass.Opaque, 0, 0, 0, frostable: false, cullBetweenTransparents, 0, forDecal);
		}
	}

	protected static ThreadLocal<CuboidWithMaterial[]> tmpCuboidTL = new ThreadLocal<CuboidWithMaterial[]>(delegate
	{
		CuboidWithMaterial[] array = new CuboidWithMaterial[4096];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new CuboidWithMaterial();
		}
		return array;
	});

	private static Cuboidf[] noSelectionBox = Array.Empty<Cuboidf>();

	private static byte[] singleByte255 = new byte[1] { 255 };

	private static Vec3f centerBase = new Vec3f(0.5f, 0f, 0.5f);

	public List<uint> VoxelCuboids = new List<uint>();

	protected uint[] originalCuboids;

	public int[] BlockIds;

	public int[] DecorIds;

	public int DecorRotations;

	protected int[] BlockIdsRotated;

	protected int[] DecorIdsRotated;

	public int rotated;

	public MeshData Mesh;

	protected Cuboidf[] selectionBoxesMetaMode;

	protected Cuboidf[] selectionBoxesStd = noSelectionBox;

	protected Cuboidf[] selectionBoxesVoxels = noSelectionBox;

	protected int prevSize = -1;

	protected int emitSideAo = 63;

	protected bool absorbAnyLight;

	public SmallBoolArray sidecenterSolid = new SmallBoolArray(0);

	public SmallBoolArray sideAlmostSolid = new SmallBoolArray(0);

	protected short rotationY;

	public float sizeRel = 1f;

	protected int totalVoxels;

	protected bool withColorMapData;

	public const int EXT_VOXELS_PER_SIDE = 18;

	public const int EXT_VOXELS_SQ = 324;

	[ThreadStatic]
	public static VoxelInfo[] tmpVoxels;

	[ThreadStatic]
	public static RefList<VoxelMaterial> tmpBlockMaterials;

	[ThreadStatic]
	public static RefList<VoxelMaterial> tmpDecorMaterials;

	private static readonly SizeConverter ConvertPlaneX = ConvertPlaneXImpl;

	private static readonly SizeConverter ConvertPlaneY = ConvertPlaneYImpl;

	private static readonly SizeConverter ConvertPlaneZ = ConvertPlaneZImpl;

	private static readonly int[] shiftOffsetByFace = new int[6] { 0, 1, 1, 0, 1, 0 };

	private static VoxelMaterial noMat = default(VoxelMaterial);

	private const int clearMaterialMask = 16777215;

	protected static CuboidWithMaterial[] tmpCuboids => tmpCuboidTL.Value;

	protected static uint[] defaultOriginalVoxelCuboids => new uint[1] { ToUint(0, 0, 0, 16, 16, 16, 0) };

	public uint[] OriginalVoxelCuboids
	{
		get
		{
			if (originalCuboids != null)
			{
				return originalCuboids;
			}
			return defaultOriginalVoxelCuboids;
		}
	}

	[Obsolete("Use BlockIds instead")]
	public int[] MaterialIds => BlockIds;

	public string BlockName { get; set; } = "";

	public float VolumeRel => (float)totalVoxels / 4096f;

	public Shape GenShape()
	{
		Shape shape = new Shape();
		CuboidWithMaterial cwm = new CuboidWithMaterial();
		shape.Elements = new ShapeElement[VoxelCuboids.Count];
		shape.Textures = new Dictionary<string, AssetLocation>();
		shape.TextureWidth = 16;
		shape.TextureHeight = 16;
		for (int i = 0; i < VoxelCuboids.Count; i++)
		{
			FromUint(VoxelCuboids[i], cwm);
			ShapeElement elem = new ShapeElement();
			elem.Name = "Cuboid" + i;
			elem.From = new double[3] { cwm.X1, cwm.Y1, cwm.Z1 };
			elem.To = new double[3] { cwm.X2, cwm.Y2, cwm.Z2 };
			Block block = Api.World.Blocks[BlockIds[cwm.Material]];
			elem.Faces = new Dictionary<string, ShapeElementFace>();
			BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
			foreach (BlockFacing facing in aLLFACES)
			{
				string texCode = block.Code.ToShortString();
				if (!block.Textures.TryGetValue(facing.Code, out var ctex))
				{
					block.Textures.TryGetValue("all", out ctex);
				}
				shape.Textures[texCode] = ctex.Base.Path.Replace("*", "1");
				elem.Faces[facing.Code] = new ShapeElementFace
				{
					Texture = texCode,
					Uv = new float[4] { 0f, 0f, 16f, 16f }
				};
			}
			shape.Elements[i] = elem;
		}
		return shape;
	}

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
	}

	public virtual void WasPlaced(Block block, string blockName)
	{
		bool collBoxCuboid = block.Attributes?.IsTrue("chiselShapeFromCollisionBox") ?? false;
		BlockIds = new int[1] { block.BlockId };
		if (!collBoxCuboid)
		{
			VoxelCuboids.Add(ToUint(0, 0, 0, 16, 16, 16, 0));
		}
		else
		{
			Cuboidf[] collboxes = block.GetCollisionBoxes(Api.World.BlockAccessor, Pos);
			originalCuboids = new uint[collboxes.Length];
			for (int i = 0; i < collboxes.Length; i++)
			{
				Cuboidf box = collboxes[i];
				uint uintbox = ToUint((int)(16f * box.X1), (int)(16f * box.Y1), (int)(16f * box.Z1), (int)(16f * box.X2), (int)(16f * box.Y2), (int)(16f * box.Z2), 0);
				VoxelCuboids.Add(uintbox);
				originalCuboids[i] = uintbox;
			}
		}
		BlockName = blockName;
		RebuildCuboidList();
		RegenSelectionBoxes(Api.World, null);
		if (Api.Side == EnumAppSide.Client && Mesh == null)
		{
			MarkMeshDirty();
		}
	}

	public override void OnBlockRemoved()
	{
		UpdateNeighbors(this);
		base.OnBlockRemoved();
	}

	public byte[] GetLightHsv(IBlockAccessor ba)
	{
		int[] matids = BlockIds;
		byte[] hsv = new byte[3];
		int q = 0;
		if (matids == null)
		{
			return hsv;
		}
		for (int i = 0; i < matids.Length; i++)
		{
			Block block = ba.GetBlock(matids[i]);
			if (block != null && block.LightHsv[2] > 0)
			{
				hsv[0] += block.LightHsv[0];
				hsv[1] += block.LightHsv[1];
				hsv[2] += block.LightHsv[2];
				q++;
			}
		}
		if (q == 0)
		{
			return hsv;
		}
		hsv[0] = (byte)(hsv[0] / q);
		hsv[1] = (byte)(hsv[1] / q);
		hsv[2] = (byte)(hsv[2] / q);
		return hsv;
	}

	public BlockSounds GetSounds()
	{
		MicroBlockSounds value = (base.Block as BlockMicroBlock).MBSounds.Value;
		value.Init(this, base.Block);
		return value;
	}

	public int GetLightAbsorption()
	{
		if (BlockIds == null || !absorbAnyLight || Api == null)
		{
			return 0;
		}
		int absorb = 99;
		for (int i = 0; i < BlockIds.Length; i++)
		{
			Block block = Api.World.GetBlock(BlockIds[i]);
			absorb = Math.Min(absorb, block.LightAbsorption);
		}
		return absorb;
	}

	public bool CanAttachBlockAt(BlockFacing blockFace, Cuboidi attachmentArea = null)
	{
		if (attachmentArea == null)
		{
			return sidecenterSolid[blockFace.Index];
		}
		HashSet<XYZ> req = new HashSet<XYZ>();
		for (int x = attachmentArea.X1; x <= attachmentArea.X2; x++)
		{
			for (int y = attachmentArea.Y1; y <= attachmentArea.Y2; y++)
			{
				for (int z = attachmentArea.Z1; z <= attachmentArea.Z2; z++)
				{
					req.Add(blockFace.Index switch
					{
						0 => new XYZ(x, y, 0), 
						1 => new XYZ(15, y, z), 
						2 => new XYZ(x, y, 15), 
						3 => new XYZ(0, y, z), 
						4 => new XYZ(x, 15, z), 
						5 => new XYZ(x, 0, z), 
						_ => new XYZ(0, 0, 0), 
					});
				}
			}
		}
		CuboidWithMaterial cwm = tmpCuboids[0];
		for (int i = 0; i < VoxelCuboids.Count; i++)
		{
			FromUint(VoxelCuboids[i], cwm);
			for (int j = cwm.X1; j < cwm.X2; j++)
			{
				for (int k = cwm.Y1; k < cwm.Y2; k++)
				{
					for (int l = cwm.Z1; l < cwm.Z2; l++)
					{
						if (j == 0 || j == 15 || k == 0 || k == 15 || l == 0 || l == 15)
						{
							req.Remove(new XYZ(j, k, l));
						}
					}
				}
			}
		}
		return req.Count == 0;
	}

	public virtual Cuboidf[] GetSelectionBoxes(IBlockAccessor world, BlockPos pos, IPlayer forPlayer = null)
	{
		if (selectionBoxesMetaMode != null && Api.Side == EnumAppSide.Client && (Api as ICoreClientAPI).Settings.Bool["renderMetaBlocks"])
		{
			return selectionBoxesMetaMode;
		}
		if (selectionBoxesStd.Length == 0 && selectionBoxesMetaMode == null)
		{
			return new Cuboidf[1] { Cuboidf.Default() };
		}
		return selectionBoxesStd;
	}

	public Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		if (selectionBoxesMetaMode != null)
		{
			return selectionBoxesMetaMode;
		}
		return selectionBoxesStd;
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		base.GetBlockInfo(forPlayer, dsc);
		string blockName = BlockName;
		if (blockName != null && blockName.IndexOf('\n') > 0)
		{
			dsc.AppendLine(Lang.Get(BlockName.Substring(BlockName.IndexOf('\n') + 1)));
		}
		else if (forPlayer.Entity?.RightHandItemSlot?.Itemstack?.Collectible is ItemChisel)
		{
			dsc.AppendLine(Lang.Get("block-chiseledblock"));
		}
		if (forPlayer?.CurrentBlockSelection?.Face != null && BlockIds != null)
		{
			EnumBlockMaterial mat = Api.World.GetBlock(BlockIds[0]).BlockMaterial;
			if ((mat == EnumBlockMaterial.Ore || mat == EnumBlockMaterial.Stone || mat == EnumBlockMaterial.Soil || mat == EnumBlockMaterial.Ceramic) && (sideAlmostSolid[forPlayer.CurrentBlockSelection.Face.Index] || (sideAlmostSolid[forPlayer.CurrentBlockSelection.Face.Opposite.Index] && VolumeRel >= 0.5f)))
			{
				dsc.AppendLine(Lang.Get("Insulating block face"));
			}
		}
	}

	public string GetPlacedBlockName()
	{
		return GetPlacedBlockName(Api, VoxelCuboids, BlockIds, BlockName);
	}

	public static string GetPlacedBlockName(ICoreAPI api, List<uint> voxelCuboids, int[] blockIds, string blockName)
	{
		if ((blockName == null || blockName == "") && blockIds != null)
		{
			int mblockid = getMajorityMaterial(voxelCuboids, blockIds);
			Block block = api.World.Blocks[mblockid];
			return block.GetHeldItemName(new ItemStack(block));
		}
		int nind = blockName.IndexOf('\n');
		return Lang.Get((nind > 0) ? blockName.Substring(0, nind) : blockName);
	}

	public int GetMajorityMaterialId(ActionBoolReturn<int> filterblockId = null)
	{
		return getMajorityMaterial(VoxelCuboids, BlockIds, filterblockId);
	}

	public static int getMajorityMaterial(List<uint> voxelCuboids, int[] blockIds, ActionBoolReturn<int> filterblockId = null)
	{
		Dictionary<int, int> volumeByBlockid = new Dictionary<int, int>();
		CuboidWithMaterial cwm = new CuboidWithMaterial();
		for (int i = 0; i < voxelCuboids.Count; i++)
		{
			FromUint(voxelCuboids[i], cwm);
			if (blockIds.Length > cwm.Material)
			{
				int blockId = blockIds[cwm.Material];
				if (volumeByBlockid.ContainsKey(blockId))
				{
					volumeByBlockid[blockId] += cwm.SizeXYZ;
				}
				else
				{
					volumeByBlockid[blockId] = cwm.SizeXYZ;
				}
			}
		}
		if (volumeByBlockid.Count == 0)
		{
			return 0;
		}
		if (filterblockId != null)
		{
			volumeByBlockid = volumeByBlockid.Where((KeyValuePair<int, int> vbb) => filterblockId?.Invoke(vbb.Key) ?? false).ToDictionary((KeyValuePair<int, int> kv) => kv.Key, (KeyValuePair<int, int> kv) => kv.Value);
		}
		if (volumeByBlockid.Count == 0)
		{
			return 0;
		}
		return volumeByBlockid.MaxBy((KeyValuePair<int, int> vbb) => vbb.Value).Key;
	}

	public void ConvertToVoxels(out BoolArray16x16x16 voxels, out byte[,,] materials)
	{
		voxels = new BoolArray16x16x16();
		materials = new byte[16, 16, 16];
		CuboidWithMaterial cwm = tmpCuboids[0];
		for (int i = 0; i < VoxelCuboids.Count; i++)
		{
			FromUint(VoxelCuboids[i], cwm);
			for (int dx = cwm.X1; dx < cwm.X2; dx++)
			{
				for (int dy = cwm.Y1; dy < cwm.Y2; dy++)
				{
					for (int dz = cwm.Z1; dz < cwm.Z2; dz++)
					{
						voxels[dx, dy, dz] = true;
						materials[dx, dy, dz] = cwm.Material;
					}
				}
			}
		}
	}

	public void RebuildCuboidList()
	{
		ConvertToVoxels(out var Voxels, out var VoxelMaterial);
		RebuildCuboidList(Voxels, VoxelMaterial);
	}

	public void FlipVoxels(BlockFacing frontFacing)
	{
		ConvertToVoxels(out var Voxels, out var VoxelMaterial);
		BoolArray16x16x16 outVoxels = new BoolArray16x16x16();
		byte[,,] outVoxelMaterial = new byte[16, 16, 16];
		for (int dx = 0; dx < 16; dx++)
		{
			for (int dy = 0; dy < 16; dy++)
			{
				for (int dz = 0; dz < 16; dz++)
				{
					outVoxels[dx, dy, dz] = Voxels[(frontFacing.Axis == EnumAxis.Z) ? (15 - dx) : dx, dy, (frontFacing.Axis == EnumAxis.X) ? (15 - dz) : dz];
					outVoxelMaterial[dx, dy, dz] = VoxelMaterial[(frontFacing.Axis == EnumAxis.Z) ? (15 - dx) : dx, dy, (frontFacing.Axis == EnumAxis.X) ? (15 - dz) : dz];
				}
			}
		}
		RebuildCuboidList(outVoxels, outVoxelMaterial);
	}

	public void TransformList(int degrees, EnumAxis? flipAroundAxis, List<uint> list)
	{
		CuboidWithMaterial cwm = tmpCuboids[0];
		Vec3d axis = new Vec3d(8.0, 8.0, 8.0);
		for (int i = 0; i < list.Count; i++)
		{
			FromUint(list[i], cwm);
			if (flipAroundAxis == EnumAxis.X)
			{
				cwm.X1 = 16 - cwm.X1;
				cwm.X2 = 16 - cwm.X2;
			}
			if (flipAroundAxis == EnumAxis.Y)
			{
				cwm.Y1 = 16 - cwm.Y1;
				cwm.Y2 = 16 - cwm.Y2;
			}
			if (flipAroundAxis == EnumAxis.Z)
			{
				cwm.Z1 = 16 - cwm.Z1;
				cwm.Z2 = 16 - cwm.Z2;
			}
			Cuboidi rotated = cwm.RotatedCopy(0, -degrees, 0, axis);
			cwm.Set(rotated.X1, rotated.Y1, rotated.Z1, rotated.X2, rotated.Y2, rotated.Z2);
			list[i] = ToUint(cwm);
		}
	}

	public void RotateModel(int degrees, EnumAxis? flipAroundAxis)
	{
		TransformList(degrees, flipAroundAxis, VoxelCuboids);
		foreach (BlockEntityBehavior behavior in Behaviors)
		{
			if (behavior is IMicroblockBehavior bebhmicroblock)
			{
				bebhmicroblock.RotateModel(degrees, flipAroundAxis);
			}
		}
		if (flipAroundAxis.HasValue)
		{
			if (originalCuboids != null)
			{
				List<uint> origCubs = new List<uint>(originalCuboids);
				TransformList(degrees, flipAroundAxis, origCubs);
				originalCuboids = origCubs.ToArray();
			}
			int shift = -degrees / 90;
			SmallBoolArray prevSolid = sidecenterSolid;
			SmallBoolArray prevAlmostSolid = sideAlmostSolid;
			for (int i = 0; i < 4; i++)
			{
				sidecenterSolid[i] = prevSolid[GameMath.Mod(i + shift, 4)];
				sideAlmostSolid[i] = prevAlmostSolid[GameMath.Mod(i + shift, 4)];
			}
		}
		Api?.World.BlockAccessor.TriggerNeighbourBlockUpdate(Pos);
		rotationY = (short)((rotationY + degrees) % 360);
	}

	public void OnTransformed(IWorldAccessor worldAccessor, ITreeAttribute tree, int byDegrees, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, EnumAxis? flipAroundAxis)
	{
		uint[] cuboidValues = (tree["cuboids"] as IntArrayAttribute)?.AsUint;
		VoxelCuboids = ((cuboidValues == null) ? new List<uint>(0) : new List<uint>(cuboidValues));
		RotateModel(byDegrees, flipAroundAxis);
		tree["cuboids"] = new IntArrayAttribute(VoxelCuboids.ToArray());
		int[] materialIds = (tree["materials"] as IntArrayAttribute)?.value;
		if (materialIds != null)
		{
			int[] newMaterialIds = new int[materialIds.Length];
			for (int i = 0; i < materialIds.Length; i++)
			{
				int materialId = materialIds[i];
				if (oldBlockIdMapping.TryGetValue(materialId, out var code))
				{
					Block block = worldAccessor.GetBlock(code);
					if (block != null)
					{
						AssetLocation assetLocation = block.GetRotatedBlockCode(byDegrees);
						Block newBlock = worldAccessor.GetBlock(assetLocation);
						newMaterialIds[i] = newBlock.Id;
					}
					else
					{
						newMaterialIds[i] = materialId;
						worldAccessor.Logger.Warning("Cannot load chiseled block id mapping for rotation @ {1}, block id {0} not found block registry. Will not display correctly.", code, Pos);
					}
				}
				else
				{
					newMaterialIds[i] = materialId;
					if (materialId >= worldAccessor.Blocks.Count)
					{
						worldAccessor.Logger.Warning("Cannot load chiseled block id mapping for rotation @ {1}, block code {0} not found block registry. Will not display correctly.", materialId, Pos);
					}
				}
			}
			tree["materials"] = new IntArrayAttribute(newMaterialIds);
		}
		foreach (BlockEntityBehavior behavior in Behaviors)
		{
			if (behavior is IRotatable bhrot)
			{
				bhrot.OnTransformed(worldAccessor, tree, byDegrees, oldBlockIdMapping, oldItemIdMapping, flipAroundAxis);
			}
		}
		tree.RemoveAttribute("sideAlmostSolid");
	}

	public int GetVoxelMaterialAt(Vec3i voxelPos)
	{
		ConvertToVoxels(out var Voxels, out var VoxelMaterial);
		if (Voxels[voxelPos.X, voxelPos.Y, voxelPos.Z])
		{
			return BlockIds[VoxelMaterial[voxelPos.X, voxelPos.Y, voxelPos.Z]];
		}
		return 0;
	}

	public bool SetVoxel(Vec3i voxelPos, bool state, byte materialId, int size)
	{
		ConvertToVoxels(out var Voxels, out var VoxelMaterial);
		bool wasChanged = false;
		int endx = voxelPos.X + size;
		int endy = voxelPos.Y + size;
		int endz = voxelPos.Z + size;
		for (int x = voxelPos.X; x < endx; x++)
		{
			for (int y = voxelPos.Y; y < endy; y++)
			{
				for (int z = voxelPos.Z; z < endz; z++)
				{
					if (x < 16 && y < 16 && z < 16)
					{
						if (state)
						{
							wasChanged |= !Voxels[x, y, z] || VoxelMaterial[x, y, z] != materialId;
							Voxels[x, y, z] = true;
							VoxelMaterial[x, y, z] = materialId;
						}
						else
						{
							wasChanged |= Voxels[x, y, z];
							Voxels[x, y, z] = false;
						}
					}
				}
			}
		}
		if (!wasChanged)
		{
			return false;
		}
		RebuildCuboidList(Voxels, VoxelMaterial);
		Api.World.BlockAccessor.TriggerNeighbourBlockUpdate(Pos);
		return true;
	}

	public void BeginEdit(out BoolArray16x16x16 voxels, out byte[,,] voxelMaterial)
	{
		ConvertToVoxels(out voxels, out voxelMaterial);
	}

	public void EndEdit(BoolArray16x16x16 voxels, byte[,,] voxelMaterial)
	{
		RebuildCuboidList(voxels, voxelMaterial);
		Api.World.BlockAccessor.TriggerNeighbourBlockUpdate(Pos);
	}

	public void SetData(BoolArray16x16x16 Voxels, byte[,,] VoxelMaterial)
	{
		RebuildCuboidList(Voxels, VoxelMaterial);
		if (Api.Side == EnumAppSide.Client)
		{
			MarkMeshDirty();
		}
		RegenSelectionBoxes(Api.World, null);
		MarkDirty(redrawOnClient: true);
		if (VoxelCuboids.Count == 0)
		{
			Api.World.BlockAccessor.SetBlock(0, Pos);
		}
	}

	public bool DoEmitSideAo(int facing)
	{
		int opposite = TileSideEnum.GetOpposite(facing);
		return (emitSideAo & (63 - (1 << opposite))) != 0;
	}

	public bool DoEmitSideAoByFlag(int flag)
	{
		return (emitSideAo & flag) != 0;
	}

	public override void HistoryStateRestore()
	{
		RebuildCuboidList();
		MarkDirty(redrawOnClient: true);
	}

	protected void RebuildCuboidList(BoolArray16x16x16 Voxels, byte[,,] VoxelMaterial)
	{
		BoolArray16x16x16 VoxelVisited = new BoolArray16x16x16();
		int prevEmitSideAo = emitSideAo;
		emitSideAo = 63;
		sidecenterSolid = new SmallBoolArray(63);
		float voxelCount = 0f;
		List<uint> voxelCuboids = new List<uint>();
		int[] edgeVoxelsMissing = new int[6];
		int[] edgeCenterVoxelsMissing = new int[6];
		byte[] lightshv = GetLightHsv(Api.World.BlockAccessor);
		for (int dx = 0; dx < 16; dx++)
		{
			for (int dy = 0; dy < 16; dy++)
			{
				for (int dz = 0; dz < 16; dz++)
				{
					if (!Voxels[dx, dy, dz])
					{
						if (dz == 0)
						{
							edgeVoxelsMissing[BlockFacing.NORTH.Index]++;
							if (Math.Abs(dy - 8) < 5 && Math.Abs(dx - 8) < 5)
							{
								edgeCenterVoxelsMissing[BlockFacing.NORTH.Index]++;
							}
						}
						if (dx == 15)
						{
							edgeVoxelsMissing[BlockFacing.EAST.Index]++;
							if (Math.Abs(dy - 8) < 5 && Math.Abs(dz - 8) < 5)
							{
								edgeCenterVoxelsMissing[BlockFacing.EAST.Index]++;
							}
						}
						if (dz == 15)
						{
							edgeVoxelsMissing[BlockFacing.SOUTH.Index]++;
							if (Math.Abs(dy - 8) < 5 && Math.Abs(dx - 8) < 5)
							{
								edgeCenterVoxelsMissing[BlockFacing.SOUTH.Index]++;
							}
						}
						if (dx == 0)
						{
							edgeVoxelsMissing[BlockFacing.WEST.Index]++;
							if (Math.Abs(dy - 8) < 5 && Math.Abs(dz - 8) < 5)
							{
								edgeCenterVoxelsMissing[BlockFacing.WEST.Index]++;
							}
						}
						if (dy == 15)
						{
							edgeVoxelsMissing[BlockFacing.UP.Index]++;
							if (Math.Abs(dz - 8) < 5 && Math.Abs(dx - 8) < 5)
							{
								edgeCenterVoxelsMissing[BlockFacing.UP.Index]++;
							}
						}
						if (dy == 0)
						{
							edgeVoxelsMissing[BlockFacing.DOWN.Index]++;
							if (Math.Abs(dz - 8) < 5 && Math.Abs(dx - 8) < 5)
							{
								edgeCenterVoxelsMissing[BlockFacing.DOWN.Index]++;
							}
						}
						continue;
					}
					voxelCount += 1f;
					if (!VoxelVisited[dx, dy, dz])
					{
						CuboidWithMaterial cub = new CuboidWithMaterial
						{
							Material = VoxelMaterial[dx, dy, dz],
							X1 = dx,
							Y1 = dy,
							Z1 = dz,
							X2 = dx + 1,
							Y2 = dy + 1,
							Z2 = dz + 1
						};
						bool didGrowAny = true;
						while (didGrowAny)
						{
							didGrowAny = false;
							didGrowAny |= TryGrowX(cub, Voxels, VoxelVisited, VoxelMaterial);
							didGrowAny |= TryGrowY(cub, Voxels, VoxelVisited, VoxelMaterial);
							didGrowAny |= TryGrowZ(cub, Voxels, VoxelVisited, VoxelMaterial);
						}
						voxelCuboids.Add(ToUint(cub));
					}
				}
			}
		}
		VoxelCuboids = voxelCuboids;
		bool doEmitSideAo = edgeVoxelsMissing[0] < 64 || edgeVoxelsMissing[1] < 64 || edgeVoxelsMissing[2] < 64 || edgeVoxelsMissing[3] < 64 || edgeVoxelsMissing[4] < 64 || edgeVoxelsMissing[5] < 64;
		if (absorbAnyLight != doEmitSideAo)
		{
			int preva = GetLightAbsorption();
			absorbAnyLight = doEmitSideAo;
			int nowa = GetLightAbsorption();
			if (preva != nowa)
			{
				Api.World.BlockAccessor.MarkAbsorptionChanged(preva, nowa, Pos);
			}
		}
		int emitFlags = 0;
		for (int i = 0; i < 6; i++)
		{
			sidecenterSolid[i] = edgeCenterVoxelsMissing[i] < 5;
			if (sideAlmostSolid[i] = edgeVoxelsMissing[i] <= 32)
			{
				emitFlags += 1 << i;
			}
		}
		emitSideAo = ((lightshv[2] < 10 && doEmitSideAo) ? emitFlags : 0);
		if (BlockIds.Length == 1 && Api.World.GetBlock(BlockIds[0]).RenderPass == EnumChunkRenderPass.Meta)
		{
			emitSideAo = 0;
		}
		if (Api.Side == EnumAppSide.Client)
		{
			RedrawNeighbourChunkIfAoChanged(prevEmitSideAo);
		}
		sizeRel = voxelCount / 4096f;
		foreach (BlockEntityBehavior behavior in Behaviors)
		{
			if (behavior is IMicroblockBehavior bebhmicroblock)
			{
				bebhmicroblock.RebuildCuboidList(Voxels, VoxelMaterial);
			}
		}
		if (DisplacesLiquid())
		{
			Api.World.BlockAccessor.SetBlock(0, Pos, 2);
		}
	}

	private void RedrawNeighbourChunkIfAoChanged(int prevEmitSideAo)
	{
		prevEmitSideAo ^= emitSideAo;
		if (prevEmitSideAo == 0)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			if ((prevEmitSideAo & 1) != 0)
			{
				Api.World.BlockAccessor.RedrawNeighbouringChunk(Pos, BlockFacing.ALLFACES[i]);
			}
			prevEmitSideAo >>= 1;
		}
	}

	public bool DisplacesLiquid()
	{
		if (sideAlmostSolid[0] && sideAlmostSolid[1] && sideAlmostSolid[2] && sideAlmostSolid[3])
		{
			return sideAlmostSolid[5];
		}
		return false;
	}

	protected bool TryGrowX(CuboidWithMaterial cub, BoolArray16x16x16 voxels, BoolArray16x16x16 voxelVisited, byte[,,] voxelMaterial)
	{
		if (cub.X2 > 15)
		{
			return false;
		}
		for (int y = cub.Y1; y < cub.Y2; y++)
		{
			for (int z = cub.Z1; z < cub.Z2; z++)
			{
				if (!voxels[cub.X2, y, z] || voxelVisited[cub.X2, y, z] || voxelMaterial[cub.X2, y, z] != cub.Material)
				{
					return false;
				}
			}
		}
		for (int i = cub.Y1; i < cub.Y2; i++)
		{
			for (int j = cub.Z1; j < cub.Z2; j++)
			{
				voxelVisited[cub.X2, i, j] = true;
			}
		}
		cub.X2++;
		return true;
	}

	protected bool TryGrowY(CuboidWithMaterial cub, BoolArray16x16x16 voxels, BoolArray16x16x16 voxelVisited, byte[,,] voxelMaterial)
	{
		if (cub.Y2 > 15)
		{
			return false;
		}
		for (int x = cub.X1; x < cub.X2; x++)
		{
			for (int z = cub.Z1; z < cub.Z2; z++)
			{
				if (!voxels[x, cub.Y2, z] || voxelVisited[x, cub.Y2, z] || voxelMaterial[x, cub.Y2, z] != cub.Material)
				{
					return false;
				}
			}
		}
		for (int i = cub.X1; i < cub.X2; i++)
		{
			for (int j = cub.Z1; j < cub.Z2; j++)
			{
				voxelVisited[i, cub.Y2, j] = true;
			}
		}
		cub.Y2++;
		return true;
	}

	protected bool TryGrowZ(CuboidWithMaterial cub, BoolArray16x16x16 voxels, BoolArray16x16x16 voxelVisited, byte[,,] voxelMaterial)
	{
		if (cub.Z2 > 15)
		{
			return false;
		}
		for (int x = cub.X1; x < cub.X2; x++)
		{
			for (int y = cub.Y1; y < cub.Y2; y++)
			{
				if (!voxels[x, y, cub.Z2] || voxelVisited[x, y, cub.Z2] || voxelMaterial[x, y, cub.Z2] != cub.Material)
				{
					return false;
				}
			}
		}
		for (int i = cub.X1; i < cub.X2; i++)
		{
			for (int j = cub.Y1; j < cub.Y2; j++)
			{
				voxelVisited[i, j, cub.Z2] = true;
			}
		}
		cub.Z2++;
		return true;
	}

	public virtual void RegenSelectionBoxes(IWorldAccessor worldForResolve, IPlayer byPlayer)
	{
		Cuboidf[] selectionBoxesStdTmp = new Cuboidf[VoxelCuboids.Count];
		CuboidWithMaterial cwm = tmpCuboids[0];
		totalVoxels = 0;
		List<Cuboidf> selBoxesMetaModeTmp = null;
		bool hasMetaBlock = false;
		for (int i = 0; i < BlockIds.Length; i++)
		{
			hasMetaBlock |= worldForResolve.Blocks[BlockIds[i]].RenderPass == EnumChunkRenderPass.Meta;
		}
		if (hasMetaBlock)
		{
			selBoxesMetaModeTmp = new List<Cuboidf>();
			for (int j = 0; j < VoxelCuboids.Count; j++)
			{
				FromUint(VoxelCuboids[j], cwm);
				Cuboidf cub = cwm.ToCuboidf();
				selBoxesMetaModeTmp.Add(cub);
				Block block = worldForResolve.Blocks[BlockIds[cwm.Material]];
				if (block.RenderPass == EnumChunkRenderPass.Meta)
				{
					IMetaBlock metaBlock = block.GetInterface<IMetaBlock>(worldForResolve, Pos);
					if (metaBlock == null || !metaBlock.IsSelectable(Pos))
					{
						continue;
					}
				}
				selectionBoxesStdTmp[j] = cub;
				totalVoxels += cwm.Volume;
			}
			selectionBoxesStd = selectionBoxesStdTmp.Where((Cuboidf ele) => ele != null).ToArray();
			selectionBoxesMetaMode = selBoxesMetaModeTmp.ToArray();
		}
		else
		{
			for (int i2 = 0; i2 < VoxelCuboids.Count; i2++)
			{
				FromUint(VoxelCuboids[i2], cwm);
				selectionBoxesStdTmp[i2] = cwm.ToCuboidf();
				totalVoxels += cwm.Volume;
			}
			selectionBoxesStd = selectionBoxesStdTmp;
			selectionBoxesMetaMode = null;
		}
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		BlockIds = MaterialIdsFromAttributes(tree, worldAccessForResolve);
		DecorIds = (tree["decorIds"] as IntArrayAttribute)?.value;
		BlockName = tree.GetString("blockName");
		int rot = tree.GetInt("rotation");
		rotationY = (short)(((rot >> 10) & 0x3FF) - 360);
		DecorRotations = tree.GetInt("decorRot");
		VoxelCuboids = new List<uint>(GetVoxelCuboids(tree));
		int prevEmitSideAo = emitSideAo;
		byte[] sideAo = tree.GetBytes("emitSideAo", singleByte255);
		if (sideAo.Length != 0)
		{
			emitSideAo = sideAo[0];
			absorbAnyLight = emitSideAo != 0;
		}
		byte[] sideSolid = tree.GetBytes("sideSolid", singleByte255);
		if (sideSolid.Length != 0)
		{
			sidecenterSolid = new SmallBoolArray(sideSolid[0] & 0x3F);
		}
		byte[] sideAlmostSolid = tree.GetBytes("sideAlmostSolid", singleByte255);
		if (sideAlmostSolid.Length != 0)
		{
			this.sideAlmostSolid = new SmallBoolArray(sideAlmostSolid[0] & 0x3F);
		}
		if (tree.HasAttribute("originalCuboids"))
		{
			originalCuboids = (tree["originalCuboids"] as IntArrayAttribute)?.AsUint;
		}
		if (worldAccessForResolve.Side == EnumAppSide.Client)
		{
			if (Api != null)
			{
				Mesh = GenMesh();
				Api.World.BlockAccessor.MarkBlockModified(Pos);
				RedrawNeighbourChunkIfAoChanged(prevEmitSideAo);
			}
			int lx = Pos.X % 32;
			int lz = Pos.X % 32;
			if (Api != null)
			{
				UpdateNeighbors(this);
			}
			else if (lx == 0 || lx == 31 || lz == 0 || lz == 31)
			{
				if (lx == 0)
				{
					UpdateNeighbour(worldAccessForResolve, Pos.WestCopy());
				}
				if (lz == 0)
				{
					UpdateNeighbour(worldAccessForResolve, Pos.NorthCopy());
				}
				if (lx == 31)
				{
					UpdateNeighbour(worldAccessForResolve, Pos.EastCopy());
				}
				if (lz == 31)
				{
					UpdateNeighbour(worldAccessForResolve, Pos.SouthCopy());
				}
			}
		}
		else if (!tree.HasAttribute("sideAlmostSolid"))
		{
			if (Api == null)
			{
				Api = worldAccessForResolve.Api;
			}
			RebuildCuboidList();
		}
		RegenSelectionBoxes(worldAccessForResolve, null);
	}

	public static uint[] GetVoxelCuboids(ITreeAttribute tree)
	{
		uint[] values = (tree["cuboids"] as IntArrayAttribute)?.AsUint;
		if (values == null)
		{
			values = (tree["cuboids"] as LongArrayAttribute)?.AsUint;
		}
		if (values == null)
		{
			values = new uint[1] { ToUint(0, 0, 0, 16, 16, 16, 0) };
		}
		return values;
	}

	public static int[] MaterialIdsFromAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		if (tree["materials"] is IntArrayAttribute materialsArray)
		{
			return materialsArray.value;
		}
		if (!(tree["materials"] is StringArrayAttribute))
		{
			return new int[1] { worldAccessForResolve.GetBlock(new AssetLocation("rock-granite")).Id };
		}
		string[] codes = (tree["materials"] as StringArrayAttribute).value;
		int[] ids = new int[codes.Length];
		for (int i = 0; i < ids.Length; i++)
		{
			Block block = worldAccessForResolve.GetBlock(new AssetLocation(codes[i]));
			if (block == null)
			{
				block = worldAccessForResolve.GetBlock(new AssetLocation(codes[i] + "-free"));
				if (block == null)
				{
					block = worldAccessForResolve.GetBlock(new AssetLocation("rock-granite"));
				}
			}
			ids[i] = block.BlockId;
		}
		return ids;
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		if (BlockIds != null)
		{
			tree["materials"] = new IntArrayAttribute(BlockIds);
		}
		if (DecorIds != null)
		{
			tree["decorIds"] = new IntArrayAttribute(DecorIds);
		}
		tree.SetInt("decorRot", DecorRotations);
		tree.SetInt("rotation", rotationY + 360 << 10);
		tree["cuboids"] = new IntArrayAttribute(VoxelCuboids.ToArray());
		tree.SetBytes("emitSideAo", new byte[1] { (byte)emitSideAo });
		tree.SetBytes("sideSolid", new byte[1] { (byte)sidecenterSolid.Value() });
		tree.SetBytes("sideAlmostSolid", new byte[1] { (byte)sideAlmostSolid.Value() });
		tree.SetString("blockName", BlockName);
		if (originalCuboids != null)
		{
			tree["originalCuboids"] = new IntArrayAttribute(originalCuboids);
		}
	}

	public static uint ToUint(int minx, int miny, int minz, int maxx, int maxy, int maxz, int material)
	{
		return (uint)(minx | (miny << 4) | (minz << 8) | (maxx - 1 << 12) | (maxy - 1 << 16) | (maxz - 1 << 20) | (material << 24));
	}

	public static uint ToUint(CuboidWithMaterial cub)
	{
		return (uint)(cub.X1 | (cub.Y1 << 4) | (cub.Z1 << 8) | (cub.X2 - 1 << 12) | (cub.Y2 - 1 << 16) | (cub.Z2 - 1 << 20) | (cub.Material << 24));
	}

	public static void FromUint(uint val, CuboidWithMaterial tocuboid)
	{
		tocuboid.X1 = (int)(val & 0xF);
		tocuboid.Y1 = (int)((val >> 4) & 0xF);
		tocuboid.Z1 = (int)((val >> 8) & 0xF);
		tocuboid.X2 = (int)(((val >> 12) & 0xF) + 1);
		tocuboid.Y2 = (int)(((val >> 16) & 0xF) + 1);
		tocuboid.Z2 = (int)(((val >> 20) & 0xF) + 1);
		tocuboid.Material = (byte)((val >> 24) & 0xFF);
	}

	public override void OnLoadCollectibleMappings(IWorldAccessor worldForNewMappings, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, int schematicSeed, bool resolveImports)
	{
		base.OnLoadCollectibleMappings(worldForNewMappings, oldBlockIdMapping, oldItemIdMapping, schematicSeed, resolveImports);
		for (int i = 0; i < BlockIds.Length; i++)
		{
			if (oldBlockIdMapping != null && oldBlockIdMapping.TryGetValue(BlockIds[i], out var code))
			{
				Block block = worldForNewMappings.GetBlock(code);
				if (block == null)
				{
					worldForNewMappings.Logger.Warning("Cannot load chiseled block id mapping @ {1}, block code {0} not found block registry. Will not display correctly.", code, Pos);
				}
				else
				{
					BlockIds[i] = block.Id;
				}
			}
			else if (worldForNewMappings.GetBlock(BlockIds[i]) == null)
			{
				worldForNewMappings.Logger.Warning("Cannot load chiseled block id mapping @ {1}, block id {0} not found block registry. Will not display correctly.", BlockIds[i], Pos);
			}
		}
		if (DecorIds == null)
		{
			return;
		}
		for (int j = 0; j < DecorIds.Length; j++)
		{
			if (oldBlockIdMapping.TryGetValue(DecorIds[j], out var code2))
			{
				Block block2 = worldForNewMappings.GetBlock(code2);
				if (block2 == null)
				{
					worldForNewMappings.Logger.Warning("Cannot load chiseled decor block id mapping @ {1}, block code {0} not found block registry. Will not display correctly.", code2, Pos);
				}
				else
				{
					DecorIds[j] = block2.Id;
				}
			}
			else
			{
				worldForNewMappings.Logger.Warning("Cannot load chiseled decor block id mapping @ {1}, block id {0} not found block registry. Will not display correctly.", DecorIds[j], Pos);
			}
		}
	}

	public override void OnStoreCollectibleMappings(Dictionary<int, AssetLocation> blockIdMapping, Dictionary<int, AssetLocation> itemIdMapping)
	{
		base.OnStoreCollectibleMappings(blockIdMapping, itemIdMapping);
		for (int i = 0; i < BlockIds.Length; i++)
		{
			Block block = Api.World.GetBlock(BlockIds[i]);
			if (!(block.Code == null))
			{
				blockIdMapping[BlockIds[i]] = block.Code;
			}
		}
		if (DecorIds == null)
		{
			return;
		}
		for (int j = 0; j < DecorIds.Length; j++)
		{
			Block block2 = Api.World.GetBlock(DecorIds[j]);
			if (!(block2.Code == null))
			{
				blockIdMapping[DecorIds[j]] = block2.Code;
			}
		}
	}

	public bool NoVoxelsWithMaterial(uint index)
	{
		foreach (uint voxelCuboid in VoxelCuboids)
		{
			uint material = (voxelCuboid >> 24) & 0xFF;
			if (index == material)
			{
				return false;
			}
		}
		return true;
	}

	public virtual bool RemoveMaterial(Block block)
	{
		if (BlockIds.Contains(block.Id))
		{
			int index = BlockIds.IndexOf(block.Id);
			BlockIds = BlockIds.Remove(block.Id);
			for (int i = 0; i < VoxelCuboids.Count; i++)
			{
				int material = (int)((VoxelCuboids[i] >> 24) & 0xFF);
				if (index == material)
				{
					VoxelCuboids.RemoveAt(i);
					i--;
				}
			}
			ShiftMaterialIndicesAt(index);
			return true;
		}
		return false;
	}

	private void ShiftMaterialIndicesAt(int index)
	{
		for (int j = 0; j < VoxelCuboids.Count; j++)
		{
			uint material = (VoxelCuboids[j] >> 24) & 0xFF;
			if (material >= index)
			{
				VoxelCuboids[j] = (VoxelCuboids[j] & 0xFFFFFF) | (material - 1 << 24);
			}
		}
	}

	public MeshData GenMesh()
	{
		int[] blockIds = BlockIds;
		if (blockIds == null)
		{
			return null;
		}
		GenRotatedMaterialIds();
		MeshData mesh = CreateMesh(Api as ICoreClientAPI, VoxelCuboids, BlockIdsRotated, DecorIdsRotated, DecorRotations, OriginalVoxelCuboids, Pos);
		foreach (BlockEntityBehavior behavior in Behaviors)
		{
			if (behavior is IMicroblockBehavior bebhmicroblock)
			{
				bebhmicroblock.RegenMesh();
			}
		}
		withColorMapData = false;
		for (int i = 0; i < blockIds.Length; i++)
		{
			if (Api.World.Blocks[blockIds[i]].ClimateColorMapResolved != null)
			{
				withColorMapData = true;
				break;
			}
		}
		if (!withColorMapData && DecorIds != null)
		{
			int[] decorIds = DecorIds;
			for (int j = 0; j < decorIds.Length; j++)
			{
				if (Api.World.Blocks[decorIds[j]].ClimateColorMapResolved != null)
				{
					withColorMapData = true;
					break;
				}
			}
		}
		return mesh;
	}

	private void GenRotatedMaterialIds()
	{
		if (rotationY == 0)
		{
			BlockIdsRotated = BlockIds;
			DecorIdsRotated = DecorIds;
			return;
		}
		if (BlockIdsRotated == null || BlockIdsRotated.Length < BlockIds.Length)
		{
			BlockIdsRotated = new int[BlockIds.Length];
		}
		for (int i = 0; i < BlockIds.Length; i++)
		{
			int id = BlockIds[i];
			AssetLocation rotatedBlockCode = Api.World.GetBlock(id).GetRotatedBlockCode(rotationY);
			Block? obj = ((rotatedBlockCode == null) ? null : Api.World.GetBlock(rotatedBlockCode));
			BlockIdsRotated[i] = obj?.Id ?? id;
		}
		if (DecorIds != null)
		{
			if (DecorIdsRotated == null || DecorIdsRotated.Length < DecorIds.Length)
			{
				DecorIdsRotated = new int[DecorIds.Length];
			}
			for (int j = 0; j < 4; j++)
			{
				DecorIdsRotated[j] = DecorIds[GameMath.Mod(j + rotationY / 90, 4)];
			}
			DecorIdsRotated[4] = DecorIds[4];
			DecorIdsRotated[5] = DecorIds[5];
		}
	}

	public void RegenMesh(ICoreClientAPI capi)
	{
		GenRotatedMaterialIds();
		Mesh = CreateMesh(capi, VoxelCuboids, BlockIdsRotated, DecorIdsRotated, DecorRotations, OriginalVoxelCuboids, Pos);
	}

	public void MarkMeshDirty()
	{
		Mesh = null;
	}

	private static RefList<VoxelMaterial> getOrCreateBlockMatRefList()
	{
		return tmpBlockMaterials ?? (tmpBlockMaterials = new RefList<VoxelMaterial>());
	}

	private static RefList<VoxelMaterial> getOrCreateDecorMatRefList()
	{
		return tmpDecorMaterials ?? (tmpDecorMaterials = new RefList<VoxelMaterial>());
	}

	private static VoxelInfo[] getOrCreateCuboidInfoArray()
	{
		return tmpVoxels ?? (tmpVoxels = new VoxelInfo[5832]);
	}

	public static MeshData CreateMesh(ICoreClientAPI capi, List<uint> voxelCuboids, int[] blockIds, int[] decorIds, BlockPos posForRnd = null, uint[] originalCuboids = null, int decorRotations = 0)
	{
		return CreateMesh(capi, voxelCuboids, blockIds, decorIds, decorRotations, originalCuboids ?? defaultOriginalVoxelCuboids, posForRnd);
	}

	public unsafe static MeshData CreateMesh(ICoreClientAPI capi, List<uint> voxelCuboids, int[] blockIds, int[] decorIds, int decorRotations, uint[] originalVoxelCuboids, BlockPos pos = null)
	{
		MeshData mesh = new MeshData(24, 36).WithColorMaps().WithRenderpasses().WithXyzFaces();
		if (voxelCuboids == null || blockIds == null)
		{
			return mesh;
		}
		RefList<VoxelMaterial> blockMatList = getOrCreateBlockMatRefList();
		blockMatList.Clear();
		VoxelInfo[] voxels = getOrCreateCuboidInfoArray();
		fixed (VoxelInfo* ptr = voxels)
		{
			Unsafe.InitBlockUnaligned(ptr, byte.MaxValue, (uint)(sizeof(VoxelInfo) * voxels.Length));
			if (pos != null)
			{
				FetchNeighborVoxels(capi, blockMatList, ptr, pos);
			}
		}
		bool hasTopSoil = false;
		bool hasFrostable = false;
		int matIndex = blockMatList.Count;
		for (int i = 0; i < blockIds.Length; i++)
		{
			Block block = capi.World.GetBlock(blockIds[i]);
			blockMatList.Add(VoxelMaterial.FromBlock(capi, block, pos, cullBetweenTransparents: true));
			hasTopSoil |= block.RenderPass == EnumChunkRenderPass.TopSoil;
			hasFrostable |= block.Frostable;
		}
		if (hasTopSoil)
		{
			mesh.CustomShorts = new CustomMeshDataPartShort
			{
				InterleaveOffsets = new int[1],
				InterleaveSizes = new int[1] { 2 },
				InterleaveStride = 4,
				Conversion = DataConversion.NormalizedFloat
			};
		}
		RefList<VoxelMaterial> decorMatList = loadDecor(capi, voxelCuboids, decorIds, pos, mesh, decorRotations);
		int* origVoxelBounds = stackalloc int[6];
		FromUint(originalVoxelCuboids[0], out origVoxelBounds[3], out origVoxelBounds[5], out *origVoxelBounds, out origVoxelBounds[1], out origVoxelBounds[4], out origVoxelBounds[2], out var _);
		_ = voxelCuboids.Count;
		fixed (VoxelInfo* voxelsPtr = voxels)
		{
			int x0;
			int y0;
			int z0;
			int x1;
			int y1;
			int z1;
			int material2;
			foreach (uint voxelCuboid in voxelCuboids)
			{
				FromUint(voxelCuboid, out x0, out y0, out z0, out x1, out y1, out z1, out material2);
				FillCuboidEdges(voxelsPtr, x0, y0, z0, x1, y1, z1, matIndex + material2);
			}
			GenFaceInfo genFaceInfo = new GenFaceInfo
			{
				capi = capi,
				targetMesh = mesh,
				originalBounds = origVoxelBounds,
				subPixelPaddingx = capi.BlockTextureAtlas.SubPixelPaddingX,
				subPixelPaddingy = capi.BlockTextureAtlas.SubPixelPaddingY,
				AnyFrostable = hasFrostable
			};
			GenPlaneInfo genPlaneInfo = new GenPlaneInfo
			{
				blockMaterials = blockMatList,
				decorMaterials = decorMatList,
				voxels = voxelsPtr
			};
			foreach (uint voxelCuboid2 in voxelCuboids)
			{
				FromUint(voxelCuboid2, out x0, out y0, out z0, out x1, out y1, out z1, out material2);
				genPlaneInfo.materialIndex = matIndex + material2;
				GenCuboidMesh(ref genFaceInfo, ref genPlaneInfo, x0, y0, z0, x1, y1, z1);
			}
		}
		return mesh;
	}

	private static RefList<VoxelMaterial> loadDecor(ICoreClientAPI capi, List<uint> voxelCuboids, int[] decorIds, BlockPos pos, MeshData mesh, int decorRotations)
	{
		RefList<VoxelMaterial> decorMatList = null;
		if (decorIds != null)
		{
			decorMatList = getOrCreateDecorMatRefList();
			decorMatList.Clear();
			for (int i = 0; i < decorIds.Length; i++)
			{
				int decorId = decorIds[i];
				if (decorId == 0)
				{
					decorMatList.Add(noMat);
					continue;
				}
				Block decoblock = capi.World.GetBlock(decorId);
				JsonObject attributes = decoblock.Attributes;
				if (attributes == null || !attributes["attachas3d"].AsBool())
				{
					int rot = (decorRotations >> i * 3) & 7;
					decorMatList.Add(VoxelMaterial.FromBlock(capi, decoblock, pos, cullBetweenTransparents: true, rot));
					continue;
				}
				int rot2 = ((decorRotations >> i * 3) & 7) % 4;
				MeshData decomesh = capi.TesselatorManager.GetDefaultBlockMesh(decoblock).Clone();
				if (rot2 > 0)
				{
					decomesh.Rotate(centerBase, 0f, (float)rot2 * ((float)Math.PI / 2f), 0f);
				}
				decomesh.Translate(BlockFacing.ALLFACES[i].Normalf * getOutermostVoxelDistanceToCenter(voxelCuboids, i));
				mesh.AddMeshData(decomesh);
			}
		}
		return decorMatList;
	}

	private static float getOutermostVoxelDistanceToCenter(List<uint> voxelCuboids, int faceindex)
	{
		int d = 0;
		int edge = 0;
		switch (faceindex)
		{
		case 0:
			edge = (d = 16);
			foreach (uint cuboid6 in voxelCuboids)
			{
				d = Math.Min(d, (int)((cuboid6 >> 8) & 0xF));
			}
			break;
		case 1:
			edge = (d = 0);
			foreach (uint cuboid5 in voxelCuboids)
			{
				d = Math.Max(d, (int)(((cuboid5 >> 12) & 0xF) + 1));
			}
			break;
		case 2:
			edge = (d = 0);
			foreach (uint cuboid4 in voxelCuboids)
			{
				d = Math.Max(d, (int)(((cuboid4 >> 20) & 0xF) + 1));
			}
			break;
		case 3:
			edge = (d = 16);
			foreach (uint cuboid3 in voxelCuboids)
			{
				d = Math.Min(d, (int)(cuboid3 & 0xF));
			}
			break;
		case 4:
			edge = (d = 0);
			foreach (uint cuboid2 in voxelCuboids)
			{
				d = Math.Max(d, (int)(((cuboid2 >> 16) & 0xF) + 1));
			}
			break;
		case 5:
			edge = (d = 16);
			foreach (uint cuboid in voxelCuboids)
			{
				d = Math.Min(d, (int)((cuboid >> 4) & 0xF));
			}
			break;
		}
		return (float)Math.Abs(edge - d) / 16f;
	}

	public MeshData CreateDecalMesh(ITexPositionSource decalTexSource)
	{
		return CreateDecalMesh(Api as ICoreClientAPI, VoxelCuboids, decalTexSource, OriginalVoxelCuboids);
	}

	public unsafe static MeshData CreateDecalMesh(ICoreClientAPI capi, List<uint> voxelCuboids, ITexPositionSource decalTexSource, uint[] originalVoxelCuboids)
	{
		MeshData mesh = new MeshData(24, 36).WithColorMaps().WithRenderpasses().WithXyzFaces();
		if (voxelCuboids == null)
		{
			return mesh;
		}
		RefList<VoxelMaterial> matList = getOrCreateBlockMatRefList();
		matList.Clear();
		matList.Add(VoxelMaterial.FromTexSource(capi, decalTexSource, cullBetweenTransparents: true, forDecal: true));
		int* origVoxelBounds = stackalloc int[6];
		FromUint(originalVoxelCuboids[0], out origVoxelBounds[3], out origVoxelBounds[5], out *origVoxelBounds, out origVoxelBounds[1], out origVoxelBounds[4], out origVoxelBounds[2], out var material);
		int count = voxelCuboids.Count;
		VoxelInfo[] voxels = getOrCreateCuboidInfoArray();
		fixed (VoxelInfo* ptr = voxels)
		{
			Unsafe.InitBlockUnaligned(ptr, byte.MaxValue, (uint)(sizeof(VoxelInfo) * voxels.Length));
			int x0;
			int y0;
			int z0;
			int x1;
			int y1;
			int z1;
			for (int i = 0; i < count; i++)
			{
				FromUint(voxelCuboids[i], out x0, out y0, out z0, out x1, out y1, out z1, out material);
				FillCuboidEdges(ptr, x0, y0, z0, x1, y1, z1, 0);
			}
			GenFaceInfo genFaceInfo = new GenFaceInfo
			{
				capi = capi,
				targetMesh = mesh,
				originalBounds = origVoxelBounds,
				subPixelPaddingx = capi.BlockTextureAtlas.SubPixelPaddingX,
				subPixelPaddingy = capi.BlockTextureAtlas.SubPixelPaddingY
			};
			GenPlaneInfo genPlaneInfo = new GenPlaneInfo
			{
				blockMaterials = matList,
				voxels = ptr,
				materialIndex = 0
			};
			for (int j = 0; j < count; j++)
			{
				FromUint(voxelCuboids[j], out x0, out y0, out z0, out x1, out y1, out z1, out material);
				GenCuboidMesh(ref genFaceInfo, ref genPlaneInfo, x0, y0, z0, x1, y1, z1);
			}
		}
		return mesh;
	}

	private unsafe static void FetchNeighborVoxels(ICoreClientAPI capi, RefList<VoxelMaterial> matList, VoxelInfo* voxels, BlockPos pos)
	{
		IBlockAccessor ba = capi.World.BlockAccessor;
		BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
		foreach (BlockFacing face in aLLFACES)
		{
			BlockPos blockPos = pos.AddCopy(face);
			if (!(ba.GetBlockEntity(blockPos) is BlockEntityMicroBlock { BlockIds: var materials, VoxelCuboids: var voxCuboids }) || materials == null || voxCuboids == null)
			{
				continue;
			}
			List<uint> voxelCuboids = voxCuboids;
			int matOffset = matList.Count;
			int[] array = materials;
			foreach (int id in array)
			{
				matList.Add(VoxelMaterial.FromBlock(capi, capi.World.GetBlock(id), blockPos, cullBetweenTransparents: true));
			}
			for (int k = 0; k < voxelCuboids.Count; k++)
			{
				FromUint(voxelCuboids[k], out var x0, out var y0, out var z0, out var x1, out var y1, out var z1, out var material);
				if (material >= materials.Length)
				{
					break;
				}
				FillCuboidFace(voxels, x0, y0, z0, x1, y1, z1, matOffset + material, face);
			}
		}
	}

	private unsafe static void FillCuboidFace(VoxelInfo* cuboids, int x0, int y0, int z0, int x1, int y1, int z1, int material, BlockFacing face)
	{
		switch (face.Index)
		{
		case 0:
			if (z1 != 16)
			{
				return;
			}
			break;
		case 1:
			if (x0 != 0)
			{
				return;
			}
			break;
		case 2:
			if (z0 != 0)
			{
				return;
			}
			break;
		case 3:
			if (x1 != 16)
			{
				return;
			}
			break;
		case 4:
			if (y0 != 0)
			{
				return;
			}
			break;
		case 5:
			if (y1 != 16)
			{
				return;
			}
			break;
		}
		x0++;
		x1++;
		y0++;
		y1++;
		z0++;
		z1++;
		y0 *= 18;
		y1 *= 18;
		z0 *= 324;
		z1 *= 324;
		switch (face.Index)
		{
		case 0:
			FillPlane(cuboids, material, x0, x1, 1, y0, y1, 18, 0);
			break;
		case 1:
			FillPlane(cuboids, material, y0, y1, 18, z0, z1, 324, 17);
			break;
		case 2:
			FillPlane(cuboids, material, x0, x1, 1, y0, y1, 18, 5508);
			break;
		case 3:
			FillPlane(cuboids, material, y0, y1, 18, z0, z1, 324, 0);
			break;
		case 4:
			FillPlane(cuboids, material, x0, x1, 1, z0, z1, 324, 306);
			break;
		case 5:
			FillPlane(cuboids, material, x0, x1, 1, z0, z1, 324, 0);
			break;
		}
	}

	public unsafe static void FillCuboidEdges(VoxelInfo* cuboids, int x0, int y0, int z0, int x1, int y1, int z1, int material)
	{
		x0++;
		x1++;
		y0++;
		y1++;
		z0++;
		z1++;
		y0 *= 18;
		y1 *= 18;
		z0 *= 324;
		z1 *= 324;
		FillPlane(cuboids, material, x0, x1, 1, y0, y1, 18, z0);
		FillPlane(cuboids, material, x0, x1, 1, y0, y1, 18, z1 - 324);
		FillPlane(cuboids, material, x0, x1, 1, z0, z1, 324, y0);
		FillPlane(cuboids, material, x0, x1, 1, z0, z1, 324, y1 - 18);
		FillPlane(cuboids, material, y0, y1, 18, z0, z1, 324, x0);
		FillPlane(cuboids, material, y0, y1, 18, z0, z1, 324, x1 - 1);
	}

	public unsafe static void FillPlane(VoxelInfo* ptr, int value, int fromX, int toX, int stepX, int fromY, int toY, int stepY, int z)
	{
		for (int x = fromX; x < toX; x += stepX)
		{
			for (int y = fromY; y < toY; y += stepY)
			{
				ptr[x + y + z].Material = value;
			}
		}
	}

	public static void GenCuboidMesh(ref GenFaceInfo genFaceInfo, ref GenPlaneInfo genPlaneInfo, int x0, int y0, int z0, int x1, int y1, int z1)
	{
		x0++;
		x1++;
		y0++;
		y1++;
		z0++;
		z1++;
		y0 *= 18;
		y1 *= 18;
		z0 *= 324;
		z1 *= 324;
		genFaceInfo.SetInfo(ConvertPlaneX, 3, (genPlaneInfo.decorMaterials == null) ? noMat : genPlaneInfo.decorMaterials[3]);
		genPlaneInfo.SetCoords(z0, z1, 324, y0, y1, 18, x0, x0 - 1);
		genPlaneInfo.GenPlaneMesh(ref genFaceInfo);
		genFaceInfo.SetInfo(ConvertPlaneX, 1, (genPlaneInfo.decorMaterials == null) ? noMat : genPlaneInfo.decorMaterials[1]);
		genPlaneInfo.SetCoords(z0, z1, 324, y0, y1, 18, x1 - 1, x1);
		genPlaneInfo.GenPlaneMesh(ref genFaceInfo);
		genFaceInfo.SetInfo(ConvertPlaneY, 5, (genPlaneInfo.decorMaterials == null) ? noMat : genPlaneInfo.decorMaterials[5]);
		genPlaneInfo.SetCoords(x0, x1, 1, z0, z1, 324, y0, y0 - 18);
		genPlaneInfo.GenPlaneMesh(ref genFaceInfo);
		genFaceInfo.SetInfo(ConvertPlaneY, 4, (genPlaneInfo.decorMaterials == null) ? noMat : genPlaneInfo.decorMaterials[4]);
		genPlaneInfo.SetCoords(x0, x1, 1, z0, z1, 324, y1 - 18, y1);
		genPlaneInfo.GenPlaneMesh(ref genFaceInfo);
		genFaceInfo.SetInfo(ConvertPlaneZ, 0, (genPlaneInfo.decorMaterials == null) ? noMat : genPlaneInfo.decorMaterials[0]);
		genPlaneInfo.SetCoords(x0, x1, 1, y0, y1, 18, z0, z0 - 324);
		genPlaneInfo.GenPlaneMesh(ref genFaceInfo);
		genFaceInfo.SetInfo(ConvertPlaneZ, 2, (genPlaneInfo.decorMaterials == null) ? noMat : genPlaneInfo.decorMaterials[2]);
		genPlaneInfo.SetCoords(x0, x1, 1, y0, y1, 18, z1 - 324, z1);
		genPlaneInfo.GenPlaneMesh(ref genFaceInfo);
	}

	public static void FromUint(uint val, out int x0, out int y0, out int z0, out int x1, out int y1, out int z1, out int material)
	{
		x0 = (int)(val & 0xF);
		y0 = (int)((val >> 4) & 0xF);
		z0 = (int)((val >> 8) & 0xF);
		x1 = (int)(((val >> 12) & 0xF) + 1);
		y1 = (int)(((val >> 16) & 0xF) + 1);
		z1 = (int)(((val >> 20) & 0xF) + 1);
		material = (int)((val >> 24) & 0xFF);
	}

	private static bool isMergableMaterial(int selfMat, int otherMat, RefList<VoxelMaterial> materials)
	{
		if (selfMat == otherMat)
		{
			return true;
		}
		if (otherMat >= 0)
		{
			VoxelMaterial self = materials[selfMat];
			VoxelMaterial other = materials[otherMat];
			if (self.BlockId == other.BlockId)
			{
				return true;
			}
			if (other.BlockId == 0)
			{
				return false;
			}
			bool selfOpaque = true;
			switch (self.RenderPass)
			{
			case EnumChunkRenderPass.OpaqueNoCull:
			case EnumChunkRenderPass.BlendNoCull:
			case EnumChunkRenderPass.Liquid:
				return false;
			case EnumChunkRenderPass.Transparent:
			case EnumChunkRenderPass.TopSoil:
			case EnumChunkRenderPass.Meta:
				selfOpaque = false;
				break;
			}
			bool otherOpaque = true;
			EnumChunkRenderPass renderPass = other.RenderPass;
			if ((uint)(renderPass - 2) <= 1u || (uint)(renderPass - 5) <= 1u)
			{
				otherOpaque = false;
			}
			if (selfOpaque && otherOpaque)
			{
				return true;
			}
			if (selfOpaque)
			{
				return false;
			}
			return otherOpaque | self.CullBetweenTransparents;
		}
		return false;
	}

	private static void ConvertPlaneXImpl(int width, int height, out float sx, out float sy, out float sz)
	{
		sx = 1f / 32f;
		sy = (float)height * (1f / 32f);
		sz = (float)width * (1f / 32f);
	}

	private static void ConvertPlaneYImpl(int width, int height, out float sx, out float sy, out float sz)
	{
		sx = (float)width * (1f / 32f);
		sy = 1f / 32f;
		sz = (float)height * (1f / 32f);
	}

	private static void ConvertPlaneZImpl(int width, int height, out float sx, out float sy, out float sz)
	{
		sx = (float)width * (1f / 32f);
		sy = (float)height * (1f / 32f);
		sz = 1f / 32f;
	}

	public static void UpdateNeighbors(BlockEntityMicroBlock bm)
	{
		if (bm.Api != null && bm.Api.Side == EnumAppSide.Client)
		{
			BlockPos pos = bm.Pos;
			IWorldAccessor world = bm.Api.World;
			BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
			for (int i = 0; i < aLLFACES.Length; i++)
			{
				aLLFACES[i].IterateThruFacingOffsets(pos);
				UpdateNeighbour(world, pos);
			}
			BlockFacing.FinishIteratingAllFaces(pos);
		}
	}

	private static void UpdateNeighbour(IWorldAccessor world, BlockPos pos)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityMicroBlock { BlockIds: not null, VoxelCuboids: not null } be)
		{
			be.MarkMeshDirty();
			be.MarkDirty(redrawOnClient: true);
		}
	}

	public override void OnPlacementBySchematic(ICoreServerAPI api, IBlockAccessor blockAccessor, BlockPos pos, Dictionary<int, Dictionary<int, int>> replaceBlocks, int centerrockblockid, Block layerBlock, bool resolveImports)
	{
		base.OnPlacementBySchematic(api, blockAccessor, pos, replaceBlocks, centerrockblockid, layerBlock, resolveImports);
		if (replaceBlocks != null)
		{
			if (BlockName != null && BlockName.Length > 0 && GetPlacedBlockName(api, VoxelCuboids, BlockIds, null) == BlockName)
			{
				BlockName = null;
			}
			for (int i = 0; i < BlockIds.Length; i++)
			{
				if (replaceBlocks.TryGetValue(BlockIds[i], out var replaceByBlock) && replaceByBlock.TryGetValue(centerrockblockid, out var newBlockId))
				{
					BlockIds[i] = blockAccessor.GetBlock(newBlockId).Id;
				}
			}
		}
		if (!resolveImports)
		{
			return;
		}
		int newMatIndex = -1;
		int len = BlockIds.Length;
		for (int j = 0; j < len; j++)
		{
			if (BlockIds[j] != BlockMicroBlock.BlockLayerMetaBlockId)
			{
				continue;
			}
			for (int k = 0; k < VoxelCuboids.Count; k++)
			{
				if (((VoxelCuboids[k] >> 24) & 0xFF) != j)
				{
					continue;
				}
				if (layerBlock == null)
				{
					VoxelCuboids.RemoveAt(k);
					k--;
					continue;
				}
				if (newMatIndex < 0)
				{
					BlockIds = BlockIds.Append(layerBlock.Id);
					newMatIndex = BlockIds.Length - 1;
				}
				VoxelCuboids[k] = (VoxelCuboids[k] & 0xFFFFFF) | (uint)(newMatIndex << 24);
			}
		}
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
	{
		if (Mesh == null)
		{
			Mesh = GenMesh();
		}
		base.OnTesselation(mesher, tesselator);
		if (withColorMapData)
		{
			ColorMapData cmapdata = (Api as ICoreClientAPI).World.GetColorMapData(Api.World.Blocks[BlockIds[0]], Pos.X, Pos.Y, Pos.Z);
			mesher.AddMeshData(Mesh, cmapdata);
		}
		else
		{
			mesher.AddMeshData(Mesh);
		}
		base.Block = Api.World.BlockAccessor.GetBlock(Pos);
		return true;
	}

	public bool CanAccept(Block decorBlock)
	{
		JsonObject attributes = decorBlock.Attributes;
		if (attributes == null)
		{
			return true;
		}
		return attributes["chiselBlockAttachable"]?.AsBool(defaultValue: true) != false;
	}

	public void SetDecor(Block blockToPlace, BlockFacing face)
	{
		if (DecorIds == null)
		{
			DecorIds = new int[6];
		}
		int rotfaceindex = (face.IsVertical ? face.Index : BlockFacing.HORIZONTALS_ANGLEORDER[GameMath.Mod(face.HorizontalAngleIndex - rotationY / 90, 4)].Index);
		DecorIds[rotfaceindex] = blockToPlace.Id;
		MarkDirty(redrawOnClient: true);
	}

	public int GetDecor(BlockFacing face)
	{
		if (DecorIds == null)
		{
			return 0;
		}
		int rotfaceindex = (face.IsVertical ? face.Index : BlockFacing.HORIZONTALS_ANGLEORDER[GameMath.Mod(face.HorizontalAngleIndex - rotationY / 90, 4)].Index);
		return DecorIds[rotfaceindex];
	}

	public bool ExchangeWith(ItemSlot fromSlot, ItemSlot toSlot)
	{
		Block fromBlock = fromSlot.Itemstack?.Block;
		Block toBlock = toSlot.Itemstack?.Block;
		if (fromBlock == null || toBlock == null)
		{
			return false;
		}
		bool exchanged = false;
		for (int i = 0; i < BlockIds.Length; i++)
		{
			if (BlockIds[i] == fromBlock.Id)
			{
				BlockIds[i] = toBlock.Id;
				exchanged = true;
			}
		}
		foreach (BlockEntityBehavior behavior in Behaviors)
		{
			if (behavior is IMaterialExchangeable exchangeable)
			{
				exchangeable.ExchangeWith(fromSlot, toSlot);
			}
		}
		RegenSelectionBoxes(Api.World, null);
		MarkDirty(redrawOnClient: true);
		return exchanged;
	}

	public static Cuboidf GetEncompassingHitbox(List<uint> VoxelCuboids)
	{
		CuboidWithMaterial cwm = new CuboidWithMaterial();
		Cuboidf box = new Cuboidf(16f, 16f, 16f, 0f, 0f, 0f);
		for (int i = 0; i < VoxelCuboids.Count; i++)
		{
			FromUint(VoxelCuboids[i], cwm);
			box.X1 = Math.Min(box.X1, cwm.X1);
			box.Y1 = Math.Min(box.Y1, cwm.Y1);
			box.Z1 = Math.Min(box.Z1, cwm.Z1);
			box.X2 = Math.Max(box.X2, cwm.X2);
			box.Y2 = Math.Max(box.Y2, cwm.Y2);
			box.Z2 = Math.Max(box.Z2, cwm.Z2);
		}
		return box;
	}

	public Cuboidf GetEncompassingHitbox()
	{
		return GetEncompassingHitbox(VoxelCuboids);
	}
}
