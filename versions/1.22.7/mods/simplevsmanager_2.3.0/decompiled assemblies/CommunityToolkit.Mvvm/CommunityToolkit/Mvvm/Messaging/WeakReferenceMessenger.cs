using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using CommunityToolkit.Mvvm.Messaging.Internals;

namespace CommunityToolkit.Mvvm.Messaging
{
	// Token: 0x02000013 RID: 19
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeakReferenceMessenger : IMessenger
	{
		// Token: 0x06000066 RID: 102 RVA: 0x000039E1 File Offset: 0x00001BE1
		public WeakReferenceMessenger()
		{
			Action<object> callback;
			if ((callback = WeakReferenceMessenger.<>O.<0>__Gen2GcCallbackProxy) == null)
			{
				callback = (WeakReferenceMessenger.<>O.<0>__Gen2GcCallbackProxy = new Action<object>(WeakReferenceMessenger.<.ctor>g__Gen2GcCallbackProxy|1_0));
			}
			Gen2GcCallback.Register(callback, this);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003A15 File Offset: 0x00001C15
		public static WeakReferenceMessenger Default { get; } = new WeakReferenceMessenger();

		// Token: 0x06000068 RID: 104 RVA: 0x00003A1C File Offset: 0x00001C1C
		public bool IsRegistered<TMessage, [Nullable(0)] TToken>(object recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			Dictionary2<Type2, ConditionalWeakTable2<object, object>> dictionary = this.recipientsMap;
			bool result;
			lock (dictionary)
			{
				Type2 type = new Type2(typeof(TMessage), typeof(TToken));
				ConditionalWeakTable2<object, object> conditionalWeakTable;
				if (!this.recipientsMap.TryGetValue(type, ref conditionalWeakTable))
				{
					result = false;
				}
				else if (typeof(TToken) == typeof(Unit))
				{
					object obj;
					result = conditionalWeakTable.TryGetValue(recipient, ref obj);
				}
				else
				{
					object obj2;
					result = (conditionalWeakTable.TryGetValue(recipient, ref obj2) && Unsafe.As<Dictionary2<TToken, object>>(obj2).ContainsKey(token));
				}
			}
			return result;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003AE4 File Offset: 0x00001CE4
		public void Register<TRecipient, TMessage, [Nullable(0)] TToken>(TRecipient recipient, TToken token, MessageHandler<TRecipient, TMessage> handler) where TRecipient : class where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			ArgumentNullException.ThrowIfNull(handler, "handler");
			this.Register<TMessage, TToken>(recipient, token, new MessageHandlerDispatcher.For<TRecipient, TMessage>(handler));
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003B1F File Offset: 0x00001D1F
		internal void Register<TMessage, [Nullable(0)] TToken>(IRecipient<TMessage> recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			this.Register<TMessage, TToken>(recipient, token, null);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003B2C File Offset: 0x00001D2C
		private void Register<TMessage, [Nullable(0)] TToken>(object recipient, TToken token, [Nullable(2)] MessageHandlerDispatcher dispatcher) where TMessage : class where TToken : IEquatable<TToken>
		{
			Dictionary2<Type2, ConditionalWeakTable2<object, object>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				Type2 type = new Type2(typeof(TMessage), typeof(TToken));
				ref ConditionalWeakTable2<object, object> orAddValueRef = this.recipientsMap.GetOrAddValueRef(type);
				if (orAddValueRef == null)
				{
					orAddValueRef = new ConditionalWeakTable2<object, object>();
				}
				if (typeof(TToken) == typeof(Unit))
				{
					if (!orAddValueRef.TryAdd(recipient, dispatcher))
					{
						WeakReferenceMessenger.ThrowInvalidOperationExceptionForDuplicateRegistration();
					}
				}
				else
				{
					ConditionalWeakTable2<object, object> conditionalWeakTable = orAddValueRef;
					ConditionalWeakTable<object, object>.CreateValueCallback createValueCallback;
					if ((createValueCallback = WeakReferenceMessenger.<>c__8<TMessage, TToken>.<>9__8_0) == null)
					{
						createValueCallback = (WeakReferenceMessenger.<>c__8<TMessage, TToken>.<>9__8_0 = new ConditionalWeakTable<object, object>.CreateValueCallback(WeakReferenceMessenger.<>c__8<TMessage, TToken>.<>9.<Register>b__8_0));
					}
					ref object orAddValueRef2 = Unsafe.As<Dictionary2<TToken, object>>(conditionalWeakTable.GetValue(recipient, createValueCallback)).GetOrAddValueRef(token);
					if (orAddValueRef2 != null)
					{
						WeakReferenceMessenger.ThrowInvalidOperationExceptionForDuplicateRegistration();
					}
					orAddValueRef2 = dispatcher;
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003C04 File Offset: 0x00001E04
		public void UnregisterAll(object recipient)
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			Dictionary2<Type2, ConditionalWeakTable2<object, object>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				Dictionary2<Type2, ConditionalWeakTable2<object, object>>.Enumerator enumerator = this.recipientsMap.GetEnumerator();
				while (enumerator.MoveNext())
				{
					enumerator.GetValue().Remove(recipient);
				}
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003C70 File Offset: 0x00001E70
		public void UnregisterAll<[Nullable(0)] TToken>(object recipient, TToken token) where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			if (typeof(TToken) == typeof(Unit))
			{
				throw new NotImplementedException();
			}
			Dictionary2<Type2, ConditionalWeakTable2<object, object>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				Dictionary2<Type2, ConditionalWeakTable2<object, object>>.Enumerator enumerator = this.recipientsMap.GetEnumerator();
				while (enumerator.MoveNext())
				{
					object obj;
					if (enumerator.GetKey().TToken == typeof(TToken) && enumerator.GetValue().TryGetValue(recipient, ref obj))
					{
						Unsafe.As<Dictionary2<TToken, object>>(obj).TryRemove(token);
					}
				}
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003D34 File Offset: 0x00001F34
		public void Unregister<TMessage, [Nullable(0)] TToken>(object recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			Dictionary2<Type2, ConditionalWeakTable2<object, object>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				Type2 type = new Type2(typeof(TMessage), typeof(TToken));
				ConditionalWeakTable2<object, object> conditionalWeakTable;
				if (this.recipientsMap.TryGetValue(type, ref conditionalWeakTable))
				{
					object obj;
					if (typeof(TToken) == typeof(Unit))
					{
						conditionalWeakTable.Remove(recipient);
					}
					else if (conditionalWeakTable.TryGetValue(recipient, ref obj))
					{
						Unsafe.As<Dictionary2<TToken, object>>(obj).TryRemove(token);
					}
				}
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003DF0 File Offset: 0x00001FF0
		public TMessage Send<TMessage, [Nullable(0)] TToken>(TMessage message, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(message, "message");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			int num = 0;
			Dictionary2<Type2, ConditionalWeakTable2<object, object>> dictionary = this.recipientsMap;
			ArrayPoolBufferWriter<object> arrayPoolBufferWriter;
			lock (dictionary)
			{
				Type2 type = new Type2(typeof(TMessage), typeof(TToken));
				ConditionalWeakTable2<object, object> conditionalWeakTable;
				if (!this.recipientsMap.TryGetValue(type, ref conditionalWeakTable))
				{
					return message;
				}
				arrayPoolBufferWriter = ArrayPoolBufferWriter<object>.Create();
				using (ConditionalWeakTable2<object, object>.Enumerator enumerator = conditionalWeakTable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj;
						if (typeof(TToken) == typeof(Unit))
						{
							arrayPoolBufferWriter.Add(enumerator.GetValue());
							arrayPoolBufferWriter.Add(enumerator.GetKey());
							num++;
						}
						else if (Unsafe.As<Dictionary2<TToken, object>>(enumerator.GetValue()).TryGetValue(token, ref obj))
						{
							arrayPoolBufferWriter.Add(obj);
							arrayPoolBufferWriter.Add(enumerator.GetKey());
							num++;
						}
					}
				}
			}
			try
			{
				WeakReferenceMessenger.SendAll<TMessage>(arrayPoolBufferWriter.Span, num, message);
			}
			finally
			{
				arrayPoolBufferWriter.Dispose();
			}
			return message;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003F40 File Offset: 0x00002140
		[MethodImpl(8)]
		internal unsafe static void SendAll<TMessage>([Nullable(new byte[]
		{
			0,
			2
		})] ReadOnlySpan<object> pairs, int i, TMessage message) where TMessage : class
		{
			ReadOnlySpan<object> readOnlySpan = pairs.Slice(0, 2 * i);
			ref object ptr = MemoryMarshal.GetReference<object>(readOnlySpan);
			ref object ptr2 = Unsafe.Add<object>(ref ptr, readOnlySpan.Length);
			while (Unsafe.IsAddressLessThan<object>(ref ptr, ref ptr2))
			{
				object obj = ptr;
				object obj2 = *Unsafe.Add<object>(ref ptr, 1);
				if (obj == null)
				{
					Unsafe.As<IRecipient<TMessage>>(obj2).Receive(message);
				}
				else
				{
					Unsafe.As<MessageHandlerDispatcher>(obj).Invoke(obj2, message);
				}
				ptr = Unsafe.Add<object>(ref ptr, 2);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003FB4 File Offset: 0x000021B4
		public void Cleanup()
		{
			Dictionary2<Type2, ConditionalWeakTable2<object, object>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				this.CleanupWithoutLock();
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003FF4 File Offset: 0x000021F4
		public void Reset()
		{
			Dictionary2<Type2, ConditionalWeakTable2<object, object>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				this.recipientsMap.Clear();
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000403C File Offset: 0x0000223C
		private void CleanupWithNonBlockingLock()
		{
			object obj = this.recipientsMap;
			bool flag = false;
			try
			{
				Monitor.TryEnter(obj, ref flag);
				if (flag)
				{
					this.CleanupWithoutLock();
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(obj);
				}
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004080 File Offset: 0x00002280
		private unsafe void CleanupWithoutLock()
		{
			using (ArrayPoolBufferWriter<Type2> arrayPoolBufferWriter = ArrayPoolBufferWriter<Type2>.Create())
			{
				using (ArrayPoolBufferWriter<object> arrayPoolBufferWriter2 = ArrayPoolBufferWriter<object>.Create())
				{
					Dictionary2<Type2, ConditionalWeakTable2<object, object>>.Enumerator enumerator = this.recipientsMap.GetEnumerator();
					while (enumerator.MoveNext())
					{
						arrayPoolBufferWriter2.Reset();
						bool flag = false;
						if (enumerator.GetKey().TToken == typeof(Unit))
						{
							using (ConditionalWeakTable2<object, object>.Enumerator enumerator2 = enumerator.GetValue().GetEnumerator())
							{
								if (!enumerator2.MoveNext())
								{
									goto IL_F2;
								}
								flag = true;
								goto IL_F2;
							}
							goto IL_70;
						}
						goto IL_70;
						IL_F2:
						if (!flag)
						{
							arrayPoolBufferWriter.Add(enumerator.GetKey());
							continue;
						}
						continue;
						IL_70:
						using (ConditionalWeakTable2<object, object>.Enumerator enumerator3 = enumerator.GetValue().GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								if (Unsafe.As<IDictionary2>(enumerator3.GetValue()).Count == 0)
								{
									arrayPoolBufferWriter2.Add(enumerator3.GetKey());
								}
								else
								{
									flag = true;
								}
							}
						}
						ReadOnlySpan<object> span = arrayPoolBufferWriter2.Span;
						for (int i = 0; i < span.Length; i++)
						{
							object obj = *span[i];
							enumerator.GetValue().Remove(obj);
						}
						goto IL_F2;
					}
					ReadOnlySpan<Type2> span2 = arrayPoolBufferWriter.Span;
					for (int i = 0; i < span2.Length; i++)
					{
						Type2 type = *span2[i];
						this.recipientsMap.TryRemove(type);
					}
				}
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004250 File Offset: 0x00002450
		private static void ThrowInvalidOperationExceptionForDuplicateRegistration()
		{
			throw new InvalidOperationException("The target recipient has already subscribed to the target message.");
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004268 File Offset: 0x00002468
		[CompilerGenerated]
		internal static void <.ctor>g__Gen2GcCallbackProxy|1_0(object target)
		{
			((WeakReferenceMessenger)target).CleanupWithNonBlockingLock();
		}

		// Token: 0x04000026 RID: 38
		[Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})]
		private readonly Dictionary2<Type2, ConditionalWeakTable2<object, object>> recipientsMap = new Dictionary2<Type2, ConditionalWeakTable2<object, object>>();

		// Token: 0x02000059 RID: 89
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040000A0 RID: 160
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static Action<object> <0>__Gen2GcCallbackProxy;
		}
	}
}
