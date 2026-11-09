using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000178 RID: 376
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Animation
	{
		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x0005A4E5 File Offset: 0x000586E5
		public List<PLayerKeyFrame> PlayerKeyFrames { get; } = new List<PLayerKeyFrame>();

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060012A5 RID: 4773 RVA: 0x0005A4ED File Offset: 0x000586ED
		public List<ItemKeyFrame> ItemKeyFrames { get; } = new List<ItemKeyFrame>();

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x0005A4F5 File Offset: 0x000586F5
		public List<SoundFrame> SoundFrames { get; } = new List<SoundFrame>();

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060012A7 RID: 4775 RVA: 0x0005A4FD File Offset: 0x000586FD
		public List<CallbackFrame> CallbackFrames { get; } = new List<CallbackFrame>();

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0005A505 File Offset: 0x00058705
		public List<ParticlesFrame> ParticlesFrames { get; } = new List<ParticlesFrame>();

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060012A9 RID: 4777 RVA: 0x0005A50D File Offset: 0x0005870D
		public TimeSpan TotalDuration
		{
			get
			{
				List<PLayerKeyFrame> playerKeyFrames = this.PlayerKeyFrames;
				return playerKeyFrames[playerKeyFrames.Count - 1].Time;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x0005A527 File Offset: 0x00058727
		// (set) Token: 0x060012AB RID: 4779 RVA: 0x0005A52F File Offset: 0x0005872F
		public TimeSpan ItemAnimationStart { get; set; } = TimeSpan.Zero;

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0005A538 File Offset: 0x00058738
		// (set) Token: 0x060012AD RID: 4781 RVA: 0x0005A540 File Offset: 0x00058740
		public TimeSpan ItemAnimationEnd { get; set; } = TimeSpan.Zero;

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x0005A549 File Offset: 0x00058749
		// (set) Token: 0x060012AF RID: 4783 RVA: 0x0005A551 File Offset: 0x00058751
		public bool Hold { get; set; }

		// Token: 0x060012B0 RID: 4784 RVA: 0x0005A55C File Offset: 0x0005875C
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

		// Token: 0x060012B1 RID: 4785 RVA: 0x0005A6B0 File Offset: 0x000588B0
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

		// Token: 0x060012B2 RID: 4786 RVA: 0x0005A7B4 File Offset: 0x000589B4
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

		// Token: 0x060012B3 RID: 4787 RVA: 0x0005A8AC File Offset: 0x00058AAC
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

		// Token: 0x060012B4 RID: 4788 RVA: 0x0005A9B0 File Offset: 0x00058BB0
		public void PlaySounds(SoundsSynchronizerClient soundsManager, TimeSpan previousDuration, TimeSpan currentDuration)
		{
			this.PlaySounds(soundsManager, previousDuration, currentDuration, 1f);
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0005A9C0 File Offset: 0x00058BC0
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

		// Token: 0x060012B6 RID: 4790 RVA: 0x0005AA54 File Offset: 0x00058C54
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

		// Token: 0x060012B7 RID: 4791 RVA: 0x0005AAE8 File Offset: 0x00058CE8
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

		// Token: 0x060012B8 RID: 4792 RVA: 0x0005AB68 File Offset: 0x00058D68
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

		// Token: 0x060012B9 RID: 4793 RVA: 0x0005ABEA File Offset: 0x00058DEA
		public bool Finished(TimeSpan currentDuration)
		{
			return currentDuration >= this.TotalDuration;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0005ABF8 File Offset: 0x00058DF8
		public override string ToString()
		{
			return AnimationJson.FromAnimation(this).ToString();
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0005AC08 File Offset: 0x00058E08
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

		// Token: 0x060012BC RID: 4796 RVA: 0x0005ACB4 File Offset: 0x00058EB4
		public PlayerItemFrame StillItemFrame(int itemFrame, float frameProgress)
		{
			TimeSpan timeStamp = this.ItemAnimationStart + (this.ItemAnimationEnd - this.ItemAnimationStart) * (double)this.ItemKeyFrames[itemFrame].DurationFraction * (double)frameProgress;
			return this.StillFrame((float)(timeStamp / this.TotalDuration));
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0005AD0F File Offset: 0x00058F0F
		public PlayerItemFrame StillFrame(float progress)
		{
			return this.Interpolate(PlayerItemFrame.Zero, (double)progress * this.TotalDuration);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0005AD2C File Offset: 0x00058F2C
		public Animation Clone()
		{
			return new Animation(this.PlayerKeyFrames, this.ItemKeyFrames, this.SoundFrames, this.ParticlesFrames, this.CallbackFrames)
			{
				ItemAnimationStart = this.ItemAnimationStart,
				ItemAnimationEnd = this.ItemAnimationEnd,
				Hold = this.Hold
			};
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0005AD80 File Offset: 0x00058F80
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

		// Token: 0x060012C0 RID: 4800 RVA: 0x0005AF0C File Offset: 0x0005910C
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

		// Token: 0x060012C1 RID: 4801 RVA: 0x0005B0A0 File Offset: 0x000592A0
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

		// Token: 0x060012C2 RID: 4802 RVA: 0x0005B148 File Offset: 0x00059348
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

		// Token: 0x040008B5 RID: 2229
		public static readonly Animation Zero = new Animation(new PLayerKeyFrame[]
		{
			PLayerKeyFrame.Zero
		});

		// Token: 0x040008B6 RID: 2230
		internal int _playerFrameIndex;

		// Token: 0x040008B7 RID: 2231
		internal int _itemFrameIndex;

		// Token: 0x040008B8 RID: 2232
		internal bool _playerFrameEdited = true;

		// Token: 0x040008B9 RID: 2233
		internal int _soundsFrameIndex;

		// Token: 0x040008BA RID: 2234
		internal int _particlesFrameIndex;

		// Token: 0x040008BB RID: 2235
		internal int _callbackFrameIndex;

		// Token: 0x040008BC RID: 2236
		internal float _frameProgress = 1f;
	}
}
