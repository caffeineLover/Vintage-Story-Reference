using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries
{
	// Token: 0x02000096 RID: 150
	internal abstract class CompactFontFormatDictionaryReader<TResult, TBuilder>
	{
		// Token: 0x06000359 RID: 857
		public abstract TResult Read(CompactFontFormatData data, IReadOnlyList<string> stringIndex);

		// Token: 0x0600035A RID: 858 RVA: 0x00019368 File Offset: 0x00017568
		protected TBuilder ReadDictionary(TBuilder builder, CompactFontFormatData data, IReadOnlyList<string> stringIndex)
		{
			while (data.CanRead())
			{
				this.operands.Clear();
				int num = 0;
				byte b;
				for (;;)
				{
					num++;
					if (num > 256)
					{
						goto Block_1;
					}
					b = data.ReadByte();
					if (b <= 21)
					{
						break;
					}
					if (b == 28)
					{
						int num2 = (int)data.ReadByte() << 8 | (int)data.ReadByte();
						this.operands.Add(new CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand(num2));
					}
					else if (b == 29)
					{
						int num3 = (int)data.ReadByte() << 24 | (int)data.ReadByte() << 16 | (int)data.ReadByte() << 8 | (int)data.ReadByte();
						this.operands.Add(new CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand(num3));
					}
					else if (b == 30)
					{
						decimal num4 = CompactFontFormatDictionaryReader<TResult, TBuilder>.ReadRealNumber(data);
						this.operands.Add(new CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand(num4));
					}
					else if (b >= 32 && b <= 246)
					{
						int num5 = (int)(b - 139);
						this.operands.Add(new CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand(num5));
					}
					else if (b >= 247 && b <= 250)
					{
						int num6 = (int)(b - 247) * 256 + (int)data.ReadByte() + 108;
						this.operands.Add(new CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand(num6));
					}
					else
					{
						if (b < 251 || b > 254)
						{
							goto IL_19E;
						}
						int num7 = (int)(-(int)(b - 251)) * 256 - (int)data.ReadByte() - 108;
						this.operands.Add(new CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand(num7));
					}
				}
				CompactFontFormatDictionaryReader<TResult, TBuilder>.OperandKey operandKey = (b == 12) ? new CompactFontFormatDictionaryReader<TResult, TBuilder>.OperandKey(b, data.ReadByte()) : new CompactFontFormatDictionaryReader<TResult, TBuilder>.OperandKey(b);
				this.ApplyOperation(builder, this.operands, operandKey, stringIndex);
				continue;
				Block_1:
				throw new InvalidOperationException("Got caught in an infinite loop trying to read a CFF dictionary.");
				IL_19E:
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(62, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The first dictionary byte was not in the range 29 - 254. Got ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(b);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return builder;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00019558 File Offset: 0x00017758
		private static decimal ReadRealNumber(CompactFontFormatData data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			while (!flag)
			{
				byte b = data.ReadByte();
				int num = (int)(b / 16);
				int num2 = (int)(b % 16);
				for (int i = 0; i < 2; i++)
				{
					int num3 = (i == 0) ? num : num2;
					switch (num3)
					{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
						stringBuilder.Append(num3);
						flag2 = false;
						break;
					case 10:
						stringBuilder.Append(".");
						break;
					case 11:
						if (!flag3)
						{
							stringBuilder.Append("E");
							flag2 = true;
							flag3 = true;
						}
						break;
					case 12:
						if (!flag3)
						{
							stringBuilder.Append("E-");
							flag2 = true;
							flag3 = true;
						}
						break;
					case 13:
						break;
					case 14:
						stringBuilder.Append("-");
						break;
					case 15:
						flag = true;
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(30, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Did not expect nibble value: ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			if (flag2)
			{
				stringBuilder.Append("0");
			}
			if (stringBuilder.Length == 0)
			{
				return 0m;
			}
			if (!flag3)
			{
				return decimal.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture);
			}
			return decimal.Parse(stringBuilder.ToString(), 167, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600035C RID: 860
		protected abstract void ApplyOperation(TBuilder builder, List<CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand> operands, CompactFontFormatDictionaryReader<TResult, TBuilder>.OperandKey operandKey, IReadOnlyList<string> stringIndex);

		// Token: 0x0600035D RID: 861 RVA: 0x000196D0 File Offset: 0x000178D0
		protected static string GetString(List<CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand> operands, IReadOnlyList<string> stringIndex)
		{
			if (operands.Count == 0)
			{
				throw new InvalidOperationException("Cannot read a string from an empty operands array.");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (operands[0].Int == null)
			{
				defaultInterpolatedStringHandler..ctor(64, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The first operand for reading a string was not an integer. Got: ");
				defaultInterpolatedStringHandler.AppendFormatted<decimal>(operands[0].Decimal);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int value = operands[0].Int.Value;
			if (value >= 0 && value <= 390)
			{
				return CompactFontFormatStandardStrings.GetName(value);
			}
			int num = value - 391;
			if (num >= 0 && num < stringIndex.Count)
			{
				return stringIndex[num];
			}
			defaultInterpolatedStringHandler..ctor(3, 1);
			defaultInterpolatedStringHandler.AppendLiteral("SID");
			defaultInterpolatedStringHandler.AppendFormatted<int>(value);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000197B0 File Offset: 0x000179B0
		protected static PdfRectangle GetBoundingBox(List<CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand> operands)
		{
			if (operands.Count != 4)
			{
				return default(PdfRectangle);
			}
			return new PdfRectangle((double)operands[0].Decimal, (double)operands[1].Decimal, (double)operands[2].Decimal, (double)operands[3].Decimal);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0001982C File Offset: 0x00017A2C
		protected static decimal[] ToArray(List<CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand> operands)
		{
			decimal[] array = new decimal[operands.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = operands[i].Decimal;
			}
			return array;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001986C File Offset: 0x00017A6C
		protected static int GetIntOrDefault(List<CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand> operands, int defaultValue = 0)
		{
			if (operands.Count == 0)
			{
				return defaultValue;
			}
			CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand operand = operands[0];
			if (operand.Int != null)
			{
				return operand.Int.Value;
			}
			return defaultValue;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000198B0 File Offset: 0x00017AB0
		protected static int[] ReadDeltaToIntArray(List<CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand> operands)
		{
			int[] array = new int[operands.Count];
			if (operands.Count == 0)
			{
				return array;
			}
			array[0] = (int)operands[0].Decimal;
			for (int i = 1; i < operands.Count; i++)
			{
				int num = array[i - 1];
				decimal @decimal = operands[i].Decimal;
				array[i] = (int)(num + @decimal);
			}
			return array;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00019928 File Offset: 0x00017B28
		protected static decimal[] ReadDeltaToArray(List<CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand> operands)
		{
			decimal[] array = new decimal[operands.Count];
			if (operands.Count == 0)
			{
				return array;
			}
			array[0] = operands[0].Decimal;
			for (int i = 1; i < operands.Count; i++)
			{
				decimal num = array[i - 1];
				decimal @decimal = operands[i].Decimal;
				array[i] = num + @decimal;
			}
			return array;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0001999D File Offset: 0x00017B9D
		protected CompactFontFormatDictionaryReader()
		{
			this.operands = new List<CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand>();
			base..ctor();
		}

		// Token: 0x04000329 RID: 809
		private readonly List<CompactFontFormatDictionaryReader<TResult, TBuilder>.Operand> operands;

		// Token: 0x020000DB RID: 219
		protected struct Operand
		{
			// Token: 0x17000269 RID: 617
			// (get) Token: 0x06000573 RID: 1395 RVA: 0x00020370 File Offset: 0x0001E570
			public readonly int? Int
			{
				[CompilerGenerated]
				get
				{
					return this.<Int>k__BackingField;
				}
			}

			// Token: 0x1700026A RID: 618
			// (get) Token: 0x06000574 RID: 1396 RVA: 0x00020378 File Offset: 0x0001E578
			public readonly decimal Decimal
			{
				[CompilerGenerated]
				get
				{
					return this.<Decimal>k__BackingField;
				}
			}

			// Token: 0x06000575 RID: 1397 RVA: 0x00020380 File Offset: 0x0001E580
			public Operand(int integer)
			{
				this.<Int>k__BackingField = new int?(integer);
				this.<Decimal>k__BackingField = integer;
			}

			// Token: 0x06000576 RID: 1398 RVA: 0x0002039A File Offset: 0x0001E59A
			public Operand(decimal d)
			{
				this.<Int>k__BackingField = default(int?);
				this.<Decimal>k__BackingField = d;
			}
		}

		// Token: 0x020000DC RID: 220
		protected struct OperandKey
		{
			// Token: 0x1700026B RID: 619
			// (get) Token: 0x06000577 RID: 1399 RVA: 0x000203AF File Offset: 0x0001E5AF
			public readonly byte Byte0
			{
				[CompilerGenerated]
				get
				{
					return this.<Byte0>k__BackingField;
				}
			}

			// Token: 0x1700026C RID: 620
			// (get) Token: 0x06000578 RID: 1400 RVA: 0x000203B7 File Offset: 0x0001E5B7
			public readonly byte? Byte1
			{
				[CompilerGenerated]
				get
				{
					return this.<Byte1>k__BackingField;
				}
			}

			// Token: 0x06000579 RID: 1401 RVA: 0x000203BF File Offset: 0x0001E5BF
			public OperandKey(byte byte0)
			{
				this.<Byte0>k__BackingField = byte0;
				this.<Byte1>k__BackingField = default(byte?);
			}

			// Token: 0x0600057A RID: 1402 RVA: 0x000203D4 File Offset: 0x0001E5D4
			public OperandKey(byte byte0, byte byte1)
			{
				this.<Byte0>k__BackingField = byte0;
				this.<Byte1>k__BackingField = new byte?(byte1);
			}
		}
	}
}
