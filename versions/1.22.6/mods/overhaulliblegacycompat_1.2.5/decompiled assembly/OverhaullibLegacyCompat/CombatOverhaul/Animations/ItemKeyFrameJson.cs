using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000171 RID: 369
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ItemKeyFrameJson
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x00059F7E File Offset: 0x0005817E
		// (set) Token: 0x0600128C RID: 4748 RVA: 0x00059F86 File Offset: 0x00058186
		public float DurationFraction { get; set; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x00059F8F File Offset: 0x0005818F
		// (set) Token: 0x0600128E RID: 4750 RVA: 0x00059F97 File Offset: 0x00058197
		public string EasingFunction { get; set; } = "Linear";

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x0600128F RID: 4751 RVA: 0x00059FA0 File Offset: 0x000581A0
		// (set) Token: 0x06001290 RID: 4752 RVA: 0x00059FA8 File Offset: 0x000581A8
		public Dictionary<string, float?[]> Elements { get; set; } = new Dictionary<string, float?[]>();

		// Token: 0x06001291 RID: 4753 RVA: 0x00059FB4 File Offset: 0x000581B4
		public ItemKeyFrame ToKeyFrame()
		{
			EasingFunctionType function = Enum.Parse<EasingFunctionType>(this.EasingFunction);
			return new ItemKeyFrame(new ItemFrame(Enumerable.ToDictionary<KeyValuePair<string, float?[]>, string, AnimationElement>(this.Elements, (KeyValuePair<string, float?[]> entry) => entry.Key, (KeyValuePair<string, float?[]> entry) => new AnimationElement(entry.Value))), this.DurationFraction, function);
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0005A028 File Offset: 0x00058228
		public static ItemKeyFrameJson FromKeyFrame(ItemKeyFrame frame)
		{
			ItemKeyFrameJson itemKeyFrameJson = new ItemKeyFrameJson();
			itemKeyFrameJson.DurationFraction = frame.DurationFraction;
			itemKeyFrameJson.EasingFunction = frame.EasingFunction.ToString();
			itemKeyFrameJson.Elements = Enumerable.ToDictionary<KeyValuePair<string, AnimationElement>, string, float?[]>(frame.Frame.Elements, (KeyValuePair<string, AnimationElement> entry) => entry.Key, (KeyValuePair<string, AnimationElement> entry) => entry.Value.ToArray());
			return itemKeyFrameJson;
		}
	}
}
