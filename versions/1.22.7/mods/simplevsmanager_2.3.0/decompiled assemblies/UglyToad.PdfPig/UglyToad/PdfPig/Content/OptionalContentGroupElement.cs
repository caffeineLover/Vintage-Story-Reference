using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001EB RID: 491
	public class OptionalContentGroupElement
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x0003A4E5 File Offset: 0x000386E5
		public string Type { get; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0003A4ED File Offset: 0x000386ED
		public string Name { get; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x0003A4F5 File Offset: 0x000386F5
		public IReadOnlyList<string> Intent { get; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x0003A4FD File Offset: 0x000386FD
		public IReadOnlyDictionary<string, IToken> Usage { get; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x0003A505 File Offset: 0x00038705
		public MarkedContentElement MarkedContent { get; }

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0003A510 File Offset: 0x00038710
		internal OptionalContentGroupElement(MarkedContentElement markedContentElement, IPdfTokenScanner pdfTokenScanner)
		{
			this.MarkedContent = markedContentElement;
			NameToken nameToken;
			if (markedContentElement.Properties.TryGet(NameToken.Type, pdfTokenScanner, out nameToken))
			{
				this.Type = nameToken.Data;
			}
			else
			{
				StringToken stringToken;
				if (!markedContentElement.Properties.TryGet(NameToken.Type, pdfTokenScanner, out stringToken))
				{
					throw new ArgumentException("Cannot parse optional content's Type from Properties. This is a required field.", "Properties");
				}
				this.Type = stringToken.Data;
			}
			string type = this.Type;
			if (!(type == "OCG"))
			{
				if (!(type == "OCMD"))
				{
					throw new ArgumentException("Unknown Optional Content of type '" + this.Type + "' not known.", "Type");
				}
				DictionaryToken dictionaryToken;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (markedContentElement.Properties.TryGet(NameToken.Ocgs, pdfTokenScanner, out dictionaryToken))
				{
					defaultInterpolatedStringHandler..ctor(0, 1);
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Ocgs);
					throw new NotImplementedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ArrayToken arrayToken;
				if (markedContentElement.Properties.TryGet(NameToken.Ocgs, pdfTokenScanner, out arrayToken))
				{
					defaultInterpolatedStringHandler..ctor(0, 1);
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Ocgs);
					throw new NotImplementedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				NameToken nameToken2;
				if (markedContentElement.Properties.TryGet(NameToken.P, pdfTokenScanner, out nameToken2))
				{
					defaultInterpolatedStringHandler..ctor(0, 1);
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.P);
					throw new NotImplementedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ArrayToken arrayToken2;
				if (markedContentElement.Properties.TryGet(NameToken.VE, pdfTokenScanner, out arrayToken2))
				{
					defaultInterpolatedStringHandler..ctor(0, 1);
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.VE);
					throw new NotImplementedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			else
			{
				NameToken nameToken3;
				if (markedContentElement.Properties.TryGet(NameToken.Name, pdfTokenScanner, out nameToken3))
				{
					this.Name = nameToken3.Data;
				}
				else
				{
					StringToken stringToken2;
					if (!markedContentElement.Properties.TryGet(NameToken.Name, pdfTokenScanner, out stringToken2))
					{
						throw new ArgumentException("Cannot parse optional content's Name from Properties. This is a required field.", "Properties");
					}
					this.Name = stringToken2.Data;
				}
				NameToken nameToken4;
				StringToken stringToken3;
				ArrayToken arrayToken3;
				if (markedContentElement.Properties.TryGet(NameToken.Intent, pdfTokenScanner, out nameToken4))
				{
					this.Intent = new string[]
					{
						nameToken4.Data
					};
				}
				else if (markedContentElement.Properties.TryGet(NameToken.Intent, pdfTokenScanner, out stringToken3))
				{
					this.Intent = new string[]
					{
						stringToken3.Data
					};
				}
				else if (markedContentElement.Properties.TryGet(NameToken.Intent, pdfTokenScanner, out arrayToken3))
				{
					List<string> list = new List<string>();
					foreach (IToken token in arrayToken3.Data)
					{
						NameToken nameToken5 = token as NameToken;
						if (nameToken5 != null)
						{
							list.Add(nameToken5.Data);
						}
						else
						{
							StringToken stringToken4 = token as StringToken;
							if (stringToken4 == null)
							{
								throw new NotImplementedException();
							}
							list.Add(stringToken4.Data);
						}
					}
					this.Intent = list;
				}
				else
				{
					this.Intent = new string[]
					{
						"View"
					};
				}
				DictionaryToken dictionaryToken2;
				if (markedContentElement.Properties.TryGet(NameToken.Usage, pdfTokenScanner, out dictionaryToken2))
				{
					this.Usage = dictionaryToken2.Data;
					return;
				}
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0003A844 File Offset: 0x00038A44
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(8, 4);
			defaultInterpolatedStringHandler.AppendFormatted(this.Type);
			defaultInterpolatedStringHandler.AppendLiteral(" - ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" [");
			defaultInterpolatedStringHandler.AppendFormatted(string.Join(",", this.Intent));
			defaultInterpolatedStringHandler.AppendLiteral("]: ");
			MarkedContentElement markedContent = this.MarkedContent;
			defaultInterpolatedStringHandler.AppendFormatted((markedContent != null) ? markedContent.ToString() : null);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
