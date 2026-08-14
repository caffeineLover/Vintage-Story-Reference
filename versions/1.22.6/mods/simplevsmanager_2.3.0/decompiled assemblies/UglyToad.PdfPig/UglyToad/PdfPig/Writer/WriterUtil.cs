using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x0200001D RID: 29
	internal static class WriterUtil
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00008A98 File Offset: 0x00006C98
		public static Dictionary<string, IToken> GetOrCreateDict(this Dictionary<NameToken, IToken> dict, NameToken key)
		{
			IToken token;
			if (!dict.TryGetValue(key, ref token))
			{
				Dictionary<string, IToken> dictionary = new Dictionary<string, IToken>();
				dict[key] = DictionaryToken.With(dictionary);
				return dictionary;
			}
			DictionaryToken dictionaryToken = token as DictionaryToken;
			if (dictionaryToken == null)
			{
				string text = "Expected dictionary token, got ";
				Type type = token.GetType();
				throw new ApplicationException(text + ((type != null) ? type.ToString() : null));
			}
			Dictionary<string, IToken> dictionary2 = dictionaryToken.Data as Dictionary<string, IToken>;
			if (dictionary2 != null)
			{
				return dictionary2;
			}
			dictionary2 = Enumerable.ToDictionary<KeyValuePair<string, IToken>, string, IToken>(dictionaryToken.Data, (KeyValuePair<string, IToken> x) => x.Key, (KeyValuePair<string, IToken> x) => x.Value);
			dict[key] = DictionaryToken.With(dictionary2);
			return dictionary2;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008B60 File Offset: 0x00006D60
		public static Dictionary<string, IToken> GetOrCreateDict(this Dictionary<string, IToken> dict, string key)
		{
			IToken token;
			if (!dict.TryGetValue(key, ref token))
			{
				Dictionary<string, IToken> dictionary = new Dictionary<string, IToken>();
				dict[key] = DictionaryToken.With(dictionary);
				return dictionary;
			}
			DictionaryToken dictionaryToken = token as DictionaryToken;
			if (dictionaryToken == null)
			{
				string text = "Expected dictionary token, got ";
				Type type = token.GetType();
				throw new ApplicationException(text + ((type != null) ? type.ToString() : null));
			}
			Dictionary<string, IToken> dictionary2 = dictionaryToken.Data as Dictionary<string, IToken>;
			if (dictionary2 != null)
			{
				return dictionary2;
			}
			dictionary2 = Enumerable.ToDictionary<KeyValuePair<string, IToken>, string, IToken>(dictionaryToken.Data, (KeyValuePair<string, IToken> x) => x.Key, (KeyValuePair<string, IToken> x) => x.Value);
			dict[key] = DictionaryToken.With(dictionary2);
			return dictionary2;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008C28 File Offset: 0x00006E28
		public static IToken CopyToken(IPdfStreamWriter writer, IToken tokenToCopy, IPdfTokenScanner tokenScanner, IDictionary<IndirectReference, IndirectReferenceToken> referencesFromDocument, Dictionary<IndirectReference, IndirectReferenceToken> callstack = null)
		{
			if (callstack == null)
			{
				callstack = new Dictionary<IndirectReference, IndirectReferenceToken>();
			}
			DictionaryToken dictionaryToken = tokenToCopy as DictionaryToken;
			if (dictionaryToken != null)
			{
				Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
				foreach (KeyValuePair<string, IToken> keyValuePair in dictionaryToken.Data)
				{
					string key = keyValuePair.Key;
					IToken value = keyValuePair.Value;
					dictionary.Add(NameToken.Create(key), WriterUtil.CopyToken(writer, value, tokenScanner, referencesFromDocument, callstack));
				}
				return new DictionaryToken(dictionary);
			}
			ArrayToken arrayToken = tokenToCopy as ArrayToken;
			if (arrayToken != null)
			{
				List<IToken> list = new List<IToken>(arrayToken.Length);
				foreach (IToken tokenToCopy2 in arrayToken.Data)
				{
					list.Add(WriterUtil.CopyToken(writer, tokenToCopy2, tokenScanner, referencesFromDocument, callstack));
				}
				return new ArrayToken(list);
			}
			IndirectReferenceToken indirectReferenceToken = tokenToCopy as IndirectReferenceToken;
			if (indirectReferenceToken == null)
			{
				StreamToken streamToken = tokenToCopy as StreamToken;
				if (streamToken != null)
				{
					DictionaryToken dictionaryToken2 = WriterUtil.CopyToken(writer, streamToken.StreamDictionary, tokenScanner, referencesFromDocument, callstack) as DictionaryToken;
					IReadOnlyList<byte> data = streamToken.Data;
					return new StreamToken(dictionaryToken2, data);
				}
				if (!(tokenToCopy is ObjectToken))
				{
					return tokenToCopy;
				}
				throw new NotSupportedException("Copying a Object token is not supported");
			}
			else
			{
				IndirectReferenceToken indirectReferenceToken2;
				if (referencesFromDocument.TryGetValue(indirectReferenceToken.Data, ref indirectReferenceToken2))
				{
					return indirectReferenceToken2;
				}
				if (callstack.ContainsKey(indirectReferenceToken.Data) && callstack[indirectReferenceToken.Data] == null)
				{
					indirectReferenceToken2 = writer.ReserveObjectNumber();
					callstack[indirectReferenceToken.Data] = indirectReferenceToken2;
					referencesFromDocument.Add(indirectReferenceToken.Data, indirectReferenceToken2);
					return indirectReferenceToken2;
				}
				callstack.Add(indirectReferenceToken.Data, null);
				IToken token = DirectObjectFinder.Get<IToken>(indirectReferenceToken.Data, tokenScanner);
				if (token == null)
				{
					return null;
				}
				IToken token2 = WriterUtil.CopyToken(writer, token, tokenScanner, referencesFromDocument, callstack);
				if (callstack[indirectReferenceToken.Data] != null)
				{
					return writer.WriteToken(token2, callstack[indirectReferenceToken.Data]);
				}
				indirectReferenceToken2 = writer.WriteToken(token2);
				referencesFromDocument.Add(indirectReferenceToken.Data, indirectReferenceToken2);
				return indirectReferenceToken2;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008E58 File Offset: 0x00007058
		internal static IEnumerable<ValueTuple<DictionaryToken, IReadOnlyList<DictionaryToken>>> WalkTree(PageTreeNode node, List<DictionaryToken> parents = null)
		{
			if (parents == null)
			{
				parents = new List<DictionaryToken>();
			}
			if (node.IsPage)
			{
				yield return new ValueTuple<DictionaryToken, IReadOnlyList<DictionaryToken>>(node.NodeDictionary, parents);
				yield break;
			}
			parents = Enumerable.ToList<DictionaryToken>(parents);
			parents.Add(node.NodeDictionary);
			foreach (PageTreeNode node2 in node.Children)
			{
				foreach (ValueTuple<DictionaryToken, IReadOnlyList<DictionaryToken>> valueTuple in WriterUtil.WalkTree(node2, parents))
				{
					yield return valueTuple;
				}
				IEnumerator<ValueTuple<DictionaryToken, IReadOnlyList<DictionaryToken>>> enumerator2 = null;
			}
			IEnumerator<PageTreeNode> enumerator = null;
			yield break;
			yield break;
		}
	}
}
