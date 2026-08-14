using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200000D RID: 13
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmissionPhaseObjectGraphVisitorArgs
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000037AC File Offset: 0x000019AC
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000037B4 File Offset: 0x000019B4
		public IObjectGraphVisitor<IEmitter> InnerVisitor { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000037BD File Offset: 0x000019BD
		// (set) Token: 0x0600007A RID: 122 RVA: 0x000037C5 File Offset: 0x000019C5
		public IEventEmitter EventEmitter { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000037CE File Offset: 0x000019CE
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000037D6 File Offset: 0x000019D6
		public ObjectSerializer NestedObjectSerializer { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000037DF File Offset: 0x000019DF
		// (set) Token: 0x0600007E RID: 126 RVA: 0x000037E7 File Offset: 0x000019E7
		public IEnumerable<IYamlTypeConverter> TypeConverters { get; private set; }

		// Token: 0x0600007F RID: 127 RVA: 0x000037F0 File Offset: 0x000019F0
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

		// Token: 0x06000080 RID: 128 RVA: 0x00003873 File Offset: 0x00001A73
		public T GetPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] T>() where T : IObjectGraphVisitor<Nothing>
		{
			return Enumerable.Single<T>(Enumerable.OfType<T>(this.preProcessingPhaseVisitors));
		}

		// Token: 0x04000025 RID: 37
		private readonly IEnumerable<IObjectGraphVisitor<Nothing>> preProcessingPhaseVisitors;
	}
}
