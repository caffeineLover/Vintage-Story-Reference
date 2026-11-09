using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000081 RID: 129
	[NullableContext(1)]
	[Nullable(0)]
	public static class OnlineModlistContentValidator
	{
		// Token: 0x06000B73 RID: 2931 RVA: 0x000470D8 File Offset: 0x000452D8
		public static bool TryFindBlockedWord([Nullable(new byte[]
		{
			1,
			2
		})] IEnumerable<string> textValues, out string blockedWord)
		{
			ArgumentNullException.ThrowIfNull(textValues, "textValues");
			foreach (string text in textValues)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					foreach (OnlineModlistContentValidator.BlockedTerm blockedTerm in OnlineModlistContentValidator.BlockedTerms.Value)
					{
						if (blockedTerm.Pattern.IsMatch(text))
						{
							blockedWord = blockedTerm.Text;
							return true;
						}
					}
				}
			}
			blockedWord = string.Empty;
			return false;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0004718C File Offset: 0x0004538C
		private static IReadOnlyList<OnlineModlistContentValidator.BlockedTerm> LoadBlockedTerms()
		{
			Assembly assembly = typeof(OnlineModlistContentValidator).Assembly;
			string text = Enumerable.SingleOrDefault<string>(assembly.GetManifestResourceNames(), (string name) => name.EndsWith(".swearWords.xml", 5));
			if (text == null)
			{
				throw new InvalidOperationException("The embedded online modlist blocked-word list could not be found.");
			}
			Stream manifestResourceStream = assembly.GetManifestResourceStream(text);
			if (manifestResourceStream == null)
			{
				throw new InvalidOperationException("The embedded online modlist blocked-word list could not be opened.");
			}
			IReadOnlyList<OnlineModlistContentValidator.BlockedTerm> result;
			using (Stream stream = manifestResourceStream)
			{
				using (XmlReader xmlReader = XmlReader.Create(stream, new XmlReaderSettings
				{
					DtdProcessing = 0,
					XmlResolver = null
				}))
				{
					XElement root = XDocument.Load(xmlReader, 0).Root;
					List<OnlineModlistContentValidator.BlockedTerm> list;
					if (root == null)
					{
						list = null;
					}
					else
					{
						IEnumerable<string> enumerable = Enumerable.Distinct<string>(Enumerable.Where<string>(Enumerable.Select<XElement, string>(root.Elements("word"), (XElement element) => element.Value.Trim()), (string value) => value.Length > 0), StringComparer.OrdinalIgnoreCase);
						Func<string, OnlineModlistContentValidator.BlockedTerm> func;
						if ((func = OnlineModlistContentValidator.<>O.<0>__CreateBlockedTerm) == null)
						{
							func = (OnlineModlistContentValidator.<>O.<0>__CreateBlockedTerm = new Func<string, OnlineModlistContentValidator.BlockedTerm>(OnlineModlistContentValidator.CreateBlockedTerm));
						}
						list = Enumerable.ToList<OnlineModlistContentValidator.BlockedTerm>(Enumerable.Select<string, OnlineModlistContentValidator.BlockedTerm>(enumerable, func));
					}
					List<OnlineModlistContentValidator.BlockedTerm> list2 = list;
					if (list2 == null || list2.Count <= 0)
					{
						throw new InvalidDataException("The online modlist blocked-word list is empty.");
					}
					result = list2;
				}
			}
			return result;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x000472FC File Offset: 0x000454FC
		private static OnlineModlistContentValidator.BlockedTerm CreateBlockedTerm(string text)
		{
			IEnumerable<string> enumerable = Enumerable.Where<string>(Regex.Split(text, "\\s+"), (string part) => part.Length > 0);
			Func<string, string> func;
			if ((func = OnlineModlistContentValidator.<>O.<1>__Escape) == null)
			{
				func = (OnlineModlistContentValidator.<>O.<1>__Escape = new Func<string, string>(Regex.Escape));
			}
			IEnumerable<string> enumerable2 = Enumerable.Select<string, string>(enumerable, func);
			string text2 = string.Join("\\s+", enumerable2);
			string text3 = "(?<![\\p{L}\\p{N}])" + text2 + "(?![\\p{L}\\p{N}])";
			return new OnlineModlistContentValidator.BlockedTerm(text, new Regex(text3, 513, OnlineModlistContentValidator.MatchTimeout));
		}

		// Token: 0x0400053A RID: 1338
		private const string BlockedWordsResourceSuffix = ".swearWords.xml";

		// Token: 0x0400053B RID: 1339
		private static readonly TimeSpan MatchTimeout = TimeSpan.FromSeconds(1.0);

		// Token: 0x0400053C RID: 1340
		private static readonly Lazy<IReadOnlyList<OnlineModlistContentValidator.BlockedTerm>> BlockedTerms = new Lazy<IReadOnlyList<OnlineModlistContentValidator.BlockedTerm>>(new Func<IReadOnlyList<OnlineModlistContentValidator.BlockedTerm>>(OnlineModlistContentValidator.LoadBlockedTerms));

		// Token: 0x020002FE RID: 766
		[Nullable(0)]
		private sealed class BlockedTerm : IEquatable<OnlineModlistContentValidator.BlockedTerm>
		{
			// Token: 0x060018DC RID: 6364 RVA: 0x00091552 File Offset: 0x0008F752
			public BlockedTerm(string Text, Regex Pattern)
			{
				this.Text = Text;
				this.Pattern = Pattern;
				base..ctor();
			}

			// Token: 0x17000526 RID: 1318
			// (get) Token: 0x060018DD RID: 6365 RVA: 0x00091568 File Offset: 0x0008F768
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(OnlineModlistContentValidator.BlockedTerm);
				}
			}

			// Token: 0x17000527 RID: 1319
			// (get) Token: 0x060018DE RID: 6366 RVA: 0x00091574 File Offset: 0x0008F774
			// (set) Token: 0x060018DF RID: 6367 RVA: 0x0009157C File Offset: 0x0008F77C
			public string Text { get; set; }

			// Token: 0x17000528 RID: 1320
			// (get) Token: 0x060018E0 RID: 6368 RVA: 0x00091585 File Offset: 0x0008F785
			// (set) Token: 0x060018E1 RID: 6369 RVA: 0x0009158D File Offset: 0x0008F78D
			public Regex Pattern { get; set; }

			// Token: 0x060018E2 RID: 6370 RVA: 0x00091598 File Offset: 0x0008F798
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("BlockedTerm");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060018E3 RID: 6371 RVA: 0x000915E4 File Offset: 0x0008F7E4
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Text = ");
				builder.Append(this.Text);
				builder.Append(", Pattern = ");
				builder.Append(this.Pattern);
				return true;
			}

			// Token: 0x060018E4 RID: 6372 RVA: 0x0009161E File Offset: 0x0008F81E
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(OnlineModlistContentValidator.BlockedTerm left, OnlineModlistContentValidator.BlockedTerm right)
			{
				return !(left == right);
			}

			// Token: 0x060018E5 RID: 6373 RVA: 0x0009162A File Offset: 0x0008F82A
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(OnlineModlistContentValidator.BlockedTerm left, OnlineModlistContentValidator.BlockedTerm right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060018E6 RID: 6374 RVA: 0x0009163E File Offset: 0x0008F83E
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Text>k__BackingField)) * -1521134295 + EqualityComparer<Regex>.Default.GetHashCode(this.<Pattern>k__BackingField);
			}

			// Token: 0x060018E7 RID: 6375 RVA: 0x0009167E File Offset: 0x0008F87E
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as OnlineModlistContentValidator.BlockedTerm);
			}

			// Token: 0x060018E8 RID: 6376 RVA: 0x0009168C File Offset: 0x0008F88C
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(OnlineModlistContentValidator.BlockedTerm other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Text>k__BackingField, other.<Text>k__BackingField) && EqualityComparer<Regex>.Default.Equals(this.<Pattern>k__BackingField, other.<Pattern>k__BackingField));
			}

			// Token: 0x060018EA RID: 6378 RVA: 0x000916ED File Offset: 0x0008F8ED
			[CompilerGenerated]
			private BlockedTerm(OnlineModlistContentValidator.BlockedTerm original)
			{
				this.Text = original.<Text>k__BackingField;
				this.Pattern = original.<Pattern>k__BackingField;
			}

			// Token: 0x060018EB RID: 6379 RVA: 0x0009170D File Offset: 0x0008F90D
			[CompilerGenerated]
			public void Deconstruct(out string Text, out Regex Pattern)
			{
				Text = this.Text;
				Pattern = this.Pattern;
			}
		}

		// Token: 0x020002FF RID: 767
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04001334 RID: 4916
			[Nullable(0)]
			public static Func<string, OnlineModlistContentValidator.BlockedTerm> <0>__CreateBlockedTerm;

			// Token: 0x04001335 RID: 4917
			[Nullable(0)]
			public static Func<string, string> <1>__Escape;
		}
	}
}
