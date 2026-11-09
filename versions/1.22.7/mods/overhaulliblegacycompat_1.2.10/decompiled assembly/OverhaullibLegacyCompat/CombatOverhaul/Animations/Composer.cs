using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000182 RID: 386
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Composer
	{
		// Token: 0x0600132F RID: 4911 RVA: 0x0005C650 File Offset: 0x0005A850
		[NullableContext(2)]
		public Composer(SoundsSynchronizerClient soundsManager, ParticleEffectsManager particleEffectsManager, [Nullable(1)] EntityPlayer player)
		{
			this._soundsManager = soundsManager;
			this._particleEffectsManager = particleEffectsManager;
			this._player = player;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0005C6BC File Offset: 0x0005A8BC
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

		// Token: 0x06001331 RID: 4913 RVA: 0x0005CA50 File Offset: 0x0005AC50
		public void Play(AnimationRequest request)
		{
			this._requestsQueue.Enqueue(request);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0005CA5E File Offset: 0x0005AC5E
		public void Stop(string category)
		{
			this._states.Remove(category);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0005CA6D File Offset: 0x0005AC6D
		public void StopAll()
		{
			this._states.Clear();
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0005CA7A File Offset: 0x0005AC7A
		public bool AnyActiveAnimations()
		{
			return this._states.Count > 0;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0005CA8A File Offset: 0x0005AC8A
		public void SetSpeedModifier(AnimationSpeedModifierDelegate modifier)
		{
			this._speedModifierDuration = TimeSpan.Zero;
			this._speedModifierDelegate = modifier;
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0005CA9E File Offset: 0x0005AC9E
		public void StopSpeedModifier()
		{
			this._speedModifierDelegate = null;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0005CAA7 File Offset: 0x0005ACA7
		public bool IsSpeedModifierActive()
		{
			return this._speedModifierDelegate != null;
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0005CAB4 File Offset: 0x0005ACB4
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

		// Token: 0x06001339 RID: 4921 RVA: 0x0005CBF0 File Offset: 0x0005ADF0
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

		// Token: 0x040008EC RID: 2284
		private readonly Dictionary<string, Composer.AnimatorState> _states = new Dictionary<string, Composer.AnimatorState>();

		// Token: 0x040008ED RID: 2285
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

		// Token: 0x040008EE RID: 2286
		private readonly List<string> _callbackScratch = new List<string>();

		// Token: 0x040008EF RID: 2287
		private readonly List<string> _categoriesToRemove = new List<string>();

		// Token: 0x040008F0 RID: 2288
		private readonly Queue<AnimationRequest> _requestsQueue = new Queue<AnimationRequest>();

		// Token: 0x040008F1 RID: 2289
		[Nullable(2)]
		private readonly SoundsSynchronizerClient _soundsManager;

		// Token: 0x040008F2 RID: 2290
		[Nullable(2)]
		private readonly ParticleEffectsManager _particleEffectsManager;

		// Token: 0x040008F3 RID: 2291
		private readonly EntityPlayer _player;

		// Token: 0x040008F4 RID: 2292
		private TimeSpan _speedModifierDuration = TimeSpan.Zero;

		// Token: 0x040008F5 RID: 2293
		[Nullable(2)]
		private AnimationSpeedModifierDelegate _speedModifierDelegate;

		// Token: 0x020002C9 RID: 713
		[NullableContext(0)]
		private enum AnimatorWeightState
		{
			// Token: 0x04000E1A RID: 3610
			EaseIn,
			// Token: 0x04000E1B RID: 3611
			Stay,
			// Token: 0x04000E1C RID: 3612
			EaseOut,
			// Token: 0x04000E1D RID: 3613
			Finished
		}

		// Token: 0x020002CA RID: 714
		[Nullable(0)]
		private sealed class AnimatorState
		{
			// Token: 0x060018DB RID: 6363 RVA: 0x0006F214 File Offset: 0x0006D414
			public AnimatorState(AnimationRequest request, Animator animator)
			{
				this.Request = request;
				this.Animator = animator;
			}

			// Token: 0x1700056D RID: 1389
			// (get) Token: 0x060018DC RID: 6364 RVA: 0x0006F22A File Offset: 0x0006D42A
			public Animator Animator { get; }

			// Token: 0x1700056E RID: 1390
			// (get) Token: 0x060018DD RID: 6365 RVA: 0x0006F232 File Offset: 0x0006D432
			// (set) Token: 0x060018DE RID: 6366 RVA: 0x0006F23A File Offset: 0x0006D43A
			public AnimationRequest Request { get; set; }

			// Token: 0x1700056F RID: 1391
			// (get) Token: 0x060018DF RID: 6367 RVA: 0x0006F243 File Offset: 0x0006D443
			// (set) Token: 0x060018E0 RID: 6368 RVA: 0x0006F24B File Offset: 0x0006D44B
			public float PreviousWeight { get; set; }

			// Token: 0x17000570 RID: 1392
			// (get) Token: 0x060018E1 RID: 6369 RVA: 0x0006F254 File Offset: 0x0006D454
			// (set) Token: 0x060018E2 RID: 6370 RVA: 0x0006F25C File Offset: 0x0006D45C
			public float CurrentWeight { get; set; }

			// Token: 0x17000571 RID: 1393
			// (get) Token: 0x060018E3 RID: 6371 RVA: 0x0006F265 File Offset: 0x0006D465
			// (set) Token: 0x060018E4 RID: 6372 RVA: 0x0006F26D File Offset: 0x0006D46D
			public Composer.AnimatorWeightState WeightState { get; set; }

			// Token: 0x17000572 RID: 1394
			// (get) Token: 0x060018E5 RID: 6373 RVA: 0x0006F276 File Offset: 0x0006D476
			// (set) Token: 0x060018E6 RID: 6374 RVA: 0x0006F27E File Offset: 0x0006D47E
			public TimeSpan CurrentTime { get; set; }

			// Token: 0x17000573 RID: 1395
			// (get) Token: 0x060018E7 RID: 6375 RVA: 0x0006F287 File Offset: 0x0006D487
			// (set) Token: 0x060018E8 RID: 6376 RVA: 0x0006F28F File Offset: 0x0006D48F
			public bool CallbacksCalled { get; set; }
		}
	}
}
