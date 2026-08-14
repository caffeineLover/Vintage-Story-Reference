using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200008B RID: 139
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmissionPhaseObjectGraphVisitorArgs
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00010B51 File Offset: 0x0000ED51
		// (set) Token: 0x0600038C RID: 908 RVA: 0x00010B59 File Offset: 0x0000ED59
		public IObjectGraphVisitor<IEmitter> InnerVisitor { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00010B62 File Offset: 0x0000ED62
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00010B6A File Offset: 0x0000ED6A
		public IEventEmitter EventEmitter { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00010B73 File Offset: 0x0000ED73
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00010B7B File Offset: 0x0000ED7B
		public ObjectSerializer NestedObjectSerializer { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00010B84 File Offset: 0x0000ED84
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00010B8C File Offset: 0x0000ED8C
		public IEnumerable<IYamlTypeConverter> TypeConverters { get; private set; }

		// Token: 0x06000393 RID: 915 RVA: 0x00010B98 File Offset: 0x0000ED98
		public EmissionPhaseObjectGraphVisitorArgs(IObjectGraphVisitor<IEmitter> innerVisitor, IEventEmitter eventEmitter, IEnumerable<IObjectGraphVisitor<Nothing>> preProcessingPhaseVisitors, IEnumerable<IYamlTypeConverter> typeConverters, ObjectSerializer nestedObjectSerializer)
		{
			if (innerVisitor == null)
			{
				throw new ArgumentNullException("innerVisitor");
			}
			this.InnerVisitor = innerVisitor;
			if (eventEmitter == null)
			{
				throw new ArgumentNullException("eventEmitter");
			}
			this.EventEmitter = eventEmitter;
			if (preProcessingPhaseVisitors == null)
			{
				throw new ArgumentNullException("preProcessingPhaseVisitors");
			}
			this.preProcessingPhaseVisitors = preProcessingPhaseVisitors;
			if (typeConverters == null)
			{
				throw new ArgumentNullException("typeConverters");
			}
			this.TypeConverters = typeConverters;
			if (nestedObjectSerializer == null)
			{
				throw new ArgumentNullException("nestedObjectSerializer");
			}
			this.NestedObjectSerializer = nestedObjectSerializer;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00010C1B File Offset: 0x0000EE1B
		public T GetPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] T>() where T : IObjectGraphVisitor<Nothing>
		{
			return Enumerable.Single<T>(Enumerable.OfType<T>(this.preProcessingPhaseVisitors));
		}

		// Token: 0x040001C6 RID: 454
		private readonly IEnumerable<IObjectGraphVisitor<Nothing>> preProcessingPhaseVisitors;
	}
}
