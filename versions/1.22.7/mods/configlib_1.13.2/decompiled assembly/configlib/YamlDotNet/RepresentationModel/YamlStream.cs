using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200018C RID: 396
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlStream : IEnumerable<YamlDocument>, IEnumerable
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0001BAB3 File Offset: 0x00019CB3
		public IList<YamlDocument> Documents
		{
			get
			{
				return this.documents;
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001BABB File Offset: 0x00019CBB
		public YamlStream()
		{
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001BACE File Offset: 0x00019CCE
		public YamlStream(params YamlDocument[] documents) : this(documents)
		{
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001BAD8 File Offset: 0x00019CD8
		public YamlStream(IEnumerable<YamlDocument> documents)
		{
			foreach (YamlDocument yamlDocument in documents)
			{
				this.documents.Add(yamlDocument);
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001BB38 File Offset: 0x00019D38
		public void Add(YamlDocument document)
		{
			this.documents.Add(document);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001BB46 File Offset: 0x00019D46
		public void Load(TextReader input)
		{
			this.Load(new Parser(input));
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001BB54 File Offset: 0x00019D54
		public void Load(IParser parser)
		{
			this.documents.Clear();
			parser.Consume<StreamStart>();
			StreamEnd streamEnd;
			while (!parser.TryConsume(out streamEnd))
			{
				YamlDocument yamlDocument = new YamlDocument(parser);
				this.documents.Add(yamlDocument);
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001BB92 File Offset: 0x00019D92
		public void Save(TextWriter output)
		{
			this.Save(output, true);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001BB9C File Offset: 0x00019D9C
		public void Save(TextWriter output, bool assignAnchors)
		{
			this.Save(new Emitter(output), assignAnchors);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001BBAC File Offset: 0x00019DAC
		public void Save(IEmitter emitter, bool assignAnchors)
		{
			emitter.Emit(new StreamStart());
			foreach (YamlDocument yamlDocument in this.documents)
			{
				yamlDocument.Save(emitter, assignAnchors);
			}
			emitter.Emit(new StreamEnd());
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001BC10 File Offset: 0x00019E10
		public void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001BC19 File Offset: 0x00019E19
		public IEnumerator<YamlDocument> GetEnumerator()
		{
			return this.documents.GetEnumerator();
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001BC26 File Offset: 0x00019E26
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400036B RID: 875
		private readonly IList<YamlDocument> documents = new List<YamlDocument>();
	}
}
