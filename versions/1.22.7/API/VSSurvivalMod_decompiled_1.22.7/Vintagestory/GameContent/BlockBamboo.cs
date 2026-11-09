using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockBamboo : Block, ITreeGenerator, ICustomTreeFellingBehavior
{
	private static Random rand = new Random();

	private bool isSegmentWithLeaves;

	private IBlockAccessor? lockFreeBa;

	private Dictionary<int, int[]> windModeByFlagCount = new Dictionary<int, int[]>();

	private Vec3i windDir = new Vec3i();

	public int MaxPlantHeight { get; private set; }

	private string? domain
	{
		get
		{
			if (!(Code.Domain == "game"))
			{
				return Code.Domain;
			}
			return null;
		}
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		MaxPlantHeight = Attributes?["maxPlantHeight"].AsInt(15) ?? 15;
		if (api.Side == EnumAppSide.Client)
		{
			lockFreeBa = api.World.GetLockFreeBlockAccessor();
		}
		if (Variant["type"] == "grown" && Variant["part"] == "segment1")
		{
			(api as ICoreServerAPI)?.RegisterTreeGenerator(AssetLocation.Create(FirstCodePart() + "-grown-" + Variant["color"], domain), this);
		}
		if (RandomDrawOffset > 0)
		{
			JsonObject overrider = Attributes?["overrideRandomDrawOffset"];
			if (overrider != null && overrider.Exists)
			{
				RandomDrawOffset = overrider.AsInt(1);
			}
		}
		isSegmentWithLeaves = Variant["part"] == "segment2" || Variant["part"] == "segment3";
	}

	public string? Type()
	{
		return Variant["color"];
	}

	public Block? NextSegment(IBlockAccessor blockAccess)
	{
		string text = Variant["part"];
		int nextSegment = text[text.Length - 1].ToString().ToInt() + 1;
		if (nextSegment > 3 || nextSegment < 1)
		{
			return null;
		}
		return blockAccess.GetBlock(CodeWithVariant("part", "segment" + nextSegment));
	}

	public void GrowTree(IBlockAccessor blockAccessor, BlockPos pos, TreeGenParams treegenParams, IRandom random)
	{
		float f = ((treegenParams.otherBlockChance == 0f) ? (3f + (float)rand.NextDouble() * 6f) : ((3f + (float)rand.NextDouble() * 4f) * 3f * 3f));
		int quantity = GameMath.RoundRandom(rand, f);
		BlockPos npos = pos.Copy();
		float sizeModifier = GameMath.Mix(treegenParams.size, 1f, 0.5f);
		sizeModifier *= 1f + (float)rand.NextDouble() * 0.5f;
		while (quantity-- > 0)
		{
			float dist = Math.Max(1f, pos.DistanceTo(npos) - 2f);
			GrowStalk(blockAccessor, npos.UpCopy(), dist, sizeModifier, treegenParams.vinesGrowthChance);
			npos.Set(pos);
			npos.X += rand.Next(8) - 4;
			npos.Z += rand.Next(8) - 4;
			bool foundSuitableBlock = false;
			for (int y = 2; y >= -2; y--)
			{
				npos.Y = pos.Y + y;
				if (blockAccessor.GetBlock(npos).Fertility > 0)
				{
					foundSuitableBlock = true;
					break;
				}
			}
			if (!foundSuitableBlock)
			{
				break;
			}
		}
	}

	private void GrowStalk(IBlockAccessor blockAccessor, BlockPos upos, float centerDist, float sizeModifier, float vineGrowthChance)
	{
		Block block = this;
		float heightf = (float)(8 + rand.Next(5)) * sizeModifier;
		heightf = Math.Max(1f, heightf - centerDist);
		int height = (int)heightf;
		int nextSegmentAtHeight = height / 3;
		BlockPos npos = upos.Copy();
		Block shootBlock = blockAccessor.GetBlock(AssetLocation.Create("sapling-" + Variant["color"] + FirstCodePart() + "shoots-free", domain));
		Block blockLeaves = blockAccessor.GetBlock(AssetLocation.Create(FirstCodePart() + "leaves-" + Variant["color"] + "-grown", domain));
		BlockFacing[] hORIZONTALS = BlockFacing.HORIZONTALS;
		foreach (BlockFacing face in hORIZONTALS)
		{
			if (rand.NextDouble() > 0.75)
			{
				BlockPos bpos = npos.Set(upos).Add(face);
				if (blockAccessor.GetBlock(bpos).Replaceable >= shootBlock.Replaceable && blockAccessor.GetBlockBelow(bpos, 1, 1).Fertility > 0 && blockAccessor.GetBlock(bpos, 2).BlockId == 0)
				{
					blockAccessor.SetBlock(shootBlock.BlockId, bpos);
				}
			}
		}
		if (height < 4)
		{
			block = (block as BlockBamboo)?.NextSegment(blockAccessor);
			if (block == null)
			{
				return;
			}
		}
		for (int j = 0; j < height; j++)
		{
			if (!blockAccessor.GetBlock(upos).IsReplacableBy(block))
			{
				break;
			}
			blockAccessor.SetBlock(block.BlockId, upos);
			if (nextSegmentAtHeight <= j)
			{
				block = (block as BlockBamboo)?.NextSegment(blockAccessor);
				nextSegmentAtHeight += height / 3;
			}
			if (block == null)
			{
				break;
			}
			if (block.Variant["part"] == "segment3")
			{
				hORIZONTALS = BlockFacing.ALLFACES;
				foreach (BlockFacing facing in hORIZONTALS)
				{
					if (facing == BlockFacing.DOWN)
					{
						continue;
					}
					float chanceFac = ((facing == BlockFacing.UP) ? 0f : 0.25f);
					if (!(rand.NextDouble() > (double)chanceFac))
					{
						continue;
					}
					npos.Set(upos.X + facing.Normali.X, upos.Y + facing.Normali.Y, upos.Z + facing.Normali.Z);
					if (rand.NextDouble() > 0.33)
					{
						BlockPos bpos2 = npos.DownCopy();
						if (blockAccessor.GetBlock(bpos2).Replaceable >= blockLeaves.Replaceable)
						{
							blockAccessor.SetBlock(blockLeaves.BlockId, bpos2);
						}
					}
					if (blockAccessor.GetBlock(npos).Replaceable < blockLeaves.Replaceable)
					{
						continue;
					}
					blockAccessor.SetBlock(blockLeaves.BlockId, npos);
					BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
					foreach (BlockFacing facing2 in aLLFACES)
					{
						if (rand.NextDouble() > 0.5)
						{
							npos.Set(upos.X + facing.Normali.X + facing2.Normali.X, upos.Y + facing.Normali.Y + facing2.Normali.Y, upos.Z + facing.Normali.Z + facing2.Normali.Z);
							if (blockAccessor.GetBlock(npos).Replaceable >= blockLeaves.Replaceable)
							{
								blockAccessor.SetBlock(blockLeaves.BlockId, npos);
							}
							break;
						}
					}
				}
			}
			upos.Up();
		}
	}

	public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing, int rndIndex = -1)
	{
		if (!isSegmentWithLeaves || Variant["part"] != "segment3")
		{
			return base.GetRandomColor(capi, pos, facing, rndIndex);
		}
		if (Textures == null || Textures.Count == 0)
		{
			return 0;
		}
		if (!Textures.TryGetValue(facing.Code, out var tex))
		{
			tex = Textures.First().Value;
		}
		if (tex?.Baked == null)
		{
			return 0;
		}
		int color = capi.BlockTextureAtlas.GetRandomColor(tex.Baked.TextureSubId, rndIndex);
		return capi.World.ApplyColorMapOnRgba("climatePlantTint", SeasonColorMap, color, pos.X, pos.Y, pos.Z);
	}

	public override void OnDecalTesselation(IWorldAccessor world, MeshData decalMesh, BlockPos pos)
	{
		bool enableWind = world.BlockAccessor.GetLightLevel(pos, EnumLightLevelType.OnlySunLight) >= 14;
		windDir.X = Math.Sign(GlobalConstants.CurrentWindSpeedClient.X);
		windDir.Z = 0;
		applyWindSwayToMesh(decalMesh, enableWind, pos, windDir);
		base.OnDecalTesselation(world, decalMesh, pos);
	}

	public override void OnJsonTesselation(ref MeshData sourceMesh, ref int[] lightRgbsByCorner, BlockPos pos, Block[] chunkExtBlocks, int extIndex3d)
	{
		bool enableWind = ((lightRgbsByCorner[24] >> 24) & 0xFF) >= 159;
		windDir.X = Math.Sign(GlobalConstants.CurrentWindSpeedClient.X);
		windDir.Z = 0;
		applyWindSwayToMesh(sourceMesh, enableWind, pos, windDir);
	}

	private void applyWindSwayToMesh(MeshData sourceMesh, bool enableWind, BlockPos pos, Vec3i windDir)
	{
		if (lockFreeBa == null)
		{
			return;
		}
		if (!windModeByFlagCount.TryGetValue(sourceMesh.FlagsCount, out int[] origFlags))
		{
			int[] array = (windModeByFlagCount[sourceMesh.FlagsCount] = new int[sourceMesh.FlagsCount]);
			origFlags = array;
			for (int i = 0; i < origFlags.Length; i++)
			{
				origFlags[i] = sourceMesh.Flags[i] & 0x1E000000;
			}
		}
		bool sideDisableWindWaveDown = false;
		Block nblock = lockFreeBa.GetBlockBelow(pos, 1, 1);
		if (nblock.VertexFlags.WindMode == EnumWindBitMode.NoWind && nblock.SideSolid[4])
		{
			sideDisableWindWaveDown = true;
		}
		else if (nblock is BlockBamboo)
		{
			nblock = lockFreeBa.GetBlockRaw(pos.X + windDir.X, pos.InternalY - 1, pos.Z + windDir.Z);
			if (nblock.VertexFlags.WindMode == EnumWindBitMode.NoWind && nblock.SideSolid[3])
			{
				sideDisableWindWaveDown = true;
			}
		}
		int groundOffset = 1;
		nblock = lockFreeBa.GetBlockRaw(pos.X + windDir.X, pos.InternalY, pos.Z + windDir.Z);
		if (nblock.VertexFlags.WindMode == EnumWindBitMode.NoWind && nblock.SideSolid[3])
		{
			enableWind = false;
		}
		if (enableWind)
		{
			bool bambooLeavesFound = isSegmentWithLeaves;
			bool continuousBambooCane = true;
			for (; groundOffset < 8; groundOffset++)
			{
				Block block = api.World.BlockAccessor.GetBlockBelow(pos, groundOffset);
				Block blockInWindDir = ((block is BlockBamboo) ? api.World.BlockAccessor.GetBlockRaw(pos.X + windDir.X, pos.InternalY - groundOffset, pos.Z + windDir.Z) : null);
				if ((block.VertexFlags.WindMode == EnumWindBitMode.NoWind && block.SideSolid[4]) || (blockInWindDir != null && blockInWindDir.VertexFlags.WindMode == EnumWindBitMode.NoWind && blockInWindDir.SideSolid[3]))
				{
					break;
				}
				if (blockInWindDir == null)
				{
					continuousBambooCane = false;
				}
				if (!bambooLeavesFound && continuousBambooCane && block is BlockBamboo { isSegmentWithLeaves: not false })
				{
					bambooLeavesFound = true;
				}
			}
			int y = 0;
			while (!bambooLeavesFound && ++y < MaxPlantHeight)
			{
				Block block = api.World.BlockAccessor.GetBlockAbove(pos, y, 1);
				if (block is BlockBamboo bam2)
				{
					bambooLeavesFound = bam2.isSegmentWithLeaves;
					continue;
				}
				if (block is BlockWithLeavesMotion)
				{
					bambooLeavesFound = true;
				}
				break;
			}
			if (!bambooLeavesFound)
			{
				enableWind = false;
			}
		}
		int clearFlags = 33554431;
		int verticesCount = sourceMesh.VerticesCount;
		if (!enableWind)
		{
			for (int vertexNum = 0; vertexNum < verticesCount; vertexNum++)
			{
				sourceMesh.Flags[vertexNum] &= clearFlags;
			}
			return;
		}
		for (int j = 0; j < verticesCount; j++)
		{
			int flag = sourceMesh.Flags[j] & clearFlags;
			float fy = sourceMesh.xyz[j * 3 + 1];
			if (fy > 0.05f || !sideDisableWindWaveDown)
			{
				flag |= origFlags[j] | (GameMath.Clamp(groundOffset + ((fy < 0.95f) ? (-1) : 0), 0, 7) << 29);
			}
			sourceMesh.Flags[j] = flag;
		}
	}

	public EnumTreeFellingBehavior GetTreeFellingBehavior(BlockPos pos, Vec3i fromDir, int spreadIndex)
	{
		return EnumTreeFellingBehavior.ChopSpreadVertical;
	}
}
