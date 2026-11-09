using System;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace ModernWpf.Markup
{
	// Token: 0x02000252 RID: 594
	[MarkupExtensionReturnType(typeof(KeyTime))]
	public class KeyTimeExtension : MarkupExtension
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x00032A68 File Offset: 0x00030C68
		// (set) Token: 0x06001123 RID: 4387 RVA: 0x00032A70 File Offset: 0x00030C70
		public TimeSpan TimeSpan { get; set; } = TimeSpan.Zero;

		// Token: 0x06001124 RID: 4388 RVA: 0x00032A79 File Offset: 0x00030C79
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return KeyTime.FromTimeSpan(this.TimeSpan);
		}
	}
}
