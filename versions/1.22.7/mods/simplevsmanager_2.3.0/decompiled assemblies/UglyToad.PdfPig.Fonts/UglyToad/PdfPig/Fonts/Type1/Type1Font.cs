using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Type1.CharStrings;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Fonts.Type1
{
	// Token: 0x02000009 RID: 9
	public class Type1Font
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000033 RID: 51 RVA: 0x0000292F File Offset: 0x00000B2F
		public string Name { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002937 File Offset: 0x00000B37
		public IReadOnlyDictionary<int, string> Encoding { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000293F File Offset: 0x00000B3F
		public TransformationMatrix FontMatrix { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002947 File Offset: 0x00000B47
		public PdfRectangle BoundingBox { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000294F File Offset: 0x00000B4F
		public Type1PrivateDictionary PrivateDictionary { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002957 File Offset: 0x00000B57
		internal Type1CharStrings CharStrings { get; }

		// Token: 0x06000039 RID: 57 RVA: 0x00002960 File Offset: 0x00000B60
		internal Type1Font(string name, IReadOnlyDictionary<int, string> encoding, ArrayToken fontMatrix, PdfRectangle boundingBox, Type1PrivateDictionary privateDictionary, Type1CharStrings charStrings)
		{
			this.Name = name;
			this.Encoding = encoding;
			this.FontMatrix = Type1Font.GetFontTransformationMatrix(fontMatrix);
			this.BoundingBox = boundingBox;
			if (privateDictionary == null)
			{
				throw new ArgumentNullException("privateDictionary");
			}
			this.PrivateDictionary = privateDictionary;
			if (charStrings == null)
			{
				throw new ArgumentNullException("charStrings");
			}
			this.CharStrings = charStrings;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000029C3 File Offset: 0x00000BC3
		public PdfRectangle? GetCharacterBoundingBox(string characterName)
		{
			return PdfSubpath.GetBoundingRectangle(this.GetCharacterPath(characterName));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000029D1 File Offset: 0x00000BD1
		public bool ContainsNamedCharacter(string name)
		{
			return this.CharStrings.CharStrings.ContainsKey(name);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000029E4 File Offset: 0x00000BE4
		private static TransformationMatrix GetFontTransformationMatrix(ArrayToken array)
		{
			if (array == null || array.Data.Count != 6)
			{
				return TransformationMatrix.FromValues(0.001, 0.0, 0.0, 0.001, 0.0, 0.0);
			}
			double @double = ((NumericToken)array.Data[0]).Double;
			double double2 = ((NumericToken)array.Data[1]).Double;
			double double3 = ((NumericToken)array.Data[2]).Double;
			double double4 = ((NumericToken)array.Data[3]).Double;
			double double5 = ((NumericToken)array.Data[4]).Double;
			double double6 = ((NumericToken)array.Data[5]).Double;
			return TransformationMatrix.FromValues(@double, double2, double3, double4, double5, double6);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002AD4 File Offset: 0x00000CD4
		public IReadOnlyList<PdfSubpath> GetCharacterPath(string characterName)
		{
			IReadOnlyList<PdfSubpath> result;
			if (!this.CharStrings.TryGenerate(characterName, out result))
			{
				return null;
			}
			return result;
		}
	}
}
