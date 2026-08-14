using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000034 RID: 52
	public static class WarningBeaconStructure
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00008A2F File Offset: 0x00006C2F
		public static BlockPos CenterLocal
		{
			get
			{
				return new BlockPos(0, 0, 3);
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00008A3C File Offset: 0x00006C3C
		static WarningBeaconStructure()
		{
			WarningBeaconStructure._structurePositions.Add(new BlockPos(0, -1, 0));
			for (int x = -2; x <= 2; x++)
			{
				for (int z = 1; z <= 5; z++)
				{
					WarningBeaconStructure._structurePositions.Add(new BlockPos(x, -1, z));
				}
			}
			for (int x2 = -1; x2 <= 1; x2++)
			{
				for (int z2 = 2; z2 <= 4; z2++)
				{
					WarningBeaconStructure._reservedLocal3x3.Add(new BlockPos(x2, 0, z2));
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00008AD8 File Offset: 0x00006CD8
		public static IReadOnlyList<BlockPos> StructurePositions
		{
			get
			{
				return WarningBeaconStructure._structurePositions;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00008ADF File Offset: 0x00006CDF
		public static IReadOnlyList<BlockPos> ReservedLocal3x3
		{
			get
			{
				return WarningBeaconStructure._reservedLocal3x3;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008AE8 File Offset: 0x00006CE8
		public static BlockPos LocalToWorld(BlockPos lucernePos, int localX, int localY, int localZ, BlockFacing facing)
		{
			int wx = lucernePos.X;
			int wy = lucernePos.Y + localY;
			int wz = lucernePos.Z;
			string text = ((facing != null) ? facing.Code : null) ?? "north";
			if (!(text == "north"))
			{
				if (!(text == "south"))
				{
					if (!(text == "east"))
					{
						if (!(text == "west"))
						{
							wx += localX;
							wz -= localZ;
						}
						else
						{
							wx -= localZ;
							wz -= localX;
						}
					}
					else
					{
						wx += localZ;
						wz += localX;
					}
				}
				else
				{
					wx -= localX;
					wz += localZ;
				}
			}
			else
			{
				wx += localX;
				wz -= localZ;
			}
			return new BlockPos(wx, wy, wz);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008B94 File Offset: 0x00006D94
		public static bool IsValidConstructionBlock(Block block)
		{
			if (block == null || block.Id == 0)
			{
				return false;
			}
			AssetLocation code = block.Code;
			string path = (((code != null) ? code.Path : null) ?? "").ToLowerInvariant();
			string mat = (block.BlockMaterial.ToString() ?? "").ToLowerInvariant();
			return mat == "stone" || mat == "rock" || (path.Contains("stone") || path.Contains("brick") || path.Contains("cobble") || path.Contains("rock") || path.Contains("fireclay") || path.Contains("refractory"));
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00008C5C File Offset: 0x00006E5C
		public static bool SatisfiesChiselVoxelRequirement(IBlockAccessor accessor, BlockPos pos, Block block)
		{
			if (block == null || block.Id == 0)
			{
				return false;
			}
			if (!WarningBeaconStructure.IsValidConstructionBlock(block))
			{
				return false;
			}
			AssetLocation code = block.Code;
			string path = ((code != null) ? code.Path : null) ?? "";
			if (path.IndexOf("microblock", 5) < 0 && path.IndexOf("chisel", 5) < 0)
			{
				return true;
			}
			BlockEntity be = accessor.GetBlockEntity(pos);
			int filled;
			int maxVoxels;
			return be != null && WarningBeaconStructure.TryGetFilledVoxelCount(be, out filled, out maxVoxels) && maxVoxels > 0 && filled >= maxVoxels / 2;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008CF0 File Offset: 0x00006EF0
		private static bool TryGetFilledVoxelCount(object be, out int filled, out int maxVoxels)
		{
			filled = 0;
			maxVoxels = 0;
			if (be == null)
			{
				return false;
			}
			Type type = be.GetType();
			PropertyInfo propertyInfo;
			if ((propertyInfo = type.GetProperty("TotalVoxels")) == null)
			{
				propertyInfo = (type.GetProperty("VoxelCount") ?? type.GetProperty("FilledVoxels"));
			}
			PropertyInfo countProp = propertyInfo;
			if (countProp != null && countProp.PropertyType == typeof(int))
			{
				try
				{
					filled = (int)countProp.GetValue(be);
					maxVoxels = ((type.Name.IndexOf("MicroBlock", 5) >= 0) ? 512 : 4096);
					return true;
				}
				catch
				{
					return false;
				}
			}
			FieldInfo voxelsField = type.GetField("Voxels", 52);
			if (voxelsField != null)
			{
				try
				{
					Array arr = voxelsField.GetValue(be) as Array;
					if (arr != null && arr.Rank == 3)
					{
						maxVoxels = arr.Length;
						int i = 0;
						foreach (object v in arr)
						{
							bool b;
							bool flag;
							if (v is bool)
							{
								b = (bool)v;
								flag = true;
							}
							else
							{
								flag = false;
							}
							if (flag && b)
							{
								i++;
							}
						}
						filled = i;
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00008E68 File Offset: 0x00007068
		public static bool ValidateStructure(IBlockAccessor accessor, BlockPos lucernePos, BlockFacing facing, Block lucerneBlock, out int filled, out int total)
		{
			total = WarningBeaconStructure._structurePositions.Count;
			filled = 0;
			foreach (BlockPos local in WarningBeaconStructure._structurePositions)
			{
				BlockPos worldPos = WarningBeaconStructure.LocalToWorld(lucernePos, local.X, local.Y, local.Z, facing);
				Block block = accessor.GetBlock(worldPos);
				if (WarningBeaconStructure.SatisfiesChiselVoxelRequirement(accessor, worldPos, block))
				{
					filled++;
				}
			}
			return filled == total;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008F00 File Offset: 0x00007100
		public static void RegisterLucerne(BlockPos lucernePos, BlockFacing facing)
		{
			object @lock = WarningBeaconStructure._lock;
			lock (@lock)
			{
				for (int i = 0; i < WarningBeaconStructure._registeredLucernes.Count; i++)
				{
					if (WarningBeaconStructure._registeredLucernes[i].Item1.Equals(lucernePos))
					{
						return;
					}
				}
				WarningBeaconStructure._registeredLucernes.Add(new ValueTuple<BlockPos, BlockFacing>(lucernePos.Copy(), facing));
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008F80 File Offset: 0x00007180
		public static void UnregisterLucerne(BlockPos lucernePos)
		{
			object @lock = WarningBeaconStructure._lock;
			lock (@lock)
			{
				for (int i = WarningBeaconStructure._registeredLucernes.Count - 1; i >= 0; i--)
				{
					if (WarningBeaconStructure._registeredLucernes[i].Item1.Equals(lucernePos))
					{
						WarningBeaconStructure._registeredLucernes.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008FF4 File Offset: 0x000071F4
		public static bool IsInReservedZone(IBlockAccessor accessor, BlockPos worldPos)
		{
			object @lock = WarningBeaconStructure._lock;
			lock (@lock)
			{
				foreach (ValueTuple<BlockPos, BlockFacing> valueTuple in WarningBeaconStructure._registeredLucernes)
				{
					BlockPos lucernePos = valueTuple.Item1;
					BlockFacing facing = valueTuple.Item2;
					foreach (BlockPos local in WarningBeaconStructure._reservedLocal3x3)
					{
						BlockPos w = WarningBeaconStructure.LocalToWorld(lucernePos, local.X, local.Y, local.Z, facing);
						if (w.X == worldPos.X && w.Z == worldPos.Z && worldPos.Y >= w.Y && worldPos.Y <= w.Y + 2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04000076 RID: 118
		public const int SizeX = 5;

		// Token: 0x04000077 RID: 119
		public const int SizeZ = 5;

		// Token: 0x04000078 RID: 120
		private static readonly List<BlockPos> _structurePositions = new List<BlockPos>();

		// Token: 0x04000079 RID: 121
		private static readonly List<BlockPos> _reservedLocal3x3 = new List<BlockPos>();

		// Token: 0x0400007A RID: 122
		[TupleElementNames(new string[]
		{
			"lucernePos",
			"facing"
		})]
		private static readonly List<ValueTuple<BlockPos, BlockFacing>> _registeredLucernes = new List<ValueTuple<BlockPos, BlockFacing>>();

		// Token: 0x0400007B RID: 123
		private static readonly object _lock = new object();
	}
}
