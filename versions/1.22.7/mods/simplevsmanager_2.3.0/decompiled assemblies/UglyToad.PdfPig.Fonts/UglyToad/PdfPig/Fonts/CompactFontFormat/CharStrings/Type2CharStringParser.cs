using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.CharStrings
{
	// Token: 0x020000A0 RID: 160
	internal static class Type2CharStringParser
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x0001A817 File Offset: 0x00018A17
		public static LazyType2Command GetCommand(Type2CharStrings.CommandSequence.CommandIdentifier identifier)
		{
			if (identifier.IsMultiByteCommand)
			{
				return Type2CharStringParser.TwoByteCommandStore[identifier.CommandId];
			}
			return Type2CharStringParser.SingleByteCommandStore[identifier.CommandId];
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001A848 File Offset: 0x00018A48
		public static Type2CharStrings Parse(IReadOnlyList<IReadOnlyList<byte>> charStringBytes, CompactFontFormatSubroutinesSelector subroutinesSelector, ICompactFontFormatCharset charset)
		{
			if (charStringBytes == null)
			{
				throw new ArgumentNullException("charStringBytes");
			}
			if (subroutinesSelector == null)
			{
				throw new ArgumentNullException("subroutinesSelector");
			}
			Dictionary<string, Type2CharStrings.CommandSequence> dictionary = new Dictionary<string, Type2CharStrings.CommandSequence>();
			for (int i = 0; i < charStringBytes.Count; i++)
			{
				IEnumerable<byte> enumerable = charStringBytes[i];
				string nameByGlyphId = charset.GetNameByGlyphId(i);
				ValueTuple<CompactFontFormatIndex, CompactFontFormatIndex> subroutines = subroutinesSelector.GetSubroutines(i);
				CompactFontFormatIndex item = subroutines.Item1;
				CompactFontFormatIndex item2 = subroutines.Item2;
				Type2CharStrings.CommandSequence commandSequence = Type2CharStringParser.ParseSingle(Enumerable.ToList<byte>(enumerable), item2, item);
				dictionary[nameByGlyphId] = commandSequence;
			}
			return new Type2CharStrings(dictionary);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001A8CC File Offset: 0x00018ACC
		private static Type2CharStrings.CommandSequence ParseSingle(List<byte> bytes, CompactFontFormatIndex localSubroutines, CompactFontFormatIndex globalSubroutines)
		{
			List<float> list = new List<float>();
			List<Type2CharStrings.CommandSequence.CommandIdentifier> list2 = new List<Type2CharStrings.CommandSequence.CommandIdentifier>();
			for (int i = 0; i < bytes.Count; i++)
			{
				byte b = bytes[i];
				if (b <= 31 && b != 28)
				{
					Type2CharStrings.CommandSequence.CommandIdentifier? command = Type2CharStringParser.GetCommand(b, bytes, list, list2, localSubroutines, globalSubroutines, ref i);
					if (command != null)
					{
						list2.Add(command.Value);
					}
				}
				else
				{
					float num = Type2CharStringParser.InterpretNumber(b, bytes, ref i);
					list.Add(num);
				}
			}
			return new Type2CharStrings.CommandSequence(list, list2);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001A94C File Offset: 0x00018B4C
		private static float InterpretNumber(byte b, IReadOnlyList<byte> bytes, ref int i)
		{
			int num;
			if (b == 28)
			{
				num = i + 1;
				i = num;
				byte b2 = (byte)(bytes[num] << 8);
				num = i + 1;
				i = num;
				return (float)((short)(b2 | bytes[num]));
			}
			if (b >= 32 && b <= 246)
			{
				return (float)(b - 139);
			}
			if (b >= 247 && b <= 250)
			{
				num = i + 1;
				i = num;
				byte b3 = bytes[num];
				return (float)((int)(b - 247) * 256 + (int)b3 + 108);
			}
			if (b >= 251 && b <= 254)
			{
				num = i + 1;
				i = num;
				byte b4 = bytes[num];
				return (float)(-((int)(b - 251) * 256) - (int)b4 - 108);
			}
			num = i + 1;
			i = num;
			float num2 = (float)((short)(bytes[num] << 8));
			num = i + 1;
			i = num;
			float num3 = num2 + (float)bytes[num];
			num = i + 1;
			i = num;
			int num4 = (int)bytes[num] << 8;
			num = i + 1;
			i = num;
			int num5 = num4 + (int)bytes[num];
			return num3 + (float)num5 / 65535f;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001AA50 File Offset: 0x00018C50
		private static Type2CharStrings.CommandSequence.CommandIdentifier? GetCommand(byte b, List<byte> bytes, List<float> precedingValues, List<Type2CharStrings.CommandSequence.CommandIdentifier> precedingCommands, CompactFontFormatIndex localSubroutines, CompactFontFormatIndex globalSubroutines, ref int i)
		{
			if (b == 12)
			{
				int num = i + 1;
				i = num;
				byte b2 = bytes[num];
				if (Type2CharStringParser.TwoByteCommandStore.ContainsKey(b2))
				{
					return new Type2CharStrings.CommandSequence.CommandIdentifier?(new Type2CharStrings.CommandSequence.CommandIdentifier(precedingValues.Count, true, b2));
				}
				return new Type2CharStrings.CommandSequence.CommandIdentifier?(new Type2CharStrings.CommandSequence.CommandIdentifier(precedingValues.Count, false, byte.MaxValue));
			}
			else
			{
				if (b == 10 || b == 29)
				{
					bool flag = b == 10;
					int num2 = (int)precedingValues[precedingValues.Count - 1];
					int num3 = Type2BuildCharContext.CountToBias(flag ? localSubroutines.Count : globalSubroutines.Count);
					int index = num2 + num3;
					IReadOnlyList<byte> readOnlyList = flag ? localSubroutines[index] : globalSubroutines[index];
					bytes.RemoveRange(i - 1, 2);
					bytes.InsertRange(i - 1, readOnlyList);
					precedingValues.RemoveAt(precedingValues.Count - 1);
					i -= 2;
					return default(Type2CharStrings.CommandSequence.CommandIdentifier?);
				}
				if (b == 19 || b == 20)
				{
					int num4 = Type2CharStringParser.CalculatePrecedingHintBytes(precedingValues, precedingCommands);
					i += num4;
				}
				if (!Type2CharStringParser.SingleByteCommandStore.ContainsKey(b))
				{
					return new Type2CharStrings.CommandSequence.CommandIdentifier?(new Type2CharStrings.CommandSequence.CommandIdentifier(precedingValues.Count, false, byte.MaxValue));
				}
				if (b == 11)
				{
					return default(Type2CharStrings.CommandSequence.CommandIdentifier?);
				}
				return new Type2CharStrings.CommandSequence.CommandIdentifier?(new Type2CharStrings.CommandSequence.CommandIdentifier(precedingValues.Count, false, b));
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001ABA0 File Offset: 0x00018DA0
		private static int CalculatePrecedingHintBytes(List<float> precedingValues, List<Type2CharStrings.CommandSequence.CommandIdentifier> precedingCommands)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			for (int i = -1; i < precedingValues.Count; i++)
			{
				if (i >= 0)
				{
					num2++;
				}
				for (int j = 0; j < precedingCommands.Count; j++)
				{
					Type2CharStrings.CommandSequence.CommandIdentifier commandIdentifier = precedingCommands[j];
					if (commandIdentifier.CommandIndex == i + 1)
					{
						if (!commandIdentifier.IsMultiByteCommand && (commandIdentifier.CommandId == 19 || commandIdentifier.CommandId == 20) && !flag)
						{
							flag = true;
							num += Type2CharStringParser.<CalculatePrecedingHintBytes>g__SafeStemCount|14_0(num2);
						}
						else if (!commandIdentifier.IsMultiByteCommand && !Type2CharStringParser.HintingCommandBytes.Contains(commandIdentifier.CommandId))
						{
							num2 = 0;
						}
						else if (commandIdentifier.IsMultiByteCommand && commandIdentifier.CommandId > 35)
						{
							num2 = 0;
						}
						else
						{
							num += Type2CharStringParser.<CalculatePrecedingHintBytes>g__SafeStemCount|14_0(num2);
							num2 = 0;
						}
						if (flag)
						{
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
			int num3 = num;
			if (num2 > 0 && !flag)
			{
				num3 += Type2CharStringParser.<CalculatePrecedingHintBytes>g__SafeStemCount|14_0(num2);
			}
			return (int)Math.Ceiling((double)num3 / 8.0);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001ACA8 File Offset: 0x00018EA8
		// Note: this type is marked as 'beforefieldinit'.
		static Type2CharStringParser()
		{
			HashSet<byte> hashSet = new HashSet<byte>();
			hashSet.Add(1);
			hashSet.Add(3);
			hashSet.Add(18);
			hashSet.Add(23);
			Type2CharStringParser.HintingCommandBytes = hashSet;
			Dictionary<byte, LazyType2Command> dictionary = new Dictionary<byte, LazyType2Command>();
			dictionary.Add(1, new LazyType2Command("hstem", 2, delegate(Type2BuildCharContext ctx)
			{
				int num = ctx.Stack.Length / 2;
				ValueTuple<double, double>[] array = new ValueTuple<double, double>[num];
				double num2 = ctx.Stack.PopBottom();
				double num3 = num2 + ctx.Stack.PopBottom();
				array[0] = new ValueTuple<double, double>(num2, num3);
				double num4 = num3;
				for (int i = 1; i < num; i++)
				{
					double num5 = ctx.Stack.PopBottom();
					double num6 = ctx.Stack.PopBottom();
					array[i] = new ValueTuple<double, double>(num4 + num5, num4 + num5 + num6);
					num4 = num4 + num5 + num6;
				}
				ctx.AddHorizontalStemHints(array);
				ctx.Stack.Clear();
			}));
			dictionary.Add(3, new LazyType2Command("vstem", 2, delegate(Type2BuildCharContext ctx)
			{
				int num = ctx.Stack.Length / 2;
				ValueTuple<double, double>[] array = new ValueTuple<double, double>[num];
				double num2 = ctx.Stack.PopBottom();
				double num3 = num2 + ctx.Stack.PopBottom();
				array[0] = new ValueTuple<double, double>(num2, num3);
				double num4 = num3;
				for (int i = 1; i < num; i++)
				{
					double num5 = ctx.Stack.PopBottom();
					double num6 = ctx.Stack.PopBottom();
					array[i] = new ValueTuple<double, double>(num4 + num5, num4 + num5 + num6);
					num4 = num4 + num5 + num6;
				}
				ctx.AddVerticalStemHints(array);
				ctx.Stack.Clear();
			}));
			dictionary.Add(4, new LazyType2Command("vmoveto", 1, delegate(Type2BuildCharContext ctx)
			{
				double dy = ctx.Stack.PopBottom();
				ctx.AddVerticallMoveTo(dy);
				ctx.Stack.Clear();
			}));
			dictionary.Add(5, new LazyType2Command("rlineto", 2, delegate(Type2BuildCharContext ctx)
			{
				int num = ctx.Stack.Length / 2;
				for (int i = 0; i < num; i++)
				{
					double dx = ctx.Stack.PopBottom();
					double dy = ctx.Stack.PopBottom();
					ctx.AddRelativeLine(dx, dy);
				}
				ctx.Stack.Clear();
			}));
			dictionary.Add(6, new LazyType2Command("hlineto", 1, delegate(Type2BuildCharContext ctx)
			{
				bool flag = ctx.Stack.Length % 2 != 0;
				int num = ctx.Stack.Length - (flag ? 1 : 0);
				if (flag)
				{
					double dx = ctx.Stack.PopBottom();
					ctx.AddRelativeHorizontalLine(dx);
					for (int i = 0; i < num; i += 2)
					{
						ctx.AddRelativeVerticalLine(ctx.Stack.PopBottom());
						ctx.AddRelativeHorizontalLine(ctx.Stack.PopBottom());
					}
				}
				else
				{
					for (int j = 0; j < num; j += 2)
					{
						ctx.AddRelativeHorizontalLine(ctx.Stack.PopBottom());
						ctx.AddRelativeVerticalLine(ctx.Stack.PopBottom());
					}
				}
				ctx.Stack.Clear();
			}));
			dictionary.Add(7, new LazyType2Command("vlineto", 1, delegate(Type2BuildCharContext ctx)
			{
				bool flag = ctx.Stack.Length % 2 != 0;
				int num = ctx.Stack.Length - (flag ? 1 : 0);
				if (flag)
				{
					double dy = ctx.Stack.PopBottom();
					ctx.AddRelativeVerticalLine(dy);
					for (int i = 0; i < num; i += 2)
					{
						ctx.AddRelativeHorizontalLine(ctx.Stack.PopBottom());
						ctx.AddRelativeVerticalLine(ctx.Stack.PopBottom());
					}
				}
				else
				{
					for (int j = 0; j < num; j += 2)
					{
						ctx.AddRelativeVerticalLine(ctx.Stack.PopBottom());
						ctx.AddRelativeHorizontalLine(ctx.Stack.PopBottom());
					}
				}
				ctx.Stack.Clear();
			}));
			dictionary.Add(8, new LazyType2Command("rrcurveto", 6, delegate(Type2BuildCharContext ctx)
			{
				int num = ctx.Stack.Length / 6;
				for (int i = 0; i < num; i++)
				{
					ctx.AddRelativeBezierCurve(ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom());
				}
				ctx.Stack.Clear();
			}));
			dictionary.Add(10, new LazyType2Command("callsubr", 1, delegate(Type2BuildCharContext ctx)
			{
			}));
			dictionary.Add(11, new LazyType2Command("return", 0, delegate(Type2BuildCharContext ctx)
			{
			}));
			dictionary.Add(14, new LazyType2Command("endchar", 0, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Clear();
			}));
			dictionary.Add(18, new LazyType2Command("hstemhm", 2, delegate(Type2BuildCharContext ctx)
			{
				int num = ctx.Stack.Length / 2;
				ValueTuple<double, double>[] array = new ValueTuple<double, double>[num];
				double num2 = ctx.Stack.PopBottom();
				double num3 = num2 + ctx.Stack.PopBottom();
				array[0] = new ValueTuple<double, double>(num2, num3);
				double num4 = num3;
				for (int i = 1; i < num; i++)
				{
					double num5 = ctx.Stack.PopBottom();
					double num6 = ctx.Stack.PopBottom();
					array[i] = new ValueTuple<double, double>(num4 + num5, num4 + num5 + num6);
					num4 = num4 + num5 + num6;
				}
				ctx.AddHorizontalStemHints(array);
				ctx.Stack.Clear();
			}));
			dictionary.Add(19, new LazyType2Command("hintmask", 0, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Clear();
			}));
			dictionary.Add(20, new LazyType2Command("cntrmask", 0, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Clear();
			}));
			dictionary.Add(21, new LazyType2Command("rmoveto", 2, delegate(Type2BuildCharContext ctx)
			{
				double dx = ctx.Stack.PopBottom();
				double dy = ctx.Stack.PopBottom();
				ctx.AddRelativeMoveTo(dx, dy);
				ctx.Stack.Clear();
			}));
			dictionary.Add(22, new LazyType2Command("hmoveto", 1, delegate(Type2BuildCharContext ctx)
			{
				double dx = ctx.Stack.PopBottom();
				ctx.AddHorizontalMoveTo(dx);
				ctx.Stack.Clear();
			}));
			dictionary.Add(23, new LazyType2Command("vstemhm", 2, delegate(Type2BuildCharContext ctx)
			{
				int num = ctx.Stack.Length / 2;
				ValueTuple<double, double>[] array = new ValueTuple<double, double>[num];
				double num2 = ctx.Stack.PopBottom();
				double num3 = num2 + ctx.Stack.PopBottom();
				array[0] = new ValueTuple<double, double>(num2, num3);
				double num4 = num3;
				for (int i = 1; i < num; i++)
				{
					double num5 = ctx.Stack.PopBottom();
					double num6 = ctx.Stack.PopBottom();
					array[i] = new ValueTuple<double, double>(num4 + num5, num4 + num5 + num6);
					num4 = num4 + num5 + num6;
				}
				ctx.AddVerticalStemHints(array);
				ctx.Stack.Clear();
			}));
			dictionary.Add(24, new LazyType2Command("rcurveline", 8, delegate(Type2BuildCharContext ctx)
			{
				int num = (ctx.Stack.Length - 2) / 6;
				for (int i = 0; i < num; i++)
				{
					ctx.AddRelativeBezierCurve(ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom());
				}
				ctx.AddRelativeLine(ctx.Stack.PopBottom(), ctx.Stack.PopBottom());
				ctx.Stack.Clear();
			}));
			dictionary.Add(25, new LazyType2Command("rlinecurve", 8, delegate(Type2BuildCharContext ctx)
			{
				int num = (ctx.Stack.Length - 6) / 2;
				for (int i = 0; i < num; i++)
				{
					ctx.AddRelativeLine(ctx.Stack.PopBottom(), ctx.Stack.PopBottom());
				}
				ctx.AddRelativeBezierCurve(ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom());
				ctx.Stack.Clear();
			}));
			dictionary.Add(26, new LazyType2Command("vvcurveto", 4, delegate(Type2BuildCharContext ctx)
			{
				bool flag = ctx.Stack.Length % 4 != 0;
				int num = ctx.Stack.Length / 4;
				for (int i = 0; i < num; i++)
				{
					double dx = 0.0;
					if (i == 0 && flag)
					{
						dx = ctx.Stack.PopBottom();
					}
					double dy = ctx.Stack.PopBottom();
					double dx2 = ctx.Stack.PopBottom();
					double dy2 = ctx.Stack.PopBottom();
					double dy3 = ctx.Stack.PopBottom();
					ctx.AddRelativeBezierCurve(dx, dy, dx2, dy2, 0.0, dy3);
				}
				ctx.Stack.Clear();
			}));
			dictionary.Add(27, new LazyType2Command("hhcurveto", 4, delegate(Type2BuildCharContext ctx)
			{
				if (ctx.Stack.Length % 4 != 0)
				{
					double dy = ctx.Stack.PopBottom();
					double dx = ctx.Stack.PopBottom();
					double dx2 = ctx.Stack.PopBottom();
					double dy2 = ctx.Stack.PopBottom();
					double dx3 = ctx.Stack.PopBottom();
					ctx.AddRelativeBezierCurve(dx, dy, dx2, dy2, dx3, 0.0);
				}
				int num = ctx.Stack.Length / 4;
				for (int i = 0; i < num; i++)
				{
					double dx4 = ctx.Stack.PopBottom();
					double dx5 = ctx.Stack.PopBottom();
					double dy3 = ctx.Stack.PopBottom();
					double dx6 = ctx.Stack.PopBottom();
					ctx.AddRelativeBezierCurve(dx4, 0.0, dx5, dy3, dx6, 0.0);
				}
				ctx.Stack.Clear();
			}));
			dictionary.Add(29, new LazyType2Command("callgsubr", 1, delegate(Type2BuildCharContext ctx)
			{
			}));
			dictionary.Add(30, new LazyType2Command("vhcurveto", 4, delegate(Type2BuildCharContext ctx)
			{
				int num = ctx.Stack.Length % 8;
				if (num <= 1)
				{
					int num2 = (ctx.Stack.Length - num) / 8;
					for (int i = 0; i < num2; i++)
					{
						double dy = ctx.Stack.PopBottom();
						double dx = ctx.Stack.PopBottom();
						double dy2 = ctx.Stack.PopBottom();
						double dx2 = ctx.Stack.PopBottom();
						ctx.AddRelativeBezierCurve(0.0, dy, dx, dy2, dx2, 0.0);
						double dx3 = ctx.Stack.PopBottom();
						double dx4 = ctx.Stack.PopBottom();
						double dy3 = ctx.Stack.PopBottom();
						double dy4 = ctx.Stack.PopBottom();
						double dx5 = 0.0;
						if (i == num2 - 1 && num == 1)
						{
							dx5 = ctx.Stack.PopBottom();
						}
						ctx.AddRelativeBezierCurve(dx3, 0.0, dx4, dy3, dx5, dy4);
					}
				}
				else
				{
					if (num != 4 && num != 5)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(48, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unexpected number of arguments for vhcurve to: ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(ctx.Stack.Length);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int num3 = (ctx.Stack.Length - num) / 8;
					double dy5 = ctx.Stack.PopBottom();
					double dx6 = ctx.Stack.PopBottom();
					double dy6 = ctx.Stack.PopBottom();
					double dx7 = ctx.Stack.PopBottom();
					double dy7 = (ctx.Stack.Length == 1) ? ctx.Stack.PopBottom() : 0.0;
					ctx.AddRelativeBezierCurve(0.0, dy5, dx6, dy6, dx7, dy7);
					for (int j = 0; j < num3; j++)
					{
						double dx8 = ctx.Stack.PopBottom();
						double dx9 = ctx.Stack.PopBottom();
						double dy8 = ctx.Stack.PopBottom();
						double dy9 = ctx.Stack.PopBottom();
						ctx.AddRelativeBezierCurve(dx8, 0.0, dx9, dy8, 0.0, dy9);
						double dy10 = ctx.Stack.PopBottom();
						double dx10 = ctx.Stack.PopBottom();
						double dy11 = ctx.Stack.PopBottom();
						double dx11 = ctx.Stack.PopBottom();
						double dy12 = 0.0;
						if (j == num3 - 1 && num == 5)
						{
							dy12 = ctx.Stack.PopBottom();
						}
						ctx.AddRelativeBezierCurve(0.0, dy10, dx10, dy11, dx11, dy12);
					}
				}
				ctx.Stack.Clear();
			}));
			dictionary.Add(31, new LazyType2Command("hvcurveto", 4, delegate(Type2BuildCharContext ctx)
			{
				int num = ctx.Stack.Length % 8;
				if (num <= 1)
				{
					int num2 = (ctx.Stack.Length - num) / 8;
					for (int i = 0; i < num2; i++)
					{
						double dx = ctx.Stack.PopBottom();
						double dx2 = ctx.Stack.PopBottom();
						double dy = ctx.Stack.PopBottom();
						double dy2 = ctx.Stack.PopBottom();
						ctx.AddRelativeBezierCurve(dx, 0.0, dx2, dy, 0.0, dy2);
						double dy3 = ctx.Stack.PopBottom();
						double dx3 = ctx.Stack.PopBottom();
						double dy4 = ctx.Stack.PopBottom();
						double dx4 = ctx.Stack.PopBottom();
						double dy5 = 0.0;
						if (i == num2 - 1 && num == 1)
						{
							dy5 = ctx.Stack.PopBottom();
						}
						ctx.AddRelativeBezierCurve(0.0, dy3, dx3, dy4, dx4, dy5);
					}
				}
				else
				{
					if (num != 4 && num != 5)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(48, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unexpected number of arguments for hvcurve to: ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(ctx.Stack.Length);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int num3 = (ctx.Stack.Length - num) / 8;
					double dx5 = ctx.Stack.PopBottom();
					double dx6 = ctx.Stack.PopBottom();
					double dy6 = ctx.Stack.PopBottom();
					double dy7 = ctx.Stack.PopBottom();
					double dx7 = (ctx.Stack.Length == 1) ? ctx.Stack.PopBottom() : 0.0;
					ctx.AddRelativeBezierCurve(dx5, 0.0, dx6, dy6, dx7, dy7);
					for (int j = 0; j < num3; j++)
					{
						double dy8 = ctx.Stack.PopBottom();
						double dx8 = ctx.Stack.PopBottom();
						double dy9 = ctx.Stack.PopBottom();
						double dx9 = ctx.Stack.PopBottom();
						ctx.AddRelativeBezierCurve(0.0, dy8, dx8, dy9, dx9, 0.0);
						double dx10 = ctx.Stack.PopBottom();
						double dx11 = ctx.Stack.PopBottom();
						double dy10 = ctx.Stack.PopBottom();
						double dy11 = ctx.Stack.PopBottom();
						double dx12 = 0.0;
						if (j == num3 - 1 && num == 5)
						{
							dx12 = ctx.Stack.PopBottom();
						}
						ctx.AddRelativeBezierCurve(dx10, 0.0, dx11, dy10, dx12, dy11);
					}
				}
				ctx.Stack.Clear();
			}));
			dictionary.Add(byte.MaxValue, new LazyType2Command("unknown", -1, delegate(Type2BuildCharContext x)
			{
			}));
			Type2CharStringParser.SingleByteCommandStore = dictionary;
			Dictionary<byte, LazyType2Command> dictionary2 = new Dictionary<byte, LazyType2Command>();
			dictionary2.Add(3, new LazyType2Command("and", 2, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push((double)((ctx.Stack.PopTop() != 0.0 && ctx.Stack.PopTop() != 0.0) ? 1 : 0));
			}));
			dictionary2.Add(4, new LazyType2Command("or", 2, delegate(Type2BuildCharContext ctx)
			{
				double num = ctx.Stack.PopTop();
				double num2 = ctx.Stack.PopTop();
				ctx.Stack.Push((double)((num != 0.0 || num2 != 0.0) ? 1 : 0));
			}));
			dictionary2.Add(5, new LazyType2Command("not", 1, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push((double)((ctx.Stack.PopTop() == 0.0) ? 1 : 0));
			}));
			dictionary2.Add(9, new LazyType2Command("abs", 1, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push(Math.Abs(ctx.Stack.PopTop()));
			}));
			dictionary2.Add(10, new LazyType2Command("add", 2, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push(ctx.Stack.PopTop() + ctx.Stack.PopTop());
			}));
			dictionary2.Add(11, new LazyType2Command("sub", 2, delegate(Type2BuildCharContext ctx)
			{
				double num = ctx.Stack.PopTop();
				double num2 = ctx.Stack.PopTop();
				ctx.Stack.Push(num2 - num);
			}));
			dictionary2.Add(12, new LazyType2Command("div", 2, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push(ctx.Stack.PopTop() / ctx.Stack.PopTop());
			}));
			dictionary2.Add(14, new LazyType2Command("neg", 1, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push(-1.0 * Math.Abs(ctx.Stack.PopTop()));
			}));
			dictionary2.Add(15, new LazyType2Command("eq", 2, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push((double)((ctx.Stack.PopTop() == ctx.Stack.PopTop()) ? 1 : 0));
			}));
			dictionary2.Add(18, new LazyType2Command("drop", 1, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.PopTop();
			}));
			dictionary2.Add(20, new LazyType2Command("put", 2, delegate(Type2BuildCharContext ctx)
			{
				ctx.AddToTransientArray(ctx.Stack.PopTop(), (int)ctx.Stack.PopTop());
			}));
			dictionary2.Add(21, new LazyType2Command("get", 1, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push(ctx.GetFromTransientArray((int)ctx.Stack.PopTop()));
			}));
			dictionary2.Add(22, new LazyType2Command("ifelse", 4, delegate(Type2BuildCharContext x)
			{
			}));
			dictionary2.Add(23, new LazyType2Command("random", 0, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push(0.5);
			}));
			dictionary2.Add(24, new LazyType2Command("mul", 2, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push(ctx.Stack.PopTop() * ctx.Stack.PopTop());
			}));
			dictionary2.Add(26, new LazyType2Command("sqrt", 1, delegate(Type2BuildCharContext ctx)
			{
				ctx.Stack.Push(Math.Sqrt(ctx.Stack.PopTop()));
			}));
			dictionary2.Add(27, new LazyType2Command("dup", 1, delegate(Type2BuildCharContext ctx)
			{
				double value = ctx.Stack.PopTop();
				ctx.Stack.Push(value);
				ctx.Stack.Push(value);
			}));
			dictionary2.Add(28, new LazyType2Command("exch", 2, delegate(Type2BuildCharContext ctx)
			{
				double value = ctx.Stack.PopTop();
				double value2 = ctx.Stack.PopTop();
				ctx.Stack.Push(value);
				ctx.Stack.Push(value2);
			}));
			dictionary2.Add(29, new LazyType2Command("index", 2, delegate(Type2BuildCharContext ctx)
			{
				double num = ctx.Stack.PopTop();
				double value = ctx.Stack.CopyElementAt((int)num);
				ctx.Stack.Push(value);
			}));
			dictionary2.Add(30, new LazyType2Command("roll", 3, delegate(Type2BuildCharContext ctx)
			{
			}));
			dictionary2.Add(34, new LazyType2Command("hflex", 7, delegate(Type2BuildCharContext ctx)
			{
				double dx = ctx.Stack.PopBottom();
				double dx2 = ctx.Stack.PopBottom();
				double num = ctx.Stack.PopBottom();
				double dx3 = ctx.Stack.PopBottom();
				double dx4 = ctx.Stack.PopBottom();
				double dx5 = ctx.Stack.PopBottom();
				double dx6 = ctx.Stack.PopBottom();
				ctx.AddRelativeBezierCurve(dx, 0.0, dx2, num, dx3, 0.0);
				ctx.AddRelativeBezierCurve(dx4, 0.0, dx5, -num, dx6, 0.0);
				ctx.Stack.Clear();
			}));
			dictionary2.Add(35, new LazyType2Command("flex", 13, delegate(Type2BuildCharContext ctx)
			{
				ctx.AddRelativeBezierCurve(ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom());
				ctx.AddRelativeBezierCurve(ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom(), ctx.Stack.PopBottom());
				ctx.Stack.PopBottom();
				ctx.Stack.Clear();
			}));
			dictionary2.Add(36, new LazyType2Command("hflex1", 9, delegate(Type2BuildCharContext ctx)
			{
				double dx = ctx.Stack.PopBottom();
				double num = ctx.Stack.PopBottom();
				double dx2 = ctx.Stack.PopBottom();
				double num2 = ctx.Stack.PopBottom();
				double dx3 = ctx.Stack.PopBottom();
				double dx4 = ctx.Stack.PopBottom();
				double dx5 = ctx.Stack.PopBottom();
				double num3 = ctx.Stack.PopBottom();
				double dx6 = ctx.Stack.PopBottom();
				ctx.AddRelativeBezierCurve(dx, num, dx2, num2, dx3, 0.0);
				ctx.AddRelativeBezierCurve(dx4, 0.0, dx5, num3, dx6, -num3 - num2 - num);
				ctx.Stack.Clear();
			}));
			dictionary2.Add(37, new LazyType2Command("flex1", 11, delegate(Type2BuildCharContext ctx)
			{
				double num = ctx.Stack.PopBottom();
				double num2 = ctx.Stack.PopBottom();
				double num3 = ctx.Stack.PopBottom();
				double num4 = ctx.Stack.PopBottom();
				double num5 = ctx.Stack.PopBottom();
				double num6 = ctx.Stack.PopBottom();
				double num7 = ctx.Stack.PopBottom();
				double num8 = ctx.Stack.PopBottom();
				double num9 = ctx.Stack.PopBottom();
				double num10 = ctx.Stack.PopBottom();
				double num11 = ctx.Stack.PopBottom();
				double num12 = num + num3 + num5 + num7 + num9;
				double num13 = num2 + num4 + num6 + num8 + num10;
				bool flag = Math.Abs(num12) > Math.Abs(num13);
				ctx.AddRelativeBezierCurve(num, num2, num3, num4, num5, num6);
				ctx.AddRelativeBezierCurve(num7, num8, num9, num10, flag ? num11 : 0.0, flag ? 0.0 : num11);
				ctx.Stack.Clear();
			}));
			Type2CharStringParser.TwoByteCommandStore = dictionary2;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0001B381 File Offset: 0x00019581
		[CompilerGenerated]
		internal static int <CalculatePrecedingHintBytes>g__SafeStemCount|14_0(int counts)
		{
			if (counts % 2 == 0)
			{
				return counts / 2;
			}
			return (counts - 1) / 2;
		}

		// Token: 0x04000361 RID: 865
		private const byte HstemByte = 1;

		// Token: 0x04000362 RID: 866
		private const byte VstemByte = 3;

		// Token: 0x04000363 RID: 867
		private const byte HstemhmByte = 18;

		// Token: 0x04000364 RID: 868
		private const byte HintmaskByte = 19;

		// Token: 0x04000365 RID: 869
		private const byte CntrmaskByte = 20;

		// Token: 0x04000366 RID: 870
		private const byte VstemhmByte = 23;

		// Token: 0x04000367 RID: 871
		private static readonly HashSet<byte> HintingCommandBytes;

		// Token: 0x04000368 RID: 872
		private static readonly IReadOnlyDictionary<byte, LazyType2Command> SingleByteCommandStore;

		// Token: 0x04000369 RID: 873
		private static readonly IReadOnlyDictionary<byte, LazyType2Command> TwoByteCommandStore;
	}
}
