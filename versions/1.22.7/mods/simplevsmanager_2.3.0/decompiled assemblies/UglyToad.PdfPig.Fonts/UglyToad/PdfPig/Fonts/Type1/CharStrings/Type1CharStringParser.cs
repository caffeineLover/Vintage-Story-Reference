using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands;
using UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Arithmetic;
using UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Hint;
using UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction;
using UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.StartFinishOutline;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings
{
	// Token: 0x02000014 RID: 20
	internal static class Type1CharStringParser
	{
		// Token: 0x06000088 RID: 136 RVA: 0x0000589C File Offset: 0x00003A9C
		public static Type1CharStrings Parse(IReadOnlyList<Type1CharstringDecryptedBytes> charStrings, IReadOnlyList<Type1CharstringDecryptedBytes> subroutines)
		{
			if (charStrings == null)
			{
				throw new ArgumentNullException("charStrings");
			}
			if (subroutines == null)
			{
				throw new ArgumentNullException("subroutines");
			}
			Dictionary<string, Type1CharStrings.CommandSequence> dictionary = new Dictionary<string, Type1CharStrings.CommandSequence>(charStrings.Count);
			Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
			for (int i = 0; i < charStrings.Count; i++)
			{
				Type1CharstringDecryptedBytes type1CharstringDecryptedBytes = charStrings[i];
				IReadOnlyList<Union<double, LazyType1Command>> commands = Type1CharStringParser.ParseSingle(type1CharstringDecryptedBytes.Bytes);
				dictionary[type1CharstringDecryptedBytes.Name] = new Type1CharStrings.CommandSequence(commands);
				dictionary2[i] = type1CharstringDecryptedBytes.Name;
			}
			Dictionary<int, Type1CharStrings.CommandSequence> dictionary3 = new Dictionary<int, Type1CharStrings.CommandSequence>(subroutines.Count);
			foreach (Type1CharstringDecryptedBytes type1CharstringDecryptedBytes2 in subroutines)
			{
				IReadOnlyList<Union<double, LazyType1Command>> commands2 = Type1CharStringParser.ParseSingle(type1CharstringDecryptedBytes2.Bytes);
				dictionary3[type1CharstringDecryptedBytes2.Index] = new Type1CharStrings.CommandSequence(commands2);
			}
			return new Type1CharStrings(dictionary, dictionary2, dictionary3);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005994 File Offset: 0x00003B94
		private static IReadOnlyList<Union<double, LazyType1Command>> ParseSingle(IReadOnlyList<byte> charStringBytes)
		{
			List<Union<double, LazyType1Command>> list = new List<Union<double, LazyType1Command>>();
			for (int i = 0; i < charStringBytes.Count; i++)
			{
				byte b = charStringBytes[i];
				if (b <= 31)
				{
					LazyType1Command command = Type1CharStringParser.GetCommand(b, charStringBytes, ref i);
					if (command != null)
					{
						list.Add(new Union<double, LazyType1Command>.Case2(command));
					}
				}
				else
				{
					int num = Type1CharStringParser.InterpretNumber(b, charStringBytes, ref i);
					list.Add(new Union<double, LazyType1Command>.Case1((double)num));
				}
			}
			return list;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000059FC File Offset: 0x00003BFC
		private static int InterpretNumber(byte b, IReadOnlyList<byte> bytes, ref int i)
		{
			if (b >= 32 && b <= 246)
			{
				return (int)(b - 139);
			}
			int num;
			if (b >= 247 && b <= 250)
			{
				num = i + 1;
				i = num;
				byte b2 = bytes[num];
				return (int)(b - 247) * 256 + (int)b2 + 108;
			}
			if (b >= 251 && b <= 254)
			{
				num = i + 1;
				i = num;
				byte b3 = bytes[num];
				return -((int)(b - 251) * 256) - (int)b3 - 108;
			}
			num = i + 1;
			i = num;
			int num2 = (int)bytes[num] << 24;
			num = i + 1;
			i = num;
			int num3 = num2 | (int)bytes[num] << 16;
			num = i + 1;
			i = num;
			int num4 = num3 | (int)bytes[num] << 8;
			num = i + 1;
			i = num;
			return num4 | (int)bytes[num];
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005ACC File Offset: 0x00003CCC
		public static LazyType1Command GetCommand(byte v, IReadOnlyList<byte> bytes, ref int i)
		{
			switch (v)
			{
			case 1:
				return HStemCommand.Lazy;
			case 3:
				return VStemCommand.Lazy;
			case 4:
				return VMoveToCommand.Lazy;
			case 5:
				return RLineToCommand.Lazy;
			case 6:
				return HLineToCommand.Lazy;
			case 7:
				return VLineToCommand.Lazy;
			case 8:
				return RelativeRCurveToCommand.Lazy;
			case 9:
				return ClosePathCommand.Lazy;
			case 10:
				return CallSubrCommand.Lazy;
			case 11:
				return ReturnCommand.Lazy;
			case 12:
			{
				int num = i + 1;
				i = num;
				byte b = bytes[num];
				if (b <= 12)
				{
					switch (b)
					{
					case 0:
						return DotSectionCommand.Lazy;
					case 1:
						return VStem3Command.Lazy;
					case 2:
						return HStem3Command.Lazy;
					case 3:
					case 4:
					case 5:
						break;
					case 6:
						return SeacCommand.Lazy;
					case 7:
						return SbwCommand.Lazy;
					default:
						if (b == 12)
						{
							return DivCommand.Lazy;
						}
						break;
					}
				}
				else
				{
					if (b == 16)
					{
						return CallOtherSubrCommand.Lazy;
					}
					if (b == 17)
					{
						return PopCommand.Lazy;
					}
					if (b == 33)
					{
						return SetCurrentPointCommand.Lazy;
					}
				}
				break;
			}
			case 13:
				return HsbwCommand.Lazy;
			case 14:
				return EndCharCommand.Lazy;
			case 21:
				return RMoveToCommand.Lazy;
			case 22:
				return HMoveToCommand.Lazy;
			case 30:
				return VhCurveToCommand.Lazy;
			case 31:
				return HvCurveToCommand.Lazy;
			}
			return null;
		}
	}
}
