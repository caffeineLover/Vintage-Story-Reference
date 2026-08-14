using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000AE RID: 174
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class LazyComponentRegistrationList<[Nullable(2)] TArgument, [Nullable(2)] TComponent> : IEnumerable<Func<TArgument, TComponent>>, IEnumerable
	{
		// Token: 0x0600040E RID: 1038 RVA: 0x00010D98 File Offset: 0x0000EF98
		public LazyComponentRegistrationList<TArgument, TComponent> Clone()
		{
			LazyComponentRegistrationList<TArgument, TComponent> lazyComponentRegistrationList = new LazyComponentRegistrationList<TArgument, TComponent>();
			foreach (LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration lazyComponentRegistration in this.entries)
			{
				lazyComponentRegistrationList.entries.Add(lazyComponentRegistration);
			}
			return lazyComponentRegistrationList;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00010DF8 File Offset: 0x0000EFF8
		public void Clear()
		{
			this.entries.Clear();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00010E05 File Offset: 0x0000F005
		public void Add(Type componentType, Func<TArgument, TComponent> factory)
		{
			this.entries.Add(new LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration(componentType, factory));
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00010E1C File Offset: 0x0000F01C
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
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00010E7F File Offset: 0x0000F07F
		public int Count
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00010E8C File Offset: 0x0000F08C
		public IEnumerable<Func<TArgument, TComponent>> InReverseOrder
		{
			get
			{
				LazyComponentRegistrationList<TArgument, TComponent>.<get_InReverseOrder>d__10 <get_InReverseOrder>d__ = new LazyComponentRegistrationList<TArgument, TComponent>.<get_InReverseOrder>d__10(-2);
				<get_InReverseOrder>d__.<>4__this = this;
				return <get_InReverseOrder>d__;
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00010E9C File Offset: 0x0000F09C
		public IRegistrationLocationSelectionSyntax<TComponent> CreateRegistrationLocationSelector(Type componentType, Func<TArgument, TComponent> factory)
		{
			return new LazyComponentRegistrationList<TArgument, TComponent>.RegistrationLocationSelector(this, new LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration(componentType, factory));
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00010EAB File Offset: 0x0000F0AB
		public ITrackingRegistrationLocationSelectionSyntax<TComponent> CreateTrackingRegistrationLocationSelector(Type componentType, Func<TComponent, TArgument, TComponent> factory)
		{
			return new LazyComponentRegistrationList<TArgument, TComponent>.TrackingRegistrationLocationSelector(this, new LazyComponentRegistrationList<TArgument, TComponent>.TrackingLazyComponentRegistration(componentType, factory));
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00010EBA File Offset: 0x0000F0BA
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

		// Token: 0x06000417 RID: 1047 RVA: 0x00010EEB File Offset: 0x0000F0EB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00010EF4 File Offset: 0x0000F0F4
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

		// Token: 0x06000419 RID: 1049 RVA: 0x00010F33 File Offset: 0x0000F133
		private void EnsureNoDuplicateRegistrationType(Type componentType)
		{
			if (this.IndexOfRegistration(componentType) != -1)
			{
				throw new InvalidOperationException("A component of type '" + componentType.FullName + "' has already been registered.");
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00010F5A File Offset: 0x0000F15A
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

		// Token: 0x0600041B RID: 1051 RVA: 0x00010F94 File Offset: 0x0000F194
		public LazyComponentRegistrationList()
		{
			this.entries = new List<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration>();
			base..ctor();
		}

		// Token: 0x040001D4 RID: 468
		[Nullable(new byte[]
		{
			1,
			1,
			0,
			0
		})]
		private readonly List<LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration> entries;

		// Token: 0x020000AF RID: 175
		[Nullable(0)]
		public sealed class LazyComponentRegistration
		{
			// Token: 0x0600041C RID: 1052 RVA: 0x00010FA7 File Offset: 0x0000F1A7
			public LazyComponentRegistration(Type componentType, Func<TArgument, TComponent> factory)
			{
				this.ComponentType = componentType;
				this.Factory = factory;
			}

			// Token: 0x040001D5 RID: 469
			public readonly Type ComponentType;

			// Token: 0x040001D6 RID: 470
			public readonly Func<TArgument, TComponent> Factory;
		}

		// Token: 0x020000B0 RID: 176
		[Nullable(0)]
		public sealed class TrackingLazyComponentRegistration
		{
			// Token: 0x0600041D RID: 1053 RVA: 0x00010FBD File Offset: 0x0000F1BD
			public TrackingLazyComponentRegistration(Type componentType, Func<TComponent, TArgument, TComponent> factory)
			{
				this.ComponentType = componentType;
				this.Factory = factory;
			}

			// Token: 0x040001D7 RID: 471
			public readonly Type ComponentType;

			// Token: 0x040001D8 RID: 472
			public readonly Func<TComponent, TArgument, TComponent> Factory;
		}

		// Token: 0x020000B1 RID: 177
		[NullableContext(0)]
		private class RegistrationLocationSelector : IRegistrationLocationSelectionSyntax<TComponent>
		{
			// Token: 0x0600041E RID: 1054 RVA: 0x00010FD3 File Offset: 0x0000F1D3
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

			// Token: 0x0600041F RID: 1055 RVA: 0x00010FEC File Offset: 0x0000F1EC
			void IRegistrationLocationSelectionSyntax<!1>.InsteadOf<TRegistrationType>()
			{
				if (this.newRegistration.ComponentType != typeof(TRegistrationType))
				{
					this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				}
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				this.registrations.entries[num] = this.newRegistration;
			}

			// Token: 0x06000420 RID: 1056 RVA: 0x00011050 File Offset: 0x0000F250
			void IRegistrationLocationSelectionSyntax<!1>.After<TRegistrationType>()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				this.registrations.entries.Insert(num + 1, this.newRegistration);
			}

			// Token: 0x06000421 RID: 1057 RVA: 0x00011098 File Offset: 0x0000F298
			void IRegistrationLocationSelectionSyntax<!1>.Before<TRegistrationType>()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				int num = this.registrations.EnsureRegistrationExists<TRegistrationType>();
				this.registrations.entries.Insert(num, this.newRegistration);
			}

			// Token: 0x06000422 RID: 1058 RVA: 0x000110DE File Offset: 0x0000F2DE
			void IRegistrationLocationSelectionSyntax<!1>.OnBottom()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				this.registrations.entries.Add(this.newRegistration);
			}

			// Token: 0x06000423 RID: 1059 RVA: 0x0001110C File Offset: 0x0000F30C
			void IRegistrationLocationSelectionSyntax<!1>.OnTop()
			{
				this.registrations.EnsureNoDuplicateRegistrationType(this.newRegistration.ComponentType);
				this.registrations.entries.Insert(0, this.newRegistration);
			}

			// Token: 0x040001D9 RID: 473
			[Nullable(1)]
			private readonly LazyComponentRegistrationList<TArgument, TComponent> registrations;

			// Token: 0x040001DA RID: 474
			[Nullable(new byte[]
			{
				1,
				0,
				0
			})]
			private readonly LazyComponentRegistrationList<TArgument, TComponent>.LazyComponentRegistration newRegistration;
		}

		// Token: 0x020000B2 RID: 178
		[NullableContext(0)]
		private class TrackingRegistrationLocationSelector : ITrackingRegistrationLocationSelectionSyntax<TComponent>
		{
			// Token: 0x06000424 RID: 1060 RVA: 0x0001113B File Offset: 0x0000F33B
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

			// Token: 0x06000425 RID: 1061 RVA: 0x00011154 File Offset: 0x0000F354
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

			// Token: 0x040001DB RID: 475
			[Nullable(1)]
			private readonly LazyComponentRegistrationList<TArgument, TComponent> registrations;

			// Token: 0x040001DC RID: 476
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
