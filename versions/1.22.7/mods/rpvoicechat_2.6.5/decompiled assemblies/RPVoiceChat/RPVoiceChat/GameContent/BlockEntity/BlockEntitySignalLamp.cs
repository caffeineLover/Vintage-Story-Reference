using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000EC RID: 236
	public class BlockEntitySignalLamp : BlockEntity
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00029805 File Offset: 0x00027A05
		private BEBehaviorAnimatable Animatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0002980D File Offset: 0x00027A0D
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

		// Token: 0x060007B3 RID: 1971 RVA: 0x00029828 File Offset: 0x00027A28
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (api.Side == 2)
			{
				this.InitializeClientAnimator();
			}
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00029840 File Offset: 0x00027A40
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

		// Token: 0x060007B5 RID: 1973 RVA: 0x000298D7 File Offset: 0x00027AD7
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

		// Token: 0x060007B6 RID: 1974 RVA: 0x00029910 File Offset: 0x00027B10
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

		// Token: 0x060007B7 RID: 1975 RVA: 0x000299E4 File Offset: 0x00027BE4
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

		// Token: 0x060007B8 RID: 1976 RVA: 0x00029A94 File Offset: 0x00027C94
		private void PlayShutterSound()
		{
			this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/signallamp/shutter.ogg"), (double)this.Pos.X + 0.5, (double)this.Pos.Y + 0.5, (double)this.Pos.Z + 0.5, null, false, 6f, 0.25f);
		}

		// Token: 0x04000374 RID: 884
		private const string SignalLampShapeCode = "block/signallamp";

		// Token: 0x04000375 RID: 885
		private const string SlatesAnimationCode = "slates";
	}
}
