using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.Type1.Parser
{
	// Token: 0x02000010 RID: 16
	internal class Type1DataToken : Type1Token
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00004CF4 File Offset: 0x00002EF4
		public IReadOnlyList<byte> Data { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00004CFC File Offset: 0x00002EFC
		public override bool IsPrivateDictionary { get; }

		// Token: 0x06000069 RID: 105 RVA: 0x00004D04 File Offset: 0x00002F04
		public Type1DataToken(Type1Token.TokenType type, IReadOnlyList<byte> data) : base(string.Empty, type)
		{
			if (type != Type1Token.TokenType.Charstring)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(79, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid token type for type 1 token receiving bytes, expected Charstring, got ");
				defaultInterpolatedStringHandler.AppendFormatted<Type1Token.TokenType>(type);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Data = data;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004D60 File Offset: 0x00002F60
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(29, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Token[type = ");
			defaultInterpolatedStringHandler.AppendFormatted<Type1Token.TokenType>(base.Type);
			defaultInterpolatedStringHandler.AppendLiteral(", data = ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Data.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" bytes]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
