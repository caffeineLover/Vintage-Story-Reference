using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.ObjectPool;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020000C5 RID: 197
	[NullableContext(1)]
	[Nullable(0)]
	public class Emitter : IEmitter
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x0000F11E File Offset: 0x0000D31E
		public Emitter(TextWriter output) : this(output, EmitterSettings.Default)
		{
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000F12C File Offset: 0x0000D32C
		public Emitter(TextWriter output, int bestIndent) : this(output, bestIndent, int.MaxValue)
		{
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000F13B File Offset: 0x0000D33B
		public Emitter(TextWriter output, int bestIndent, int bestWidth) : this(output, bestIndent, bestWidth, false)
		{
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000F148 File Offset: 0x0000D348
		public Emitter(TextWriter output, int bestIndent, int bestWidth, bool isCanonical) : this(output, new EmitterSettings(bestIndent, bestWidth, isCanonical, 1024, false, false, null, false))
		{
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000F170 File Offset: 0x0000D370
		public Emitter(TextWriter output, EmitterSettings settings)
		{
			this.bestIndent = settings.BestIndent;
			this.bestWidth = settings.BestWidth;
			this.isCanonical = settings.IsCanonical;
			this.maxSimpleKeyLength = settings.MaxSimpleKeyLength;
			this.skipAnchorName = settings.SkipAnchorName;
			this.forceIndentLess = !settings.IndentSequences;
			this.useUtf16SurrogatePair = settings.UseUtf16SurrogatePairs;
			this.output = output;
			this.output.NewLine = settings.NewLine;
			this.outputUsesUnicodeEncoding = Emitter.IsUnicode(output.Encoding);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000F250 File Offset: 0x0000D450
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

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000F2AC File Offset: 0x0000D4AC
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
			foreach (ParsingEvent parsingEvent in this.events)
			{
				switch (parsingEvent.Type)
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
			return true;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000F394 File Offset: 0x0000D594
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

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000F435 File Offset: 0x0000D635
		private void AnalyzeAnchor(AnchorName anchor, bool isAlias)
		{
			this.anchorData.Anchor = anchor;
			this.anchorData.IsAlias = isAlias;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000F450 File Offset: 0x0000D650
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
				using (StringLookAheadBufferPool.BufferWrapper bufferWrapper = StringLookAheadBufferPool.Rent(value))
				{
					CharacterAnalyzer<StringLookAheadBuffer> characterAnalyzer = new CharacterAnalyzer<StringLookAheadBuffer>(bufferWrapper.Buffer);
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
							if (characterAnalyzer.Check("#,[]{}&*!|>\"%@`'", 0))
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
				}
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

		// Token: 0x060004BA RID: 1210 RVA: 0x0000F85C File Offset: 0x0000DA5C
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
				string @string = this.output.Encoding.GetString(bytes, 0, bytes.Length);
				result = @string.Equals(value);
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

		// Token: 0x060004BB RID: 1211 RVA: 0x0000F8D0 File Offset: 0x0000DAD0
		private static bool IsUnicode(Encoding encoding)
		{
			return encoding is UTF8Encoding || encoding is UnicodeEncoding || encoding is UTF7Encoding;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000F8F0 File Offset: 0x0000DAF0
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

		// Token: 0x060004BD RID: 1213 RVA: 0x0000F994 File Offset: 0x0000DB94
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

		// Token: 0x060004BE RID: 1214 RVA: 0x0000FAB4 File Offset: 0x0000DCB4
		private void EmitComment(YamlDotNet.Core.Events.Comment comment)
		{
			if (this.flowLevel > 0 || this.state == EmitterState.FlowMappingFirstKey || this.state == EmitterState.FlowSequenceFirstItem)
			{
				return;
			}
			string[] array = comment.Value.Split(Emitter.NewLineSeparators, 0);
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

		// Token: 0x060004BF RID: 1215 RVA: 0x0000FB7C File Offset: 0x0000DD7C
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

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000FBBC File Offset: 0x0000DDBC
		private void EmitDocumentStart(ParsingEvent evt, bool isFirst)
		{
			YamlDotNet.Core.Events.DocumentStart documentStart = evt as YamlDotNet.Core.Events.DocumentStart;
			if (documentStart != null)
			{
				bool flag = documentStart.IsImplicit && isFirst && !this.isCanonical;
				TagDirectiveCollection tagDirectiveCollection = Emitter.NonDefaultTagsAmong(documentStart.Tags);
				if (!isFirst && !this.isDocumentEndWritten && (documentStart.Version != null || tagDirectiveCollection.Count > 0))
				{
					this.isDocumentEndWritten = false;
					this.WriteIndicator("...", true, false, false);
					this.WriteIndent();
				}
				if (documentStart.Version != null)
				{
					Emitter.AnalyzeVersionDirective(documentStart.Version);
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
				foreach (TagDirective value2 in Constants.DefaultTagDirectives)
				{
					Emitter.AppendTagDirectiveTo(value2, true, this.tagDirectives);
				}
				if (tagDirectiveCollection.Count > 0)
				{
					flag = false;
					foreach (TagDirective value3 in Constants.DefaultTagDirectives)
					{
						Emitter.AppendTagDirectiveTo(value3, true, tagDirectiveCollection);
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

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000FDF8 File Offset: 0x0000DFF8
		private static TagDirectiveCollection NonDefaultTagsAmong([Nullable(new byte[]
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

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000FE74 File Offset: 0x0000E074
		private static void AnalyzeVersionDirective(VersionDirective versionDirective)
		{
			if (versionDirective.Version.Major != 1 || versionDirective.Version.Minor > 3)
			{
				throw new YamlException("Incompatible %YAML directive");
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000FE9D File Offset: 0x0000E09D
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

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000FEBD File Offset: 0x0000E0BD
		private void EmitDocumentContent(ParsingEvent evt)
		{
			this.states.Push(EmitterState.DocumentEnd);
			this.EmitNode(evt, false, false);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000FED4 File Offset: 0x0000E0D4
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

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000FF62 File Offset: 0x0000E162
		private void EmitAlias()
		{
			this.ProcessAnchor();
			this.state = this.states.Pop();
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0000FF7C File Offset: 0x0000E17C
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

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000FFCC File Offset: 0x0000E1CC
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
			if (scalarStyle == ScalarStyle.ForcePlain)
			{
				scalarStyle = ScalarStyle.Plain;
			}
			this.scalarData.Style = scalarStyle;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00010124 File Offset: 0x0000E324
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

		// Token: 0x060004CA RID: 1226 RVA: 0x000101D8 File Offset: 0x0000E3D8
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

		// Token: 0x060004CB RID: 1227 RVA: 0x000102B8 File Offset: 0x0000E4B8
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

		// Token: 0x060004CC RID: 1228 RVA: 0x000103B0 File Offset: 0x0000E5B0
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
					IL_2C8:
					flag = false;
					goto IL_325;
					IL_1B1:
					ushort num = (ushort)c;
					if (num <= 255)
					{
						this.Write('x');
						this.Write(num.ToString("X02", CultureInfo.InvariantCulture));
						goto IL_2C8;
					}
					if (!Emitter.IsHighSurrogate(c))
					{
						this.Write('u');
						this.Write(num.ToString("X04", CultureInfo.InvariantCulture));
						goto IL_2C8;
					}
					if (i + 1 < value.Length && Emitter.IsLowSurrogate(value.get_Chars(i + 1)))
					{
						if (this.useUtf16SurrogatePair)
						{
							this.Write('u');
							this.Write(num.ToString("X04", CultureInfo.InvariantCulture));
							this.Write('\\');
							this.Write('u');
							this.Write(((ushort)value.get_Chars(i + 1)).ToString("X04", CultureInfo.InvariantCulture));
						}
						else
						{
							this.Write('U');
							this.Write(char.ConvertToUtf32(c, value.get_Chars(i + 1)).ToString("X08", CultureInfo.InvariantCulture));
						}
						i++;
						goto IL_2C8;
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
				IL_325:;
			}
			this.WriteIndicator("\"", false, false, false);
			this.isWhitespace = false;
			this.isIndentation = false;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00010710 File Offset: 0x0000E910
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

		// Token: 0x060004CE RID: 1230 RVA: 0x000107B8 File Offset: 0x0000E9B8
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

		// Token: 0x060004CF RID: 1231 RVA: 0x00010917 File Offset: 0x0000EB17
		private static bool IsSpace(char character)
		{
			return character == ' ';
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001091E File Offset: 0x0000EB1E
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

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001095A File Offset: 0x0000EB5A
		private static bool IsBlank(char character)
		{
			return character == ' ' || character == '\t';
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00010968 File Offset: 0x0000EB68
		private static bool IsPrintable(char character)
		{
			return character == '\t' || character == '\n' || character == '\r' || (character >= ' ' && character <= '~') || character == '\u0085' || (character >= '\u00a0' && character <= '퟿') || (character >= '' && character <= '�');
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000109BD File Offset: 0x0000EBBD
		private static bool IsHighSurrogate(char c)
		{
			return '\ud800' <= c && c <= '\udbff';
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000109D4 File Offset: 0x0000EBD4
		private static bool IsLowSurrogate(char c)
		{
			return '\udc00' <= c && c <= '\udfff';
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x000109EC File Offset: 0x0000EBEC
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

		// Token: 0x060004D6 RID: 1238 RVA: 0x00010A40 File Offset: 0x0000EC40
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

		// Token: 0x060004D7 RID: 1239 RVA: 0x00010A94 File Offset: 0x0000EC94
		private void ProcessAnchor()
		{
			if (!this.anchorData.Anchor.IsEmpty && !this.skipAnchorName)
			{
				this.WriteIndicator(this.anchorData.IsAlias ? "*" : "&", true, false, false);
				this.WriteAnchor(this.anchorData.Anchor);
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00010AF0 File Offset: 0x0000ECF0
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

		// Token: 0x060004D9 RID: 1241 RVA: 0x00010B84 File Offset: 0x0000ED84
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

		// Token: 0x060004DA RID: 1242 RVA: 0x00010BE4 File Offset: 0x0000EDE4
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

		// Token: 0x060004DB RID: 1243 RVA: 0x00010CC0 File Offset: 0x0000EEC0
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

		// Token: 0x060004DC RID: 1244 RVA: 0x00010DD4 File Offset: 0x0000EFD4
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

		// Token: 0x060004DD RID: 1245 RVA: 0x00010E34 File Offset: 0x0000F034
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

		// Token: 0x060004DE RID: 1246 RVA: 0x00010EB4 File Offset: 0x0000F0B4
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

		// Token: 0x060004DF RID: 1247 RVA: 0x00010F4E File Offset: 0x0000F14E
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

		// Token: 0x060004E0 RID: 1248 RVA: 0x00010F80 File Offset: 0x0000F180
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

		// Token: 0x060004E1 RID: 1249 RVA: 0x00010FD8 File Offset: 0x0000F1D8
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

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001104C File Offset: 0x0000F24C
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
				num = Emitter.AnchorNameLength(this.anchorData.Anchor);
				goto IL_130;
			case EventType.Scalar:
				if (this.scalarData.IsMultiline)
				{
					return false;
				}
				num = Emitter.AnchorNameLength(this.anchorData.Anchor) + Emitter.SafeStringLength(this.tagData.Handle) + Emitter.SafeStringLength(this.tagData.Suffix) + Emitter.SafeStringLength(this.scalarData.Value);
				goto IL_130;
			case EventType.SequenceStart:
				if (!this.CheckEmptySequence())
				{
					return false;
				}
				num = Emitter.AnchorNameLength(this.anchorData.Anchor) + Emitter.SafeStringLength(this.tagData.Handle) + Emitter.SafeStringLength(this.tagData.Suffix);
				goto IL_130;
			case EventType.MappingStart:
				if (!this.CheckEmptySequence())
				{
					return false;
				}
				num = Emitter.AnchorNameLength(this.anchorData.Anchor) + Emitter.SafeStringLength(this.tagData.Handle) + Emitter.SafeStringLength(this.tagData.Suffix);
				goto IL_130;
			}
			return false;
			IL_130:
			return num <= this.maxSimpleKeyLength;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00011195 File Offset: 0x0000F395
		private static int AnchorNameLength(AnchorName value)
		{
			if (!value.IsEmpty)
			{
				return value.Value.Length;
			}
			return 0;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x000111AE File Offset: 0x0000F3AE
		[NullableContext(2)]
		private static int SafeStringLength(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return value.Length;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000111BB File Offset: 0x0000F3BB
		private bool CheckEmptySequence()
		{
			return this.CheckEmptyStructure<SequenceStart, SequenceEnd>();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000111C3 File Offset: 0x0000F3C3
		private bool CheckEmptyMapping()
		{
			return this.CheckEmptyStructure<MappingStart, MappingEnd>();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000111CC File Offset: 0x0000F3CC
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

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001124C File Offset: 0x0000F44C
		private void WriteBlockScalarHints(string value)
		{
			using (StringLookAheadBufferPool.BufferWrapper bufferWrapper = StringLookAheadBufferPool.Rent(value))
			{
				CharacterAnalyzer<StringLookAheadBuffer> characterAnalyzer = new CharacterAnalyzer<StringLookAheadBuffer>(bufferWrapper.Buffer);
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
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001130C File Offset: 0x0000F50C
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

		// Token: 0x060004EA RID: 1258 RVA: 0x00011340 File Offset: 0x0000F540
		private void WriteIndent()
		{
			int num = Math.Max(this.indent, 0);
			bool flag = !this.isIndentation || this.column > num || (this.column == num && !this.isWhitespace);
			if (flag)
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

		// Token: 0x060004EB RID: 1259 RVA: 0x000113B0 File Offset: 0x0000F5B0
		private void WriteAnchor(AnchorName value)
		{
			this.Write(value.Value);
			this.isWhitespace = false;
			this.isIndentation = false;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000113CD File Offset: 0x0000F5CD
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

		// Token: 0x060004ED RID: 1261 RVA: 0x000113F4 File Offset: 0x0000F5F4
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

		// Token: 0x060004EE RID: 1262 RVA: 0x00011423 File Offset: 0x0000F623
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
						builder.AppendFormat(CultureInfo.InvariantCulture, "%{0:X02}", b);
					}
					result = builder.ToString();
				}
				return result;
			});
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001144F File Offset: 0x0000F64F
		private void Write(char value)
		{
			this.output.Write(value);
			this.column++;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001146B File Offset: 0x0000F66B
		private void Write(string value)
		{
			this.output.Write(value);
			this.column += value.Length;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001148C File Offset: 0x0000F68C
		private void WriteBreak(char breakCharacter = '\n')
		{
			if (breakCharacter == '\n')
			{
				this.output.WriteLine();
			}
			else
			{
				this.output.Write(breakCharacter);
			}
			this.column = 0;
		}

		// Token: 0x04000127 RID: 295
		private static readonly Regex UriReplacer = new Regex("[^0-9A-Za-z_\\-;?@=$~\\\\\\)\\]/:&+,\\.\\*\\(\\[!]", 24);

		// Token: 0x04000128 RID: 296
		private static readonly string[] NewLineSeparators = new string[]
		{
			"\r\n",
			"\r",
			"\n"
		};

		// Token: 0x04000129 RID: 297
		private readonly TextWriter output;

		// Token: 0x0400012A RID: 298
		private readonly bool outputUsesUnicodeEncoding;

		// Token: 0x0400012B RID: 299
		private readonly int maxSimpleKeyLength;

		// Token: 0x0400012C RID: 300
		private readonly bool isCanonical;

		// Token: 0x0400012D RID: 301
		private readonly bool skipAnchorName;

		// Token: 0x0400012E RID: 302
		private readonly int bestIndent;

		// Token: 0x0400012F RID: 303
		private readonly int bestWidth;

		// Token: 0x04000130 RID: 304
		private EmitterState state;

		// Token: 0x04000131 RID: 305
		private readonly Stack<EmitterState> states = new Stack<EmitterState>();

		// Token: 0x04000132 RID: 306
		private readonly Queue<ParsingEvent> events = new Queue<ParsingEvent>();

		// Token: 0x04000133 RID: 307
		private readonly Stack<int> indents = new Stack<int>();

		// Token: 0x04000134 RID: 308
		private readonly TagDirectiveCollection tagDirectives = new TagDirectiveCollection();

		// Token: 0x04000135 RID: 309
		private int indent;

		// Token: 0x04000136 RID: 310
		private int flowLevel;

		// Token: 0x04000137 RID: 311
		private bool isMappingContext;

		// Token: 0x04000138 RID: 312
		private bool isSimpleKeyContext;

		// Token: 0x04000139 RID: 313
		private int column;

		// Token: 0x0400013A RID: 314
		private bool isWhitespace;

		// Token: 0x0400013B RID: 315
		private bool isIndentation;

		// Token: 0x0400013C RID: 316
		private readonly bool forceIndentLess;

		// Token: 0x0400013D RID: 317
		private readonly bool useUtf16SurrogatePair;

		// Token: 0x0400013E RID: 318
		private bool isDocumentEndWritten;

		// Token: 0x0400013F RID: 319
		private readonly Emitter.AnchorData anchorData = new Emitter.AnchorData();

		// Token: 0x04000140 RID: 320
		private readonly Emitter.TagData tagData = new Emitter.TagData();

		// Token: 0x04000141 RID: 321
		private readonly Emitter.ScalarData scalarData = new Emitter.ScalarData();

		// Token: 0x02000196 RID: 406
		[NullableContext(0)]
		private class AnchorData
		{
			// Token: 0x04000344 RID: 836
			public AnchorName Anchor;

			// Token: 0x04000345 RID: 837
			public bool IsAlias;
		}

		// Token: 0x02000197 RID: 407
		[NullableContext(2)]
		[Nullable(0)]
		private class TagData
		{
			// Token: 0x04000346 RID: 838
			public string Handle;

			// Token: 0x04000347 RID: 839
			public string Suffix;
		}

		// Token: 0x02000198 RID: 408
		[NullableContext(0)]
		private class ScalarData
		{
			// Token: 0x04000348 RID: 840
			[Nullable(1)]
			public string Value = string.Empty;

			// Token: 0x04000349 RID: 841
			public bool IsMultiline;

			// Token: 0x0400034A RID: 842
			public bool IsFlowPlainAllowed;

			// Token: 0x0400034B RID: 843
			public bool IsBlockPlainAllowed;

			// Token: 0x0400034C RID: 844
			public bool IsSingleQuotedAllowed;

			// Token: 0x0400034D RID: 845
			public bool IsBlockAllowed;

			// Token: 0x0400034E RID: 846
			public bool HasSingleQuotes;

			// Token: 0x0400034F RID: 847
			public ScalarStyle Style;
		}
	}
}
