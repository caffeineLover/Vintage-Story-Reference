using System;
using ModernWpf;
using ModernWpf.Controls;

namespace Microsoft.UI.Private.Controls
{
	// Token: 0x02000006 RID: 6
	internal class RepeaterTestHooks
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020BD File Offset: 0x000002BD
		internal void NotifyBuildTreeCompletedImpl()
		{
			TypedEventHandler<object, object> buildTreeCompleted = this.m_buildTreeCompleted;
			if (buildTreeCompleted == null)
			{
				return;
			}
			buildTreeCompleted.Invoke(null, null);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020D1 File Offset: 0x000002D1
		public static int GetElementFactoryElementIndex(object getArgs)
		{
			return ((ElementFactoryGetArgs)getArgs).Index;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020DE File Offset: 0x000002DE
		public static object CreateRepeaterElementFactoryGetArgs()
		{
			return new ElementFactoryGetArgs();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020E5 File Offset: 0x000002E5
		public static object CreateRepeaterElementFactoryRecycleArgs()
		{
			return new ElementFactoryRecycleArgs();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020EC File Offset: 0x000002EC
		public static string GetLayoutId(object layout)
		{
			Layout layout2 = layout as Layout;
			if (layout2 != null)
			{
				return layout2.LayoutId;
			}
			return string.Empty;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002110 File Offset: 0x00000310
		public static void SetLayoutId(object layout, string id)
		{
			Layout layout2 = layout as Layout;
			if (layout2 != null)
			{
				layout2.LayoutId = id;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000C RID: 12 RVA: 0x00002130 File Offset: 0x00000330
		// (remove) Token: 0x0600000D RID: 13 RVA: 0x00002168 File Offset: 0x00000368
		private event TypedEventHandler<object, object> m_buildTreeCompleted;

		// Token: 0x0600000E RID: 14 RVA: 0x0000219D File Offset: 0x0000039D
		private static void EnsureHooks()
		{
			if (RepeaterTestHooks.s_testHooks == null)
			{
				RepeaterTestHooks.s_testHooks = new RepeaterTestHooks();
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600000F RID: 15 RVA: 0x000021B0 File Offset: 0x000003B0
		// (remove) Token: 0x06000010 RID: 16 RVA: 0x000021C2 File Offset: 0x000003C2
		public static event TypedEventHandler<object, object> BuildTreeCompleted
		{
			add
			{
				RepeaterTestHooks.EnsureHooks();
				RepeaterTestHooks.s_testHooks.m_buildTreeCompleted += value;
			}
			remove
			{
				if (RepeaterTestHooks.s_testHooks != null)
				{
					RepeaterTestHooks.s_testHooks.m_buildTreeCompleted -= value;
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021D6 File Offset: 0x000003D6
		private static void NotifyBuildTreeCompleted()
		{
			if (RepeaterTestHooks.s_testHooks != null)
			{
				RepeaterTestHooks.s_testHooks.NotifyBuildTreeCompletedImpl();
			}
		}

		// Token: 0x04000004 RID: 4
		private static RepeaterTestHooks s_testHooks;
	}
}
