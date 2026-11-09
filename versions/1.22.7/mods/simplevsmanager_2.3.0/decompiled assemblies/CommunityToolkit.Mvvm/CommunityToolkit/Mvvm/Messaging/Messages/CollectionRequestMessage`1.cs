using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging.Messages
{
	// Token: 0x02000016 RID: 22
	[NullableContext(1)]
	[Nullable(0)]
	public class CollectionRequestMessage<[Nullable(2)] T> : IEnumerable<T>, IEnumerable
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000043D9 File Offset: 0x000025D9
		public IReadOnlyCollection<T> Responses
		{
			get
			{
				return this.responses;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000043E1 File Offset: 0x000025E1
		public void Reply(T response)
		{
			this.responses.Add(response);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000043EF File Offset: 0x000025EF
		[EditorBrowsable(1)]
		[MethodImpl(256)]
		public IEnumerator<T> GetEnumerator()
		{
			return this.responses.GetEnumerator();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004401 File Offset: 0x00002601
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004409 File Offset: 0x00002609
		public CollectionRequestMessage()
		{
			this.responses = new List<T>();
			base..ctor();
		}

		// Token: 0x0400002C RID: 44
		private readonly List<T> responses;
	}
}
