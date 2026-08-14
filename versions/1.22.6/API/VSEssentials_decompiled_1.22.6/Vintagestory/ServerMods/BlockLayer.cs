using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.ServerMods;

[JsonObject(MemberSerialization.OptIn)]
public class BlockLayer
{
	[JsonProperty]
	public string Name;

	[JsonProperty]
	public string ID;

	[JsonProperty]
	public AssetLocation BlockCode;

	[JsonProperty]
	public BlockLayerCodeByMin[] BlockCodeByMin;

	[JsonProperty]
	public BlockLayerCodeByBiome[] BlockCodeByBiome;

	[JsonProperty]
	public int MinTemp = -30;

	[JsonProperty]
	public int MaxTemp = 40;

	[JsonProperty]
	public float MinRain;

	[JsonProperty]
	public float MaxRain = 1f;

	[JsonProperty]
	public float MinFertility;

	[JsonProperty]
	public float MaxFertility = 1f;

	[JsonProperty]
	public float MinY;

	[JsonProperty]
	public float MaxY = 1f;

	[JsonProperty]
	public int Biome = -1;

	[JsonProperty]
	public int Thickness = 1;

	[JsonProperty]
	public double[] NoiseAmplitudes;

	[JsonProperty]
	public double[] NoiseFrequencies;

	[JsonProperty]
	public double NoiseThreshold = 0.5;

	private NormalizedSimplexNoise noiseGen;

	public int BlockId;

	public Dictionary<int, int> BlockIdMapping;

	public void Init(ICoreServerAPI api, RockStrataConfig rockstrata, Random rnd)
	{
		ResolveBlockIds(api, rockstrata);
		if (NoiseAmplitudes != null)
		{
			noiseGen = new NormalizedSimplexNoise(NoiseAmplitudes, NoiseFrequencies, rnd.Next());
		}
	}

	public bool NoiseOk(BlockPos pos)
	{
		if (noiseGen != null)
		{
			return noiseGen.Noise((double)pos.X / 10.0, (double)pos.Y / 10.0, (double)pos.Z / 10.0) > NoiseThreshold;
		}
		return true;
	}

	private void ResolveBlockIds(ICoreServerAPI api, RockStrataConfig rockstrata)
	{
		if (BlockCode != null && BlockCode.Path.Length > 0)
		{
			if (BlockCode.Path.Contains("{rocktype}"))
			{
				BlockIdMapping = new Dictionary<int, int>();
				for (int i = 0; i < rockstrata.Variants.Length; i++)
				{
					if (!rockstrata.Variants[i].IsDeposit)
					{
						string rocktype = rockstrata.Variants[i].BlockCode.Path.Split('-')[1];
						Block rockBlock = api.World.GetBlock(rockstrata.Variants[i].BlockCode);
						Block rocktypedBlock = api.World.GetBlock(BlockCode.CopyWithPath(BlockCode.Path.Replace("{rocktype}", rocktype)));
						if (rockBlock != null && rocktypedBlock != null)
						{
							BlockIdMapping[rockBlock.BlockId] = rocktypedBlock.BlockId;
						}
					}
				}
			}
			else
			{
				BlockId = api.WorldManager.GetBlockId(BlockCode);
			}
		}
		else
		{
			BlockCode = null;
		}
		if (BlockCodeByMin != null)
		{
			for (int j = 0; j < BlockCodeByMin.Length; j++)
			{
				AssetLocation blockCode = BlockCodeByMin[j].BlockCode;
				if (blockCode.Path.Contains("{rocktype}"))
				{
					BlockCodeByMin[j].BlockIdMapping = new Dictionary<int, int>();
					for (int k = 0; k < rockstrata.Variants.Length; k++)
					{
						string rocktype2 = rockstrata.Variants[k].BlockCode.Path.Split('-')[1];
						Block rockBlock2 = api.World.GetBlock(rockstrata.Variants[k].BlockCode);
						Block rocktypedBlock2 = api.World.GetBlock(blockCode.CopyWithPath(blockCode.Path.Replace("{rocktype}", rocktype2)));
						if (rockBlock2 != null && rocktypedBlock2 != null)
						{
							BlockCodeByMin[j].BlockIdMapping[rockBlock2.BlockId] = rocktypedBlock2.BlockId;
						}
					}
				}
				else
				{
					BlockCodeByMin[j].BlockId = api.WorldManager.GetBlockId(blockCode);
				}
			}
		}
		if (BlockCodeByBiome == null)
		{
			return;
		}
		for (int l = 0; l < BlockCodeByBiome.Length; l++)
		{
			AssetLocation blockCode2 = BlockCodeByBiome[l].BlockCode;
			if (blockCode2 != null && blockCode2.Path.Contains("{rocktype}"))
			{
				BlockCodeByBiome[l].BlockIdMapping = new Dictionary<int, int>();
				for (int m = 0; m < rockstrata.Variants.Length; m++)
				{
					string rocktype3 = rockstrata.Variants[m].BlockCode.Path.Split('-')[1];
					Block rockBlock3 = api.World.GetBlock(rockstrata.Variants[m].BlockCode);
					Block rocktypedBlock3 = api.World.GetBlock(blockCode2.CopyWithPath(blockCode2.Path.Replace("{rocktype}", rocktype3)));
					if (rockBlock3 != null && rocktypedBlock3 != null)
					{
						BlockCodeByBiome[l].BlockIdMapping[rockBlock3.BlockId] = rocktypedBlock3.BlockId;
					}
				}
			}
			else if (blockCode2 != null)
			{
				BlockCodeByBiome[l].BlockId = api.WorldManager.GetBlockId(blockCode2);
			}
		}
	}

