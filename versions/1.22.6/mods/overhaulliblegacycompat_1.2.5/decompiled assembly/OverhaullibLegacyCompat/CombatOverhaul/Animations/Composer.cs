using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000175 RID: 373
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Composer
	{
		// Token: 0x060012B8 RID: 4792 RVA: 0x0005ABFC File Offset: 0x00058DFC
		[NullableContext(2)]
		public Composer(SoundsSynchronizerClient soundsManager, ParticleEffectsManager particleEffectsManager, [Nullable(1)] EntityPlayer player)
		{
			this._soundsManager = soundsManager;
			this._particleEffectsManager = particleEffectsManager;
			this._player = player;
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0005AC68 File Offset: 0x00058E68
		public PlayerItemFrame Compose(TimeSpan delta)
		{
			while (this._requestsQueue.Count > 0)
			{
				AnimationRequest request = this._requestsQueue.Dequeue();
				this.ProcessRequest(request);
			}
			if (this._speedModifierDelegate != null)
			{
				this._speedModifierDuration += delta;
				if (!this._speedModifierDelegate(this._speedModifierDuration, ref delta))
				{
					this._speedModifierDelegate = null;
				}
			}
			if (this._states.Count == 0)
			{
				return PlayerItemFrame.Empty;
			}
			foreach (Composer.AnimatorState state in this._states.Values)
			{
				state.CurrentTime += delta;
				this.ProcessWeight(state);
			}
			this._frameScratch.Clear();
			foreach (Composer.AnimatorState state2 in this._states.Values)
			{
				PlayerItemFrame frame = state2.Animator.Animate(delta, this._callbackScratch);
				this._frameScratch.Add(new ValueTuple<PlayerItemFrame, float>(frame, state2.CurrentWeight));
				foreach (string callbackId in this._callbackScratch)
				{
					Action<string> callbackHandler = state2.Request.CallbackHandler;
					if (callbackHandler != null)
					{
						callbackHandler.Invoke(callbackId);
					}
				}
				if (state2.Animator.Finished())
				{
					foreach (string callbackId2 in state2.Animator.GetUnfiredCallbacks())
					{
						Action<string> callbackHandler2 = state2.Request.CallbackHandler;
						if (callbackHandler2 != null)
						{
							callbackHandler2.Invoke(callbackId2);
						}
					}
					state2.Animator.ClearUnfiredCallbacks();
				}
			}
			PlayerItemFrame result = PlayerItemFrame.Compose(this._frameScratch);
			this._categoriesToRemove.Clear();
			foreach (KeyValuePair<string, Composer.AnimatorState> keyValuePair in this._states)
			{
				string text;
				Composer.AnimatorState animatorState;
				keyValuePair.Deconstruct(ref text, ref animatorState);
				string category = text;
				Composer.AnimatorState state3 = animatorState;
				if (state3.Animator.Finished() && state3.WeightState == Composer.AnimatorWeightState.Finished)
				{
					Func<bool> callback = state3.Request.FinishCallback;
					bool removeCategory = true;
					if (callback != null && !state3.CallbacksCalled)
					{
						removeCategory = !callback.Invoke();
						state3.CallbacksCalled = true;
					}
					if (removeCategory)
					{
						this._categoriesToRemove.Add(category);
					}
				}
				if (state3.Animator.Stopped() && state3.Request.FinishCallback != null)
				{
					Func<bool> callback2 = state3.Request.FinishCallback;
					bool removeCategory2 = false;
					if (callback2 != null && !state3.CallbacksCalled)
					{
						removeCategory2 = !callback2.Invoke();
						state3.CallbacksCalled = true;
					}
					if (removeCategory2)
					{
						this._categoriesToRemove.Add(category);
					}
				}
			}
			foreach (string category2 in this._categoriesToRemove)
			{
				this._states.Remove(category2);
			}
			return result;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0005AFFC File Offset: 0x000591FC
		public void Play(AnimationRequest request)
		{
			this._requestsQueue.Enqueue(request);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0005B00A File Offset: 0x0005920A
		public void Stop(string category)
		{
			this._states.Remove(category);
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0005B019 File Offset: 0x00059219
		public void StopAll()
		{
			this._states.Clear();
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0005B026 File Offset: 0x00059226
		public bool AnyActiveAnimations()
		{
			return this._states.Count > 0;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0005B036 File Offset: 0x00059236
		public void SetSpeedModifier(AnimationSpeedModifierDelegate modifier)
		{
			this._speedModifierDuration = TimeSpan.Zero;
			this._speedModifierDelegate = modifier;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0005B04A File Offset: 0x0005924A
		public void StopSpeedModifier()
		{
			this._speedModifierDelegate = null;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0005B053 File Offset: 0x00059253
		public bool IsSpeedModifierActive()
		{
			return this._speedModifierDelegate != null;
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0005B060 File Offset: 0x00059260
		private void ProcessWeight(Composer.AnimatorState state)
		{
			switch (state.WeightState)
			{
			case Composer.AnimatorWeightState.EaseIn:
			{
				float progress = Math.Clamp((float)(state.CurrentTime / state.Request.EaseInDuration), 0f, 1f);
				state.CurrentWeight = state.PreviousWeight + (state.Request.Weight - state.PreviousWeight) * progress;
				if (progress >= 1f)
				{
					state.CurrentWeight = state.Request.Weight;
					state.WeightState = Composer.AnimatorWeightState.Stay;
					return;
				}
				break;
			}
			case Composer.AnimatorWeightState.Stay:
				if (state.Request.EaseOut && state.Animator.Finished())
				{
					state.WeightState = Composer.AnimatorWeightState.EaseOut;
					return;
				}
				break;
			case Composer.AnimatorWeightState.EaseOut:
			{
				float progress2 = Math.Clamp((float)((state.CurrentTime - state.Request.Animation.TotalDuration / (double)state.Request.AnimationSpeed) / state.Request.EaseOutDuration), 0f, 1f);
				state.CurrentWeight = state.Request.Weight * (1f - progress2);
				if (progress2 >= 1f)
				{
					state.CurrentWeight = 0f;
					state.WeightState = Composer.AnimatorWeightState.Finished;
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0005B19C File Offset: 0x0005939C
		private void ProcessRequest(AnimationRequest request)
		{
			Composer.AnimatorState state;
			if (this._states.TryGetValue(request.Category, ref state))
			{
				state.Animator.Play(request.Animation, request.AnimationSpeed);
				state.Request = request;
				state.PreviousWeight = state.CurrentWeight;
				state.WeightState = Composer.AnimatorWeightState.EaseIn;
				state.CurrentTime = TimeSpan.Zero;
				state.CallbacksCalled = false;
				return;
			}
			Animator animator = new Animator(request.Animation, this._soundsManager, this._particleEffectsManager, this._player, request.AnimationSpeed);
			this._states.Add(request.Category, new Composer.AnimatorState(request, animator));
		}

		// Token: 0x040008C4 RID: 2244
		private readonly Dictionary<string, Composer.AnimatorState> _states = new Dictionary<string, Composer.AnimatorState>();

		// Token: 0x040008C5 RID: 2245
		[TupleElementNames(new string[]
		{
			"frame",
			"weight"
		})]
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private readonly List<ValueTuple<PlayerItemFrame, float>> _frameScratch = new List<ValueTuple<PlayerItemFrame, float>>();

		// Token: 0x040008C6 RID: 2246
		private readonly List<string> _callbackScratch = new List<string>();

		// Token: 0x040008C7 RID: 2247
		private readonly List<string> _categoriesToRemove = new List<string>();

		// Token: 0x040008C8 RID: 2248
		private readonly Queue<AnimationRequest> _requestsQueue = new Queue<AnimationRequest>();

		// Token: 0x040008C9 RID: 2249
		[Nullable(2)]
		private readonly SoundsSynchronizerClient _soundsManager;

		// Token: 0x040008CA RID: 2250
		[Nullable(2)]
		private readonly ParticleEffectsManager _particleEffectsManager;

		// Token: 0x040008CB RID: 2251
		private readonly EntityPlayer _player;

		// Token: 0x040008CC RID: 2252
		private TimeSpan _speedModifierDuration = TimeSpan.Zero;

		// Token: 0x040008CD RID: 2253
		[Nullable(2)]
		private AnimationSpeedModifierDelegate _speedModifierDelegate;

		// Token: 0x020002B6 RID: 694
		[NullableContext(0)]
		private enum AnimatorWeightState
		{
			// Token: 0x04000DCE RID: 3534
			EaseIn,
			// Token: 0x04000DCF RID: 3535
			Stay,
			// Token: 0x04000DD0 RID: 3536
			EaseOut,
			// Token: 0x04000DD1 RID: 3537
			Finished
		}

		// Token: 0x020002B7 RID: 695
		[Nullable(0)]
		private sealed class AnimatorState
		{
			// Token: 0x06001836 RID: 6198 RVA: 0x0006CC78 File Offset: 0x0006AE78
			public AnimatorState(AnimationRequest request, Animator animator)
			{
				this.Request = request;
				this.Animator = animator;
			}

			// Token: 0x17000543 RID: 1347
			// (get) Token: 0x06001837 RID: 6199 RVA: 0x0006CC8E File Offset: 0x0006AE8E
			public Animator Animator { get; }

			// Token: 0x17000544 RID: 1348
			// (get) Token: 0x06001838 RID: 6200 RVA: 0x0006CC96 File Offset: 0x0006AE96
			// (set) Token: 0x06001839 RID: 6201 RVA: 0x0006CC9E File Offset: 0x0006AE9E
			public AnimationRequest Request { get; set; }

			// Token: 0x17000545 RID: 1349
			// (get) Token: 0x0600183A RID: 6202 RVA: 0x0006CCA7 File Offset: 0x0006AEA7
			// (set) Token: 0x0600183B RID: 6203 RVA: 0x0006CCAF File Offset: 0x0006AEAF
			public float PreviousWeight { get; set; }

			// Token: 0x17000546 RID: 1350
			// (get) Token: 0x0600183C RID: 6204 RVA: 0x0006CCB8 File Offset: 0x0006AEB8
			// (set) Token: 0x0600183D RID: 6205 RVA: 0x0006CCC0 File Offset: 0x0006AEC0
			public float CurrentWeight { get; set; }

			// Token: 0x17000547 RID: 1351
			// (get) Token: 0x0600183E RID: 6206 RVA: 0x0006CCC9 File Offset: 0x0006AEC9
			// (set) Token: 0x0600183F RID: 6207 RVA: 0x0006CCD1 File Offset: 0x0006AED1
			public Composer.AnimatorWeightState WeightState { get; set; }

			// Token: 0x17000548 RID: 1352
			// (get) Token: 0x06001840 RID: 6208 RVA: 0x0006CCDA File Offset: 0x0006AEDA
			// (set) Token: 0x06001841 RID: 6209 RVA: 0x0006CCE2 File Offset: 0x0006AEE2
			public TimeSpan CurrentTime { get; set; }

			// Token: 0x17000549 RID: 1353
			// (get) Token: 0x06001842 RID: 6210 RVA: 0x0006CCEB File Offset: 0x0006AEEB
			// (set) Token: 0x06001843 RID: 6211 RVA: 0x0006CCF3 File Offset: 0x0006AEF3
			public bool CallbacksCalled { get; set; }
		}
	}
}
