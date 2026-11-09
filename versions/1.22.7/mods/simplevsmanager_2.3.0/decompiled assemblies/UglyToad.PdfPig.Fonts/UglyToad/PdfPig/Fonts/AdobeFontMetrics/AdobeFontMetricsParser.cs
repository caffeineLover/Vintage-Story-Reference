using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.AdobeFontMetrics
{
	// Token: 0x020000B3 RID: 179
	public static class AdobeFontMetricsParser
	{
		// Token: 0x0600049C RID: 1180 RVA: 0x0001E39C File Offset: 0x0001C59C
		public static AdobeFontMetrics Parse(IInputBytes bytes, bool useReducedDataSet)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = AdobeFontMetricsParser.ReadString(bytes, stringBuilder);
			if (!string.Equals("StartFontMetrics", text, 5))
			{
				throw new InvalidFontFormatException("The AFM file was not valid, it did not start with StartFontMetrics.");
			}
			AdobeFontMetricsBuilder adobeFontMetricsBuilder = new AdobeFontMetricsBuilder(AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder));
			while ((text = AdobeFontMetricsParser.ReadString(bytes, stringBuilder)) != "EndFontMetrics")
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 1897437205U)
				{
					if (num <= 1272698556U)
					{
						if (num <= 476263234U)
						{
							if (num != 179515601U)
							{
								if (num != 469818072U)
								{
									if (num == 476263234U)
									{
										if (text == "FamilyName")
										{
											adobeFontMetricsBuilder.FamilyName = AdobeFontMetricsParser.ReadLine(bytes, stringBuilder);
										}
									}
								}
								else if (text == "XHeight")
								{
									adobeFontMetricsBuilder.XHeight = AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
								}
							}
							else if (text == "Notice")
							{
								adobeFontMetricsBuilder.Notice = AdobeFontMetricsParser.ReadLine(bytes, stringBuilder);
							}
						}
						else if (num <= 1105455451U)
						{
							if (num != 897139351U)
							{
								if (num == 1105455451U)
								{
									if (text == "StdHW")
									{
										adobeFontMetricsBuilder.StdHw = AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
									}
								}
							}
							else if (text == "EncodingScheme")
							{
								adobeFontMetricsBuilder.EncodingScheme = AdobeFontMetricsParser.ReadLine(bytes, stringBuilder);
							}
						}
						else if (num != 1204131559U)
						{
							if (num == 1272698556U)
							{
								if (text == "UnderlinePosition")
								{
									adobeFontMetricsBuilder.UnderlinePosition = AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
								}
							}
						}
						else if (!(text == "StartKernData"))
						{
						}
					}
					else if (num <= 1573770551U)
					{
						if (num != 1348852243U)
						{
							if (num != 1393414316U)
							{
								if (num == 1573770551U)
								{
									if (text == "Version")
									{
										adobeFontMetricsBuilder.Version = AdobeFontMetricsParser.ReadLine(bytes, stringBuilder);
									}
								}
							}
							else if (text == "Ascender")
							{
								adobeFontMetricsBuilder.Ascender = AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
							}
						}
						else if (text == "FontName")
						{
							adobeFontMetricsBuilder.FontName = AdobeFontMetricsParser.ReadLine(bytes, stringBuilder);
						}
					}
					else if (num <= 1873186290U)
					{
						if (num != 1607214848U)
						{
							if (num == 1873186290U)
							{
								if (text == "EscChar")
								{
									adobeFontMetricsBuilder.EscapeCharacter = (int)AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
								}
							}
						}
						else if (text == "CharacterSet")
						{
							adobeFontMetricsBuilder.CharacterSet = AdobeFontMetricsParser.ReadLine(bytes, stringBuilder);
						}
					}
					else if (num != 1882644808U)
					{
						if (num == 1897437205U)
						{
							if (text == "IsBaseFont")
							{
								adobeFontMetricsBuilder.IsBaseFont = AdobeFontMetricsParser.ReadBool(bytes, stringBuilder);
							}
						}
					}
					else if (text == "StartCharMetrics")
					{
						int num2 = (int)AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
						for (int i = 0; i < num2; i++)
						{
							AdobeFontMetricsIndividualCharacterMetric adobeFontMetricsIndividualCharacterMetric = AdobeFontMetricsParser.ReadCharacterMetric(bytes, stringBuilder);
							adobeFontMetricsBuilder.CharacterMetrics.Add(adobeFontMetricsIndividualCharacterMetric);
						}
						string text2 = AdobeFontMetricsParser.ReadString(bytes, stringBuilder);
						if (text2 != "EndCharMetrics")
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
							defaultInterpolatedStringHandler..ctor(64, 2);
							defaultInterpolatedStringHandler.AppendLiteral("The character metrics section did not end with ");
							defaultInterpolatedStringHandler.AppendFormatted("EndCharMetrics");
							defaultInterpolatedStringHandler.AppendLiteral(" instead it was ");
							defaultInterpolatedStringHandler.AppendFormatted(text2);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
				}
				else if (num <= 2556651472U)
				{
					if (num <= 2361969293U)
					{
						if (num != 2175677473U)
						{
							if (num != 2229825399U)
							{
								if (num == 2361969293U)
								{
									if (text == "IsFixedV")
									{
										adobeFontMetricsBuilder.IsFixedV = AdobeFontMetricsParser.ReadBool(bytes, stringBuilder);
									}
								}
							}
							else if (text == "FullName")
							{
								adobeFontMetricsBuilder.FullName = AdobeFontMetricsParser.ReadLine(bytes, stringBuilder);
							}
						}
						else if (text == "StdVW")
						{
							adobeFontMetricsBuilder.StdVw = AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
						}
					}
					else if (num <= 2503448551U)
					{
						if (num != 2454773910U)
						{
							if (num == 2503448551U)
							{
								if (text == "CharWidth")
								{
									adobeFontMetricsBuilder.SetCharacterWidth(AdobeFontMetricsParser.ReadDouble(bytes, stringBuilder), AdobeFontMetricsParser.ReadDouble(bytes, stringBuilder));
								}
							}
						}
						else if (text == "VVector")
						{
							adobeFontMetricsBuilder.SetVVector(AdobeFontMetricsParser.ReadDouble(bytes, stringBuilder), AdobeFontMetricsParser.ReadDouble(bytes, stringBuilder));
						}
					}
					else if (num != 2555355134U)
					{
						if (num == 2556651472U)
						{
							if (text == "ItalicAngle")
							{
								adobeFontMetricsBuilder.ItalicAngle = AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
							}
						}
					}
					else if (text == "Comment")
					{
						adobeFontMetricsBuilder.Comments.Add(AdobeFontMetricsParser.ReadLine(bytes, stringBuilder));
					}
				}
				else if (num <= 3358974499U)
				{
					if (num <= 2890663381U)
					{
						if (num != 2570065113U)
						{
							if (num == 2890663381U)
							{
								if (text == "IsFixedPitch")
								{
									adobeFontMetricsBuilder.IsFixedPitch = AdobeFontMetricsParser.ReadBool(bytes, stringBuilder);
								}
							}
						}
						else if (text == "Weight")
						{
							adobeFontMetricsBuilder.Weight = AdobeFontMetricsParser.ReadLine(bytes, stringBuilder);
						}
					}
					else if (num != 3075589058U)
					{
						if (num == 3358974499U)
						{
							if (text == "FontBBox")
							{
								adobeFontMetricsBuilder.SetBoundingBox(AdobeFontMetricsParser.ReadDouble(bytes, stringBuilder), AdobeFontMetricsParser.ReadDouble(bytes, stringBuilder), AdobeFontMetricsParser.ReadDouble(bytes, stringBuilder), AdobeFontMetricsParser.ReadDouble(bytes, stringBuilder));
							}
						}
					}
					else if (text == "MappingScheme")
					{
						adobeFontMetricsBuilder.MappingScheme = (int)AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
					}
				}
				else if (num <= 3760500662U)
				{
					if (num != 3385073588U)
					{
						if (num == 3760500662U)
						{
							if (text == "Descender")
							{
								adobeFontMetricsBuilder.Descender = AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
							}
						}
					}
					else if (text == "CapHeight")
					{
						adobeFontMetricsBuilder.CapHeight = AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
					}
				}
				else if (num != 3858027415U)
				{
					if (num == 4235129593U)
					{
						if (text == "Characters")
						{
							adobeFontMetricsBuilder.Characters = (int)AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
						}
					}
				}
				else if (text == "UnderlineThickness")
				{
					adobeFontMetricsBuilder.UnderlineThickness = AdobeFontMetricsParser.ReadDecimal(bytes, stringBuilder);
				}
			}
			return adobeFontMetricsBuilder.Build();
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001EB2C File Offset: 0x0001CD2C
		private static decimal ReadDecimal(IInputBytes input, StringBuilder stringBuilder)
		{
			return decimal.Parse(AdobeFontMetricsParser.ReadString(input, stringBuilder), CultureInfo.InvariantCulture);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001EB3F File Offset: 0x0001CD3F
		private static double ReadDouble(IInputBytes input, StringBuilder stringBuilder)
		{
			return (double)AdobeFontMetricsParser.ReadDecimal(input, stringBuilder);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001EB50 File Offset: 0x0001CD50
		private static bool ReadBool(IInputBytes input, StringBuilder stringBuilder)
		{
			string text = AdobeFontMetricsParser.ReadString(input, stringBuilder);
			if (text == "true")
			{
				return true;
			}
			if (!(text == "false"))
			{
				throw new InvalidFontFormatException("The AFM should have contained a boolean but instead contained: " + text + ".");
			}
			return false;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001EB9C File Offset: 0x0001CD9C
		private static string ReadString(IInputBytes input, StringBuilder stringBuilder)
		{
			stringBuilder.Clear();
			if (input.IsAtEnd())
			{
				return "EndFontMetrics";
			}
			while (ReadHelper.IsWhitespace(input.CurrentByte) && input.MoveNext())
			{
			}
			stringBuilder.Append((char)input.CurrentByte);
			while (input.MoveNext() && !ReadHelper.IsWhitespace(input.CurrentByte))
			{
				stringBuilder.Append((char)input.CurrentByte);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001EC0C File Offset: 0x0001CE0C
		private static string ReadLine(IInputBytes input, StringBuilder stringBuilder)
		{
			stringBuilder.Clear();
			while (ReadHelper.IsWhitespace(input.CurrentByte) && input.MoveNext())
			{
			}
			stringBuilder.Append((char)input.CurrentByte);
			while (input.MoveNext() && !ReadHelper.IsEndOfLine(input.CurrentByte))
			{
				stringBuilder.Append((char)input.CurrentByte);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001EC6C File Offset: 0x0001CE6C
		private static AdobeFontMetricsIndividualCharacterMetric ReadCharacterMetric(IInputBytes bytes, StringBuilder stringBuilder)
		{
			string[] array = AdobeFontMetricsParser.ReadLine(bytes, stringBuilder).Split(AdobeFontMetricsParser.IndividualCharmetricsSplit, 1);
			AdobeFontMetricsIndividualCharacterMetricBuilder adobeFontMetricsIndividualCharacterMetricBuilder = new AdobeFontMetricsIndividualCharacterMetricBuilder();
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string[] array3 = array2[i].Split(AdobeFontMetricsParser.CharmetricsKeySplit, 1);
				string text = array3[0];
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 2398637586U)
				{
					if (num <= 1524842110U)
					{
						if (num != 922207114U)
						{
							if (num != 938984733U)
							{
								if (num != 1524842110U)
								{
									goto IL_4AD;
								}
								if (!(text == "W0X"))
								{
									goto IL_4AD;
								}
								adobeFontMetricsIndividualCharacterMetricBuilder.WidthXDirection0 = double.Parse(array3[1], CultureInfo.InvariantCulture);
							}
							else
							{
								if (!(text == "WY"))
								{
									goto IL_4AD;
								}
								adobeFontMetricsIndividualCharacterMetricBuilder.WidthY = double.Parse(array3[1], CultureInfo.InvariantCulture);
							}
						}
						else
						{
							if (!(text == "WX"))
							{
								goto IL_4AD;
							}
							adobeFontMetricsIndividualCharacterMetricBuilder.WidthX = double.Parse(array3[1], CultureInfo.InvariantCulture);
						}
					}
					else if (num <= 2063232301U)
					{
						if (num != 1541619729U)
						{
							if (num != 2063232301U)
							{
								goto IL_4AD;
							}
							if (!(text == "VV"))
							{
								goto IL_4AD;
							}
							adobeFontMetricsIndividualCharacterMetricBuilder.VVector = new AdobeFontMetricsVector(double.Parse(array3[1], CultureInfo.InvariantCulture), double.Parse(array3[2], CultureInfo.InvariantCulture));
						}
						else
						{
							if (!(text == "W0Y"))
							{
								goto IL_4AD;
							}
							adobeFontMetricsIndividualCharacterMetricBuilder.WidthYDirection0 = double.Parse(array3[1], CultureInfo.InvariantCulture);
						}
					}
					else if (num != 2128224206U)
					{
						if (num != 2398637586U)
						{
							goto IL_4AD;
						}
						if (!(text == "W0"))
						{
							goto IL_4AD;
						}
						adobeFontMetricsIndividualCharacterMetricBuilder.WidthXDirection0 = double.Parse(array3[1], CultureInfo.InvariantCulture);
						adobeFontMetricsIndividualCharacterMetricBuilder.WidthYDirection0 = double.Parse(array3[2], CultureInfo.InvariantCulture);
					}
					else
					{
						if (!(text == "CH"))
						{
							goto IL_4AD;
						}
						int characterCode = int.Parse(array3[1], 515, CultureInfo.InvariantCulture);
						adobeFontMetricsIndividualCharacterMetricBuilder.CharacterCode = characterCode;
					}
				}
				else if (num <= 3322673650U)
				{
					if (num <= 2682644916U)
					{
						if (num != 2415415205U)
						{
							if (num != 2682644916U)
							{
								goto IL_4AD;
							}
							if (!(text == "W1Y"))
							{
								goto IL_4AD;
							}
							adobeFontMetricsIndividualCharacterMetricBuilder.WidthYDirection1 = double.Parse(array3[1], CultureInfo.InvariantCulture);
						}
						else
						{
							if (!(text == "W1"))
							{
								goto IL_4AD;
							}
							adobeFontMetricsIndividualCharacterMetricBuilder.WidthXDirection1 = double.Parse(array3[1], CultureInfo.InvariantCulture);
							adobeFontMetricsIndividualCharacterMetricBuilder.WidthYDirection1 = double.Parse(array3[2], CultureInfo.InvariantCulture);
						}
					}
					else if (num != 2699422535U)
					{
						if (num != 3322673650U)
						{
							goto IL_4AD;
						}
						if (!(text == "C"))
						{
							goto IL_4AD;
						}
						int characterCode2 = int.Parse(array3[1], CultureInfo.InvariantCulture);
						adobeFontMetricsIndividualCharacterMetricBuilder.CharacterCode = characterCode2;
					}
					else
					{
						if (!(text == "W1X"))
						{
							goto IL_4AD;
						}
						adobeFontMetricsIndividualCharacterMetricBuilder.WidthXDirection1 = double.Parse(array3[1], CultureInfo.InvariantCulture);
					}
				}
				else
				{
					if (num <= 3373006507U)
					{
						if (num != 3339451269U)
						{
							if (num != 3373006507U)
							{
								goto IL_4AD;
							}
							if (!(text == "L"))
							{
								goto IL_4AD;
							}
							adobeFontMetricsIndividualCharacterMetricBuilder.Ligature = new AdobeFontMetricsLigature(array3[1], array3[2]);
							goto IL_4C5;
						}
						else if (!(text == "B"))
						{
							goto IL_4AD;
						}
					}
					else if (num != 3406561745U)
					{
						if (num != 3524005078U)
						{
							goto IL_4AD;
						}
						if (!(text == "W"))
						{
							goto IL_4AD;
						}
						adobeFontMetricsIndividualCharacterMetricBuilder.WidthX = double.Parse(array3[1], CultureInfo.InvariantCulture);
						adobeFontMetricsIndividualCharacterMetricBuilder.WidthY = double.Parse(array3[2], CultureInfo.InvariantCulture);
						goto IL_4C5;
					}
					else
					{
						if (!(text == "N"))
						{
							goto IL_4AD;
						}
						object locker = AdobeFontMetricsParser.Locker;
						lock (locker)
						{
							string text2 = array3[1];
							string text3;
							if (!AdobeFontMetricsParser.CharacterNames.TryGetValue(text2, ref text3))
							{
								text3 = text2;
								AdobeFontMetricsParser.CharacterNames[text2] = text3;
							}
							adobeFontMetricsIndividualCharacterMetricBuilder.Name = text3;
							goto IL_4C5;
						}
					}
					adobeFontMetricsIndividualCharacterMetricBuilder.BoundingBox = new PdfRectangle(double.Parse(array3[1], CultureInfo.InvariantCulture), double.Parse(array3[2], CultureInfo.InvariantCulture), double.Parse(array3[3], CultureInfo.InvariantCulture), double.Parse(array3[4], CultureInfo.InvariantCulture));
				}
				IL_4C5:
				i++;
				continue;
				IL_4AD:
				throw new InvalidFontFormatException("Unknown CharMetrics command '" + array3[0] + "'.");
			}
			return adobeFontMetricsIndividualCharacterMetricBuilder.Build();
		}

		// Token: 0x040003D1 RID: 977
		private static readonly object Locker = new object();

		// Token: 0x040003D2 RID: 978
		private static readonly Dictionary<string, string> CharacterNames = new Dictionary<string, string>();

		// Token: 0x040003D3 RID: 979
		public const string Comment = "Comment";

		// Token: 0x040003D4 RID: 980
		public const string StartFontMetrics = "StartFontMetrics";

		// Token: 0x040003D5 RID: 981
		public const string EndFontMetrics = "EndFontMetrics";

		// Token: 0x040003D6 RID: 982
		public const string FontName = "FontName";

		// Token: 0x040003D7 RID: 983
		public const string FullName = "FullName";

		// Token: 0x040003D8 RID: 984
		public const string FamilyName = "FamilyName";

		// Token: 0x040003D9 RID: 985
		public const string Weight = "Weight";

		// Token: 0x040003DA RID: 986
		public const string FontBbox = "FontBBox";

		// Token: 0x040003DB RID: 987
		public const string Version = "Version";

		// Token: 0x040003DC RID: 988
		public const string Notice = "Notice";

		// Token: 0x040003DD RID: 989
		public const string EncodingScheme = "EncodingScheme";

		// Token: 0x040003DE RID: 990
		public const string MappingScheme = "MappingScheme";

		// Token: 0x040003DF RID: 991
		public const string EscChar = "EscChar";

		// Token: 0x040003E0 RID: 992
		public const string CharacterSet = "CharacterSet";

		// Token: 0x040003E1 RID: 993
		public const string Characters = "Characters";

		// Token: 0x040003E2 RID: 994
		public const string IsBaseFont = "IsBaseFont";

		// Token: 0x040003E3 RID: 995
		public const string VVector = "VVector";

		// Token: 0x040003E4 RID: 996
		public const string IsFixedV = "IsFixedV";

		// Token: 0x040003E5 RID: 997
		public const string CapHeight = "CapHeight";

		// Token: 0x040003E6 RID: 998
		public const string XHeight = "XHeight";

		// Token: 0x040003E7 RID: 999
		public const string Ascender = "Ascender";

		// Token: 0x040003E8 RID: 1000
		public const string Descender = "Descender";

		// Token: 0x040003E9 RID: 1001
		public const string UnderlinePosition = "UnderlinePosition";

		// Token: 0x040003EA RID: 1002
		public const string UnderlineThickness = "UnderlineThickness";

		// Token: 0x040003EB RID: 1003
		public const string ItalicAngle = "ItalicAngle";

		// Token: 0x040003EC RID: 1004
		public const string CharWidth = "CharWidth";

		// Token: 0x040003ED RID: 1005
		public const string IsFixedPitch = "IsFixedPitch";

		// Token: 0x040003EE RID: 1006
		public const string StartCharMetrics = "StartCharMetrics";

		// Token: 0x040003EF RID: 1007
		public const string EndCharMetrics = "EndCharMetrics";

		// Token: 0x040003F0 RID: 1008
		public const string CharmetricsC = "C";

		// Token: 0x040003F1 RID: 1009
		public const string CharmetricsCh = "CH";

		// Token: 0x040003F2 RID: 1010
		public const string CharmetricsWx = "WX";

		// Token: 0x040003F3 RID: 1011
		public const string CharmetricsW0X = "W0X";

		// Token: 0x040003F4 RID: 1012
		public const string CharmetricsW1X = "W1X";

		// Token: 0x040003F5 RID: 1013
		public const string CharmetricsWy = "WY";

		// Token: 0x040003F6 RID: 1014
		public const string CharmetricsW0Y = "W0Y";

		// Token: 0x040003F7 RID: 1015
		public const string CharmetricsW1Y = "W1Y";

		// Token: 0x040003F8 RID: 1016
		public const string CharmetricsW = "W";

		// Token: 0x040003F9 RID: 1017
		public const string CharmetricsW0 = "W0";

		// Token: 0x040003FA RID: 1018
		public const string CharmetricsW1 = "W1";

		// Token: 0x040003FB RID: 1019
		public const string CharmetricsVv = "VV";

		// Token: 0x040003FC RID: 1020
		public const string CharmetricsN = "N";

		// Token: 0x040003FD RID: 1021
		public const string CharmetricsB = "B";

		// Token: 0x040003FE RID: 1022
		public const string CharmetricsL = "L";

		// Token: 0x040003FF RID: 1023
		public const string StdHw = "StdHW";

		// Token: 0x04000400 RID: 1024
		public const string StdVw = "StdVW";

		// Token: 0x04000401 RID: 1025
		public const string StartTrackKern = "StartTrackKern";

		// Token: 0x04000402 RID: 1026
		public const string EndTrackKern = "EndTrackKern";

		// Token: 0x04000403 RID: 1027
		public const string StartKernData = "StartKernData";

		// Token: 0x04000404 RID: 1028
		public const string EndKernData = "EndKernData";

		// Token: 0x04000405 RID: 1029
		public const string StartKernPairs = "StartKernPairs";

		// Token: 0x04000406 RID: 1030
		public const string EndKernPairs = "EndKernPairs";

		// Token: 0x04000407 RID: 1031
		public const string StartKernPairs0 = "StartKernPairs0";

		// Token: 0x04000408 RID: 1032
		public const string StartKernPairs1 = "StartKernPairs1";

		// Token: 0x04000409 RID: 1033
		public const string StartComposites = "StartComposites";

		// Token: 0x0400040A RID: 1034
		public const string EndComposites = "EndComposites";

		// Token: 0x0400040B RID: 1035
		public const string Cc = "CC";

		// Token: 0x0400040C RID: 1036
		public const string Pcc = "PCC";

		// Token: 0x0400040D RID: 1037
		public const string KernPairKp = "KP";

		// Token: 0x0400040E RID: 1038
		public const string KernPairKph = "KPH";

		// Token: 0x0400040F RID: 1039
		public const string KernPairKpx = "KPX";

		// Token: 0x04000410 RID: 1040
		public const string KernPairKpy = "KPY";

		// Token: 0x04000411 RID: 1041
		private static readonly char[] IndividualCharmetricsSplit = new char[]
		{
			';'
		};

		// Token: 0x04000412 RID: 1042
		private static readonly char[] CharmetricsKeySplit = new char[]
		{
			' '
		};
	}
}
