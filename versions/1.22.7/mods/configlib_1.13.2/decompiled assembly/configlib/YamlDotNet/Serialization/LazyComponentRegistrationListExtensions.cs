using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000B2 RID: 178
	[NullableContext(1)]
	[Nullable(0)]
	internal static class LazyComponentRegistrationListExtensions
	{
		// Token: 0x060003F7 RID: 1015 RVA: 0x000109DB File Offset: 0x0000EBDB
		public static TComponent BuildComponentChain<[Nullable(2)] TComponent>(this LazyComponentRegistrationList<TComponent, TComponent> registrations, TComponent innerComponent)
		{
			IEnumerable<Func<TComponent, TComponent>> inReverseOrder = registrations.InReverseOrder;
			Func<TComponent, Func<TComponent, TComponent>, TComponent> func;
			if ((func = LazyComponentRegistrationListExtensions.<>c__0<TComponent>.<>9__0_0) == null)
			{
				func = (LazyComponentRegistrationListExtensions.<>c__0<TComponent>.<>9__0_0 = new Func<TComponent, Func<TComponent, TComponent>, TComponent>(LazyComponentRegistrationListExtensions.<>c__0<TComponent>.<>9.<BuildComponentChain>b__0_0));
			}
			return Enumerable.Aggregate<Func<TComponent, TComponent>, TComponent>(inReverseOrder, innerComponent, func);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00010A08 File Offset: 0x0000EC08
		public static TComponent BuildComponentChain<[Nullable(2)] TArgument, [Nullable(2)] TComponent>(this LazyComponentRegistrationList<TArgument, TComponent> registrations, TComponent innerComponent, Func<TComponent, TArgument> argumentBuilder)
		{
			LazyComponentRegistrationListExtensions.<>c__DisplayClass1_0<TArgument, TComponent> CS$<>8__locals1 = new LazyComponentRegistrationListExtensions.<>c__DisplayClass1_0<TArgument, TComponent>();
			CS$<>8__locals1.argumentBuilder = argumentBuilder;
			return Enumerable.Aggregate<Func<TArgument, TComponent>, TComponent>(registrations.InReverseOrder, innerComponent, new Func<TComponent, Func<TArgument, TComponent>, TComponent>(CS$<>8__locals1.<BuildComponentChain>b__0));
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00010A3A File Offset: 0x0000EC3A
		public static List<TComponent> BuildComponentList<[Nullable(2)] TComponent>(this LazyComponentRegistrationList<Nothing, TComponent> registrations)
		{
			Func<Func<Nothing, TComponent>, TComponent> func;
			if ((func = LazyComponentRegistrationListExtensions.<>c__2<TComponent>.<>9__2_0) == null)
			{
				func = (LazyComponentRegistrationListExtensions.<>c__2<TComponent>.<>9__2_0 = new Func<Func<Nothing, TComponent>, TComponent>(LazyComponentRegistrationListExtensions.<>c__2<TComponent>.<>9.<BuildComponentList>b__2_0));
			}
			return Enumerable.ToList<TComponent>(Enumerable.Select<Func<Nothing, TComponent>, TComponent>(registrations, func));
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00010A68 File Offset: 0x0000EC68
		public static List<TComponent> BuildComponentList<[Nullable(2)] TArgument, [Nullable(2)] TComponent>(this LazyComponentRegistrationList<TArgument, TComponent> registrations, TArgument argument)
		{
			LazyComponentRegistrationListExtensions.<>c__DisplayClass3_0<TArgument, TComponent> CS$<>8__locals1 = new LazyComponentRegistrationListExtensions.<>c__DisplayClass3_0<TArgument, TComponent>();
			CS$<>8__locals1.argument = argument;
			return Enumerable.ToList<TComponent>(Enumerable.Select<Func<TArgument, TComponent>, TComponent>(registrations, new Func<Func<TArgument, TComponent>, TComponent>(CS$<>8__locals1.<BuildComponentList>b__0)));
		}
	}
}
