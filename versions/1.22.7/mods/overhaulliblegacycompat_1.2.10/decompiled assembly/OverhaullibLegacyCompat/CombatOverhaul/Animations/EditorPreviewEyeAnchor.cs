using System;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000195 RID: 405
	internal static class EditorPreviewEyeAnchor
	{
		// Token: 0x06001393 RID: 5011 RVA: 0x000601FF File Offset: 0x0005E3FF
		internal static float ResolveY(float localEyePositionY, float eyeHeight, bool frozenEditorPreview)
		{
			if (!frozenEditorPreview)
			{
				return localEyePositionY;
			}
			return eyeHeight;
		}
	}
}
