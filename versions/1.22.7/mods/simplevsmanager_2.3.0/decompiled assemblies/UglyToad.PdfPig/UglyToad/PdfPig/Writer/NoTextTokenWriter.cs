using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Parser;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x02000010 RID: 16
	internal class NoTextTokenWriter : TokenWriter
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00002FFC File Offset: 0x000011FC
		protected override void WriteStream(StreamToken streamToken, Stream outputStream)
		{
			StreamToken streamToken2;
			if (!this.TryGetStreamWithoutText(streamToken, out streamToken2))
			{
				streamToken2 = streamToken;
			}
			base.WriteDictionary(streamToken2.StreamDictionary, outputStream);
			this.WriteLineBreak(outputStream);
			outputStream.Write(TokenWriter.StreamStart, 0, TokenWriter.StreamStart.Length);
			this.WriteLineBreak(outputStream);
			outputStream.Write(Enumerable.ToArray<byte>(streamToken2.Data), 0, streamToken2.Data.Count);
			this.WriteLineBreak(outputStream);
			outputStream.Write(TokenWriter.StreamEnd, 0, TokenWriter.StreamEnd.Length);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000307C File Offset: 0x0000127C
		private bool TryGetStreamWithoutText(StreamToken streamToken, out StreamToken outputStreamToken)
		{
			FilterProviderWithLookup filterProvider = new FilterProviderWithLookup(DefaultFilterProvider.Instance);
			IReadOnlyList<byte> readOnlyList;
			try
			{
				readOnlyList = streamToken.Decode(filterProvider);
			}
			catch
			{
				outputStreamToken = null;
				return false;
			}
			PageContentParser pageContentParser = new PageContentParser(new ReflectionGraphicsStateOperationFactory());
			IReadOnlyList<IGraphicsStateOperation> readOnlyList2;
			try
			{
				readOnlyList2 = pageContentParser.Parse(1, new ByteArrayInputBytes(readOnlyList), new NoOpLog());
			}
			catch (Exception)
			{
				outputStreamToken = null;
				return false;
			}
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bool flag = false;
				foreach (IGraphicsStateOperation graphicsStateOperation in readOnlyList2)
				{
					if (graphicsStateOperation.Operator == "Tj" || graphicsStateOperation.Operator == "TJ")
					{
						flag = true;
					}
					else
					{
						graphicsStateOperation.Write(memoryStream);
					}
				}
				if (!flag)
				{
					outputStreamToken = null;
					result = false;
				}
				else
				{
					memoryStream.Seek(0L, 0);
					outputStreamToken = DataCompresser.CompressToStream(memoryStream.ToArray());
					result = true;
				}
			}
			return result;
		}
	}
}
