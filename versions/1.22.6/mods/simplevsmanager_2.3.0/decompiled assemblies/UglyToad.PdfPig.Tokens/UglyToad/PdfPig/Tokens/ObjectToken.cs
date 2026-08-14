using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x0200000F RID: 15
	public class ObjectToken : IDataToken<IToken>, IToken, IEquatable<IToken>
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00004E0F File Offset: 0x0000300F
		public long Position { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00004E17 File Offset: 0x00003017
		public IndirectReference Number { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00004E1F File Offset: 0x0000301F
		public IToken Data { get; }

		// Token: 0x06000057 RID: 87 RVA: 0x00004E27 File Offset: 0x00003027
		public ObjectToken(long position, IndirectReference number, IToken data)
		{
			this.Position = position;
			this.Number = number;
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.Data = data;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004E54 File Offset: 0x00003054
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			ObjectToken objectToken = obj as ObjectToken;
			return objectToken != null && this.Number.Equals(objectToken.Number) && this.Data.Equals(objectToken.Data);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004EA8 File Offset: 0x000030A8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(28, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Number: ");
			defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(this.Number);
			defaultInterpolatedStringHandler.AppendLiteral(", Position: ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.Position);
			defaultInterpolatedStringHandler.AppendLiteral(", Type: ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Data.GetType().Name);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
