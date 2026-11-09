using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200017D RID: 381
	[NullableContext(1)]
	[Nullable(0)]
	public class LibYamlEventStream
	{
		// Token: 0x0600078C RID: 1932 RVA: 0x00019BDD File Offset: 0x00017DDD
		public LibYamlEventStream(IParser iParser)
		{
			if (iParser == null)
			{
				throw new ArgumentNullException("iParser");
			}
			this.parser = iParser;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00019BFC File Offset: 0x00017DFC
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
												this.WriteAnchorAndTag(textWriter, sequenceStart);
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
										this.WriteAnchorAndTag(textWriter, scalar);
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
												goto IL_22E;
											case '\r':
												textWriter.Write("\\r");
												break;
											default:
												if (c != '\\')
												{
													goto IL_22E;
												}
												textWriter.Write("\\\\");
												break;
											}
											IL_236:
											i++;
											continue;
											IL_22E:
											textWriter.Write(c);
											goto IL_236;
										}
									}
								}
								else
								{
									textWriter.Write("+MAP");
									this.WriteAnchorAndTag(textWriter, mappingStart);
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

		// Token: 0x0600078E RID: 1934 RVA: 0x00019EA8 File Offset: 0x000180A8
		private void WriteAnchorAndTag(TextWriter textWriter, NodeEvent nodeEvent)
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

		// Token: 0x0400033A RID: 826
		private readonly IParser parser;
	}
}
