using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using CommunityToolkit.Mvvm.Messaging.Internals;

namespace CommunityToolkit.Mvvm.Messaging
{
	// Token: 0x02000012 RID: 18
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StrongReferenceMessenger : IMessenger
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000054 RID: 84 RVA: 0x0000307F File Offset: 0x0000127F
		public static StrongReferenceMessenger Default { get; } = new StrongReferenceMessenger();

		// Token: 0x06000055 RID: 85 RVA: 0x00003088 File Offset: 0x00001288
		public bool IsRegistered<TMessage, [Nullable(0)] TToken>(object recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			Dictionary2<StrongReferenceMessenger.Recipient, HashSet<StrongReferenceMessenger.IMapping>> dictionary = this.recipientsMap;
			bool result;
			lock (dictionary)
			{
				StrongReferenceMessenger.Mapping<TToken> mapping2;
				if (typeof(TToken) == typeof(Unit))
				{
					StrongReferenceMessenger.Mapping mapping;
					if (!this.TryGetMapping<TMessage>(out mapping))
					{
						result = false;
					}
					else
					{
						StrongReferenceMessenger.Recipient recipient2 = new StrongReferenceMessenger.Recipient(recipient);
						result = mapping.ContainsKey(recipient2);
					}
				}
				else if (!this.TryGetMapping<TMessage, TToken>(out mapping2))
				{
					result = false;
				}
				else
				{
					StrongReferenceMessenger.Recipient recipient3 = new StrongReferenceMessenger.Recipient(recipient);
					Dictionary2<TToken, object> dictionary2;
					result = (mapping2.TryGetValue(recipient3, ref dictionary2) && dictionary2.ContainsKey(token));
				}
			}
			return result;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003148 File Offset: 0x00001348
		public void Register<TRecipient, TMessage, [Nullable(0)] TToken>(TRecipient recipient, TToken token, MessageHandler<TRecipient, TMessage> handler) where TRecipient : class where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			ArgumentNullException.ThrowIfNull(handler, "handler");
			this.Register<TMessage, TToken>(recipient, token, new MessageHandlerDispatcher.For<TRecipient, TMessage>(handler));
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003183 File Offset: 0x00001383
		internal void Register<TMessage, [Nullable(0)] TToken>(IRecipient<TMessage> recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			this.Register<TMessage, TToken>(recipient, token, null);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003190 File Offset: 0x00001390
		private void Register<TMessage, [Nullable(0)] TToken>(object recipient, TToken token, [Nullable(2)] MessageHandlerDispatcher dispatcher) where TMessage : class where TToken : IEquatable<TToken>
		{
			Dictionary2<StrongReferenceMessenger.Recipient, HashSet<StrongReferenceMessenger.IMapping>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				StrongReferenceMessenger.Recipient recipient2 = new StrongReferenceMessenger.Recipient(recipient);
				StrongReferenceMessenger.IMapping mapping;
				if (typeof(TToken) == typeof(Unit))
				{
					StrongReferenceMessenger.Mapping orAddMapping = this.GetOrAddMapping<TMessage>();
					ref object orAddValueRef = orAddMapping.GetOrAddValueRef(recipient2);
					if (orAddValueRef != null)
					{
						StrongReferenceMessenger.ThrowInvalidOperationExceptionForDuplicateRegistration();
					}
					orAddValueRef = dispatcher;
					mapping = orAddMapping;
				}
				else
				{
					StrongReferenceMessenger.Mapping<TToken> orAddMapping2 = this.GetOrAddMapping<TMessage, TToken>();
					ref Dictionary2<TToken, object> orAddValueRef2 = orAddMapping2.GetOrAddValueRef(recipient2);
					if (orAddValueRef2 == null)
					{
						orAddValueRef2 = new Dictionary2<TToken, object>();
					}
					ref object orAddValueRef3 = orAddValueRef2.GetOrAddValueRef(token);
					if (orAddValueRef3 != null)
					{
						StrongReferenceMessenger.ThrowInvalidOperationExceptionForDuplicateRegistration();
					}
					orAddValueRef3 = dispatcher;
					mapping = orAddMapping2;
				}
				ref HashSet<StrongReferenceMessenger.IMapping> orAddValueRef4 = this.recipientsMap.GetOrAddValueRef(recipient2);
				if (orAddValueRef4 == null)
				{
					orAddValueRef4 = new HashSet<StrongReferenceMessenger.IMapping>();
				}
				orAddValueRef4.Add(mapping);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003260 File Offset: 0x00001460
		public void UnregisterAll(object recipient)
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			Dictionary2<StrongReferenceMessenger.Recipient, HashSet<StrongReferenceMessenger.IMapping>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				StrongReferenceMessenger.Recipient recipient2 = new StrongReferenceMessenger.Recipient(recipient);
				HashSet<StrongReferenceMessenger.IMapping> hashSet;
				if (this.recipientsMap.TryGetValue(recipient2, ref hashSet))
				{
					foreach (StrongReferenceMessenger.IMapping mapping in hashSet)
					{
						if (mapping.TryRemove(recipient2) && mapping.Count == 0)
						{
							this.typesMap.TryRemove(mapping.TypeArguments);
						}
					}
					this.recipientsMap.TryRemove(recipient2);
				}
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000332C File Offset: 0x0000152C
		public unsafe void UnregisterAll<[Nullable(0)] TToken>(object recipient, TToken token) where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			if (typeof(TToken) == typeof(Unit))
			{
				throw new NotImplementedException();
			}
			bool flag = false;
			object[] array = null;
			int num = 0;
			try
			{
				Monitor.Enter(this.recipientsMap, ref flag);
				StrongReferenceMessenger.Recipient recipient2 = new StrongReferenceMessenger.Recipient(recipient);
				HashSet<StrongReferenceMessenger.IMapping> hashSet;
				if (this.recipientsMap.TryGetValue(recipient2, ref hashSet))
				{
					array = ArrayPool<object>.Shared.Rent(hashSet.Count);
					foreach (StrongReferenceMessenger.IMapping mapping in hashSet)
					{
						IDictionary2<StrongReferenceMessenger.Recipient, IDictionary2<TToken>> dictionary = mapping as IDictionary2<StrongReferenceMessenger.Recipient, IDictionary2<TToken>>;
						if (dictionary != null)
						{
							array[num++] = dictionary;
						}
					}
					Span<object> span = MemoryExtensions.AsSpan<object>(array, 0, num);
					for (int i = 0; i < span.Length; i++)
					{
						IDictionary2<StrongReferenceMessenger.Recipient, IDictionary2<TToken>> dictionary2 = Unsafe.As<IDictionary2<StrongReferenceMessenger.Recipient, IDictionary2<TToken>>>(*span[i]);
						IDictionary2<TToken> dictionary3 = dictionary2[recipient2];
						if (dictionary3.TryRemove(token) && dictionary3.Count == 0)
						{
							dictionary2.TryRemove(recipient2);
							StrongReferenceMessenger.IMapping mapping2 = Unsafe.As<StrongReferenceMessenger.IMapping>(dictionary2);
							hashSet.Remove(mapping2);
							if (hashSet.Count == 0)
							{
								this.recipientsMap.TryRemove(recipient2);
							}
							if (dictionary2.Count == 0)
							{
								this.typesMap.TryRemove(mapping2.TypeArguments);
							}
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this.recipientsMap);
				}
				if (array != null)
				{
					MemoryExtensions.AsSpan<object>(array, 0, num).Clear();
					ArrayPool<object>.Shared.Return(array, false);
				}
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000034F8 File Offset: 0x000016F8
		public void Unregister<TMessage, [Nullable(0)] TToken>(object recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(recipient, "recipient");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			Dictionary2<StrongReferenceMessenger.Recipient, HashSet<StrongReferenceMessenger.IMapping>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				StrongReferenceMessenger.Mapping<TToken> mapping2;
				if (typeof(TToken) == typeof(Unit))
				{
					StrongReferenceMessenger.Mapping mapping;
					if (this.TryGetMapping<TMessage>(out mapping))
					{
						StrongReferenceMessenger.Recipient recipient2 = new StrongReferenceMessenger.Recipient(recipient);
						if (mapping.TryRemove(recipient2))
						{
							mapping.TryRemove(recipient2);
							if (mapping.Count == 0)
							{
								this.typesMap.TryRemove(mapping.TypeArguments);
							}
							HashSet<StrongReferenceMessenger.IMapping> hashSet = this.recipientsMap[recipient2];
							hashSet.Remove(mapping);
							if (hashSet.Count == 0)
							{
								this.recipientsMap.TryRemove(recipient2);
							}
						}
					}
				}
				else if (this.TryGetMapping<TMessage, TToken>(out mapping2))
				{
					StrongReferenceMessenger.Recipient recipient3 = new StrongReferenceMessenger.Recipient(recipient);
					Dictionary2<TToken, object> dictionary2;
					if (mapping2.TryGetValue(recipient3, ref dictionary2))
					{
						if (dictionary2.TryRemove(token) && dictionary2.Count == 0)
						{
							mapping2.TryRemove(recipient3);
							if (mapping2.Count == 0)
							{
								this.typesMap.TryRemove(mapping2.TypeArguments);
							}
							HashSet<StrongReferenceMessenger.IMapping> hashSet2 = this.recipientsMap[recipient3];
							hashSet2.Remove(mapping2);
							if (hashSet2.Count == 0)
							{
								this.recipientsMap.TryRemove(recipient3);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003670 File Offset: 0x00001870
		public unsafe TMessage Send<TMessage, [Nullable(0)] TToken>(TMessage message, TToken token) where TMessage : class where TToken : IEquatable<TToken>
		{
			ArgumentNullException.ThrowIfNull(message, "message");
			ArgumentNullException.For<TToken>.ThrowIfNull(token, "token");
			int num = 0;
			Dictionary2<StrongReferenceMessenger.Recipient, HashSet<StrongReferenceMessenger.IMapping>> dictionary = this.recipientsMap;
			object[] array;
			Span<object> span;
			lock (dictionary)
			{
				if (typeof(TToken) == typeof(Unit))
				{
					StrongReferenceMessenger.Mapping mapping;
					if (!this.TryGetMapping<TMessage>(out mapping))
					{
						return message;
					}
					int count = mapping.Count;
					if (count == 0)
					{
						return message;
					}
					span = (array = ArrayPool<object>.Shared.Rent(2 * count));
					Dictionary2<StrongReferenceMessenger.Recipient, object>.Enumerator enumerator = mapping.GetEnumerator();
					while (enumerator.MoveNext())
					{
						*span[2 * num] = enumerator.GetValue();
						*span[2 * num + 1] = enumerator.GetKey().Target;
						num++;
					}
				}
				else
				{
					StrongReferenceMessenger.Mapping<TToken> mapping2;
					if (!this.TryGetMapping<TMessage, TToken>(out mapping2))
					{
						return message;
					}
					int count2 = mapping2.Count;
					if (count2 == 0)
					{
						return message;
					}
					span = (array = ArrayPool<object>.Shared.Rent(2 * count2));
					Dictionary2<StrongReferenceMessenger.Recipient, Dictionary2<TToken, object>>.Enumerator enumerator2 = mapping2.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						object obj;
						if (enumerator2.GetValue().TryGetValue(token, ref obj))
						{
							*span[2 * num] = obj;
							*span[2 * num + 1] = enumerator2.GetKey().Target;
							num++;
						}
					}
				}
			}
			try
			{
				WeakReferenceMessenger.SendAll<TMessage>(span, num, message);
			}
			finally
			{
				Array.Clear(array, 0, 2 * num);
				ArrayPool<object>.Shared.Return(array, false);
			}
			return message;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003838 File Offset: 0x00001A38
		void IMessenger.Cleanup()
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000383C File Offset: 0x00001A3C
		public void Reset()
		{
			Dictionary2<StrongReferenceMessenger.Recipient, HashSet<StrongReferenceMessenger.IMapping>> dictionary = this.recipientsMap;
			lock (dictionary)
			{
				this.recipientsMap.Clear();
				this.typesMap.Clear();
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000388C File Offset: 0x00001A8C
		[MethodImpl(256)]
		private bool TryGetMapping<TMessage>([Nullable(2)] [NotNullWhen(true)] out StrongReferenceMessenger.Mapping mapping) where TMessage : class
		{
			Type2 type = new Type2(typeof(TMessage), typeof(Unit));
			StrongReferenceMessenger.IMapping mapping2;
			if (this.typesMap.TryGetValue(type, ref mapping2))
			{
				mapping = Unsafe.As<StrongReferenceMessenger.Mapping>(mapping2);
				return true;
			}
			mapping = null;
			return false;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000038D4 File Offset: 0x00001AD4
		[MethodImpl(256)]
		private bool TryGetMapping<TMessage, [Nullable(0)] TToken>([Nullable(new byte[]
		{
			2,
			1
		})] [NotNullWhen(true)] out StrongReferenceMessenger.Mapping<TToken> mapping) where TMessage : class where TToken : IEquatable<TToken>
		{
			Type2 type = new Type2(typeof(TMessage), typeof(TToken));
			StrongReferenceMessenger.IMapping mapping2;
			if (this.typesMap.TryGetValue(type, ref mapping2))
			{
				mapping = Unsafe.As<StrongReferenceMessenger.Mapping<TToken>>(mapping2);
				return true;
			}
			mapping = null;
			return false;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000391C File Offset: 0x00001B1C
		[MethodImpl(256)]
		private StrongReferenceMessenger.Mapping GetOrAddMapping<TMessage>() where TMessage : class
		{
			Type2 type = new Type2(typeof(TMessage), typeof(Unit));
			ref StrongReferenceMessenger.IMapping orAddValueRef = this.typesMap.GetOrAddValueRef(type);
			if (orAddValueRef == null)
			{
				orAddValueRef = StrongReferenceMessenger.Mapping.Create<TMessage>();
			}
			return Unsafe.As<StrongReferenceMessenger.Mapping>(orAddValueRef);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003964 File Offset: 0x00001B64
		[MethodImpl(256)]
		private StrongReferenceMessenger.Mapping<TToken> GetOrAddMapping<TMessage, [Nullable(0)] TToken>() where TMessage : class where TToken : IEquatable<TToken>
		{
			Type2 type = new Type2(typeof(TMessage), typeof(TToken));
			ref StrongReferenceMessenger.IMapping orAddValueRef = this.typesMap.GetOrAddValueRef(type);
			if (orAddValueRef == null)
			{
				orAddValueRef = StrongReferenceMessenger.Mapping<TToken>.Create<TMessage>();
			}
			return Unsafe.As<StrongReferenceMessenger.Mapping<TToken>>(orAddValueRef);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000039AB File Offset: 0x00001BAB
		private static void ThrowInvalidOperationExceptionForDuplicateRegistration()
		{
			throw new InvalidOperationException("The target recipient has already subscribed to the target message.");
		}

		// Token: 0x04000023 RID: 35
		private readonly Dictionary2<StrongReferenceMessenger.Recipient, HashSet<StrongReferenceMessenger.IMapping>> recipientsMap = new Dictionary2<StrongReferenceMessenger.Recipient, HashSet<StrongReferenceMessenger.IMapping>>();

		// Token: 0x04000024 RID: 36
		private readonly Dictionary2<Type2, StrongReferenceMessenger.IMapping> typesMap = new Dictionary2<Type2, StrongReferenceMessenger.IMapping>();

		// Token: 0x02000055 RID: 85
		[NullableContext(0)]
		[Nullable(new byte[]
		{
			0,
			2
		})]
		private sealed class Mapping : Dictionary2<StrongReferenceMessenger.Recipient, object>, StrongReferenceMessenger.IMapping, IDictionary2<StrongReferenceMessenger.Recipient>, IDictionary2
		{
			// Token: 0x060001DE RID: 478 RVA: 0x00008280 File Offset: 0x00006480
			[NullableContext(1)]
			private Mapping(Type messageType)
			{
				this.TypeArguments = new Type2(messageType, typeof(Unit));
			}

			// Token: 0x060001DF RID: 479 RVA: 0x0000829E File Offset: 0x0000649E
			[NullableContext(1)]
			[MethodImpl(256)]
			public static StrongReferenceMessenger.Mapping Create<TMessage>() where TMessage : class
			{
				return new StrongReferenceMessenger.Mapping(typeof(TMessage));
			}

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x060001E0 RID: 480 RVA: 0x000082AF File Offset: 0x000064AF
			public Type2 TypeArguments { get; }
		}

		// Token: 0x02000056 RID: 86
		[NullableContext(0)]
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			2
		})]
		private sealed class Mapping<TToken> : Dictionary2<StrongReferenceMessenger.Recipient, Dictionary2<TToken, object>>, StrongReferenceMessenger.IMapping, IDictionary2<StrongReferenceMessenger.Recipient>, IDictionary2 where TToken : IEquatable<TToken>
		{
			// Token: 0x060001E1 RID: 481 RVA: 0x000082B7 File Offset: 0x000064B7
			[NullableContext(1)]
			private Mapping(Type messageType)
			{
				this.<TypeArguments>k__BackingField = new Type2(messageType, typeof(TToken));
			}

			// Token: 0x060001E2 RID: 482 RVA: 0x000082D5 File Offset: 0x000064D5
			[NullableContext(1)]
			[MethodImpl(256)]
			public static StrongReferenceMessenger.Mapping<TToken> Create<TMessage>() where TMessage : class
			{
				return new StrongReferenceMessenger.Mapping<TToken>(typeof(TMessage));
			}

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x060001E3 RID: 483 RVA: 0x000082E6 File Offset: 0x000064E6
			public Type2 TypeArguments
			{
				[CompilerGenerated]
				get
				{
					return this.<TypeArguments>k__BackingField;
				}
			}
		}

		// Token: 0x02000057 RID: 87
		[NullableContext(0)]
		private interface IMapping : IDictionary2<StrongReferenceMessenger.Recipient>, IDictionary2
		{
			// Token: 0x17000044 RID: 68
			// (get) Token: 0x060001E4 RID: 484
			Type2 TypeArguments { get; }
		}

		// Token: 0x02000058 RID: 88
		[NullableContext(0)]
		private readonly struct Recipient : IEquatable<StrongReferenceMessenger.Recipient>
		{
			// Token: 0x060001E5 RID: 485 RVA: 0x000082EE File Offset: 0x000064EE
			[NullableContext(1)]
			[MethodImpl(256)]
			public Recipient(object target)
			{
				this.Target = target;
			}

			// Token: 0x060001E6 RID: 486 RVA: 0x000082F7 File Offset: 0x000064F7
			[MethodImpl(256)]
			public bool Equals(StrongReferenceMessenger.Recipient other)
			{
				return this.Target == other.Target;
			}

			// Token: 0x060001E7 RID: 487 RVA: 0x00008308 File Offset: 0x00006508
			[NullableContext(2)]
			public override bool Equals(object obj)
			{
				if (obj is StrongReferenceMessenger.Recipient)
				{
					StrongReferenceMessenger.Recipient other = (StrongReferenceMessenger.Recipient)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x060001E8 RID: 488 RVA: 0x0000832D File Offset: 0x0000652D
			[MethodImpl(256)]
			public override int GetHashCode()
			{
				return RuntimeHelpers.GetHashCode(this.Target);
			}

			// Token: 0x0400009F RID: 159
			[Nullable(1)]
			public readonly object Target;
		}
	}
}
