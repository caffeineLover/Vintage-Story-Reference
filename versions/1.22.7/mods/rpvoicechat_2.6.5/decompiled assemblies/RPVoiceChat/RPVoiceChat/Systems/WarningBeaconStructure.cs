using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200004E RID: 78
	public static class WarningBeaconStructure
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000C1F7 File Offset: 0x0000A3F7
		public static BlockPos CenterLocal
		{
			get
			{
				return new BlockPos(0, 0, 3);
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000C204 File Offset: 0x0000A404
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

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000C2A0 File Offset: 0x0000A4A0
		public static IReadOnlyList<BlockPos> StructurePositions
		{
			get
			{
				return WarningBeaconStructure._structurePositions;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000C2A7 File Offset: 0x0000A4A7
		public static IReadOnlyList<BlockPos> ReservedLocal3x3
		{
			get
			{
				return WarningBeaconStructure._reservedLocal3x3;
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000C2B0 File Offset: 0x0000A4B0
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

		// Token: 0x060001CC RID: 460 RVA: 0x0000C35C File Offset: 0x0000A55C
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

		// Token: 0x060001CD RID: 461 RVA: 0x0000C424 File Offset: 0x0000A624
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

		// Token: 0x060001CE RID: 462 RVA: 0x0000C4B8 File Offset: 0x0000A6B8
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

		// Token: 0x060001CF RID: 463 RVA: 0x0000C630 File Offset: 0x0000A830
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

		// Token: 0x060001D0 RID: 464 RVA: 0x0000C6C8 File Offset: 0x0000A8C8
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

		// Token: 0x060001D1 RID: 465 RVA: 0x0000C748 File Offset: 0x0000A948
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

		// Token: 0x060001D2 RID: 466 RVA: 0x0000C7BC File Offset: 0x0000A9BC
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

		// Token: 0x040000C1 RID: 193
		public const int SizeX = 5;

		// Token: 0x040000C2 RID: 194
		public const int SizeZ = 5;

		// Token: 0x040000C3 RID: 195
		private static readonly List<BlockPos> _structurePositions = new List<BlockPos>();

		// Token: 0x040000C4 RID: 196
		private static readonly List<BlockPos> _reservedLocal3x3 = new List<BlockPos>();

		// Token: 0x040000C5 RID: 197
		[TupleElementNames(new string[]
		{
			"lucernePos",
			"facing"
		})]
		private static readonly List<ValueTuple<BlockPos, BlockFacing>> _registeredLucernes = new List<ValueTuple<BlockPos, BlockFacing>>();

		// Token: 0x040000C6 RID: 198
		private static readonly object _lock = new object();
	}
}
