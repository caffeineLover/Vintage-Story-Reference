using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.CrossReference
{
	// Token: 0x020001D7 RID: 471
	public class CrossReferenceTable
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00038E75 File Offset: 0x00037075
		[NotNull]
		public IReadOnlyDictionary<IndirectReference, long> ObjectOffsets
		{
			get
			{
				return this.objectOffsets;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00038E7D File Offset: 0x0003707D
		public CrossReferenceType Type { get; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00038E85 File Offset: 0x00037085
		[NotNull]
		public TrailerDictionary Trailer { get; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00038E8D File Offset: 0x0003708D
		[NotNull]
		public IReadOnlyList<CrossReferenceTable.CrossReferenceOffset> CrossReferenceOffsets { get; }

		// Token: 0x06000B23 RID: 2851 RVA: 0x00038E98 File Offset: 0x00037098
		internal CrossReferenceTable(CrossReferenceType type, IReadOnlyDictionary<IndirectReference, long> objectOffsets, TrailerDictionary trailer, IReadOnlyList<CrossReferenceTable.CrossReferenceOffset> crossReferenceOffsets)
		{
			if (objectOffsets == null)
			{
				throw new ArgumentNullException("objectOffsets");
			}
			this.Type = type;
			if (trailer == null)
			{
				throw new ArgumentNullException("trailer");
			}
			this.Trailer = trailer;
			if (crossReferenceOffsets == null)
			{
				throw new ArgumentNullException("crossReferenceOffsets");
			}
			this.CrossReferenceOffsets = crossReferenceOffsets;
			Dictionary<IndirectReference, long> dictionary = new Dictionary<IndirectReference, long>();
			foreach (KeyValuePair<IndirectReference, long> keyValuePair in objectOffsets)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
			this.objectOffsets = dictionary;
		}

		// Token: 0x040005BB RID: 1467
		private readonly Dictionary<IndirectReference, long> objectOffsets;

		// Token: 0x020002B9 RID: 697
		public struct CrossReferenceOffset
		{
			// Token: 0x1700041D RID: 1053
			// (get) Token: 0x06000F09 RID: 3849 RVA: 0x00043D0A File Offset: 0x00041F0A
			public readonly long Current { get; }

			// Token: 0x1700041E RID: 1054
			// (get) Token: 0x06000F0A RID: 3850 RVA: 0x00043D12 File Offset: 0x00041F12
			public readonly long? Previous { get; }

			// Token: 0x06000F0B RID: 3851 RVA: 0x00043D1A File Offset: 0x00041F1A
			public CrossReferenceOffset(long current, long? previous)
			{
				this.Current = current;
				this.Previous = previous;
			}

			// Token: 0x06000F0C RID: 3852 RVA: 0x00043D2C File Offset: 0x00041F2C
			public override string ToString()
			{
				string text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (this.Previous == null)
				{
					text = string.Empty;
				}
				else
				{
					defaultInterpolatedStringHandler..ctor(1, 1);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<long?>(this.Previous);
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				string text2 = text;
				defaultInterpolatedStringHandler..ctor(0, 2);
				defaultInterpolatedStringHandler.AppendFormatted<long>(this.Current);
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}
	}
}
