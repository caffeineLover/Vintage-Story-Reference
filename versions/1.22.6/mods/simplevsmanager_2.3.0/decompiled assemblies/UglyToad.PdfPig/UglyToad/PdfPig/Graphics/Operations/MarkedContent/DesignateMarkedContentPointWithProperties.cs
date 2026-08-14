using System;
using System.IO;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations.MarkedContent
{
	// Token: 0x0200014C RID: 332
	public class DesignateMarkedContentPointWithProperties : IGraphicsStateOperation
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00025066 File Offset: 0x00023266
		public string Operator
		{
			get
			{
				return "DP";
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x0002506D File Offset: 0x0002326D
		public NameToken Name { get; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00025075 File Offset: 0x00023275
		[CanBeNull]
		public NameToken PropertyDictionaryName { get; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0002507D File Offset: 0x0002327D
		[CanBeNull]
		public DictionaryToken Properties { get; }

		// Token: 0x0600078D RID: 1933 RVA: 0x00025085 File Offset: 0x00023285
		public DesignateMarkedContentPointWithProperties(NameToken name, NameToken propertyDictionaryName)
		{
			this.Name = name;
			this.PropertyDictionaryName = propertyDictionaryName;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0002509B File Offset: 0x0002329B
		public DesignateMarkedContentPointWithProperties(NameToken name, DictionaryToken properties)
		{
			this.Name = name;
			this.Properties = properties;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x000250B1 File Offset: 0x000232B1
		public void Run(IOperationContext operationContext)
		{
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000250B4 File Offset: 0x000232B4
		public void Write(Stream stream)
		{
			DesignateMarkedContentPointWithProperties.TokenWriter.WriteToken(this.Name, stream);
			stream.WriteWhiteSpace();
			if (this.PropertyDictionaryName != null)
			{
				DesignateMarkedContentPointWithProperties.TokenWriter.WriteToken(this.PropertyDictionaryName, stream);
			}
			else
			{
				DesignateMarkedContentPointWithProperties.TokenWriter.WriteToken(this.Properties, stream);
			}
			stream.WriteWhiteSpace();
			stream.WriteText("DP", false);
			stream.WriteNewLine();
		}

		// Token: 0x0400039E RID: 926
		private static readonly TokenWriter TokenWriter = new TokenWriter();

		// Token: 0x0400039F RID: 927
		public const string Symbol = "DP";
	}
}
