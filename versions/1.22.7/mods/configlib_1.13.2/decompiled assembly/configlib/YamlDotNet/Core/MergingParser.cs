using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020001BB RID: 443
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MergingParser : IParser
	{
		// Token: 0x0600099A RID: 2458 RVA: 0x0001FA4C File Offset: 0x0001DC4C
		public MergingParser(IParser innerParser)
		{
			this.events = new MergingParser.ParsingEventCollection();
			this.merged = false;
			this.iterator = this.events.GetEnumerator();
			this.innerParser = innerParser;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0001FA7E File Offset: 0x0001DC7E
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

		// Token: 0x0600099C RID: 2460 RVA: 0x0001FA96 File Offset: 0x0001DC96
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

		// Token: 0x0600099D RID: 2461 RVA: 0x0001FAD4 File Offset: 0x0001DCD4
		private void Merge()
		{
			while (this.innerParser.MoveNext())
			{
				this.events.Add(this.innerParser.Current);
			}
			foreach (LinkedListNode<ParsingEvent> linkedListNode in this.events)
			{
				if (this.IsMergeToken(linkedListNode))
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

		// Token: 0x0600099E RID: 2462 RVA: 0x0001FB8C File Offset: 0x0001DD8C
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

		// Token: 0x0600099F RID: 2463 RVA: 0x0001FBD0 File Offset: 0x0001DDD0
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

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001FC14 File Offset: 0x0001DE14
		private bool IsMergeToken(LinkedListNode<ParsingEvent> node)
		{
			Scalar scalar = node.Value as Scalar;
			return scalar != null && scalar.Value == "<<";
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001FC44 File Offset: 0x0001DE44
		private bool HandleAnchorAlias(LinkedListNode<ParsingEvent> node, LinkedListNode<ParsingEvent> anchorNode, AnchorAlias anchorAlias)
		{
			IEnumerable<ParsingEvent> mappingEvents = this.GetMappingEvents(anchorAlias.Value);
			this.events.AddAfter(node, mappingEvents);
			this.events.MarkDeleted(anchorNode);
			return true;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001FC78 File Offset: 0x0001DE78
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

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001FCC8 File Offset: 0x0001DEC8
		private IEnumerable<ParsingEvent> GetMappingEvents(AnchorName anchor)
		{
			MergingParser.ParsingEventCloner cloner = new MergingParser.ParsingEventCloner();
			int nesting = 0;
			return Enumerable.Select<ParsingEvent, ParsingEvent>(Enumerable.TakeWhile<ParsingEvent>(Enumerable.Select<LinkedListNode<ParsingEvent>, ParsingEvent>(Enumerable.Where<LinkedListNode<ParsingEvent>>(this.events.FromAnchor(anchor), (LinkedListNode<ParsingEvent> e) => !this.events.IsDeleted(e)), (LinkedListNode<ParsingEvent> e) => e.Value), (ParsingEvent e) => (nesting += e.NestingIncrease) >= 0), (ParsingEvent e) => cloner.Clone(e));
		}

		// Token: 0x040003E9 RID: 1001
		private readonly MergingParser.ParsingEventCollection events;

		// Token: 0x040003EA RID: 1002
		private readonly IParser innerParser;

		// Token: 0x040003EB RID: 1003
		private IEnumerator<LinkedListNode<ParsingEvent>> iterator;

		// Token: 0x040003EC RID: 1004
		private bool merged;

		// Token: 0x020001BC RID: 444
		[Nullable(0)]
		private sealed class ParsingEventCollection : IEnumerable<LinkedListNode<ParsingEvent>>, IEnumerable
		{
			// Token: 0x060009A4 RID: 2468 RVA: 0x0001FD57 File Offset: 0x0001DF57
			public ParsingEventCollection()
			{
				this.events = new LinkedList<ParsingEvent>();
				this.deleted = new HashSet<LinkedListNode<ParsingEvent>>();
				this.references = new Dictionary<AnchorName, LinkedListNode<ParsingEvent>>();
			}

			// Token: 0x060009A5 RID: 2469 RVA: 0x0001FD80 File Offset: 0x0001DF80
			public void AddAfter(LinkedListNode<ParsingEvent> node, IEnumerable<ParsingEvent> items)
			{
				foreach (ParsingEvent parsingEvent in items)
				{
					node = this.events.AddAfter(node, parsingEvent);
				}
			}

			// Token: 0x060009A6 RID: 2470 RVA: 0x0001FDD0 File Offset: 0x0001DFD0
			public void Add(ParsingEvent item)
			{
				LinkedListNode<ParsingEvent> node = this.events.AddLast(item);
				this.AddReference(item, node);
			}

			// Token: 0x060009A7 RID: 2471 RVA: 0x0001FDF2 File Offset: 0x0001DFF2
			public void MarkDeleted(LinkedListNode<ParsingEvent> node)
			{
				this.deleted.Add(node);
			}

			// Token: 0x060009A8 RID: 2472 RVA: 0x0001FE01 File Offset: 0x0001E001
			public bool IsDeleted(LinkedListNode<ParsingEvent> node)
			{
				return this.deleted.Contains(node);
			}

			// Token: 0x060009A9 RID: 2473 RVA: 0x0001FE10 File Offset: 0x0001E010
			public void CleanMarked()
			{
				foreach (LinkedListNode<ParsingEvent> linkedListNode in this.deleted)
				{
					this.events.Remove(linkedListNode);
				}
			}

			// Token: 0x060009AA RID: 2474 RVA: 0x0001FE68 File Offset: 0x0001E068
			public IEnumerable<LinkedListNode<ParsingEvent>> FromAnchor(AnchorName anchor)
			{
				LinkedListNode<ParsingEvent> next = this.references[anchor].Next;
				return this.Enumerate(next);
			}

			// Token: 0x060009AB RID: 2475 RVA: 0x0001FE8E File Offset: 0x0001E08E
			public IEnumerator<LinkedListNode<ParsingEvent>> GetEnumerator()
			{
				return this.Enumerate(this.events.First).GetEnumerator();
			}

			// Token: 0x060009AC RID: 2476 RVA: 0x0001FEA6 File Offset: 0x0001E0A6
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060009AD RID: 2477 RVA: 0x0001FEAE File Offset: 0x0001E0AE
			private IEnumerable<LinkedListNode<ParsingEvent>> Enumerate([Nullable(new byte[]
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

			// Token: 0x060009AE RID: 2478 RVA: 0x0001FEC0 File Offset: 0x0001E0C0
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

			// Token: 0x040003ED RID: 1005
			private readonly LinkedList<ParsingEvent> events;

			// Token: 0x040003EE RID: 1006
			private readonly HashSet<LinkedListNode<ParsingEvent>> deleted;

			// Token: 0x040003EF RID: 1007
			private readonly Dictionary<AnchorName, LinkedListNode<ParsingEvent>> references;
		}

		// Token: 0x020001BE RID: 446
		[Nullable(0)]
		private sealed class ParsingEventCloner : IParsingEventVisitor
		{
			// Token: 0x060009B7 RID: 2487 RVA: 0x0001FFC0 File Offset: 0x0001E1C0
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

			// Token: 0x060009B8 RID: 2488 RVA: 0x0002001E File Offset: 0x0001E21E
			void IParsingEventVisitor.Visit(AnchorAlias e)
			{
				this.clonedEvent = new AnchorAlias(e.Value, e.Start, e.End);
			}

			// Token: 0x060009B9 RID: 2489 RVA: 0x0000209F File Offset: 0x0000029F
			void IParsingEventVisitor.Visit(StreamStart e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060009BA RID: 2490 RVA: 0x0000209F File Offset: 0x0000029F
			void IParsingEventVisitor.Visit(StreamEnd e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060009BB RID: 2491 RVA: 0x0000209F File Offset: 0x0000029F
			void IParsingEventVisitor.Visit(DocumentStart e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060009BC RID: 2492 RVA: 0x0000209F File Offset: 0x0000029F
			void IParsingEventVisitor.Visit(DocumentEnd e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060009BD RID: 2493 RVA: 0x00020040 File Offset: 0x0001E240
			void IParsingEventVisitor.Visit(Scalar e)
			{
				this.clonedEvent = new Scalar(AnchorName.Empty, e.Tag, e.Value, e.Style, e.IsPlainImplicit, e.IsQuotedImplicit, e.Start, e.End, false);
			}

			// Token: 0x060009BE RID: 2494 RVA: 0x00020088 File Offset: 0x0001E288
			void IParsingEventVisitor.Visit(SequenceStart e)
			{
				this.clonedEvent = new SequenceStart(AnchorName.Empty, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
			}

			// Token: 0x060009BF RID: 2495 RVA: 0x000200B8 File Offset: 0x0001E2B8
			void IParsingEventVisitor.Visit(SequenceEnd e)
			{
				Mark start = e.Start;
				Mark end = e.End;
				this.clonedEvent = new SequenceEnd(ref start, ref end);
			}

			// Token: 0x060009C0 RID: 2496 RVA: 0x000200E2 File Offset: 0x0001E2E2
			void IParsingEventVisitor.Visit(MappingStart e)
			{
				this.clonedEvent = new MappingStart(AnchorName.Empty, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
			}

			// Token: 0x060009C1 RID: 2497 RVA: 0x00020114 File Offset: 0x0001E314
			void IParsingEventVisitor.Visit(MappingEnd e)
			{
				Mark start = e.Start;
				Mark end = e.End;
				this.clonedEvent = new MappingEnd(ref start, ref end);
			}

			// Token: 0x060009C2 RID: 2498 RVA: 0x0000209F File Offset: 0x0000029F
			void IParsingEventVisitor.Visit(Comment e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x040003F5 RID: 1013
			[Nullable(2)]
			private ParsingEvent clonedEvent;
		}
	}
}
