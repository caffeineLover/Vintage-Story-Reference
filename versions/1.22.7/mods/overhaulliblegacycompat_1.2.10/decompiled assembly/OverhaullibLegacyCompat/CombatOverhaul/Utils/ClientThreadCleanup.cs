using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace CombatOverhaul.Utils
{
	// Token: 0x0200001F RID: 31
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ClientThreadCleanup
	{
		// Token: 0x06000225 RID: 549 RVA: 0x00009308 File Offset: 0x00007508
		[NullableContext(2)]
		public static void DisposeRenderer(ICoreClientAPI clientApi, IRenderer renderer, EnumRenderStage stage, [Nullable(1)] string taskCode)
		{
			if (clientApi == null || renderer == null)
			{
				return;
			}
			ClientThreadCleanup.Run(clientApi, delegate
			{
				try
				{
					clientApi.Event.UnregisterRenderer(renderer, stage);
				}
				finally
				{
					renderer.Dispose();
				}
			}, taskCode);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000935C File Offset: 0x0000755C
		public static void Run(ICoreClientAPI clientApi, Action cleanup, string taskCode)
		{
			if (RuntimeEnv.MainThreadId == Environment.CurrentManagedThreadId)
			{
				ClientThreadCleanup.RunSafely(clientApi, cleanup, taskCode);
				return;
			}
			try
			{
				clientApi.Event.EnqueueMainThreadTask(delegate()
				{
					ClientThreadCleanup.RunSafely(clientApi, cleanup, taskCode);
				}, taskCode);
			}
			catch (Exception exception)
			{
				ILogger logger = clientApi.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(80, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[OverhaullibLegacyCompat] Could not queue client cleanup '");
				defaultInterpolatedStringHandler.AppendFormatted(taskCode);
				defaultInterpolatedStringHandler.AppendLiteral("' on the main thread: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009430 File Offset: 0x00007630
		private static void RunSafely(ICoreClientAPI clientApi, Action cleanup, string taskCode)
		{
			try
			{
				cleanup.Invoke();
			}
			catch (Exception exception)
			{
				ILogger logger = clientApi.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(52, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[OverhaullibLegacyCompat] Client cleanup '");
				defaultInterpolatedStringHandler.AppendFormatted(taskCode);
				defaultInterpolatedStringHandler.AppendLiteral("' failed: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}
	}
}
