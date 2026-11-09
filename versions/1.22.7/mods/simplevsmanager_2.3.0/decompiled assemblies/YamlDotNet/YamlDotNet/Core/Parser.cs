using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020000D3 RID: 211
	[NullableContext(1)]
	[Nullable(0)]
	public class Parser : IParser
	{
		// Token: 0x06000547 RID: 1351 RVA: 0x000121EC File Offset: 0x000103EC
		[NullableContext(2)]
		private Token GetCurrentToken()
		{
			if (this.currentToken == null)
			{
				while (this.scanner.MoveNextWithoutConsuming())
				{
					this.currentToken = this.scanner.Current;
					YamlDotNet.Core.Tokens.Comment comment = this.currentToken as YamlDotNet.Core.Tokens.Comment;
					if (comment == null)
					{
						break;
					}
					this.pendingEvents.Enqueue(new YamlDotNet.Core.Events.Comment(comment.Value, comment.IsInline, comment.Start, comment.End));
					this.scanner.ConsumeCurrent();
				}
			}
			return this.currentToken;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00012269 File Offset: 0x00010469
		public Parser(TextReader input) : this(new Scanner(input, true))
		{
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00012278 File Offset: 0x00010478
		public Parser(IScanner scanner)
		{
			this.scanner = scanner;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x000122A8 File Offset: 0x000104A8
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x000122B0 File Offset: 0x000104B0
		[Nullable(2)]
		public ParsingEvent Current { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x0600054C RID: 1356 RVA: 0x000122BC File Offset: 0x000104BC
		public bool MoveNext()
		{
			if (this.state == ParserState.StreamEnd)
			{
				this.Current = null;
				return false;
			}
			if (this.pendingEvents.Count == 0)
			{
				this.pendingEvents.Enqueue(this.StateMachine());
			}
			this.Current = this.pendingEvents.Dequeue();
			return true;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001230C File Offset: 0x0001050C
		private ParsingEvent StateMachine()
		{
			switch (this.state)
			{
			case ParserState.StreamStart:
				return this.ParseStreamStart();
			case ParserState.ImplicitDocumentStart:
				return this.ParseDocumentStart(true);
			case ParserState.DocumentStart:
				return this.ParseDocumentStart(false);
			case ParserState.DocumentContent:
				return this.ParseDocumentContent();
			case ParserState.DocumentEnd:
				return this.ParseDocumentEnd();
			case ParserState.BlockNode:
				return this.ParseNode(true, false);
			case ParserState.BlockNodeOrIndentlessSequence:
				return this.ParseNode(true, true);
			case ParserState.FlowNode:
				return this.ParseNode(false, false);
			case ParserState.BlockSequenceFirstEntry:
				return this.ParseBlockSequenceEntry(true);
			case ParserState.BlockSequenceEntry:
				return this.ParseBlockSequenceEntry(false);
			case ParserState.IndentlessSequenceEntry:
				return this.ParseIndentlessSequenceEntry();
			case ParserState.BlockMappingFirstKey:
				return this.ParseBlockMappingKey(true);
			case ParserState.BlockMappingKey:
				return this.ParseBlockMappingKey(false);
			case ParserState.BlockMappingValue:
				return this.ParseBlockMappingValue();
			case ParserState.FlowSequenceFirstEntry:
				return this.ParseFlowSequenceEntry(true);
			case ParserState.FlowSequenceEntry:
				return this.ParseFlowSequenceEntry(false);
			case ParserState.FlowSequenceEntryMappingKey:
				return this.ParseFlowSequenceEntryMappingKey();
			case ParserState.FlowSequenceEntryMappingValue:
				return this.ParseFlowSequenceEntryMappingValue();
			case ParserState.FlowSequenceEntryMappingEnd:
				return this.ParseFlowSequenceEntryMappingEnd();
			case ParserState.FlowMappingFirstKey:
				return this.ParseFlowMappingKey(true);
			case ParserState.FlowMappingKey:
				return this.ParseFlowMappingKey(false);
			case ParserState.FlowMappingValue:
				return this.ParseFlowMappingValue(false);
			case ParserState.FlowMappingEmptyValue:
				return this.ParseFlowMappingValue(true);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00012443 File Offset: 0x00010643
		private void Skip()
		{
			if (this.currentToken != null)
			{
				this.currentToken = null;
				this.scanner.ConsumeCurrent();
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00012460 File Offset: 0x00010660
		private YamlDotNet.Core.Events.StreamStart ParseStreamStart()
		{
			Token token = this.GetCurrentToken();
			YamlDotNet.Core.Tokens.StreamStart streamStart = token as YamlDotNet.Core.Tokens.StreamStart;
			Mark mark;
			Mark mark2;
			if (streamStart == null)
			{
				mark = ((token != null) ? token.Start : Mark.Empty);
				mark2 = ((token != null) ? token.End : Mark.Empty);
				throw new SemanticErrorException(ref mark, ref mark2, "Did not find expected <stream-start>.");
			}
			this.Skip();
			this.state = ParserState.ImplicitDocumentStart;
			mark = streamStart.Start;
			mark2 = streamStart.End;
			return new YamlDotNet.Core.Events.StreamStart(ref mark, ref mark2);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000124D4 File Offset: 0x000106D4
		private ParsingEvent ParseDocumentStart(bool isImplicit)
		{
			if (this.currentToken is VersionDirective)
			{
				throw new SyntaxErrorException("While parsing a document start node, could not find document end marker before version directive.");
			}
			Token token = this.GetCurrentToken();
			if (!isImplicit)
			{
				while (token is YamlDotNet.Core.Tokens.DocumentEnd)
				{
					this.Skip();
					token = this.GetCurrentToken();
				}
			}
			if (token == null)
			{
				throw new SyntaxErrorException("Reached the end of the stream while parsing a document start.");
			}
			if (token is YamlDotNet.Core.Tokens.Scalar && (this.state == ParserState.ImplicitDocumentStart || this.state == ParserState.DocumentStart))
			{
				isImplicit = true;
			}
			if ((isImplicit && !(token is VersionDirective) && !(token is TagDirective) && !(token is YamlDotNet.Core.Tokens.DocumentStart) && !(token is YamlDotNet.Core.Tokens.StreamEnd) && !(token is YamlDotNet.Core.Tokens.DocumentEnd)) || token is BlockMappingStart)
			{
				TagDirectiveCollection tags = new TagDirectiveCollection();
				this.ProcessDirectives(tags);
				this.states.Push(ParserState.DocumentEnd);
				this.state = ParserState.BlockNode;
				return new YamlDotNet.Core.Events.DocumentStart(null, tags, true, token.Start, token.End);
			}
			if (!(token is YamlDotNet.Core.Tokens.StreamEnd) && !(token is YamlDotNet.Core.Tokens.DocumentEnd))
			{
				Mark start = token.Start;
				TagDirectiveCollection tags2 = new TagDirectiveCollection();
				VersionDirective versionDirective = this.ProcessDirectives(tags2);
				Token token2 = this.GetCurrentToken();
				if (token2 == null)
				{
					throw new SemanticErrorException("Reached the end of the stream while parsing a document start");
				}
				token = token2;
				if (!(token is YamlDotNet.Core.Tokens.DocumentStart))
				{
					Mark start2 = token.Start;
					Mark end = token.End;
					throw new SemanticErrorException(ref start2, ref end, "Did not find expected <document start>.");
				}
				this.states.Push(ParserState.DocumentEnd);
				this.state = ParserState.DocumentContent;
				Mark end2 = token.End;
				this.Skip();
				return new YamlDotNet.Core.Events.DocumentStart(versionDirective, tags2, false, start, end2);
			}
			else
			{
				if (token is YamlDotNet.Core.Tokens.DocumentEnd)
				{
					this.Skip();
				}
				this.state = ParserState.StreamEnd;
				Token token3 = this.GetCurrentToken();
				if (token3 == null)
				{
					throw new SemanticErrorException("Reached the end of the stream while parsing a document start");
				}
				token = token3;
				Mark start2 = token.Start;
				Mark end = token.End;
				YamlDotNet.Core.Events.StreamEnd result = new YamlDotNet.Core.Events.StreamEnd(ref start2, ref end);
				if (this.scanner.MoveNextWithoutConsuming())
				{
					throw new InvalidOperationException("The scanner should contain no more tokens.");
				}
				return result;
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000126A8 File Offset: 0x000108A8
		[return: Nullable(2)]
		private VersionDirective ProcessDirectives(TagDirectiveCollection tags)
		{
			bool flag = false;
			VersionDirective result = null;
			VersionDirective versionDirective;
			TagDirective tagDirective;
			for (;;)
			{
				versionDirective = (this.GetCurrentToken() as VersionDirective);
				if (versionDirective != null)
				{
					if (this.version != null)
					{
						break;
					}
					if (versionDirective.Version.Major != 1 || versionDirective.Version.Minor > 3)
					{
						goto IL_55;
					}
					result = (this.version = versionDirective);
					flag = true;
				}
				else
				{
					tagDirective = (this.GetCurrentToken() as TagDirective);
					if (tagDirective == null)
					{
						goto IL_D3;
					}
					if (tags.Contains(tagDirective.Handle))
					{
						goto Block_5;
					}
					tags.Add(tagDirective);
					flag = true;
				}
				this.Skip();
			}
			Mark start = versionDirective.Start;
			Mark end = versionDirective.End;
			throw new SemanticErrorException(ref start, ref end, "Found duplicate %YAML directive.");
			IL_55:
			start = versionDirective.Start;
			end = versionDirective.End;
			throw new SemanticErrorException(ref start, ref end, "Found incompatible YAML document.");
			Block_5:
			start = tagDirective.Start;
			end = tagDirective.End;
			throw new SemanticErrorException(ref start, ref end, "Found duplicate %TAG directive.");
			IL_D3:
			if (this.GetCurrentToken() is YamlDotNet.Core.Tokens.DocumentStart && (this.version == null || (this.version.Version.Major == 1 && this.version.Version.Minor > 1)))
			{
				if (this.GetCurrentToken() is YamlDotNet.Core.Tokens.DocumentStart && this.version == null)
				{
					this.version = new VersionDirective(new Version(1, 2));
				}
				flag = true;
			}
			Parser.AddTagDirectives(tags, Constants.DefaultTagDirectives);
			if (flag)
			{
				this.tagDirectives.Clear();
			}
			Parser.AddTagDirectives(this.tagDirectives, tags);
			return result;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00012820 File Offset: 0x00010A20
		private static void AddTagDirectives(TagDirectiveCollection directives, IEnumerable<TagDirective> source)
		{
			foreach (TagDirective tagDirective in source)
			{
				if (!directives.Contains(tagDirective))
				{
					directives.Add(tagDirective);
				}
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00012874 File Offset: 0x00010A74
		private ParsingEvent ParseDocumentContent()
		{
			if (this.GetCurrentToken() is VersionDirective || this.GetCurrentToken() is TagDirective || this.GetCurrentToken() is YamlDotNet.Core.Tokens.DocumentStart || this.GetCurrentToken() is YamlDotNet.Core.Tokens.DocumentEnd || this.GetCurrentToken() is YamlDotNet.Core.Tokens.StreamEnd)
			{
				this.state = this.states.Pop();
				Mark currentPosition = this.scanner.CurrentPosition;
				return Parser.ProcessEmptyScalar(currentPosition);
			}
			return this.ParseNode(true, false);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000128F0 File Offset: 0x00010AF0
		private static YamlDotNet.Core.Events.Scalar ProcessEmptyScalar(in Mark position)
		{
			return new YamlDotNet.Core.Events.Scalar(AnchorName.Empty, TagName.Empty, string.Empty, ScalarStyle.Plain, true, false, position, position, false);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00012924 File Offset: 0x00010B24
		private ParsingEvent ParseNode(bool isBlock, bool isIndentlessSequence)
		{
			Error error = this.GetCurrentToken() as Error;
			Mark start;
			Mark end;
			if (error != null)
			{
				start = error.Start;
				end = error.End;
				throw new SemanticErrorException(ref start, ref end, error.Value);
			}
			Token token = this.GetCurrentToken();
			if (token == null)
			{
				throw new SemanticErrorException("Reached the end of the stream while parsing a node");
			}
			Token token2 = token;
			YamlDotNet.Core.Tokens.AnchorAlias anchorAlias = token2 as YamlDotNet.Core.Tokens.AnchorAlias;
			if (anchorAlias != null)
			{
				this.state = this.states.Pop();
				ParsingEvent result = new YamlDotNet.Core.Events.AnchorAlias(anchorAlias.Value, anchorAlias.Start, anchorAlias.End);
				this.Skip();
				return result;
			}
			Mark start2 = token2.Start;
			AnchorName anchor = AnchorName.Empty;
			TagName empty = TagName.Empty;
			Anchor anchor2 = null;
			Tag tag = null;
			Tag tag2;
			for (;;)
			{
				if (!anchor.IsEmpty)
				{
					goto IL_CA;
				}
				Anchor anchor3 = token2 as Anchor;
				if (anchor3 == null)
				{
					goto IL_CA;
				}
				anchor2 = anchor3;
				anchor = anchor3.Value;
				this.Skip();
				IL_236:
				Token token3 = this.GetCurrentToken();
				if (token3 == null)
				{
					goto Block_16;
				}
				token2 = token3;
				continue;
				IL_CA:
				if (!empty.IsEmpty)
				{
					goto IL_173;
				}
				tag2 = (token2 as Tag);
				if (tag2 != null)
				{
					tag = tag2;
					if (string.IsNullOrEmpty(tag2.Handle))
					{
						empty = new TagName(tag2.Suffix);
					}
					else
					{
						if (!this.tagDirectives.Contains(tag2.Handle))
						{
							break;
						}
						empty = new TagName(this.tagDirectives[tag2.Handle].Prefix + tag2.Suffix);
					}
					this.Skip();
					goto IL_236;
				}
				goto IL_173;
			}
			start = tag2.Start;
			end = tag2.End;
			throw new SemanticErrorException(ref start, ref end, "While parsing a node, found undefined tag handle.");
			IL_173:
			Anchor anchor4 = token2 as Anchor;
			if (anchor4 != null)
			{
				start = anchor4.Start;
				end = anchor4.End;
				throw new SemanticErrorException(ref start, ref end, "While parsing a node, found more than one anchor.");
			}
			YamlDotNet.Core.Tokens.AnchorAlias anchorAlias2 = token2 as YamlDotNet.Core.Tokens.AnchorAlias;
			if (anchorAlias2 != null)
			{
				start = anchorAlias2.Start;
				end = anchorAlias2.End;
				throw new SemanticErrorException(ref start, ref end, "While parsing a node, did not find expected token.");
			}
			Error error2 = token2 as Error;
			if (error2 != null)
			{
				if (tag != null && anchor2 != null && !anchor.IsEmpty)
				{
					return new YamlDotNet.Core.Events.Scalar(anchor, default(TagName), string.Empty, ScalarStyle.Any, false, false, anchor2.Start, anchor2.End, false);
				}
				start = error2.Start;
				end = error2.End;
				throw new SemanticErrorException(ref start, ref end, error2.Value);
			}
			else
			{
				bool isEmpty = empty.IsEmpty;
				if (isIndentlessSequence && this.GetCurrentToken() is BlockEntry)
				{
					this.state = ParserState.IndentlessSequenceEntry;
					return new SequenceStart(anchor, empty, isEmpty, SequenceStyle.Block, start2, token2.End);
				}
				YamlDotNet.Core.Tokens.Scalar scalar = token2 as YamlDotNet.Core.Tokens.Scalar;
				if (scalar != null)
				{
					bool isPlainImplicit = false;
					bool isQuotedImplicit = false;
					if ((scalar.Style == ScalarStyle.Plain && empty.IsEmpty) || empty.IsNonSpecific)
					{
						isPlainImplicit = true;
					}
					else if (empty.IsEmpty)
					{
						isQuotedImplicit = true;
					}
					this.state = this.states.Pop();
					this.Skip();
					ParsingEvent result2 = new YamlDotNet.Core.Events.Scalar(anchor, empty, scalar.Value, scalar.Style, isPlainImplicit, isQuotedImplicit, start2, scalar.End, scalar.IsKey);
					if (!anchor.IsEmpty && this.scanner.MoveNextWithoutConsuming())
					{
						this.currentToken = this.scanner.Current;
						if (this.currentToken is Error)
						{
							error = (this.currentToken as Error);
							start = error.Start;
							end = error.End;
							throw new SemanticErrorException(ref start, ref end, error.Value);
						}
					}
					if (this.state == ParserState.FlowMappingKey && !(this.scanner.Current is FlowMappingEnd) && this.scanner.MoveNextWithoutConsuming())
					{
						this.currentToken = this.scanner.Current;
						if (this.currentToken != null && !(this.currentToken is FlowEntry) && !(this.currentToken is FlowMappingEnd))
						{
							start = this.currentToken.Start;
							end = this.currentToken.End;
							throw new SemanticErrorException(ref start, ref end, "While parsing a flow mapping, did not find expected ',' or '}'.");
						}
					}
					return result2;
				}
				FlowSequenceStart flowSequenceStart = token2 as FlowSequenceStart;
				if (flowSequenceStart != null)
				{
					this.state = ParserState.FlowSequenceFirstEntry;
					return new SequenceStart(anchor, empty, isEmpty, SequenceStyle.Flow, start2, flowSequenceStart.End);
				}
				FlowMappingStart flowMappingStart = token2 as FlowMappingStart;
				if (flowMappingStart != null)
				{
					this.state = ParserState.FlowMappingFirstKey;
					return new MappingStart(anchor, empty, isEmpty, MappingStyle.Flow, start2, flowMappingStart.End);
				}
				if (isBlock)
				{
					BlockSequenceStart blockSequenceStart = token2 as BlockSequenceStart;
					if (blockSequenceStart != null)
					{
						this.state = ParserState.BlockSequenceFirstEntry;
						return new SequenceStart(anchor, empty, isEmpty, SequenceStyle.Block, start2, blockSequenceStart.End);
					}
					BlockMappingStart blockMappingStart = token2 as BlockMappingStart;
					if (blockMappingStart != null)
					{
						this.state = ParserState.BlockMappingFirstKey;
						return new MappingStart(anchor, empty, isEmpty, MappingStyle.Block, start2, blockMappingStart.End);
					}
				}
				if (!anchor.IsEmpty || !empty.IsEmpty)
				{
					this.state = this.states.Pop();
					return new YamlDotNet.Core.Events.Scalar(anchor, empty, string.Empty, ScalarStyle.Plain, isEmpty, false, start2, token2.End, false);
				}
				start = token2.Start;
				end = token2.End;
				throw new SemanticErrorException(ref start, ref end, "While parsing a node, did not find expected node content.");
			}
			Block_16:
			throw new SemanticErrorException("Reached the end of the stream while parsing a node");
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00012E28 File Offset: 0x00011028
		private YamlDotNet.Core.Events.DocumentEnd ParseDocumentEnd()
		{
			Token token = this.GetCurrentToken();
			if (token == null)
			{
				throw new SemanticErrorException("Reached the end of the stream while parsing a document end");
			}
			Token token2 = token;
			bool isImplicit = true;
			Mark start = token2.Start;
			Mark end = start;
			if (token2 is YamlDotNet.Core.Tokens.DocumentEnd)
			{
				end = token2.End;
				this.Skip();
				isImplicit = false;
			}
			else if (!(this.currentToken is YamlDotNet.Core.Tokens.StreamEnd) && !(this.currentToken is YamlDotNet.Core.Tokens.DocumentStart) && !(this.currentToken is FlowSequenceEnd) && !(this.currentToken is VersionDirective) && (!(this.Current is YamlDotNet.Core.Events.Scalar) || !(this.currentToken is Error)))
			{
				throw new SemanticErrorException(ref start, ref end, "Did not find expected <document end>.");
			}
			if (this.version != null && this.version.Version.Major == 1 && this.version.Version.Minor > 1)
			{
				this.version = null;
			}
			this.state = ParserState.DocumentStart;
			return new YamlDotNet.Core.Events.DocumentEnd(isImplicit, start, end);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00012F10 File Offset: 0x00011110
		private ParsingEvent ParseBlockSequenceEntry(bool isFirst)
		{
			if (isFirst)
			{
				this.GetCurrentToken();
				this.Skip();
			}
			Token token = this.GetCurrentToken();
			BlockEntry blockEntry = token as BlockEntry;
			if (blockEntry != null)
			{
				Mark end = blockEntry.End;
				this.Skip();
				token = this.GetCurrentToken();
				if (!(token is BlockEntry) && !(token is BlockEnd))
				{
					this.states.Push(ParserState.BlockSequenceEntry);
					return this.ParseNode(true, false);
				}
				this.state = ParserState.BlockSequenceEntry;
				return Parser.ProcessEmptyScalar(end);
			}
			else
			{
				BlockEnd blockEnd = token as BlockEnd;
				Mark mark;
				Mark mark2;
				if (blockEnd != null)
				{
					this.state = this.states.Pop();
					mark = blockEnd.Start;
					mark2 = blockEnd.End;
					ParsingEvent result = new SequenceEnd(ref mark, ref mark2);
					this.Skip();
					return result;
				}
				mark = ((token != null) ? token.Start : Mark.Empty);
				mark2 = ((token != null) ? token.End : Mark.Empty);
				throw new SemanticErrorException(ref mark, ref mark2, "While parsing a block collection, did not find expected '-' indicator.");
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00012FFC File Offset: 0x000111FC
		private ParsingEvent ParseIndentlessSequenceEntry()
		{
			Token token = this.GetCurrentToken();
			BlockEntry blockEntry = token as BlockEntry;
			if (blockEntry == null)
			{
				this.state = this.states.Pop();
				Mark mark = (token != null) ? token.Start : Mark.Empty;
				Mark mark2 = (token != null) ? token.End : Mark.Empty;
				return new SequenceEnd(ref mark, ref mark2);
			}
			Mark end = blockEntry.End;
			this.Skip();
			token = this.GetCurrentToken();
			if (!(token is BlockEntry) && !(token is Key) && !(token is Value) && !(token is BlockEnd))
			{
				this.states.Push(ParserState.IndentlessSequenceEntry);
				return this.ParseNode(true, false);
			}
			this.state = ParserState.IndentlessSequenceEntry;
			return Parser.ProcessEmptyScalar(end);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000130B4 File Offset: 0x000112B4
		private ParsingEvent ParseBlockMappingKey(bool isFirst)
		{
			if (isFirst)
			{
				this.GetCurrentToken();
				this.Skip();
			}
			Token token = this.GetCurrentToken();
			Key key = token as Key;
			if (key != null)
			{
				Mark end = key.End;
				this.Skip();
				token = this.GetCurrentToken();
				if (!(token is Key) && !(token is Value) && !(token is BlockEnd))
				{
					this.states.Push(ParserState.BlockMappingValue);
					return this.ParseNode(true, true);
				}
				this.state = ParserState.BlockMappingValue;
				return Parser.ProcessEmptyScalar(end);
			}
			else
			{
				Value value = token as Value;
				Mark mark;
				if (value != null)
				{
					this.Skip();
					mark = value.End;
					return Parser.ProcessEmptyScalar(mark);
				}
				YamlDotNet.Core.Tokens.AnchorAlias anchorAlias = token as YamlDotNet.Core.Tokens.AnchorAlias;
				if (anchorAlias != null)
				{
					this.Skip();
					return new YamlDotNet.Core.Events.AnchorAlias(anchorAlias.Value, anchorAlias.Start, anchorAlias.End);
				}
				BlockEnd blockEnd = token as BlockEnd;
				Mark mark2;
				if (blockEnd != null)
				{
					this.state = this.states.Pop();
					mark = blockEnd.Start;
					mark2 = blockEnd.End;
					ParsingEvent result = new MappingEnd(ref mark, ref mark2);
					this.Skip();
					return result;
				}
				Error error = this.GetCurrentToken() as Error;
				if (error != null)
				{
					mark = error.Start;
					mark2 = error.End;
					throw new SyntaxErrorException(ref mark, ref mark2, error.Value);
				}
				mark = ((token != null) ? token.Start : Mark.Empty);
				mark2 = ((token != null) ? token.End : Mark.Empty);
				throw new SemanticErrorException(ref mark, ref mark2, "While parsing a block mapping, did not find expected key.");
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001322C File Offset: 0x0001142C
		private ParsingEvent ParseBlockMappingValue()
		{
			Token token = this.GetCurrentToken();
			Value value = token as Value;
			if (value != null)
			{
				Mark end = value.End;
				this.Skip();
				token = this.GetCurrentToken();
				if (!(token is Key) && !(token is Value) && !(token is BlockEnd))
				{
					this.states.Push(ParserState.BlockMappingKey);
					return this.ParseNode(true, true);
				}
				this.state = ParserState.BlockMappingKey;
				return Parser.ProcessEmptyScalar(end);
			}
			else
			{
				Error error = token as Error;
				Mark mark;
				if (error != null)
				{
					mark = error.Start;
					Mark end2 = error.End;
					throw new SemanticErrorException(ref mark, ref end2, error.Value);
				}
				this.state = ParserState.BlockMappingKey;
				mark = ((token != null) ? token.Start : Mark.Empty);
				return Parser.ProcessEmptyScalar(mark);
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000132E8 File Offset: 0x000114E8
		private ParsingEvent ParseFlowSequenceEntry(bool isFirst)
		{
			if (isFirst)
			{
				this.GetCurrentToken();
				this.Skip();
			}
			Token token = this.GetCurrentToken();
			Mark mark;
			Mark mark2;
			ParsingEvent result;
			if (!(token is FlowSequenceEnd))
			{
				if (!isFirst)
				{
					if (!(token is FlowEntry))
					{
						mark = ((token != null) ? token.Start : Mark.Empty);
						mark2 = ((token != null) ? token.End : Mark.Empty);
						throw new SemanticErrorException(ref mark, ref mark2, "While parsing a flow sequence, did not find expected ',' or ']'.");
					}
					this.Skip();
					token = this.GetCurrentToken();
				}
				if (token is Key)
				{
					this.state = ParserState.FlowSequenceEntryMappingKey;
					result = new MappingStart(AnchorName.Empty, TagName.Empty, true, MappingStyle.Flow);
					this.Skip();
					return result;
				}
				if (!(token is FlowSequenceEnd))
				{
					this.states.Push(ParserState.FlowSequenceEntry);
					return this.ParseNode(false, false);
				}
			}
			this.state = this.states.Pop();
			mark = ((token != null) ? token.Start : Mark.Empty);
			mark2 = ((token != null) ? token.End : Mark.Empty);
			result = new SequenceEnd(ref mark, ref mark2);
			this.Skip();
			return result;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000133F0 File Offset: 0x000115F0
		private ParsingEvent ParseFlowSequenceEntryMappingKey()
		{
			Token token = this.GetCurrentToken();
			if (!(token is Value) && !(token is FlowEntry) && !(token is FlowSequenceEnd))
			{
				this.states.Push(ParserState.FlowSequenceEntryMappingValue);
				return this.ParseNode(false, false);
			}
			Mark mark = (token != null) ? token.End : Mark.Empty;
			this.Skip();
			this.state = ParserState.FlowSequenceEntryMappingValue;
			return Parser.ProcessEmptyScalar(mark);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00013458 File Offset: 0x00011658
		private ParsingEvent ParseFlowSequenceEntryMappingValue()
		{
			Token token = this.GetCurrentToken();
			if (token is Value)
			{
				this.Skip();
				token = this.GetCurrentToken();
				if (!(token is FlowEntry) && !(token is FlowSequenceEnd))
				{
					this.states.Push(ParserState.FlowSequenceEntryMappingEnd);
					return this.ParseNode(false, false);
				}
			}
			this.state = ParserState.FlowSequenceEntryMappingEnd;
			Mark mark = (token != null) ? token.Start : Mark.Empty;
			return Parser.ProcessEmptyScalar(mark);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000134C8 File Offset: 0x000116C8
		private MappingEnd ParseFlowSequenceEntryMappingEnd()
		{
			this.state = ParserState.FlowSequenceEntry;
			Token token = this.GetCurrentToken();
			Mark mark = (token != null) ? token.Start : Mark.Empty;
			Mark mark2 = (token != null) ? token.End : Mark.Empty;
			return new MappingEnd(ref mark, ref mark2);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00013510 File Offset: 0x00011710
		private ParsingEvent ParseFlowMappingKey(bool isFirst)
		{
			if (isFirst)
			{
				this.GetCurrentToken();
				this.Skip();
			}
			Token token = this.GetCurrentToken();
			Mark mark;
			Mark mark2;
			if (!(token is FlowMappingEnd))
			{
				if (!isFirst)
				{
					if (token is FlowEntry)
					{
						this.Skip();
						token = this.GetCurrentToken();
					}
					else if (!(token is YamlDotNet.Core.Tokens.Scalar))
					{
						mark = ((token != null) ? token.Start : Mark.Empty);
						mark2 = ((token != null) ? token.End : Mark.Empty);
						throw new SemanticErrorException(ref mark, ref mark2, "While parsing a flow mapping,  did not find expected ',' or '}'.");
					}
				}
				if (token is Key)
				{
					this.Skip();
					token = this.GetCurrentToken();
					if (!(token is Value) && !(token is FlowEntry) && !(token is FlowMappingEnd))
					{
						this.states.Push(ParserState.FlowMappingValue);
						return this.ParseNode(false, false);
					}
					this.state = ParserState.FlowMappingValue;
					mark = ((token != null) ? token.Start : Mark.Empty);
					return Parser.ProcessEmptyScalar(mark);
				}
				else
				{
					if (token is YamlDotNet.Core.Tokens.Scalar)
					{
						this.states.Push(ParserState.FlowMappingValue);
						return this.ParseNode(false, false);
					}
					if (!(token is FlowMappingEnd))
					{
						this.states.Push(ParserState.FlowMappingEmptyValue);
						return this.ParseNode(false, false);
					}
				}
			}
			this.state = this.states.Pop();
			this.Skip();
			mark = ((token != null) ? token.Start : Mark.Empty);
			mark2 = ((token != null) ? token.End : Mark.Empty);
			return new MappingEnd(ref mark, ref mark2);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00013674 File Offset: 0x00011874
		private ParsingEvent ParseFlowMappingValue(bool isEmpty)
		{
			Token token = this.GetCurrentToken();
			if (!isEmpty && token is Value)
			{
				this.Skip();
				token = this.GetCurrentToken();
				if (!(token is FlowEntry) && !(token is FlowMappingEnd))
				{
					this.states.Push(ParserState.FlowMappingKey);
					return this.ParseNode(false, false);
				}
			}
			this.state = ParserState.FlowMappingKey;
			if (!isEmpty)
			{
				YamlDotNet.Core.Tokens.Scalar scalar = token as YamlDotNet.Core.Tokens.Scalar;
				if (scalar != null)
				{
					this.Skip();
					return new YamlDotNet.Core.Events.Scalar(AnchorName.Empty, TagName.Empty, scalar.Value, scalar.Style, false, false, token.Start, scalar.End, false);
				}
			}
			Mark mark = (token != null) ? token.Start : Mark.Empty;
			return Parser.ProcessEmptyScalar(mark);
		}

		// Token: 0x04000174 RID: 372
		private readonly Stack<ParserState> states = new Stack<ParserState>();

		// Token: 0x04000175 RID: 373
		private readonly TagDirectiveCollection tagDirectives = new TagDirectiveCollection();

		// Token: 0x04000176 RID: 374
		private ParserState state;

		// Token: 0x04000177 RID: 375
		private readonly IScanner scanner;

		// Token: 0x04000178 RID: 376
		[Nullable(2)]
		private Token currentToken;

		// Token: 0x04000179 RID: 377
		[Nullable(2)]
		private VersionDirective version;

		// Token: 0x0400017B RID: 379
		private readonly Parser.EventQueue pendingEvents = new Parser.EventQueue();

		// Token: 0x0200019F RID: 415
		[Nullable(0)]
		private class EventQueue
		{
			// Token: 0x060008DB RID: 2267 RVA: 0x0001AD94 File Offset: 0x00018F94
			public void Enqueue(ParsingEvent @event)
			{
				EventType type = @event.Type;
				if (type == EventType.StreamStart || type == EventType.DocumentStart)
				{
					this.highPriorityEvents.Enqueue(@event);
					return;
				}
				this.normalPriorityEvents.Enqueue(@event);
			}

			// Token: 0x060008DC RID: 2268 RVA: 0x0001ADC9 File Offset: 0x00018FC9
			public ParsingEvent Dequeue()
			{
				if (this.highPriorityEvents.Count <= 0)
				{
					return this.normalPriorityEvents.Dequeue();
				}
				return this.highPriorityEvents.Dequeue();
			}

			// Token: 0x1700012B RID: 299
			// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001ADF0 File Offset: 0x00018FF0
			public int Count
			{
				get
				{
					return this.highPriorityEvents.Count + this.normalPriorityEvents.Count;
				}
			}

			// Token: 0x0400035F RID: 863
			private readonly Queue<ParsingEvent> highPriorityEvents = new Queue<ParsingEvent>();

			// Token: 0x04000360 RID: 864
			private readonly Queue<ParsingEvent> normalPriorityEvents = new Queue<ParsingEvent>();
		}
	}
}
