using System;

namespace Xabe.FFmpeg
{
	// Token: 0x0200001B RID: 27
	public class Conversions
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00004D3F File Offset: 0x00002F3F
		public IConversion New()
		{
			return Conversion.New();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004D46 File Offset: 0x00002F46
		internal Conversions()
		{
		}

		// Token: 0x04000290 RID: 656
		public readonly Snippets FromSnippet = new Snippets();
	}
}
