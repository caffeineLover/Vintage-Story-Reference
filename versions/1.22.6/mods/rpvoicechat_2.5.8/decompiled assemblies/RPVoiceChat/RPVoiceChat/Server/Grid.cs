using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace RPVoiceChat.Server
{
	// Token: 0x02000040 RID: 64
	internal sealed class Grid
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000C320 File Offset: 0x0000A520
		public int CellSize { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000C328 File Offset: 0x0000A528
		public int CellShift { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000C330 File Offset: 0x0000A530
		public IReadOnlyList<GridPlayer> Players
		{
			get
			{
				return this.players;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000C338 File Offset: 0x0000A538
		public bool IsEmpty
		{
			get
			{
				return this.players.Length == 0;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000C344 File Offset: 0x0000A544
		private Grid(int cellSizeBlocks, GridPlayer[] players)
		{
			this.CellSize = Grid.SanitizeCellSize(cellSizeBlocks);
			int shift = 0;
			int value = this.CellSize;
			while ((value >>= 1) != 0)
			{
				shift++;
			}
			this.CellShift = shift;
			this.players = (players ?? Array.Empty<GridPlayer>());
			this.playerIndexByUid = new Dictionary<string, int>(this.players.Length);
			this.cellsByDimension = new Dictionary<int, Dictionary<long, List<int>>>();
			for (int index = 0; index < this.players.Length; index++)
			{
				GridPlayer player = this.players[index];
				if (!string.IsNullOrEmpty(player.PlayerUID))
				{
					this.playerIndexByUid[player.PlayerUID] = index;
				}
				this.AddToCell(index, player.Dimension, this.CoordToCell(player.X), this.CoordToCell(player.Z));
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000C414 File Offset: 0x0000A614
		public static Grid Build(ICoreServerAPI api, int cellSizeBlocks)
		{
			if (((api != null) ? api.World : null) == null)
			{
				return Grid.Empty;
			}
			List<GridPlayer> snapshots = new List<GridPlayer>();
			IPlayer[] allOnlinePlayers = api.World.AllOnlinePlayers;
			for (int i = 0; i < allOnlinePlayers.Length; i++)
			{
				IServerPlayer player = allOnlinePlayers[i] as IServerPlayer;
				if (player != null && player.Entity != null && player.Entity.Pos != null && player.ConnectionState == 4)
				{
					EntityPos pos = player.Entity.Pos;
					snapshots.Add(new GridPlayer(player, pos.Dimension, pos.X, pos.Y, pos.Z, player.WorldData.CurrentGameMode == 3));
				}
			}
			if (snapshots.Count != 0)
			{
				return new Grid(cellSizeBlocks, snapshots.ToArray());
			}
			return Grid.Empty;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000C4DC File Offset: 0x0000A6DC
		public bool TryGetPlayer(string playerUid, out GridPlayer player)
		{
			int index;
			if (!string.IsNullOrEmpty(playerUid) && this.playerIndexByUid.TryGetValue(playerUid, ref index) && index >= 0 && index < this.players.Length)
			{
				player = this.players[index];
				return true;
			}
			player = default(GridPlayer);
			return false;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000C52C File Offset: 0x0000A72C
		public int CollectNear(int dimension, double x, double z, int radiusBlocks, List<GridPlayer> into, bool clear = true)
		{
			if (into == null)
			{
				throw new ArgumentNullException("into");
			}
			if (clear)
			{
				into.Clear();
			}
			if (radiusBlocks < 0)
			{
				return into.Count;
			}
			Dictionary<long, List<int>> dimensionCells;
			if (!this.cellsByDimension.TryGetValue(dimension, ref dimensionCells))
			{
				return into.Count;
			}
			int centerCellX = this.CoordToCell(x);
			int centerCellZ = this.CoordToCell(z);
			int radiusCells = radiusBlocks + (this.CellSize - 1) >> this.CellShift;
			for (int cellZ = centerCellZ - radiusCells; cellZ <= centerCellZ + radiusCells; cellZ++)
			{
				for (int cellX = centerCellX - radiusCells; cellX <= centerCellX + radiusCells; cellX++)
				{
					List<int> bucket;
					if (dimensionCells.TryGetValue(Grid.CellKey(cellX, cellZ), ref bucket))
					{
						for (int i = 0; i < bucket.Count; i++)
						{
							int playerIndex = bucket[i];
							if (playerIndex < this.players.Length)
							{
								into.Add(this.players[playerIndex]);
							}
						}
					}
				}
			}
			return into.Count;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000C621 File Offset: 0x0000A821
		public int CoordToCell(double coord)
		{
			return this.BlockToCell((int)Math.Floor(coord));
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000C630 File Offset: 0x0000A830
		public int BlockToCell(int blockCoord)
		{
			return blockCoord >> this.CellShift;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000C63D File Offset: 0x0000A83D
		public static long CellKey(int cellX, int cellZ)
		{
			return (long)cellX << 32 | (long)((ulong)cellZ);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000C648 File Offset: 0x0000A848
		public static int SanitizeCellSize(int cellSizeBlocks)
		{
			if (cellSizeBlocks < 8)
			{
				cellSizeBlocks = 8;
			}
			if (cellSizeBlocks > 512)
			{
				cellSizeBlocks = 512;
			}
			int powerOfTwo;
			for (powerOfTwo = 1; powerOfTwo < cellSizeBlocks; powerOfTwo <<= 1)
			{
			}
			return powerOfTwo;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000C678 File Offset: 0x0000A878
		private void AddToCell(int playerIndex, int dimension, int cellX, int cellZ)
		{
			Dictionary<long, List<int>> dimensionCells;
			if (!this.cellsByDimension.TryGetValue(dimension, ref dimensionCells))
			{
				dimensionCells = new Dictionary<long, List<int>>(256);
				this.cellsByDimension[dimension] = dimensionCells;
			}
			long key = Grid.CellKey(cellX, cellZ);
			List<int> bucket;
			if (!dimensionCells.TryGetValue(key, ref bucket))
			{
				bucket = new List<int>(4);
				dimensionCells[key] = bucket;
			}
			bucket.Add(playerIndex);
		}

		// Token: 0x040000A7 RID: 167
		public static readonly Grid Empty = new Grid(64, Array.Empty<GridPlayer>());

		// Token: 0x040000A8 RID: 168
		private readonly GridPlayer[] players;

		// Token: 0x040000A9 RID: 169
		private readonly Dictionary<string, int> playerIndexByUid;

		// Token: 0x040000AA RID: 170
		private readonly Dictionary<int, Dictionary<long, List<int>>> cellsByDimension;
	}
}
