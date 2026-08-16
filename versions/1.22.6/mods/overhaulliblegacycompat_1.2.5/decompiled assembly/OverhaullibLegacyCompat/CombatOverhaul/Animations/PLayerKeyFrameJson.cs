using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000172 RID: 370
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PLayerKeyFrameJson
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0005A0D3 File Offset: 0x000582D3
		// (set) Token: 0x06001295 RID: 4757 RVA: 0x0005A0DB File Offset: 0x000582DB
		public float EasingTime { get; set; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x0005A0E4 File Offset: 0x000582E4
		// (set) Token: 0x06001297 RID: 4759 RVA: 0x0005A0EC File Offset: 0x000582EC
		public string EasingFunction { get; set; } = "Linear";

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x0005A0F5 File Offset: 0x000582F5
		// (set) Token: 0x06001299 RID: 4761 RVA: 0x0005A0FD File Offset: 0x000582FD
		public bool DetachedAnchor { get; set; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x0005A106 File Offset: 0x00058306
		// (set) Token: 0x0600129B RID: 4763 RVA: 0x0005A10E File Offset: 0x0005830E
		public bool SwitchArms { get; set; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600129C RID: 4764 RVA: 0x0005A117 File Offset: 0x00058317
		// (set) Token: 0x0600129D RID: 4765 RVA: 0x0005A11F File Offset: 0x0005831F
		public bool PitchFollow { get; set; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x0005A128 File Offset: 0x00058328
		// (set) Token: 0x0600129F RID: 4767 RVA: 0x0005A130 File Offset: 0x00058330
		public bool PitchDontFollow { get; set; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x0005A139 File Offset: 0x00058339
		// (set) Token: 0x060012A1 RID: 4769 RVA: 0x0005A141 File Offset: 0x00058341
		public float FOVMultiplier { get; set; } = 1f;

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x0005A14A File Offset: 0x0005834A
		// (set) Token: 0x060012A3 RID: 4771 RVA: 0x0005A152 File Offset: 0x00058352
		public float BobbingAmplitude { get; set; } = 1f;

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x0005A15B File Offset: 0x0005835B
		// (set) Token: 0x060012A5 RID: 4773 RVA: 0x0005A163 File Offset: 0x00058363
		public Dictionary<string, float?[]> Elements { get; set; } = new Dictionary<string, float?[]>();

		// Token: 0x060012A6 RID: 4774 RVA: 0x0005A16C File Offset: 0x0005836C
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

		// Token: 0x060012A7 RID: 4775 RVA: 0x0005A5EC File Offset: 0x000587EC
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
