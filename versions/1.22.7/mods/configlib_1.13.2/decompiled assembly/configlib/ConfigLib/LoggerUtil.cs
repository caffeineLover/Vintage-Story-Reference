using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace ConfigLib
{
	// Token: 0x0200003C RID: 60
	[NullableContext(1)]
	[Nullable(0)]
	internal static class LoggerUtil
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x00009826 File Offset: 0x00007A26
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

		// Token: 0x060001C2 RID: 450 RVA: 0x00009842 File Offset: 0x00007A42
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

		// Token: 0x060001C3 RID: 451 RVA: 0x0000985E File Offset: 0x00007A5E
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

		// Token: 0x060001C4 RID: 452 RVA: 0x0000987A File Offset: 0x00007A7A
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

		// Token: 0x060001C5 RID: 453 RVA: 0x00009896 File Offset: 0x00007A96
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

		// Token: 0x060001C6 RID: 454 RVA: 0x000098B2 File Offset: 0x00007AB2
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

		// Token: 0x060001C7 RID: 455 RVA: 0x000098CE File Offset: 0x00007ACE
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

		// Token: 0x060001C8 RID: 456 RVA: 0x000098EA File Offset: 0x00007AEA
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

		// Token: 0x060001C9 RID: 457 RVA: 0x00009906 File Offset: 0x00007B06
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

		// Token: 0x060001CA RID: 458 RVA: 0x00009922 File Offset: 0x00007B22
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

		// Token: 0x060001CB RID: 459 RVA: 0x00002176 File Offset: 0x00000376
		public static void Dev([Nullable(2)] ICoreAPI api, object caller, string format)
		{
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00002176 File Offset: 0x00000376
		public static void Dev([Nullable(2)] ICoreAPI api, Type type, string format)
		{
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009940 File Offset: 0x00007B40
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

		// Token: 0x060001CE RID: 462 RVA: 0x000099B4 File Offset: 0x00007BB4
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

		// Token: 0x060001CF RID: 463 RVA: 0x00009A28 File Offset: 0x00007C28
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

		// Token: 0x060001D0 RID: 464 RVA: 0x00009AB0 File Offset: 0x00007CB0
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

		// Token: 0x040000CB RID: 203
		private const string _prefix = "[Config lib]";
	}
}
