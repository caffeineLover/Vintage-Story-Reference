using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E8 RID: 488
	[NullableContext(1)]
	[Nullable(0)]
	public class TagDirective : Token
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0002592F File Offset: 0x00023B2F
		public string Handle { get; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00025937 File Offset: 0x00023B37
		public string Prefix { get; }

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002593F File Offset: 0x00023B3F
		public TagDirective(string handle, string prefix) : this(handle, prefix, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00025954 File Offset: 0x00023B54
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

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000259C8 File Offset: 0x00023BC8
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			TagDirective tagDirective = obj as TagDirective;
			return tagDirective != null && this.Handle.Equals(tagDirective.Handle) && this.Prefix.Equals(tagDirective.Prefix);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00025A05 File Offset: 0x00023C05
		public override int GetHashCode()
		{
			return this.Handle.GetHashCode() ^ this.Prefix.GetHashCode();
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00025A1E File Offset: 0x00023C1E
		public override string ToString()
		{
			return this.Handle + " => " + this.Prefix;
		}

		// Token: 0x04000486 RID: 1158
		private static readonly Regex TagHandlePattern = new Regex("^!([0-9A-Za-z_\\-]*!)?$", 8);
	}
}
