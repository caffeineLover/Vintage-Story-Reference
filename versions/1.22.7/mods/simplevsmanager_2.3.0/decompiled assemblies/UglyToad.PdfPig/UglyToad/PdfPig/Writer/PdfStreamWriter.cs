using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x02000019 RID: 25
	internal class PdfStreamWriter : IPdfStreamWriter, IDisposable
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00007784 File Offset: 0x00005984
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x0000778C File Offset: 0x0000598C
		protected bool DisposeStream { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00007795 File Offset: 0x00005995
		// (set) Token: 0x060000BB RID: 187 RVA: 0x0000779D File Offset: 0x0000599D
		protected bool Initialized { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000077A6 File Offset: 0x000059A6
		// (set) Token: 0x060000BD RID: 189 RVA: 0x000077AE File Offset: 0x000059AE
		protected int CurrentNumber { get; set; } = 1;

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000077B7 File Offset: 0x000059B7
		// (set) Token: 0x060000BF RID: 191 RVA: 0x000077BF File Offset: 0x000059BF
		public Stream Stream { get; protected set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000077C8 File Offset: 0x000059C8
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x000077D0 File Offset: 0x000059D0
		public bool AttemptDeduplication { get; set; } = true;

		// Token: 0x060000C2 RID: 194 RVA: 0x000077DC File Offset: 0x000059DC
		internal PdfStreamWriter(Stream baseStream, bool disposeStream = true, ITokenWriter tokenWriter = null, Action<decimal> recordVersion = null)
		{
			if (baseStream == null)
			{
				throw new ArgumentNullException("baseStream");
			}
			this.Stream = baseStream;
			if (!baseStream.CanWrite)
			{
				throw new ArgumentException("Output stream must be writable");
			}
			this.recordVersion = recordVersion;
			this.DisposeStream = disposeStream;
			this.TokenWriter = (tokenWriter ?? new TokenWriter());
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00007850 File Offset: 0x00005A50
		public virtual IndirectReferenceToken WriteToken(IToken token)
		{
			if (!this.Initialized)
			{
				this.InitializePdf(1.2m);
			}
			IndirectReferenceToken indirectReferenceToken = this.ReserveObjectNumber();
			this.offsets.Add(indirectReferenceToken.Data, this.Stream.Position);
			ObjectToken token2 = new ObjectToken(this.Stream.Position, indirectReferenceToken.Data, token);
			this.TokenWriter.WriteToken(token2, this.Stream);
			return indirectReferenceToken;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000078C4 File Offset: 0x00005AC4
		public virtual IndirectReferenceToken WriteToken(IToken token, IndirectReferenceToken indirectReference)
		{
			if (!this.Initialized)
			{
				this.InitializePdf(1.2m);
			}
			this.offsets.Add(indirectReference.Data, this.Stream.Position);
			ObjectToken token2 = new ObjectToken(this.Stream.Position, indirectReference.Data, token);
			this.TokenWriter.WriteToken(token2, this.Stream);
			return indirectReference;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00007934 File Offset: 0x00005B34
		public IndirectReferenceToken ReserveObjectNumber()
		{
			int currentNumber = this.CurrentNumber;
			this.CurrentNumber = currentNumber + 1;
			return new IndirectReferenceToken(new IndirectReference((long)currentNumber, 0));
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00007960 File Offset: 0x00005B60
		public void InitializePdf(decimal version)
		{
			Action<decimal> action = this.recordVersion;
			if (action != null)
			{
				action.Invoke(version);
			}
			PdfStreamWriter.WriteString("%PDF-" + version.ToString("0.0", CultureInfo.InvariantCulture), this.Stream);
			this.Stream.WriteText("%", false);
			this.Stream.WriteByte(169);
			this.Stream.WriteByte(205);
			this.Stream.WriteByte(196);
			this.Stream.WriteByte(210);
			this.Stream.WriteNewLine();
			this.Initialized = true;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00007A08 File Offset: 0x00005C08
		public void CompletePdf(IndirectReferenceToken catalogReference, IndirectReferenceToken documentInformationReference = null)
		{
			this.TokenWriter.WriteCrossReferenceTable(this.offsets, catalogReference.Data, this.Stream, (documentInformationReference != null) ? new IndirectReference?(documentInformationReference.Data) : default(IndirectReference?));
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00007A4C File Offset: 0x00005C4C
		private static void WriteString(string text, Stream stream)
		{
			byte[] array = OtherEncodings.StringAsLatin1Bytes(text);
			stream.Write(array, 0, array.Length);
			stream.WriteNewLine();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00007A71 File Offset: 0x00005C71
		public void Dispose()
		{
			if (this.DisposeStream)
			{
				Stream stream = this.Stream;
				if (stream != null)
				{
					stream.Dispose();
				}
			}
			this.Stream = null;
		}

		// Token: 0x0400005F RID: 95
		private readonly Action<decimal> recordVersion;

		// Token: 0x04000060 RID: 96
		protected const decimal DefaultVersion = 1.2m;

		// Token: 0x04000061 RID: 97
		protected Dictionary<IndirectReference, long> offsets = new Dictionary<IndirectReference, long>();

		// Token: 0x04000065 RID: 101
		protected readonly ITokenWriter TokenWriter;
	}
}
