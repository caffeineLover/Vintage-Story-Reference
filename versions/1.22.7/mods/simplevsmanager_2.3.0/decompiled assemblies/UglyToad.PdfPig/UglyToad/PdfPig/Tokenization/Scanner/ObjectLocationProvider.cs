using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization.Scanner
{
	// Token: 0x02000084 RID: 132
	internal class ObjectLocationProvider : IObjectLocationProvider
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0000E60E File Offset: 0x0000C80E
		public ObjectLocationProvider(Func<CrossReferenceTable> crossReferenceTable, IInputBytes bytes)
		{
			this.crossReferenceTable = crossReferenceTable;
			this.bytes = bytes;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000E63C File Offset: 0x0000C83C
		public bool TryGetOffset(IndirectReference reference, out long offset)
		{
			if (!this.loadedFromTable)
			{
				CrossReferenceTable crossReferenceTable = this.crossReferenceTable.Invoke();
				if (crossReferenceTable != null)
				{
					foreach (KeyValuePair<IndirectReference, long> keyValuePair in crossReferenceTable.ObjectOffsets)
					{
						this.offsets[keyValuePair.Key] = keyValuePair.Value;
					}
					this.loadedFromTable = true;
				}
			}
			if (this.offsets.TryGetValue(reference, ref offset))
			{
				return true;
			}
			if (this.bruteForcedOffsets == null)
			{
				this.bruteForcedOffsets = BruteForceSearcher.GetObjectLocations(this.bytes);
			}
			return this.bruteForcedOffsets.TryGetValue(reference, ref offset);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000E6F0 File Offset: 0x0000C8F0
		public void UpdateOffset(IndirectReference reference, long offset)
		{
			this.offsets[reference] = offset;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000E6FF File Offset: 0x0000C8FF
		public bool TryGetCached(IndirectReference reference, out ObjectToken objectToken)
		{
			return this.cache.TryGetValue(reference, ref objectToken);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000E710 File Offset: 0x0000C910
		public void Cache(ObjectToken objectToken, bool force = false)
		{
			if (objectToken == null)
			{
				throw new ArgumentNullException();
			}
			CrossReferenceTable crossReferenceTable = this.crossReferenceTable.Invoke();
			long num;
			if (!force && crossReferenceTable != null && crossReferenceTable.ObjectOffsets.TryGetValue(objectToken.Number, ref num) && objectToken.Position != num)
			{
				return;
			}
			this.cache[objectToken.Number] = objectToken;
		}

		// Token: 0x040000F3 RID: 243
		private readonly Dictionary<IndirectReference, ObjectToken> cache = new Dictionary<IndirectReference, ObjectToken>();

		// Token: 0x040000F4 RID: 244
		private readonly Func<CrossReferenceTable> crossReferenceTable;

		// Token: 0x040000F5 RID: 245
		private readonly IInputBytes bytes;

		// Token: 0x040000F6 RID: 246
		private IReadOnlyDictionary<IndirectReference, long> bruteForcedOffsets;

		// Token: 0x040000F7 RID: 247
		private bool loadedFromTable;

		// Token: 0x040000F8 RID: 248
		private readonly Dictionary<IndirectReference, long> offsets = new Dictionary<IndirectReference, long>();
	}
}
