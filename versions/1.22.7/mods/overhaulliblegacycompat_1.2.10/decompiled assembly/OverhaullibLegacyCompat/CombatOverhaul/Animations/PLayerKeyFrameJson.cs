using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017E RID: 382
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PLayerKeyFrameJson
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x0005B9F3 File Offset: 0x00059BF3
		// (set) Token: 0x06001309 RID: 4873 RVA: 0x0005B9FB File Offset: 0x00059BFB
		public float EasingTime { get; set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x0005BA04 File Offset: 0x00059C04
		// (set) Token: 0x0600130B RID: 4875 RVA: 0x0005BA0C File Offset: 0x00059C0C
		public string EasingFunction { get; set; } = "Linear";

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x0005BA15 File Offset: 0x00059C15
		// (set) Token: 0x0600130D RID: 4877 RVA: 0x0005BA1D File Offset: 0x00059C1D
		public bool DetachedAnchor { get; set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x0005BA26 File Offset: 0x00059C26
		// (set) Token: 0x0600130F RID: 4879 RVA: 0x0005BA2E File Offset: 0x00059C2E
		public bool SwitchArms { get; set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x0005BA37 File Offset: 0x00059C37
		// (set) Token: 0x06001311 RID: 4881 RVA: 0x0005BA3F File Offset: 0x00059C3F
		public bool PitchFollow { get; set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001312 RID: 4882 RVA: 0x0005BA48 File Offset: 0x00059C48
		// (set) Token: 0x06001313 RID: 4883 RVA: 0x0005BA50 File Offset: 0x00059C50
		public bool PitchDontFollow { get; set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x0005BA59 File Offset: 0x00059C59
		// (set) Token: 0x06001315 RID: 4885 RVA: 0x0005BA61 File Offset: 0x00059C61
		public float FOVMultiplier { get; set; } = 1f;

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x0005BA6A File Offset: 0x00059C6A
		// (set) Token: 0x06001317 RID: 4887 RVA: 0x0005BA72 File Offset: 0x00059C72
		public float BobbingAmplitude { get; set; } = 1f;

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x0005BA7B File Offset: 0x00059C7B
		// (set) Token: 0x06001319 RID: 4889 RVA: 0x0005BA83 File Offset: 0x00059C83
		public Dictionary<string, float?[]> Elements { get; set; } = new Dictionary<string, float?[]>();

		// Token: 0x0600131A RID: 4890 RVA: 0x0005BA8C File Offset: 0x00059C8C
		public PLayerKeyFrame ToKeyFrame()
		{
			TimeSpan time = TimeSpan.FromMilliseconds((double)this.EasingTime);
			EasingFunctionType function = Enum.Parse<EasingFunctionType>(this.EasingFunction);
			RightHandFrame? rightHand = default(RightHandFrame?);
			if (this.Elements.ContainsKey("ItemAnchor") || this.Elements.ContainsKey("LowerArmR") || this.Elements.ContainsKey("UpperArmR"))
			{
				rightHand = new RightHandFrame?(new RightHandFrame(this.Elements.ContainsKey("ItemAnchor") ? new AnimationElement(this.Elements["ItemAnchor"]) : AnimationElement.Zero, this.Elements.ContainsKey("LowerArmR") ? new AnimationElement(this.Elements["LowerArmR"]) : AnimationElement.Zero, this.Elements.ContainsKey("UpperArmR") ? new AnimationElement(this.Elements["UpperArmR"]) : AnimationElement.Zero));
			}
			LeftHandFrame? leftHand = default(LeftHandFrame?);
			if (this.Elements.ContainsKey("ItemAnchorL") || this.Elements.ContainsKey("LowerArmL") || this.Elements.ContainsKey("UpperArmL"))
			{
				leftHand = new LeftHandFrame?(new LeftHandFrame(this.Elements.ContainsKey("ItemAnchorL") ? new AnimationElement(this.Elements["ItemAnchorL"]) : AnimationElement.Zero, this.Elements.ContainsKey("LowerArmL") ? new AnimationElement(this.Elements["LowerArmL"]) : AnimationElement.Zero, this.Elements.ContainsKey("UpperArmL") ? new AnimationElement(this.Elements["UpperArmL"]) : AnimationElement.Zero));
			}
			OtherPartsFrame? otherParts = default(OtherPartsFrame?);
			if (this.Elements.ContainsKey("Neck") || this.Elements.ContainsKey("Head") || this.Elements.ContainsKey("UpperFootR") || this.Elements.ContainsKey("UpperFootL") || this.Elements.ContainsKey("LowerFootR") || this.Elements.ContainsKey("LowerFootL"))
			{
				otherParts = new OtherPartsFrame?(new OtherPartsFrame(this.Elements.ContainsKey("Neck") ? new AnimationElement(this.Elements["Neck"]) : AnimationElement.Zero, this.Elements.ContainsKey("Head") ? new AnimationElement(this.Elements["Head"]) : AnimationElement.Zero, this.Elements.ContainsKey("UpperFootR") ? new AnimationElement(this.Elements["UpperFootR"]) : AnimationElement.Zero, this.Elements.ContainsKey("UpperFootL") ? new AnimationElement(this.Elements["UpperFootL"]) : AnimationElement.Zero, this.Elements.ContainsKey("LowerFootR") ? new AnimationElement(this.Elements["LowerFootR"]) : AnimationElement.Zero, this.Elements.ContainsKey("LowerFootL") ? new AnimationElement(this.Elements["LowerFootL"]) : AnimationElement.Zero));
			}
			AnimationElement? torso = this.Elements.ContainsKey("UpperTorso") ? new AnimationElement?(new AnimationElement(this.Elements["UpperTorso"])) : default(AnimationElement?);
			AnimationElement? anchor = this.Elements.ContainsKey("DetachedAnchor") ? new AnimationElement?(new AnimationElement(this.Elements["DetachedAnchor"])) : default(AnimationElement?);
			AnimationElement? lowerTorso = this.Elements.ContainsKey("LowerTorso") ? new AnimationElement?(new AnimationElement(this.Elements["LowerTorso"])) : default(AnimationElement?);
			float pitch = this.PitchFollow ? 1f : 0.8f;
			pitch = (this.PitchDontFollow ? 0f : pitch);
			RightHandFrame? rightHand2 = rightHand;
			LeftHandFrame? leftHand2 = leftHand;
			OtherPartsFrame? otherParts2 = otherParts;
			AnimationElement? upperTorso = torso;
			AnimationElement? detachedAnchorFrame = anchor;
			bool detachedAnchor = this.DetachedAnchor;
			bool switchArms = this.SwitchArms;
			float pitchFollow = pitch;
			float fovmultiplier = this.FOVMultiplier;
			float bobbingAmplitude = this.BobbingAmplitude;
			AnimationElement? lowerTorso2 = lowerTorso;
			return new PLayerKeyFrame(new PlayerFrame(rightHand2, leftHand2, otherParts2, upperTorso, detachedAnchorFrame, detachedAnchor, switchArms, pitchFollow, fovmultiplier, bobbingAmplitude, default(float?), lowerTorso2), time, function);
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0005BF0C File Offset: 0x0005A10C
		public static PLayerKeyFrameJson FromKeyFrame(PLayerKeyFrame frame)
		{
			PLayerKeyFrameJson result = new PLayerKeyFrameJson
			{
				EasingTime = (float)frame.Time.TotalMilliseconds,
				EasingFunction = frame.EasingType.ToString(),
				DetachedAnchor = frame.Frame.DetachedAnchor,
				SwitchArms = frame.Frame.SwitchArms,
				PitchFollow = (Math.Abs(frame.Frame.PitchFollow - 1f) < 1E-06f),
				PitchDontFollow = (Math.Abs(frame.Frame.PitchFollow - 0f) < 1E-06f),
				FOVMultiplier = frame.Frame.FovMultiplier,
				BobbingAmplitude = frame.Frame.BobbingAmplitude
			};
			if (frame.Frame.RightHand != null)
			{
				RightHandFrame rightHand = frame.Frame.RightHand.Value;
				result.Elements.Add("ItemAnchor", rightHand.ItemAnchor.ToArray());
				result.Elements.Add("LowerArmR", rightHand.LowerArmR.ToArray());
				result.Elements.Add("UpperArmR", rightHand.UpperArmR.ToArray());
			}
			if (frame.Frame.LeftHand != null)
			{
				LeftHandFrame leftHand = frame.Frame.LeftHand.Value;
				result.Elements.Add("ItemAnchorL", leftHand.ItemAnchorL.ToArray());
				result.Elements.Add("LowerArmL", leftHand.LowerArmL.ToArray());
				result.Elements.Add("UpperArmL", leftHand.UpperArmL.ToArray());
			}
			if (frame.Frame.OtherParts != null)
			{
				OtherPartsFrame otherParts = frame.Frame.OtherParts.Value;
				result.Elements.Add("Neck", otherParts.Neck.ToArray());
				result.Elements.Add("Head", otherParts.Head.ToArray());
				result.Elements.Add("UpperFootR", otherParts.UpperFootR.ToArray());
				result.Elements.Add("UpperFootL", otherParts.UpperFootL.ToArray());
				result.Elements.Add("LowerFootR", otherParts.LowerFootR.ToArray());
				result.Elements.Add("LowerFootL", otherParts.LowerFootL.ToArray());
			}
			if (frame.Frame.UpperTorso != null)
			{
				result.Elements.Add("UpperTorso", frame.Frame.UpperTorso.Value.ToArray());
			}
			if (frame.Frame.LowerTorso != null)
			{
				result.Elements.Add("LowerTorso", frame.Frame.LowerTorso.Value.ToArray());
			}
			if (frame.Frame.DetachedAnchorFrame != null)
			{
				result.Elements.Add("DetachedAnchor", frame.Frame.DetachedAnchorFrame.Value.ToArray());
			}
			return result;
		}
	}
}
