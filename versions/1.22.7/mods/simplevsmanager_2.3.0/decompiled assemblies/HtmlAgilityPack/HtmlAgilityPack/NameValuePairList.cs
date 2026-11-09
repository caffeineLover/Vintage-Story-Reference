using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200002C RID: 44
	[NullableContext(1)]
	[Nullable(0)]
	internal class NameValuePairList
	{
		// Token: 0x06000276 RID: 630 RVA: 0x00011B2A File Offset: 0x0000FD2A
		internal NameValuePairList() : this(null)
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00011B33 File Offset: 0x0000FD33
		internal NameValuePairList(string text)
		{
			this.Text = text;
			this._allPairs = new List<KeyValuePair<string, string>>();
			this._pairsWithName = new Dictionary<string, List<KeyValuePair<string, string>>>();
			this.Parse(text);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00011B5F File Offset: 0x0000FD5F
		internal static string GetNameValuePairsValue(string text, string name)
		{
			return new NameValuePairList(text).GetNameValuePairValue(name);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00011B70 File Offset: 0x0000FD70
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		internal List<KeyValuePair<string, string>> GetNameValuePairs(string name)
		{
			if (name == null)
			{
				return this._allPairs;
			}
			List<KeyValuePair<string, string>> result;
			if (!this._pairsWithName.TryGetValue(name, ref result))
			{
				return new List<KeyValuePair<string, string>>();
			}
			return result;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00011BA0 File Offset: 0x0000FDA0
		internal string GetNameValuePairValue(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			List<KeyValuePair<string, string>> nameValuePairs = this.GetNameValuePairs(name);
			if (nameValuePairs.Count == 0)
			{
				return string.Empty;
			}
			return nameValuePairs[0].Value.Trim();
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00011BE0 File Offset: 0x0000FDE0
		private void Parse(string text)
		{
			this._allPairs.Clear();
			this._pairsWithName.Clear();
			if (text == null)
			{
				return;
			}
			foreach (string text2 in text.Split(';', 0))
			{
				if (text2.Length != 0)
				{
					string[] array2 = text2.Split(new char[]
					{
						'='
					}, 2);
					if (array2.Length != 0)
					{
						KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(array2[0].Trim().ToLowerInvariant(), (array2.Length < 2) ? "" : array2[1]);
						this._allPairs.Add(keyValuePair);
						List<KeyValuePair<string, string>> list;
						if (!this._pairsWithName.TryGetValue(keyValuePair.Key, ref list))
						{
							list = new List<KeyValuePair<string, string>>();
							this._pairsWithName.Add(keyValuePair.Key, list);
						}
						list.Add(keyValuePair);
					}
				}
			}
		}

		// Token: 0x040000E0 RID: 224
		internal readonly string Text;

		// Token: 0x040000E1 RID: 225
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private List<KeyValuePair<string, string>> _allPairs;

		// Token: 0x040000E2 RID: 226
		[Nullable(new byte[]
		{
			1,
			1,
			1,
			0,
			1,
			1
		})]
		private Dictionary<string, List<KeyValuePair<string, string>>> _pairsWithName;
	}
}
