using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Config;

namespace TentBagReworked.Config
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class Lang
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00005080 File Offset: 0x00003280
		public static string UnpackError(params object[] _)
		{
			return Lang.Get("tentbag:unpack-error", Array.Empty<object>());
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005091 File Offset: 0x00003291
		public static string IllegalItemError(params object[] args)
		{
			return Lang.Get("tentbag:illegal-item-error", args);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000509E File Offset: 0x0000329E
		public static string SolidGroundError(params object[] _)
		{
			return Lang.Get("tentbag:solid-ground-error", Array.Empty<object>());
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000050AF File Offset: 0x000032AF
		public static string ClearAreaError(params object[] _)
		{
			return Lang.Get("tentbag:clear-area-error", Array.Empty<object>());
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000050C0 File Offset: 0x000032C0
		public static string UnpackHungerError(params object[] _)
		{
			return Lang.Get("tentbag:unpack-hunger-error", Array.Empty<object>());
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000050D1 File Offset: 0x000032D1
		public static string PackHungerError(params object[] _)
		{
			return Lang.Get("tentbag:pack-hunger-error", Array.Empty<object>());
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000050E2 File Offset: 0x000032E2
		public static string EmptyBuildError(params object[] _)
		{
			return Lang.Get("tentbag:empty-build-error", Array.Empty<object>());
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000050F3 File Offset: 0x000032F3
		public static string WpRemoveError(params object[] _)
		{
			return Lang.Get("tentbag:wp-remove-error", Array.Empty<object>());
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005104 File Offset: 0x00003304
		public static string PackFloorOnlyError(params object[] _)
		{
			return Lang.Get("tentbag:pack-floor-only", Array.Empty<object>());
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005115 File Offset: 0x00003315
		public static string PackSuccess(params object[] args)
		{
			return Lang.Get("tentbag:pack-success", args);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005122 File Offset: 0x00003322
		public static string UnpackSuccess(params object[] args)
		{
			return Lang.Get("tentbag:unpack-success", args);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000512F File Offset: 0x0000332F
		public static string WaypointTitle(params object[] args)
		{
			return Lang.Get("tentbag:waypoint-title", args);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000513C File Offset: 0x0000333C
		public static string StatusOn(params object[] _)
		{
			return Lang.Get("tentbag:status-on", Array.Empty<object>());
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000514D File Offset: 0x0000334D
		public static string StatusOff(params object[] _)
		{
			return Lang.Get("tentbag:status-off", Array.Empty<object>());
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000515E File Offset: 0x0000335E
		public static string PreviewToggled(params object[] args)
		{
			return Lang.Get("tentbag:preview-toggled", args);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000516B File Offset: 0x0000336B
		public static string HotkeyTogglePreview(params object[] _)
		{
			return Lang.Get("tentbag:hotkey-toggle-preview", Array.Empty<object>());
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000517C File Offset: 0x0000337C
		public static string HotkeyHighlightGrabArea(params object[] _)
		{
			return Lang.Get("tentbag:hotkey-highlight-grab-area", Array.Empty<object>());
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000518D File Offset: 0x0000338D
		public static string HotkeyRotate(params object[] _)
		{
			return Lang.Get("tentbag:hotkey-rotate", Array.Empty<object>());
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000519E File Offset: 0x0000339E
		public static string RotationInfo(params object[] args)
		{
			return Lang.Get("tentbag:rotation-info", args);
		}
	}
}
