using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.CharStrings
{
	// Token: 0x020000A1 RID: 161
	internal class Type2CharStrings
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001B391 File Offset: 0x00019591
		public IReadOnlyDictionary<string, Type2CharStrings.CommandSequence> CharStrings { get; }

		// Token: 0x060003E2 RID: 994 RVA: 0x0001B399 File Offset: 0x00019599
		public Type2CharStrings(IReadOnlyDictionary<string, Type2CharStrings.CommandSequence> charStrings)
		{
			if (charStrings == null)
			{
				throw new ArgumentNullException("charStrings");
			}
			this.CharStrings = charStrings;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001B3D0 File Offset: 0x000195D0
		public Type2Glyph Generate(string name, double defaultWidthX, double nominalWidthX)
		{
			object obj = this.locker;
			Type2Glyph type2Glyph;
			lock (obj)
			{
				Type2Glyph result;
				if (this.glyphs.TryGetValue(name, ref result))
				{
					return result;
				}
				Type2CharStrings.CommandSequence commandSequence;
				if (!this.CharStrings.TryGetValue(name, ref commandSequence) && !this.CharStrings.TryGetValue(".notdef", ref commandSequence))
				{
					throw new InvalidOperationException("No charstring sequence with the name /" + name + " in this font.");
				}
				try
				{
					type2Glyph = Type2CharStrings.Run(commandSequence, defaultWidthX, nominalWidthX);
					this.glyphs[name] = type2Glyph;
				}
				catch (Exception ex)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to interpret charstring for symbol with name: ");
					defaultInterpolatedStringHandler.AppendFormatted(name);
					defaultInterpolatedStringHandler.AppendLiteral(". Commands: ");
					defaultInterpolatedStringHandler.AppendFormatted<Type2CharStrings.CommandSequence>(commandSequence);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
				}
			}
			return type2Glyph;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001B4D4 File Offset: 0x000196D4
		private static Type2Glyph Run(Type2CharStrings.CommandSequence sequence, double defaultWidthX, double nominalWidthX)
		{
			Type2BuildCharContext type2BuildCharContext = new Type2BuildCharContext();
			bool flag = false;
			for (int i = -1; i < sequence.Values.Count; i++)
			{
				if (i >= 0)
				{
					float num = sequence.Values[i];
					type2BuildCharContext.Stack.Push((double)num);
				}
				foreach (Type2CharStrings.CommandSequence.CommandIdentifier identifier in sequence.GetCommandsAt(i + 1))
				{
					LazyType2Command command = Type2CharStringParser.GetCommand(identifier);
					bool flag2 = sequence.Values.Count + sequence.CommandIdentifiers.Count == 1;
					if (!flag)
					{
						flag = true;
						string name = command.Name;
						uint num2 = <PrivateImplementationDetails>.ComputeStringHash(name);
						if (num2 <= 2910336499U)
						{
							if (num2 <= 1890266854U)
							{
								if (num2 != 902428304U)
								{
									if (num2 != 1890266854U)
									{
										goto IL_24E;
									}
									if (!(name == "cntrmask"))
									{
										goto IL_24E;
									}
								}
								else if (!(name == "hintmask"))
								{
									goto IL_24E;
								}
								Type2CharStrings.SetWidthFromArgumentsIfPresent(type2BuildCharContext, nominalWidthX, 0);
								goto IL_250;
							}
							if (num2 != 1985583060U)
							{
								if (num2 != 2872262030U)
								{
									if (num2 != 2910336499U)
									{
										goto IL_24E;
									}
									if (!(name == "hmoveto"))
									{
										goto IL_24E;
									}
									goto IL_214;
								}
								else if (!(name == "hstem"))
								{
									goto IL_24E;
								}
							}
							else if (!(name == "vstem"))
							{
								goto IL_24E;
							}
						}
						else if (num2 <= 2958076653U)
						{
							if (num2 != 2945644814U)
							{
								if (num2 != 2958076653U)
								{
									goto IL_24E;
								}
								if (!(name == "hstemhm"))
								{
									goto IL_24E;
								}
							}
							else
							{
								if (!(name == "endchar"))
								{
									goto IL_24E;
								}
								if (flag2)
								{
									type2BuildCharContext.Width = new double?(defaultWidthX);
									goto IL_250;
								}
								Type2CharStrings.SetWidthFromArgumentsIfPresent(type2BuildCharContext, nominalWidthX, 0);
								goto IL_250;
							}
						}
						else if (num2 != 3233314721U)
						{
							if (num2 != 4154594221U)
							{
								if (num2 != 4186326235U)
								{
									goto IL_24E;
								}
								if (!(name == "vstemhm"))
								{
									goto IL_24E;
								}
							}
							else
							{
								if (!(name == "rmoveto"))
								{
									goto IL_24E;
								}
								Type2CharStrings.SetWidthFromArgumentsIfPresent(type2BuildCharContext, nominalWidthX, 2);
								goto IL_250;
							}
						}
						else
						{
							if (!(name == "vmoveto"))
							{
								goto IL_24E;
							}
							goto IL_214;
						}
						if (type2BuildCharContext.Stack.Length % 2 != 0)
						{
							type2BuildCharContext.Width = new double?(nominalWidthX + type2BuildCharContext.Stack.PopBottom());
							goto IL_250;
						}
						goto IL_250;
						IL_214:
						Type2CharStrings.SetWidthFromArgumentsIfPresent(type2BuildCharContext, nominalWidthX, 1);
						goto IL_250;
						IL_24E:
						flag = false;
					}
					IL_250:
					command.Run(type2BuildCharContext);
				}
			}
			return new Type2Glyph(type2BuildCharContext.Path, type2BuildCharContext.Width);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001B798 File Offset: 0x00019998
		private static void SetWidthFromArgumentsIfPresent(Type2BuildCharContext context, double nomimalWidthX, int expectedArgumentLength)
		{
			if (context.Stack.Length > expectedArgumentLength)
			{
				context.Width = new double?(nomimalWidthX + context.Stack.PopBottom());
			}
		}

		// Token: 0x0400036A RID: 874
		private readonly object locker = new object();

		// Token: 0x0400036B RID: 875
		private readonly Dictionary<string, Type2Glyph> glyphs = new Dictionary<string, Type2Glyph>();

		// Token: 0x020000E0 RID: 224
		public class CommandSequence
		{
			// Token: 0x17000273 RID: 627
			// (get) Token: 0x060005BB RID: 1467 RVA: 0x000218F3 File Offset: 0x0001FAF3
			public IReadOnlyList<float> Values { get; }

			// Token: 0x17000274 RID: 628
			// (get) Token: 0x060005BC RID: 1468 RVA: 0x000218FB File Offset: 0x0001FAFB
			public IReadOnlyList<Type2CharStrings.CommandSequence.CommandIdentifier> CommandIdentifiers { get; }

			// Token: 0x060005BD RID: 1469 RVA: 0x00021903 File Offset: 0x0001FB03
			public CommandSequence(IReadOnlyList<float> values, IReadOnlyList<Type2CharStrings.CommandSequence.CommandIdentifier> commandIdentifiers)
			{
				this.Values = values;
				this.CommandIdentifiers = commandIdentifiers;
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x00021919 File Offset: 0x0001FB19
			public IEnumerable<Type2CharStrings.CommandSequence.CommandIdentifier> GetCommandsAt(int index)
			{
				foreach (Type2CharStrings.CommandSequence.CommandIdentifier commandIdentifier in this.CommandIdentifiers)
				{
					if (commandIdentifier.CommandIndex == index)
					{
						yield return commandIdentifier;
					}
				}
				IEnumerator<Type2CharStrings.CommandSequence.CommandIdentifier> enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x00021930 File Offset: 0x0001FB30
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = -1; i < this.Values.Count; i++)
				{
					if (i >= 0)
					{
						stringBuilder.AppendLine(this.Values[i].ToString("N", CultureInfo.InvariantCulture));
					}
					foreach (Type2CharStrings.CommandSequence.CommandIdentifier identifier in this.GetCommandsAt(i + 1))
					{
						stringBuilder.AppendLine(Type2CharStringParser.GetCommand(identifier).Name);
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x020000E7 RID: 231
			public struct CommandIdentifier
			{
				// Token: 0x17000275 RID: 629
				// (get) Token: 0x060005CE RID: 1486 RVA: 0x00021AB4 File Offset: 0x0001FCB4
				public readonly int CommandIndex { get; }

				// Token: 0x17000276 RID: 630
				// (get) Token: 0x060005CF RID: 1487 RVA: 0x00021ABC File Offset: 0x0001FCBC
				public readonly bool IsMultiByteCommand { get; }

				// Token: 0x17000277 RID: 631
				// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00021AC4 File Offset: 0x0001FCC4
				public readonly byte CommandId { get; }

				// Token: 0x060005D1 RID: 1489 RVA: 0x00021ACC File Offset: 0x0001FCCC
				public CommandIdentifier(int commandIndex, bool isMultiByteCommand, byte commandId)
				{
					this.CommandIndex = commandIndex;
					this.IsMultiByteCommand = isMultiByteCommand;
					this.CommandId = commandId;
				}
			}
		}
	}
}
