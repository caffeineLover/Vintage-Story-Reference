using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000030 RID: 48
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class LazyComponentRegistrationList<[Nullable(2)] TArgument, [Nullable(2)] TComponent> : IEnumerable<Func<TArgument, TComponent>>, IEnumerable
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00003A0C File Offset: 0x00001C0C
		public LazyComponentRegistrationList<TArgument, TComponent> Clone()
		{
			LazyComponentRegistrationList<TArgument, TComponent> lazyComponentRegistrationList = new LazyComponentRegistrationList<TArgument, TComponent>();
			foreach (LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration lazyComponentRegistration in this.entries)
			{
				lazyComponentRegistrationList.entries.Add(lazyComponentRegistration);
			}
			return lazyComponentRegistrationList;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00003A6C File Offset: 0x00001C6C
		public void Clear()
		{
			this.entries.Clear();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00003A79 File Offset: 0x00001C79
		public void Add(Type componentType, Func<TArgument, TComponent> factory)
		{
			this.entries.Add(new LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration(componentType, factory));
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00003A90 File Offset: 0x00001C90
		public void Remove(Type componentType)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ComponentType == componentType)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
			throw new KeyNotFoundException("A component registration of type '" + componentType.FullName + "' was not found.");
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00003AF3 File Offset: 0x00001CF3
		public int Count
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00003B00 File Offset: 0x00001D00
		public IEnumerable<Func<TArgument, TComponent>> InReverseOrder
		{
			get
			{
				LazyComponentRegistrationList<TArgument, TComponent>.<get_InReverseOrder>d__10 <get_InReverseOrder>d__ = new LazyComponentRegistrationList<TArgument, TComponent>.<get_InReverseOrder>d__10(-2);
				<get_InReverseOrder>d__.<>4__this = this;
				return <get_InReverseOrder>d__;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00003B1D File Offset: 0x00001D1D
		public IRegistrationLocationSelectionSyntax<TComponent> CreateRegistrationLocationSelector(Type componentType, Func<TArgument, TComponent> factory)
		{
			return new LazyComponentRegistrationList<TArgument, TComponent>.RegistrationLocationSelector(this, new LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration(componentType, factory));
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003B2C File Offset: 0x00001D2C
		public ITrackingRegistrationLocationSelectionSyntax<TComponent> CreateTrackingRegistrationLocationSelector(Type componentType, Func<TComponent, TArgument, TComponent> factory)
		{
			return new LazyComponentRegistrationList<TArgument, TComponent>.TrackingRegistrationLocationSelector(this, new LazyComponentRegistrationList<TArgument, TComponent>.TrackingLazyComponentRegistration(componentType, factory));
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00003B3B File Offset: 0x00001D3B
		public IEnumerator<Func<TArgument, TComponent>> GetEnumerator()
		{
			IEnumerable<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration> enumerable = this.entries;
			Func<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration, Func<TArgument, TComponent>> func;
			if ((func = LazyComponentRegistrationList<TArgument, TComponent>.<>c.<>9__13_0) == null)
			{
				func = (LazyComponentRegistrationList<TArgument, TComponent>.<>c.<>9__13_0 = new Func<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration, Func<TArgument, TComponent>>(LazyComponentRegistrationList<TArgument, TComponent>.<>c.<>9.<GetEnumerator>b__13_0));
			}
			return Enumerable.Select<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration, Func<TArgument, TComponent>>(enumerable, func).GetEnumerator();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00003B6C File Offset: 0x00001D6C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00003B74 File Offset: 0x00001D74
		private int IndexOfRegistration(Type registrationType)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (registrationType == this.entries[i].ComponentType)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00003BB3 File Offset: 0x00001DB3
		private void EnsureNoDuplicateRegistrationType(Type componentType)
		{
			if (this.IndexOfRegistration(componentType) != -1)
			{
				throw new InvalidOperationException("A component of type '" + componentType.FullName + "' has already been registered.");
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00003BDC File Offset: 0x00001DDC
		[NullableContext(2)]
		private int EnsureRegistrationExists<TRegistrationType>()
		{
			int num = this.IndexOfRegistration(typeof(TRegistrationType));
			if (num == -1)
			{
				throw new InvalidOperationException("A component of type '" + typeof(TRegistrationType).FullName + "' has not been registered.");
			}
			return num;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00003C23 File Offset: 0x00001E23
		public LazyComponentRegistrationList()
		{
			this.entries = new List<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration>();
			base..ctor();
		}

		// Token: 0x04000033 RID: 51
		[Nullable(new byte[]
		{
			1,
			1,
			0,
			0
		})]
		private readonly List<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration> entries;

		// Token: 0x02000125 RID: 293
		[Nullable(0)]
		public sealed class LazyComponentRegistration
		{
			// Token: 0x060006F2 RID: 1778 RVA: 0x00017FA7 File Offset: 0x000161A7
			public LazyComponentRegistration(Type componentType, Func<TArgument, TComponent> factory)
			{
				this.ComponentType = componentType;
				this.Factory = factory;
			}

			// Token: 0x04000230 RID: 560
			public readonly Type ComponentType;

			// Token: 0x04000231 RID: 561
			public readonly Func<TArgument, TComponent> Factory;
		}

		// Token: 0x02000126 RID: 294
		[Nullable(0)]
		public sealed class TrackingLazyComponentRegistration
		{
			// Token: 0x060006F3 RID: 1779 RVA: 0x00017FBD File Offset: 0x000161BD
			public TrackingLazyComponentRegistration(Type componentType, Func<TComponent, TArgument, TComponent> factory)
			{
				this.ComponentType = componentType;
				this.Factory = factory;
			}

			// Token: 0x04000232 RID: 562
			public readonly Type ComponentType;

			// Token: 0x04000233 RID: 563
			public readonly Func<TComponent, TArgument, TComponent> Factory;
		}

		// Token: 0x02000127 RID: 295
		[NullableContext(0)]
		private class RegistrationLocationSelector : IRegistrationLocationSelectionSyntax<TComponent>
		{
			// Token: 0x060006F4 RID: 1780 RVA: 0x00017FD3 File Offset: 0x000161D3
			[NullableContext(1)]
			public RegistrationLocationSelector(LazyComponentRegistrationList<TArgument, TComponent> registrations, [Nullable(new byte[]
			{
				1,
				0,
				0
			})] LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration newRegistration)
			{
				this.registrations = registrations;
				this.newRegistration = newRegistration;
			}

			// Token: 0x060006F5 RID: 1781 RVA: 0x00017FEC File Offset: 0x000161EC
			void IRegistrationLocationSelectionSyntax<!1>.InsteadOf<TRegistrationType>()
			{
				if (this.newRegistration.ComponentType != typeof(TRegistrationType))
				{
					this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				}
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				this.registrations.entries[num] = this.newRegistration;
			}

			// Token: 0x060006F6 RID: 1782 RVA: 0x00018050 File Offset: 0x00016250
			void IRegistrationLocationSelectionSyntax<!1>.After<TRegistrationType>()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				this.registrations.entries.Insert(num + 1, this.newRegistration);
			}

			// Token: 0x060006F7 RID: 1783 RVA: 0x00018098 File Offset: 0x00016298
			void IRegistrationLocationSelectionSyntax<!1>.Before<TRegistrationType>()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				this.registrations.entries.Insert(num, this.newRegistration);
			}

			// Token: 0x060006F8 RID: 1784 RVA: 0x000180DE File Offset: 0x000162DE
			void IRegistrationLocationSelectionSyntax<!1>.OnBottom()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				this.registrations.entries.Add(this.newRegistration);
			}

			// Token: 0x060006F9 RID: 1785 RVA: 0x0001810C File Offset: 0x0001630C
			void IRegistrationLocationSelectionSyntax<!1>.OnTop()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				this.registrations.entries.Insert(0, this.newRegistration);
			}

			// Token: 0x04000234 RID: 564
			[Nullable(1)]
			private readonly LazyComponentRegistrationList<TArgument, TComponent> registrations;

			// Token: 0x04000235 RID: 565
			[Nullable(new byte[]
			{
				1,
				0,
				0
			})]
			private readonly LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration newRegistration;
		}

		// Token: 0x02000128 RID: 296
		[NullableContext(0)]
		private class TrackingRegistrationLocationSelector : ITrackingRegistrationLocationSelectionSyntax<TComponent>
		{
			// Token: 0x060006FA RID: 1786 RVA: 0x0001813B File Offset: 0x0001633B
			[NullableContext(1)]
			public TrackingRegistrationLocationSelector(LazyComponentRegistrationList<TArgument, TComponent> registrations, [Nullable(new byte[]
			{
				1,
				0,
				0
			})] LazyComponentRegistrationList<TArgument, TComponent>.TrackingLazyComponentRegistration newRegistration)
			{
				this.registrations = registrations;
				this.newRegistration = newRegistration;
			}

			// Token: 0x060006FB RID: 1787 RVA: 0x00018154 File Offset: 0x00016354
			void ITrackingRegistrationLocationSelectionSyntax<!1>.InsteadOf<TRegistrationType>()
			{
				LazyComponentRegistrationList<TArgument, TComponent>.TrackingRegistrationLocationSelector.<>c__DisplayClass3_0<TRegistrationType> CS$<>8__locals1 = new LazyComponentRegistrationList<TArgument, TComponent>.TrackingRegistrationLocationSelector.<>c__DisplayClass3_0<TRegistrationType>();
				CS$<>8__locals1.<>4__this = this;
				if (this.newRegistration.ComponentType != typeof(TRegistrationType))
				{
					this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				}
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				CS$<>8__locals1.innerComponentFactory = this.registrations.entries[num].Factory;
				this.registrations.entries[num] = new LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration(this.newRegistration.ComponentType, new Func<TArgument, TComponent>(CS$<>8__locals1.<YamlDotNet.Serialization.ITrackingRegistrationLocationSelectionSyntax<TComponent>.InsteadOf>b__0));
			}

			// Token: 0x04000236 RID: 566
			[Nullable(1)]
			private readonly LazyComponentRegistrationList<TArgument, TComponent> registrations;

			// Token: 0x04000237 RID: 567
			[Nullable(new byte[]
			{
				1,
				0,
				0
			})]
			private readonly LazyComponentRegistrationList<TArgument, TComponent>.TrackingLazyComponentRegistration newRegistration;
		}
	}
}
