using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;

namespace UglyToad.PdfPig.Parser.Parts.CrossReference
{
	// Token: 0x020000CF RID: 207
	internal struct TableSubsectionDefinition
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00017E88 File Offset: 0x00016088
		public readonly long FirstNumber { get; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00017E90 File Offset: 0x00016090
		public readonly int Count { get; }

		// Token: 0x060003E8 RID: 1000 RVA: 0x00017E98 File Offset: 0x00016098
		public TableSubsectionDefinition(long firstNumber, int count)
		{
			if (count < 0)
			{
				string text = "count";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(45, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Count must be 0 or positive, instead it was ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(count);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.FirstNumber = firstNumber;
			this.Count = count;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00017EF4 File Offset: 0x000160F4
		public static bool TryRead(ILog log, IInputBytes bytes, out TableSubsectionDefinition definition)
		{
			definition = default(TableSubsectionDefinition);
			string text = ReadHelper.ReadLine(bytes);
			string[] array = text.Split(TableSubsectionDefinition.Splitters, 1);
			if (array.Length != 2)
			{
				return false;
			}
			bool result;
			try
			{
				long firstNumber = long.Parse(array[0], CultureInfo.InvariantCulture);
				int count = int.Parse(array[1], CultureInfo.InvariantCulture);
				definition = new TableSubsectionDefinition(firstNumber, count);
				result = true;
			}
			catch (Exception ex)
			{
				log.Error("The format for the subsection definition was invalid, expected [long] [int], instead got '" + text + "'", ex);
				result = false;
			}
			return result;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00017F84 File Offset: 0x00016184
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.FirstNumber);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Count);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040001FD RID: 509
		private static readonly char[] Splitters = new char[]
		{
			' '
		};
	}
}
