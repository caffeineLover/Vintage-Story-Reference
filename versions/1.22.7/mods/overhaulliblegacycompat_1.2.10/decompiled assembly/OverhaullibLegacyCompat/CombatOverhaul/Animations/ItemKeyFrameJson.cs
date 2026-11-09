using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017D RID: 381
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ItemKeyFrameJson
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0005B89E File Offset: 0x00059A9E
		// (set) Token: 0x06001300 RID: 4864 RVA: 0x0005B8A6 File Offset: 0x00059AA6
		public float DurationFraction { get; set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0005B8AF File Offset: 0x00059AAF
		// (set) Token: 0x06001302 RID: 4866 RVA: 0x0005B8B7 File Offset: 0x00059AB7
		public string EasingFunction { get; set; } = "Linear";

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x0005B8C0 File Offset: 0x00059AC0
		// (set) Token: 0x06001304 RID: 4868 RVA: 0x0005B8C8 File Offset: 0x00059AC8
		public Dictionary<string, float?[]> Elements { get; set; } = new Dictionary<string, float?[]>();

		// Token: 0x06001305 RID: 4869 RVA: 0x0005B8D4 File Offset: 0x00059AD4
		public ItemKeyFrame ToKeyFrame()
		{
			EasingFunctionType function = Enum.Parse<EasingFunctionType>(this.EasingFunction);
			return new ItemKeyFrame(new ItemFrame(Enumerable.ToDictionary<KeyValuePair<string, float?[]>, string, AnimationElement>(this.Elements, (KeyValuePair<string, float?[]> entry) => entry.Key, (KeyValuePair<string, float?[]> entry) => new AnimationElement(entry.Value))), this.DurationFraction, function);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0005B948 File Offset: 0x00059B48
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
