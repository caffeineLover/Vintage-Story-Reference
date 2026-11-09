using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000F5 RID: 245
	[NullableContext(1)]
	[Nullable(0)]
	public class TagDirective : Token
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x00017095 File Offset: 0x00015295
		public string Handle { get; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0001709D File Offset: 0x0001529D
		public string Prefix { get; }

		// Token: 0x0600061D RID: 1565 RVA: 0x000170A5 File Offset: 0x000152A5
		public TagDirective(string handle, string prefix) : this(handle, prefix, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000170BC File Offset: 0x000152BC
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

		// Token: 0x0600061F RID: 1567 RVA: 0x00017130 File Offset: 0x00015330
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			TagDirective tagDirective = obj as TagDirective;
			return tagDirective != null && this.Handle.Equals(tagDirective.Handle) && this.Prefix.Equals(tagDirective.Prefix);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001716D File Offset: 0x0001536D
		public override int GetHashCode()
		{
			return this.Handle.GetHashCode() ^ this.Prefix.GetHashCode();
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00017186 File Offset: 0x00015386
		public override string ToString()
		{
			return this.Handle + " => " + this.Prefix;
		}

		// Token: 0x040001D0 RID: 464
		private static readonly Regex TagHandlePattern = new Regex("^!([0-9A-Za-z_\\-]*!)?$", 8);
	}
}
