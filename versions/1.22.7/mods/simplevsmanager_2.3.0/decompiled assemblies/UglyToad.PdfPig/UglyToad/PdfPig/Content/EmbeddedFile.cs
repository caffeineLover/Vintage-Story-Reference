using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E1 RID: 481
	public class EmbeddedFile
	{
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00039A24 File Offset: 0x00037C24
		public string Name { get; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00039A2C File Offset: 0x00037C2C
		public string FileSpecification { get; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x00039A34 File Offset: 0x00037C34
		public IReadOnlyList<byte> Bytes { get; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00039A3C File Offset: 0x00037C3C
		public StreamToken Stream { get; }

		// Token: 0x06000B6F RID: 2927 RVA: 0x00039A44 File Offset: 0x00037C44
		internal EmbeddedFile(string name, string fileSpecification, IReadOnlyList<byte> bytes, StreamToken stream)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
			this.FileSpecification = fileSpecification;
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.Bytes = bytes;
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.Stream = stream;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00039AA4 File Offset: 0x00037CA4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(this.Stream.StreamDictionary);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
