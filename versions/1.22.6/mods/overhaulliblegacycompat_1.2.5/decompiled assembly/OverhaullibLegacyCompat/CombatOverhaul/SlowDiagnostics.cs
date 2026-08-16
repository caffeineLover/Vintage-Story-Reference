using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Common;
using Vintagestory.Server;

namespace CombatOverhaul
{
	// Token: 0x02000004 RID: 4
	[NullableContext(1)]
	[Nullable(0)]
	internal static class SlowDiagnostics
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000021A0 File Offset: 0x000003A0
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000021A7 File Offset: 0x000003A7
		internal static bool Enabled { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000025 RID: 37 RVA: 0x000021B0 File Offset: 0x000003B0
		// (remove) Token: 0x06000026 RID: 38 RVA: 0x000021E4 File Offset: 0x000003E4
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		internal static event Action<string> DiagnosticLogWritten;

		// Token: 0x06000027 RID: 39 RVA: 0x00002218 File Offset: 0x00000418
		internal static void Configure(ICoreAPI api, Settings settings, string harmonyId)
		{
			if (api.Side != 1)
			{
				return;
			}
			SlowDiagnostics.Api = api;
			SlowDiagnostics.HarmonyId = harmonyId;
			SlowDiagnostics.NetworkThresholdTicks = SlowDiagnostics.MillisecondsToStopwatchTicks(settings.SlowDiagnosticsNetworkThresholdMs);
			SlowDiagnostics.RangedStatusThresholdTicks = SlowDiagnostics.MillisecondsToStopwatchTicks(settings.SlowDiagnosticsRangedStatusThresholdMs);
			SlowDiagnostics.LogCooldownTicks = SlowDiagnostics.MillisecondsToStopwatchTicks(settings.SlowDiagnosticsLogCooldownMs);
			if (settings.SlowDiagnosticsEnabled)
			{
				SlowDiagnostics.Enabled = true;
				SlowDiagnostics.PatchNetwork(harmonyId);
				return;
			}
			SlowDiagnostics.Enabled = false;
			SlowDiagnostics.UnpatchNetwork(harmonyId);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000228C File Offset: 0x0000048C
		internal static void Shutdown(string harmonyId)
		{
			SlowDiagnostics.Enabled = false;
			SlowDiagnostics.UnpatchNetwork(harmonyId);
			SlowDiagnostics.Api = null;
			SlowDiagnostics.HarmonyId = null;
			SlowDiagnostics.RateLimiter.Clear();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000022B0 File Offset: 0x000004B0
		internal static void InvokeRangedWeaponStatusSubscribers([Nullable(2)] RangedWeaponSystemServer.RangedWeaponStatusChangedDelegate subscribers, IServerPlayer player, ItemSlot weaponSlot, RangedWeaponStatus status, bool mainHand)
		{
			if (subscribers == null)
			{
				return;
			}
			foreach (RangedWeaponSystemServer.RangedWeaponStatusChangedDelegate subscriber in subscribers.GetInvocationList())
			{
				long startedAt = Stopwatch.GetTimestamp();
				try
				{
					subscriber(player.Entity, weaponSlot, status);
				}
				finally
				{
					SlowDiagnostics.RecordRangedStatusSubscriber(subscriber, player, weaponSlot, status, mainHand, startedAt);
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002314 File Offset: 0x00000514
		internal static SlowCustomPacketDetails DescribeCustomPacket(NetworkChannel channel, Packet_CustomPacket packet)
		{
			string channelName = channel.ChannelName;
			int channelId = packet.ChannelId;
			int messageId = packet.MessageId;
			string messageTypeName = SlowDiagnostics.ResolveMessageTypeName(channel, packet.MessageId);
			byte[] data = packet.Data;
			return new SlowCustomPacketDetails(channelName, channelId, messageId, messageTypeName, (data != null) ? data.Length : 0, SlowDiagnostics.ResolveHandlerDescription(channel, packet.MessageId));
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002354 File Offset: 0x00000554
		private static void PatchNetwork(string harmonyId)
		{
			if (SlowDiagnostics.NetworkPatched)
			{
				return;
			}
			MethodInfo target = AccessTools.Method(typeof(NetworkChannel), "OnPacket", null, null);
			MethodInfo prefix = AccessTools.Method(typeof(SlowDiagnostics), "NetworkPacketPrefix", null, null);
			MethodInfo finalizer = AccessTools.Method(typeof(SlowDiagnostics), "NetworkPacketFinalizer", null, null);
			if (target == null || prefix == null || finalizer == null)
			{
				LoggerUtil.Warn(SlowDiagnostics.Api, typeof(SlowDiagnostics), "Slow diagnostics could not find NetworkChannel.OnPacket patch methods.");
				return;
			}
			try
			{
				new Harmony(harmonyId).Patch(target, new HarmonyMethod(prefix), null, null, new HarmonyMethod(finalizer));
				SlowDiagnostics.NetworkPatched = true;
			}
			catch (Exception exception)
			{
				ICoreAPI api = SlowDiagnostics.Api;
				Type typeFromHandle = typeof(SlowDiagnostics);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(58, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Slow diagnostics failed to patch NetworkChannel.OnPacket:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Warn(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002454 File Offset: 0x00000654
		private static void UnpatchNetwork(string harmonyId)
		{
			if (!SlowDiagnostics.NetworkPatched)
			{
				return;
			}
			MethodInfo target = AccessTools.Method(typeof(NetworkChannel), "OnPacket", null, null);
			if (target != null)
			{
				Harmony harmony = new Harmony(harmonyId);
				harmony.Unpatch(target, 1, harmonyId);
				harmony.Unpatch(target, 4, harmonyId);
			}
			SlowDiagnostics.NetworkPatched = false;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000024A6 File Offset: 0x000006A6
		private static void NetworkPacketPrefix(out long __state)
		{
			__state = (SlowDiagnostics.Enabled ? Stopwatch.GetTimestamp() : 0L);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000024BA File Offset: 0x000006BA
		[return: Nullable(2)]
		private static Exception NetworkPacketFinalizer(NetworkChannel __instance, Packet_CustomPacket __0, IServerPlayer __1, long __state, [Nullable(2)] Exception __exception)
		{
			if (__state != 0L)
			{
				SlowDiagnostics.RecordCustomPacket(__instance, __0, __1, __state, __exception);
			}
			return __exception;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000024CC File Offset: 0x000006CC
		private static void RecordCustomPacket(NetworkChannel channel, Packet_CustomPacket packet, IServerPlayer player, long startedAt, [Nullable(2)] Exception exception)
		{
			long elapsedTicks = Stopwatch.GetTimestamp() - startedAt;
			if (elapsedTicks < SlowDiagnostics.NetworkThresholdTicks)
			{
				return;
			}
			SlowCustomPacketDetails details = SlowDiagnostics.DescribeCustomPacket(channel, packet);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(9, 3);
			defaultInterpolatedStringHandler.AppendLiteral("custom:");
			defaultInterpolatedStringHandler.AppendFormatted<int>(details.ChannelId);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted<int>(details.MessageId);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(details.HandlerDescription);
			string key = defaultInterpolatedStringHandler.ToStringAndClear();
			if (!SlowDiagnostics.RateLimiter.ShouldLog(key, Stopwatch.GetTimestamp(), SlowDiagnostics.LogCooldownTicks))
			{
				return;
			}
			string exceptionText = (exception == null) ? "" : (" exception=" + exception.GetType().FullName + ": " + exception.Message);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(105, 9);
			defaultInterpolatedStringHandler2.AppendLiteral("Slow custom packet ");
			defaultInterpolatedStringHandler2.AppendFormatted<double>(SlowDiagnostics.StopwatchTicksToMilliseconds(elapsedTicks), "F2");
			defaultInterpolatedStringHandler2.AppendLiteral(" ms ");
			defaultInterpolatedStringHandler2.AppendLiteral("channel='");
			defaultInterpolatedStringHandler2.AppendFormatted(details.ChannelName);
			defaultInterpolatedStringHandler2.AppendLiteral("' channelId=");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(details.ChannelId);
			defaultInterpolatedStringHandler2.AppendLiteral(" messageId=");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(details.MessageId);
			defaultInterpolatedStringHandler2.AppendLiteral(" ");
			defaultInterpolatedStringHandler2.AppendLiteral("messageType='");
			defaultInterpolatedStringHandler2.AppendFormatted(details.MessageTypeName);
			defaultInterpolatedStringHandler2.AppendLiteral("' payloadBytes=");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(details.PayloadBytes);
			defaultInterpolatedStringHandler2.AppendLiteral(" ");
			defaultInterpolatedStringHandler2.AppendLiteral("player='");
			defaultInterpolatedStringHandler2.AppendFormatted(SlowDiagnostics.FormatPlayer(player));
			defaultInterpolatedStringHandler2.AppendLiteral("' handler='");
			defaultInterpolatedStringHandler2.AppendFormatted(details.HandlerDescription);
			defaultInterpolatedStringHandler2.AppendLiteral("'");
			defaultInterpolatedStringHandler2.AppendFormatted(exceptionText);
			SlowDiagnostics.LogVerbose(defaultInterpolatedStringHandler2.ToStringAndClear());
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000026BC File Offset: 0x000008BC
		private static void RecordRangedStatusSubscriber(RangedWeaponSystemServer.RangedWeaponStatusChangedDelegate subscriber, IServerPlayer player, ItemSlot weaponSlot, RangedWeaponStatus status, bool mainHand, long startedAt)
		{
			long elapsedTicks = Stopwatch.GetTimestamp() - startedAt;
			if (elapsedTicks < SlowDiagnostics.RangedStatusThresholdTicks)
			{
				return;
			}
			string subscriberDescription = SlowDiagnostics.DescribeDelegate(subscriber);
			string key = "ranged-status:" + subscriberDescription;
			if (!SlowDiagnostics.RateLimiter.ShouldLog(key, Stopwatch.GetTimestamp(), SlowDiagnostics.LogCooldownTicks))
			{
				return;
			}
			ItemStack itemstack = weaponSlot.Itemstack;
			string text;
			if (itemstack == null)
			{
				text = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code = collectible.Code;
					text = ((code != null) ? code.ToString() : null);
				}
			}
			string itemCode = text ?? "<empty>";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(79, 6);
			defaultInterpolatedStringHandler.AppendLiteral("Slow ranged status subscriber ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(SlowDiagnostics.StopwatchTicksToMilliseconds(elapsedTicks), "F2");
			defaultInterpolatedStringHandler.AppendLiteral(" ms ");
			defaultInterpolatedStringHandler.AppendLiteral("status=");
			defaultInterpolatedStringHandler.AppendFormatted<RangedWeaponStatus>(status);
			defaultInterpolatedStringHandler.AppendLiteral(" hand=");
			defaultInterpolatedStringHandler.AppendFormatted(mainHand ? "main" : "off");
			defaultInterpolatedStringHandler.AppendLiteral(" item='");
			defaultInterpolatedStringHandler.AppendFormatted(itemCode);
			defaultInterpolatedStringHandler.AppendLiteral("' ");
			defaultInterpolatedStringHandler.AppendLiteral("player='");
			defaultInterpolatedStringHandler.AppendFormatted(SlowDiagnostics.FormatPlayer(player));
			defaultInterpolatedStringHandler.AppendLiteral("' subscriber='");
			defaultInterpolatedStringHandler.AppendFormatted(subscriberDescription);
			defaultInterpolatedStringHandler.AppendLiteral("'");
			SlowDiagnostics.LogVerbose(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000280C File Offset: 0x00000A0C
		private static string ResolveMessageTypeName(NetworkChannel channel, int messageId)
		{
			FieldInfo messageTypesField = SlowDiagnostics.MessageTypesField;
			Dictionary<Type, int> messageTypes = ((messageTypesField != null) ? messageTypesField.GetValue(channel) : null) as Dictionary<Type, int>;
			if (messageTypes != null)
			{
				foreach (KeyValuePair<Type, int> keyValuePair in messageTypes)
				{
					Type type2;
					int num;
					keyValuePair.Deconstruct(ref type2, ref num);
					Type type = type2;
					if (num == messageId)
					{
						return type.FullName ?? type.Name;
					}
				}
			}
			return "<unknown>";
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000028A0 File Offset: 0x00000AA0
		private static string ResolveHandlerDescription(NetworkChannel channel, int messageId)
		{
			FieldInfo handlersField = SlowDiagnostics.HandlersField;
			Action<Packet_CustomPacket, IServerPlayer>[] handlers = ((handlersField != null) ? handlersField.GetValue(channel) : null) as Action<Packet_CustomPacket, IServerPlayer>[];
			if (handlers != null && messageId >= 0 && messageId < handlers.Length)
			{
				return SlowDiagnostics.DescribeDelegate(SlowDiagnostics.UnwrapCapturedHandler(handlers[messageId]));
			}
			return "<none>";
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000028E8 File Offset: 0x00000AE8
		[NullableContext(2)]
		private static Delegate UnwrapCapturedHandler(Delegate handler)
		{
			if (handler == null || handler.Target == null)
			{
				return handler;
			}
			foreach (FieldInfo field in handler.Target.GetType().GetFields(52))
			{
				if (typeof(Delegate).IsAssignableFrom(field.FieldType))
				{
					Delegate captured = field.GetValue(handler.Target) as Delegate;
					if (captured != null && captured != handler)
					{
						return captured;
					}
				}
			}
			return handler;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000295C File Offset: 0x00000B5C
		private static string DescribeDelegate([Nullable(2)] Delegate handler)
		{
			if (handler == null)
			{
				return "<none>";
			}
			MethodInfo method = handler.Method;
			Type type;
			if ((type = method.DeclaringType) == null)
			{
				object target = handler.Target;
				type = ((target != null) ? target.GetType() : null);
			}
			Type type2 = type;
			string text;
			if ((text = ((type2 != null) ? type2.Assembly.GetName().Name : null)) == null)
			{
				text = (method.Module.Assembly.GetName().Name ?? "<unknown>");
			}
			string assemblyName = text;
			string typeName = ((type2 != null) ? type2.FullName : null) ?? "<unknown>";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted(assemblyName);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(typeName);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted(method.Name);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002A2A File Offset: 0x00000C2A
		private static string FormatPlayer(IServerPlayer player)
		{
			return (string.IsNullOrEmpty(player.PlayerName) ? "<unknown>" : player.PlayerName) + "/" + player.PlayerUID;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A56 File Offset: 0x00000C56
		private static void LogVerbose(string message)
		{
			Action<string> diagnosticLogWritten = SlowDiagnostics.DiagnosticLogWritten;
			if (diagnosticLogWritten != null)
			{
				diagnosticLogWritten.Invoke(message);
			}
			LoggerUtil.Verbose(SlowDiagnostics.Api, typeof(SlowDiagnostics), message);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A7E File Offset: 0x00000C7E
		private static long MillisecondsToStopwatchTicks(int milliseconds)
		{
			if (milliseconds <= 0)
			{
				return 0L;
			}
			return (long)Math.Ceiling((double)milliseconds * (double)Stopwatch.Frequency / 1000.0);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002AA0 File Offset: 0x00000CA0
		private static double StopwatchTicksToMilliseconds(long ticks)
		{
			return (double)ticks * 1000.0 / (double)Stopwatch.Frequency;
		}

		// Token: 0x04000002 RID: 2
		[Nullable(2)]
		private static readonly FieldInfo MessageTypesField = typeof(NetworkChannelBase).GetField("messageTypes", 36);

		// Token: 0x04000003 RID: 3
		[Nullable(2)]
		private static readonly FieldInfo HandlersField = typeof(NetworkChannel).GetField("handlers", 36);

		// Token: 0x04000004 RID: 4
		private static readonly SlowLogRateLimiter RateLimiter = new SlowLogRateLimiter();

		// Token: 0x04000005 RID: 5
		[Nullable(2)]
		private static ICoreAPI Api;

		// Token: 0x04000006 RID: 6
		[Nullable(2)]
		private static string HarmonyId;

		// Token: 0x04000007 RID: 7
		private static bool NetworkPatched;

		// Token: 0x04000008 RID: 8
		private static long NetworkThresholdTicks = SlowDiagnostics.MillisecondsToStopwatchTicks(50);

		// Token: 0x04000009 RID: 9
		private static long RangedStatusThresholdTicks = SlowDiagnostics.MillisecondsToStopwatchTicks(10);

		// Token: 0x0400000A RID: 10
		private static long LogCooldownTicks = SlowDiagnostics.MillisecondsToStopwatchTicks(5000);
	}
}
