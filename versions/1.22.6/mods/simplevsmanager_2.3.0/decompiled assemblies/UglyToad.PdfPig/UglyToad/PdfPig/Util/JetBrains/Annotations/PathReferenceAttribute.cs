using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200004D RID: 77
	[AttributeUsage(2048)]
	internal sealed class PathReferenceAttribute : Attribute
	{
		// Token: 0x06000197 RID: 407 RVA: 0x0000E1CC File Offset: 0x0000C3CC
		public PathReferenceAttribute()
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000E1D4 File Offset: 0x0000C3D4
		public PathReferenceAttribute([NotNull] [PathReference] string basePath)
		{
			this.BasePath = basePath;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000E1E3 File Offset: 0x0000C3E3
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000E1EB File Offset: 0x0000C3EB
		[CanBeNull]
		public string BasePath { get; private set; }
	}
}
