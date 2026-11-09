using System;
using System.Collections.Generic;

namespace WinRT
{
	// Token: 0x020001B7 RID: 439
	internal sealed class EventRegistrationTokenTable<T> where T : class, Delegate
	{
		// Token: 0x06000E12 RID: 3602 RVA: 0x0002ACB0 File Offset: 0x00028EB0
		public EventRegistrationToken AddEventHandler(T handler)
		{
			EventRegistrationToken result;
			if (handler == null)
			{
				result = default(EventRegistrationToken);
				return result;
			}
			Dictionary<EventRegistrationToken, T> tokens = this.m_tokens;
			lock (tokens)
			{
				result = this.AddEventHandlerNoLock(handler);
			}
			return result;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0002AD0C File Offset: 0x00028F0C
		private EventRegistrationToken AddEventHandlerNoLock(T handler)
		{
			EventRegistrationToken eventRegistrationToken = EventRegistrationTokenTable<T>.GetPreferredToken(handler);
			while (this.m_tokens.ContainsKey(eventRegistrationToken))
			{
				eventRegistrationToken = new EventRegistrationToken
				{
					Value = eventRegistrationToken.Value + 1L
				};
			}
			this.m_tokens[eventRegistrationToken] = handler;
			return eventRegistrationToken;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0002AD58 File Offset: 0x00028F58
		private static EventRegistrationToken GetPreferredToken(T handler)
		{
			Delegate[] invocationList = ((Delegate)((object)handler)).GetInvocationList();
			uint hashCode;
			if (invocationList.Length == 1)
			{
				hashCode = (uint)invocationList[0].Method.GetHashCode();
			}
			else
			{
				hashCode = (uint)handler.GetHashCode();
			}
			ulong value = (ulong)typeof(T).MetadataToken << 32 | (ulong)hashCode;
			return new EventRegistrationToken
			{
				Value = (long)value
			};
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0002ADC0 File Offset: 0x00028FC0
		public bool RemoveEventHandler(EventRegistrationToken token, out T handler)
		{
			Dictionary<EventRegistrationToken, T> tokens = this.m_tokens;
			lock (tokens)
			{
				if (this.m_tokens.TryGetValue(token, ref handler))
				{
					this.RemoveEventHandlerNoLock(token);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0002AE18 File Offset: 0x00029018
		private void RemoveEventHandlerNoLock(EventRegistrationToken token)
		{
			T t;
			if (this.m_tokens.TryGetValue(token, ref t))
			{
				this.m_tokens.Remove(token);
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0002AE42 File Offset: 0x00029042
		public EventRegistrationTokenTable()
		{
			this.m_tokens = new Dictionary<EventRegistrationToken, T>();
			base..ctor();
		}

		// Token: 0x040002C2 RID: 706
		private readonly Dictionary<EventRegistrationToken, T> m_tokens;
	}
}
