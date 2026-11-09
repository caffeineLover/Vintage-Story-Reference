using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000028 RID: 40
	[NullableContext(1)]
	[Nullable(0)]
	public static class LoggerUtil
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000C01B File Offset: 0x0000A21B
		public static void Notify([Nullable(2)] ICoreAPI api, object caller, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Notification(LoggerUtil.Format(caller, format));
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000C037 File Offset: 0x0000A237
		public static void Notify([Nullable(2)] ICoreAPI api, Type type, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Notification(LoggerUtil.Format(type, format));
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000C053 File Offset: 0x0000A253
		public static void Warn([Nullable(2)] ICoreAPI api, object caller, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Warning(LoggerUtil.Format(caller, format));
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000C06F File Offset: 0x0000A26F
		public static void Warn([Nullable(2)] ICoreAPI api, Type type, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Warning(LoggerUtil.Format(type, format));
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000C08B File Offset: 0x0000A28B
		public static void Error([Nullable(2)] ICoreAPI api, object caller, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Error(LoggerUtil.Format(caller, format));
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000C0A7 File Offset: 0x0000A2A7
		public static void Error([Nullable(2)] ICoreAPI api, Type type, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Error(LoggerUtil.Format(type, format));
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C0C3 File Offset: 0x0000A2C3
		public static void Debug([Nullable(2)] ICoreAPI api, object caller, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Debug(LoggerUtil.Format(caller, format));
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000C0DF File Offset: 0x0000A2DF
		public static void Debug([Nullable(2)] ICoreAPI api, Type type, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Debug(LoggerUtil.Format(type, format));
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000C0FB File Offset: 0x0000A2FB
		public static void Verbose([Nullable(2)] ICoreAPI api, object caller, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.VerboseDebug(LoggerUtil.Format(caller, format));
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C117 File Offset: 0x0000A317
		public static void Verbose([Nullable(2)] ICoreAPI api, Type type, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.VerboseDebug(LoggerUtil.Format(type, format));
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000C133 File Offset: 0x0000A333
		public static void Audit([Nullable(2)] ICoreAPI api, object caller, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Audit(LoggerUtil.Format(caller, format));
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000C14F File Offset: 0x0000A34F
		public static void Audit([Nullable(2)] ICoreAPI api, Type type, string format)
		{
			if (api != null)
			{
				ILogger logger = api.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Audit(LoggerUtil.Format(type, format));
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000C16B File Offset: 0x0000A36B
		public static void Dev([Nullable(2)] ICoreAPI api, object caller, string format)
		{
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000C16D File Offset: 0x0000A36D
		public static void Dev([Nullable(2)] ICoreAPI api, Type type, string format)
		{
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000C16F File Offset: 0x0000A36F
		public static void Dev([Nullable(2)] ICoreAPI api, string format)
		{
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000C171 File Offset: 0x0000A371
		public static void Mark([Nullable(2)] ICoreAPI api, string marker)
		{
			if (api != null)
			{
				IWorldAccessor world = api.World;
				if (world == null)
				{
					return;
				}
				FrameProfilerUtil frameProfiler = world.FrameProfiler;
				if (frameProfiler == null)
				{
					return;
				}
				frameProfiler.Mark("ovhlib-", marker);
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000C198 File Offset: 0x0000A398
		public static string Format(object caller, string format)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted("[Overhaul lib]");
			defaultInterpolatedStringHandler.AppendLiteral(" [");
			defaultInterpolatedStringHandler.AppendFormatted(LoggerUtil.GetCallerTypeName(caller));
			defaultInterpolatedStringHandler.AppendLiteral("] ");
			defaultInterpolatedStringHandler.AppendFormatted(format);
			return defaultInterpolatedStringHandler.ToStringAndClear().Replace("{", "{{").Replace("}", "}}");
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000C20C File Offset: 0x0000A40C
		public static string Format(Type type, string format)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted("[Overhaul lib]");
			defaultInterpolatedStringHandler.AppendLiteral(" [");
			defaultInterpolatedStringHandler.AppendFormatted(LoggerUtil.GetTypeName(type));
			defaultInterpolatedStringHandler.AppendLiteral("] ");
			defaultInterpolatedStringHandler.AppendFormatted(format);
			return defaultInterpolatedStringHandler.ToStringAndClear().Replace("{", "{{").Replace("}", "}}");
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000C280 File Offset: 0x0000A480
		public static string Format(string format)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted("[Overhaul lib]");
			defaultInterpolatedStringHandler.AppendFormatted(format);
			return defaultInterpolatedStringHandler.ToStringAndClear().Replace("{", "{{").Replace("}", "}}");
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000C2D0 File Offset: 0x0000A4D0
		public static string GetCallerTypeName(object caller)
		{
			Type type = caller.GetType();
			if (type.IsGenericType)
			{
				string text2 = type.Name.Split(new char[]
				{
					'`'
				}, 1)[0];
				string text = Enumerable.Aggregate<string>(Enumerable.Select<Type, string>(type.GetGenericArguments(), new Func<Type, string>(LoggerUtil.GetTypeName)), (string first, string second) => first + "," + second);
				return text2 + "<" + text + ">";
			}
			return type.Name;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000C358 File Offset: 0x0000A558
		public static string GetTypeName(Type type)
		{
			if (type.IsGenericType)
			{
				string text2 = type.Name.Split(new char[]
				{
					'`'
				}, 1)[0];
				string text = Enumerable.Aggregate<string>(Enumerable.Select<Type, string>(type.GetGenericArguments(), new Func<Type, string>(LoggerUtil.GetTypeName)), (string first, string second) => first + "," + second);
				return text2 + "<" + text + ">";
			}
			return type.Name;
		}

		// Token: 0x0400010D RID: 269
		private const string _prefix = "[Overhaul lib]";

		// Token: 0x0400010E RID: 270
		private const string _markPrefix = "ovhlib-";
	}
}
