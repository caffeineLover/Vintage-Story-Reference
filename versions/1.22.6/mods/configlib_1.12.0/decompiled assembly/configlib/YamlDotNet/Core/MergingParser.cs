using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020001BF RID: 447
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MergingParser : IParser
	{
		// Token: 0x060009D6 RID: 2518 RVA: 0x00020394 File Offset: 0x0001E594
		public MergingParser(IParser innerParser)
		{
			this.events = new MergingParser.ParsingEventCollection();
			this.merged = false;
			this.iterator = this.events.GetEnumerator();
			this.innerParser = innerParser;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x000203C6 File Offset: 0x0001E5C6
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

		// Token: 0x060009D8 RID: 2520 RVA: 0x000203DE File Offset: 0x0001E5DE
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

		// Token: 0x060009D9 RID: 2521 RVA: 0x0002041C File Offset: 0x0001E61C
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

		// Token: 0x060009DA RID: 2522 RVA: 0x000204D4 File Offset: 0x0001E6D4
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

		// Token: 0x060009DB RID: 2523 RVA: 0x00020518 File Offset: 0x0001E718
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

		// Token: 0x060009DC RID: 2524 RVA: 0x0002055C File Offset: 0x0001E75C
		private bool IsMergeToken(LinkedListNode<ParsingEvent> node)
		{
			Scalar scalar = node.Value as Scalar;
			return scalar != null && scalar.Value == "<<";
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0002058C File Offset: 0x0001E78C
		private bool HandleAnchorAlias(LinkedListNode<ParsingEvent> node, LinkedListNode<ParsingEvent> anchorNode, AnchorAlias anchorAlias)
		{
			IEnumerable<ParsingEvent> mappingEvents = this.GetMappingEvents(anchorAlias.Value);
			this.events.AddAfter(node, mappingEvents);
			this.events.MarkDeleted(anchorNode);
			return true;
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x000205C0 File Offset: 0x0001E7C0
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

		// Token: 0x060009DF RID: 2527 RVA: 0x00020610 File Offset: 0x0001E810
		private IEnumerable<ParsingEvent> GetMappingEvents(AnchorName anchor)
		{
			MergingParser.ParsingEventCloner cloner = new MergingParser.ParsingEventCloner();
			int nesting = 0;
			return Enumerable.Select<ParsingEvent, ParsingEvent>(Enumerable.TakeWhile<ParsingEvent>(Enumerable.Select<LinkedListNode<ParsingEvent>, ParsingEvent>(Enumerable.Where<LinkedListNode<ParsingEvent>>(this.events.FromAnchor(anchor), (LinkedListNode<ParsingEvent> e) => !this.events.IsDeleted(e)), (LinkedListNode<ParsingEvent> e) => e.Value), (ParsingEvent e) => (nesting += e.NestingIncrease) >= 0), (ParsingEvent e) => cloner.Clone(e));
		}

		// Token: 0x04000417 RID: 1047
		private readonly MergingParser.ParsingEventCollection events;

		// Token: 0x04000418 RID: 1048
		private readonly IParser innerParser;

		// Token: 0x04000419 RID: 1049
		private IEnumerator<LinkedListNode<ParsingEvent>> iterator;

		// Token: 0x0400041A RID: 1050
		private bool merged;

		// Token: 0x020001C0 RID: 448
		[Nullable(0)]
		private sealed class ParsingEventCollection : IEnumerable<LinkedListNode<ParsingEvent>>, IEnumerable
		{
			// Token: 0x060009E0 RID: 2528 RVA: 0x0002069F File Offset: 0x0001E89F
			public ParsingEventCollection()
			{
				this.events = new LinkedList<ParsingEvent>();
				this.deleted = new HashSet<LinkedListNode<ParsingEvent>>();
				this.references = new Dictionary<AnchorName, LinkedListNode<ParsingEvent>>();
			}

			// Token: 0x060009E1 RID: 2529 RVA: 0x000206C8 File Offset: 0x0001E8C8
			public void AddAfter(LinkedListNode<ParsingEvent> node, IEnumerable<ParsingEvent> items)
			{
				foreach (ParsingEvent parsingEvent in items)
				{
					node = this.events.AddAfter(node, parsingEvent);
				}
			}

			// Token: 0x060009E2 RID: 2530 RVA: 0x00020718 File Offset: 0x0001E918
			public void Add(ParsingEvent item)
			{
				LinkedListNode<ParsingEvent> node = this.events.AddLast(item);
				this.AddReference(item, node);
			}

			// Token: 0x060009E3 RID: 2531 RVA: 0x0002073A File Offset: 0x0001E93A
			public void MarkDeleted(LinkedListNode<ParsingEvent> node)
			{
				this.deleted.Add(node);
			}

			// Token: 0x060009E4 RID: 2532 RVA: 0x00020749 File Offset: 0x0001E949
			public bool IsDeleted(LinkedListNode<ParsingEvent> node)
			{
				return this.deleted.Contains(node);
			}

			// Token: 0x060009E5 RID: 2533 RVA: 0x00020758 File Offset: 0x0001E958
			public void CleanMarked()
			{
				foreach (LinkedListNode<ParsingEvent> linkedListNode in this.deleted)
				{
					this.events.Remove(linkedListNode);
				}
			}

			// Token: 0x060009E6 RID: 2534 RVA: 0x000207B0 File Offset: 0x0001E9B0
			public IEnumerable<LinkedListNode<ParsingEvent>> FromAnchor(AnchorName anchor)
			{
				LinkedListNode<ParsingEvent> next = this.references[anchor].Next;
				return this.Enumerate(next);
			}

			// Token: 0x060009E7 RID: 2535 RVA: 0x000207D6 File Offset: 0x0001E9D6
			public IEnumerator<LinkedListNode<ParsingEvent>> GetEnumerator()
			{
				return this.Enumerate(this.events.First).GetEnumerator();
			}

			// Token: 0x060009E8 RID: 2536 RVA: 0x000207EE File Offset: 0x0001E9EE
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060009E9 RID: 2537 RVA: 0x000207F6 File Offset: 0x0001E9F6
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

			// Token: 0x060009EA RID: 2538 RVA: 0x00020808 File Offset: 0x0001EA08
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

			// Token: 0x0400041B RID: 1051
			private readonly LinkedList<ParsingEvent> events;

			// Token: 0x0400041C RID: 1052
			private readonly HashSet<LinkedListNode<ParsingEvent>> deleted;

			// Token: 0x0400041D RID: 1053
			private readonly Dictionary<AnchorName, LinkedListNode<ParsingEvent>> references;
		}

		// Token: 0x020001C2 RID: 450
		[Nullable(0)]
		private sealed class ParsingEventCloner : IParsingEventVisitor
		{
			// Token: 0x060009F3 RID: 2547 RVA: 0x00020908 File Offset: 0x0001EB08
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

			// Token: 0x060009F4 RID: 2548 RVA: 0x00020966 File Offset: 0x0001EB66
			void IParsingEventVisitor.Visit(AnchorAlias e)
			{
				this.clonedEvent = new AnchorAlias(e.Value, e.Start, e.End);
			}

			// Token: 0x060009F5 RID: 2549 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IParsingEventVisitor.Visit(StreamStart e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060009F6 RID: 2550 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IParsingEventVisitor.Visit(StreamEnd e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060009F7 RID: 2551 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IParsingEventVisitor.Visit(DocumentStart e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060009F8 RID: 2552 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IParsingEventVisitor.Visit(DocumentEnd e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060009F9 RID: 2553 RVA: 0x00020988 File Offset: 0x0001EB88
			void IParsingEventVisitor.Visit(Scalar e)
			{
				this.clonedEvent = new Scalar(AnchorName.Empty, e.Tag, e.Value, e.Style, e.IsPlainImplicit, e.IsQuotedImplicit, e.Start, e.End, false);
			}

			// Token: 0x060009FA RID: 2554 RVA: 0x000209D0 File Offset: 0x0001EBD0
			void IParsingEventVisitor.Visit(SequenceStart e)
			{
				this.clonedEvent = new SequenceStart(AnchorName.Empty, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
			}

			// Token: 0x060009FB RID: 2555 RVA: 0x00020A00 File Offset: 0x0001EC00
			void IParsingEventVisitor.Visit(SequenceEnd e)
			{
				Mark start = e.Start;
				Mark end = e.End;
				this.clonedEvent = new SequenceEnd(ref start, ref end);
			}

			// Token: 0x060009FC RID: 2556 RVA: 0x00020A2A File Offset: 0x0001EC2A
			void IParsingEventVisitor.Visit(MappingStart e)
			{
				this.clonedEvent = new MappingStart(AnchorName.Empty, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
			}

			// Token: 0x060009FD RID: 2557 RVA: 0x00020A5C File Offset: 0x0001EC5C
			void IParsingEventVisitor.Visit(MappingEnd e)
			{
				Mark start = e.Start;
				Mark end = e.End;
				this.clonedEvent = new MappingEnd(ref start, ref end);
			}

			// Token: 0x060009FE RID: 2558 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IParsingEventVisitor.Visit(Comment e)
			{
				throw new NotSupportedException();
			}

			// Token: 0x04000423 RID: 1059
			[Nullable(2)]
			private ParsingEvent clonedEvent;
		}
	}
}
