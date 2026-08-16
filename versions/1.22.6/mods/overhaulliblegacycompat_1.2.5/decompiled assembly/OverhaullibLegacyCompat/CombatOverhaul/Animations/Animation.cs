using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200016C RID: 364
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Animation
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x00058BC5 File Offset: 0x00056DC5
		public List<PLayerKeyFrame> PlayerKeyFrames { get; } = new List<PLayerKeyFrame>();

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x00058BCD File Offset: 0x00056DCD
		public List<ItemKeyFrame> ItemKeyFrames { get; } = new List<ItemKeyFrame>();

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x00058BD5 File Offset: 0x00056DD5
		public List<SoundFrame> SoundFrames { get; } = new List<SoundFrame>();

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x00058BDD File Offset: 0x00056DDD
		public List<CallbackFrame> CallbackFrames { get; } = new List<CallbackFrame>();

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x00058BE5 File Offset: 0x00056DE5
		public List<ParticlesFrame> ParticlesFrames { get; } = new List<ParticlesFrame>();

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x00058BED File Offset: 0x00056DED
		public TimeSpan TotalDuration
		{
			get
			{
				List<PLayerKeyFrame> playerKeyFrames = this.PlayerKeyFrames;
				return playerKeyFrames[playerKeyFrames.Count - 1].Time;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x00058C07 File Offset: 0x00056E07
		// (set) Token: 0x06001237 RID: 4663 RVA: 0x00058C0F File Offset: 0x00056E0F
		public TimeSpan ItemAnimationStart { get; set; } = TimeSpan.Zero;

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001238 RID: 4664 RVA: 0x00058C18 File Offset: 0x00056E18
		// (set) Token: 0x06001239 RID: 4665 RVA: 0x00058C20 File Offset: 0x00056E20
		public TimeSpan ItemAnimationEnd { get; set; } = TimeSpan.Zero;

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600123A RID: 4666 RVA: 0x00058C29 File Offset: 0x00056E29
		// (set) Token: 0x0600123B RID: 4667 RVA: 0x00058C31 File Offset: 0x00056E31
		public bool Hold { get; set; }

		// Token: 0x0600123C RID: 4668 RVA: 0x00058C3C File Offset: 0x00056E3C
		public Animation(IEnumerable<PLayerKeyFrame> playerFrames, IEnumerable<ItemKeyFrame> itemFrames, IEnumerable<SoundFrame> soundFrames, IEnumerable<ParticlesFrame> particlesFrames, IEnumerable<CallbackFrame> callbackFrames)
		{
			if (!Enumerable.Any<PLayerKeyFrame>(playerFrames))
			{
				throw new ArgumentException("Frames number should be at least 1");
			}
			this.PlayerKeyFrames = Enumerable.ToList<PLayerKeyFrame>(playerFrames);
			this.ItemKeyFrames = Enumerable.ToList<ItemKeyFrame>(itemFrames);
			this.SoundFrames = Enumerable.ToList<SoundFrame>(soundFrames);
			this.ParticlesFrames = Enumerable.ToList<ParticlesFrame>(particlesFrames);
			this.CallbackFrames = Enumerable.ToList<CallbackFrame>(callbackFrames);
			this.PlayerKeyFrames.Sort((PLayerKeyFrame x, PLayerKeyFrame y) => (int)(x.Time - y.Time).TotalMilliseconds);
			this.ItemKeyFrames.Sort((ItemKeyFrame x, ItemKeyFrame y) => (int)((x.DurationFraction - y.DurationFraction) * 1000000f));
			this.SoundFrames.Sort((SoundFrame x, SoundFrame y) => (int)((x.DurationFraction - y.DurationFraction) * 1000000f));
			this.ItemAnimationEnd = this.TotalDuration;
			this.ProcessPlayerKeyFrames();
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00058D90 File Offset: 0x00056F90
		public Animation(IEnumerable<PLayerKeyFrame> playerFrames, IEnumerable<ItemKeyFrame> itemFrames)
		{
			if (!Enumerable.Any<PLayerKeyFrame>(playerFrames))
			{
				throw new ArgumentException("Frames number should be at least 1");
			}
			this.PlayerKeyFrames = Enumerable.ToList<PLayerKeyFrame>(playerFrames);
			this.ItemKeyFrames = Enumerable.ToList<ItemKeyFrame>(itemFrames);
			this.PlayerKeyFrames.Sort((PLayerKeyFrame x, PLayerKeyFrame y) => (int)(x.Time - y.Time).TotalMilliseconds);
			this.ItemKeyFrames.Sort((ItemKeyFrame x, ItemKeyFrame y) => (int)((x.DurationFraction - y.DurationFraction) * 1000000f));
			this.ItemAnimationEnd = this.TotalDuration;
			this.ProcessPlayerKeyFrames();
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00058E94 File Offset: 0x00057094
		public Animation(IEnumerable<PLayerKeyFrame> playerFrames)
		{
			if (!Enumerable.Any<PLayerKeyFrame>(playerFrames))
			{
				throw new ArgumentException("Frames number should be at least 1");
			}
			this.PlayerKeyFrames = Enumerable.ToList<PLayerKeyFrame>(playerFrames);
			this.PlayerKeyFrames.Sort((PLayerKeyFrame x, PLayerKeyFrame y) => (int)(x.Time - y.Time).TotalMilliseconds);
			this.ItemKeyFrames.Sort((ItemKeyFrame x, ItemKeyFrame y) => (int)((x.DurationFraction - y.DurationFraction) * 1000000f));
			this.ItemAnimationEnd = this.TotalDuration;
			this.ProcessPlayerKeyFrames();
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00058F8C File Offset: 0x0005718C
		public Animation(IEnumerable<PLayerKeyFrame> playerFrames, string itemAnimation, Shape itemShape)
		{
			if (!Enumerable.Any<PLayerKeyFrame>(playerFrames))
			{
				throw new ArgumentException("Frames number should be at least 1");
			}
			this.PlayerKeyFrames = Enumerable.ToList<PLayerKeyFrame>(playerFrames);
			this.ItemKeyFrames = ItemKeyFrame.FromVanillaAnimation(itemAnimation, itemShape);
			this.PlayerKeyFrames.Sort((PLayerKeyFrame x, PLayerKeyFrame y) => (int)(x.Time - y.Time).TotalMilliseconds);
			this.ItemKeyFrames.Sort((ItemKeyFrame x, ItemKeyFrame y) => (int)((x.DurationFraction - y.DurationFraction) * 1000000f));
			this.ItemAnimationEnd = this.TotalDuration;
			this.ProcessPlayerKeyFrames();
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00059090 File Offset: 0x00057290
		public void PlaySounds(SoundsSynchronizerClient soundsManager, TimeSpan previousDuration, TimeSpan currentDuration)
		{
			this.PlaySounds(soundsManager, previousDuration, currentDuration, 1f);
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x000590A0 File Offset: 0x000572A0
		public void PlaySounds(SoundsSynchronizerClient soundsManager, TimeSpan previousDuration, TimeSpan currentDuration, float animationSpeed)
		{
			if (previousDuration == TimeSpan.Zero)
			{
				previousDuration = TimeSpan.FromMilliseconds(-1L);
			}
			TimeSpan totalDuration = this.TotalDuration;
			foreach (SoundFrame frame in this.SoundFrames)
			{
				TimeSpan frameTime = (double)frame.DurationFraction * totalDuration;
				if (!(frameTime <= previousDuration) && !(frameTime > currentDuration))
				{
					soundsManager.Play(frame, animationSpeed);
				}
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00059134 File Offset: 0x00057334
		public void SpawnParticles(EntityPlayer player, ParticleEffectsManager particlesManager, TimeSpan previousDuration, TimeSpan currentDuration)
		{
			TimeSpan totalDuration = this.TotalDuration;
			foreach (ParticlesFrame frame in this.ParticlesFrames)
			{
				TimeSpan frameTime = (double)frame.DurationFraction * totalDuration;
				if (!(frameTime <= previousDuration) && !(frameTime > currentDuration))
				{
					particlesManager.Spawn(player, frame.Code, frame.Position, frame.Velocity, frame.Intensity);
				}
			}
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x000591C8 File Offset: 0x000573C8
		public void GetCallbacks(TimeSpan previousDuration, TimeSpan currentDuration, List<string> callbacks)
		{
			TimeSpan totalDuration = this.TotalDuration;
			foreach (CallbackFrame frame in this.CallbackFrames)
			{
				TimeSpan frameTime = (double)frame.DurationFraction * totalDuration;
				if (!(frameTime <= previousDuration) && !(frameTime > currentDuration))
				{
					callbacks.Add(frame.Code);
				}
			}
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00059248 File Offset: 0x00057448
		public PlayerItemFrame Interpolate(PlayerItemFrame previousAnimationFrame, TimeSpan currentDuration)
		{
			if (this.Finished(currentDuration))
			{
				List<PLayerKeyFrame> playerKeyFrames = this.PlayerKeyFrames;
				PlayerFrame frame = playerKeyFrames[playerKeyFrames.Count - 1].Frame;
				ItemFrame? item;
				if (this.ItemKeyFrames.Count == 0)
				{
					item = default(ItemFrame?);
				}
				else
				{
					List<ItemKeyFrame> itemKeyFrames = this.ItemKeyFrames;
					item = new ItemFrame?(itemKeyFrames[itemKeyFrames.Count - 1].Frame);
				}
				return new PlayerItemFrame(frame, item);
			}
			TimeSpan adjustedCurrentDuration;
			PlayerFrame player = this.InterpolatePlayerFrame(previousAnimationFrame, currentDuration, out adjustedCurrentDuration);
			ItemFrame? itemFrame = this.InterpolateItemFrame(previousAnimationFrame, adjustedCurrentDuration);
			return new PlayerItemFrame(player, itemFrame);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000592CA File Offset: 0x000574CA
		public bool Finished(TimeSpan currentDuration)
		{
			return currentDuration >= this.TotalDuration;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x000592D8 File Offset: 0x000574D8
		public override string ToString()
		{
			return AnimationJson.FromAnimation(this).ToString();
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x000592E8 File Offset: 0x000574E8
		public PlayerItemFrame StillPlayerFrame(int playerFrame, float frameProgress)
		{
			TimeSpan timestamp;
			if (playerFrame == 0)
			{
				timestamp = this.PlayerKeyFrames[playerFrame].Time * (double)frameProgress - TimeSpan.FromMilliseconds(1L);
			}
			else
			{
				timestamp = this.PlayerKeyFrames[playerFrame - 1].Time + (this.PlayerKeyFrames[playerFrame].Time - this.PlayerKeyFrames[playerFrame - 1].Time) * (double)frameProgress - TimeSpan.FromMilliseconds(1L);
			}
			if (timestamp < TimeSpan.Zero)
			{
				timestamp = TimeSpan.Zero;
			}
			return this.Interpolate(PlayerItemFrame.Zero, timestamp);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00059394 File Offset: 0x00057594
		public PlayerItemFrame StillItemFrame(int itemFrame, float frameProgress)
		{
			TimeSpan timeStamp = this.ItemAnimationStart + (this.ItemAnimationEnd - this.ItemAnimationStart) * (double)this.ItemKeyFrames[itemFrame].DurationFraction * (double)frameProgress;
			return this.StillFrame((float)(timeStamp / this.TotalDuration));
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x000593EF File Offset: 0x000575EF
		public PlayerItemFrame StillFrame(float progress)
		{
			return this.Interpolate(PlayerItemFrame.Zero, (double)progress * this.TotalDuration);
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0005940C File Offset: 0x0005760C
		public Animation Clone()
		{
			return new Animation(this.PlayerKeyFrames, this.ItemKeyFrames, this.SoundFrames, this.ParticlesFrames, this.CallbackFrames)
			{
				ItemAnimationStart = this.ItemAnimationStart,
				ItemAnimationEnd = this.ItemAnimationEnd,
				Hold = this.Hold
			};
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00059460 File Offset: 0x00057660
		private ItemFrame? InterpolateItemFrame(PlayerItemFrame previousAnimationFrame, TimeSpan currentDuration)
		{
			if (this.ItemKeyFrames.Count == 0)
			{
				return default(ItemFrame?);
			}
			TimeSpan totalDurationWithEasing = this.ItemAnimationEnd - this.ItemAnimationStart;
			float progress = Math.Clamp((float)((currentDuration - this.ItemAnimationStart) / totalDurationWithEasing), 0f, 1f);
			int nextItemKeyFrame = 0;
			while (nextItemKeyFrame < this.ItemKeyFrames.Count && this.ItemKeyFrames[nextItemKeyFrame].DurationFraction <= progress)
			{
				nextItemKeyFrame++;
			}
			if (nextItemKeyFrame >= this.ItemKeyFrames.Count)
			{
				nextItemKeyFrame = this.ItemKeyFrames.Count - 1;
			}
			float itemFrameProgress;
			if (nextItemKeyFrame == 0)
			{
				itemFrameProgress = progress / this.ItemKeyFrames[0].DurationFraction;
			}
			else
			{
				float previousFrameProgress = this.ItemKeyFrames[nextItemKeyFrame - 1].DurationFraction;
				float progressRange = this.ItemKeyFrames[nextItemKeyFrame].DurationFraction - previousFrameProgress;
				itemFrameProgress = (progress - previousFrameProgress) / progressRange;
			}
			if (nextItemKeyFrame != 0)
			{
				return new ItemFrame?(this.ItemKeyFrames[nextItemKeyFrame].Interpolate(this.ItemKeyFrames[nextItemKeyFrame - 1].Frame, itemFrameProgress));
			}
			if (this.ItemKeyFrames[0].DurationFraction <= 0f)
			{
				return new ItemFrame?(this.ItemKeyFrames[0].Frame);
			}
			ItemFrame previousFrame = previousAnimationFrame.Item ?? ItemFrame.Empty;
			return new ItemFrame?(this.ItemKeyFrames[0].Interpolate(previousFrame, itemFrameProgress));
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x000595EC File Offset: 0x000577EC
		private PlayerFrame InterpolatePlayerFrame(PlayerItemFrame previousAnimationFrame, TimeSpan currentDuration, out TimeSpan adjustedCurrentDuration)
		{
			int nextPlayerKeyFrame = 0;
			while (nextPlayerKeyFrame < this.PlayerKeyFrames.Count && !(this.PlayerKeyFrames[nextPlayerKeyFrame].Time > currentDuration))
			{
				nextPlayerKeyFrame++;
			}
			if (nextPlayerKeyFrame != 0)
			{
				TimeSpan frameDuration = this.PlayerKeyFrames[nextPlayerKeyFrame].Time - this.PlayerKeyFrames[nextPlayerKeyFrame - 1].Time;
				float frameProgress = (float)((currentDuration - this.PlayerKeyFrames[nextPlayerKeyFrame - 1].Time) / frameDuration);
				adjustedCurrentDuration = this.PlayerKeyFrames[nextPlayerKeyFrame - 1].Time + frameDuration * (double)EasingFunctions.Get(this.PlayerKeyFrames[nextPlayerKeyFrame].EasingFunction)(frameProgress);
				return this.PlayerKeyFrames[nextPlayerKeyFrame].Interpolate(this.PlayerKeyFrames[nextPlayerKeyFrame - 1].Frame, frameProgress);
			}
			if (this.PlayerKeyFrames[0].Time <= TimeSpan.Zero)
			{
				adjustedCurrentDuration = TimeSpan.Zero;
				return this.PlayerKeyFrames[0].Frame;
			}
			float frameProgress2 = (float)(currentDuration / this.PlayerKeyFrames[nextPlayerKeyFrame].Time);
			adjustedCurrentDuration = currentDuration * (double)EasingFunctions.Get(this.PlayerKeyFrames[nextPlayerKeyFrame].EasingFunction)(frameProgress2);
			return this.PlayerKeyFrames[0].Interpolate(previousAnimationFrame.Player, frameProgress2);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00059780 File Offset: 0x00057980
		private void ProcessPlayerKeyFrames()
		{
			int skipCount = 0;
			for (int index = 0; index < this.PlayerKeyFrames.Count; index++)
			{
				if (this.PlayerKeyFrames[index].EasingType == EasingFunctionType.Skip)
				{
					skipCount++;
				}
				else if (skipCount > 0)
				{
					this.ProcessPlayerKeyFrames(index, skipCount);
					skipCount = 0;
				}
			}
			PLayerKeyFrame lastFrame = this.PlayerKeyFrames[this.PlayerKeyFrames.Count - 1];
			if (lastFrame.EasingType == EasingFunctionType.Skip)
			{
				this.PlayerKeyFrames[this.PlayerKeyFrames.Count - 1] = new PLayerKeyFrame(lastFrame.Frame, lastFrame.Time, lastFrame.EasingFunction, lastFrame.EasingType, lastFrame.FrameProgressRange);
			}
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00059828 File Offset: 0x00057A28
		private void ProcessPlayerKeyFrames(int endIndex, int skipCount)
		{
			TimeSpan time = this.PlayerKeyFrames[endIndex].Time;
			TimeSpan startTime;
			if (endIndex == skipCount)
			{
				startTime = TimeSpan.Zero;
			}
			else
			{
				startTime = this.PlayerKeyFrames[endIndex - skipCount - 1].Time;
			}
			TimeSpan duration = time - startTime;
			EasingFunctionType easingFunction = this.PlayerKeyFrames[endIndex].EasingType;
			TimeSpan previousFrameTime = startTime;
			for (int index = endIndex - skipCount; index <= endIndex; index++)
			{
				PLayerKeyFrame oldFrame = this.PlayerKeyFrames[index];
				double startProgress = previousFrameTime / duration;
				double endProgress = oldFrame.Time / duration;
				PLayerKeyFrame newFrame = new PLayerKeyFrame(oldFrame.Frame, oldFrame.Time, easingFunction, oldFrame.EasingType, new Vector2((float)startProgress, (float)endProgress));
				this.PlayerKeyFrames[index] = newFrame;
				previousFrameTime = oldFrame.Time;
			}
		}

		// Token: 0x0400088F RID: 2191
		public static readonly Animation Zero = new Animation(new PLayerKeyFrame[]
		{
			PLayerKeyFrame.Zero
		});

		// Token: 0x04000890 RID: 2192
		internal int _playerFrameIndex;

		// Token: 0x04000891 RID: 2193
		internal int _itemFrameIndex;

		// Token: 0x04000892 RID: 2194
		internal bool _playerFrameEdited = true;

		// Token: 0x04000893 RID: 2195
		internal int _soundsFrameIndex;

		// Token: 0x04000894 RID: 2196
		internal int _particlesFrameIndex;

		// Token: 0x04000895 RID: 2197
		internal int _callbackFrameIndex;

		// Token: 0x04000896 RID: 2198
		internal float _frameProgress = 1f;
	}
}
