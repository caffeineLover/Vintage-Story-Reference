using System;
using System.Collections.Generic;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000002 RID: 2
	public class ArrayToken : IDataToken<IReadOnlyList<IToken>>, IToken, IEquatable<IToken>
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public IReadOnlyList<IToken> Data { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public int Length { get; }

		// Token: 0x17000003 RID: 3
		public IToken this[int i]
		{
			get
			{
				return this.Data[i];
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002070 File Offset: 0x00000270
		public ArrayToken(IReadOnlyList<IToken> data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			List<IToken> list = new List<IToken>(data.Count);
			int i = 0;
			while (i < data.Count)
			{
				IToken token = data[i];
				if (i < 2 || token != OperatorToken.R)
				{
					goto IL_8C;
				}
				NumericToken numericToken = data[i - 1] as NumericToken;
				if (numericToken == null)
				{
					goto IL_8C;
				}
				NumericToken numericToken2 = data[i - 2] as NumericToken;
				if (numericToken2 == null)
				{
					goto IL_8C;
				}
				list.RemoveRange(list.Count - 2, 2);
				list.Add(new IndirectReferenceToken(new IndirectReference(numericToken2.Long, numericToken.Int)));
				IL_93:
				i++;
				continue;
				IL_8C:
				list.Add(token);
				goto IL_93;
			}
			this.Data = list;
			this.Length = this.Data.Count;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002138 File Offset: 0x00000338
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[ ");
			for (int i = 0; i < this.Data.Count; i++)
			{
				IToken token = this.Data[i];
				stringBuilder.Append(token);
				if (i < this.Data.Count - 1)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021AC File Offset: 0x000003AC
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			ArrayToken arrayToken = obj as ArrayToken;
			if (arrayToken == null)
			{
				return false;
			}
			if (arrayToken.Length != this.Length)
			{
				return false;
			}
			for (int i = 0; i < this.Length; i++)
			{
				if (!this.Data[i].Equals(arrayToken[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
