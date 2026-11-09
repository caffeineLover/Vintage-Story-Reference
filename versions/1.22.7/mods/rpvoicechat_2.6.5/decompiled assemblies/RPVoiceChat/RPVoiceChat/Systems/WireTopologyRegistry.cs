using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000059 RID: 89
	public static class WireTopologyRegistry
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000E711 File Offset: 0x0000C911
		public static int EdgeCount
		{
			get
			{
				return WireTopologyRegistry.edges.Count;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000E720 File Offset: 0x0000C920
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

		// Token: 0x0600022E RID: 558 RVA: 0x0000E78B File Offset: 0x0000C98B
		public static void Clear()
		{
			WireTopologyRegistry.edges.Clear();
			WireTopologyRegistry.adjacency.Clear();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000E7A4 File Offset: 0x0000C9A4
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

		// Token: 0x06000230 RID: 560 RVA: 0x0000E868 File Offset: 0x0000CA68
		public static byte[] ToSaveBytes()
		{
			return SerializerUtil.Serialize<WireTopologyData>(new WireTopologyData
			{
				Edges = new List<WireEdge>(WireTopologyRegistry.edges)
			});
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E884 File Offset: 0x0000CA84
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

		// Token: 0x06000232 RID: 562 RVA: 0x0000E8DA File Offset: 0x0000CADA
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

		// Token: 0x06000233 RID: 563 RVA: 0x0000E910 File Offset: 0x0000CB10
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

		// Token: 0x06000234 RID: 564 RVA: 0x0000E94D File Offset: 0x0000CB4D
		public static IEnumerable<BlockPos> GetNeighborPositions(BlockPos pos)
		{
			WireTopologyRegistry.<GetNeighborPositions>d__12 <GetNeighborPositions>d__ = new WireTopologyRegistry.<GetNeighborPositions>d__12(-2);
			<GetNeighborPositions>d__.<>3__pos = pos;
			return <GetNeighborPositions>d__;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000E960 File Offset: 0x0000CB60
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

		// Token: 0x06000236 RID: 566 RVA: 0x0000EA0C File Offset: 0x0000CC0C
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

		// Token: 0x06000237 RID: 567 RVA: 0x0000EAA4 File Offset: 0x0000CCA4
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

		// Token: 0x06000238 RID: 568 RVA: 0x0000EB24 File Offset: 0x0000CD24
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

		// Token: 0x06000239 RID: 569 RVA: 0x0000EBA8 File Offset: 0x0000CDA8
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

		// Token: 0x040000E0 RID: 224
		public const string TopologyDataKey = "rpvc:wire-topology";

		// Token: 0x040000E1 RID: 225
		private static readonly HashSet<WireEdge> edges = new HashSet<WireEdge>();

		// Token: 0x040000E2 RID: 226
		private static readonly Dictionary<string, HashSet<string>> adjacency = new Dictionary<string, HashSet<string>>();
	}
}
