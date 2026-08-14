using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000190 RID: 400
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlStream : IEnumerable<YamlDocument>, IEnumerable
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x0001C3FB File Offset: 0x0001A5FB
		public IList<YamlDocument> Documents
		{
			get
			{
				return this.documents;
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0001C403 File Offset: 0x0001A603
		public YamlStream()
		{
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0001C416 File Offset: 0x0001A616
		public YamlStream(params YamlDocument[] documents) : this(documents)
		{
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0001C420 File Offset: 0x0001A620
		public YamlStream(IEnumerable<YamlDocument> documents)
		{
			foreach (YamlDocument yamlDocument in documents)
			{
				this.documents.Add(yamlDocument);
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0001C480 File Offset: 0x0001A680
		public void Add(YamlDocument document)
		{
			this.documents.Add(document);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0001C48E File Offset: 0x0001A68E
		public void Load(TextReader input)
		{
			this.Load(new Parser(input));
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0001C49C File Offset: 0x0001A69C
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

		// Token: 0x06000872 RID: 2162 RVA: 0x0001C4DA File Offset: 0x0001A6DA
		public void Save(TextWriter output)
		{
			this.Save(output, true);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0001C4E4 File Offset: 0x0001A6E4
		public void Save(TextWriter output, bool assignAnchors)
		{
			this.Save(new Emitter(output), assignAnchors);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0001C4F4 File Offset: 0x0001A6F4
		public void Save(IEmitter emitter, bool assignAnchors)
		{
			emitter.Emit(new StreamStart());
			foreach (YamlDocument yamlDocument in this.documents)
			{
				yamlDocument.Save(emitter, assignAnchors);
			}
			emitter.Emit(new StreamEnd());
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0001C558 File Offset: 0x0001A758
		public void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0001C561 File Offset: 0x0001A761
		public IEnumerator<YamlDocument> GetEnumerator()
		{
			return this.documents.GetEnumerator();
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0001C56E File Offset: 0x0001A76E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000399 RID: 921
		private readonly IList<YamlDocument> documents = new List<YamlDocument>();
	}
}
