using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings
{
	// Token: 0x02000015 RID: 21
	internal class Type1CharStrings
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00005C4C File Offset: 0x00003E4C
		public IReadOnlyDictionary<string, Type1CharStrings.CommandSequence> CharStrings { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00005C54 File Offset: 0x00003E54
		public IReadOnlyDictionary<int, Type1CharStrings.CommandSequence> Subroutines { get; }

		// Token: 0x0600008E RID: 142 RVA: 0x00005C5C File Offset: 0x00003E5C
		public Type1CharStrings(IReadOnlyDictionary<string, Type1CharStrings.CommandSequence> charStrings, IReadOnlyDictionary<int, string> charStringIndexToName, IReadOnlyDictionary<int, Type1CharStrings.CommandSequence> subroutines)
		{
			if (charStringIndexToName == null)
			{
				throw new ArgumentNullException("charStringIndexToName");
			}
			this.charStringIndexToName = charStringIndexToName;
			if (charStrings == null)
			{
				throw new ArgumentNullException("charStrings");
			}
			this.CharStrings = charStrings;
			if (subroutines == null)
			{
				throw new ArgumentNullException("subroutines");
			}
			this.Subroutines = subroutines;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005CC8 File Offset: 0x00003EC8
		public bool TryGenerate(string name, out IReadOnlyList<PdfSubpath> path)
		{
			path = new List<PdfSubpath>();
			object obj = this.locker;
			lock (obj)
			{
				if (this.glyphs.TryGetValue(name, ref path))
				{
					return true;
				}
				Type1CharStrings.CommandSequence sequence;
				if (!this.CharStrings.TryGetValue(name, ref sequence))
				{
					return false;
				}
				try
				{
					path = this.Run(sequence);
					this.glyphs[name] = path;
				}
				catch
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005D5C File Offset: 0x00003F5C
		private IReadOnlyList<PdfSubpath> Run(Type1CharStrings.CommandSequence sequence)
		{
			Type1BuildCharContext type1BuildCharContext = new Type1BuildCharContext(this.Subroutines, delegate(int i)
			{
				string text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (!this.charStringIndexToName.TryGetValue(i, ref text))
				{
					defaultInterpolatedStringHandler..ctor(66, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to retrieve Type 1 charstring by index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" which did not exist.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				IReadOnlyList<PdfSubpath> result;
				if (this.glyphs.TryGetValue(text, ref result))
				{
					return result;
				}
				Type1CharStrings.CommandSequence sequence2;
				if (!this.CharStrings.TryGetValue(text, ref sequence2))
				{
					defaultInterpolatedStringHandler..ctor(105, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to retrieve Type 1 charstring by index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" which mapped to name ");
					defaultInterpolatedStringHandler.AppendFormatted(text);
					defaultInterpolatedStringHandler.AppendLiteral(" but was not found in the charstrings.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				IReadOnlyList<PdfSubpath> readOnlyList = this.Run(sequence2);
				this.glyphs[text] = readOnlyList;
				return readOnlyList;
			}, delegate(string s)
			{
				IReadOnlyList<PdfSubpath> result;
				if (this.glyphs.TryGetValue(s, ref result))
				{
					return result;
				}
				Type1CharStrings.CommandSequence sequence2;
				if (!this.CharStrings.TryGetValue(s, ref sequence2))
				{
					throw new InvalidOperationException("Tried to retrieve Type 1 charstring by name " + s + " but it was not found in the charstrings.");
				}
				IReadOnlyList<PdfSubpath> readOnlyList = this.Run(sequence2);
				this.glyphs[s] = readOnlyList;
				return readOnlyList;
			});
			foreach (Union<double, LazyType1Command> union in sequence.Commands)
			{
				double value;
				LazyType1Command lazyType1Command;
				if (union.TryGetFirst(ref value))
				{
					type1BuildCharContext.Stack.Push(value);
				}
				else if (union.TryGetSecond(ref lazyType1Command))
				{
					lazyType1Command.Run(type1BuildCharContext);
				}
			}
			return type1BuildCharContext.Path;
		}

		// Token: 0x04000060 RID: 96
		private readonly IReadOnlyDictionary<int, string> charStringIndexToName;

		// Token: 0x04000061 RID: 97
		private readonly object locker = new object();

		// Token: 0x04000062 RID: 98
		private readonly Dictionary<string, IReadOnlyList<PdfSubpath>> glyphs = new Dictionary<string, IReadOnlyList<PdfSubpath>>();

		// Token: 0x020000C1 RID: 193
		public class CommandSequence
		{
			// Token: 0x1700022C RID: 556
			// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0001F4EC File Offset: 0x0001D6EC
			public IReadOnlyList<Union<double, LazyType1Command>> Commands { get; }

			// Token: 0x060004F4 RID: 1268 RVA: 0x0001F4F4 File Offset: 0x0001D6F4
			public CommandSequence(IReadOnlyList<Union<double, LazyType1Command>> commands)
			{
				if (commands == null)
				{
					throw new ArgumentNullException("commands");
				}
				this.Commands = commands;
			}

			// Token: 0x060004F5 RID: 1269 RVA: 0x0001F512 File Offset: 0x0001D712
			public override string ToString()
			{
				return string.Join(", ", Enumerable.Select<Union<double, LazyType1Command>, string>(this.Commands, (Union<double, LazyType1Command> x) => x.ToString()));
			}
		}
	}
}
