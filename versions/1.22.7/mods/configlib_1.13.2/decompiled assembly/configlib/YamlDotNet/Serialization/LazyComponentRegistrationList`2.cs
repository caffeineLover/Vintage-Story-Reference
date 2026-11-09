using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000AA RID: 170
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class LazyComponentRegistrationList<[Nullable(2)] TArgument, [Nullable(2)] TComponent> : IEnumerable<Func<TArgument, TComponent>>, IEnumerable
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x00010450 File Offset: 0x0000E650
		public LazyComponentRegistrationList<TArgument, TComponent> Clone()
		{
			LazyComponentRegistrationList<TArgument, TComponent> lazyComponentRegistrationList = new LazyComponentRegistrationList<TArgument, TComponent>();
			foreach (LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration lazyComponentRegistration in this.entries)
			{
				lazyComponentRegistrationList.entries.Add(lazyComponentRegistration);
			}
			return lazyComponentRegistrationList;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000104B0 File Offset: 0x0000E6B0
		public void Clear()
		{
			this.entries.Clear();
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000104BD File Offset: 0x0000E6BD
		public void Add(Type componentType, Func<TArgument, TComponent> factory)
		{
			this.entries.Add(new LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration(componentType, factory));
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000104D4 File Offset: 0x0000E6D4
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

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00010537 File Offset: 0x0000E737
		public int Count
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00010544 File Offset: 0x0000E744
		public IEnumerable<Func<TArgument, TComponent>> InReverseOrder
		{
			get
			{
				LazyComponentRegistrationList<TArgument, TComponent>.<get_InReverseOrder>d__10 <get_InReverseOrder>d__ = new LazyComponentRegistrationList<TArgument, TComponent>.<get_InReverseOrder>d__10(-2);
				<get_InReverseOrder>d__.<>4__this = this;
				return <get_InReverseOrder>d__;
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00010554 File Offset: 0x0000E754
		public IRegistrationLocationSelectionSyntax<TComponent> CreateRegistrationLocationSelector(Type componentType, Func<TArgument, TComponent> factory)
		{
			return new LazyComponentRegistrationList<TArgument, TComponent>.RegistrationLocationSelector(this, new LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration(componentType, factory));
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00010563 File Offset: 0x0000E763
		public ITrackingRegistrationLocationSelectionSyntax<TComponent> CreateTrackingRegistrationLocationSelector(Type componentType, Func<TComponent, TArgument, TComponent> factory)
		{
			return new LazyComponentRegistrationList<TArgument, TComponent>.TrackingRegistrationLocationSelector(this, new LazyComponentRegistrationList<TArgument, TComponent>.TrackingLazyComponentRegistration(componentType, factory));
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00010572 File Offset: 0x0000E772
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

		// Token: 0x060003DB RID: 987 RVA: 0x000105A3 File Offset: 0x0000E7A3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000105AC File Offset: 0x0000E7AC
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

		// Token: 0x060003DD RID: 989 RVA: 0x000105EB File Offset: 0x0000E7EB
		private void EnsureNoDuplicateRegistrationType(Type componentType)
		{
			if (this.IndexOfRegistration(componentType) != -1)
			{
				throw new InvalidOperationException("A component of type '" + componentType.FullName + "' has already been registered.");
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00010612 File Offset: 0x0000E812
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

		// Token: 0x060003DF RID: 991 RVA: 0x0001064C File Offset: 0x0000E84C
		public LazyComponentRegistrationList()
		{
			this.entries = new List<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration>();
			base..ctor();
		}

		// Token: 0x040001A6 RID: 422
		[Nullable(new byte[]
		{
			1,
			1,
			0,
			0
		})]
		private readonly List<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration> entries;

		// Token: 0x020000AB RID: 171
		[Nullable(0)]
		public sealed class LazyComponentRegistration
		{
			// Token: 0x060003E0 RID: 992 RVA: 0x0001065F File Offset: 0x0000E85F
			public LazyComponentRegistration(Type componentType, Func<TArgument, TComponent> factory)
			{
				this.ComponentType = componentType;
				this.Factory = factory;
			}

			// Token: 0x040001A7 RID: 423
			public readonly Type ComponentType;

			// Token: 0x040001A8 RID: 424
			public readonly Func<TArgument, TComponent> Factory;
		}

		// Token: 0x020000AC RID: 172
		[Nullable(0)]
		public sealed class TrackingLazyComponentRegistration
		{
			// Token: 0x060003E1 RID: 993 RVA: 0x00010675 File Offset: 0x0000E875
			public TrackingLazyComponentRegistration(Type componentType, Func<TComponent, TArgument, TComponent> factory)
			{
				this.ComponentType = componentType;
				this.Factory = factory;
			}

			// Token: 0x040001A9 RID: 425
			public readonly Type ComponentType;

			// Token: 0x040001AA RID: 426
			public readonly Func<TComponent, TArgument, TComponent> Factory;
		}

		// Token: 0x020000AD RID: 173
		[NullableContext(0)]
		private class RegistrationLocationSelector : IRegistrationLocationSelectionSyntax<TComponent>
		{
			// Token: 0x060003E2 RID: 994 RVA: 0x0001068B File Offset: 0x0000E88B
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

			// Token: 0x060003E3 RID: 995 RVA: 0x000106A4 File Offset: 0x0000E8A4
			void IRegistrationLocationSelectionSyntax<!1>.InsteadOf<TRegistrationType>()
			{
				if (this.newRegistration.ComponentType != typeof(TRegistrationType))
				{
					this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				}
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				this.registrations.entries[num] = this.newRegistration;
			}

			// Token: 0x060003E4 RID: 996 RVA: 0x00010708 File Offset: 0x0000E908
			void IRegistrationLocationSelectionSyntax<!1>.After<TRegistrationType>()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				this.registrations.entries.Insert(num + 1, this.newRegistration);
			}

			// Token: 0x060003E5 RID: 997 RVA: 0x00010750 File Offset: 0x0000E950
			void IRegistrationLocationSelectionSyntax<!1>.Before<TRegistrationType>()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				this.registrations.entries.Insert(num, this.newRegistration);
			}

			// Token: 0x060003E6 RID: 998 RVA: 0x00010796 File Offset: 0x0000E996
			void IRegistrationLocationSelectionSyntax<!1>.OnBottom()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				this.registrations.entries.Add(this.newRegistration);
			}

			// Token: 0x060003E7 RID: 999 RVA: 0x000107C4 File Offset: 0x0000E9C4
			void IRegistrationLocationSelectionSyntax<!1>.OnTop()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				this.registrations.entries.Insert(0, this.newRegistration);
			}

			// Token: 0x040001AB RID: 427
			[Nullable(1)]
			private readonly LazyComponentRegistrationList<TArgument, TComponent> registrations;

			// Token: 0x040001AC RID: 428
			[Nullable(new byte[]
			{
				1,
				0,
				0
			})]
			private readonly LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration newRegistration;
		}

		// Token: 0x020000AE RID: 174
		[NullableContext(0)]
		private class TrackingRegistrationLocationSelector : ITrackingRegistrationLocationSelectionSyntax<TComponent>
		{
			// Token: 0x060003E8 RID: 1000 RVA: 0x000107F3 File Offset: 0x0000E9F3
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

			// Token: 0x060003E9 RID: 1001 RVA: 0x0001080C File Offset: 0x0000EA0C
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

			// Token: 0x040001AD RID: 429
			[Nullable(1)]
			private readonly LazyComponentRegistrationList<TArgument, TComponent> registrations;

			// Token: 0x040001AE RID: 430
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
