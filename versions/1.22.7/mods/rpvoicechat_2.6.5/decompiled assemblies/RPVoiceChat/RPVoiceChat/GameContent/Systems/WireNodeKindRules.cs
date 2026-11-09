using System;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000C6 RID: 198
	public static class WireNodeKindRules
	{
		// Token: 0x060005F7 RID: 1527 RVA: 0x000203F9 File Offset: 0x0001E5F9
		public static bool IsRadioFamilyEndpoint(WireNodeKind kind)
		{
			return kind == WireNodeKind.Radio || kind == WireNodeKind.RadioConsole || kind == WireNodeKind.RadioEmitter;
		}
	}
}
