using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200004C RID: 76
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DynamicComponentProxy
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00004EF7 File Offset: 0x000030F7
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00004EFF File Offset: 0x000030FF
		internal Action<object> SetState { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00004F08 File Offset: 0x00003108
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00004F10 File Offset: 0x00003110
		internal Func<object> GetState { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00004F19 File Offset: 0x00003119
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00004F21 File Offset: 0x00003121
		internal Func<DynamicContext, DynamicComponentComposeResult> Compose { get; private set; }

		// Token: 0x060001DB RID: 475 RVA: 0x00004F2C File Offset: 0x0000312C
		[NullableContext(0)]
		[return: Nullable(1)]
		internal static DynamicComponentProxy CreateFrom<TState>([Nullable(new byte[]
		{
			1,
			0
		})] IDynamicComponent<TState> component) where TState : struct
		{
			DynamicComponentProxy.<>c__DisplayClass12_0<TState> CS$<>8__locals1 = new DynamicComponentProxy.<>c__DisplayClass12_0<TState>();
			CS$<>8__locals1.component = component;
			return new DynamicComponentProxy
			{
				GetState = new Func<object>(CS$<>8__locals1.<CreateFrom>b__0),
				SetState = new Action<object>(CS$<>8__locals1.<CreateFrom>b__1),
				Compose = new Func<DynamicContext, DynamicComponentComposeResult>(CS$<>8__locals1.component.Compose)
			};
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00004F88 File Offset: 0x00003188
		internal static DynamicComponentProxy CreateFrom(IDynamicComponent component)
		{
			DynamicComponentProxy dynamicComponentProxy = new DynamicComponentProxy();
			dynamicComponentProxy.GetState = (() => null);
			dynamicComponentProxy.SetState = delegate(object _)
			{
			};
			dynamicComponentProxy.Compose = new Func<DynamicContext, DynamicComponentComposeResult>(component.Compose);
			return dynamicComponentProxy;
		}
	}
}
