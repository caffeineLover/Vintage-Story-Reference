using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000043 RID: 67
	public static class RadioStationNameResolver
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public static string Resolve(IWorldAccessor world, string frequency)
		{
			string tuned = RadioFrequencyUtil.Normalize(frequency);
			if (((world != null) ? world.BlockAccessor : null) == null || tuned.Length == 0)
			{
				return "";
			}
			foreach (BlockEntityRadioEmitter emitter in RadioBlockIndex.GetLoadedEmitters(world))
			{
				if (emitter != null && emitter.HasSufficientTransmitPower() && RadioFrequencyUtil.Matches(emitter.IsRepeaterMode ? RadioFrequencyUtil.Normalize(emitter.RepeaterFrequency) : RadioFrequencyUtil.Normalize(emitter.GetConsoleFrequency()), tuned))
				{
					string consoleDisplayName = emitter.GetConsoleDisplayName();
					string name = ((consoleDisplayName != null) ? consoleDisplayName.Trim() : null) ?? "";
					if (name.Length > 0)
					{
						return name;
					}
				}
			}
			return "";
		}
	}
}
