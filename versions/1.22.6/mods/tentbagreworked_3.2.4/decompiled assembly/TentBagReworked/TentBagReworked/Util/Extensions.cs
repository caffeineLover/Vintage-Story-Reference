using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace TentBagReworked.Util
{
	// Token: 0x02000004 RID: 4
	[NullableContext(1)]
	[Nullable(0)]
	public static class Extensions
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00003D90 File Offset: 0x00001F90
		public static int ToColor(this string value)
		{
			bool flag = value.StartsWith('#');
			if (flag)
			{
				value = value.Substring(1);
			}
			else
			{
				bool flag2 = value.StartsWith("0x", 1);
				if (flag2)
				{
					value = value.Substring(2);
				}
				else
				{
					bool flag3 = value.StartsWith("&h", 1);
					if (flag3)
					{
						value = value.Substring(2);
					}
				}
			}
			return int.Parse(value, 515);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003E00 File Offset: 0x00002000
		public static int Reverse(this int color)
		{
			int a = color >> 24 & 255;
			int r = color >> 16 & 255;
			int g = color >> 8 & 255;
			int b = color & 255;
			return a << 24 | b << 16 | g << 8 | r;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003E4C File Offset: 0x0000204C
		public static void ReduceOnlySaturation(this EntityPlayer entity, float amount)
		{
			EntityBehaviorHunger hunger = entity.GetBehavior<EntityBehaviorHunger>();
			bool flag = hunger == null;
			if (!flag)
			{
				hunger.Saturation = Math.Max(0f, hunger.Saturation - amount);
				FieldInfo fieldInfo;
				if ((fieldInfo = Extensions._sprintCounter) == null)
				{
					fieldInfo = (Extensions._sprintCounter = hunger.GetType().GetField("sprintCounter", 36));
				}
				FieldInfo fieldInfo2 = fieldInfo;
				if (fieldInfo2 != null)
				{
					fieldInfo2.SetValue(hunger, 0);
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003EBC File Offset: 0x000020BC
		public static void AddAreaWithoutEntities(this BlockSchematic bs, IWorldAccessor world, IBlockAccessor blockAccess, BlockPos start, BlockPos end)
		{
			BlockPos startPos = new BlockPos(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Min(start.Z, end.Z), start.dimension);
			BlockPos finalPos = new BlockPos(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y), Math.Max(start.Z, end.Z), start.dimension);
			bs.OriginalPos = start;
			BlockPos readPos = new BlockPos(start.dimension);
			for (int x = startPos.X; x < finalPos.X + 1; x++)
			{
				for (int y = startPos.Y; y < finalPos.Y + 1; y++)
				{
					for (int z = startPos.Z; z < finalPos.Z + 1; z++)
					{
						readPos.Set(x, y, z);
						int blockId = blockAccess.GetBlock(readPos, 1).BlockId;
						int fluidId = blockAccess.GetBlock(readPos, 2).BlockId;
						bool flag = fluidId == blockId;
						if (flag)
						{
							blockId = 0;
						}
						BlockPos keyPos = new BlockPos(x, y, z);
						bs.BlocksUnpacked[keyPos] = blockId;
						bs.FluidsLayerUnpacked[keyPos] = fluidId;
						BlockEntity blockEntity = blockAccess.GetBlockEntity(readPos);
						bool flag2 = blockEntity != null;
						if (flag2)
						{
							bool flag3 = blockEntity.Api == null;
							if (flag3)
							{
								blockEntity.Initialize(world.Api);
							}
							bs.BlockEntitiesUnpacked[keyPos] = bs.EncodeBlockEntityData(blockEntity);
							blockEntity.OnStoreCollectibleMappings(bs.BlockCodes, bs.ItemCodes);
						}
						Dictionary<int, Block> decors = blockAccess.GetSubDecors(readPos);
						bool flag4 = decors != null;
						if (flag4)
						{
							bs.DecorsUnpacked[keyPos] = decors;
						}
					}
				}
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000040C8 File Offset: 0x000022C8
		public static bool PackIncludingAir(this BlockSchematic bs, IWorldAccessor world, BlockPos startPos, int solidBlockCount)
		{
			bs.Indices.Clear();
			bs.BlockIds.Clear();
			bs.BlockEntities.Clear();
			bs.Entities.Clear();
			bs.DecorIndices.Clear();
			bs.DecorIds.Clear();
			bs.SizeX = 0;
			bs.SizeY = 0;
			bs.SizeZ = 0;
			int minX = int.MaxValue;
			int minY = int.MaxValue;
			int minZ = int.MaxValue;
			foreach (KeyValuePair<BlockPos, int> val in bs.BlocksUnpacked)
			{
				minX = Math.Min(minX, val.Key.X);
				minY = Math.Min(minY, val.Key.Y);
				minZ = Math.Min(minZ, val.Key.Z);
				int blockId = val.Value;
				int dx = val.Key.X - startPos.X;
				int dy = val.Key.Y - startPos.Y;
				int dz = val.Key.Z - startPos.Z;
				bool flag = dx >= 1024 || dy >= 1024 || dz >= 1024;
				if (flag)
				{
					world.Logger.Warning("Export format does not support areas larger than 1024 blocks in any direction. Will not pack.");
					bs.PackedOffset = new FastVec3i(0, 0, 0);
					return false;
				}
			}
			BlockPos blockPos;
			int num;
			foreach (KeyValuePair<BlockPos, int> keyValuePair in bs.BlocksUnpacked)
			{
				keyValuePair.Deconstruct(ref blockPos, ref num);
				BlockPos pos = blockPos;
				int blockid = num;
				int fluidid = CollectionExtensions.GetValueOrDefault<BlockPos, int>(bs.FluidsLayerUnpacked, pos, 0);
				bs.BlockCodes[blockid] = world.BlockAccessor.GetBlock(blockid).Code;
				bs.BlockCodes[fluidid] = world.BlockAccessor.GetBlock(fluidid).Code;
				int dx2 = pos.X - minX;
				int dy2 = pos.Y - minY;
				int dz2 = pos.Z - minZ;
				bs.SizeX = Math.Max(dx2, bs.SizeX);
				bs.SizeY = Math.Max(dy2, bs.SizeY);
				bs.SizeZ = Math.Max(dz2, bs.SizeZ);
				bs.Indices.Add((uint)(dy2 << 20 | dz2 << 10 | dx2));
				bool flag2 = fluidid == 0;
				if (flag2)
				{
					bs.BlockIds.Add(blockid);
				}
				else
				{
					bool flag3 = blockid == 0;
					if (flag3)
					{
						bs.BlockIds.Add(fluidid);
					}
					else
					{
						bs.BlockIds.Add(blockid);
						bs.Indices.Add((uint)(dy2 << 20 | dz2 << 10 | dx2));
						bs.BlockIds.Add(fluidid);
					}
				}
			}
			foreach (KeyValuePair<BlockPos, int> keyValuePair in bs.FluidsLayerUnpacked)
			{
				keyValuePair.Deconstruct(ref blockPos, ref num);
				BlockPos pos2 = blockPos;
				int blockId2 = num;
				bool flag4 = bs.BlocksUnpacked.ContainsKey(pos2);
				if (!flag4)
				{
					bs.BlockCodes[blockId2] = world.BlockAccessor.GetBlock(blockId2).Code;
					int dx3 = pos2.X - minX;
					int dy3 = pos2.Y - minY;
					int dz3 = pos2.Z - minZ;
					bs.SizeX = Math.Max(dx3, bs.SizeX);
					bs.SizeY = Math.Max(dy3, bs.SizeY);
					bs.SizeZ = Math.Max(dz3, bs.SizeZ);
					bs.Indices.Add((uint)(dy3 << 20 | dz3 << 10 | dx3));
					bs.BlockIds.Add(blockId2);
				}
			}
			foreach (KeyValuePair<BlockPos, Dictionary<int, Block>> keyValuePair2 in bs.DecorsUnpacked)
			{
				Dictionary<int, Block> dictionary;
				keyValuePair2.Deconstruct(ref blockPos, ref dictionary);
				BlockPos pos3 = blockPos;
				Dictionary<int, Block> decors = dictionary;
				int dx4 = pos3.X - minX;
				int dy4 = pos3.Y - minY;
				int dz4 = pos3.Z - minZ;
				bs.SizeX = Math.Max(dx4, bs.SizeX);
				bs.SizeY = Math.Max(dy4, bs.SizeY);
				bs.SizeZ = Math.Max(dz4, bs.SizeZ);
				foreach (KeyValuePair<int, Block> keyValuePair3 in decors)
				{
					Block block;
					keyValuePair3.Deconstruct(ref num, ref block);
					int faceAndSubposition = num;
					Block decorBlock = block;
					bs.BlockCodes[decorBlock.BlockId] = decorBlock.Code;
					bs.DecorIndices.Add((uint)(dy4 << 20 | dz4 << 10 | dx4));
					bs.DecorIds.Add(((long)faceAndSubposition << 24) + (long)decorBlock.BlockId);
				}
			}
			bs.SizeX++;
			bs.SizeY++;
			bs.SizeZ++;
			foreach (KeyValuePair<BlockPos, string> val2 in bs.BlockEntitiesUnpacked)
			{
				int dx5 = val2.Key.X - minX;
				int dy5 = val2.Key.Y - minY;
				int dz5 = val2.Key.Z - minZ;
				bs.BlockEntities[(uint)(dy5 << 20 | dz5 << 10 | dx5)] = val2.Value;
			}
			BlockPos minPos = new BlockPos(minX, minY, minZ);
			foreach (Entity e in bs.EntitiesUnpacked)
			{
				using (MemoryStream ms = new MemoryStream())
				{
					BinaryWriter writer = new BinaryWriter(ms);
					writer.Write(world.ClassRegistry.GetEntityClassName(e.GetType()));
					e.WillExport(minPos);
					e.ToBytes(writer, false);
					e.DidImportOrExport(minPos);
					bs.Entities.Add(Ascii85.Encode(ms.ToArray()));
				}
			}
			bs.PackedOffset = new FastVec3i(minX - startPos.X, minY - startPos.Y, minZ - startPos.Z);
			bs.BlocksUnpacked.Clear();
			bs.FluidsLayerUnpacked.Clear();
			bs.DecorsUnpacked.Clear();
			bs.BlockEntitiesUnpacked.Clear();
			bs.EntitiesUnpacked.Clear();
			world.Logger.Debug("Packing schematic with {0} solid blocks", new object[]
			{
				solidBlockCount
			});
			return true;
		}

		// Token: 0x04000028 RID: 40
		[Nullable(2)]
		private static FieldInfo _sprintCounter;
	}
}
