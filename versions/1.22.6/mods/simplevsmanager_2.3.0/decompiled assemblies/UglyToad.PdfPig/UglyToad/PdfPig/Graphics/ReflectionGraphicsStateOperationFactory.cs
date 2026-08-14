using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Graphics.Operations.ClippingPaths;
using UglyToad.PdfPig.Graphics.Operations.Compatibility;
using UglyToad.PdfPig.Graphics.Operations.General;
using UglyToad.PdfPig.Graphics.Operations.InlineImages;
using UglyToad.PdfPig.Graphics.Operations.MarkedContent;
using UglyToad.PdfPig.Graphics.Operations.PathConstruction;
using UglyToad.PdfPig.Graphics.Operations.PathPainting;
using UglyToad.PdfPig.Graphics.Operations.SpecialGraphicsState;
using UglyToad.PdfPig.Graphics.Operations.TextObjects;
using UglyToad.PdfPig.Graphics.Operations.TextPositioning;
using UglyToad.PdfPig.Graphics.Operations.TextShowing;
using UglyToad.PdfPig.Graphics.Operations.TextState;
using UglyToad.PdfPig.Tokenization;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x0200010E RID: 270
	internal class ReflectionGraphicsStateOperationFactory : IGraphicsStateOperationFactory
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x00020494 File Offset: 0x0001E694
		public ReflectionGraphicsStateOperationFactory()
		{
			Type[] types = Assembly.GetAssembly(typeof(ReflectionGraphicsStateOperationFactory)).GetTypes();
			Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
			foreach (Type type in types)
			{
				if (!type.IsInterface && typeof(IGraphicsStateOperation).IsAssignableFrom(type))
				{
					FieldInfo field = type.GetField("Symbol");
					if (field == null)
					{
						throw new InvalidOperationException("An operation type was defined without the public const Symbol being declared. Type was: " + type.FullName);
					}
					string text = field.GetValue(null).ToString();
					dictionary[text] = type;
				}
			}
			this.operations = dictionary;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00020538 File Offset: 0x0001E738
		private static decimal[] TokensToDecimalArray(IReadOnlyList<IToken> tokens, bool exceptLast = false)
		{
			List<decimal> list = ReflectionGraphicsStateOperationFactory.DecimalListPool.Borrow();
			for (int i = 0; i < tokens.Count - (exceptLast ? 1 : 0); i++)
			{
				IToken token = tokens[i];
				ArrayToken arrayToken = token as ArrayToken;
				if (arrayToken != null)
				{
					for (int j = 0; j < arrayToken.Length; j++)
					{
						NumericToken numericToken = arrayToken[j] as NumericToken;
						if (numericToken == null)
						{
							IEnumerable<decimal> enumerable = list.ToArray();
							ReflectionGraphicsStateOperationFactory.DecimalListPool.Return(list);
							return Enumerable.ToArray<decimal>(enumerable);
						}
						list.Add(numericToken.Data);
					}
				}
				NumericToken numericToken2 = token as NumericToken;
				if (numericToken2 == null)
				{
					IEnumerable<decimal> enumerable2 = list.ToArray();
					ReflectionGraphicsStateOperationFactory.DecimalListPool.Return(list);
					return Enumerable.ToArray<decimal>(enumerable2);
				}
				list.Add(numericToken2.Data);
			}
			decimal[] result = list.ToArray();
			ReflectionGraphicsStateOperationFactory.DecimalListPool.Return(list);
			return result;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00020610 File Offset: 0x0001E810
		private static int OperandToInt(IToken token)
		{
			NumericToken numericToken = token as NumericToken;
			if (numericToken == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid operand token encountered when expecting numeric: ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(token);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return numericToken.Int;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00020664 File Offset: 0x0001E864
		private static decimal OperandToDecimal(IToken token)
		{
			NumericToken numericToken = token as NumericToken;
			if (numericToken == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid operand token encountered when expecting numeric: ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(token);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return numericToken.Data;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000206B8 File Offset: 0x0001E8B8
		public IGraphicsStateOperation Create(OperatorToken op, IReadOnlyList<IToken> operands)
		{
			string data = op.Data;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(data);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num <= 1683261492U)
			{
				if (num <= 989170495U)
				{
					if (num <= 620062877U)
					{
						if (num <= 519397163U)
						{
							if (num <= 115160189U)
							{
								if (num != 37869084U)
								{
									if (num == 115160189U)
									{
										if (data == "B*")
										{
											return FillPathEvenOddRuleAndStroke.Value;
										}
									}
								}
								else if (data == "EMC")
								{
									return EndMarkedContent.Value;
								}
							}
							else if (num != 485841925U)
							{
								if (num == 519397163U)
								{
									if (data == "TJ")
									{
										if (operands.Count == 0)
										{
											throw new InvalidOperationException("Cannot have 0 parameters for a TJ operator.");
										}
										if (operands.Count == 1)
										{
											ArrayToken arrayToken = operands[0] as ArrayToken;
											if (arrayToken != null)
											{
												return new ShowTextsWithPositioning(arrayToken.Data);
											}
										}
										return new ShowTextsWithPositioning(Enumerable.ToArray<IToken>(operands));
									}
								}
							}
							else if (data == "TD")
							{
								return new MoveToNextLineWithOffsetSetLeading(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]));
							}
						}
						else if (num <= 571247302U)
						{
							if (num != 522354377U)
							{
								if (num == 571247302U)
								{
									if (data == "'")
									{
										if (operands.Count != 1)
										{
											defaultInterpolatedStringHandler..ctor(79, 1);
											defaultInterpolatedStringHandler.AppendLiteral("Attempted to create a move to next line and show text operation with ");
											defaultInterpolatedStringHandler.AppendFormatted<int>(operands.Count);
											defaultInterpolatedStringHandler.AppendLiteral(" operands.");
											throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
										}
										StringToken stringToken = operands[0] as StringToken;
										if (stringToken != null)
										{
											return new MoveToNextLineShowText(stringToken.Data);
										}
										HexToken hexToken = operands[0] as HexToken;
										if (hexToken != null)
										{
											return new MoveToNextLineShowText(Enumerable.ToArray<byte>(hexToken.Bytes));
										}
										string text = "Tried to create a move to next line and show text operation with operand type: ";
										IToken token = operands[0];
										throw new InvalidOperationException(text + (((token != null) ? token.GetType().Name : null) ?? "null"));
									}
								}
							}
							else if (data == "f*")
							{
								return FillPathEvenOddRule.Value;
							}
						}
						else if (num != 601711140U)
						{
							if (num == 620062877U)
							{
								if (data == "TL")
								{
									return new SetTextLeading(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
								}
							}
						}
						else if (data == "BI")
						{
							return BeginInlineImage.Value;
						}
					}
					else if (num <= 718021640U)
					{
						if (num <= 684613497U)
						{
							if (num != 650911164U)
							{
								if (num == 684613497U)
								{
									if (data == "DP")
									{
										NameToken name = (NameToken)operands[0];
										DictionaryToken dictionaryToken = operands[1] as DictionaryToken;
										if (dictionaryToken != null)
										{
											return new DesignateMarkedContentPointWithProperties(name, dictionaryToken);
										}
										NameToken nameToken = operands[1] as NameToken;
										if (nameToken != null)
										{
											return new DesignateMarkedContentPointWithProperties(name, nameToken);
										}
										string text2 = string.Join(", ", Enumerable.Select<IToken, string>(operands, (IToken x) => x.ToString()));
										throw new PdfDocumentFormatException("Attempted to set a marked-content point with invalid parameters: [" + text2 + "]");
									}
								}
							}
							else if (data == "ET")
							{
								return EndText.Value;
							}
						}
						else if (num != 685599235U)
						{
							if (num == 718021640U)
							{
								if (data == "EX")
								{
									return EndCompatibilitySection.Value;
								}
							}
						}
						else if (data == "BT")
						{
							return BeginText.Value;
						}
					}
					else if (num <= 886930663U)
					{
						if (num != 853522520U)
						{
							if (num == 886930663U)
							{
								if (data == "BX")
								{
									return BeginCompatibilitySection.Value;
								}
							}
						}
						else if (data == "MP")
						{
							return new DesignateMarkedContentPoint((NameToken)operands[0]);
						}
					}
					else if (num != 905282400U)
					{
						if (num != 911343923U)
						{
							if (num == 989170495U)
							{
								if (data == "Tf")
								{
									return new SetFontAndSize((NameToken)operands[0], ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]));
								}
							}
						}
						else if (data == "sc")
						{
							return new SetNonStrokeColor(ReflectionGraphicsStateOperationFactory.TokensToDecimalArray(operands, false));
						}
					}
					else if (data == "Tc")
					{
						return new SetCharacterSpacing(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
					}
				}
				else if (num <= 1240834780U)
				{
					if (num <= 1140169066U)
					{
						if (num <= 1056280971U)
						{
							if (num != 1022725733U)
							{
								if (num == 1056280971U)
								{
									if (data == "Tj")
									{
										if (operands.Count != 1)
										{
											defaultInterpolatedStringHandler..ctor(57, 1);
											defaultInterpolatedStringHandler.AppendLiteral("Attempted to create a show text operation with ");
											defaultInterpolatedStringHandler.AppendFormatted<int>(operands.Count);
											defaultInterpolatedStringHandler.AppendLiteral(" operands.");
											throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
										}
										StringToken stringToken2 = operands[0] as StringToken;
										if (stringToken2 != null)
										{
											return new ShowText(stringToken2.Data);
										}
										HexToken hexToken2 = operands[0] as HexToken;
										if (hexToken2 != null)
										{
											return new ShowText(Enumerable.ToArray<byte>(hexToken2.Bytes));
										}
										string text3 = "Tried to create a show text operation with operand type: ";
										IToken token2 = operands[0];
										throw new InvalidOperationException(text3 + (((token2 != null) ? token2.GetType().Name : null) ?? "null"));
									}
								}
							}
							else if (data == "Td")
							{
								return new MoveToNextLineWithOffset(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]));
							}
						}
						else if (num != 1062342494U)
						{
							if (num == 1140169066U)
							{
								if (data == "Tm")
								{
									return new SetTextMatrix(ReflectionGraphicsStateOperationFactory.TokensToDecimalArray(operands, false));
								}
							}
						}
						else if (data == "sh")
						{
							return new PaintShading((NameToken)operands[0]);
						}
					}
					else if (num <= 1177122803U)
					{
						if (num != 1173724304U)
						{
							if (num == 1177122803U)
							{
								if (data == "cs")
								{
									return new SetNonStrokeColorSpace((NameToken)operands[0]);
								}
							}
						}
						else if (data == "Ts")
						{
							return new SetTextRise(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
						}
					}
					else if (num != 1190501923U)
					{
						if (num == 1240834780U)
						{
							if (data == "Tw")
							{
								return new SetWordSpacing(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
							}
						}
					}
					else if (data == "Tr")
					{
						return new SetTextRenderingMode(ReflectionGraphicsStateOperationFactory.OperandToInt(operands[0]));
					}
				}
				else if (num <= 1481930064U)
				{
					if (num <= 1324722875U)
					{
						if (num != 1310755375U)
						{
							if (num == 1324722875U)
							{
								if (data == "Tz")
								{
									return new SetHorizontalScaling(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
								}
							}
						}
						else if (data == "gs")
						{
							return new SetGraphicsStateParametersFromDictionary((NameToken)operands[0]);
						}
					}
					else if (num != 1443004851U)
					{
						if (num == 1481930064U)
						{
							if (data == "re")
							{
								if (operands.Count == 0)
								{
									return null;
								}
								return new AppendRectangle(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[2]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[3]));
							}
						}
					}
					else if (data == "SC")
					{
						return new SetStrokeColor(ReflectionGraphicsStateOperationFactory.TokensToDecimalArray(operands, false));
					}
				}
				else if (num <= 1640937780U)
				{
					if (num != 1515485302U)
					{
						if (num == 1640937780U)
						{
							if (data == "Do")
							{
								return new InvokeNamedXObject((NameToken)operands[0]);
							}
						}
					}
					else if (data == "rg")
					{
						return new SetNonStrokeColorDeviceRgb(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[2]));
					}
				}
				else if (num != 1660068167U)
				{
					if (num != 1680451373U)
					{
						if (num == 1683261492U)
						{
							if (data == "ri")
							{
								return new SetColorRenderingIntent((NameToken)operands[0]);
							}
						}
					}
					else if (data == "cm")
					{
						return new ModifyCurrentTransformationMatrix(ReflectionGraphicsStateOperationFactory.TokensToDecimalArray(operands, false));
					}
				}
				else if (data == "SCN")
				{
					NameToken nameToken2 = operands[operands.Count - 1] as NameToken;
					if (nameToken2 != null)
					{
						return new SetStrokeColorAdvanced(Enumerable.ToList<decimal>(Enumerable.Select<IToken, decimal>(Enumerable.Take<IToken>(operands, operands.Count - 1), (IToken x) => ((NumericToken)x).Data)), nameToken2);
					}
					if (Enumerable.All<IToken>(operands, (IToken x) => x is NumericToken))
					{
						return new SetStrokeColorAdvanced(Enumerable.ToList<decimal>(Enumerable.Select<IToken, decimal>(operands, (IToken x) => ((NumericToken)x).Data)));
					}
					string text4 = string.Join(", ", Enumerable.Select<IToken, string>(operands, (IToken x) => x.ToString()));
					throw new PdfDocumentFormatException("Attempted to set a stroke color space (SCN) with invalid arguments: [" + text4 + "]");
				}
			}
			else if (num <= 3759678823U)
			{
				if (num <= 3272340793U)
				{
					if (num <= 2267815133U)
					{
						if (num <= 2047146230U)
						{
							if (num != 1708783731U)
							{
								if (num == 2047146230U)
								{
									if (data == "RG")
									{
										return new SetStrokeColorDeviceRgb(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[2]));
									}
								}
							}
							else if (data == "CS")
							{
								return new SetStrokeColorSpace((NameToken)operands[0]);
							}
						}
						else if (num != 2130048587U)
						{
							if (num == 2267815133U)
							{
								if (data == "b*")
								{
									return CloseFillPathEvenOddRuleAndStroke.Value;
								}
							}
						}
						else if (data == "T*")
						{
							return MoveToNextLine.Value;
						}
					}
					else if (num <= 3026571952U)
					{
						if (num != 2767745204U)
						{
							if (num == 3026571952U)
							{
								if (data == "BDC")
								{
									NameToken name2 = (NameToken)operands[0];
									DictionaryToken dictionaryToken2 = operands[1] as DictionaryToken;
									if (dictionaryToken2 != null)
									{
										return new BeginMarkedContentWithProperties(name2, dictionaryToken2);
									}
									NameToken nameToken3 = operands[1] as NameToken;
									if (nameToken3 != null)
									{
										return new BeginMarkedContentWithProperties(name2, nameToken3);
									}
									string text5 = string.Join(", ", Enumerable.Select<IToken, string>(operands, (IToken x) => x.ToString()));
									throw new PdfDocumentFormatException("Attempted to set a marked-content sequence with invalid parameters: [" + text5 + "]");
								}
							}
						}
						else if (data == "W*")
						{
							return ModifyClippingByEvenOddIntersect.Value;
						}
					}
					else if (num != 3255563174U)
					{
						if (num == 3272340793U)
						{
							if (data == "F")
							{
								return FillPathNonZeroWindingCompatibility.Value;
							}
						}
					}
					else if (data == "G")
					{
						return new SetStrokeColorDeviceGray(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
					}
				}
				else if (num <= 3456894602U)
				{
					if (num <= 3339451269U)
					{
						if (num != 3296146689U)
						{
							if (num == 3339451269U)
							{
								if (data == "B")
								{
									return FillPathNonZeroWindingAndStroke.Value;
								}
							}
						}
						else if (data == "BMC")
						{
							return new BeginMarkedContent((NameToken)operands[0]);
						}
					}
					else if (num != 3356228888U)
					{
						if (num == 3456894602U)
						{
							if (data == "K")
							{
								return new SetStrokeColorDeviceCmyk(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[2]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[3]));
							}
						}
					}
					else if (data == "M")
					{
						return new SetMiterLimit(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
					}
				}
				else if (num <= 3524005078U)
				{
					if (num != 3473672221U)
					{
						if (num == 3524005078U)
						{
							if (data == "W")
							{
								return ModifyClippingByNonZeroWindingIntersect.Value;
							}
						}
					}
					else if (data == "J")
					{
						return new SetLineCap(ReflectionGraphicsStateOperationFactory.OperandToInt(operands[0]));
					}
				}
				else if (num != 3557560316U)
				{
					if (num != 3591115554U)
					{
						if (num == 3759678823U)
						{
							if (data == "scn")
							{
								NameToken nameToken4 = operands[operands.Count - 1] as NameToken;
								if (nameToken4 != null)
								{
									return new SetNonStrokeColorAdvanced(Enumerable.ToList<decimal>(Enumerable.Select<IToken, decimal>(Enumerable.Take<IToken>(operands, operands.Count - 1), (IToken x) => ((NumericToken)x).Data)), nameToken4);
								}
								if (Enumerable.All<IToken>(operands, (IToken x) => x is NumericToken))
								{
									return new SetNonStrokeColorAdvanced(Enumerable.ToList<decimal>(Enumerable.Select<IToken, decimal>(operands, (IToken x) => ((NumericToken)x).Data)));
								}
								string text6 = string.Join(", ", Enumerable.Select<IToken, string>(operands, (IToken x) => x.ToString()));
								throw new PdfDocumentFormatException("Attempted to set a non-stroke color space (scn) with invalid arguments: [" + text6 + "]");
							}
						}
					}
					else if (data == "S")
					{
						return StrokePath.Value;
					}
				}
				else if (data == "Q")
				{
					return Pop.Value;
				}
			}
			else if (num <= 3943445553U)
			{
				if (num <= 3859557458U)
				{
					if (num <= 3792446982U)
					{
						if (num != 3775669363U)
						{
							if (num == 3792446982U)
							{
								if (data == "g")
								{
									return new SetNonStrokeColorDeviceGray(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
								}
							}
						}
						else if (data == "d")
						{
							return new SetLineDashPattern(ReflectionGraphicsStateOperationFactory.TokensToDecimalArray(operands, true), ReflectionGraphicsStateOperationFactory.OperandToInt(operands[operands.Count - 1]));
						}
					}
					else if (num != 3809224601U)
					{
						if (num == 3859557458U)
						{
							if (data == "c")
							{
								if (operands.Count == 0)
								{
									return null;
								}
								return new AppendDualControlPointBezierCurve(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[2]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[3]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[4]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[5]));
							}
						}
					}
					else if (data == "f")
					{
						return FillPathNonZeroWinding.Value;
					}
				}
				else if (num <= 3893112696U)
				{
					if (num != 3876335077U)
					{
						if (num == 3893112696U)
						{
							if (data == "m")
							{
								return new BeginNewSubpath(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]));
							}
						}
					}
					else if (data == "b")
					{
						return CloseFillPathNonZeroWindingAndStroke.Value;
					}
				}
				else if (num != 3909890315U)
				{
					if (num == 3943445553U)
					{
						if (data == "n")
						{
							return EndPath.Value;
						}
					}
				}
				else if (data == "l")
				{
					return new AppendStraightLineSegment(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]));
				}
			}
			else if (num <= 4010556029U)
			{
				if (num <= 3977000791U)
				{
					if (num != 3960223172U)
					{
						if (num == 3977000791U)
						{
							if (data == "h")
							{
								return CloseSubpath.Value;
							}
						}
					}
					else if (data == "i")
					{
						return new SetFlatnessTolerance(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
					}
				}
				else if (num != 3993778410U)
				{
					if (num == 4010556029U)
					{
						if (data == "j")
						{
							return new SetLineJoin(ReflectionGraphicsStateOperationFactory.OperandToInt(operands[0]));
						}
					}
				}
				else if (data == "k")
				{
					return new SetNonStrokeColorDeviceCmyk(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[2]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[3]));
				}
			}
			else if (num <= 4077666505U)
			{
				if (num != 4060888886U)
				{
					if (num == 4077666505U)
					{
						if (data == "v")
						{
							if (operands.Count == 0)
							{
								return null;
							}
							return new AppendStartControlPointBezierCurve(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[2]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[3]));
						}
					}
				}
				else if (data == "w")
				{
					return new SetLineWidth(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]));
				}
			}
			else if (num != 4094444124U)
			{
				if (num != 4127999362U)
				{
					if (num == 4228665076U)
					{
						if (data == "y")
						{
							if (operands.Count == 0)
							{
								return null;
							}
							return new AppendEndControlPointBezierCurve(ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[0]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[1]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[2]), ReflectionGraphicsStateOperationFactory.OperandToDecimal(operands[3]));
						}
					}
				}
				else if (data == "s")
				{
					return CloseAndStrokePath.Value;
				}
			}
			else if (data == "q")
			{
				return Push.Value;
			}
			Type type;
			if (!this.operations.TryGetValue(op.Data, ref type))
			{
				return null;
			}
			ConstructorInfo[] constructors = type.GetConstructors(52);
			if (constructors.Length == 0)
			{
				throw new InvalidOperationException("No constructors to invoke were found for operation type: " + type.FullName);
			}
			ConstructorInfo constructorInfo = constructors[0];
			if (constructorInfo.IsPrivate)
			{
				return (IGraphicsStateOperation)type.GetField("Value").GetValue(null);
			}
			ParameterInfo[] parameters = constructorInfo.GetParameters();
			int i = 0;
			List<object> list = new List<object>();
			foreach (ParameterInfo parameterInfo in parameters)
			{
				if (i >= operands.Count)
				{
					defaultInterpolatedStringHandler..ctor(54, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Fewer operands ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(operands.Count);
					defaultInterpolatedStringHandler.AppendLiteral(" found than required (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i + 1);
					defaultInterpolatedStringHandler.AppendLiteral(") for operator: ");
					defaultInterpolatedStringHandler.AppendFormatted(op.Data);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (parameterInfo.ParameterType == typeof(decimal))
				{
					NumericToken numericToken = operands[i] as NumericToken;
					if (numericToken == null)
					{
						defaultInterpolatedStringHandler..ctor(63, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Expected a decimal parameter for operation type ");
						defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
						defaultInterpolatedStringHandler.AppendLiteral(". Instead got: ");
						defaultInterpolatedStringHandler.AppendFormatted<IToken>(operands[i]);
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					list.Add(numericToken.Data);
					i++;
				}
				else if (parameterInfo.ParameterType == typeof(int))
				{
					NumericToken numericToken2 = operands[i] as NumericToken;
					if (numericToken2 == null)
					{
						defaultInterpolatedStringHandler..ctor(64, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Expected an integer parameter for operation type ");
						defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
						defaultInterpolatedStringHandler.AppendLiteral(". Instead got: ");
						defaultInterpolatedStringHandler.AppendFormatted<IToken>(operands[i]);
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					list.Add(numericToken2.Int);
					i++;
				}
				else if (parameterInfo.ParameterType == typeof(decimal[]))
				{
					ArrayToken arrayToken2 = operands[i] as ArrayToken;
					if (arrayToken2 != null)
					{
						list.Add(Enumerable.ToArray<decimal>(Enumerable.Select<NumericToken, decimal>(Enumerable.OfType<NumericToken>(arrayToken2.Data), (NumericToken x) => x.Data)));
						i++;
					}
					else
					{
						List<decimal> list2 = new List<decimal>();
						while (i < operands.Count)
						{
							NumericToken numericToken3 = operands[i] as NumericToken;
							if (numericToken3 == null)
							{
								break;
							}
							list2.Add(numericToken3.Data);
							i++;
						}
						list.Add(list2.ToArray());
					}
				}
				else if (parameterInfo.ParameterType == typeof(NameToken))
				{
					NameToken nameToken5 = operands[i] as NameToken;
					if (nameToken5 != null)
					{
						list.Add(nameToken5);
					}
					else
					{
						StringToken stringToken3 = operands[i] as StringToken;
						if (stringToken3 == null)
						{
							defaultInterpolatedStringHandler..ctor(69, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Expected a decimal array parameter for operation type ");
							defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
							defaultInterpolatedStringHandler.AppendLiteral(". Instead got: ");
							defaultInterpolatedStringHandler.AppendFormatted<IToken>(operands[i]);
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						list.Add(NameToken.Create(stringToken3.Data));
					}
					i++;
				}
				else
				{
					if (!(parameterInfo.ParameterType == typeof(string)))
					{
						defaultInterpolatedStringHandler..ctor(48, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported parameter type ");
						defaultInterpolatedStringHandler.AppendFormatted(parameterInfo.ParameterType.FullName);
						defaultInterpolatedStringHandler.AppendLiteral(" for operation type ");
						defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new NotImplementedException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					StringToken stringToken4 = operands[i] as StringToken;
					if (stringToken4 != null)
					{
						list.Add(stringToken4.Data);
					}
					else
					{
						HexToken hexToken3 = operands[i] as HexToken;
						if (hexToken3 == null)
						{
							defaultInterpolatedStringHandler..ctor(62, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Expected a string parameter for operation type ");
							defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
							defaultInterpolatedStringHandler.AppendLiteral(". Instead got: ");
							defaultInterpolatedStringHandler.AppendFormatted<IToken>(operands[i]);
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						list.Add(hexToken3.Data);
					}
					i++;
				}
			}
			return (IGraphicsStateOperation)constructorInfo.Invoke(list.ToArray());
		}

		// Token: 0x040002ED RID: 749
		private static readonly ListPool<decimal> DecimalListPool = new ListPool<decimal>(10);

		// Token: 0x040002EE RID: 750
		private readonly IReadOnlyDictionary<string, Type> operations;
	}
}
