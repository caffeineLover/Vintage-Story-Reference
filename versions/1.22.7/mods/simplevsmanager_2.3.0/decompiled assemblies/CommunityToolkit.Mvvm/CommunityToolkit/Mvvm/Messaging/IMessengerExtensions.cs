using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging.Internals;

namespace CommunityToolkit.Mvvm.Messaging
{
	// Token: 0x0200000F RID: 15
	[NullableContext(1)]
	[Nullable(0)]
	public static class IMessengerExtensions
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00002ADC File Offset: 0x00000CDC
		public static bool IsRegistered<TMessage>(this IMessenger messenger, object recipient) where TMessage : class
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			return messenger.IsRegistered<TMessage, Unit>(recipient, default(Unit));
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002B10 File Offset: 0x00000D10
		[RequiresUnreferencedCode("This method requires the generated CommunityToolkit.Mvvm.Messaging.__Internals.__IMessengerExtensions type not to be removed to use the fast path. If this type is removed by the linker, or if the target recipient was created dynamically and was missed by the source generator, a slower fallback path using a compiled LINQ expression will be used. This will have more overhead in the first invocation of this method for any given recipient type.")]
		[RequiresDynamicCode("This method requires the generated CommunityToolkit.Mvvm.Messaging.__Internals.__IMessengerExtensions type not to be removed to use the fast path. If that is present, the method is AOT safe, as the only methods being invoked to register the messages will be the ones produced by the source generator. If it isn't, this method will need to dynamically create the generic methods to register messages, which might not be available at runtime.")]
		public static void RegisterAll(this IMessenger messenger, object recipient)
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ConditionalWeakTable<Type, Action<IMessenger, object>> registrationMethods = IMessengerExtensions.DiscoveredRecipients.RegistrationMethods;
			Type type = recipient.GetType();
			ConditionalWeakTable<Type, Action<IMessenger, object>>.CreateValueCallback createValueCallback;
			if ((createValueCallback = IMessengerExtensions.<>O.<0>__LoadRegistrationMethodsForType) == null)
			{
				createValueCallback = (IMessengerExtensions.<>O.<0>__LoadRegistrationMethodsForType = new ConditionalWeakTable<Type, Action<IMessenger, object>>.CreateValueCallback(IMessengerExtensions.<RegisterAll>g__LoadRegistrationMethodsForType|4_0));
			}
			Action<IMessenger, object> value = registrationMethods.GetValue(type, createValueCallback);
			if (value != null)
			{
				value.Invoke(messenger, recipient);
				return;
			}
			messenger.RegisterAll(recipient, default(Unit));
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002B7C File Offset: 0x00000D7C
		[RequiresUnreferencedCode("This method requires the generated CommunityToolkit.Mvvm.Messaging.__Internals.__IMessengerExtensions type not to be removed to use the fast path. If this type is removed by the linker, or if the target recipient was created dynamically and was missed by the source generator, a slower fallback path using a compiled LINQ expression will be used. This will have more overhead in the first invocation of this method for any given recipient type.")]
		[RequiresDynamicCode("The generic methods to register messages might not be available at runtime.")]
		public static void RegisterAll<[Nullable(0)] TToken>(this IMessenger messenger, object recipient, TToken token) where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			ConditionalWeakTable<Type, Action<IMessenger, object, TToken>> registrationMethods = IMessengerExtensions.DiscoveredRecipients<TToken>.RegistrationMethods;
			Type type = recipient.GetType();
			ConditionalWeakTable<Type, Action<IMessenger, object, TToken>>.CreateValueCallback createValueCallback;
			if ((createValueCallback = IMessengerExtensions.<RegisterAll>O__5_0<TToken>.<0>__LoadRegistrationMethodsForType) == null)
			{
				createValueCallback = (IMessengerExtensions.<RegisterAll>O__5_0<TToken>.<0>__LoadRegistrationMethodsForType = new ConditionalWeakTable<Type, Action<IMessenger, object, TToken>>.CreateValueCallback(IMessengerExtensions.<RegisterAll>g__LoadRegistrationMethodsForType|5_0<TToken>));
			}
			registrationMethods.GetValue(type, createValueCallback).Invoke(messenger, recipient, token);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002BE0 File Offset: 0x00000DE0
		public static void Register<TMessage>(this IMessenger messenger, IRecipient<TMessage> recipient) where TMessage : class
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			WeakReferenceMessenger weakReferenceMessenger = messenger as WeakReferenceMessenger;
			if (weakReferenceMessenger != null)
			{
				weakReferenceMessenger.Register<TMessage, Unit>(recipient, default(Unit));
				return;
			}
			StrongReferenceMessenger strongReferenceMessenger = messenger as StrongReferenceMessenger;
			if (strongReferenceMessenger != null)
			{
				strongReferenceMessenger.Register<TMessage, Unit>(recipient, default(Unit));
				return;
			}
			Unit token = default(Unit);
			MessageHandler<IRecipient<TMessage>, TMessage> handler;
			if ((handler = IMessengerExtensions.<>c__6<TMessage>.<>9__6_0) == null)
			{
				handler = (IMessengerExtensions.<>c__6<TMessage>.<>9__6_0 = new MessageHandler<IRecipient<TMessage>, TMessage>(IMessengerExtensions.<>c__6<TMessage>.<>9.<Register>b__6_0));
			}
			messenger.Register<IRecipient<TMessage>, TMessage, Unit>(recipient, token, handler);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002C68 File Offset: 0x00000E68
		public static void Register<TMessage, [Nullable(0)] TToken>(this IMessenger messenger, IRecipient<TMessage> recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			WeakReferenceMessenger weakReferenceMessenger = messenger as WeakReferenceMessenger;
			if (weakReferenceMessenger != null)
			{
				weakReferenceMessenger.Register<TMessage, TToken>(recipient, token);
				return;
			}
			StrongReferenceMessenger strongReferenceMessenger = messenger as StrongReferenceMessenger;
			if (strongReferenceMessenger != null)
			{
				strongReferenceMessenger.Register<TMessage, TToken>(recipient, token);
				return;
			}
			MessageHandler<IRecipient<TMessage>, TMessage> handler;
			if ((handler = IMessengerExtensions.<>c__7<TMessage, TToken>.<>9__7_0) == null)
			{
				handler = (IMessengerExtensions.<>c__7<TMessage, TToken>.<>9__7_0 = new MessageHandler<IRecipient<TMessage>, TMessage>(IMessengerExtensions.<>c__7<TMessage, TToken>.<>9.<Register>b__7_0));
			}
			messenger.Register<IRecipient<TMessage>, TMessage, TToken>(recipient, token, handler);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002CE4 File Offset: 0x00000EE4
		public static void Register<TMessage>(this IMessenger messenger, object recipient, MessageHandler<object, TMessage> handler) where TMessage : class
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.ThrowIfNull(handler, "handler");
			messenger.Register<object, TMessage, Unit>(recipient, default(Unit), handler);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002D24 File Offset: 0x00000F24
		public static void Register<TRecipient, TMessage>(this IMessenger messenger, TRecipient recipient, MessageHandler<TRecipient, TMessage> handler) where TRecipient : class where TMessage : class
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.ThrowIfNull(handler, "handler");
			messenger.Register<TRecipient, TMessage, Unit>(recipient, default(Unit), handler);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002D68 File Offset: 0x00000F68
		public static void Register<TMessage, [Nullable(0)] TToken>(this IMessenger messenger, object recipient, TToken token, MessageHandler<object, TMessage> handler) where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			ArgumentNullException.ThrowIfNull(handler, "handler");
			messenger.Register<object, TMessage, TToken>(recipient, token, handler);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public static void Unregister<TMessage>(this IMessenger messenger, object recipient) where TMessage : class
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			messenger.Unregister<TMessage, Unit>(recipient, default(Unit));
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public static TMessage Send<TMessage>(this IMessenger messenger) where TMessage : class, new()
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			return messenger.Send<TMessage, Unit>(Activator.CreateInstance<TMessage>(), default(Unit));
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002E00 File Offset: 0x00001000
		public static TMessage Send<TMessage>(this IMessenger messenger, TMessage message) where TMessage : class
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.ThrowIfNull(message, "message");
			return messenger.Send<TMessage, Unit>(message, default(Unit));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002E38 File Offset: 0x00001038
		public static TMessage Send<TMessage, [Nullable(0)] TToken>(this IMessenger messenger, TToken token) where TMessage : class, new() where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			return messenger.Send<TMessage, TToken>(Activator.CreateInstance<TMessage>(), token);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002E5C File Offset: 0x0000105C
		public static IObservable<TMessage> CreateObservable<TMessage>(this IMessenger messenger) where TMessage : class
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			return new IMessengerExtensions.Observable<TMessage>(messenger);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002E6F File Offset: 0x0000106F
		public static IObservable<TMessage> CreateObservable<TMessage, [Nullable(0)] TToken>(this IMessenger messenger, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			return new IMessengerExtensions.Observable<TMessage, TToken>(messenger, token);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002E90 File Offset: 0x00001090
		[CompilerGenerated]
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		[return: Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		internal static Action<IMessenger, object> <RegisterAll>g__LoadRegistrationMethodsForType|4_0(Type recipientType)
		{
			Type type = recipientType.Assembly.GetType("CommunityToolkit.Mvvm.Messaging.__Internals.__IMessengerExtensions");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("CreateAllMessagesRegistrator", new Type[]
				{
					recipientType
				});
				if (method != null)
				{
					return (Action<IMessenger, object>)method.Invoke(null, new object[1]);
				}
			}
			return null;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002EE0 File Offset: 0x000010E0
		[CompilerGenerated]
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		[RequiresDynamicCode("The generic methods to register messages might not be available at runtime.")]
		internal static Action<IMessenger, object, TToken> <RegisterAll>g__LoadRegistrationMethodsForType|5_0<TToken>(Type recipientType) where TToken : IEquatable<TToken>
		{
			Type type = recipientType.Assembly.GetType("CommunityToolkit.Mvvm.Messaging.__Internals.__IMessengerExtensions");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("CreateAllMessagesRegistratorWithToken", new Type[]
				{
					recipientType
				});
				if (method != null)
				{
					return (Action<IMessenger, object, TToken>)method.MakeGenericMethod(new Type[]
					{
						typeof(TToken)
					}).Invoke(null, new object[1]);
				}
			}
			return IMessengerExtensions.<RegisterAll>g__LoadRegistrationMethodsForTypeFallback|5_1<TToken>(recipientType);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002F4C File Offset: 0x0000114C
		[CompilerGenerated]
		[RequiresDynamicCode("The generic methods to register messages might not be available at runtime.")]
		internal static Action<IMessenger, object, TToken> <RegisterAll>g__LoadRegistrationMethodsForTypeFallback|5_1<TToken>(Type recipientType) where TToken : IEquatable<TToken>
		{
			IMessengerExtensions.<>c__DisplayClass5_0<TToken> CS$<>8__locals1 = new IMessengerExtensions.<>c__DisplayClass5_0<TToken>();
			IEnumerable<Type> interfaces = recipientType.GetInterfaces();
			Func<Type, bool> func;
			if ((func = IMessengerExtensions.<>c__5<TToken>.<>9__5_2) == null)
			{
				func = (IMessengerExtensions.<>c__5<TToken>.<>9__5_2 = new Func<Type, bool>(IMessengerExtensions.<>c__5<TToken>.<>9.<RegisterAll>b__5_2));
			}
			IEnumerable<Type> enumerable = Enumerable.Where<Type>(interfaces, func);
			var func2;
			if ((func2 = IMessengerExtensions.<>c__5<TToken>.<>9__5_3) == null)
			{
				func2 = (IMessengerExtensions.<>c__5<TToken>.<>9__5_3 = new Func<Type, <>f__AnonymousType1<Type, Type>>(IMessengerExtensions.<>c__5<TToken>.<>9.<RegisterAll>b__5_3));
			}
			var enumerable2 = Enumerable.Select(enumerable, func2);
			var func3;
			if ((func3 = IMessengerExtensions.<>c__5<TToken>.<>9__5_4) == null)
			{
				func3 = (IMessengerExtensions.<>c__5<TToken>.<>9__5_4 = new Func<<>f__AnonymousType1<Type, Type>, MethodInfo>(IMessengerExtensions.<>c__5<TToken>.<>9.<RegisterAll>b__5_4));
			}
			MethodInfo[] array = Enumerable.ToArray<MethodInfo>(Enumerable.Select(enumerable2, func3));
			if (array.Length == 0)
			{
				Action<IMessenger, object, TToken> result;
				if ((result = IMessengerExtensions.<>c__5<TToken>.<>9__5_5) == null)
				{
					result = (IMessengerExtensions.<>c__5<TToken>.<>9__5_5 = new Action<IMessenger, object, TToken>(IMessengerExtensions.<>c__5<TToken>.<>9.<RegisterAll>b__5_5));
				}
				return result;
			}
			CS$<>8__locals1.arg0 = Expression.Parameter(typeof(IMessenger));
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
			CS$<>8__locals1.arg2 = Expression.Parameter(typeof(TToken));
			CS$<>8__locals1.inst1 = Expression.Convert(parameterExpression, recipientType);
			return Expression.Lambda<Action<IMessenger, object, TToken>>(Expression.Block(Enumerable.Select<MethodInfo, MethodCallExpression>(array, new Func<MethodInfo, MethodCallExpression>(CS$<>8__locals1.<RegisterAll>b__6))), new ParameterExpression[]
			{
				CS$<>8__locals1.arg0,
				parameterExpression,
				CS$<>8__locals1.arg2
			}).Compile();
		}

		// Token: 0x0200004A RID: 74
		[NullableContext(0)]
		private static class MethodInfos
		{
			// Token: 0x04000089 RID: 137
			[Nullable(1)]
			public static readonly MethodInfo RegisterIRecipient = new Action<IMessenger, IRecipient<object>, Unit>(IMessengerExtensions.Register<object, Unit>).Method.GetGenericMethodDefinition();
		}

		// Token: 0x0200004B RID: 75
		[NullableContext(0)]
		private static class DiscoveredRecipients
		{
			// Token: 0x0400008A RID: 138
			[Nullable(new byte[]
			{
				1,
				1,
				2,
				1,
				1
			})]
			public static readonly ConditionalWeakTable<Type, Action<IMessenger, object>> RegistrationMethods = new ConditionalWeakTable<Type, Action<IMessenger, object>>();
		}

		// Token: 0x0200004C RID: 76
		[NullableContext(0)]
		private static class DiscoveredRecipients<TToken> where TToken : IEquatable<TToken>
		{
			// Token: 0x060001CB RID: 459 RVA: 0x00008153 File Offset: 0x00006353
			// Note: this type is marked as 'beforefieldinit'.
			static DiscoveredRecipients()
			{
				IMessengerExtensions.DiscoveredRecipients<TToken>.RegistrationMethods = new ConditionalWeakTable<Type, Action<IMessenger, object, TToken>>();
			}

			// Token: 0x0400008B RID: 139
			[Nullable(1)]
			public static readonly ConditionalWeakTable<Type, Action<IMessenger, object, TToken>> RegistrationMethods;
		}

		// Token: 0x0200004D RID: 77
		[Nullable(0)]
		private sealed class Observable<TMessage> : IObservable<TMessage> where TMessage : class
		{
			// Token: 0x060001CC RID: 460 RVA: 0x0000815F File Offset: 0x0000635F
			public Observable(IMessenger messenger)
			{
				this.messenger = messenger;
			}

			// Token: 0x060001CD RID: 461 RVA: 0x0000816E File Offset: 0x0000636E
			public IDisposable Subscribe(IObserver<TMessage> observer)
			{
				return new IMessengerExtensions.Observable<TMessage>.Recipient(this.messenger, observer);
			}

			// Token: 0x0400008C RID: 140
			private readonly IMessenger messenger;

			// Token: 0x0200006D RID: 109
			[Nullable(0)]
			private sealed class Recipient : IRecipient<TMessage>, IDisposable
			{
				// Token: 0x06000226 RID: 550 RVA: 0x00008FEB File Offset: 0x000071EB
				public Recipient(IMessenger messenger, IObserver<TMessage> observer)
				{
					this.messenger = messenger;
					this.observer = observer;
					messenger.Register(this);
				}

				// Token: 0x06000227 RID: 551 RVA: 0x00009008 File Offset: 0x00007208
				public void Receive(TMessage message)
				{
					this.observer.OnNext(message);
				}

				// Token: 0x06000228 RID: 552 RVA: 0x00009016 File Offset: 0x00007216
				public void Dispose()
				{
					this.messenger.Unregister(this);
				}

				// Token: 0x040000DE RID: 222
				private readonly IMessenger messenger;

				// Token: 0x040000DF RID: 223
				private readonly IObserver<TMessage> observer;
			}
		}

		// Token: 0x0200004E RID: 78
		[Nullable(0)]
		private sealed class Observable<TMessage, [Nullable(0)] TToken> : IObservable<TMessage> where TMessage : class where TToken : IEquatable<TToken>
		{
			// Token: 0x060001CE RID: 462 RVA: 0x0000817C File Offset: 0x0000637C
			public Observable(IMessenger messenger, TToken token)
			{
				this.messenger = messenger;
				this.token = token;
			}

			// Token: 0x060001CF RID: 463 RVA: 0x00008192 File Offset: 0x00006392
			public IDisposable Subscribe(IObserver<TMessage> observer)
			{
				return new IMessengerExtensions.Observable<TMessage, TToken>.Recipient(this.messenger, observer, this.token);
			}

			// Token: 0x0400008D RID: 141
			private readonly IMessenger messenger;

			// Token: 0x0400008E RID: 142
			private readonly TToken token;

			// Token: 0x0200006E RID: 110
			[Nullable(0)]
			private sealed class Recipient : IRecipient<TMessage>, IDisposable
			{
				// Token: 0x06000229 RID: 553 RVA: 0x00009024 File Offset: 0x00007224
				public Recipient(IMessenger messenger, IObserver<TMessage> observer, TToken token)
				{
					this.messenger = messenger;
					this.observer = observer;
					this.token = token;
					messenger.Register(this, token);
				}

				// Token: 0x0600022A RID: 554 RVA: 0x00009049 File Offset: 0x00007249
				public void Receive(TMessage message)
				{
					this.observer.OnNext(message);
				}

				// Token: 0x0600022B RID: 555 RVA: 0x00009057 File Offset: 0x00007257
				public void Dispose()
				{
					this.messenger.Unregister<TMessage, TToken>(this, this.token);
				}

				// Token: 0x040000E0 RID: 224
				private readonly IMessenger messenger;

				// Token: 0x040000E1 RID: 225
				private readonly IObserver<TMessage> observer;

				// Token: 0x040000E2 RID: 226
				private readonly TToken token;
			}
		}

		// Token: 0x0200004F RID: 79
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400008F RID: 143
			[Nullable(new byte[]
			{
				0,
				1,
				2,
				1,
				1
			})]
			public static ConditionalWeakTable<Type, Action<IMessenger, object>>.CreateValueCallback <0>__LoadRegistrationMethodsForType;
		}

		// Token: 0x02000054 RID: 84
		[CompilerGenerated]
		private static class <RegisterAll>O__5_0<TToken> where TToken : IEquatable<TToken>
		{
			// Token: 0x0400009C RID: 156
			[Nullable(new byte[]
			{
				0,
				1,
				1,
				1,
				1,
				1
			})]
			public static ConditionalWeakTable<Type, Action<IMessenger, object, TToken>>.CreateValueCallback <0>__LoadRegistrationMethodsForType;
		}
	}
}
