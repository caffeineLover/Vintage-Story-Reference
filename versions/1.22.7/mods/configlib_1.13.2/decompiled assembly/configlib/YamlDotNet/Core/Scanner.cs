using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020001C7 RID: 455
	[NullableContext(1)]
	[Nullable(0)]
	public class Scanner : IScanner
	{
		// Token: 0x060009F8 RID: 2552 RVA: 0x00021A04 File Offset: 0x0001FC04
		private bool IsDocumentStart()
		{
			return !this.analyzer.EndOfInput && this.cursor.LineOffset == 0 && this.analyzer.Check('-', 0) && this.analyzer.Check('-', 1) && this.analyzer.Check('-', 2) && this.analyzer.IsWhiteBreakOrZero(3);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00021A6C File Offset: 0x0001FC6C
		private bool IsDocumentEnd()
		{
			return !this.analyzer.EndOfInput && this.cursor.LineOffset == 0 && this.analyzer.Check('.', 0) && this.analyzer.Check('.', 1) && this.analyzer.Check('.', 2) && this.analyzer.IsWhiteBreakOrZero(3);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00021AD1 File Offset: 0x0001FCD1
		private bool IsDocumentIndicator()
		{
			return this.IsDocumentStart() || this.IsDocumentEnd();
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x00021AE3 File Offset: 0x0001FCE3
		// (set) Token: 0x060009FC RID: 2556 RVA: 0x00021AEB File Offset: 0x0001FCEB
		public bool SkipComments { get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x00021AF4 File Offset: 0x0001FCF4
		// (set) Token: 0x060009FE RID: 2558 RVA: 0x00021AFC File Offset: 0x0001FCFC
		[Nullable(2)]
		public Token Current { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x060009FF RID: 2559 RVA: 0x00021B08 File Offset: 0x0001FD08
		public Scanner(TextReader input, bool skipComments = true)
		{
			this.analyzer = new CharacterAnalyzer<LookAheadBuffer>(new LookAheadBuffer(input, 1024));
			this.cursor = new Cursor();
			this.SkipComments = skipComments;
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00021B70 File Offset: 0x0001FD70
		public Mark CurrentPosition
		{
			get
			{
				return this.cursor.Mark();
			}
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00021B7D File Offset: 0x0001FD7D
		public bool MoveNext()
		{
			if (this.Current != null)
			{
				this.ConsumeCurrent();
			}
			return this.MoveNextWithoutConsuming();
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00021B94 File Offset: 0x0001FD94
		public bool MoveNextWithoutConsuming()
		{
			if (!this.tokenAvailable && !this.streamEndProduced)
			{
				this.FetchMoreTokens();
			}
			if (this.tokens.Count > 0)
			{
				this.Current = this.tokens.Dequeue();
				this.tokenAvailable = false;
				return true;
			}
			this.Current = null;
			return false;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00021BE7 File Offset: 0x0001FDE7
		public void ConsumeCurrent()
		{
			this.tokensParsed++;
			this.tokenAvailable = false;
			this.previous = this.Current;
			this.Current = null;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00021C11 File Offset: 0x0001FE11
		private char ReadCurrentCharacter()
		{
			char result = this.analyzer.Peek(0);
			this.Skip();
			return result;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00021C25 File Offset: 0x0001FE25
		private char ReadLine()
		{
			if (this.analyzer.Check("\r\n\u0085", 0))
			{
				this.SkipLine();
				return '\n';
			}
			char result = this.analyzer.Peek(0);
			this.SkipLine();
			return result;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00021C58 File Offset: 0x0001FE58
		private void FetchMoreTokens()
		{
			for (;;)
			{
				bool flag = false;
				if (this.tokens.Count == 0)
				{
					flag = true;
				}
				else
				{
					foreach (SimpleKey simpleKey in this.simpleKeys)
					{
						if (simpleKey.IsPossible && simpleKey.TokenNumber == this.tokensParsed)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					break;
				}
				this.FetchNextToken();
			}
			this.tokenAvailable = true;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00021CE4 File Offset: 0x0001FEE4
		private static bool StartsWith(StringBuilder what, char start)
		{
			return what.Length > 0 && what.get_Chars(0) == start;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00021CFC File Offset: 0x0001FEFC
		private void StaleSimpleKeys()
		{
			foreach (SimpleKey simpleKey in this.simpleKeys)
			{
				if (simpleKey.IsPossible && (simpleKey.Line < this.cursor.Line || simpleKey.Index + 1024 < this.cursor.Index))
				{
					if (simpleKey.IsRequired)
					{
						Mark mark = this.cursor.Mark();
						this.tokens.Enqueue(new Error("While scanning a simple key, could not find expected ':'.", mark, mark));
					}
					simpleKey.MarkAsImpossible();
				}
			}
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00021DB0 File Offset: 0x0001FFB0
		private void FetchNextToken()
		{
			if (!this.streamStartProduced)
			{
				this.FetchStreamStart();
				return;
			}
			this.ScanToNextToken();
			this.StaleSimpleKeys();
			this.UnrollIndent(this.cursor.LineOffset);
			this.analyzer.Buffer.Cache(4);
			if (this.analyzer.Buffer.EndOfInput)
			{
				this.lastScalar = null;
				this.FetchStreamEnd();
			}
			if (this.cursor.LineOffset == 0 && this.analyzer.Check('%', 0))
			{
				this.lastScalar = null;
				this.FetchDirective();
				return;
			}
			if (this.IsDocumentStart())
			{
				this.lastScalar = null;
				this.FetchDocumentIndicator(true);
				return;
			}
			if (this.IsDocumentEnd())
			{
				this.lastScalar = null;
				this.FetchDocumentIndicator(false);
				return;
			}
			if (this.analyzer.Check('[', 0))
			{
				this.lastScalar = null;
				this.FetchFlowCollectionStart(true);
				return;
			}
			if (this.analyzer.Check('{', 0))
			{
				this.lastScalar = null;
				this.FetchFlowCollectionStart(false);
				return;
			}
			if (this.analyzer.Check(']', 0))
			{
				this.lastScalar = null;
				this.FetchFlowCollectionEnd(true);
				return;
			}
			if (this.analyzer.Check('}', 0))
			{
				this.lastScalar = null;
				this.FetchFlowCollectionEnd(false);
				return;
			}
			if (this.analyzer.Check(',', 0))
			{
				this.lastScalar = null;
				this.FetchFlowEntry();
				return;
			}
			if (this.analyzer.Check('-', 0))
			{
				if (this.analyzer.IsWhiteBreakOrZero(1))
				{
					this.FetchBlockEntry();
					return;
				}
				if (this.flowLevel > 0 && this.analyzer.Check(",[]{}", 1))
				{
					this.tokens.Enqueue(new Error("Invalid key indicator format.", this.cursor.Mark(), this.cursor.Mark()));
				}
			}
			if (this.analyzer.Check('?', 0) && (this.flowLevel > 0 || this.analyzer.IsWhiteBreakOrZero(1)) && this.analyzer.IsWhiteBreakOrZero(1))
			{
				this.FetchKey();
				return;
			}
			if (this.analyzer.Check(':', 0) && (this.flowLevel > 0 || this.analyzer.IsWhiteBreakOrZero(1)) && (!this.simpleKeyAllowed || this.flowLevel <= 0) && (!this.flowScalarFetched || !this.analyzer.Check(':', 1)) && (this.analyzer.IsWhiteBreakOrZero(1) || this.analyzer.Check(',', 1) || this.flowScalarFetched || this.flowCollectionFetched || this.startFlowCollectionFetched))
			{
				if (this.lastScalar != null)
				{
					this.lastScalar.IsKey = true;
					this.lastScalar = null;
				}
				this.FetchValue();
				return;
			}
			if (this.analyzer.Check('*', 0))
			{
				this.FetchAnchor(true);
				return;
			}
			if (this.analyzer.Check('&', 0))
			{
				this.FetchAnchor(false);
				return;
			}
			if (this.analyzer.Check('!', 0))
			{
				this.FetchTag();
				return;
			}
			if (this.analyzer.Check('|', 0) && this.flowLevel == 0)
			{
				this.FetchBlockScalar(true);
				return;
			}
			if (this.analyzer.Check('>', 0) && this.flowLevel == 0)
			{
				this.FetchBlockScalar(false);
				return;
			}
			if (this.analyzer.Check('\'', 0))
			{
				this.FetchQuotedScalar(true);
				return;
			}
			if (this.analyzer.Check('"', 0))
			{
				this.FetchQuotedScalar(false);
				return;
			}
			if ((!this.analyzer.IsWhiteBreakOrZero(0) && !this.analyzer.Check("-?:,[]{}#&*!|>'\"%@`", 0)) || (this.analyzer.Check('-', 0) && !this.analyzer.IsWhite(1)) || (this.analyzer.Check("?:", 0) && !this.analyzer.IsWhiteBreakOrZero(1)) || (this.simpleKeyAllowed && this.flowLevel > 0))
			{
				if (this.plainScalarFollowedByComment)
				{
					Mark mark = this.cursor.Mark();
					this.tokens.Enqueue(new Error("While scanning plain scalar, found a comment between adjacent scalars.", mark, mark));
				}
				if ((this.flowScalarFetched || (this.flowCollectionFetched && !this.startFlowCollectionFetched)) && this.analyzer.Check(':', 0))
				{
					this.Skip();
				}
				this.flowScalarFetched = false;
				this.flowCollectionFetched = false;
				this.startFlowCollectionFetched = false;
				this.plainScalarFollowedByComment = false;
				this.FetchPlainScalar();
				return;
			}
			if (this.simpleKeyAllowed && this.indent >= this.cursor.LineOffset && this.analyzer.IsTab(0))
			{
				throw new SyntaxErrorException("While scanning a mapping, found invalid tab as indentation.");
			}
			if (this.analyzer.IsWhiteBreakOrZero(0))
			{
				this.Skip();
				return;
			}
			Mark mark2 = this.cursor.Mark();
			this.Skip();
			Mark mark3 = this.cursor.Mark();
			throw new SyntaxErrorException(ref mark2, ref mark3, "While scanning for the next token, found character that cannot start any token.");
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00022286 File Offset: 0x00020486
		private bool CheckWhiteSpace()
		{
			return this.analyzer.Check(' ', 0) || ((this.flowLevel > 0 || !this.simpleKeyAllowed) && this.analyzer.Check('\t', 0));
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000222BB File Offset: 0x000204BB
		private void Skip()
		{
			this.cursor.Skip();
			this.analyzer.Buffer.Skip(1);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x000222DC File Offset: 0x000204DC
		private void SkipLine()
		{
			if (this.analyzer.IsCrLf(0))
			{
				this.cursor.SkipLineByOffset(2);
				this.analyzer.Buffer.Skip(2);
				return;
			}
			if (this.analyzer.IsBreak(0))
			{
				this.cursor.SkipLineByOffset(1);
				this.analyzer.Buffer.Skip(1);
				return;
			}
			if (!this.analyzer.IsZero(0))
			{
				throw new InvalidOperationException("Not at a break.");
			}
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0002235A File Offset: 0x0002055A
		private void ScanToNextToken()
		{
			for (;;)
			{
				if (!this.CheckWhiteSpace())
				{
					this.ProcessComment();
					if (!this.analyzer.IsBreak(0))
					{
						break;
					}
					this.SkipLine();
					if (this.flowLevel == 0)
					{
						this.simpleKeyAllowed = true;
					}
				}
				else
				{
					this.Skip();
				}
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00022398 File Offset: 0x00020598
		private void ProcessComment()
		{
			if (this.analyzer.Check('#', 0))
			{
				Mark start = this.cursor.Mark();
				this.Skip();
				while (this.analyzer.IsSpace(0))
				{
					this.Skip();
				}
				using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
				{
					StringBuilder builder = builderWrapper.Builder;
					while (!this.analyzer.IsBreakOrZero(0))
					{
						builder.Append(this.ReadCurrentCharacter());
					}
					if (!this.SkipComments)
					{
						bool isInline = this.previous != null && this.previous.End.Line == start.Line && this.previous.End.Column != 1 && !(this.previous is StreamStart);
						this.tokens.Enqueue(new Comment(builder.ToString(), isInline, start, this.cursor.Mark()));
					}
				}
			}
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x000224A8 File Offset: 0x000206A8
		private void FetchStreamStart()
		{
			this.simpleKeys.Push(new SimpleKey());
			this.simpleKeyAllowed = true;
			this.streamStartProduced = true;
			Mark mark = this.cursor.Mark();
			this.tokens.Enqueue(new StreamStart(ref mark, ref mark));
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x000224F4 File Offset: 0x000206F4
		private void UnrollIndent(int column)
		{
			if (this.flowLevel != 0)
			{
				return;
			}
			while (this.indent > column)
			{
				Mark mark = this.cursor.Mark();
				this.tokens.Enqueue(new BlockEnd(ref mark, ref mark));
				this.indent = this.indents.Pop();
			}
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00022544 File Offset: 0x00020744
		private void FetchStreamEnd()
		{
			this.cursor.ForceSkipLineAfterNonBreak();
			this.UnrollIndent(-1);
			this.RemoveSimpleKey();
			this.simpleKeyAllowed = false;
			this.streamEndProduced = true;
			Mark mark = this.cursor.Mark();
			this.tokens.Enqueue(new StreamEnd(ref mark, ref mark));
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00022598 File Offset: 0x00020798
		private void FetchDirective()
		{
			this.UnrollIndent(-1);
			this.RemoveSimpleKey();
			this.simpleKeyAllowed = false;
			Token token = this.ScanDirective();
			if (token != null)
			{
				this.tokens.Enqueue(token);
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x000225D0 File Offset: 0x000207D0
		[NullableContext(2)]
		private Token ScanDirective()
		{
			Mark mark = this.cursor.Mark();
			this.Skip();
			string text = this.ScanDirectiveName(mark);
			Token result;
			if (!(text == "YAML"))
			{
				if (!(text == "TAG"))
				{
					while (!this.analyzer.EndOfInput && !this.analyzer.Check('#', 0) && !this.analyzer.IsBreak(0))
					{
						this.Skip();
					}
					return null;
				}
				result = this.ScanTagDirectiveValue(mark);
			}
			else
			{
				if (!(this.previous is DocumentStart) && !(this.previous is StreamStart) && !(this.previous is DocumentEnd))
				{
					Mark mark2 = this.cursor.Mark();
					throw new SemanticErrorException(ref mark, ref mark2, "While scanning a version directive, did not find preceding <document end>.");
				}
				result = this.ScanVersionDirectiveValue(mark);
			}
			while (this.analyzer.IsWhite(0))
			{
				this.Skip();
			}
			this.ProcessComment();
			if (!this.analyzer.IsBreakOrZero(0))
			{
				Mark mark2 = this.cursor.Mark();
				throw new SyntaxErrorException(ref mark, ref mark2, "While scanning a directive, did not find expected comment or line break.");
			}
			if (this.analyzer.IsBreak(0))
			{
				this.SkipLine();
			}
			return result;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x000226F8 File Offset: 0x000208F8
		private void FetchDocumentIndicator(bool isStartToken)
		{
			this.UnrollIndent(-1);
			this.RemoveSimpleKey();
			this.simpleKeyAllowed = false;
			Mark start = this.cursor.Mark();
			this.Skip();
			this.Skip();
			this.Skip();
			if (isStartToken)
			{
				InsertionQueue<Token> insertionQueue = this.tokens;
				Mark mark = this.cursor.Mark();
				insertionQueue.Enqueue(new DocumentStart(ref start, ref mark));
				return;
			}
			Token token = null;
			while (!this.analyzer.EndOfInput && !this.analyzer.IsBreak(0) && !this.analyzer.Check('#', 0))
			{
				if (!this.analyzer.IsWhite(0))
				{
					token = new Error("While scanning a document end, found invalid content after '...' marker.", start, this.cursor.Mark());
					break;
				}
				this.Skip();
			}
			this.tokens.Enqueue(new DocumentEnd(ref start, ref start));
			if (token != null)
			{
				this.tokens.Enqueue(token);
			}
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x000227DC File Offset: 0x000209DC
		private void FetchFlowCollectionStart(bool isSequenceToken)
		{
			this.SaveSimpleKey();
			this.IncreaseFlowLevel();
			this.simpleKeyAllowed = true;
			Mark mark = this.cursor.Mark();
			this.Skip();
			Token token;
			if (isSequenceToken)
			{
				token = new FlowSequenceStart(ref mark, ref mark);
				this.flowSequenceStartLine = token.Start.Line;
			}
			else
			{
				token = new FlowMappingStart(ref mark, ref mark);
			}
			this.tokens.Enqueue(token);
			this.startFlowCollectionFetched = true;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0002284E File Offset: 0x00020A4E
		private void IncreaseFlowLevel()
		{
			this.simpleKeys.Push(new SimpleKey());
			this.flowLevel++;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00022870 File Offset: 0x00020A70
		private void FetchFlowCollectionEnd(bool isSequenceToken)
		{
			this.RemoveSimpleKey();
			this.DecreaseFlowLevel();
			this.simpleKeyAllowed = false;
			Mark mark = this.cursor.Mark();
			this.Skip();
			Token token = null;
			Token token2;
			if (isSequenceToken)
			{
				if (this.analyzer.Check('#', 0))
				{
					token = new Error("While scanning a flow sequence end, found invalid comment after ']'.", mark, mark);
				}
				token2 = new FlowSequenceEnd(ref mark, ref mark);
			}
			else
			{
				token2 = new FlowMappingEnd(ref mark, ref mark);
			}
			this.tokens.Enqueue(token2);
			if (token != null)
			{
				this.tokens.Enqueue(token);
			}
			this.flowCollectionFetched = true;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x000228FC File Offset: 0x00020AFC
		private void DecreaseFlowLevel()
		{
			if (this.flowLevel > 0)
			{
				this.flowLevel--;
				this.simpleKeys.Pop();
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00022924 File Offset: 0x00020B24
		private void FetchFlowEntry()
		{
			this.RemoveSimpleKey();
			this.simpleKeyAllowed = true;
			Mark start = this.cursor.Mark();
			this.Skip();
			Mark end = this.cursor.Mark();
			if (this.analyzer.Check('#', 0))
			{
				this.tokens.Enqueue(new Error("While scanning a flow entry, found invalid comment after comma.", start, end));
				return;
			}
			this.tokens.Enqueue(new FlowEntry(ref start, ref end));
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x00022998 File Offset: 0x00020B98
		private void FetchBlockEntry()
		{
			Mark mark;
			if (this.flowLevel == 0)
			{
				if (!this.simpleKeyAllowed)
				{
					if (this.previousAnchor != null)
					{
						mark = this.previousAnchor.End;
						if (mark.Line == this.cursor.Line)
						{
							mark = this.previousAnchor.Start;
							Mark end = this.previousAnchor.End;
							throw new SemanticErrorException(ref mark, ref end, "Anchor before sequence entry on same line is not allowed.");
						}
					}
					Mark mark2 = this.cursor.Mark();
					this.tokens.Enqueue(new Error("Block sequence entries are not allowed in this context.", mark2, mark2));
				}
				this.RollIndent(this.cursor.LineOffset, -1, true, this.cursor.Mark());
			}
			this.RemoveSimpleKey();
			this.simpleKeyAllowed = true;
			Mark mark3 = this.cursor.Mark();
			this.Skip();
			InsertionQueue<Token> insertionQueue = this.tokens;
			mark = this.cursor.Mark();
			insertionQueue.Enqueue(new BlockEntry(ref mark3, ref mark));
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00022A88 File Offset: 0x00020C88
		private void FetchKey()
		{
			if (this.flowLevel == 0)
			{
				if (!this.simpleKeyAllowed)
				{
					Mark mark = this.cursor.Mark();
					throw new SyntaxErrorException(ref mark, ref mark, "Mapping keys are not allowed in this context.");
				}
				this.RollIndent(this.cursor.LineOffset, -1, false, this.cursor.Mark());
			}
			this.RemoveSimpleKey();
			this.simpleKeyAllowed = (this.flowLevel == 0);
			Mark mark2 = this.cursor.Mark();
			this.Skip();
			InsertionQueue<Token> insertionQueue = this.tokens;
			Mark mark3 = this.cursor.Mark();
			insertionQueue.Enqueue(new Key(ref mark2, ref mark3));
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00022B28 File Offset: 0x00020D28
		private void FetchValue()
		{
			SimpleKey simpleKey = this.simpleKeys.Peek();
			Mark mark;
			if (simpleKey.IsPossible)
			{
				InsertionQueue<Token> insertionQueue = this.tokens;
				int num = simpleKey.TokenNumber - this.tokensParsed;
				mark = simpleKey.Mark;
				Mark mark2 = simpleKey.Mark;
				insertionQueue.Insert(num, new Key(ref mark, ref mark2));
				this.RollIndent(simpleKey.LineOffset, simpleKey.TokenNumber, false, simpleKey.Mark);
				simpleKey.MarkAsImpossible();
				this.simpleKeyAllowed = false;
			}
			else
			{
				bool flag = this.flowLevel == 0;
				if (flag)
				{
					if (!this.simpleKeyAllowed)
					{
						Mark mark3 = this.cursor.Mark();
						this.tokens.Enqueue(new Error("Mapping values are not allowed in this context.", mark3, mark3));
						return;
					}
					this.RollIndent(this.cursor.LineOffset, -1, false, this.cursor.Mark());
					if (this.cursor.LineOffset == 0 && simpleKey.LineOffset == 0)
					{
						InsertionQueue<Token> insertionQueue2 = this.tokens;
						int count = this.tokens.Count;
						mark = simpleKey.Mark;
						Mark mark2 = simpleKey.Mark;
						insertionQueue2.Insert(count, new Key(ref mark, ref mark2));
						flag = false;
					}
				}
				this.simpleKeyAllowed = flag;
			}
			Mark mark4 = this.cursor.Mark();
			this.Skip();
			InsertionQueue<Token> insertionQueue3 = this.tokens;
			mark = this.cursor.Mark();
			insertionQueue3.Enqueue(new Value(ref mark4, ref mark));
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00022C84 File Offset: 0x00020E84
		private void RollIndent(int column, int number, bool isSequence, Mark position)
		{
			if (this.flowLevel > 0)
			{
				return;
			}
			if (this.indent < column)
			{
				this.indents.Push(this.indent);
				this.indent = column;
				Token token;
				if (isSequence)
				{
					token = new BlockSequenceStart(ref position, ref position);
				}
				else
				{
					token = new BlockMappingStart(ref position, ref position);
				}
				if (number == -1)
				{
					this.tokens.Enqueue(token);
					return;
				}
				this.tokens.Insert(number - this.tokensParsed, token);
			}
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00022CFA File Offset: 0x00020EFA
		private void FetchAnchor(bool isAlias)
		{
			this.SaveSimpleKey();
			this.simpleKeyAllowed = false;
			this.tokens.Enqueue(this.ScanAnchor(isAlias));
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00022D1C File Offset: 0x00020F1C
		private Token ScanAnchor(bool isAlias)
		{
			Mark start = this.cursor.Mark();
			this.Skip();
			bool flag = false;
			if (isAlias)
			{
				SimpleKey simpleKey = this.simpleKeys.Peek();
				flag = (simpleKey.IsRequired && simpleKey.IsPossible);
			}
			Token result;
			using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
			{
				StringBuilder builder = builderWrapper.Builder;
				while (!this.analyzer.IsWhiteBreakOrZero(0) && !this.analyzer.Check("[]{},", 0) && (!flag || !this.analyzer.Check(':', 0) || !this.analyzer.IsWhiteBreakOrZero(1)))
				{
					builder.Append(this.ReadCurrentCharacter());
				}
				if (builder.Length == 0 || (!this.analyzer.IsWhiteBreakOrZero(0) && !this.analyzer.Check("?:,]}%@`", 0)))
				{
					Mark mark = this.cursor.Mark();
					throw new SyntaxErrorException(ref start, ref mark, "While scanning an anchor or alias, found value containing disallowed: []{},");
				}
				AnchorName value = new AnchorName(builder.ToString());
				if (isAlias)
				{
					result = new AnchorAlias(value, start, this.cursor.Mark());
				}
				else
				{
					result = (this.previousAnchor = new Anchor(value, start, this.cursor.Mark()));
				}
			}
			return result;
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00022E70 File Offset: 0x00021070
		private void FetchTag()
		{
			this.SaveSimpleKey();
			this.simpleKeyAllowed = false;
			this.tokens.Enqueue(this.ScanTag());
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00022E90 File Offset: 0x00021090
		private Token ScanTag()
		{
			Mark start = this.cursor.Mark();
			string text;
			string text2;
			if (this.analyzer.Check('<', 1))
			{
				text = string.Empty;
				this.Skip();
				this.Skip();
				text2 = this.ScanTagUri(null, start);
				if (!this.analyzer.Check('>', 0))
				{
					Mark mark = this.cursor.Mark();
					throw new SyntaxErrorException(ref start, ref mark, "While scanning a tag, did not find the expected '>'.");
				}
				this.Skip();
			}
			else
			{
				string text3 = this.ScanTagHandle(false, start);
				if (text3.Length > 1 && text3.get_Chars(0) == '!' && text3.get_Chars(text3.Length - 1) == '!')
				{
					text = text3;
					text2 = this.ScanTagUri(null, start);
				}
				else
				{
					text2 = this.ScanTagUri(text3, start);
					text = "!";
					if (text2.Length == 0)
					{
						text2 = text;
						text = string.Empty;
					}
				}
			}
			if (!this.analyzer.IsWhiteBreakOrZero(0) && !this.analyzer.Check(',', 0))
			{
				Mark mark = this.cursor.Mark();
				throw new SyntaxErrorException(ref start, ref mark, "While scanning a tag, did not find expected whitespace, comma or line break.");
			}
			return new Tag(text, text2, start, this.cursor.Mark());
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00022FB5 File Offset: 0x000211B5
		private void FetchBlockScalar(bool isLiteral)
		{
			this.SaveSimpleKey();
			this.simpleKeyAllowed = true;
			this.tokens.Enqueue(this.ScanBlockScalar(isLiteral));
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00022FD8 File Offset: 0x000211D8
		private Token ScanBlockScalar(bool isLiteral)
		{
			Token result;
			using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
			{
				StringBuilder builder = builderWrapper.Builder;
				using (StringBuilderPool.BuilderWrapper builderWrapper2 = StringBuilderPool.Rent())
				{
					StringBuilder builder2 = builderWrapper2.Builder;
					using (StringBuilderPool.BuilderWrapper builderWrapper3 = StringBuilderPool.Rent())
					{
						StringBuilder builder3 = builderWrapper3.Builder;
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						bool flag = false;
						bool? flag2 = default(bool?);
						Mark start = this.cursor.Mark();
						this.Skip();
						if (this.analyzer.Check("+-", 0))
						{
							num = (this.analyzer.Check('+', 0) ? 1 : -1);
							this.Skip();
							if (this.analyzer.IsDigit(0))
							{
								if (this.analyzer.Check('0', 0))
								{
									Mark mark = this.cursor.Mark();
									throw new SyntaxErrorException(ref start, ref mark, "While scanning a block scalar, found an indentation indicator equal to 0.");
								}
								num2 = this.analyzer.AsDigit(0);
								this.Skip();
							}
						}
						else if (this.analyzer.IsDigit(0))
						{
							if (this.analyzer.Check('0', 0))
							{
								Mark mark = this.cursor.Mark();
								throw new SyntaxErrorException(ref start, ref mark, "While scanning a block scalar, found an indentation indicator equal to 0.");
							}
							num2 = this.analyzer.AsDigit(0);
							this.Skip();
							if (this.analyzer.Check("+-", 0))
							{
								num = (this.analyzer.Check('+', 0) ? 1 : -1);
								this.Skip();
							}
						}
						if (this.analyzer.Check('#', 0))
						{
							Mark mark = this.cursor.Mark();
							throw new SyntaxErrorException(ref start, ref mark, "While scanning a block scalar, found a comment without whtespace after '>' indicator.");
						}
						while (this.analyzer.IsWhite(0))
						{
							this.Skip();
						}
						this.ProcessComment();
						if (!this.analyzer.IsBreakOrZero(0))
						{
							Mark mark = this.cursor.Mark();
							throw new SyntaxErrorException(ref start, ref mark, "While scanning a block scalar, did not find expected comment or line break.");
						}
						if (this.analyzer.IsBreak(0))
						{
							this.SkipLine();
							if (flag2 == null)
							{
								flag2 = new bool?(true);
							}
							else
							{
								bool? flag3 = flag2;
								bool flag4 = true;
								if (flag3.GetValueOrDefault() == flag4 & flag3 != null)
								{
									flag2 = new bool?(false);
								}
							}
						}
						Mark end = this.cursor.Mark();
						if (num2 != 0)
						{
							num3 = ((this.indent >= 0) ? (this.indent + num2) : num2);
						}
						num3 = this.ScanBlockScalarBreaks(num3, builder3, isLiteral, ref end, ref flag2);
						flag2 = new bool?(false);
						while (this.cursor.LineOffset == num3 && !this.analyzer.IsZero(0) && !this.IsDocumentEnd())
						{
							bool flag5 = this.analyzer.IsWhite(0);
							if (!isLiteral && Scanner.StartsWith(builder2, '\n') && !flag && !flag5)
							{
								if (builder3.Length == 0)
								{
									builder.Append(' ');
								}
								builder2.Length = 0;
							}
							else
							{
								builder.Append(builder2.ToString());
								builder2.Length = 0;
							}
							builder.Append(builder3.ToString());
							builder3.Length = 0;
							flag = this.analyzer.IsWhite(0);
							while (!this.analyzer.IsBreakOrZero(0))
							{
								builder.Append(this.ReadCurrentCharacter());
							}
							char c = this.ReadLine();
							if (c != '\0')
							{
								builder2.Append(c);
							}
							num3 = this.ScanBlockScalarBreaks(num3, builder3, isLiteral, ref end, ref flag2);
						}
						if (num != -1)
						{
							builder.Append(builder2);
						}
						if (num == 1)
						{
							builder.Append(builder3);
						}
						ScalarStyle style = isLiteral ? ScalarStyle.Literal : ScalarStyle.Folded;
						result = new Scalar(builder.ToString(), style, start, end);
					}
				}
			}
			return result;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x000233CC File Offset: 0x000215CC
		private int ScanBlockScalarBreaks(int currentIndent, StringBuilder breaks, bool isLiteral, ref Mark end, ref bool? isFirstLine)
		{
			int num = 0;
			int num2 = -1;
			end = this.cursor.Mark();
			for (;;)
			{
				if ((currentIndent != 0 && this.cursor.LineOffset >= currentIndent) || !this.analyzer.IsSpace(0))
				{
					if (this.cursor.LineOffset > num)
					{
						num = this.cursor.LineOffset;
					}
					if (!this.analyzer.IsBreak(0))
					{
						break;
					}
					bool? flag = isFirstLine;
					bool flag2 = true;
					if (flag.GetValueOrDefault() == flag2 & flag != null)
					{
						isFirstLine = new bool?(false);
						num2 = this.cursor.LineOffset;
					}
					breaks.Append(this.ReadLine());
					end = this.cursor.Mark();
				}
				else
				{
					this.Skip();
				}
			}
			if (isLiteral)
			{
				bool? flag = isFirstLine;
				bool flag2 = true;
				if (flag.GetValueOrDefault() == flag2 & flag != null)
				{
					int num3 = this.cursor.LineOffset;
					int num4 = 0;
					while (!this.analyzer.IsBreak(num4) && this.analyzer.IsSpace(num4))
					{
						num4++;
						num3++;
					}
					if (this.analyzer.IsBreak(num4) && num3 > this.cursor.LineOffset)
					{
						isFirstLine = new bool?(false);
						num2 = num3;
					}
				}
			}
			if (isLiteral && num2 > 1 && currentIndent < num2 - 1)
			{
				Mark mark = this.cursor.Mark();
				throw new SemanticErrorException(ref end, ref mark, "While scanning a literal block scalar, found extra spaces in first line.");
			}
			if (!isLiteral && num > this.cursor.LineOffset && num2 > -1)
			{
				Mark mark = this.cursor.Mark();
				throw new SemanticErrorException(ref end, ref mark, "While scanning a literal block scalar, found more spaces in lines above first content line.");
			}
			if (currentIndent == 0 && (this.cursor.LineOffset > 0 || this.indent > -1))
			{
				currentIndent = Math.Max(num, Math.Max(this.indent + 1, 1));
			}
			return currentIndent;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x000235BC File Offset: 0x000217BC
		private void FetchQuotedScalar(bool isSingleQuoted)
		{
			this.SaveSimpleKey();
			this.simpleKeyAllowed = false;
			this.flowScalarFetched = (this.flowLevel > 0);
			Scalar scalar = this.ScanFlowScalar(isSingleQuoted);
			this.tokens.Enqueue(scalar);
			this.lastScalar = scalar;
			if (!isSingleQuoted && this.analyzer.Check('#', 0))
			{
				Mark mark = this.cursor.Mark();
				this.tokens.Enqueue(new Error("While scanning a flow sequence end, found invalid comment after double-quoted scalar.", mark, mark));
			}
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00023638 File Offset: 0x00021838
		private Scalar ScanFlowScalar(bool isSingleQuoted)
		{
			Mark start = this.cursor.Mark();
			this.Skip();
			using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
			{
				StringBuilder builder = builderWrapper.Builder;
				using (StringBuilderPool.BuilderWrapper builderWrapper2 = StringBuilderPool.Rent())
				{
					StringBuilder builder2 = builderWrapper2.Builder;
					using (StringBuilderPool.BuilderWrapper builderWrapper3 = StringBuilderPool.Rent())
					{
						StringBuilder builder3 = builderWrapper3.Builder;
						using (StringBuilderPool.BuilderWrapper builderWrapper4 = StringBuilderPool.Rent())
						{
							StringBuilder builder4 = builderWrapper4.Builder;
							bool flag = false;
							Mark mark;
							while (!this.IsDocumentIndicator())
							{
								if (this.analyzer.IsZero(0))
								{
									mark = this.cursor.Mark();
									throw new SyntaxErrorException(ref start, ref mark, "While scanning a quoted scalar, found unexpected end of stream.");
								}
								if (flag && !isSingleQuoted && this.indent >= this.cursor.LineOffset)
								{
									mark = this.cursor.Mark();
									throw new SyntaxErrorException(ref start, ref mark, "While scanning a multi-line double-quoted scalar, found wrong indentation.");
								}
								flag = false;
								while (!this.analyzer.IsWhiteBreakOrZero(0))
								{
									if (isSingleQuoted && this.analyzer.Check('\'', 0) && this.analyzer.Check('\'', 1))
									{
										builder.Append('\'');
										this.Skip();
										this.Skip();
									}
									else
									{
										if (this.analyzer.Check(isSingleQuoted ? '\'' : '"', 0))
										{
											break;
										}
										if (!isSingleQuoted && this.analyzer.Check('\\', 0) && this.analyzer.IsBreak(1))
										{
											this.Skip();
											this.SkipLine();
											flag = true;
											break;
										}
										if (!isSingleQuoted && this.analyzer.Check('\\', 0))
										{
											int num = 0;
											char c = this.analyzer.Peek(1);
											if (c != 'U')
											{
												if (c != 'u')
												{
													if (c == 'x')
													{
														num = 2;
													}
													else
													{
														char c2;
														if (!Scanner.SimpleEscapeCodes.TryGetValue(c, ref c2))
														{
															mark = this.cursor.Mark();
															throw new SyntaxErrorException(ref start, ref mark, "While scanning a quoted scalar, found unknown escape character.");
														}
														builder.Append(c2);
													}
												}
												else
												{
													num = 4;
												}
											}
											else
											{
												num = 8;
											}
											this.Skip();
											this.Skip();
											if (num > 0)
											{
												int num2 = 0;
												for (int i = 0; i < num; i++)
												{
													if (!this.analyzer.IsHex(i))
													{
														mark = this.cursor.Mark();
														throw new SyntaxErrorException(ref start, ref mark, "While scanning a quoted scalar, did not find expected hexadecimal number.");
													}
													num2 = (num2 << 4) + this.analyzer.AsHex(i);
												}
												if (num2 >= 55296 && num2 <= 57343)
												{
													for (int j = 0; j < num; j++)
													{
														this.Skip();
													}
													if (this.analyzer.Peek(0) != '\\' || (this.analyzer.Peek(1) != 'u' && this.analyzer.Peek(1) != 'U'))
													{
														mark = this.cursor.Mark();
														throw new SyntaxErrorException(ref start, ref mark, "While scanning a quoted scalar, found invalid Unicode surrogates.");
													}
													this.Skip();
													if (this.analyzer.Peek(0) == 'u')
													{
														num = 4;
													}
													else
													{
														num = 8;
													}
													this.Skip();
													int num3 = 0;
													for (int k = 0; k < num; k++)
													{
														if (!this.analyzer.IsHex(0))
														{
															mark = this.cursor.Mark();
															throw new SyntaxErrorException(ref start, ref mark, "While scanning a quoted scalar, did not find expected hexadecimal number.");
														}
														num3 = (num3 << 4) + this.analyzer.AsHex(k);
													}
													for (int l = 0; l < num; l++)
													{
														this.Skip();
													}
													num2 = char.ConvertToUtf32((char)num2, (char)num3);
												}
												else
												{
													if (num2 > 1114111)
													{
														mark = this.cursor.Mark();
														throw new SyntaxErrorException(ref start, ref mark, "While scanning a quoted scalar, found invalid Unicode character escape code.");
													}
													for (int m = 0; m < num; m++)
													{
														this.Skip();
													}
												}
												builder.Append(char.ConvertFromUtf32(num2));
											}
										}
										else
										{
											builder.Append(this.ReadCurrentCharacter());
										}
									}
								}
								if (this.analyzer.Check(isSingleQuoted ? '\'' : '"', 0))
								{
									this.Skip();
									return new Scalar(builder.ToString(), isSingleQuoted ? ScalarStyle.SingleQuoted : ScalarStyle.DoubleQuoted, start, this.cursor.Mark());
								}
								while (this.analyzer.IsWhite(0) || this.analyzer.IsBreak(0))
								{
									if (this.analyzer.IsWhite(0))
									{
										if (!flag)
										{
											builder2.Append(this.ReadCurrentCharacter());
										}
										else
										{
											this.Skip();
										}
									}
									else if (!flag)
									{
										builder2.Length = 0;
										builder3.Append(this.ReadLine());
										flag = true;
									}
									else
									{
										builder4.Append(this.ReadLine());
									}
								}
								if (flag)
								{
									if (Scanner.StartsWith(builder3, '\n'))
									{
										if (builder4.Length == 0)
										{
											builder.Append(' ');
										}
										else
										{
											builder.Append(builder4.ToString());
										}
									}
									else
									{
										builder.Append(builder3.ToString());
										builder.Append(builder4.ToString());
									}
									builder3.Length = 0;
									builder4.Length = 0;
								}
								else
								{
									builder.Append(builder2.ToString());
									builder2.Length = 0;
								}
							}
							mark = this.cursor.Mark();
							throw new SyntaxErrorException(ref start, ref mark, "While scanning a quoted scalar, found unexpected document indicator.");
						}
					}
				}
			}
			Scalar result;
			return result;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00023BF4 File Offset: 0x00021DF4
		private void FetchPlainScalar()
		{
			this.SaveSimpleKey();
			this.simpleKeyAllowed = false;
			bool flag = false;
			Scalar scalar = this.ScanPlainScalar(ref flag);
			this.lastScalar = scalar;
			if (flag && this.analyzer.Check(':', 0) && this.flowLevel == 0 && this.indent < this.cursor.LineOffset)
			{
				this.tokens.Enqueue(new Error("While scanning a multiline plain scalar, found invalid mapping.", this.cursor.Mark(), this.cursor.Mark()));
			}
			this.tokens.Enqueue(scalar);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00023C88 File Offset: 0x00021E88
		private Scalar ScanPlainScalar(ref bool isMultiline)
		{
			Scalar result;
			using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
			{
				StringBuilder builder = builderWrapper.Builder;
				using (StringBuilderPool.BuilderWrapper builderWrapper2 = StringBuilderPool.Rent())
				{
					StringBuilder builder2 = builderWrapper2.Builder;
					using (StringBuilderPool.BuilderWrapper builderWrapper3 = StringBuilderPool.Rent())
					{
						StringBuilder builder3 = builderWrapper3.Builder;
						using (StringBuilderPool.BuilderWrapper builderWrapper4 = StringBuilderPool.Rent())
						{
							StringBuilder builder4 = builderWrapper4.Builder;
							bool flag = false;
							int num = this.indent + 1;
							Mark mark = this.cursor.Mark();
							Mark end = mark;
							SimpleKey simpleKey = this.simpleKeys.Peek();
							while (!this.IsDocumentIndicator())
							{
								if (this.analyzer.Check('#', 0))
								{
									if (this.indent < 0 && this.flowLevel == 0)
									{
										this.plainScalarFollowedByComment = true;
										break;
									}
									break;
								}
								else
								{
									bool flag2 = this.analyzer.Check('*', 0) && (!simpleKey.IsPossible || !simpleKey.IsRequired);
									while (!this.analyzer.IsWhiteBreakOrZero(0))
									{
										if ((this.analyzer.Check(':', 0) && !flag2 && (this.analyzer.IsWhiteBreakOrZero(1) || (this.flowLevel > 0 && this.analyzer.Check(',', 1)))) || (this.flowLevel > 0 && this.analyzer.Check(",[]{}", 0)))
										{
											if (this.flowLevel == 0 && !simpleKey.IsPossible)
											{
												this.tokens.Enqueue(new Error("While scanning a plain scalar value, found invalid mapping.", this.cursor.Mark(), this.cursor.Mark()));
												break;
											}
											break;
										}
										else
										{
											if (flag || builder2.Length > 0)
											{
												if (flag)
												{
													if (Scanner.StartsWith(builder3, '\n'))
													{
														if (builder4.Length == 0)
														{
															builder.Append(' ');
														}
														else
														{
															builder.Append(builder4);
														}
													}
													else
													{
														builder.Append(builder3);
														builder.Append(builder4);
													}
													builder3.Length = 0;
													builder4.Length = 0;
													flag = false;
												}
												else
												{
													builder.Append(builder2);
													builder2.Length = 0;
												}
											}
											if (this.flowLevel > 0 && this.cursor.LineOffset < num)
											{
												throw new Exception();
											}
											builder.Append(this.ReadCurrentCharacter());
											end = this.cursor.Mark();
										}
									}
									if (!this.analyzer.IsWhite(0) && !this.analyzer.IsBreak(0))
									{
										break;
									}
									while (this.analyzer.IsWhite(0) || this.analyzer.IsBreak(0))
									{
										if (this.analyzer.IsWhite(0))
										{
											if (flag && this.cursor.LineOffset < num && this.analyzer.IsTab(0))
											{
												Mark mark2 = this.cursor.Mark();
												throw new SyntaxErrorException(ref mark, ref mark2, "While scanning a plain scalar, found a tab character that violate indentation.");
											}
											if (!flag)
											{
												builder2.Append(this.ReadCurrentCharacter());
											}
											else
											{
												this.Skip();
											}
										}
										else
										{
											isMultiline = true;
											if (!flag)
											{
												builder2.Length = 0;
												builder3.Append(this.ReadLine());
												flag = true;
											}
											else
											{
												builder4.Append(this.ReadLine());
											}
										}
									}
									if (this.flowLevel == 0 && this.cursor.LineOffset < num)
									{
										break;
									}
								}
							}
							if (flag)
							{
								this.simpleKeyAllowed = true;
							}
							result = new Scalar(builder.ToString(), ScalarStyle.Plain, mark, end);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00024078 File Offset: 0x00022278
		private void RemoveSimpleKey()
		{
			SimpleKey simpleKey = this.simpleKeys.Peek();
			if (simpleKey.IsPossible && simpleKey.IsRequired)
			{
				Mark mark = simpleKey.Mark;
				Mark mark2 = simpleKey.Mark;
				throw new SyntaxErrorException(ref mark, ref mark2, "While scanning a simple key, could not find expected ':'.");
			}
			simpleKey.MarkAsImpossible();
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x000240C4 File Offset: 0x000222C4
		private string ScanDirectiveName(in Mark start)
		{
			string result;
			using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
			{
				StringBuilder builder = builderWrapper.Builder;
				while (this.analyzer.IsAlphaNumericDashOrUnderscore(0))
				{
					builder.Append(this.ReadCurrentCharacter());
				}
				if (builder.Length == 0)
				{
					Mark mark = this.cursor.Mark();
					throw new SyntaxErrorException(ref start, ref mark, "While scanning a directive, could not find expected directive name.");
				}
				if (this.analyzer.EndOfInput)
				{
					Mark mark = this.cursor.Mark();
					throw new SyntaxErrorException(ref start, ref mark, "While scanning a directive, found unexpected end of stream.");
				}
				if (!this.analyzer.IsWhiteBreakOrZero(0))
				{
					Mark mark = this.cursor.Mark();
					throw new SyntaxErrorException(ref start, ref mark, "While scanning a directive, found unexpected non-alphabetical character.");
				}
				result = builder.ToString();
			}
			return result;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00024194 File Offset: 0x00022394
		private void SkipWhitespaces()
		{
			while (this.analyzer.IsWhite(0))
			{
				this.Skip();
			}
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000241AC File Offset: 0x000223AC
		private Token ScanVersionDirectiveValue(in Mark start)
		{
			this.SkipWhitespaces();
			int major = this.ScanVersionDirectiveNumber(start);
			if (!this.analyzer.Check('.', 0))
			{
				Mark mark = this.cursor.Mark();
				throw new SyntaxErrorException(ref start, ref mark, "While scanning a %YAML directive, did not find expected digit or '.' character.");
			}
			this.Skip();
			int minor = this.ScanVersionDirectiveNumber(start);
			return new VersionDirective(new Version(major, minor), start, start);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00024218 File Offset: 0x00022418
		private Token ScanTagDirectiveValue(in Mark start)
		{
			this.SkipWhitespaces();
			string handle = this.ScanTagHandle(true, start);
			if (!this.analyzer.IsWhite(0))
			{
				Mark mark = this.cursor.Mark();
				throw new SyntaxErrorException(ref start, ref mark, "While scanning a %TAG directive, did not find expected whitespace.");
			}
			this.SkipWhitespaces();
			string prefix = this.ScanTagUri(null, start);
			if (!this.analyzer.IsWhiteBreakOrZero(0))
			{
				Mark mark = this.cursor.Mark();
				throw new SyntaxErrorException(ref start, ref mark, "While scanning a %TAG directive, did not find expected whitespace or line break.");
			}
			return new TagDirective(handle, prefix, start, start);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000242B0 File Offset: 0x000224B0
		private string ScanTagUri([Nullable(2)] string head, Mark start)
		{
			string result;
			using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
			{
				StringBuilder builder = builderWrapper.Builder;
				if (head != null && head.Length > 1)
				{
					StringBuilder stringBuilder = builder;
					ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(head);
					ref ReadOnlySpan<char> ptr = ref readOnlySpan;
					stringBuilder.Append(ptr.Slice(1, ptr.Length - 1));
				}
				while (this.analyzer.IsAlphaNumericDashOrUnderscore(0) || this.analyzer.Check(";/?:@&=+$.!~*'()[]%", 0) || (this.analyzer.Check(',', 0) && !this.analyzer.IsBreak(1)))
				{
					if (this.analyzer.Check('%', 0))
					{
						builder.Append(this.ScanUriEscapes(start));
					}
					else if (this.analyzer.Check('+', 0))
					{
						builder.Append(' ');
						this.Skip();
					}
					else
					{
						builder.Append(this.ReadCurrentCharacter());
					}
				}
				if (builder.Length == 0)
				{
					result = string.Empty;
				}
				else
				{
					string text = builder.ToString();
					if (text.EndsWith(","))
					{
						Mark mark = this.cursor.Mark();
						Mark mark2 = this.cursor.Mark();
						throw new SyntaxErrorException(ref mark, ref mark2, "Unexpected comma at end of tag");
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00024408 File Offset: 0x00022608
		private string ScanUriEscapes(in Mark start)
		{
			byte[] array = Scanner.EmptyBytes;
			int num = 0;
			int num2 = 0;
			Mark mark;
			while (this.analyzer.Check('%', 0) && this.analyzer.IsHex(1) && this.analyzer.IsHex(2))
			{
				int num3 = (this.analyzer.AsHex(1) << 4) + this.analyzer.AsHex(2);
				if (num2 == 0)
				{
					num2 = (((num3 & 128) == 0) ? 1 : (((num3 & 224) == 192) ? 2 : (((num3 & 240) == 224) ? 3 : (((num3 & 248) == 240) ? 4 : 0))));
					if (num2 == 0)
					{
						mark = this.cursor.Mark();
						throw new SyntaxErrorException(ref start, ref mark, "While scanning a tag, found an incorrect leading UTF-8 octet.");
					}
					array = new byte[num2];
				}
				else if ((num3 & 192) != 128)
				{
					mark = this.cursor.Mark();
					throw new SyntaxErrorException(ref start, ref mark, "While scanning a tag, found an incorrect trailing UTF-8 octet.");
				}
				array[num++] = (byte)num3;
				this.Skip();
				this.Skip();
				this.Skip();
				if (--num2 <= 0)
				{
					string @string = Encoding.UTF8.GetString(array, 0, num);
					if (@string.Length == 0 || @string.Length > 2)
					{
						mark = this.cursor.Mark();
						throw new SyntaxErrorException(ref start, ref mark, "While scanning a tag, found an incorrect UTF-8 sequence.");
					}
					return @string;
				}
			}
			mark = this.cursor.Mark();
			throw new SyntaxErrorException(ref start, ref mark, "While scanning a tag, did not find URI escaped octet.");
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00024580 File Offset: 0x00022780
		private string ScanTagHandle(bool isDirective, Mark start)
		{
			if (!this.analyzer.Check('!', 0))
			{
				Mark mark = this.cursor.Mark();
				throw new SyntaxErrorException(ref start, ref mark, "While scanning a tag, did not find expected '!'.");
			}
			string result;
			using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
			{
				StringBuilder builder = builderWrapper.Builder;
				builder.Append(this.ReadCurrentCharacter());
				while (this.analyzer.IsAlphaNumericDashOrUnderscore(0))
				{
					builder.Append(this.ReadCurrentCharacter());
				}
				if (this.analyzer.Check('!', 0))
				{
					builder.Append(this.ReadCurrentCharacter());
				}
				else if (isDirective && (builder.Length != 1 || builder.get_Chars(0) != '!'))
				{
					Mark mark = this.cursor.Mark();
					throw new SyntaxErrorException(ref start, ref mark, "While scanning a tag directive, did not find expected '!'.");
				}
				result = builder.ToString();
			}
			return result;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00024668 File Offset: 0x00022868
		private int ScanVersionDirectiveNumber(in Mark start)
		{
			int num = 0;
			int num2 = 0;
			while (this.analyzer.IsDigit(0))
			{
				if (++num2 > 9)
				{
					Mark mark = this.cursor.Mark();
					throw new SyntaxErrorException(ref start, ref mark, "While scanning a %YAML directive, found extremely long version number.");
				}
				num = num * 10 + this.analyzer.AsDigit(0);
				this.Skip();
			}
			if (num2 == 0)
			{
				Mark mark = this.cursor.Mark();
				throw new SyntaxErrorException(ref start, ref mark, "While scanning a %YAML directive, did not find expected version number.");
			}
			return num;
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x000246E4 File Offset: 0x000228E4
		private void SaveSimpleKey()
		{
			bool isRequired = this.flowLevel == 0 && this.indent == this.cursor.LineOffset;
			if (this.simpleKeyAllowed)
			{
				SimpleKey simpleKey = new SimpleKey(isRequired, this.tokensParsed + this.tokens.Count, this.cursor);
				this.RemoveSimpleKey();
				this.simpleKeys.Pop();
				this.simpleKeys.Push(simpleKey);
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00024758 File Offset: 0x00022958
		// Note: this type is marked as 'beforefieldinit'.
		static Scanner()
		{
			SortedDictionary<char, char> sortedDictionary = new SortedDictionary<char, char>();
			sortedDictionary.Add('0', '\0');
			sortedDictionary.Add('a', '\a');
			sortedDictionary.Add('b', '\b');
			sortedDictionary.Add('t', '\t');
			sortedDictionary.Add('\t', '\t');
			sortedDictionary.Add('n', '\n');
			sortedDictionary.Add('v', '\v');
			sortedDictionary.Add('f', '\f');
			sortedDictionary.Add('r', '\r');
			sortedDictionary.Add('e', '\u001b');
			sortedDictionary.Add(' ', ' ');
			sortedDictionary.Add('"', '"');
			sortedDictionary.Add('\\', '\\');
			sortedDictionary.Add('/', '/');
			sortedDictionary.Add('N', '\u0085');
			sortedDictionary.Add('_', '\u00a0');
			sortedDictionary.Add('L', '\u2028');
			sortedDictionary.Add('P', '\u2029');
			Scanner.SimpleEscapeCodes = sortedDictionary;
			Scanner.EmptyBytes = new byte[0];
		}

		// Token: 0x04000427 RID: 1063
		private const int MaxVersionNumberLength = 9;

		// Token: 0x04000428 RID: 1064
		private static readonly SortedDictionary<char, char> SimpleEscapeCodes;

		// Token: 0x04000429 RID: 1065
		private readonly Stack<int> indents = new Stack<int>();

		// Token: 0x0400042A RID: 1066
		private readonly InsertionQueue<Token> tokens = new InsertionQueue<Token>(128);

		// Token: 0x0400042B RID: 1067
		private readonly Stack<SimpleKey> simpleKeys = new Stack<SimpleKey>();

		// Token: 0x0400042C RID: 1068
		private readonly CharacterAnalyzer<LookAheadBuffer> analyzer;

		// Token: 0x0400042D RID: 1069
		private readonly Cursor cursor;

		// Token: 0x0400042E RID: 1070
		private bool streamStartProduced;

		// Token: 0x0400042F RID: 1071
		private bool streamEndProduced;

		// Token: 0x04000430 RID: 1072
		private bool plainScalarFollowedByComment;

		// Token: 0x04000431 RID: 1073
		private int flowSequenceStartLine;

		// Token: 0x04000432 RID: 1074
		private bool flowCollectionFetched;

		// Token: 0x04000433 RID: 1075
		private bool startFlowCollectionFetched;

		// Token: 0x04000434 RID: 1076
		private int indent = -1;

		// Token: 0x04000435 RID: 1077
		private bool flowScalarFetched;

		// Token: 0x04000436 RID: 1078
		private bool simpleKeyAllowed;

		// Token: 0x04000437 RID: 1079
		private int flowLevel;

		// Token: 0x04000438 RID: 1080
		private int tokensParsed;

		// Token: 0x04000439 RID: 1081
		private bool tokenAvailable;

		// Token: 0x0400043A RID: 1082
		[Nullable(2)]
		private Token previous;

		// Token: 0x0400043B RID: 1083
		[Nullable(2)]
		private Anchor previousAnchor;

		// Token: 0x0400043C RID: 1084
		[Nullable(2)]
		private Scalar lastScalar;

		// Token: 0x0400043F RID: 1087
		private static readonly byte[] EmptyBytes;
	}
}