	public int GetBlockId(double posRand, float temp, float rainRel, float fertilityRel, int firstBlockId, BlockPos pos, int mapheight, int biome = -1)
	{
		if (noiseGen != null && noiseGen.Noise((double)pos.X / 20.0, (double)pos.Y / 20.0, (double)pos.Z / 20.0) < NoiseThreshold)
		{
			return 0;
		}
		if (BlockCode != null)
		{
			int mapppedBlockId = BlockId;
			if (BlockIdMapping != null)
			{
				BlockIdMapping.TryGetValue(firstBlockId, out mapppedBlockId);
			}
			return mapppedBlockId;
		}
		float yrel = (float)pos.Y / (float)mapheight;
		if (BlockCodeByBiome != null)
		{
			for (int i = 0; i < BlockCodeByBiome.Length; i++)
			{
				BlockLayerCodeByBiome blcv = BlockCodeByBiome[i];
				if (blcv.Biome == biome && (double)(Math.Abs(yrel - GameMath.Clamp(yrel, blcv.MinY, blcv.MaxY)) * 10f) <= posRand)
				{
					int mapppedBlockId2 = blcv.BlockId;
					if (blcv.BlockIdMapping != null)
					{
						blcv.BlockIdMapping.TryGetValue(firstBlockId, out mapppedBlockId2);
					}
					return mapppedBlockId2;
				}
			}
		}
		if (BlockCodeByMin != null)
		{
			for (int j = 0; j < BlockCodeByMin.Length; j++)
			{
				BlockLayerCodeByMin blcv2 = BlockCodeByMin[j];
				float ydist = Math.Abs(yrel - GameMath.Clamp(yrel, blcv2.MinY, blcv2.MaxY)) * 10f;
				float num = Math.Abs(temp - GameMath.Max(temp, blcv2.MinTemp));
				float rainDist = Math.Abs(rainRel - GameMath.Max(rainRel, blcv2.MinRain));
				float fertDist = Math.Abs(fertilityRel - GameMath.Clamp(fertilityRel, blcv2.MinFertility, blcv2.MaxFertility));
				if ((double)(num + rainDist + fertDist + ydist) <= posRand)
				{
					int mapppedBlockId3 = blcv2.BlockId;
					if (blcv2.BlockIdMapping != null)
					{
						blcv2.BlockIdMapping.TryGetValue(firstBlockId, out mapppedBlockId3);
					}
					return mapppedBlockId3;
				}
			}
		}
		return 0;
	}

	public float CalcTrfDistance(float temperature, float rainRel, float fertilityRel)
	{
		float num = Math.Abs(temperature - GameMath.Clamp(temperature, MinTemp, MaxTemp));
		float rainDist = Math.Abs(rainRel - GameMath.Clamp(rainRel, MinRain, MaxRain)) * 10f;
		float fertDist = Math.Abs(fertilityRel - GameMath.Clamp(fertilityRel, MinFertility, MaxFertility)) * 10f;
		return num + rainDist + fertDist;
	}

	public float CalcYDistance(int posY, int mapheight)
	{
		float num = (float)posY / (float)mapheight;
		return Math.Abs(num - GameMath.Clamp(num, MinY, MaxY)) * 10f;
	}
}
