using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;
using UglyToad.PdfPig.XObjects;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x02000110 RID: 272
	public class XObjectContentRecord
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00022008 File Offset: 0x00020208
		public XObjectType Type { get; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00022010 File Offset: 0x00020210
		[NotNull]
		public StreamToken Stream { get; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00022018 File Offset: 0x00020218
		public TransformationMatrix AppliedTransformation { get; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00022020 File Offset: 0x00020220
		public RenderingIntent DefaultRenderingIntent { get; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x00022028 File Offset: 0x00020228
		public ColorSpaceDetails DefaultColorSpace { get; }

		// Token: 0x060005EE RID: 1518 RVA: 0x00022030 File Offset: 0x00020230
		internal XObjectContentRecord(XObjectType type, StreamToken stream, TransformationMatrix appliedTransformation, RenderingIntent defaultRenderingIntent, ColorSpaceDetails defaultColorSpace)
		{
			this.Type = type;
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.Stream = stream;
			this.AppliedTransformation = appliedTransformation;
			this.DefaultRenderingIntent = defaultRenderingIntent;
			this.DefaultColorSpace = defaultColorSpace;
		}
	}
}
