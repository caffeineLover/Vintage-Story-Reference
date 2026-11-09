using System;
using System.Runtime.Versioning;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI.ViewManagement
{
	// Token: 0x02000025 RID: 37
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal enum UIElementType
	{
		// Token: 0x04000057 RID: 87
		ActiveCaption,
		// Token: 0x04000058 RID: 88
		Background,
		// Token: 0x04000059 RID: 89
		ButtonFace,
		// Token: 0x0400005A RID: 90
		ButtonText,
		// Token: 0x0400005B RID: 91
		CaptionText,
		// Token: 0x0400005C RID: 92
		GrayText,
		// Token: 0x0400005D RID: 93
		Highlight,
		// Token: 0x0400005E RID: 94
		HighlightText,
		// Token: 0x0400005F RID: 95
		Hotlight,
		// Token: 0x04000060 RID: 96
		InactiveCaption,
		// Token: 0x04000061 RID: 97
		InactiveCaptionText,
		// Token: 0x04000062 RID: 98
		Window,
		// Token: 0x04000063 RID: 99
		WindowText,
		// Token: 0x04000064 RID: 100
		[SupportedOSPlatform("Windows10.0.10240.0")]
		AccentColor = 1000,
		// Token: 0x04000065 RID: 101
		[SupportedOSPlatform("Windows10.0.10240.0")]
		TextHigh,
		// Token: 0x04000066 RID: 102
		[SupportedOSPlatform("Windows10.0.10240.0")]
		TextMedium,
		// Token: 0x04000067 RID: 103
		[SupportedOSPlatform("Windows10.0.10240.0")]
		TextLow,
		// Token: 0x04000068 RID: 104
		[SupportedOSPlatform("Windows10.0.10240.0")]
		TextContrastWithHigh,
		// Token: 0x04000069 RID: 105
		[SupportedOSPlatform("Windows10.0.10240.0")]
		NonTextHigh,
		// Token: 0x0400006A RID: 106
		[SupportedOSPlatform("Windows10.0.10240.0")]
		NonTextMediumHigh,
		// Token: 0x0400006B RID: 107
		[SupportedOSPlatform("Windows10.0.10240.0")]
		NonTextMedium,
		// Token: 0x0400006C RID: 108
		[SupportedOSPlatform("Windows10.0.10240.0")]
		NonTextMediumLow,
		// Token: 0x0400006D RID: 109
		[SupportedOSPlatform("Windows10.0.10240.0")]
		NonTextLow,
		// Token: 0x0400006E RID: 110
		[SupportedOSPlatform("Windows10.0.10240.0")]
		PageBackground,
		// Token: 0x0400006F RID: 111
		[SupportedOSPlatform("Windows10.0.10240.0")]
		PopupBackground,
		// Token: 0x04000070 RID: 112
		[SupportedOSPlatform("Windows10.0.10240.0")]
		OverlayOutsidePopup
	}
}
