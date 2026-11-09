using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Graphics.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Operations.General
{
	// Token: 0x02000151 RID: 337
	public class SetColorRenderingIntent : IGraphicsStateOperation
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x000252F1 File Offset: 0x000234F1
		public string Operator
		{
			get
			{
				return "ri";
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x000252F8 File Offset: 0x000234F8
		public NameToken RenderingIntent { get; }

		// Token: 0x060007AC RID: 1964 RVA: 0x00025300 File Offset: 0x00023500
		public SetColorRenderingIntent(NameToken renderingIntent)
		{
			if (renderingIntent == null)
			{
				throw new ArgumentNullException("renderingIntent");
			}
			this.RenderingIntent = renderingIntent;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0002531E File Offset: 0x0002351E
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().RenderingIntent = this.RenderingIntent.ToRenderingIntent();
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0002533B File Offset: 0x0002353B
		public void Write(Stream stream)
		{
			stream.WriteText("/" + this.RenderingIntent.Data + " ri", false);
			stream.WriteNewLine();
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00025364 File Offset: 0x00023564
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.RenderingIntent);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("ri");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040003AB RID: 939
		public const string Symbol = "ri";
	}
}
