using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003E RID: 62
	public static class WireTopologyRegistry
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000AD7D File Offset: 0x00008F7D
		public static int EdgeCount
		{
			get
			{
				return WireTopologyRegistry.edges.Count;
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000AD8C File Offset: 0x00008F8C
		private static string PosKey(BlockPos pos)
		{
			if (!(pos == null))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(2, 3);
				defaultInterpolatedStringHandler.AppendFormatted<int>(pos.X);
				defaultInterpolatedStringHandler.AppendLiteral("|");
				defaultInterpolatedStringHandler.AppendFormatted<int>(pos.Y);
				defaultInterpolatedStringHandler.AppendLiteral("|");
				defaultInterpolatedStringHandler.AppendFormatted<int>(pos.Z);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return "";
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000ADF7 File Offset: 0x00008FF7
		public static void Clear()
		{
			WireTopologyRegistry.edges.Clear();
			WireTopologyRegistry.adjacency.Clear();
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000AE10 File Offset: 0x00009010
		public static void LoadFromSave(byte[] data)
		{
			WireTopologyRegistry.Clear();
			if (data == null || data.Length == 0)
			{
				return;
			}
			WireTopologyData topology = SerializerUtil.Deserialize<WireTopologyData>(data);
			if (((topology != null) ? topology.Edges : null) == null)
			{
				return;
			}
			foreach (WireEdge edge in topology.Edges)
			{
				if (!(((edge != null) ? edge.PosA : null) == null) && !(((edge != null) ? edge.PosB : null) == null) && !edge.PosA.Equals(edge.PosB))
				{
					WireTopologyRegistry.AddEdge(edge.PosA, edge.PosB, false);
				}
			}
			WireTopologyRegistry.RebuildAdjacency();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000AED4 File Offset: 0x000090D4
		public static byte[] ToSaveBytes()
		{
			return SerializerUtil.Serialize<WireTopologyData>(new WireTopologyData
			{
				Edges = new List<WireEdge>(WireTopologyRegistry.edges)
			});
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000AEF0 File Offset: 0x000090F0
		public static bool AddEdge(BlockPos a, BlockPos b, bool rebuildAdjacency = true)
		{
			if (a == null || b == null || a.Equals(b))
			{
				return false;
			}
			WireEdge edge = new WireEdge(a, b);
			if (!WireTopologyRegistry.edges.Add(edge))
			{
				return false;
			}
			if (rebuildAdjacency)
			{
				WireTopologyRegistry.Link(a.Copy(), b.Copy());
			}
			return true;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000AF46 File Offset: 0x00009146
		public static bool RemoveEdge(BlockPos a, BlockPos b)
		{
			if (a == null || b == null)
			{
				return false;
			}
			if (!WireTopologyRegistry.edges.Remove(new WireEdge(a, b)))
			{
				return false;
			}
			WireTopologyRegistry.Unlink(a, b);
			return true;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000AF7C File Offset: 0x0000917C
		public static void RemoveAllEdgesAt(BlockPos pos)
		{
			if (pos == null)
			{
				return;
			}
			foreach (BlockPos neighbor in Enumerable.ToArray<BlockPos>(WireTopologyRegistry.GetNeighborPositions(pos)))
			{
				WireTopologyRegistry.RemoveEdge(pos, neighbor);
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000AFB9 File Offset: 0x000091B9
		public static IEnumerable<BlockPos> GetNeighborPositions(BlockPos pos)
		{
			WireTopologyRegistry.<GetNeighborPositions>d__12 <GetNeighborPositions>d__ = new WireTopologyRegistry.<GetNeighborPositions>d__12(-2);
			<GetNeighborPositions>d__.<>3__pos = pos;
			return <GetNeighborPositions>d__;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000AFCC File Offset: 0x000091CC
		public static HashSet<BlockPos> GetConnectedComponent(BlockPos start)
		{
			HashSet<BlockPos> component = new HashSet<BlockPos>();
			if (start == null)
			{
				return component;
			}
			Queue<BlockPos> queue = new Queue<BlockPos>();
			BlockPos startCopy = start.Copy();
			queue.Enqueue(startCopy);
			component.Add(startCopy);
			while (queue.Count > 0)
			{
				foreach (BlockPos neighbor in WireTopologyRegistry.GetNeighborPositions(queue.Dequeue()))
				{
					if (!(neighbor == null) && component.Add(neighbor.Copy()))
					{
						queue.Enqueue(neighbor.Copy());
					}
				}
			}
			return component;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000B078 File Offset: 0x00009278
		private static void RebuildAdjacency()
		{
			WireTopologyRegistry.adjacency.Clear();
			foreach (WireEdge edge in WireTopologyRegistry.edges)
			{
				if (!(((edge != null) ? edge.PosA : null) == null) && !(((edge != null) ? edge.PosB : null) == null))
				{
					WireTopologyRegistry.Link(edge.PosA.Copy(), edge.PosB.Copy());
				}
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000B110 File Offset: 0x00009310
		private static void Link(BlockPos a, BlockPos b)
		{
			string keyA = WireTopologyRegistry.PosKey(a);
			string keyB = WireTopologyRegistry.PosKey(b);
			if (keyA.Length == 0 || keyB.Length == 0)
			{
				return;
			}
			HashSet<string> aNeighbors;
			if (!WireTopologyRegistry.adjacency.TryGetValue(keyA, ref aNeighbors))
			{
				aNeighbors = new HashSet<string>();
				WireTopologyRegistry.adjacency[keyA] = aNeighbors;
			}
			HashSet<string> bNeighbors;
			if (!WireTopologyRegistry.adjacency.TryGetValue(keyB, ref bNeighbors))
			{
				bNeighbors = new HashSet<string>();
				WireTopologyRegistry.adjacency[keyB] = bNeighbors;
			}
			aNeighbors.Add(keyB);
			bNeighbors.Add(keyA);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000B190 File Offset: 0x00009390
		private static void Unlink(BlockPos a, BlockPos b)
		{
			string keyA = WireTopologyRegistry.PosKey(a);
			string keyB = WireTopologyRegistry.PosKey(b);
			if (keyA.Length == 0 || keyB.Length == 0)
			{
				return;
			}
			HashSet<string> aNeighbors;
			if (WireTopologyRegistry.adjacency.TryGetValue(keyA, ref aNeighbors))
			{
				aNeighbors.Remove(keyB);
				if (aNeighbors.Count == 0)
				{
					WireTopologyRegistry.adjacency.Remove(keyA);
				}
			}
			HashSet<string> bNeighbors;
			if (WireTopologyRegistry.adjacency.TryGetValue(keyB, ref bNeighbors))
			{
				bNeighbors.Remove(keyA);
				if (bNeighbors.Count == 0)
				{
					WireTopologyRegistry.adjacency.Remove(keyB);
				}
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000B214 File Offset: 0x00009414
		private static BlockPos ParsePosKey(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			string[] parts = key.Split('|', 0);
			if (parts.Length != 3)
			{
				return null;
			}
			int x;
			int y;
			int z;
			if (!int.TryParse(parts[0], ref x) || !int.TryParse(parts[1], ref y) || !int.TryParse(parts[2], ref z))
			{
				return null;
			}
			return new BlockPos(x, y, z);
		}

		// Token: 0x04000095 RID: 149
		public const string TopologyDataKey = "rpvc:wire-topology";

		// Token: 0x04000096 RID: 150
		private static readonly HashSet<WireEdge> edges = new HashSet<WireEdge>();

		// Token: 0x04000097 RID: 151
		private static readonly Dictionary<string, HashSet<string>> adjacency = new Dictionary<string, HashSet<string>>();
	}
}
