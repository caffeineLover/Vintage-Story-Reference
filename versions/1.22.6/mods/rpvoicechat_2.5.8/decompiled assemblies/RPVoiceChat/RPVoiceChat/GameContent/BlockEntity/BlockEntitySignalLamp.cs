using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000B3 RID: 179
	public class BlockEntitySignalLamp : BlockEntity
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0001E5E3 File Offset: 0x0001C7E3
		private BEBehaviorAnimatable Animatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001E5EB File Offset: 0x0001C7EB
		private BlockEntityAnimationUtil AnimUtil
		{
			get
			{
				BEBehaviorAnimatable animatable = this.Animatable;
				if (animatable == null)
				{
					return null;
				}
				return animatable.animUtil;
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001E606 File Offset: 0x0001C806
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (api.Side == 2)
			{
				this.InitializeClientAnimator();
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001E620 File Offset: 0x0001C820
		public void StartSlatesAnimation()
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			bool flag;
			if (animUtil == null)
			{
				flag = false;
			}
			else
			{
				Dictionary<string, AnimationMetaData> activeAnimationsByAnimCode = animUtil.activeAnimationsByAnimCode;
				flag = ((activeAnimationsByAnimCode != null) ? new bool?(activeAnimationsByAnimCode.ContainsKey("slates")) : default(bool?)).GetValueOrDefault();
			}
			bool wasRunning = flag;
			if (animUtil != null && !wasRunning)
			{
				animUtil.StartAnimation(new AnimationMetaData
				{
					Animation = "slates",
					Code = "slates"
				});
			}
			if (!wasRunning)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 1)
				{
					this.MarkDirty(false, null);
					this.PlayShutterSound();
				}
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001E6B7 File Offset: 0x0001C8B7
		public void StopSlatesAnimation()
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil != null)
			{
				animUtil.StopAnimation("slates");
			}
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				this.MarkDirty(false, null);
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001E6F0 File Offset: 0x0001C8F0
		private void InitializeClientAnimator()
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil != null && animUtil.animator == null)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 2)
				{
					Block block = base.Block;
					if (((block != null) ? block.Code : null) != null)
					{
						AssetLocation assetLoc = new AssetLocation(base.Block.Code.Domain, "shapes/block/signallamp.json");
						Shape shape = Shape.TryGet(this.Api, assetLoc);
						if (((shape != null) ? shape.Animations : null) != null && shape.Animations.Length != 0)
						{
							shape.InitForAnimations(this.Api.Logger, "block/signallamp", Array.Empty<string>());
						}
					}
					float rotYDeg = this.GetBlockSideRotY();
					animUtil.InitializeAnimator("block/signallamp", null, null, new Vec3f(0f, rotYDeg, 0f));
					return;
				}
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001E7C4 File Offset: 0x0001C9C4
		private float GetBlockSideRotY()
		{
			Block block = base.Block;
			bool flag;
			string side;
			if (block == null)
			{
				flag = false;
			}
			else
			{
				RelaxedReadOnlyDictionary<string, string> variant = block.Variant;
				flag = ((variant != null) ? new bool?(variant.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
			}
			float result;
			if (flag)
			{
				float num;
				if (!(side == "north"))
				{
					if (!(side == "east"))
					{
						if (!(side == "west"))
						{
							if (!(side == "south"))
							{
								num = 0f;
							}
							else
							{
								num = 180f;
							}
						}
						else
						{
							num = 90f;
						}
					}
					else
					{
						num = 270f;
					}
				}
				else
				{
					num = 0f;
				}
				result = num;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001E874 File Offset: 0x0001CA74
		private void PlayShutterSound()
		{
			this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/signallamp/shutter.ogg"), (double)this.Pos.X + 0.5, (double)this.Pos.Y + 0.5, (double)this.Pos.Z + 0.5, null, false, 6f, 0.25f);
		}

		// Token: 0x04000295 RID: 661
		private const string SignalLampShapeCode = "block/signallamp";

		// Token: 0x04000296 RID: 662
		private const string SlatesAnimationCode = "slates";
	}
}
