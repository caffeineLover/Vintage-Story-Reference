using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000005 RID: 5
	public class DictionaryToken : IDataToken<IReadOnlyDictionary<string, IToken>>, IToken, IEquatable<IToken>, IEquatable<DictionaryToken>
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002308 File Offset: 0x00000508
		public IReadOnlyDictionary<string, IToken> Data { get; }

		// Token: 0x06000014 RID: 20 RVA: 0x00002310 File Offset: 0x00000510
		public DictionaryToken(IReadOnlyDictionary<NameToken, IToken> data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			Dictionary<string, IToken> dictionary = new Dictionary<string, IToken>(data.Count);
			foreach (KeyValuePair<NameToken, IToken> keyValuePair in data)
			{
				dictionary[keyValuePair.Key.Data] = keyValuePair.Value;
			}
			this.Data = dictionary;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002394 File Offset: 0x00000594
		private DictionaryToken(IReadOnlyDictionary<string, IToken> data)
		{
			this.Data = data;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023A3 File Offset: 0x000005A3
		public bool TryGet(NameToken name, out IToken token)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.Data.TryGetValue(name.Data, ref token);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023CC File Offset: 0x000005CC
		public bool TryGet<T>(NameToken name, out T token) where T : IToken
		{
			token = default(T);
			IToken token2;
			if (this.TryGet(name, out token2) && token2 is T)
			{
				T t = (T)((object)token2);
				token = t;
				return true;
			}
			return false;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002406 File Offset: 0x00000606
		public bool ContainsKey(NameToken name)
		{
			return this.Data.ContainsKey(name.Data);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002419 File Offset: 0x00000619
		public DictionaryToken With(NameToken key, IToken value)
		{
			return this.With(key.Data, value);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002428 File Offset: 0x00000628
		public DictionaryToken With(string key, IToken value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Dictionary<string, IToken> dictionary = new Dictionary<string, IToken>(this.Data.Count + 1);
			foreach (KeyValuePair<string, IToken> keyValuePair in this.Data)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
			dictionary[key] = value;
			return new DictionaryToken(dictionary);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024C0 File Offset: 0x000006C0
		public DictionaryToken Without(NameToken key)
		{
			return this.Without(key.Data);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000024D0 File Offset: 0x000006D0
		public DictionaryToken Without(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			Dictionary<string, IToken> dictionary = new Dictionary<string, IToken>(this.Data.ContainsKey(key) ? (this.Data.Count - 1) : this.Data.Count);
			IEnumerable<KeyValuePair<string, IToken>> data = this.Data;
			Func<KeyValuePair<string, IToken>, bool> <>9__0;
			Func<KeyValuePair<string, IToken>, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = ((KeyValuePair<string, IToken> x) => !x.Key.Equals(key)));
			}
			foreach (KeyValuePair<string, IToken> keyValuePair in Enumerable.Where<KeyValuePair<string, IToken>>(data, func))
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
			return new DictionaryToken(dictionary);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000025A8 File Offset: 0x000007A8
		public static DictionaryToken With(IReadOnlyDictionary<string, IToken> data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			return new DictionaryToken(data);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000025BF File Offset: 0x000007BF
		public bool Equals(IToken obj)
		{
			return this.Equals(obj as DictionaryToken);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000025D0 File Offset: 0x000007D0
		public bool Equals(DictionaryToken other)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (this.Data.Count != other.Data.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, IToken> keyValuePair in other.Data)
			{
				IToken token;
				if (!this.Data.TryGetValue(keyValuePair.Key, ref token) || !token.Equals(keyValuePair.Value))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002668 File Offset: 0x00000868
		public override string ToString()
		{
			return string.Join(", ", Enumerable.Select<KeyValuePair<string, IToken>, string>(this.Data, delegate(KeyValuePair<string, IToken> x)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(4, 2);
				defaultInterpolatedStringHandler.AppendLiteral("<");
				defaultInterpolatedStringHandler.AppendFormatted(x.Key);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(x.Value);
				defaultInterpolatedStringHandler.AppendLiteral(">");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}));
		}
	}
}
