using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ModernWpf.Controls
{
	// Token: 0x0200007B RID: 123
	internal class NumberBoxParser
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x000156E8 File Offset: 0x000138E8
		private static IList<MathToken> GetTokens(string input, INumberBoxNumberFormatter numberParser)
		{
			List<MathToken> list = new List<MathToken>();
			bool flag = true;
			while (input.Length > 0)
			{
				char c = input.get_Chars(0);
				if (c != ' ')
				{
					if (flag)
					{
						if (c == '(')
						{
							list.Add(new MathToken(MathTokenType.Parenthesis, c));
						}
						else
						{
							ValueTuple<double, int> nextNumber = NumberBoxParser.GetNextNumber(input, numberParser);
							double item = nextNumber.Item1;
							int item2 = nextNumber.Item2;
							if (item2 <= 0)
							{
								return NumberBoxParser.s_emptyTokens;
							}
							list.Add(new MathToken(MathTokenType.Numeric, item));
							input = input.SafeSubstring(item2 - 1);
							flag = false;
						}
					}
					else if ("+-*/^".IndexOf(c) >= 0)
					{
						list.Add(new MathToken(MathTokenType.Operator, c));
						flag = true;
					}
					else
					{
						if (c != ')')
						{
							return NumberBoxParser.s_emptyTokens;
						}
						list.Add(new MathToken(MathTokenType.Parenthesis, c));
					}
				}
				input = input.SafeSubstring(1);
			}
			return list;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000157B8 File Offset: 0x000139B8
		private static ValueTuple<double, int> GetNextNumber(string input, INumberBoxNumberFormatter numberParser)
		{
			Match match = new Regex("^-?([^-+/*\\(\\)\\^\\s]+)").Match(input);
			if (match.Success)
			{
				int length = match.Value.Length;
				double? num = numberParser.ParseDouble(input.Substring(0, length));
				if (num != null)
				{
					return new ValueTuple<double, int>(num.Value, length);
				}
			}
			return new ValueTuple<double, int>(double.NaN, 0);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00015820 File Offset: 0x00013A20
		private static int GetPrecedenceValue(char c)
		{
			int result = 0;
			if (c == '*' || c == '/')
			{
				result = 1;
			}
			else if (c == '^')
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00015848 File Offset: 0x00013A48
		private static IList<MathToken> ConvertInfixToPostfix(IList<MathToken> infixTokens)
		{
			List<MathToken> list = new List<MathToken>();
			Stack<MathToken> stack = new Stack<MathToken>();
			using (IEnumerator<MathToken> enumerator = infixTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MathToken mathToken = enumerator.Current;
					if (mathToken.Type == MathTokenType.Numeric)
					{
						list.Add(mathToken);
					}
					else if (mathToken.Type == MathTokenType.Operator)
					{
						while (stack.Count > 0)
						{
							MathToken mathToken2 = stack.Peek();
							if (mathToken2.Type == MathTokenType.Parenthesis || NumberBoxParser.GetPrecedenceValue(mathToken2.Char) < NumberBoxParser.GetPrecedenceValue(mathToken.Char))
							{
								break;
							}
							list.Add(stack.Pop());
						}
						stack.Push(mathToken);
					}
					else if (mathToken.Type == MathTokenType.Parenthesis)
					{
						if (mathToken.Char == '(')
						{
							stack.Push(mathToken);
						}
						else
						{
							while (stack.Count > 0 && stack.Peek().Char != '(')
							{
								list.Add(stack.Pop());
							}
							if (stack.Count == 0)
							{
								return NumberBoxParser.s_emptyTokens;
							}
							stack.Pop();
						}
					}
				}
				goto IL_116;
			}
			IL_F6:
			if (stack.Peek().Type == MathTokenType.Parenthesis)
			{
				return NumberBoxParser.s_emptyTokens;
			}
			list.Add(stack.Pop());
			IL_116:
			if (stack.Count <= 0)
			{
				return list;
			}
			goto IL_F6;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00015988 File Offset: 0x00013B88
		private static double? ComputePostfixExpression(IList<MathToken> tokens)
		{
			Stack<double> stack = new Stack<double>();
			foreach (MathToken mathToken in tokens)
			{
				if (mathToken.Type == MathTokenType.Operator)
				{
					if (stack.Count < 2)
					{
						double? result = default(double?);
						return result;
					}
					double num = stack.Pop();
					double num2 = stack.Pop();
					char @char = mathToken.Char;
					double num3;
					switch (@char)
					{
					case '*':
						num3 = num * num2;
						break;
					case '+':
						num3 = num + num2;
						break;
					case ',':
					case '.':
						goto IL_CA;
					case '-':
						num3 = num2 - num;
						break;
					case '/':
						if (num == 0.0)
						{
							return new double?(double.NaN);
						}
						num3 = num2 / num;
						break;
					default:
						if (@char != '^')
						{
							goto IL_CA;
						}
						num3 = Math.Pow(num2, num);
						break;
					}
					stack.Push(num3);
					continue;
					IL_CA:
					return default(double?);
				}
				else if (mathToken.Type == MathTokenType.Numeric)
				{
					stack.Push(mathToken.Value);
				}
			}
			if (stack.Count != 1)
			{
				return default(double?);
			}
			return new double?(stack.Peek());
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00015AD4 File Offset: 0x00013CD4
		public static double? Compute(string expr, INumberBoxNumberFormatter numberParser)
		{
			IList<MathToken> tokens = NumberBoxParser.GetTokens(expr, numberParser);
			if (tokens.Count > 0)
			{
				IList<MathToken> list = NumberBoxParser.ConvertInfixToPostfix(tokens);
				if (list.Count > 0)
				{
					return NumberBoxParser.ComputePostfixExpression(list);
				}
			}
			return default(double?);
		}

		// Token: 0x040002C9 RID: 713
		private const string c_numberBoxOperators = "+-*/^";

		// Token: 0x040002CA RID: 714
		private static readonly MathToken[] s_emptyTokens = new MathToken[0];
	}
}
