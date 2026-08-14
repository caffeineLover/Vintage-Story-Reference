using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000150 RID: 336
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ObjectNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x00017E50 File Offset: 0x00016050
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

		// Token: 0x060006FE RID: 1790 RVA: 0x00017EB8 File Offset: 0x000160B8
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

		// Token: 0x04000312 RID: 786
		private readonly IObjectFactory objectFactory;

		// Token: 0x04000313 RID: 787
		private readonly ITypeInspector typeDescriptor;

		// Token: 0x04000314 RID: 788
		private readonly bool ignoreUnmatched;

		// Token: 0x04000315 RID: 789
		private readonly bool duplicateKeyChecking;

		// Token: 0x04000316 RID: 790
		private readonly ITypeConverter typeConverter;
	}
}
