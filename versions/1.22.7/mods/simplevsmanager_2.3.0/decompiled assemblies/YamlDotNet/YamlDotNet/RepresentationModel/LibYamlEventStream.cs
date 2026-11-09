using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000A7 RID: 167
	[NullableContext(1)]
	[Nullable(0)]
	public class LibYamlEventStream
	{
		// Token: 0x0600037E RID: 894 RVA: 0x0000C845 File Offset: 0x0000AA45
		public LibYamlEventStream(IParser iParser)
		{
			if (iParser == null)
			{
				throw new ArgumentNullException("iParser");
			}
			this.parser = iParser;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000C864 File Offset: 0x0000AA64
		public void WriteTo(TextWriter textWriter)
		{
			while (this.parser.MoveNext())
			{
				ParsingEvent parsingEvent = this.parser.Current;
				AnchorAlias anchorAlias = parsingEvent as AnchorAlias;
				if (anchorAlias == null)
				{
					DocumentEnd documentEnd = parsingEvent as DocumentEnd;
					if (documentEnd == null)
					{
						DocumentStart documentStart = parsingEvent as DocumentStart;
						if (documentStart == null)
						{
							if (!(parsingEvent is MappingEnd))
							{
								MappingStart mappingStart = parsingEvent as MappingStart;
								if (mappingStart == null)
								{
									Scalar scalar = parsingEvent as Scalar;
									if (scalar == null)
									{
										if (!(parsingEvent is SequenceEnd))
										{
											SequenceStart sequenceStart = parsingEvent as SequenceStart;
											if (sequenceStart == null)
											{
												if (!(parsingEvent is StreamEnd))
												{
													if (parsingEvent is StreamStart)
													{
														textWriter.Write("+STR");
													}
												}
												else
												{
													textWriter.Write("-STR");
												}
											}
											else
											{
												textWriter.Write("+SEQ");
												LibYamlEventStream.WriteAnchorAndTag(textWriter, sequenceStart);
											}
										}
										else
										{
											textWriter.Write("-SEQ");
										}
									}
									else
									{
										textWriter.Write("=VAL");
										LibYamlEventStream.WriteAnchorAndTag(textWriter, scalar);
										switch (scalar.Style)
										{
										case ScalarStyle.SingleQuoted:
											textWriter.Write(" '");
											break;
										case ScalarStyle.DoubleQuoted:
											textWriter.Write(" \"");
											break;
										case ScalarStyle.Literal:
											textWriter.Write(" |");
											break;
										case ScalarStyle.Folded:
											textWriter.Write(" >");
											break;
										default:
											textWriter.Write(" :");
											break;
										}
										string value = scalar.Value;
										int i = 0;
										while (i < value.Length)
										{
											char c = value.get_Chars(i);
											switch (c)
											{
											case '\b':
												textWriter.Write("\\b");
												break;
											case '\t':
												textWriter.Write("\\t");
												break;
											case '\n':
												textWriter.Write("\\n");
												break;
											case '\v':
											case '\f':
												goto IL_22C;
											case '\r':
												textWriter.Write("\\r");
												break;
											default:
												if (c != '\\')
												{
													goto IL_22C;
												}
												textWriter.Write("\\\\");
												break;
											}
											IL_234:
											i++;
											continue;
											IL_22C:
											textWriter.Write(c);
											goto IL_234;
										}
									}
								}
								else
								{
									textWriter.Write("+MAP");
									LibYamlEventStream.WriteAnchorAndTag(textWriter, mappingStart);
								}
							}
							else
							{
								textWriter.Write("-MAP");
							}
						}
						else
						{
							textWriter.Write("+DOC");
							if (!documentStart.IsImplicit)
							{
								textWriter.Write(" ---");
							}
						}
					}
					else
					{
						textWriter.Write("-DOC");
						if (!documentEnd.IsImplicit)
						{
							textWriter.Write(" ...");
						}
					}
				}
				else
				{
					textWriter.Write("=ALI *");
					textWriter.Write(anchorAlias.Value);
				}
				textWriter.WriteLine();
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000CB0C File Offset: 0x0000AD0C
		private static void WriteAnchorAndTag(TextWriter textWriter, NodeEvent nodeEvent)
		{
			if (!nodeEvent.Anchor.IsEmpty)
			{
				textWriter.Write(" &");
				textWriter.Write(nodeEvent.Anchor);
			}
			if (!nodeEvent.Tag.IsEmpty)
			{
				textWriter.Write(" <");
				textWriter.Write(nodeEvent.Tag.Value);
				textWriter.Write(">");
			}
		}

		// Token: 0x04000102 RID: 258
		private readonly IParser parser;
	}
}
