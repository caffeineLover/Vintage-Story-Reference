using System;
using System.Collections.Generic;
using System.IO;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x0200001A RID: 26
	public static class PdfTextRemover
	{
		// Token: 0x060000CB RID: 203 RVA: 0x00007AA8 File Offset: 0x00005CA8
		public static byte[] RemoveText(string filePath, IReadOnlyList<int> pagesBundle = null)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfTextRemover.RemoveText(memoryStream, filePath, pagesBundle);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00007AE8 File Offset: 0x00005CE8
		public static void RemoveText(Stream output, string filePath, IReadOnlyList<int> pagesBundle = null)
		{
			using (FileStream fileStream = File.OpenRead(filePath))
			{
				PdfTextRemover.RemoveText(fileStream, output, pagesBundle);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00007B20 File Offset: 0x00005D20
		public static byte[] RemoveText(byte[] file, IReadOnlyList<int> pagesBundle = null)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfTextRemover.RemoveText(PdfDocument.Open(file, null), memoryStream, pagesBundle);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00007B74 File Offset: 0x00005D74
		public static void RemoveText(Stream stream, Stream output, IReadOnlyList<int> pagesBundle = null)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			PdfTextRemover.RemoveText(PdfDocument.Open(stream, null), output, pagesBundle);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00007BA0 File Offset: 0x00005DA0
		public static void RemoveText(PdfDocument file, Stream output, IReadOnlyList<int> pagesBundle = null)
		{
			using (PdfDocumentBuilder pdfDocumentBuilder = new PdfDocumentBuilder(output, false, PdfWriterType.Default, file.Version, new NoTextTokenWriter()))
			{
				if (pagesBundle == null)
				{
					for (int i = 1; i <= file.NumberOfPages; i++)
					{
						pdfDocumentBuilder.AddPage(file, i);
					}
				}
				else
				{
					foreach (int pageNumber in pagesBundle)
					{
						pdfDocumentBuilder.AddPage(file, pageNumber);
					}
				}
			}
		}
	}
}
