using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace RPVoiceChat.Server
{
	// Token: 0x0200005B RID: 91
	internal sealed class Grid
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000FD98 File Offset: 0x0000DF98
		public int CellSize { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000FDA0 File Offset: 0x0000DFA0
		public int CellShift { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000FDA8 File Offset: 0x0000DFA8
		public IReadOnlyList<GridPlayer> Players
		{
			get
			{
				return this.players;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000FDB0 File Offset: 0x0000DFB0
		public bool IsEmpty
		{
			get
			{
				return this.players.Length == 0;
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000FDBC File Offset: 0x0000DFBC
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

		// Token: 0x0600025B RID: 603 RVA: 0x0000FE8C File Offset: 0x0000E08C
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

		// Token: 0x0600025C RID: 604 RVA: 0x0000FF54 File Offset: 0x0000E154
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

		// Token: 0x0600025D RID: 605 RVA: 0x0000FFA4 File Offset: 0x0000E1A4
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

		// Token: 0x0600025E RID: 606 RVA: 0x00010099 File Offset: 0x0000E299
		public int CoordToCell(double coord)
		{
			return this.BlockToCell((int)Math.Floor(coord));
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000100A8 File Offset: 0x0000E2A8
		public int BlockToCell(int blockCoord)
		{
			return blockCoord >> this.CellShift;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000100B5 File Offset: 0x0000E2B5
		public static long CellKey(int cellX, int cellZ)
		{
			return (long)cellX << 32 | (long)((ulong)cellZ);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000100C0 File Offset: 0x0000E2C0
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

		// Token: 0x06000262 RID: 610 RVA: 0x000100F0 File Offset: 0x0000E2F0
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

		// Token: 0x040000F4 RID: 244
		public static readonly Grid Empty = new Grid(64, Array.Empty<GridPlayer>());

		// Token: 0x040000F5 RID: 245
		private readonly GridPlayer[] players;

		// Token: 0x040000F6 RID: 246
		private readonly Dictionary<string, int> playerIndexByUid;

		// Token: 0x040000F7 RID: 247
		private readonly Dictionary<int, Dictionary<long, List<int>>> cellsByDimension;
	}
}
