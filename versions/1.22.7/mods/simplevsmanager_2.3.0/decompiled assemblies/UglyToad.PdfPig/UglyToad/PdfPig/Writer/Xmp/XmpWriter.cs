using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer.Xmp
{
	// Token: 0x0200001E RID: 30
	internal static class XmpWriter
	{
		// Token: 0x060000EC RID: 236 RVA: 0x00008E70 File Offset: 0x00007070
		public static StreamToken GenerateXmpStream(PdfDocumentBuilder.DocumentInformationBuilder builder, decimal version, PdfAStandard standard)
		{
			XNamespace xnamespace = "adobe:ns:meta/";
			XNamespace xnamespace2 = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
			XAttribute xattribute = new XAttribute(xnamespace2 + "about", string.Empty);
			XElement xelement = new XElement(xnamespace2 + "Description", xattribute);
			XElement parent = xelement;
			string prefix = "dc";
			string ns = "http://purl.org/dc/elements/1.1/";
			List<XmpWriter.SchemaMapper> list = new List<XmpWriter.SchemaMapper>();
			list.Add(new XmpWriter.SchemaMapper("format", (PdfDocumentBuilder.DocumentInformationBuilder b) => "application/pdf"));
			list.Add(new XmpWriter.SchemaMapper("creator", (PdfDocumentBuilder.DocumentInformationBuilder b) => b.Author));
			list.Add(new XmpWriter.SchemaMapper("description", (PdfDocumentBuilder.DocumentInformationBuilder b) => b.Subject));
			list.Add(new XmpWriter.SchemaMapper("title", (PdfDocumentBuilder.DocumentInformationBuilder b) => b.Title));
			XmpWriter.AddElementsForSchema(parent, prefix, ns, builder, list);
			XElement parent2 = xelement;
			string prefix2 = "xmp";
			string ns2 = "http://ns.adobe.com/xap/1.0/";
			List<XmpWriter.SchemaMapper> list2 = new List<XmpWriter.SchemaMapper>();
			list2.Add(new XmpWriter.SchemaMapper("CreatorTool", (PdfDocumentBuilder.DocumentInformationBuilder b) => b.Creator));
			XmpWriter.AddElementsForSchema(parent2, prefix2, ns2, builder, list2);
			XElement parent3 = xelement;
			string prefix3 = "pdf";
			string ns3 = "http://ns.adobe.com/pdf/1.3/";
			List<XmpWriter.SchemaMapper> list3 = new List<XmpWriter.SchemaMapper>();
			list3.Add(new XmpWriter.SchemaMapper("PDFVersion", (PdfDocumentBuilder.DocumentInformationBuilder b) => version.ToString("F1", CultureInfo.InvariantCulture)));
			list3.Add(new XmpWriter.SchemaMapper("Producer", (PdfDocumentBuilder.DocumentInformationBuilder b) => b.Producer));
			XmpWriter.AddElementsForSchema(parent3, prefix3, ns3, builder, list3);
			XElement versionAndConformanceLevelIdentificationElement = XmpWriter.GetVersionAndConformanceLevelIdentificationElement(xnamespace2, xattribute, standard);
			string text = new XDocument(new object[]
			{
				new XElement(xnamespace + "xmpmeta", new object[]
				{
					XmpWriter.GetNamespaceAttribute("x", "adobe:ns:meta/"),
					new XAttribute(xnamespace + "xmptk", "Adobe XMP Core 5.6-c014 79.156797, 2014/08/20-09:53:02        "),
					new XElement(xnamespace2 + "RDF", new object[]
					{
						XmpWriter.GetNamespaceAttribute("rdf", xnamespace2),
						xelement,
						versionAndConformanceLevelIdentificationElement
					})
				})
			}).ToString(0).Replace("\r\n", "\n");
			text = "<?xpacket begin=\"﻿\" id=\"W5M0MpCehiHzreSzNTczkc9d\"?>\n" + text + "\n<?xpacket end=\"r\"?>";
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			dictionary.Add(NameToken.Type, NameToken.Metadata);
			dictionary.Add(NameToken.Subtype, NameToken.Xml);
			dictionary.Add(NameToken.Length, new NumericToken(bytes.Length));
			return new StreamToken(new DictionaryToken(dictionary), bytes);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00009154 File Offset: 0x00007354
		private static XAttribute GetNamespaceAttribute(string prefix, XNamespace ns)
		{
			return new XAttribute(XNamespace.Xmlns + prefix, ns);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00009168 File Offset: 0x00007368
		private static void AddElementsForSchema(XElement parent, string prefix, string ns, PdfDocumentBuilder.DocumentInformationBuilder builder, List<XmpWriter.SchemaMapper> mappers)
		{
			XNamespace xnamespace = XNamespace.Get(ns);
			parent.Add(XmpWriter.GetNamespaceAttribute(prefix, xnamespace));
			foreach (XmpWriter.SchemaMapper schemaMapper in mappers)
			{
				string text = schemaMapper.ValueFunc.Invoke(builder);
				if (text != null)
				{
					parent.Add(new XElement(xnamespace + schemaMapper.Name, text));
				}
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000091EC File Offset: 0x000073EC
		private static XElement GetVersionAndConformanceLevelIdentificationElement(XNamespace rdf, XAttribute emptyRdfAbout, PdfAStandard standard)
		{
			XNamespace xnamespace = "http://www.aiim.org/pdfa/ns/id/";
			XElement xelement = new XElement(rdf + "Description", new object[]
			{
				emptyRdfAbout,
				XmpWriter.GetNamespaceAttribute("pdfaid", xnamespace)
			});
			int num;
			string text;
			switch (standard)
			{
			case PdfAStandard.A1B:
				num = 1;
				text = "B";
				break;
			case PdfAStandard.A1A:
				num = 1;
				text = "A";
				break;
			case PdfAStandard.A2B:
				num = 2;
				text = "B";
				break;
			case PdfAStandard.A2A:
				num = 2;
				text = "A";
				break;
			case PdfAStandard.A3B:
				num = 3;
				text = "B";
				break;
			case PdfAStandard.A3A:
				num = 3;
				text = "A";
				break;
			default:
				throw new ArgumentOutOfRangeException("standard", standard, null);
			}
			xelement.Add(new XElement(xnamespace + "part", num));
			xelement.Add(new XElement(xnamespace + "conformance", text));
			return xelement;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000092D4 File Offset: 0x000074D4
		private static XElement GetExtensionSchemasElement(XNamespace rdf, XAttribute emptyRdfAbout)
		{
			XNamespace xnamespace = "http://www.aiim.org/pdfa/ns/extension/";
			XNamespace xnamespace2 = "http://www.aiim.org/pdfa/ns/schema#";
			XNamespace xnamespace3 = "http://www.aiim.org/pdfa/ns/property#";
			XElement xelement = new XElement(rdf + "Description", new object[]
			{
				emptyRdfAbout,
				XmpWriter.GetNamespaceAttribute("pdfaExtension", xnamespace),
				XmpWriter.GetNamespaceAttribute("pdfaSchema", xnamespace2),
				XmpWriter.GetNamespaceAttribute("pdfaProperty", xnamespace3)
			});
			XElement xelement2 = new XElement(xnamespace + "schemas", new XElement(rdf + "Bag"));
			XElement xelement3 = new XElement(rdf + "li", new XAttribute(rdf + "parseType", "Resource"));
			xelement3.Add(new XElement(xnamespace2 + "namespaceURI", "http://www.aiim.org/pdfa/ns/id/"));
			xelement3.Add(new XElement(xnamespace2 + "prefix", "pdfaid"));
			xelement3.Add(new XElement(xnamespace2 + "schema", "PDF/A ID Schema"));
			XElement xelement4 = new XElement(xnamespace2 + "property", new XElement(rdf + "Seq"));
			XElement xelement5 = Enumerable.Last<XElement>(xelement4.Elements());
			xelement5.Add(XmpWriter.GetSchemaPropertyListItem(rdf, xnamespace3, "part", "Part of PDF/A standard", "internal", "Integer"));
			xelement5.Add(XmpWriter.GetSchemaPropertyListItem(rdf, xnamespace3, "amd", "Amendment of PDF/A standard", "internal", "Text"));
			xelement5.Add(XmpWriter.GetSchemaPropertyListItem(rdf, xnamespace3, "conformance", "Conformance level of PDF/A standard", "internal", "Text"));
			xelement3.Add(xelement4);
			Enumerable.Last<XElement>(xelement2.Elements()).Add(xelement3);
			xelement.Add(xelement2);
			return xelement;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00009494 File Offset: 0x00007694
		private static XElement GetSchemaPropertyListItem(XNamespace rdfNs, XNamespace pdfaPropertyNs, string name, string description, string category = "internal", string valueType = "Text")
		{
			XElement xelement = new XElement(rdfNs + "li", new XAttribute(rdfNs + "parseType", "Resource"));
			xelement.Add(new XElement(pdfaPropertyNs + "category", category));
			xelement.Add(new XElement(pdfaPropertyNs + "description", description));
			xelement.Add(new XElement(pdfaPropertyNs + "name", name));
			xelement.Add(new XElement(pdfaPropertyNs + "valueType", valueType));
			return xelement;
		}

		// Token: 0x04000087 RID: 135
		private const string Xmptk = "Adobe XMP Core 5.6-c014 79.156797, 2014/08/20-09:53:02        ";

		// Token: 0x04000088 RID: 136
		private const string RdfNamespace = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

		// Token: 0x04000089 RID: 137
		private const string XmpMetaPrefix = "x";

		// Token: 0x0400008A RID: 138
		private const string XmpMetaNamespace = "adobe:ns:meta/";

		// Token: 0x0400008B RID: 139
		private const string DublinCorePrefix = "dc";

		// Token: 0x0400008C RID: 140
		private const string DublinCoreNamespace = "http://purl.org/dc/elements/1.1/";

		// Token: 0x0400008D RID: 141
		private const string XmpBasicPrefix = "xmp";

		// Token: 0x0400008E RID: 142
		private const string XmpBasicNamespace = "http://ns.adobe.com/xap/1.0/";

		// Token: 0x0400008F RID: 143
		private const string XmpRightsManagementPrefix = "xmpRights";

		// Token: 0x04000090 RID: 144
		private const string XmpRightsManagementNamespace = "http://ns.adobe.com/xap/1.0/rights/";

		// Token: 0x04000091 RID: 145
		private const string XmpMediaManagementPrefix = "xmpMM";

		// Token: 0x04000092 RID: 146
		private const string XmpMediaManagementNamespace = "http://ns.adobe.com/xap/1.0/mm/";

		// Token: 0x04000093 RID: 147
		private const string AdobePdfPrefix = "pdf";

		// Token: 0x04000094 RID: 148
		private const string AdobePdfNamespace = "http://ns.adobe.com/pdf/1.3/";

		// Token: 0x04000095 RID: 149
		private const string PdfAIdentificationExtensionPrefix = "pdfaid";

		// Token: 0x04000096 RID: 150
		private const string PdfAIdentificationExtensionNamespace = "http://www.aiim.org/pdfa/ns/id/";

		// Token: 0x02000240 RID: 576
		private class SchemaMapper
		{
			// Token: 0x170003EE RID: 1006
			// (get) Token: 0x06000D40 RID: 3392 RVA: 0x00040547 File Offset: 0x0003E747
			public string Name { get; }

			// Token: 0x170003EF RID: 1007
			// (get) Token: 0x06000D41 RID: 3393 RVA: 0x0004054F File Offset: 0x0003E74F
			public Func<PdfDocumentBuilder.DocumentInformationBuilder, string> ValueFunc { get; }

			// Token: 0x06000D42 RID: 3394 RVA: 0x00040557 File Offset: 0x0003E757
			public SchemaMapper(string name, Func<PdfDocumentBuilder.DocumentInformationBuilder, string> valueFunc)
			{
				this.Name = name;
				this.ValueFunc = valueFunc;
			}
		}
	}
}
