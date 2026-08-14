using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x02000014 RID: 20
	internal class PdfDedupStreamWriter : PdfStreamWriter
	{
		// Token: 0x06000060 RID: 96 RVA: 0x00003254 File Offset: 0x00001454
		public PdfDedupStreamWriter(Stream stream, bool dispose, ITokenWriter tokenWriter = null, Action<decimal> recordVersion = null) : base(stream, dispose, tokenWriter, recordVersion)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000327C File Offset: 0x0000147C
		public override IndirectReferenceToken WriteToken(IToken token)
		{
			if (!base.Initialized)
			{
				base.InitializePdf(1.2m);
			}
			this.ms.SetLength(0L);
			this.TokenWriter.WriteToken(token, this.ms);
			byte[] array = this.ms.ToArray();
			IndirectReferenceToken result;
			if (base.AttemptDeduplication && this.hashes.TryGetValue(array, ref result))
			{
				return result;
			}
			IndirectReferenceToken indirectReferenceToken = base.ReserveObjectNumber();
			if (base.AttemptDeduplication)
			{
				this.hashes.Add(array, indirectReferenceToken);
			}
			this.offsets.Add(indirectReferenceToken.Data, base.Stream.Position);
			this.TokenWriter.WriteObject(indirectReferenceToken.Data.ObjectNumber, indirectReferenceToken.Data.Generation, array, base.Stream);
			return indirectReferenceToken;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003350 File Offset: 0x00001550
		public override IndirectReferenceToken WriteToken(IToken token, IndirectReferenceToken indirectReference)
		{
			if (!base.Initialized)
			{
				base.InitializePdf(1.2m);
			}
			this.ms.SetLength(0L);
			this.TokenWriter.WriteToken(token, this.ms);
			byte[] array = this.ms.ToArray();
			this.hashes.Add(array, indirectReference);
			this.offsets.Add(indirectReference.Data, base.Stream.Position);
			this.TokenWriter.WriteObject(indirectReference.Data.ObjectNumber, indirectReference.Data.Generation, array, base.Stream);
			return indirectReference;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000033F9 File Offset: 0x000015F9
		public new void Dispose()
		{
			this.hashes.Clear();
			base.Dispose();
		}

		// Token: 0x0400003E RID: 62
		private readonly Dictionary<byte[], IndirectReferenceToken> hashes = new Dictionary<byte[], IndirectReferenceToken>(new PdfDedupStreamWriter.FNVByteComparison());

		// Token: 0x0400003F RID: 63
		private readonly MemoryStream ms = new MemoryStream();

		// Token: 0x02000226 RID: 550
		private class FNVByteComparison : IEqualityComparer<byte[]>
		{
			// Token: 0x06000CCF RID: 3279 RVA: 0x0003F95C File Offset: 0x0003DB5C
			public bool Equals(byte[] x, byte[] y)
			{
				if (x.Length != y.Length)
				{
					return false;
				}
				for (int i = 0; i < x.Length; i++)
				{
					if (x[i] != y[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06000CD0 RID: 3280 RVA: 0x0003F98C File Offset: 0x0003DB8C
			public int GetHashCode(byte[] obj)
			{
				PdfDedupStreamWriter.FnvHash fnvHash = PdfDedupStreamWriter.FnvHash.Create();
				foreach (byte data in obj)
				{
					fnvHash.Combine(data);
				}
				return fnvHash.HashCode;
			}
		}

		// Token: 0x02000227 RID: 551
		private struct FnvHash
		{
			// Token: 0x170003CE RID: 974
			// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0003F9CA File Offset: 0x0003DBCA
			// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x0003F9D2 File Offset: 0x0003DBD2
			public int HashCode { readonly get; private set; }

			// Token: 0x06000CD4 RID: 3284 RVA: 0x0003F9DC File Offset: 0x0003DBDC
			public static PdfDedupStreamWriter.FnvHash Create()
			{
				return new PdfDedupStreamWriter.FnvHash
				{
					HashCode = -2128831035
				};
			}

			// Token: 0x06000CD5 RID: 3285 RVA: 0x0003F9FE File Offset: 0x0003DBFE
			public void Combine(byte data)
			{
				this.HashCode ^= (int)data;
				this.HashCode *= 16777619;
			}

			// Token: 0x06000CD6 RID: 3286 RVA: 0x0003FA20 File Offset: 0x0003DC20
			public void Combine(int data)
			{
				this.Combine((byte)data);
				this.Combine((byte)(data >> 8));
				this.Combine((byte)(data >> 16));
				this.Combine((byte)(data >> 24));
			}

			// Token: 0x0400076D RID: 1901
			public const int Offset = -2128831035;

			// Token: 0x0400076E RID: 1902
			private const int Prime = 16777619;
		}
	}
}
