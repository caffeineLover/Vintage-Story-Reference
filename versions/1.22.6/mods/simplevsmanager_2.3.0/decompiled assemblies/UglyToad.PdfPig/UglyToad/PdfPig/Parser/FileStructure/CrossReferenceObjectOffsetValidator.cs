using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Parser.Parts;

namespace UglyToad.PdfPig.Parser.FileStructure
{
	// Token: 0x020000D0 RID: 208
	internal static class CrossReferenceObjectOffsetValidator
	{
		// Token: 0x060003EC RID: 1004 RVA: 0x00017FDC File Offset: 0x000161DC
		public static bool ValidateCrossReferenceOffsets(IInputBytes bytes, CrossReferenceTable crossReferenceTable, ILog log, out IReadOnlyDictionary<IndirectReference, long> actualOffsets)
		{
			actualOffsets = crossReferenceTable.ObjectOffsets;
			if (CrossReferenceObjectOffsetValidator.ValidateXrefOffsets(bytes, crossReferenceTable.ObjectOffsets, log))
			{
				return true;
			}
			Dictionary<IndirectReference, long> dictionary = new Dictionary<IndirectReference, long>();
			IReadOnlyDictionary<IndirectReference, long> objectLocations = BruteForceSearcher.GetObjectLocations(bytes);
			if (objectLocations.Count > 0)
			{
				foreach (KeyValuePair<IndirectReference, long> keyValuePair in crossReferenceTable.ObjectOffsets)
				{
					if (keyValuePair.Value < 0L)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
					foreach (KeyValuePair<IndirectReference, long> keyValuePair2 in objectLocations)
					{
						dictionary[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
				actualOffsets = dictionary;
			}
			return false;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000180C4 File Offset: 0x000162C4
		private static bool ValidateXrefOffsets(IInputBytes bytes, IReadOnlyDictionary<IndirectReference, long> objectOffsets, ILog log)
		{
			if (objectOffsets == null)
			{
				return true;
			}
			foreach (KeyValuePair<IndirectReference, long> keyValuePair in objectOffsets)
			{
				IndirectReference key = keyValuePair.Key;
				long value = keyValuePair.Value;
				if (value >= 0L && !CrossReferenceObjectOffsetValidator.CheckObjectKeys(bytes, key, value))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 2);
					defaultInterpolatedStringHandler.AppendLiteral("At least one cross-reference offset was incorrect. ");
					defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(key);
					defaultInterpolatedStringHandler.AppendLiteral(" could not be found at ");
					defaultInterpolatedStringHandler.AppendFormatted<long>(value);
					defaultInterpolatedStringHandler.AppendLiteral(". ");
					log.Error(defaultInterpolatedStringHandler.ToStringAndClear() + "Using brute-force search to repair object offsets.");
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00018190 File Offset: 0x00016390
		private static bool CheckObjectKeys(IInputBytes bytes, IndirectReference objectKey, long offset)
		{
			if (offset < 6L)
			{
				return false;
			}
			long objectNumber = objectKey.ObjectNumber;
			long genId = (long)objectKey.Generation;
			long currentOffset = bytes.CurrentOffset;
			string text = ObjectHelper.CreateObjectString(objectNumber, genId);
			try
			{
				if (offset >= bytes.Length)
				{
					bytes.Seek(currentOffset);
					return false;
				}
				bytes.Seek(offset);
				if (ReadHelper.IsWhitespace(bytes.CurrentByte))
				{
					bytes.MoveNext();
				}
				if (ReadHelper.IsString(bytes, text))
				{
					bytes.Seek(currentOffset);
					return true;
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				bytes.Seek(currentOffset);
			}
			return false;
		}

		// Token: 0x04000200 RID: 512
		private const long MinimumSearchOffset = 6L;
	}
}
