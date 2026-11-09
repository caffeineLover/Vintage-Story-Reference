using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Actions;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline.Destinations;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x02000016 RID: 22
	public static class PdfMerger
	{
		// Token: 0x06000086 RID: 134 RVA: 0x000053CC File Offset: 0x000035CC
		public static byte[] Merge(string file1, string file2, IReadOnlyList<int> file1Selection = null, IReadOnlyList<int> file2Selection = null, PdfAStandard archiveStandard = PdfAStandard.None, PdfDocumentBuilder.DocumentInformationBuilder docInfoBuilder = null)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfMerger.Merge(file1, file2, memoryStream, file1Selection, file2Selection, archiveStandard, docInfoBuilder);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005414 File Offset: 0x00003614
		public static void Merge(string file1, string file2, Stream output, IReadOnlyList<int> file1Selection = null, IReadOnlyList<int> file2Selection = null, PdfAStandard archiveStandard = PdfAStandard.None, PdfDocumentBuilder.DocumentInformationBuilder docInfoBuilder = null)
		{
			if (file1 == null)
			{
				throw new ArgumentNullException("file1");
			}
			if (file2 == null)
			{
				throw new ArgumentNullException("file2");
			}
			using (FileStream fileStream = File.OpenRead(file1))
			{
				using (FileStream fileStream2 = File.OpenRead(file2))
				{
					PdfMerger.Merge(new FileStream[]
					{
						fileStream,
						fileStream2
					}, output, new IReadOnlyList<int>[]
					{
						file1Selection,
						file2Selection
					}, archiveStandard, docInfoBuilder);
				}
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000054A4 File Offset: 0x000036A4
		public static byte[] Merge(params string[] filePaths)
		{
			return PdfMerger.Merge(PdfAStandard.None, null, filePaths);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000054B0 File Offset: 0x000036B0
		public static byte[] Merge(PdfAStandard archiveStandard, PdfDocumentBuilder.DocumentInformationBuilder docInfoBuilder, params string[] filePaths)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfMerger.Merge(memoryStream, archiveStandard, docInfoBuilder, filePaths);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000054F0 File Offset: 0x000036F0
		public static void Merge(Stream output, params string[] filePaths)
		{
			PdfMerger.Merge(output, PdfAStandard.None, null, filePaths);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000054FC File Offset: 0x000036FC
		public static void Merge(Stream output, PdfAStandard archiveStandard, PdfDocumentBuilder.DocumentInformationBuilder docInfoBuilder, params string[] filePaths)
		{
			List<Stream> list = new List<Stream>(filePaths.Length);
			try
			{
				for (int i = 0; i < filePaths.Length; i++)
				{
					string text = filePaths[i];
					if (text == null)
					{
						string text2 = "filePaths";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Null filepath at index ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(i);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new ArgumentNullException(text2, defaultInterpolatedStringHandler.ToStringAndClear());
					}
					string text3 = text;
					list.Add(File.OpenRead(text3));
				}
				PdfMerger.Merge(list, output, null, archiveStandard, docInfoBuilder);
			}
			finally
			{
				foreach (Stream stream in list)
				{
					stream.Dispose();
				}
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000055C8 File Offset: 0x000037C8
		public static byte[] Merge(IReadOnlyList<byte[]> files, IReadOnlyList<IReadOnlyList<int>> pagesBundle = null, PdfAStandard archiveStandard = PdfAStandard.None, PdfDocumentBuilder.DocumentInformationBuilder docInfoBuilder = null)
		{
			if (files == null)
			{
				throw new ArgumentNullException("files");
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfMerger.Merge(Enumerable.ToArray<PdfDocument>(Enumerable.Select<byte[], PdfDocument>(files, (byte[] f) => PdfDocument.Open(f, null))), memoryStream, pagesBundle, archiveStandard, docInfoBuilder);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005640 File Offset: 0x00003840
		public static void Merge(IReadOnlyList<Stream> streams, Stream output, IReadOnlyList<IReadOnlyList<int>> pagesBundle = null, PdfAStandard archiveStandard = PdfAStandard.None, PdfDocumentBuilder.DocumentInformationBuilder docInfoBuilder = null)
		{
			if (streams == null)
			{
				throw new ArgumentNullException("streams");
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			PdfMerger.Merge(Enumerable.ToArray<PdfDocument>(Enumerable.Select<Stream, PdfDocument>(streams, (Stream f) => PdfDocument.Open(f, null))), output, pagesBundle, archiveStandard, docInfoBuilder);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000056A0 File Offset: 0x000038A0
		private static void Merge(IReadOnlyList<PdfDocument> files, Stream output, IReadOnlyList<IReadOnlyList<int>> pagesBundle, PdfAStandard archiveStandard = PdfAStandard.None, PdfDocumentBuilder.DocumentInformationBuilder docInfoBuilder = null)
		{
			decimal version = Enumerable.Max(Enumerable.Select<PdfDocument, decimal>(files, (PdfDocument x) => x.Version));
			using (PdfDocumentBuilder pdfDocumentBuilder = new PdfDocumentBuilder(output, false, PdfWriterType.Default, version, null))
			{
				pdfDocumentBuilder.ArchiveStandard = archiveStandard;
				if (docInfoBuilder != null)
				{
					pdfDocumentBuilder.IncludeDocumentInformation = true;
					pdfDocumentBuilder.DocumentInformation = docInfoBuilder;
				}
				foreach (int num in Enumerable.Range(0, files.Count))
				{
					PdfDocument pdfDocument = files[num];
					IReadOnlyList<int> readOnlyList = null;
					if (pagesBundle != null && num < pagesBundle.Count)
					{
						readOnlyList = pagesBundle[num];
					}
					int basePageNumber = pdfDocumentBuilder.Pages.Count;
					if (readOnlyList == null)
					{
						Func<int, int?> <>9__3;
						Func<PdfAction, PdfAction> <>9__2;
						for (int i = 1; i <= pdfDocument.NumberOfPages; i++)
						{
							PdfDocumentBuilder pdfDocumentBuilder2 = pdfDocumentBuilder;
							PdfDocument document = pdfDocument;
							int pageNumber = i;
							Func<PdfAction, PdfAction> copyLink;
							if ((copyLink = <>9__2) == null)
							{
								copyLink = (<>9__2 = delegate(PdfAction link)
								{
									Func<int, int?> getPageNumber;
									if ((getPageNumber = <>9__3) == null)
									{
										getPageNumber = (<>9__3 = ((int n) => new int?(basePageNumber + n)));
									}
									return PdfMerger.<Merge>g__CopyLink|10_1(link, getPageNumber);
								});
							}
							pdfDocumentBuilder2.AddPage(document, pageNumber, copyLink);
						}
					}
					else
					{
						Dictionary<int, int> pageNumbers = new Dictionary<int, int>();
						for (int j = 0; j < readOnlyList.Count; j++)
						{
							pageNumbers[readOnlyList[j]] = basePageNumber + j + 1;
						}
						Func<int, int?> <>9__5;
						Func<PdfAction, PdfAction> <>9__4;
						foreach (int num2 in readOnlyList)
						{
							PdfDocumentBuilder pdfDocumentBuilder3 = pdfDocumentBuilder;
							PdfDocument document2 = pdfDocument;
							int pageNumber2 = num2;
							Func<PdfAction, PdfAction> copyLink2;
							if ((copyLink2 = <>9__4) == null)
							{
								copyLink2 = (<>9__4 = delegate(PdfAction link)
								{
									Func<int, int?> getPageNumber;
									if ((getPageNumber = <>9__5) == null)
									{
										getPageNumber = (<>9__5 = delegate(int n)
										{
											int num3;
											if (!pageNumbers.TryGetValue(n, ref num3))
											{
												return default(int?);
											}
											return new int?(num3);
										});
									}
									return PdfMerger.<Merge>g__CopyLink|10_1(link, getPageNumber);
								});
							}
							pdfDocumentBuilder3.AddPage(document2, pageNumber2, copyLink2);
						}
					}
				}
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000058CC File Offset: 0x00003ACC
		[CompilerGenerated]
		internal static PdfAction <Merge>g__CopyLink|10_1(PdfAction action, Func<int, int?> getPageNumber)
		{
			AbstractGoToAction abstractGoToAction = action as AbstractGoToAction;
			if (abstractGoToAction == null)
			{
				return action;
			}
			int? num = getPageNumber.Invoke(abstractGoToAction.Destination.PageNumber);
			if (num == null)
			{
				return null;
			}
			ExplicitDestination destination = new ExplicitDestination(num.Value, abstractGoToAction.Destination.Type, abstractGoToAction.Destination.Coordinates);
			if (action is GoToAction)
			{
				return new GoToAction(destination);
			}
			GoToEAction goToEAction = action as GoToEAction;
			if (goToEAction != null)
			{
				return new GoToEAction(destination, goToEAction.FileSpecification);
			}
			GoToRAction goToRAction = action as GoToRAction;
			if (goToRAction == null)
			{
				return action;
			}
			return new GoToRAction(destination, goToRAction.Filename);
		}

		// Token: 0x0400004D RID: 77
		private static readonly ILog Log = new NoOpLog();

		// Token: 0x0400004E RID: 78
		private static readonly IFilterProvider FilterProvider = DefaultFilterProvider.Instance;
	}
}
