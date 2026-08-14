using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.CrossReference
{
	// Token: 0x020001D8 RID: 472
	internal class CrossReferenceTableBuilder
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x00038F44 File Offset: 0x00037144
		public IReadOnlyList<CrossReferenceTablePart> Parts
		{
			get
			{
				return this.parts;
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00038F4C File Offset: 0x0003714C
		public void Add(CrossReferenceTablePart part)
		{
			if (part == null)
			{
				throw new ArgumentNullException("part");
			}
			this.parts.Add(part);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00038F68 File Offset: 0x00037168
		public CrossReferenceTable Build(long firstCrossReferenceOffset, long offsetCorrection, ILog log)
		{
			CrossReferenceTableBuilder.<>c__DisplayClass4_0 CS$<>8__locals1 = new CrossReferenceTableBuilder.<>c__DisplayClass4_0();
			CS$<>8__locals1.firstCrossReferenceOffset = firstCrossReferenceOffset;
			CS$<>8__locals1.offsetCorrection = offsetCorrection;
			CrossReferenceType type = CrossReferenceType.Table;
			DictionaryToken dictionaryToken = new DictionaryToken(new Dictionary<NameToken, IToken>());
			Dictionary<IndirectReference, long> dictionary = new Dictionary<IndirectReference, long>();
			List<long> list = new List<long>();
			CrossReferenceTablePart crossReferenceTablePart = Enumerable.FirstOrDefault<CrossReferenceTablePart>(this.parts, (CrossReferenceTablePart x) => x.Offset == CS$<>8__locals1.firstCrossReferenceOffset);
			if (crossReferenceTablePart == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(64, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Did not find an XRef object at the specified startxref position ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(CS$<>8__locals1.firstCrossReferenceOffset);
				log.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				list.AddRange(Enumerable.Select<CrossReferenceTablePart, long>(this.parts, (CrossReferenceTablePart x) => x.Offset));
				list.Sort();
			}
			else
			{
				CrossReferenceTableBuilder.<>c__DisplayClass4_1 CS$<>8__locals2 = new CrossReferenceTableBuilder.<>c__DisplayClass4_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				type = crossReferenceTablePart.Type;
				list.Add(CS$<>8__locals2.CS$<>8__locals1.firstCrossReferenceOffset);
				CS$<>8__locals2.activePart = crossReferenceTablePart;
				using (IEnumerator<CrossReferenceTablePart> enumerator = Enumerable.Where<CrossReferenceTablePart>(this.parts, delegate(CrossReferenceTablePart x)
				{
					long? tiedToXrefAtOffset = x.TiedToXrefAtOffset;
					long offset = CS$<>8__locals2.activePart.Offset;
					return tiedToXrefAtOffset.GetValueOrDefault() == offset & tiedToXrefAtOffset != null;
				}).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CrossReferenceTablePart crossReferenceTablePart2 = enumerator.Current;
						list.Add(crossReferenceTablePart2.Offset);
					}
					goto IL_1C4;
				}
				IL_13D:
				long prevBytePos = crossReferenceTablePart.GetPreviousOffset();
				if (prevBytePos == -1L)
				{
					goto IL_1D0;
				}
				crossReferenceTablePart = Enumerable.FirstOrDefault<CrossReferenceTablePart>(this.parts, (CrossReferenceTablePart x) => x.Offset == prevBytePos || x.Offset == prevBytePos + CS$<>8__locals2.CS$<>8__locals1.offsetCorrection);
				if (crossReferenceTablePart == null)
				{
					log.Warn("Did not found XRef object pointed to by 'Prev' key at position " + prevBytePos.ToString());
					goto IL_1D0;
				}
				list.Add(prevBytePos);
				if (list.Count >= this.parts.Count)
				{
					goto IL_1D0;
				}
				IL_1C4:
				if (crossReferenceTablePart.Dictionary != null)
				{
					goto IL_13D;
				}
				IL_1D0:
				list.Reverse();
			}
			using (List<long>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					long bPos = enumerator2.Current;
					CrossReferenceTablePart crossReferenceTablePart3 = Enumerable.First<CrossReferenceTablePart>(this.parts, (CrossReferenceTablePart x) => x.Offset == bPos || x.Offset == bPos + CS$<>8__locals1.offsetCorrection);
					if (crossReferenceTablePart3.Dictionary != null)
					{
						foreach (KeyValuePair<string, IToken> keyValuePair in crossReferenceTablePart3.Dictionary.Data)
						{
							if (!keyValuePair.Key.Equals("Size", 5) || !dictionaryToken.ContainsKey(NameToken.Size))
							{
								dictionaryToken = dictionaryToken.With(keyValuePair.Key, keyValuePair.Value);
							}
						}
					}
					foreach (KeyValuePair<IndirectReference, long> keyValuePair2 in crossReferenceTablePart3.ObjectOffsets)
					{
						dictionary[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
			}
			return new CrossReferenceTable(type, dictionary, new TrailerDictionary(dictionaryToken), Enumerable.ToList<CrossReferenceTable.CrossReferenceOffset>(Enumerable.Select<CrossReferenceTablePart, CrossReferenceTable.CrossReferenceOffset>(this.parts, delegate(CrossReferenceTablePart x)
			{
				long previousOffset = x.GetPreviousOffset();
				return new CrossReferenceTable.CrossReferenceOffset(x.Offset, (previousOffset >= 0L) ? new long?(previousOffset) : default(long?));
			})));
		}

		// Token: 0x040005BF RID: 1471
		private readonly List<CrossReferenceTablePart> parts = new List<CrossReferenceTablePart>();
	}
}
