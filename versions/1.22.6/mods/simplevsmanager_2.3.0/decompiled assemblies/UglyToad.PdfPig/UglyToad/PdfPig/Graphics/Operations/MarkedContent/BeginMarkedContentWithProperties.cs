using System;
using System.IO;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations.MarkedContent
{
	// Token: 0x0200014A RID: 330
	public class BeginMarkedContentWithProperties : IGraphicsStateOperation
	{
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00024EEA File Offset: 0x000230EA
		public string Operator
		{
			get
			{
				return "BDC";
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00024EF1 File Offset: 0x000230F1
		public NameToken Name { get; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x00024EF9 File Offset: 0x000230F9
		[CanBeNull]
		public NameToken PropertyDictionaryName { get; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x00024F01 File Offset: 0x00023101
		[CanBeNull]
		public DictionaryToken Properties { get; }

		// Token: 0x0600077D RID: 1917 RVA: 0x00024F09 File Offset: 0x00023109
		public BeginMarkedContentWithProperties(NameToken name, NameToken propertyDictionaryName)
		{
			this.Name = name;
			this.PropertyDictionaryName = propertyDictionaryName;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00024F1F File Offset: 0x0002311F
		public BeginMarkedContentWithProperties(NameToken name, DictionaryToken properties)
		{
			this.Name = name;
			this.Properties = properties;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00024F35 File Offset: 0x00023135
		public void Run(IOperationContext operationContext)
		{
			operationContext.BeginMarkedContent(this.Name, this.PropertyDictionaryName, this.Properties);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00024F50 File Offset: 0x00023150
		public void Write(Stream stream)
		{
			BeginMarkedContentWithProperties.TokenWriter.WriteToken(this.Name, stream);
			stream.WriteWhiteSpace();
			if (this.PropertyDictionaryName != null)
			{
				BeginMarkedContentWithProperties.TokenWriter.WriteToken(this.PropertyDictionaryName, stream);
			}
			else
			{
				BeginMarkedContentWithProperties.TokenWriter.WriteToken(this.Properties, stream);
			}
			stream.WriteWhiteSpace();
			stream.WriteText("BDC", false);
			stream.WriteNewLine();
		}

		// Token: 0x04000396 RID: 918
		private static readonly TokenWriter TokenWriter = new TokenWriter();

		// Token: 0x04000397 RID: 919
		public const string Symbol = "BDC";
	}
}
