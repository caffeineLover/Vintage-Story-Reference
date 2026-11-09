using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ModernWpf
{
	// Token: 0x02000226 RID: 550
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Strings
	{
		// Token: 0x06000FDF RID: 4063 RVA: 0x0002D507 File Offset: 0x0002B707
		internal Strings()
		{
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0002D50F File Offset: 0x0002B70F
		[EditorBrowsable(2)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Strings.resourceMan == null)
				{
					Strings.resourceMan = new ResourceManager("ModernWpf.Resources.Strings", typeof(Strings).Assembly);
				}
				return Strings.resourceMan;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x0002D53B File Offset: 0x0002B73B
		// (set) Token: 0x06000FE2 RID: 4066 RVA: 0x0002D542 File Offset: 0x0002B742
		[EditorBrowsable(2)]
		internal static CultureInfo Culture
		{
			get
			{
				return Strings.resourceCulture;
			}
			set
			{
				Strings.resourceCulture = value;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x0002D54A File Offset: 0x0002B74A
		internal static string AppBarMoreButtonClosedToolTip
		{
			get
			{
				return Strings.ResourceManager.GetString("AppBarMoreButtonClosedToolTip", Strings.resourceCulture);
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0002D560 File Offset: 0x0002B760
		internal static string AppBarMoreButtonName
		{
			get
			{
				return Strings.ResourceManager.GetString("AppBarMoreButtonName", Strings.resourceCulture);
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0002D576 File Offset: 0x0002B776
		internal static string AppBarMoreButtonOpenToolTip
		{
			get
			{
				return Strings.ResourceManager.GetString("AppBarMoreButtonOpenToolTip", Strings.resourceCulture);
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0002D58C File Offset: 0x0002B78C
		internal static string IgnoreMenuItemLabel
		{
			get
			{
				return Strings.ResourceManager.GetString("IgnoreMenuItemLabel", Strings.resourceCulture);
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0002D5A2 File Offset: 0x0002B7A2
		internal static string ToggleSwitchOff
		{
			get
			{
				return Strings.ResourceManager.GetString("ToggleSwitchOff", Strings.resourceCulture);
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0002D5B8 File Offset: 0x0002B7B8
		internal static string ToggleSwitchOn
		{
			get
			{
				return Strings.ResourceManager.GetString("ToggleSwitchOn", Strings.resourceCulture);
			}
		}

		// Token: 0x04000415 RID: 1045
		private static ResourceManager resourceMan;

		// Token: 0x04000416 RID: 1046
		private static CultureInfo resourceCulture;
	}
}
