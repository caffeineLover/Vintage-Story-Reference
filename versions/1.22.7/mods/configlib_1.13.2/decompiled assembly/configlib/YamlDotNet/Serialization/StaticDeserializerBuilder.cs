using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.BufferedDeserialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;
using YamlDotNet.Serialization.NodeTypeResolvers;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.Schemas;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.Utilities;
using YamlDotNet.Serialization.ValueDeserializers;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000D2 RID: 210
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	public sealed class StaticDeserializerBuilder : StaticBuilderSkeleton<StaticDeserializerBuilder>
	{
		// Token: 0x060004A3 RID: 1187 RVA: 0x00011EAC File Offset: 0x000100AC
		public StaticDeserializerBuilder(StaticContext context) : base(context.GetTypeResolver())
		{
			this.context = context;
			this.factory = context.GetFactory();
			this.typeMappings = new Dictionary<Type, Type>();
			Dictionary<TagName, Type> dictionary = new Dictionary<TagName, Type>();
			dictionary.Add(FailsafeSchema.Tags.Map, typeof(Dictionary<object, object>));
			dictionary.Add(FailsafeSchema.Tags.Str, typeof(string));
			dictionary.Add(JsonSchema.Tags.Bool, typeof(bool));
			dictionary.Add(JsonSchema.Tags.Float, typeof(double));
			dictionary.Add(JsonSchema.Tags.Int, typeof(int));
			dictionary.Add(DefaultSchema.Tags.Timestamp, typeof(DateTime));
			this.tagMappings = dictionary;
			this.typeInspectorFactories.Add(typeof(CachedTypeInspector), (ITypeInspector inner) => new CachedTypeInspector(inner));
			this.typeInspectorFactories.Add(typeof(NamingConventionTypeInspector), delegate(ITypeInspector inner)
			{
				if (!(this.namingConvention is NullNamingConvention))
				{
					return new NamingConventionTypeInspector(inner, this.namingConvention);
				}
				return inner;
			});
			this.typeInspectorFactories.Add(typeof(YamlAttributesTypeInspector), (ITypeInspector inner) => new YamlAttributesTypeInspector(inner));
			LazyComponentRegistrationList<Nothing, INodeDeserializer> lazyComponentRegistrationList = new LazyComponentRegistrationList<Nothing, INodeDeserializer>();
			lazyComponentRegistrationList.Add(typeof(YamlConvertibleNodeDeserializer), (Nothing _) => new YamlConvertibleNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(YamlSerializableNodeDeserializer), (Nothing _) => new YamlSerializableNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(TypeConverterNodeDeserializer), (Nothing _) => new TypeConverterNodeDeserializer(base.BuildTypeConverters()));
			lazyComponentRegistrationList.Add(typeof(NullNodeDeserializer), (Nothing _) => new NullNodeDeserializer());
			lazyComponentRegistrationList.Add(typeof(ScalarNodeDeserializer), (Nothing _) => new ScalarNodeDeserializer(this.attemptUnknownTypeDeserialization, this.typeConverter, this.yamlFormatter));
			lazyComponentRegistrationList.Add(typeof(StaticArrayNodeDeserializer), (Nothing _) => new StaticArrayNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(StaticDictionaryNodeDeserializer), (Nothing _) => new StaticDictionaryNodeDeserializer(this.factory, this.duplicateKeyChecking));
			lazyComponentRegistrationList.Add(typeof(StaticCollectionNodeDeserializer), (Nothing _) => new StaticCollectionNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(ObjectNodeDeserializer), (Nothing _) => new ObjectNodeDeserializer(this.factory, this.BuildTypeInspector(), this.ignoreUnmatched, this.duplicateKeyChecking, this.typeConverter));
			this.nodeDeserializerFactories = lazyComponentRegistrationList;
			LazyComponentRegistrationList<Nothing, INodeTypeResolver> lazyComponentRegistrationList2 = new LazyComponentRegistrationList<Nothing, INodeTypeResolver>();
			lazyComponentRegistrationList2.Add(typeof(MappingNodeTypeResolver), (Nothing _) => new MappingNodeTypeResolver(this.typeMappings));
			lazyComponentRegistrationList2.Add(typeof(YamlConvertibleTypeResolver), (Nothing _) => new YamlConvertibleTypeResolver());
			lazyComponentRegistrationList2.Add(typeof(YamlSerializableTypeResolver), (Nothing _) => new YamlSerializableTypeResolver());
			lazyComponentRegistrationList2.Add(typeof(TagNodeTypeResolver), (Nothing _) => new TagNodeTypeResolver(this.tagMappings));
			lazyComponentRegistrationList2.Add(typeof(PreventUnknownTagsNodeTypeResolver), (Nothing _) => new PreventUnknownTagsNodeTypeResolver());
			lazyComponentRegistrationList2.Add(typeof(DefaultContainersNodeTypeResolver), (Nothing _) => new DefaultContainersNodeTypeResolver());
			this.nodeTypeResolverFactories = lazyComponentRegistrationList2;
			this.typeConverter = new NullTypeConverter();
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00002072 File Offset: 0x00000272
		protected override StaticDeserializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001221C File Offset: 0x0001041C
		internal ITypeInspector BuildTypeInspector()
		{
			ITypeInspector typeInspector = this.context.GetTypeInspector();
			return this.typeInspectorFactories.BuildComponentChain(typeInspector);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00012241 File Offset: 0x00010441
		public StaticDeserializerBuilder WithAttemptingUnquotedStringTypeDeserialization()
		{
			this.attemptUnknownTypeDeserialization = true;
			return this;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001224B File Offset: 0x0001044B
		public StaticDeserializerBuilder WithNodeDeserializer(INodeDeserializer nodeDeserializer)
		{
			return this.WithNodeDeserializer(nodeDeserializer, delegate(IRegistrationLocationSelectionSyntax<INodeDeserializer> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00012274 File Offset: 0x00010474
		public StaticDeserializerBuilder WithNodeDeserializer(INodeDeserializer nodeDeserializer, Action<IRegistrationLocationSelectionSyntax<INodeDeserializer>> where)
		{
			if (nodeDeserializer == null)
			{
				throw new ArgumentNullException("nodeDeserializer");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.nodeDeserializerFactories.CreateRegistrationLocationSelector(nodeDeserializer.GetType(), (Nothing _) => nodeDeserializer));
			return this;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000122D8 File Offset: 0x000104D8
		public StaticDeserializerBuilder WithNodeDeserializer<[Nullable(0)] TNodeDeserializer>(WrapperFactory<INodeDeserializer, TNodeDeserializer> nodeDeserializerFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeDeserializer>> where) where TNodeDeserializer : INodeDeserializer
		{
			StaticDeserializerBuilder.<>c__DisplayClass17_0<TNodeDeserializer> CS$<>8__locals1 = new StaticDeserializerBuilder.<>c__DisplayClass17_0<TNodeDeserializer>();
			CS$<>8__locals1.nodeDeserializerFactory = nodeDeserializerFactory;
			if (CS$<>8__locals1.nodeDeserializerFactory == null)
			{
				throw new ArgumentNullException("nodeDeserializerFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.nodeDeserializerFactories.CreateTrackingRegistrationLocationSelector(typeof(TNodeDeserializer), new Func<INodeDeserializer, Nothing, INodeDeserializer>(CS$<>8__locals1.<WithNodeDeserializer>b__0)));
			return this;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001233B File Offset: 0x0001053B
		public StaticDeserializerBuilder WithoutNodeDeserializer<[Nullable(0)] TNodeDeserializer>() where TNodeDeserializer : INodeDeserializer
		{
			return this.WithoutNodeDeserializer(typeof(TNodeDeserializer));
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001234D File Offset: 0x0001054D
		public StaticDeserializerBuilder WithoutNodeDeserializer(Type nodeDeserializerType)
		{
			if (nodeDeserializerType == null)
			{
				throw new ArgumentNullException("nodeDeserializerType");
			}
			this.nodeDeserializerFactories.Remove(nodeDeserializerType);
			return this;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00012370 File Offset: 0x00010570
		public StaticDeserializerBuilder WithTypeDiscriminatingNodeDeserializer(Action<ITypeDiscriminatingNodeDeserializerOptions> configureTypeDiscriminatingNodeDeserializerOptions, int maxDepth = -1, int maxLength = -1)
		{
			TypeDiscriminatingNodeDeserializerOptions typeDiscriminatingNodeDeserializerOptions = new TypeDiscriminatingNodeDeserializerOptions();
			configureTypeDiscriminatingNodeDeserializerOptions.Invoke(typeDiscriminatingNodeDeserializerOptions);
			TypeDiscriminatingNodeDeserializer nodeDeserializer = new TypeDiscriminatingNodeDeserializer(this.nodeDeserializerFactories.BuildComponentList<INodeDeserializer>(), typeDiscriminatingNodeDeserializerOptions.discriminators, maxDepth, maxLength);
			return this.WithNodeDeserializer(nodeDeserializer, delegate(IRegistrationLocationSelectionSyntax<INodeDeserializer> s)
			{
				s.Before<DictionaryNodeDeserializer>();
			});
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x000123C9 File Offset: 0x000105C9
		public StaticDeserializerBuilder WithNodeTypeResolver(INodeTypeResolver nodeTypeResolver)
		{
			return this.WithNodeTypeResolver(nodeTypeResolver, delegate(IRegistrationLocationSelectionSyntax<INodeTypeResolver> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x000123F4 File Offset: 0x000105F4
		public StaticDeserializerBuilder WithNodeTypeResolver(INodeTypeResolver nodeTypeResolver, Action<IRegistrationLocationSelectionSyntax<INodeTypeResolver>> where)
		{
			if (nodeTypeResolver == null)
			{
				throw new ArgumentNullException("nodeTypeResolver");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.nodeTypeResolverFactories.CreateRegistrationLocationSelector(nodeTypeResolver.GetType(), (Nothing _) => nodeTypeResolver));
			return this;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00012458 File Offset: 0x00010658
		public StaticDeserializerBuilder WithNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>(WrapperFactory<INodeTypeResolver, TNodeTypeResolver> nodeTypeResolverFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeTypeResolver>> where) where TNodeTypeResolver : INodeTypeResolver
		{
			StaticDeserializerBuilder.<>c__DisplayClass23_0<TNodeTypeResolver> CS$<>8__locals1 = new StaticDeserializerBuilder.<>c__DisplayClass23_0<TNodeTypeResolver>();
			CS$<>8__locals1.nodeTypeResolverFactory = nodeTypeResolverFactory;
			if (CS$<>8__locals1.nodeTypeResolverFactory == null)
			{
				throw new ArgumentNullException("nodeTypeResolverFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.nodeTypeResolverFactories.CreateTrackingRegistrationLocationSelector(typeof(TNodeTypeResolver), new Func<INodeTypeResolver, Nothing, INodeTypeResolver>(CS$<>8__locals1.<WithNodeTypeResolver>b__0)));
			return this;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000124BB File Offset: 0x000106BB
		public StaticDeserializerBuilder WithoutNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>() where TNodeTypeResolver : INodeTypeResolver
		{
			return this.WithoutNodeTypeResolver(typeof(TNodeTypeResolver));
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000124CD File Offset: 0x000106CD
		public StaticDeserializerBuilder WithoutNodeTypeResolver(Type nodeTypeResolverType)
		{
			if (nodeTypeResolverType == null)
			{
				throw new ArgumentNullException("nodeTypeResolverType");
			}
			this.nodeTypeResolverFactories.Remove(nodeTypeResolverType);
			return this;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000124F0 File Offset: 0x000106F0
		public override StaticDeserializerBuilder WithTagMapping(TagName tag, Type type)
		{
			if (tag.IsEmpty)
			{
				throw new ArgumentException("Non-specific tags cannot be maped");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Type type2;
			if (this.tagMappings.TryGetValue(tag, ref type2))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Type already has a registered type '");
				defaultInterpolatedStringHandler.AppendFormatted(type2.FullName);
				defaultInterpolatedStringHandler.AppendLiteral("' for tag '");
				defaultInterpolatedStringHandler.AppendFormatted<TagName>(tag);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "tag");
			}
			this.tagMappings.Add(tag, type);
			return this;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00012598 File Offset: 0x00010798
		public StaticDeserializerBuilder WithTypeMapping<[Nullable(2)] TInterface, [Nullable(0)] TConcrete>() where TConcrete : TInterface
		{
			Type typeFromHandle = typeof(TInterface);
			Type typeFromHandle2 = typeof(TConcrete);
			if (!typeFromHandle.IsAssignableFrom(typeFromHandle2))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(44, 2);
				defaultInterpolatedStringHandler.AppendLiteral("The type '");
				defaultInterpolatedStringHandler.AppendFormatted(typeFromHandle2.Name);
				defaultInterpolatedStringHandler.AppendLiteral("' does not implement interface '");
				defaultInterpolatedStringHandler.AppendFormatted(typeFromHandle.Name);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (this.typeMappings.ContainsKey(typeFromHandle))
			{
				this.typeMappings[typeFromHandle] = typeFromHandle2;
			}
			else
			{
				this.typeMappings.Add(typeFromHandle, typeFromHandle2);
			}
			return this;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00012644 File Offset: 0x00010844
		public StaticDeserializerBuilder WithoutTagMapping(TagName tag)
		{
			if (tag.IsEmpty)
			{
				throw new ArgumentException("Non-specific tags cannot be maped");
			}
			if (!this.tagMappings.Remove(tag))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(24, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tag '");
				defaultInterpolatedStringHandler.AppendFormatted<TagName>(tag);
				defaultInterpolatedStringHandler.AppendLiteral("' is not registered");
				throw new KeyNotFoundException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000126AB File Offset: 0x000108AB
		public StaticDeserializerBuilder IgnoreUnmatchedProperties()
		{
			this.ignoreUnmatched = true;
			return this;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000126B5 File Offset: 0x000108B5
		public StaticDeserializerBuilder WithDuplicateKeyChecking()
		{
			this.duplicateKeyChecking = true;
			return this;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x000126BF File Offset: 0x000108BF
		public IDeserializer Build()
		{
			return Deserializer.FromValueDeserializer(this.BuildValueDeserializer());
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000126CC File Offset: 0x000108CC
		public IValueDeserializer BuildValueDeserializer()
		{
			return new AliasValueDeserializer(new NodeValueDeserializer(this.nodeDeserializerFactories.BuildComponentList<INodeDeserializer>(), this.nodeTypeResolverFactories.BuildComponentList<INodeTypeResolver>(), this.typeConverter));
		}

		// Token: 0x04000204 RID: 516
		private readonly StaticContext context;

		// Token: 0x04000205 RID: 517
		private readonly StaticObjectFactory factory;

		// Token: 0x04000206 RID: 518
		private readonly LazyComponentRegistrationList<Nothing, INodeDeserializer> nodeDeserializerFactories;

		// Token: 0x04000207 RID: 519
		private readonly LazyComponentRegistrationList<Nothing, INodeTypeResolver> nodeTypeResolverFactories;

		// Token: 0x04000208 RID: 520
		private readonly Dictionary<TagName, Type> tagMappings;

		// Token: 0x04000209 RID: 521
		private readonly ITypeConverter typeConverter;

		// Token: 0x0400020A RID: 522
		private readonly Dictionary<Type, Type> typeMappings;

		// Token: 0x0400020B RID: 523
		private bool ignoreUnmatched;

		// Token: 0x0400020C RID: 524
		private bool duplicateKeyChecking;

		// Token: 0x0400020D RID: 525
		private bool attemptUnknownTypeDeserialization;
	}
}
