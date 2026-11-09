using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020000D2 RID: 210
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MergingParser : IParser
	{
		// Token: 0x0600053D RID: 1341 RVA: 0x00011EE5 File Offset: 0x000100E5
		public MergingParser(IParser innerParser)
		{
			this.events = new MergingParser.ParsingEventCollection();
			this.merged = false;
			this.iterator = this.events.GetEnumerator();
			this.innerParser = innerParser;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00011F17 File Offset: 0x00010117
		[Nullable(2)]
		public ParsingEvent Current
		{
			[NullableContext(2)]
			get
			{
				LinkedListNode<ParsingEvent> linkedListNode = this.iterator.Current;
				if (linkedListNode == null)
				{
					return null;
				}
				return linkedListNode.Value;
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00011F2F File Offset: 0x0001012F
		public bool MoveNext()
		{
			if (!this.merged)
			{
				this.Merge();
				this.events.CleanMarked();
				this.iterator = this.events.GetEnumerator();
				this.merged = true;
			}
			return this.iterator.MoveNext();
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00011F70 File Offset: 0x00010170
		private void Merge()
		{
			while (this.innerParser.MoveNext())
			{
				this.events.Add(this.innerParser.Current);
			}
			foreach (LinkedListNode<ParsingEvent> linkedListNode in this.events)
			{
				if (MergingParser.IsMergeToken(linkedListNode))
				{
					this.events.MarkDeleted(linkedListNode);
					if (!this.HandleMerge(linkedListNode.Next))
					{
						Mark start = linkedListNode.Value.Start;
						Mark end = linkedListNode.Value.End;
						throw new SemanticErrorException(ref start, ref end, "Unrecognized merge key pattern");
					}
				}
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00012024 File Offset: 0x00010224
		private bool HandleMerge([Nullable(new byte[]
		{
			2,
			1
		})] LinkedListNode<ParsingEvent> node)
		{
			if (node == null)
			{
				return false;
			}
			AnchorAlias anchorAlias = node.Value as AnchorAlias;
			if (anchorAlias != null)
			{
				return this.HandleAnchorAlias(node, node, anchorAlias);
			}
			return node.Value is SequenceStart && this.HandleSequence(node);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00012068 File Offset: 0x00010268
		private bool HandleMergeSequence(LinkedListNode<ParsingEvent> sequenceStart, [Nullable(new byte[]
		{
			2,
			1
		})] LinkedListNode<ParsingEvent> node)
		{
			if (node == null)
			{
				return false;
			}
			AnchorAlias anchorAlias = node.Value as AnchorAlias;
			if (anchorAlias != null)
			{
				return this.HandleAnchorAlias(sequenceStart, node, anchorAlias);
			}
			return node.Value is SequenceStart && this.HandleSequence(node);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x000120AC File Offset: 0x000102AC
		private static bool IsMergeToken(LinkedListNode<ParsingEvent> node)
		{
			Scalar scalar = node.Value as Scalar;
			return scalar != null && scalar.Value == "<<";
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000120DC File Offset: 0x000102DC
		private bool HandleAnchorAlias(LinkedListNode<ParsingEvent> node, LinkedListNode<ParsingEvent> anchorNode, AnchorAlias anchorAlias)
		{
			IEnumerable<ParsingEvent> mappingEvents = this.GetMappingEvents(anchorAlias.Value);
			this.events.AddAfter(node, mappingEvents);
			this.events.MarkDeleted(anchorNode);
			return true;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00012110 File Offset: 0x00010310
		private bool HandleSequence(LinkedListNode<ParsingEvent> node)
		{
			this.events.MarkDeleted(node);
			LinkedListNode<ParsingEvent> next;
			for (LinkedListNode<ParsingEvent> linkedListNode = node; linkedListNode != null; linkedListNode = next)
			{
				if (linkedListNode.Value is SequenceEnd)
				{
					this.events.MarkDeleted(linkedListNode);
					return true;
				}
				next = linkedListNode.Next;
				this.HandleMergeSequence(node, next);
			}
			return true;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00012160 File Offset: 0x00010360
		private IEnumerable<ParsingEvent> GetMappingEvents(AnchorName anchor)
		{
			MergingParser.ParsingEventCloner parsingEventCloner = new MergingParser.ParsingEventCloner();
			int nesting = 0;
			return Enumerable.Select<ParsingEvent, ParsingEvent>(Enumerable.TakeWhile<ParsingEvent>(Enumerable.Select<LinkedListNode<ParsingEvent>, ParsingEvent>(Enumerable.Where<LinkedListNode<ParsingEvent>>(this.events.FromAnchor(anchor), (LinkedListNode<ParsingEvent> e) => !this.events.IsDeleted(e)), (LinkedListNode<ParsingEvent> e) => e.Value), (ParsingEvent e) => (nesting += e.NestingIncrease) >= 0), new Func<ParsingEvent, ParsingEvent>(parsingEventCloner.Clone));
		}

		// Token: 0x04000170 RID: 368
		private readonly MergingParser.ParsingEventCollection events;

		// Token: 0x04000171 RID: 369
		private readonly IParser innerParser;

		// Token: 0x04000172 RID: 370
		private IEnumerator<LinkedListNode<ParsingEvent>> iterator;

		// Token: 0x04000173 RID: 371
		private bool merged;

		// Token: 0x0200019B RID: 411
		[Nullable(0)]
		private sealed class ParsingEventCollection : IEnumerable<LinkedListNode<ParsingEvent>>, IEnumerable
		{
			// Token: 0x060008BD RID: 2237 RVA: 0x0001A9EB File Offset: 0x00018BEB
			public ParsingEventCollection()
			{
				this.events = new LinkedList<ParsingEvent>();
				this.deleted = new HashSet<LinkedListNode<ParsingEvent>>();
				this.references = new Dictionary<AnchorName, LinkedListNode<ParsingEvent>>();
			}

			// Token: 0x060008BE RID: 2238 RVA: 0x0001AA14 File Offset: 0x00018C14
			public void AddAfter(LinkedListNode<ParsingEvent> node, IEnumerable<ParsingEvent> items)
			{
				foreach (ParsingEvent parsingEvent in items)
				{
					node = this.events.AddAfter(node, parsingEvent);
				}
			}

			// Token: 0x060008BF RID: 2239 RVA: 0x0001AA64 File Offset: 0x00018C64
			public void Add(ParsingEvent item)
			{
				LinkedListNode<ParsingEvent> node = this.events.AddLast(item);
				this.AddReference(item, node);
			}

			// Token: 0x060008C0 RID: 2240 RVA: 0x0001AA86 File Offset: 0x00018C86
			public void MarkDeleted(LinkedListNode<ParsingEvent> node)
			{
				this.deleted.Add(node);
			}

			// Token: 0x060008C1 RID: 2241 RVA: 0x0001AA95 File Offset: 0x00018C95
			public bool IsDeleted(LinkedListNode<ParsingEvent> node)
			{
				return this.deleted.Contains(node);
			}

			// Token: 0x060008C2 RID: 2242 RVA: 0x0001AAA4 File Offset: 0x00018CA4
			public void CleanMarked()
			{
				foreach (LinkedListNode<ParsingEvent> linkedListNode in this.deleted)
				{
					this.events.Remove(linkedListNode);
				}
			}

			// Token: 0x060008C3 RID: 2243 RVA: 0x0001AAFC File Offset: 0x00018CFC
			public IEnumerable<LinkedListNode<ParsingEvent>> FromAnchor(AnchorName anchor)
			{
				LinkedListNode<ParsingEvent> next = this.references[anchor].Next;
				return MergingParser.ParsingEventCollection.Enumerate(next);
			}

			// Token: 0x060008C4 RID: 2244 RVA: 0x0001AB21 File Offset: 0x00018D21
			public IEnumerator<LinkedListNode<ParsingEvent>> GetEnumerator()
			{
				return MergingParser.ParsingEventCollection.Enumerate(this.events.First).GetEnumerator();
			}

			// Token: 0x060008C5 RID: 2245 RVA: 0x0001AB38 File Offset: 0x00018D38
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060008C6 RID: 2246 RVA: 0x0001AB40 File Offset: 0x00018D40
			private static IEnumerable<LinkedListNode<ParsingEvent>> Enumerate([Nullable(new byte[]
			{
				2,
				1
			})] LinkedListNode<ParsingEvent> node)
			{
				while (node != null)
				{
					yield return node;
					node = node.Next;
				}
				yield break;
			}

			// Token: 0x060008C7 RID: 2247 RVA: 0x0001AB50 File Offset: 0x00018D50
			private void AddReference(ParsingEvent item, LinkedListNode<ParsingEvent> node)
			{
				MappingStart mappingStart = item as MappingStart;
				if (mappingStart != null)
				{
					AnchorName anchor = mappingStart.Anchor;
					if (!anchor.IsEmpty)
					{
						this.references[anchor] = node;
					}
				}
			}

			// Token: 0x04000357 RID: 855
			private readonly LinkedList<ParsingEvent> events;

			// Token: 0x04000358 RID: 856
			private readonly HashSet<LinkedListNode<ParsingEvent>> deleted;

			// Token: 0x04000359 RID: 857
			private readonly Dictionary<AnchorName, LinkedListNode<ParsingEvent>> references;
		}

		// Token: 0x0200019C RID: 412
		[Nullable(0)]
		private sealed class ParsingEventCloner : IParsingEventVisitor
		{
			// Token: 0x060008C8 RID: 2248 RVA: 0x0001AB84 File Offset: 0x00018D84
			public ParsingEvent Clone(ParsingEvent e)
			{
				e.Accept(this);
				if (this.clonedEvent == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(32, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Could not clone event of type '");
					defaultInterpolatedStringHandler.AppendFormatted<EventType>(e.Type);
					defaultInterpolatedStringHandler.AppendLiteral("'");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				return this.clonedEvent;
			}

			// Token: 0x060008C9 RID: 2249 RVA: 0x0001ABE2 File Offset: 0x00018DE2
			void IParsingEventVisitor.Visit(AnchorAlias e)
			{
				this.clonedEvent = new AnchorAlias(e.Value, e.Start, e.End);
			}

			// Token: 0x060008CA RID: 2250 RVA: 0x0001AC01 File Offset: 0x00018E01
			void IParsingEventVisitor.Visit(StreamStart e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008CB RID: 2251 RVA: 0x0001AC08 File Offset: 0x00018E08
			void IParsingEventVisitor.Visit(StreamEnd e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008CC RID: 2252 RVA: 0x0001AC0F File Offset: 0x00018E0F
			void IParsingEventVisitor.Visit(DocumentStart e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008CD RID: 2253 RVA: 0x0001AC16 File Offset: 0x00018E16
			void IParsingEventVisitor.Visit(DocumentEnd e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008CE RID: 2254 RVA: 0x0001AC20 File Offset: 0x00018E20
			void IParsingEventVisitor.Visit(Scalar e)
			{
				this.clonedEvent = new Scalar(AnchorName.Empty, e.Tag, e.Value, e.Style, e.IsPlainImplicit, e.IsQuotedImplicit, e.Start, e.End, false);
			}

			// Token: 0x060008CF RID: 2255 RVA: 0x0001AC68 File Offset: 0x00018E68
			void IParsingEventVisitor.Visit(SequenceStart e)
			{
				this.clonedEvent = new SequenceStart(AnchorName.Empty, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
			}

			// Token: 0x060008D0 RID: 2256 RVA: 0x0001AC98 File Offset: 0x00018E98
			void IParsingEventVisitor.Visit(SequenceEnd e)
			{
				Mark start = e.Start;
				Mark end = e.End;
				this.clonedEvent = new SequenceEnd(ref start, ref end);
			}

			// Token: 0x060008D1 RID: 2257 RVA: 0x0001ACC2 File Offset: 0x00018EC2
			void IParsingEventVisitor.Visit(MappingStart e)
			{
				this.clonedEvent = new MappingStart(AnchorName.Empty, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
			}

			// Token: 0x060008D2 RID: 2258 RVA: 0x0001ACF4 File Offset: 0x00018EF4
			void IParsingEventVisitor.Visit(MappingEnd e)
			{
				Mark start = e.Start;
				Mark end = e.End;
				this.clonedEvent = new MappingEnd(ref start, ref end);
			}

			// Token: 0x060008D3 RID: 2259 RVA: 0x0001AD1E File Offset: 0x00018F1E
			void IParsingEventVisitor.Visit(Comment e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400035A RID: 858
			[Nullable(2)]
			private ParsingEvent clonedEvent;
		}
	}
}
