using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Util;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200002F RID: 47
	public class CommunicationTopologyGraph
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000806D File Offset: 0x0000626D
		public int LinkCount
		{
			get
			{
				return this.links.Count;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000807A File Offset: 0x0000627A
		public void Clear()
		{
			this.links.Clear();
			this.adjacency.Clear();
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00008092 File Offset: 0x00006292
		public void LoadFromBytes(byte[] data)
		{
			this.Clear();
			if (data == null || data.Length == 0)
			{
				return;
			}
			this.LoadFromData(SerializerUtil.Deserialize<CommunicationTopologyData>(data));
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000080B0 File Offset: 0x000062B0
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

		// Token: 0x06000113 RID: 275 RVA: 0x00008148 File Offset: 0x00006348
		public byte[] ToSaveBytes()
		{
			return SerializerUtil.Serialize<CommunicationTopologyData>(new CommunicationTopologyData
			{
				Links = Enumerable.ToList<TopologyLink>(this.links)
			});
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00008168 File Offset: 0x00006368
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

		// Token: 0x06000115 RID: 277 RVA: 0x000081B0 File Offset: 0x000063B0
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

		// Token: 0x06000116 RID: 278 RVA: 0x00008200 File Offset: 0x00006400
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

		// Token: 0x06000117 RID: 279 RVA: 0x00008284 File Offset: 0x00006484
		public IEnumerable<TopologyNodeRef> GetNeighborNodes(TopologyNodeRef node, TopologyLinkKind? kindFilter = null)
		{
			CommunicationTopologyGraph.<GetNeighborNodes>d__11 <GetNeighborNodes>d__ = new CommunicationTopologyGraph.<GetNeighborNodes>d__11(-2);
			<GetNeighborNodes>d__.<>4__this = this;
			<GetNeighborNodes>d__.<>3__node = node;
			<GetNeighborNodes>d__.<>3__kindFilter = kindFilter;
			return <GetNeighborNodes>d__;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000082A4 File Offset: 0x000064A4
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

		// Token: 0x06000119 RID: 281 RVA: 0x00008348 File Offset: 0x00006548
		private TopologyLinkKind? ResolveLinkKind(string nodeKeyA, string nodeKeyB)
		{
			TopologyLink topologyLink = Enumerable.FirstOrDefault<TopologyLink>(this.links, (TopologyLink link) => link.IsValid && ((link.NodeKeyA == nodeKeyA && link.NodeKeyB == nodeKeyB) || (link.NodeKeyA == nodeKeyB && link.NodeKeyB == nodeKeyA)));
			if (topologyLink == null)
			{
				return default(TopologyLinkKind?);
			}
			return new TopologyLinkKind?(topologyLink.Kind);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00008398 File Offset: 0x00006598
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

		// Token: 0x0600011B RID: 283 RVA: 0x0000840C File Offset: 0x0000660C
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

		// Token: 0x0600011C RID: 284 RVA: 0x00008480 File Offset: 0x00006680
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

		// Token: 0x0400006B RID: 107
		private readonly HashSet<TopologyLink> links = new HashSet<TopologyLink>();

		// Token: 0x0400006C RID: 108
		private readonly Dictionary<string, HashSet<string>> adjacency = new Dictionary<string, HashSet<string>>();
	}
}
