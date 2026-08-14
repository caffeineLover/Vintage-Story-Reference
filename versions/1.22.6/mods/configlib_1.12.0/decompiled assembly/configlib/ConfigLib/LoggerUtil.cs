using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace ConfigLib
{
	// Token: 0x0200003A RID: 58
	[NullableContext(1)]
	[Nullable(0)]
	internal static class LoggerUtil
	{
		// Token: 0x0600019C RID: 412 RVA: 0x0000950A File Offset: 0x0000770A
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

		// Token: 0x0600019D RID: 413 RVA: 0x00009526 File Offset: 0x00007726
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

		// Token: 0x0600019E RID: 414 RVA: 0x00009542 File Offset: 0x00007742
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

		// Token: 0x0600019F RID: 415 RVA: 0x0000955E File Offset: 0x0000775E
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

		// Token: 0x060001A0 RID: 416 RVA: 0x0000957A File Offset: 0x0000777A
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

		// Token: 0x060001A1 RID: 417 RVA: 0x00009596 File Offset: 0x00007796
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

		// Token: 0x060001A2 RID: 418 RVA: 0x000095B2 File Offset: 0x000077B2
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

		// Token: 0x060001A3 RID: 419 RVA: 0x000095CE File Offset: 0x000077CE
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

		// Token: 0x060001A4 RID: 420 RVA: 0x000095EA File Offset: 0x000077EA
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

		// Token: 0x060001A5 RID: 421 RVA: 0x00009606 File Offset: 0x00007806
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

		// Token: 0x060001A6 RID: 422 RVA: 0x00007270 File Offset: 0x00005470
		public static void Dev([Nullable(2)] ICoreAPI api, object caller, string format)
		{
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007270 File Offset: 0x00005470
		public static void Dev([Nullable(2)] ICoreAPI api, Type type, string format)
		{
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00009624 File Offset: 0x00007824
		public static string Format(object caller, string format)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted("[Config lib]");
			defaultInterpolatedStringHandler.AppendLiteral(" [");
			defaultInterpolatedStringHandler.AppendFormatted(LoggerUtil.GetCallerTypeName(caller));
			defaultInterpolatedStringHandler.AppendLiteral("] ");
			defaultInterpolatedStringHandler.AppendFormatted(format);
			return defaultInterpolatedStringHandler.ToStringAndClear().Replace("{", "{{").Replace("}", "}}");
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009698 File Offset: 0x00007898
		public static string Format(Type type, string format)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted("[Config lib]");
			defaultInterpolatedStringHandler.AppendLiteral(" [");
			defaultInterpolatedStringHandler.AppendFormatted(LoggerUtil.GetTypeName(type));
			defaultInterpolatedStringHandler.AppendLiteral("] ");
			defaultInterpolatedStringHandler.AppendFormatted(format);
			return defaultInterpolatedStringHandler.ToStringAndClear().Replace("{", "{{").Replace("}", "}}");
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000970C File Offset: 0x0000790C
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

		// Token: 0x060001AB RID: 427 RVA: 0x00009794 File Offset: 0x00007994
		private static string GetTypeName(Type type)
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

		// Token: 0x040000C8 RID: 200
		private const string _prefix = "[Config lib]";
	}
}
