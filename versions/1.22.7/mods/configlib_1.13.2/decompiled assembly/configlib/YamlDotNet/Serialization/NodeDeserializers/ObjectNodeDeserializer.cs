using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200014C RID: 332
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ObjectNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060006C1 RID: 1729 RVA: 0x00017508 File Offset: 0x00015708
		public ObjectNodeDeserializer(IObjectFactory objectFactory, ITypeInspector typeDescriptor, bool ignoreUnmatched, bool duplicateKeyChecking, ITypeConverter typeConverter)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = objectFactory;
			if (typeDescriptor == null)
			{
				throw new ArgumentNullException("typeDescriptor");
			}
			this.typeDescriptor = typeDescriptor;
			this.ignoreUnmatched = ignoreUnmatched;
			this.duplicateKeyChecking = duplicateKeyChecking;
			if (typeConverter == null)
			{
				throw new ArgumentNullException("typeConverter");
			}
			this.typeConverter = typeConverter;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00017570 File Offset: 0x00015770
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			MappingStart mappingStart;
			if (!parser.TryConsume(out mappingStart))
			{
				value = null;
				return false;
			}
			Type type = Nullable.GetUnderlyingType(expectedType) ?? expectedType;
			value = this.objectFactory.Create(type);
			this.objectFactory.ExecuteOnDeserializing(value);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			MappingEnd mappingEnd;
			while (!parser.TryConsume(out mappingEnd))
			{
				Scalar scalar = parser.Consume<Scalar>();
				if (this.duplicateKeyChecking && !hashSet.Add(scalar.Value))
				{
					Mark start = scalar.Start;
					Mark end = scalar.End;
					throw new YamlException(ref start, ref end, "Encountered duplicate key " + scalar.Value);
				}
				try
				{
					IPropertyDescriptor property = this.typeDescriptor.GetProperty(type, null, scalar.Value, this.ignoreUnmatched);
					if (property == null)
					{
						parser.SkipThisAndNestedEvents();
					}
					else
					{
						object obj = nestedObjectDeserializer.Invoke(parser, property.Type);
						IValuePromise valuePromise = obj as IValuePromise;
						if (valuePromise != null)
						{
							object valueRef = value;
							valuePromise.ValueAvailable += delegate(object v)
							{
								object value3 = this.typeConverter.ChangeType(v, property.Type);
								property.Write(valueRef, value3);
							};
						}
						else
						{
							object value2 = this.typeConverter.ChangeType(obj, property.Type);
							property.Write(value, value2);
						}
					}
				}
				catch (SerializationException ex)
				{
					Mark start = scalar.Start;
					Mark end = scalar.End;
					throw new YamlException(ref start, ref end, ex.Message);
				}
				catch (YamlException)
				{
					throw;
				}
				catch (Exception innerException)
				{
					Mark start = scalar.Start;
					Mark end = scalar.End;
					throw new YamlException(ref start, ref end, "Exception during deserialization", innerException);
				}
			}
			this.objectFactory.ExecuteOnDeserialized(value);
			return true;
		}

		// Token: 0x040002E4 RID: 740
		private readonly IObjectFactory objectFactory;

		// Token: 0x040002E5 RID: 741
		private readonly ITypeInspector typeDescriptor;

		// Token: 0x040002E6 RID: 742
		private readonly bool ignoreUnmatched;

		// Token: 0x040002E7 RID: 743
		private readonly bool duplicateKeyChecking;

		// Token: 0x040002E8 RID: 744
		private readonly ITypeConverter typeConverter;
	}
}
