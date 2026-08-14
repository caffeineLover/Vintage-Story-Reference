using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.Util
{
	// Token: 0x02000026 RID: 38
	public static class BlockExtensions
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x000073C4 File Offset: 0x000055C4
		public static int GetRotationAngle(this Block block)
		{
			if (((block != null) ? block.Code : null) == null)
			{
				return 0;
			}
			string path = block.Code.Path;
			int result;
			if (path.EndsWith("-north"))
			{
				result = 0;
			}
			else if (path.EndsWith("-south"))
			{
				result = 180;
			}
			else if (path.EndsWith("-east"))
			{
				result = 270;
			}
			else if (path.EndsWith("-west"))
			{
				result = 90;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}
}
