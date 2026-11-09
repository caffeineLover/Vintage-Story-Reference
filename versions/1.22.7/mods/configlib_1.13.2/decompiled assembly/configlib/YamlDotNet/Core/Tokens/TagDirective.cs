using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E4 RID: 484
	[NullableContext(1)]
	[Nullable(0)]
	public class TagDirective : Token
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x00024FE7 File Offset: 0x000231E7
		public string Handle { get; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x00024FEF File Offset: 0x000231EF
		public string Prefix { get; }

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00024FF7 File Offset: 0x000231F7
		public TagDirective(string handle, string prefix) : this(handle, prefix, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002500C File Offset: 0x0002320C
		public TagDirective(string handle, string prefix, Mark start, Mark end) : base(start, end)
		{
			if (string.IsNullOrEmpty(handle))
			{
				throw new ArgumentNullException("handle", "Tag handle must not be empty.");
			}
			if (!TagDirective.TagHandlePattern.IsMatch(handle))
			{
				throw new ArgumentException("Tag handle must start and end with '!' and contain alphanumerical characters only.", "handle");
			}
			this.Handle = handle;
			if (string.IsNullOrEmpty(prefix))
			{
				throw new ArgumentNullException("prefix", "Tag prefix must not be empty.");
			}
			this.Prefix = prefix;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00025080 File Offset: 0x00023280
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			TagDirective tagDirective = obj as TagDirective;
			return tagDirective != null && this.Handle.Equals(tagDirective.Handle) && this.Prefix.Equals(tagDirective.Prefix);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000250BD File Offset: 0x000232BD
		public override int GetHashCode()
		{
			return this.Handle.GetHashCode() ^ this.Prefix.GetHashCode();
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000250D6 File Offset: 0x000232D6
		public override string ToString()
		{
			return this.Handle + " => " + this.Prefix;
		}

		// Token: 0x04000458 RID: 1112
		private static readonly Regex TagHandlePattern = new Regex("^!([0-9A-Za-z_\\-]*!)?$", 8);
	}
}
