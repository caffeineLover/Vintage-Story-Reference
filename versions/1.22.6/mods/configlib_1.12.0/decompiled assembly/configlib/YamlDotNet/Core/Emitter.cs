using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020001AD RID: 429
	[NullableContext(1)]
	[Nullable(0)]
	public class Emitter : IEmitter
	{
		// Token: 0x06000945 RID: 2373 RVA: 0x0001D670 File Offset: 0x0001B870
		public Emitter(TextWriter output) : this(output, EmitterSettings.Default)
		{
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0001D67E File Offset: 0x0001B87E
		public Emitter(TextWriter output, int bestIndent) : this(output, bestIndent, int.MaxValue)
		{
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0001D68D File Offset: 0x0001B88D
		public Emitter(TextWriter output, int bestIndent, int bestWidth) : this(output, bestIndent, bestWidth, false)
		{
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0001D69C File Offset: 0x0001B89C
		public Emitter(TextWriter output, int bestIndent, int bestWidth, bool isCanonical) : this(output, new EmitterSettings(bestIndent, bestWidth, isCanonical, 1024, false, false, null))
		{
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001D6C4 File Offset: 0x0001B8C4
		public Emitter(TextWriter output, EmitterSettings settings)
		{
			this.bestIndent = settings.BestIndent;
			this.bestWidth = settings.BestWidth;
			this.isCanonical = settings.IsCanonical;
			this.maxSimpleKeyLength = settings.MaxSimpleKeyLength;
			this.skipAnchorName = settings.SkipAnchorName;
			this.forceIndentLess = !settings.IndentSequences;
			this.newLine = settings.NewLine;
			this.output = output;
			this.outputUsesUnicodeEncoding = this.IsUnicode(output.Encoding);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001D794 File Offset: 0x0001B994
		public void Emit(ParsingEvent @event)
		{
			this.events.Enqueue(@event);
			while (!this.NeedMoreEvents())
			{
				ParsingEvent evt = this.events.Peek();
				try
				{
					this.AnalyzeEvent(evt);
					this.StateMachine(evt);
				}
				finally
				{
					this.events.Dequeue();
				}
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001D7F0 File Offset: 0x0001B9F0
		private bool NeedMoreEvents()
		{
			if (this.events.Count == 0)
			{
				return true;
			}
			EventType type = this.events.Peek().Type;
			int num;
			if (type != EventType.DocumentStart)
			{
				if (type != EventType.SequenceStart)
				{
					if (type != EventType.MappingStart)
					{
						return false;
					}
					num = 3;
				}
				else
				{
					num = 2;
				}
			}
			else
			{
				num = 1;
			}
			if (this.events.Count > num)
			{
				return false;
			}
			int num2 = 0;
			using (Queue<ParsingEvent>.Enumerator enumerator = this.events.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current.Type)
					{
					case EventType.DocumentStart:
					case EventType.SequenceStart:
					case EventType.MappingStart:
						num2++;
						break;
					case EventType.DocumentEnd:
					case EventType.SequenceEnd:
					case EventType.MappingEnd:
						num2--;
						break;
					}
					if (num2 == 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0001D8D0 File Offset: 0x0001BAD0
		private void AnalyzeEvent(ParsingEvent evt)
		{
			this.anchorData.Anchor = AnchorName.Empty;
			this.tagData.Handle = null;
			this.tagData.Suffix = null;
			YamlDotNet.Core.Events.AnchorAlias anchorAlias = evt as YamlDotNet.Core.Events.AnchorAlias;
			if (anchorAlias != null)
			{
				this.AnalyzeAnchor(anchorAlias.Value, true);
				return;
			}
			NodeEvent nodeEvent = evt as NodeEvent;
			if (nodeEvent != null)
			{
				YamlDotNet.Core.Events.Scalar scalar = evt as YamlDotNet.Core.Events.Scalar;
				if (scalar != null)
				{
					this.AnalyzeScalar(scalar);
				}
				this.AnalyzeAnchor(nodeEvent.Anchor, false);
				if (!nodeEvent.Tag.IsEmpty && (this.isCanonical || nodeEvent.IsCanonical))
				{
					this.AnalyzeTag(nodeEvent.Tag);
				}
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0001D971 File Offset: 0x0001BB71
		private void AnalyzeAnchor(AnchorName anchor, bool isAlias)
		{
			this.anchorData.Anchor = anchor;
			this.anchorData.IsAlias = isAlias;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0001D98C File Offset: 0x0001BB8C
		private void AnalyzeScalar(YamlDotNet.Core.Events.Scalar scalar)
		{
			string value = scalar.Value;
			this.scalarData.Value = value;
			if (value.Length != 0)
			{
				bool flag = false;
				bool flag2 = false;
				if (value.StartsWith("---", 4) || value.StartsWith("...", 4))
				{
					flag = true;
					flag2 = true;
				}
				CharacterAnalyzer<StringLookAheadBuffer> characterAnalyzer = new CharacterAnalyzer<StringLookAheadBuffer>(new StringLookAheadBuffer(value));
				bool flag3 = true;
				bool flag4 = characterAnalyzer.IsWhiteBreakOrZero(1);
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				bool flag8 = false;
				bool flag9 = false;
				bool flag10 = false;
				bool flag11 = false;
				bool flag12 = false;
				bool flag13 = false;
				bool flag14 = false;
				bool flag15 = false;
				bool flag16 = !this.ValueIsRepresentableInOutputEncoding(value);
				bool flag17 = false;
				bool flag18 = false;
				bool flag19 = true;
				while (!characterAnalyzer.EndOfInput)
				{
					if (flag19)
					{
						if (characterAnalyzer.Check("#,[]{}&*!|>\\\"%@`'", 0))
						{
							flag = true;
							flag2 = true;
							flag9 = characterAnalyzer.Check('\'', 0);
							flag17 |= characterAnalyzer.Check('\'', 0);
						}
						if (characterAnalyzer.Check("?:", 0))
						{
							flag = true;
							if (flag4)
							{
								flag2 = true;
							}
						}
						if (characterAnalyzer.Check('-', 0) && flag4)
						{
							flag = true;
							flag2 = true;
						}
					}
					else
					{
						if (characterAnalyzer.Check(",?[]{}", 0))
						{
							flag = true;
						}
						if (characterAnalyzer.Check(':', 0))
						{
							flag = true;
							if (flag4)
							{
								flag2 = true;
							}
						}
						if (characterAnalyzer.Check('#', 0) && flag3)
						{
							flag = true;
							flag2 = true;
						}
						flag17 |= characterAnalyzer.Check('\'', 0);
					}
					if (!flag16 && !characterAnalyzer.IsPrintable(0))
					{
						flag16 = true;
					}
					if (characterAnalyzer.IsBreak(0))
					{
						flag15 = true;
					}
					if (characterAnalyzer.IsSpace(0))
					{
						if (flag19)
						{
							flag5 = true;
						}
						if (characterAnalyzer.Buffer.Position >= characterAnalyzer.Buffer.Length - 1)
						{
							flag7 = true;
						}
						if (flag13)
						{
							flag10 = true;
							flag14 = true;
						}
						flag12 = true;
						flag13 = false;
					}
					else if (characterAnalyzer.IsBreak(0))
					{
						if (flag19)
						{
							flag6 = true;
						}
						if (characterAnalyzer.Buffer.Position >= characterAnalyzer.Buffer.Length - 1)
						{
							flag8 = true;
						}
						if (flag12)
						{
							flag11 = true;
						}
						if (flag14)
						{
							flag18 = true;
						}
						flag12 = false;
						flag13 = true;
					}
					else
					{
						flag12 = false;
						flag13 = false;
						flag14 = false;
					}
					flag3 = characterAnalyzer.IsWhiteBreakOrZero(0);
					characterAnalyzer.Skip(1);
					if (!characterAnalyzer.EndOfInput)
					{
						flag4 = characterAnalyzer.IsWhiteBreakOrZero(1);
					}
					flag19 = false;
				}
				this.scalarData.IsFlowPlainAllowed = true;
				this.scalarData.IsBlockPlainAllowed = true;
				this.scalarData.IsSingleQuotedAllowed = true;
				this.scalarData.IsBlockAllowed = true;
				if (flag5 || flag6 || flag7 || flag8 || flag9)
				{
					this.scalarData.IsFlowPlainAllowed = false;
					this.scalarData.IsBlockPlainAllowed = false;
				}
				if (flag7)
				{
					this.scalarData.IsBlockAllowed = false;
				}
				if (flag10)
				{
					this.scalarData.IsFlowPlainAllowed = false;
					this.scalarData.IsBlockPlainAllowed = false;
					this.scalarData.IsSingleQuotedAllowed = false;
				}
				if (flag11 || flag16)
				{
					this.scalarData.IsFlowPlainAllowed = false;
					this.scalarData.IsBlockPlainAllowed = false;
					this.scalarData.IsSingleQuotedAllowed = false;
				}
				if (flag18)
				{
					this.scalarData.IsBlockAllowed = false;
				}
				this.scalarData.IsMultiline = flag15;
				if (flag15)
				{
					this.scalarData.IsFlowPlainAllowed = false;
					this.scalarData.IsBlockPlainAllowed = false;
				}
				if (flag)
				{
					this.scalarData.IsFlowPlainAllowed = false;
				}
				if (flag2)
				{
					this.scalarData.IsBlockPlainAllowed = false;
				}
				this.scalarData.HasSingleQuotes = flag17;
				return;
			}
			if (scalar.Tag == "tag:yaml.org,2002:null")
			{
				this.scalarData.IsMultiline = false;
				this.scalarData.IsFlowPlainAllowed = false;
				this.scalarData.IsBlockPlainAllowed = true;
				this.scalarData.IsSingleQuotedAllowed = false;
				this.scalarData.IsBlockAllowed = false;
				return;
			}
			this.scalarData.IsMultiline = false;
			this.scalarData.IsFlowPlainAllowed = false;
			this.scalarData.IsBlockPlainAllowed = false;
			this.scalarData.IsSingleQuotedAllowed = true;
			this.scalarData.IsBlockAllowed = false;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0001DD4C File Offset: 0x0001BF4C
		private bool ValueIsRepresentableInOutputEncoding(string value)
		{
			if (this.outputUsesUnicodeEncoding)
			{
				return true;
			}
			bool result;
			try
			{
				byte[] bytes = this.output.Encoding.GetBytes(value);
				result = this.output.Encoding.GetString(bytes, 0, bytes.Length).Equals(value);
			}
			catch (EncoderFallbackException)
			{
				result = false;
			}
			catch (ArgumentOutOfRangeException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001DDBC File Offset: 0x0001BFBC
		private bool IsUnicode(Encoding encoding)
		{
			return encoding is UTF8Encoding || encoding is UnicodeEncoding || encoding is UTF7Encoding;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001DDDC File Offset: 0x0001BFDC
		private void AnalyzeTag(TagName tag)
		{
			this.tagData.Handle = tag.Value;
			foreach (TagDirective tagDirective in this.tagDirectives)
			{
				if (tag.Value.StartsWith(tagDirective.Prefix, 4))
				{
					this.tagData.Handle = tagDirective.Handle;
					this.tagData.Suffix = tag.Value.Substring(tagDirective.Prefix.Length);
					break;
				}
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001DE80 File Offset: 0x0001C080
		private void StateMachine(ParsingEvent evt)
		{
			YamlDotNet.Core.Events.Comment comment = evt as YamlDotNet.Core.Events.Comment;
			if (comment != null)
			{
				this.EmitComment(comment);
				return;
			}
			switch (this.state)
			{
			case EmitterState.StreamStart:
				this.EmitStreamStart(evt);
				return;
			case EmitterState.StreamEnd:
				throw new YamlException("Expected nothing after STREAM-END");
			case EmitterState.FirstDocumentStart:
				this.EmitDocumentStart(evt, true);
				return;
			case EmitterState.DocumentStart:
				this.EmitDocumentStart(evt, false);
				return;
			case EmitterState.DocumentContent:
				this.EmitDocumentContent(evt);
				return;
			case EmitterState.DocumentEnd:
				this.EmitDocumentEnd(evt);
				return;
			case EmitterState.FlowSequenceFirstItem:
				this.EmitFlowSequenceItem(evt, true);
				return;
			case EmitterState.FlowSequenceItem:
				this.EmitFlowSequenceItem(evt, false);
				return;
			case EmitterState.FlowMappingFirstKey:
				this.EmitFlowMappingKey(evt, true);
				return;
			case EmitterState.FlowMappingKey:
				this.EmitFlowMappingKey(evt, false);
				return;
			case EmitterState.FlowMappingSimpleValue:
				this.EmitFlowMappingValue(evt, true);
				return;
			case EmitterState.FlowMappingValue:
				this.EmitFlowMappingValue(evt, false);
				return;
			case EmitterState.BlockSequenceFirstItem:
				this.EmitBlockSequenceItem(evt, true);
				return;
			case EmitterState.BlockSequenceItem:
				this.EmitBlockSequenceItem(evt, false);
				return;
			case EmitterState.BlockMappingFirstKey:
				this.EmitBlockMappingKey(evt, true);
				return;
			case EmitterState.BlockMappingKey:
				this.EmitBlockMappingKey(evt, false);
				return;
			case EmitterState.BlockMappingSimpleValue:
				this.EmitBlockMappingValue(evt, true);
				return;
			case EmitterState.BlockMappingValue:
				this.EmitBlockMappingValue(evt, false);
				return;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0001DFA0 File Offset: 0x0001C1A0
		private void EmitComment(YamlDotNet.Core.Events.Comment comment)
		{
			if (this.flowLevel > 0 || this.state == EmitterState.FlowMappingFirstKey || this.state == EmitterState.FlowSequenceFirstItem)
			{
				return;
			}
			string[] array = comment.Value.Split(Emitter.newLineSeparators, 0);
			if (comment.IsInline)
			{
				this.Write(" # ");
				this.Write(string.Join(" ", array));
			}
			else
			{
				bool flag = this.state == EmitterState.BlockMappingFirstKey;
				if (flag)
				{
					this.IncreaseIndent(false, false);
				}
				foreach (string value in array)
				{
					this.WriteIndent();
					this.Write("# ");
					this.Write(value);
					this.WriteBreak('\n');
				}
				if (flag)
				{
					this.indent = this.indents.Pop();
				}
			}
			this.isIndentation = true;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0001E068 File Offset: 0x0001C268
		private void EmitStreamStart(ParsingEvent evt)
		{
			if (!(evt is YamlDotNet.Core.Events.StreamStart))
			{
				throw new ArgumentException("Expected STREAM-START.", "evt");
			}
			this.indent = -1;
			this.column = 0;
			this.isWhitespace = true;
			this.isIndentation = true;
			this.state = EmitterState.FirstDocumentStart;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0001E0A8 File Offset: 0x0001C2A8
		private void EmitDocumentStart(ParsingEvent evt, bool isFirst)
		{
			YamlDotNet.Core.Events.DocumentStart documentStart = evt as YamlDotNet.Core.Events.DocumentStart;
			if (documentStart != null)
			{
				bool flag = documentStart.IsImplicit && isFirst && !this.isCanonical;
				TagDirectiveCollection tagDirectiveCollection = this.NonDefaultTagsAmong(documentStart.Tags);
				if (!isFirst && !this.isDocumentEndWritten && (documentStart.Version != null || tagDirectiveCollection.Count > 0))
				{
					this.isDocumentEndWritten = false;
					this.WriteIndicator("...", true, false, false);
					this.WriteIndent();
				}
				if (documentStart.Version != null)
				{
					this.AnalyzeVersionDirective(documentStart.Version);
					Version version = documentStart.Version.Version;
					flag = false;
					this.WriteIndicator("%YAML", true, false, false);
					this.WriteIndicator(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor), true, false, false);
					this.WriteIndent();
				}
				foreach (TagDirective value in tagDirectiveCollection)
				{
					Emitter.AppendTagDirectiveTo(value, false, this.tagDirectives);
				}
				TagDirective[] defaultTagDirectives = Constants.DefaultTagDirectives;
				for (int i = 0; i < defaultTagDirectives.Length; i++)
				{
					Emitter.AppendTagDirectiveTo(defaultTagDirectives[i], true, this.tagDirectives);
				}
				if (tagDirectiveCollection.Count > 0)
				{
					flag = false;
					defaultTagDirectives = Constants.DefaultTagDirectives;
					for (int i = 0; i < defaultTagDirectives.Length; i++)
					{
						Emitter.AppendTagDirectiveTo(defaultTagDirectives[i], true, tagDirectiveCollection);
					}
					foreach (TagDirective tagDirective in tagDirectiveCollection)
					{
						this.WriteIndicator("%TAG", true, false, false);
						this.WriteTagHandle(tagDirective.Handle);
						this.WriteTagContent(tagDirective.Prefix, true);
						this.WriteIndent();
					}
				}
				if (this.CheckEmptyDocument())
				{
					flag = false;
				}
				if (!flag)
				{
					this.WriteIndent();
					this.WriteIndicator("---", true, false, false);
					if (this.isCanonical)
					{
						this.WriteIndent();
					}
				}
				this.state = EmitterState.DocumentContent;
				return;
			}
			if (evt is YamlDotNet.Core.Events.StreamEnd)
			{
				this.state = EmitterState.StreamEnd;
				return;
			}
			throw new YamlException("Expected DOCUMENT-START or STREAM-END");
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0001E2DC File Offset: 0x0001C4DC
		private TagDirectiveCollection NonDefaultTagsAmong([Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<TagDirective> tagCollection)
		{
			TagDirectiveCollection tagDirectiveCollection = new TagDirectiveCollection();
			if (tagCollection == null)
			{
				return tagDirectiveCollection;
			}
			foreach (TagDirective value in tagCollection)
			{
				Emitter.AppendTagDirectiveTo(value, false, tagDirectiveCollection);
			}
			foreach (TagDirective tagDirective in Constants.DefaultTagDirectives)
			{
				tagDirectiveCollection.Remove(tagDirective);
			}
			return tagDirectiveCollection;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0001E354 File Offset: 0x0001C554
		private void AnalyzeVersionDirective(VersionDirective versionDirective)
		{
			if (versionDirective.Version.Major != 1 || versionDirective.Version.Minor > 3)
			{
				throw new YamlException("Incompatible %YAML directive");
			}
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0001E37D File Offset: 0x0001C57D
		private static void AppendTagDirectiveTo(TagDirective value, bool allowDuplicates, TagDirectiveCollection tagDirectives)
		{
			if (tagDirectives.Contains(value))
			{
				if (!allowDuplicates)
				{
					throw new YamlException("Duplicate %TAG directive.");
				}
			}
			else
			{
				tagDirectives.Add(value);
			}
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0001E39D File Offset: 0x0001C59D
		private void EmitDocumentContent(ParsingEvent evt)
		{
			this.states.Push(EmitterState.DocumentEnd);
			this.EmitNode(evt, false, false);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0001E3B4 File Offset: 0x0001C5B4
		private void EmitNode(ParsingEvent evt, bool isMapping, bool isSimpleKey)
		{
			this.isMappingContext = isMapping;
			this.isSimpleKeyContext = isSimpleKey;
			switch (evt.Type)
			{
			case EventType.Alias:
				this.EmitAlias();
				return;
			case EventType.Scalar:
				this.EmitScalar(evt);
				return;
			case EventType.SequenceStart:
				this.EmitSequenceStart(evt);
				return;
			case EventType.MappingStart:
				this.EmitMappingStart(evt);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(62, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Expected SCALAR, SEQUENCE-START, MAPPING-START, or ALIAS, got ");
			defaultInterpolatedStringHandler.AppendFormatted<EventType>(evt.Type);
			throw new YamlException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0001E442 File Offset: 0x0001C642
		private void EmitAlias()
		{
			this.ProcessAnchor();
			this.state = this.states.Pop();
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0001E45C File Offset: 0x0001C65C
		private void EmitScalar(ParsingEvent evt)
		{
			this.SelectScalarStyle(evt);
			this.ProcessAnchor();
			this.ProcessTag();
			this.IncreaseIndent(true, false);
			this.ProcessScalar();
			this.indent = this.indents.Pop();
			this.state = this.states.Pop();
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001E4AC File Offset: 0x0001C6AC
		private void SelectScalarStyle(ParsingEvent evt)
		{
			YamlDotNet.Core.Events.Scalar scalar = (YamlDotNet.Core.Events.Scalar)evt;
			ScalarStyle scalarStyle = scalar.Style;
			bool flag = this.tagData.Handle == null && this.tagData.Suffix == null;
			if (flag && !scalar.IsPlainImplicit && !scalar.IsQuotedImplicit)
			{
				throw new YamlException("Neither tag nor isImplicit flags are specified.");
			}
			if (scalarStyle == ScalarStyle.Any)
			{
				scalarStyle = (this.scalarData.IsMultiline ? ScalarStyle.Folded : ScalarStyle.Plain);
			}
			if (this.isCanonical)
			{
				scalarStyle = ScalarStyle.DoubleQuoted;
			}
			if (this.isSimpleKeyContext && this.scalarData.IsMultiline)
			{
				scalarStyle = ScalarStyle.DoubleQuoted;
			}
			if (scalarStyle == ScalarStyle.Plain)
			{
				if ((this.flowLevel != 0 && !this.scalarData.IsFlowPlainAllowed) || (this.flowLevel == 0 && !this.scalarData.IsBlockPlainAllowed))
				{
					scalarStyle = ((this.scalarData.IsSingleQuotedAllowed && !this.scalarData.HasSingleQuotes) ? ScalarStyle.SingleQuoted : ScalarStyle.DoubleQuoted);
				}
				if (string.IsNullOrEmpty(this.scalarData.Value) && (this.flowLevel != 0 || this.isSimpleKeyContext))
				{
					scalarStyle = ScalarStyle.SingleQuoted;
				}
				if (flag && !scalar.IsPlainImplicit)
				{
					scalarStyle = ScalarStyle.SingleQuoted;
				}
			}
			if (scalarStyle == ScalarStyle.SingleQuoted && !this.scalarData.IsSingleQuotedAllowed)
			{
				scalarStyle = ScalarStyle.DoubleQuoted;
			}
			if ((scalarStyle == ScalarStyle.Literal || scalarStyle == ScalarStyle.Folded) && (!this.scalarData.IsBlockAllowed || this.flowLevel != 0 || this.isSimpleKeyContext))
			{
				scalarStyle = ScalarStyle.DoubleQuoted;
			}
			this.scalarData.Style = scalarStyle;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0001E600 File Offset: 0x0001C800
		private void ProcessScalar()
		{
			switch (this.scalarData.Style)
			{
			case ScalarStyle.Plain:
				this.WritePlainScalar(this.scalarData.Value, !this.isSimpleKeyContext);
				return;
			case ScalarStyle.SingleQuoted:
				this.WriteSingleQuotedScalar(this.scalarData.Value, !this.isSimpleKeyContext);
				return;
			case ScalarStyle.DoubleQuoted:
				this.WriteDoubleQuotedScalar(this.scalarData.Value, !this.isSimpleKeyContext);
				return;
			case ScalarStyle.Literal:
				this.WriteLiteralScalar(this.scalarData.Value);
				return;
			case ScalarStyle.Folded:
				this.WriteFoldedScalar(this.scalarData.Value);
				return;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0001E6B4 File Offset: 0x0001C8B4
		private void WritePlainScalar(string value, bool allowBreaks)
		{
			if (!this.isWhitespace)
			{
				this.Write(' ');
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value.get_Chars(i);
				char breakCharacter;
				if (Emitter.IsSpace(c))
				{
					if (allowBreaks && !flag && this.column > this.bestWidth && i + 1 < value.Length && value.get_Chars(i + 1) != ' ')
					{
						this.WriteIndent();
					}
					else
					{
						this.Write(c);
					}
					flag = true;
				}
				else if (Emitter.IsBreak(c, out breakCharacter))
				{
					if (!flag2 && c == '\n')
					{
						this.WriteBreak('\n');
					}
					this.WriteBreak(breakCharacter);
					this.isIndentation = true;
					flag2 = true;
				}
				else
				{
					if (flag2)
					{
						this.WriteIndent();
					}
					this.Write(c);
					this.isIndentation = false;
					flag = false;
					flag2 = false;
				}
			}
			this.isWhitespace = false;
			this.isIndentation = false;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0001E794 File Offset: 0x0001C994
		private void WriteSingleQuotedScalar(string value, bool allowBreaks)
		{
			this.WriteIndicator("'", true, false, false);
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value.get_Chars(i);
				char breakCharacter;
				if (c == ' ')
				{
					if (allowBreaks && !flag && this.column > this.bestWidth && i != 0 && i + 1 < value.Length && value.get_Chars(i + 1) != ' ')
					{
						this.WriteIndent();
					}
					else
					{
						this.Write(c);
					}
					flag = true;
				}
				else if (Emitter.IsBreak(c, out breakCharacter))
				{
					if (!flag2 && c == '\n')
					{
						this.WriteBreak('\n');
					}
					this.WriteBreak(breakCharacter);
					this.isIndentation = true;
					flag2 = true;
				}
				else
				{
					if (flag2)
					{
						this.WriteIndent();
					}
					if (c == '\'')
					{
						this.Write(c);
					}
					this.Write(c);
					this.isIndentation = false;
					flag = false;
					flag2 = false;
				}
			}
			this.WriteIndicator("'", false, false, false);
			this.isWhitespace = false;
			this.isIndentation = false;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001E88C File Offset: 0x0001CA8C
		private void WriteDoubleQuotedScalar(string value, bool allowBreaks)
		{
			this.WriteIndicator("\"", true, false, false);
			bool flag = false;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value.get_Chars(i);
				char c2;
				if (!Emitter.IsPrintable(c) || Emitter.IsBreak(c, out c2) || c == '"' || c == '\\')
				{
					this.Write('\\');
					if (c <= '\\')
					{
						if (c <= '\u001b')
						{
							switch (c)
							{
							case '\0':
								this.Write('0');
								break;
							case '\u0001':
							case '\u0002':
							case '\u0003':
							case '\u0004':
							case '\u0005':
							case '\u0006':
								goto IL_1B1;
							case '\a':
								this.Write('a');
								break;
							case '\b':
								this.Write('b');
								break;
							case '\t':
								this.Write('t');
								break;
							case '\n':
								this.Write('n');
								break;
							case '\v':
								this.Write('v');
								break;
							case '\f':
								this.Write('f');
								break;
							case '\r':
								this.Write('r');
								break;
							default:
								if (c != '\u001b')
								{
									goto IL_1B1;
								}
								this.Write('e');
								break;
							}
						}
						else if (c != '"')
						{
							if (c != '\\')
							{
								goto IL_1B1;
							}
							this.Write('\\');
						}
						else
						{
							this.Write('"');
						}
					}
					else if (c <= '\u00a0')
					{
						if (c != '\u0085')
						{
							if (c != '\u00a0')
							{
								goto IL_1B1;
							}
							this.Write('_');
						}
						else
						{
							this.Write('N');
						}
					}
					else if (c != '\u2028')
					{
						if (c != '\u2029')
						{
							goto IL_1B1;
						}
						this.Write('P');
					}
					else
					{
						this.Write('L');
					}
					IL_264:
					flag = false;
					goto IL_2C1;
					IL_1B1:
					ushort num = (ushort)c;
					if (num <= 255)
					{
						this.Write('x');
						this.Write(num.ToString("X02", CultureInfo.InvariantCulture));
						goto IL_264;
					}
					if (!Emitter.IsHighSurrogate(c))
					{
						this.Write('u');
						this.Write(num.ToString("X04", CultureInfo.InvariantCulture));
						goto IL_264;
					}
					if (i + 1 < value.Length && Emitter.IsLowSurrogate(value.get_Chars(i + 1)))
					{
						this.Write('U');
						this.Write(char.ConvertToUtf32(c, value.get_Chars(i + 1)).ToString("X08", CultureInfo.InvariantCulture));
						i++;
						goto IL_264;
					}
					throw new SyntaxErrorException("While writing a quoted scalar, found an orphaned high surrogate.");
				}
				else if (c == ' ')
				{
					if (allowBreaks && !flag && this.column > this.bestWidth && i > 0 && i + 1 < value.Length)
					{
						this.WriteIndent();
						if (value.get_Chars(i + 1) == ' ')
						{
							this.Write('\\');
						}
					}
					else
					{
						this.Write(c);
					}
					flag = true;
				}
				else
				{
					this.Write(c);
					flag = false;
				}
				IL_2C1:;
			}
			this.WriteIndicator("\"", false, false, false);
			this.isWhitespace = false;
			this.isIndentation = false;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0001EB88 File Offset: 0x0001CD88
		private void WriteLiteralScalar(string value)
		{
			bool flag = true;
			this.WriteIndicator("|", true, false, false);
			this.WriteBlockScalarHints(value);
			this.WriteBreak('\n');
			this.isIndentation = true;
			this.isWhitespace = true;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value.get_Chars(i);
				if (c != '\r' || i + 1 >= value.Length || value.get_Chars(i + 1) != '\n')
				{
					char breakCharacter;
					if (Emitter.IsBreak(c, out breakCharacter))
					{
						this.WriteBreak(breakCharacter);
						this.isIndentation = true;
						flag = true;
					}
					else
					{
						if (flag)
						{
							this.WriteIndent();
						}
						this.Write(c);
						this.isIndentation = false;
						flag = false;
					}
				}
			}
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0001EC30 File Offset: 0x0001CE30
		private void WriteFoldedScalar(string value)
		{
			bool flag = true;
			bool flag2 = true;
			this.WriteIndicator(">", true, false, false);
			this.WriteBlockScalarHints(value);
			this.WriteBreak('\n');
			this.isIndentation = true;
			this.isWhitespace = true;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value.get_Chars(i);
				char c2;
				if (Emitter.IsBreak(c, out c2))
				{
					if (c != '\r' || i + 1 >= value.Length || value.get_Chars(i + 1) != '\n')
					{
						if (!flag && !flag2 && c2 == '\n')
						{
							int num = 0;
							char c3;
							while (i + num < value.Length && Emitter.IsBreak(value.get_Chars(i + num), out c3))
							{
								num++;
							}
							if (i + num < value.Length && !Emitter.IsBlank(value.get_Chars(i + num)) && !Emitter.IsBreak(value.get_Chars(i + num), out c3))
							{
								this.WriteBreak('\n');
							}
						}
						this.WriteBreak(c2);
						this.isIndentation = true;
						flag = true;
					}
				}
				else
				{
					if (flag)
					{
						this.WriteIndent();
						flag2 = Emitter.IsBlank(c);
					}
					if (!flag && c == ' ' && i + 1 < value.Length && value.get_Chars(i + 1) != ' ' && this.column > this.bestWidth)
					{
						this.WriteIndent();
					}
					else
					{
						this.Write(c);
					}
					this.isIndentation = false;
					flag = false;
				}
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0001ED8F File Offset: 0x0001CF8F
		private static bool IsSpace(char character)
		{
			return character == ' ';
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0001ED96 File Offset: 0x0001CF96
		private static bool IsBreak(char character, out char breakChar)
		{
			if (character <= '\r')
			{
				if (character != '\n' && character != '\r')
				{
					goto IL_36;
				}
			}
			else if (character != '\u0085')
			{
				if (character != '\u2028' && character != '\u2029')
				{
					goto IL_36;
				}
				breakChar = character;
				return true;
			}
			breakChar = '\n';
			return true;
			IL_36:
			breakChar = '\0';
			return false;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0001EDD2 File Offset: 0x0001CFD2
		private static bool IsBlank(char character)
		{
			return character == ' ' || character == '\t';
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0001EDE0 File Offset: 0x0001CFE0
		private static bool IsPrintable(char character)
		{
			return character == '\t' || character == '\n' || character == '\r' || (character >= ' ' && character <= '~') || character == '\u0085' || (character >= '\u00a0' && character <= '퟿') || (character >= '' && character <= '�');
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0001EE35 File Offset: 0x0001D035
		private static bool IsHighSurrogate(char c)
		{
			return '\ud800' <= c && c <= '\udbff';
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0001EE4C File Offset: 0x0001D04C
		private static bool IsLowSurrogate(char c)
		{
			return '\udc00' <= c && c <= '\udfff';
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001EE64 File Offset: 0x0001D064
		private void EmitSequenceStart(ParsingEvent evt)
		{
			this.ProcessAnchor();
			this.ProcessTag();
			SequenceStart sequenceStart = (SequenceStart)evt;
			if (this.flowLevel != 0 || this.isCanonical || sequenceStart.Style == SequenceStyle.Flow || this.CheckEmptySequence())
			{
				this.state = EmitterState.FlowSequenceFirstItem;
				return;
			}
			this.state = EmitterState.BlockSequenceFirstItem;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001EEB8 File Offset: 0x0001D0B8
		private void EmitMappingStart(ParsingEvent evt)
		{
			this.ProcessAnchor();
			this.ProcessTag();
			MappingStart mappingStart = (MappingStart)evt;
			if (this.flowLevel != 0 || this.isCanonical || mappingStart.Style == MappingStyle.Flow || this.CheckEmptyMapping())
			{
				this.state = EmitterState.FlowMappingFirstKey;
				return;
			}
			this.state = EmitterState.BlockMappingFirstKey;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001EF0C File Offset: 0x0001D10C
		private void ProcessAnchor()
		{
			if (!this.anchorData.Anchor.IsEmpty && !this.skipAnchorName)
			{
				this.WriteIndicator(this.anchorData.IsAlias ? "*" : "&", true, false, false);
				this.WriteAnchor(this.anchorData.Anchor);
			}
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001EF68 File Offset: 0x0001D168
		private void ProcessTag()
		{
			if (this.tagData.Handle == null && this.tagData.Suffix == null)
			{
				return;
			}
			if (this.tagData.Handle != null)
			{
				this.WriteTagHandle(this.tagData.Handle);
				if (this.tagData.Suffix != null)
				{
					this.WriteTagContent(this.tagData.Suffix, false);
					return;
				}
			}
			else
			{
				this.WriteIndicator("!<", true, false, false);
				this.WriteTagContent(this.tagData.Suffix, false);
				this.WriteIndicator(">", false, false, false);
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0001EFFC File Offset: 0x0001D1FC
		private void EmitDocumentEnd(ParsingEvent evt)
		{
			YamlDotNet.Core.Events.DocumentEnd documentEnd = evt as YamlDotNet.Core.Events.DocumentEnd;
			if (documentEnd != null)
			{
				this.WriteIndent();
				if (!documentEnd.IsImplicit)
				{
					this.WriteIndicator("...", true, false, false);
					this.WriteIndent();
					this.isDocumentEndWritten = true;
				}
				this.state = EmitterState.DocumentStart;
				this.tagDirectives.Clear();
				return;
			}
			throw new YamlException("Expected DOCUMENT-END.");
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0001F05C File Offset: 0x0001D25C
		private void EmitFlowSequenceItem(ParsingEvent evt, bool isFirst)
		{
			if (isFirst)
			{
				this.WriteIndicator("[", true, true, false);
				this.IncreaseIndent(true, false);
				this.flowLevel++;
			}
			if (evt is SequenceEnd)
			{
				this.flowLevel--;
				this.indent = this.indents.Pop();
				if (this.isCanonical && !isFirst)
				{
					this.WriteIndicator(",", false, false, false);
					this.WriteIndent();
				}
				this.WriteIndicator("]", false, false, false);
				this.state = this.states.Pop();
				return;
			}
			if (!isFirst)
			{
				this.WriteIndicator(",", false, false, false);
			}
			if (this.isCanonical || this.column > this.bestWidth)
			{
				this.WriteIndent();
			}
			this.states.Push(EmitterState.FlowSequenceItem);
			this.EmitNode(evt, false, false);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0001F138 File Offset: 0x0001D338
		private void EmitFlowMappingKey(ParsingEvent evt, bool isFirst)
		{
			if (isFirst)
			{
				this.WriteIndicator("{", true, true, false);
				this.IncreaseIndent(true, false);
				this.flowLevel++;
			}
			if (evt is MappingEnd)
			{
				this.flowLevel--;
				this.indent = this.indents.Pop();
				if (this.isCanonical && !isFirst)
				{
					this.WriteIndicator(",", false, false, false);
					this.WriteIndent();
				}
				this.WriteIndicator("}", false, false, false);
				this.state = this.states.Pop();
				return;
			}
			if (!isFirst)
			{
				this.WriteIndicator(",", false, false, false);
			}
			if (this.isCanonical || this.column > this.bestWidth)
			{
				this.WriteIndent();
			}
			if (!this.isCanonical && this.CheckSimpleKey())
			{
				this.states.Push(EmitterState.FlowMappingSimpleValue);
				this.EmitNode(evt, true, true);
				return;
			}
			this.WriteIndicator("?", true, false, false);
			this.states.Push(EmitterState.FlowMappingValue);
			this.EmitNode(evt, true, false);
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0001F24C File Offset: 0x0001D44C
		private void EmitFlowMappingValue(ParsingEvent evt, bool isSimple)
		{
			if (isSimple)
			{
				this.WriteIndicator(":", false, false, false);
			}
			else
			{
				if (this.isCanonical || this.column > this.bestWidth)
				{
					this.WriteIndent();
				}
				this.WriteIndicator(":", true, false, false);
			}
			this.states.Push(EmitterState.FlowMappingKey);
			this.EmitNode(evt, true, false);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0001F2AC File Offset: 0x0001D4AC
		private void EmitBlockSequenceItem(ParsingEvent evt, bool isFirst)
		{
			if (isFirst)
			{
				this.IncreaseIndent(false, this.isMappingContext && !this.isIndentation);
			}
			if (evt is SequenceEnd)
			{
				this.indent = this.indents.Pop();
				this.state = this.states.Pop();
				return;
			}
			this.WriteIndent();
			this.WriteIndicator("-", true, false, true);
			this.states.Push(EmitterState.BlockSequenceItem);
			this.EmitNode(evt, false, false);
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001F32C File Offset: 0x0001D52C
		private void EmitBlockMappingKey(ParsingEvent evt, bool isFirst)
		{
			if (isFirst)
			{
				this.IncreaseIndent(false, false);
			}
			if (evt is MappingEnd)
			{
				this.indent = this.indents.Pop();
				this.state = this.states.Pop();
				return;
			}
			this.WriteIndent();
			if (this.CheckSimpleKey())
			{
				this.states.Push(EmitterState.BlockMappingSimpleValue);
				this.EmitNode(evt, true, true);
				this.WriteIndicator(":", false, false, false);
				return;
			}
			this.WriteIndicator("?", true, false, true);
			this.states.Push(EmitterState.BlockMappingValue);
			this.EmitNode(evt, true, false);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0001F3C6 File Offset: 0x0001D5C6
		private void EmitBlockMappingValue(ParsingEvent evt, bool isSimple)
		{
			if (!isSimple)
			{
				this.WriteIndent();
				this.WriteIndicator(":", true, false, true);
			}
			this.states.Push(EmitterState.BlockMappingKey);
			this.EmitNode(evt, true, false);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0001F3F8 File Offset: 0x0001D5F8
		private void IncreaseIndent(bool isFlow, bool isIndentless)
		{
			this.indents.Push(this.indent);
			if (this.indent < 0)
			{
				this.indent = (isFlow ? this.bestIndent : 0);
				return;
			}
			if (!isIndentless || !this.forceIndentLess)
			{
				this.indent += this.bestIndent;
			}
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0001F450 File Offset: 0x0001D650
		private bool CheckEmptyDocument()
		{
			int num = 0;
			foreach (ParsingEvent parsingEvent in this.events)
			{
				num++;
				if (num == 2)
				{
					YamlDotNet.Core.Events.Scalar scalar = parsingEvent as YamlDotNet.Core.Events.Scalar;
					if (scalar != null)
					{
						return string.IsNullOrEmpty(scalar.Value);
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001F4C4 File Offset: 0x0001D6C4
		private bool CheckSimpleKey()
		{
			if (this.events.Count < 1)
			{
				return false;
			}
			int num;
			switch (this.events.Peek().Type)
			{
			case EventType.Alias:
				num = this.AnchorNameLength(this.anchorData.Anchor);
				goto IL_13B;
			case EventType.Scalar:
				if (this.scalarData.IsMultiline)
				{
					return false;
				}
				num = this.AnchorNameLength(this.anchorData.Anchor) + this.SafeStringLength(this.tagData.Handle) + this.SafeStringLength(this.tagData.Suffix) + this.SafeStringLength(this.scalarData.Value);
				goto IL_13B;
			case EventType.SequenceStart:
				if (!this.CheckEmptySequence())
				{
					return false;
				}
				num = this.AnchorNameLength(this.anchorData.Anchor) + this.SafeStringLength(this.tagData.Handle) + this.SafeStringLength(this.tagData.Suffix);
				goto IL_13B;
			case EventType.MappingStart:
				if (!this.CheckEmptySequence())
				{
					return false;
				}
				num = this.AnchorNameLength(this.anchorData.Anchor) + this.SafeStringLength(this.tagData.Handle) + this.SafeStringLength(this.tagData.Suffix);
				goto IL_13B;
			}
			return false;
			IL_13B:
			return num <= this.maxSimpleKeyLength;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001F618 File Offset: 0x0001D818
		private int AnchorNameLength(AnchorName value)
		{
			if (!value.IsEmpty)
			{
				return value.Value.Length;
			}
			return 0;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0001F631 File Offset: 0x0001D831
		[NullableContext(2)]
		private int SafeStringLength(string value)
		{
			if (value != null)
			{
				return value.Length;
			}
			return 0;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0001F63E File Offset: 0x0001D83E
		private bool CheckEmptySequence()
		{
			return this.CheckEmptyStructure<SequenceStart, SequenceEnd>();
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0001F646 File Offset: 0x0001D846
		private bool CheckEmptyMapping()
		{
			return this.CheckEmptyStructure<MappingStart, MappingEnd>();
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0001F650 File Offset: 0x0001D850
		[NullableContext(0)]
		private bool CheckEmptyStructure<TStart, TEnd>() where TStart : NodeEvent where TEnd : ParsingEvent
		{
			if (this.events.Count < 2)
			{
				return false;
			}
			bool result;
			using (Queue<ParsingEvent>.Enumerator enumerator = this.events.GetEnumerator())
			{
				result = (enumerator.MoveNext() && enumerator.Current is TStart && enumerator.MoveNext() && enumerator.Current is TEnd);
			}
			return result;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0001F6D0 File Offset: 0x0001D8D0
		private void WriteBlockScalarHints(string value)
		{
			CharacterAnalyzer<StringLookAheadBuffer> characterAnalyzer = new CharacterAnalyzer<StringLookAheadBuffer>(new StringLookAheadBuffer(value));
			if (characterAnalyzer.IsSpace(0) || characterAnalyzer.IsBreak(0))
			{
				string indicator = this.bestIndent.ToString(CultureInfo.InvariantCulture);
				this.WriteIndicator(indicator, false, false, false);
			}
			string text = null;
			if (value.Length == 0 || !characterAnalyzer.IsBreak(value.Length - 1))
			{
				text = "-";
			}
			else if (value.Length >= 2 && characterAnalyzer.IsBreak(value.Length - 2))
			{
				text = "+";
			}
			if (text != null)
			{
				this.WriteIndicator(text, false, false, false);
			}
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0001F764 File Offset: 0x0001D964
		private void WriteIndicator(string indicator, bool needWhitespace, bool whitespace, bool indentation)
		{
			if (needWhitespace && !this.isWhitespace)
			{
				this.Write(' ');
			}
			this.Write(indicator);
			this.isWhitespace = whitespace;
			this.isIndentation = (this.isIndentation && indentation);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0001F798 File Offset: 0x0001D998
		private void WriteIndent()
		{
			int num = Math.Max(this.indent, 0);
			if (!this.isIndentation || this.column > num || (this.column == num && !this.isWhitespace))
			{
				this.WriteBreak('\n');
			}
			while (this.column < num)
			{
				this.Write(' ');
			}
			this.isWhitespace = true;
			this.isIndentation = true;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0001F806 File Offset: 0x0001DA06
		private void WriteAnchor(AnchorName value)
		{
			this.Write(value.Value);
			this.isWhitespace = false;
			this.isIndentation = false;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0001F823 File Offset: 0x0001DA23
		private void WriteTagHandle(string value)
		{
			if (!this.isWhitespace)
			{
				this.Write(' ');
			}
			this.Write(value);
			this.isWhitespace = false;
			this.isIndentation = false;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0001F84A File Offset: 0x0001DA4A
		private void WriteTagContent(string value, bool needsWhitespace)
		{
			if (needsWhitespace && !this.isWhitespace)
			{
				this.Write(' ');
			}
			this.Write(Emitter.UrlEncode(value));
			this.isWhitespace = false;
			this.isIndentation = false;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0001F879 File Offset: 0x0001DA79
		private static string UrlEncode(string text)
		{
			return Emitter.UriReplacer.Replace(text, delegate(Match match)
			{
				string result;
				using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
				{
					StringBuilder builder = builderWrapper.Builder;
					foreach (byte b in Encoding.UTF8.GetBytes(match.Value))
					{
						builder.AppendFormat("%{0:X02}", b);
					}
					result = builder.ToString();
				}
				return result;
			});
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0001F8A5 File Offset: 0x0001DAA5
		private void Write(char value)
		{
			this.output.Write(value);
			this.column++;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0001F8C1 File Offset: 0x0001DAC1
		private void Write(string value)
		{
			this.output.Write(value);
			this.column += value.Length;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0001F8E2 File Offset: 0x0001DAE2
		private void WriteBreak(char breakCharacter = '\n')
		{
			if (breakCharacter == '\n')
			{
				this.output.Write(this.newLine);
			}
			else
			{
				this.output.Write(breakCharacter);
			}
			this.column = 0;
		}

		// Token: 0x040003BC RID: 956
		private static readonly Regex UriReplacer = new Regex("[^0-9A-Za-z_\\-;?@=$~\\\\\\)\\]/:&+,\\.\\*\\(\\[!]", 24);

		// Token: 0x040003BD RID: 957
		private static readonly string[] newLineSeparators = new string[]
		{
			"\r\n",
			"\r",
			"\n"
		};

		// Token: 0x040003BE RID: 958
		private readonly TextWriter output;

		// Token: 0x040003BF RID: 959
		private readonly bool outputUsesUnicodeEncoding;

		// Token: 0x040003C0 RID: 960
		private readonly int maxSimpleKeyLength;

		// Token: 0x040003C1 RID: 961
		private readonly bool isCanonical;

		// Token: 0x040003C2 RID: 962
		private readonly bool skipAnchorName;

		// Token: 0x040003C3 RID: 963
		private readonly int bestIndent;

		// Token: 0x040003C4 RID: 964
		private readonly int bestWidth;

		// Token: 0x040003C5 RID: 965
		private EmitterState state;

		// Token: 0x040003C6 RID: 966
		private readonly Stack<EmitterState> states = new Stack<EmitterState>();

		// Token: 0x040003C7 RID: 967
		private readonly Queue<ParsingEvent> events = new Queue<ParsingEvent>();

		// Token: 0x040003C8 RID: 968
		private readonly Stack<int> indents = new Stack<int>();

		// Token: 0x040003C9 RID: 969
		private readonly TagDirectiveCollection tagDirectives = new TagDirectiveCollection();

		// Token: 0x040003CA RID: 970
		private int indent;

		// Token: 0x040003CB RID: 971
		private int flowLevel;

		// Token: 0x040003CC RID: 972
		private bool isMappingContext;

		// Token: 0x040003CD RID: 973
		private bool isSimpleKeyContext;

		// Token: 0x040003CE RID: 974
		private int column;

		// Token: 0x040003CF RID: 975
		private bool isWhitespace;

		// Token: 0x040003D0 RID: 976
		private bool isIndentation;

		// Token: 0x040003D1 RID: 977
		private readonly bool forceIndentLess;

		// Token: 0x040003D2 RID: 978
		private readonly string newLine;

		// Token: 0x040003D3 RID: 979
		private bool isDocumentEndWritten;

		// Token: 0x040003D4 RID: 980
		private readonly Emitter.AnchorData anchorData = new Emitter.AnchorData();

		// Token: 0x040003D5 RID: 981
		private readonly Emitter.TagData tagData = new Emitter.TagData();

		// Token: 0x040003D6 RID: 982
		private readonly Emitter.ScalarData scalarData = new Emitter.ScalarData();

		// Token: 0x020001AE RID: 430
		[NullableContext(0)]
		private class AnchorData
		{
			// Token: 0x040003D7 RID: 983
			public AnchorName Anchor;

			// Token: 0x040003D8 RID: 984
			public bool IsAlias;
		}

		// Token: 0x020001AF RID: 431
		[NullableContext(2)]
		[Nullable(0)]
		private class TagData
		{
			// Token: 0x040003D9 RID: 985
			public string Handle;

			// Token: 0x040003DA RID: 986
			public string Suffix;
		}

		// Token: 0x020001B0 RID: 432
		[NullableContext(0)]
		private class ScalarData
		{
			// Token: 0x040003DB RID: 987
			[Nullable(1)]
			public string Value = string.Empty;

			// Token: 0x040003DC RID: 988
			public bool IsMultiline;

			// Token: 0x040003DD RID: 989
			public bool IsFlowPlainAllowed;

			// Token: 0x040003DE RID: 990
			public bool IsBlockPlainAllowed;

			// Token: 0x040003DF RID: 991
			public bool IsSingleQuotedAllowed;

			// Token: 0x040003E0 RID: 992
			public bool IsBlockAllowed;

			// Token: 0x040003E1 RID: 993
			public bool HasSingleQuotes;

			// Token: 0x040003E2 RID: 994
			public ScalarStyle Style;
		}
	}
}
