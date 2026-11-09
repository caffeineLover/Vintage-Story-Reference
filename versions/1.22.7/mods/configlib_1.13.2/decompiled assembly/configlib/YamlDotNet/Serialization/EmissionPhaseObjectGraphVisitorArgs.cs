using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000087 RID: 135
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmissionPhaseObjectGraphVisitorArgs
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00010209 File Offset: 0x0000E409
		// (set) Token: 0x06000350 RID: 848 RVA: 0x00010211 File Offset: 0x0000E411
		public IObjectGraphVisitor<IEmitter> InnerVisitor { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0001021A File Offset: 0x0000E41A
		// (set) Token: 0x06000352 RID: 850 RVA: 0x00010222 File Offset: 0x0000E422
		public IEventEmitter EventEmitter { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0001022B File Offset: 0x0000E42B
		// (set) Token: 0x06000354 RID: 852 RVA: 0x00010233 File Offset: 0x0000E433
		public ObjectSerializer NestedObjectSerializer { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0001023C File Offset: 0x0000E43C
		// (set) Token: 0x06000356 RID: 854 RVA: 0x00010244 File Offset: 0x0000E444
		public IEnumerable<IYamlTypeConverter> TypeConverters { get; private set; }

		// Token: 0x06000357 RID: 855 RVA: 0x00010250 File Offset: 0x0000E450
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

		// Token: 0x06000358 RID: 856 RVA: 0x000102D3 File Offset: 0x0000E4D3
		public T GetPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] T>() where T : IObjectGraphVisitor<Nothing>
		{
			return Enumerable.Single<T>(Enumerable.OfType<T>(this.preProcessingPhaseVisitors));
		}

		// Token: 0x04000198 RID: 408
		private readonly IEnumerable<IObjectGraphVisitor<Nothing>> preProcessingPhaseVisitors;
	}
}
