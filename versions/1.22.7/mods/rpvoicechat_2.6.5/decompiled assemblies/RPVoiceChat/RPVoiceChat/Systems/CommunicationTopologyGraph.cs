using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Util;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000030 RID: 48
	public class CommunicationTopologyGraph
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00008A89 File Offset: 0x00006C89
		public int LinkCount
		{
			get
			{
				return this.links.Count;
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00008A96 File Offset: 0x00006C96
		public void Clear()
		{
			this.links.Clear();
			this.adjacency.Clear();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00008AAE File Offset: 0x00006CAE
		public void LoadFromBytes(byte[] data)
		{
			this.Clear();
			if (data == null || data.Length == 0)
			{
				return;
			}
			this.LoadFromData(SerializerUtil.Deserialize<CommunicationTopologyData>(data));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008ACC File Offset: 0x00006CCC
		public void LoadFromData(CommunicationTopologyData payload)
		{
			this.Clear();
			if (((payload != null) ? payload.Links : null) == null)
			{
				return;
			}
			foreach (TopologyLink link in payload.Links)
			{
				if (link != null && link.IsValid)
				{
					this.AddLink(TopologyNodeRef.FromKey(link.NodeKeyA), TopologyNodeRef.FromKey(link.NodeKeyB), link.Kind, false);
				}
			}
			this.RebuildAdjacency();
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00008B64 File Offset: 0x00006D64
		public byte[] ToSaveBytes()
		{
			return SerializerUtil.Serialize<CommunicationTopologyData>(new CommunicationTopologyData
			{
				Links = Enumerable.ToList<TopologyLink>(this.links)
			});
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008B84 File Offset: 0x00006D84
		public bool AddLink(TopologyNodeRef a, TopologyNodeRef b, TopologyLinkKind kind, bool rebuildAdjacency = true)
		{
			TopologyLink link = new TopologyLink(a, b, kind);
			if (!link.IsValid || !this.links.Add(link))
			{
				return false;
			}
			if (rebuildAdjacency)
			{
				this.LinkNodes(link.NodeKeyA, link.NodeKeyB);
			}
			return true;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00008BCC File Offset: 0x00006DCC
		public bool RemoveLink(TopologyNodeRef a, TopologyNodeRef b, TopologyLinkKind kind)
		{
			if (a == null || b == null || !a.IsValid || !b.IsValid)
			{
				return false;
			}
			if (!this.links.Remove(new TopologyLink(a, b, kind)))
			{
				return false;
			}
			this.UnlinkNodes(a.Key, b.Key);
			return true;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008C1C File Offset: 0x00006E1C
		public void RemoveAllLinksAt(TopologyNodeRef node, TopologyLinkKind? kindFilter = null)
		{
			if (node == null || !node.IsValid)
			{
				return;
			}
			TopologyNodeRef[] array = Enumerable.ToArray<TopologyNodeRef>(this.GetNeighborNodes(node, kindFilter));
			int i = 0;
			while (i < array.Length)
			{
				TopologyNodeRef neighbor = array[i];
				TopologyLinkKind kind = this.ResolveLinkKind(node.Key, neighbor.Key).GetValueOrDefault(TopologyLinkKind.Wireless);
				if (kindFilter == null)
				{
					goto IL_63;
				}
				TopologyLinkKind? topologyLinkKind = kindFilter;
				TopologyLinkKind topologyLinkKind2 = kind;
				if (topologyLinkKind.GetValueOrDefault() == topologyLinkKind2 & topologyLinkKind != null)
				{
					goto IL_63;
				}
				IL_6D:
				i++;
				continue;
				IL_63:
				this.RemoveLink(node, neighbor, kind);
				goto IL_6D;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008CA0 File Offset: 0x00006EA0
		public IEnumerable<TopologyNodeRef> GetNeighborNodes(TopologyNodeRef node, TopologyLinkKind? kindFilter = null)
		{
			CommunicationTopologyGraph.<GetNeighborNodes>d__11 <GetNeighborNodes>d__ = new CommunicationTopologyGraph.<GetNeighborNodes>d__11(-2);
			<GetNeighborNodes>d__.<>4__this = this;
			<GetNeighborNodes>d__.<>3__node = node;
			<GetNeighborNodes>d__.<>3__kindFilter = kindFilter;
			return <GetNeighborNodes>d__;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00008CC0 File Offset: 0x00006EC0
		public HashSet<TopologyNodeRef> GetConnectedComponent(TopologyNodeRef start, TopologyLinkKind? kindFilter = null)
		{
			HashSet<TopologyNodeRef> component = new HashSet<TopologyNodeRef>();
			if (start == null || !start.IsValid)
			{
				return component;
			}
			Queue<TopologyNodeRef> queue = new Queue<TopologyNodeRef>();
			queue.Enqueue(start);
			component.Add(start);
			while (queue.Count > 0)
			{
				TopologyNodeRef current = queue.Dequeue();
				foreach (TopologyNodeRef neighbor in this.GetNeighborNodes(current, kindFilter))
				{
					if (neighbor != null && neighbor.IsValid && component.Add(neighbor))
					{
						queue.Enqueue(neighbor);
					}
				}
			}
			return component;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00008D64 File Offset: 0x00006F64
		private TopologyLinkKind? ResolveLinkKind(string nodeKeyA, string nodeKeyB)
		{
			TopologyLink topologyLink = Enumerable.FirstOrDefault<TopologyLink>(this.links, (TopologyLink link) => link.IsValid && ((link.NodeKeyA == nodeKeyA && link.NodeKeyB == nodeKeyB) || (link.NodeKeyA == nodeKeyB && link.NodeKeyB == nodeKeyA)));
			if (topologyLink == null)
			{
				return default(TopologyLinkKind?);
			}
			return new TopologyLinkKind?(topologyLink.Kind);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00008DB4 File Offset: 0x00006FB4
		private void RebuildAdjacency()
		{
			this.adjacency.Clear();
			foreach (TopologyLink link in this.links)
			{
				if (link.IsValid)
				{
					this.LinkNodes(link.NodeKeyA, link.NodeKeyB);
				}
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00008E28 File Offset: 0x00007028
		private void LinkNodes(string keyA, string keyB)
		{
			if (string.IsNullOrEmpty(keyA) || string.IsNullOrEmpty(keyB))
			{
				return;
			}
			HashSet<string> aNeighbors;
			if (!this.adjacency.TryGetValue(keyA, ref aNeighbors))
			{
				aNeighbors = new HashSet<string>();
				this.adjacency[keyA] = aNeighbors;
			}
			HashSet<string> bNeighbors;
			if (!this.adjacency.TryGetValue(keyB, ref bNeighbors))
			{
				bNeighbors = new HashSet<string>();
				this.adjacency[keyB] = bNeighbors;
			}
			aNeighbors.Add(keyB);
			bNeighbors.Add(keyA);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00008E9C File Offset: 0x0000709C
		private void UnlinkNodes(string keyA, string keyB)
		{
			HashSet<string> aNeighbors;
			if (this.adjacency.TryGetValue(keyA, ref aNeighbors))
			{
				aNeighbors.Remove(keyB);
				if (aNeighbors.Count == 0)
				{
					this.adjacency.Remove(keyA);
				}
			}
			HashSet<string> bNeighbors;
			if (this.adjacency.TryGetValue(keyB, ref bNeighbors))
			{
				bNeighbors.Remove(keyA);
				if (bNeighbors.Count == 0)
				{
					this.adjacency.Remove(keyB);
				}
			}
		}

		// Token: 0x04000079 RID: 121
		private readonly HashSet<TopologyLink> links = new HashSet<TopologyLink>();

		// Token: 0x0400007A RID: 122
		private readonly Dictionary<string, HashSet<string>> adjacency = new Dictionary<string, HashSet<string>>();
	}
}
