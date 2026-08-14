using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace HtmlAgilityPack
{
	// Token: 0x02000026 RID: 38
	[NullableContext(1)]
	[Nullable(0)]
	public class MixedCodeDocument
	{
		// Token: 0x0600023C RID: 572 RVA: 0x000112B8 File Offset: 0x0000F4B8
		public MixedCodeDocument()
		{
			this._codefragments = new MixedCodeDocumentFragmentList(this);
			this._textfragments = new MixedCodeDocumentFragmentList(this);
			this._fragments = new MixedCodeDocumentFragmentList(this);
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00011328 File Offset: 0x0000F528
		public string Code
		{
			get
			{
				string text = "";
				int num = 0;
				foreach (MixedCodeDocumentFragment mixedCodeDocumentFragment in this._fragments)
				{
					MixedCodeDocumentFragmentType type = mixedCodeDocumentFragment._type;
					if (type != MixedCodeDocumentFragmentType.Code)
					{
						if (type == MixedCodeDocumentFragmentType.Text)
						{
							text = text + this.TokenResponseWrite + string.Format(this.TokenTextBlock, num) + "\n";
							num++;
						}
					}
					else
					{
						text = text + ((MixedCodeDocumentCodeFragment)mixedCodeDocumentFragment).Code + "\n";
					}
				}
				return text;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600023E RID: 574 RVA: 0x000113D4 File Offset: 0x0000F5D4
		public MixedCodeDocumentFragmentList CodeFragments
		{
			get
			{
				return this._codefragments;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600023F RID: 575 RVA: 0x000113DC File Offset: 0x0000F5DC
		public MixedCodeDocumentFragmentList Fragments
		{
			get
			{
				return this._fragments;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000240 RID: 576 RVA: 0x000113E4 File Offset: 0x0000F5E4
		public Encoding StreamEncoding
		{
			get
			{
				return this._streamencoding;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000113EC File Offset: 0x0000F5EC
		public MixedCodeDocumentFragmentList TextFragments
		{
			get
			{
				return this._textfragments;
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000113F4 File Offset: 0x0000F5F4
		public MixedCodeDocumentCodeFragment CreateCodeFragment()
		{
			return (MixedCodeDocumentCodeFragment)this.CreateFragment(MixedCodeDocumentFragmentType.Code);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00011402 File Offset: 0x0000F602
		public MixedCodeDocumentTextFragment CreateTextFragment()
		{
			return (MixedCodeDocumentTextFragment)this.CreateFragment(MixedCodeDocumentFragmentType.Text);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00011410 File Offset: 0x0000F610
		public void Load(Stream stream)
		{
			this.Load(new StreamReader(stream));
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0001141E File Offset: 0x0000F61E
		public void Load(Stream stream, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0001142D File Offset: 0x0000F62D
		public void Load(Stream stream, Encoding encoding)
		{
			this.Load(new StreamReader(stream, encoding));
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0001143C File Offset: 0x0000F63C
		public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0001144C File Offset: 0x0000F64C
		public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			this.Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0001145E File Offset: 0x0000F65E
		public void Load(string path)
		{
			this.Load(new StreamReader(path));
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0001146C File Offset: 0x0000F66C
		public void Load(string path, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(path, detectEncodingFromByteOrderMarks));
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0001147B File Offset: 0x0000F67B
		public void Load(string path, Encoding encoding)
		{
			this.Load(new StreamReader(path, encoding));
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0001148A File Offset: 0x0000F68A
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks));
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0001149A File Offset: 0x0000F69A
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			this.Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize));
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000114AC File Offset: 0x0000F6AC
		public void Load(TextReader reader)
		{
			this._codefragments.Clear();
			this._textfragments.Clear();
			using (StreamReader streamReader = reader as StreamReader)
			{
				if (streamReader != null)
				{
					this._streamencoding = streamReader.CurrentEncoding;
				}
				this._text = reader.ReadToEnd();
			}
			this.Parse();
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00011514 File Offset: 0x0000F714
		public void LoadHtml(string html)
		{
			this.Load(new StringReader(html));
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00011524 File Offset: 0x0000F724
		public void Save(Stream outStream)
		{
			StreamWriter writer = new StreamWriter(outStream, this.GetOutEncoding());
			this.Save(writer);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00011548 File Offset: 0x0000F748
		public void Save(Stream outStream, Encoding encoding)
		{
			StreamWriter writer = new StreamWriter(outStream, encoding);
			this.Save(writer);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00011564 File Offset: 0x0000F764
		public void Save(string filename)
		{
			StreamWriter writer = new StreamWriter(filename, false, this.GetOutEncoding());
			this.Save(writer);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00011588 File Offset: 0x0000F788
		public void Save(string filename, Encoding encoding)
		{
			StreamWriter writer = new StreamWriter(filename, false, encoding);
			this.Save(writer);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000115A5 File Offset: 0x0000F7A5
		public void Save(StreamWriter writer)
		{
			this.Save(writer);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x000115AE File Offset: 0x0000F7AE
		public void Save(TextWriter writer)
		{
			writer.Flush();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x000115B6 File Offset: 0x0000F7B6
		internal MixedCodeDocumentFragment CreateFragment(MixedCodeDocumentFragmentType type)
		{
			if (type == MixedCodeDocumentFragmentType.Code)
			{
				return new MixedCodeDocumentCodeFragment(this);
			}
			if (type == MixedCodeDocumentFragmentType.Text)
			{
				return new MixedCodeDocumentTextFragment(this);
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x000115D2 File Offset: 0x0000F7D2
		internal Encoding GetOutEncoding()
		{
			if (this._streamencoding != null)
			{
				return this._streamencoding;
			}
			return Encoding.UTF8;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000115E8 File Offset: 0x0000F7E8
		private void IncrementPosition()
		{
			this._index++;
			if (this._c == 10)
			{
				this._lineposition = 1;
				this._line++;
				return;
			}
			this._lineposition++;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00011628 File Offset: 0x0000F828
		private void Parse()
		{
			this._state = MixedCodeDocument.ParseState.Text;
			this._index = 0;
			this._currentfragment = this.CreateFragment(MixedCodeDocumentFragmentType.Text);
			while (this._index < this._text.Length)
			{
				this._c = (int)this._text.get_Chars(this._index);
				this.IncrementPosition();
				MixedCodeDocument.ParseState state = this._state;
				if (state != MixedCodeDocument.ParseState.Text)
				{
					if (state == MixedCodeDocument.ParseState.Code)
					{
						if (this._index + this.TokenCodeEnd.Length < this._text.Length && this._text.Substring(this._index - 1, this.TokenCodeEnd.Length) == this.TokenCodeEnd)
						{
							this._state = MixedCodeDocument.ParseState.Text;
							this._currentfragment.Length = this._index + this.TokenCodeEnd.Length - this._currentfragment.Index;
							this._index += this.TokenCodeEnd.Length;
							this._lineposition += this.TokenCodeEnd.Length;
							this._currentfragment = this.CreateFragment(MixedCodeDocumentFragmentType.Text);
							this.SetPosition();
						}
					}
				}
				else if (this._index + this.TokenCodeStart.Length < this._text.Length && this._text.Substring(this._index - 1, this.TokenCodeStart.Length) == this.TokenCodeStart)
				{
					this._state = MixedCodeDocument.ParseState.Code;
					this._currentfragment.Length = this._index - 1 - this._currentfragment.Index;
					this._currentfragment = this.CreateFragment(MixedCodeDocumentFragmentType.Code);
					this.SetPosition();
				}
			}
			this._currentfragment.Length = this._index - this._currentfragment.Index;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0001180C File Offset: 0x0000FA0C
		private void SetPosition()
		{
			this._currentfragment.Line = this._line;
			this._currentfragment._lineposition = this._lineposition;
			this._currentfragment.Index = this._index - 1;
			this._currentfragment.Length = 0;
		}

		// Token: 0x040000C3 RID: 195
		private int _c;

		// Token: 0x040000C4 RID: 196
		internal MixedCodeDocumentFragmentList _codefragments;

		// Token: 0x040000C5 RID: 197
		private MixedCodeDocumentFragment _currentfragment;

		// Token: 0x040000C6 RID: 198
		internal MixedCodeDocumentFragmentList _fragments;

		// Token: 0x040000C7 RID: 199
		private int _index;

		// Token: 0x040000C8 RID: 200
		private int _line;

		// Token: 0x040000C9 RID: 201
		private int _lineposition;

		// Token: 0x040000CA RID: 202
		private MixedCodeDocument.ParseState _state;

		// Token: 0x040000CB RID: 203
		private Encoding _streamencoding;

		// Token: 0x040000CC RID: 204
		internal string _text;

		// Token: 0x040000CD RID: 205
		internal MixedCodeDocumentFragmentList _textfragments;

		// Token: 0x040000CE RID: 206
		public string TokenCodeEnd = "%>";

		// Token: 0x040000CF RID: 207
		public string TokenCodeStart = "<%";

		// Token: 0x040000D0 RID: 208
		public string TokenDirective = "@";

		// Token: 0x040000D1 RID: 209
		public string TokenResponseWrite = "Response.Write ";

		// Token: 0x040000D2 RID: 210
		private string TokenTextBlock = "TextBlock({0})";

		// Token: 0x0200004F RID: 79
		[NullableContext(0)]
		private enum ParseState
		{
			// Token: 0x04000189 RID: 393
			Text,
			// Token: 0x0400018A RID: 394
			Code
		}
	}
}
