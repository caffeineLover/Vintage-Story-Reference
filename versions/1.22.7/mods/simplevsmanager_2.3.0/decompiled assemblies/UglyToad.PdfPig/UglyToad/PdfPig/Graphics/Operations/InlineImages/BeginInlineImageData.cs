using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations.InlineImages
{
	// Token: 0x0200014F RID: 335
	public class BeginInlineImageData : IGraphicsStateOperation
	{
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x000251AA File Offset: 0x000233AA
		public string Operator
		{
			get
			{
				return "ID";
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x000251B1 File Offset: 0x000233B1
		public IReadOnlyDictionary<NameToken, IToken> Dictionary { get; }

		// Token: 0x060007A0 RID: 1952 RVA: 0x000251B9 File Offset: 0x000233B9
		public BeginInlineImageData(IReadOnlyDictionary<NameToken, IToken> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.Dictionary = dictionary;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x000251D7 File Offset: 0x000233D7
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetInlineImageProperties(this.Dictionary);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x000251E8 File Offset: 0x000233E8
		public void Write(Stream stream)
		{
			TokenWriter instance = TokenWriter.Instance;
			foreach (KeyValuePair<NameToken, IToken> keyValuePair in this.Dictionary)
			{
				NameToken key = keyValuePair.Key;
				IToken value = keyValuePair.Value;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(key);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				stream.WriteText(defaultInterpolatedStringHandler.ToStringAndClear(), false);
				instance.WriteToken(value, stream);
				stream.WriteNewLine();
			}
			stream.WriteText("ID", false);
			stream.WriteNewLine();
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00025294 File Offset: 0x00023494
		public override string ToString()
		{
			return "ID";
		}

		// Token: 0x040003A7 RID: 935
		public const string Symbol = "ID";
	}
}
