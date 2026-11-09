using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000B0 RID: 176
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlStream : IEnumerable<YamlDocument>, IEnumerable
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000DF6D File Offset: 0x0000C16D
		public IList<YamlDocument> Documents
		{
			get
			{
				return this.documents;
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000DF75 File Offset: 0x0000C175
		public YamlStream()
		{
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000DF88 File Offset: 0x0000C188
		public YamlStream(params YamlDocument[] documents) : this(documents)
		{
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000DF94 File Offset: 0x0000C194
		public YamlStream(IEnumerable<YamlDocument> documents)
		{
			foreach (YamlDocument yamlDocument in documents)
			{
				this.documents.Add(yamlDocument);
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000DFF4 File Offset: 0x0000C1F4
		public void Add(YamlDocument document)
		{
			this.documents.Add(document);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000E002 File Offset: 0x0000C202
		public void Load(TextReader input)
		{
			this.Load(new Parser(input));
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000E010 File Offset: 0x0000C210
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

		// Token: 0x060003FA RID: 1018 RVA: 0x0000E04E File Offset: 0x0000C24E
		public void Save(TextWriter output)
		{
			this.Save(output, true);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000E058 File Offset: 0x0000C258
		public void Save(TextWriter output, bool assignAnchors)
		{
			this.Save(new Emitter(output), assignAnchors);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000E068 File Offset: 0x0000C268
		public void Save(IEmitter emitter, bool assignAnchors)
		{
			emitter.Emit(new StreamStart());
			foreach (YamlDocument yamlDocument in this.documents)
			{
				yamlDocument.Save(emitter, assignAnchors);
			}
			emitter.Emit(new StreamEnd());
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000E0D4 File Offset: 0x0000C2D4
		public void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000E0DD File Offset: 0x0000C2DD
		public IEnumerator<YamlDocument> GetEnumerator()
		{
			return this.documents.GetEnumerator();
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000E0EF File Offset: 0x0000C2EF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000116 RID: 278
		private readonly List<YamlDocument> documents = new List<YamlDocument>();
	}
}
