using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.ObjectGraphTraversalStrategies;
using YamlDotNet.Serialization.ObjectGraphVisitors;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000BC RID: 188
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	[RequiresDynamicCode("This builder configures the serializer to use reflection which is not compatible with ahead-of-time compilation or assembly trimming. You need to use the code generator/analyzer to generate static code and use the 'StaticSerializerBuilder' object instead of this one.")]
	public sealed class SerializerBuilder : BuilderSkeleton<SerializerBuilder>
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x00010DD8 File Offset: 0x0000EFD8
		public SerializerBuilder() : base(new DynamicTypeResolver())
		{
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
			this.typeInspectorFactories.Add(typeof(YamlAttributeOverridesInspector), delegate(ITypeInspector inner)
			{
				if (this.overrides == null)
				{
					return inner;
				}
				return new YamlAttributeOverridesInspector(inner, this.overrides.Clone());
			});
			LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> lazyComponentRegistrationList = new LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>();
			lazyComponentRegistrationList.Add(typeof(AnchorAssigner), (IEnumerable<IYamlTypeConverter> typeConverters) => new AnchorAssigner(typeConverters));
			this.preProcessingPhaseObjectGraphVisitorFactories = lazyComponentRegistrationList;
			LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> lazyComponentRegistrationList2 = new LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>>();
			lazyComponentRegistrationList2.Add(typeof(CustomSerializationObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new CustomSerializationObjectGraphVisitor(args.InnerVisitor, args.TypeConverters, args.NestedObjectSerializer));
			lazyComponentRegistrationList2.Add(typeof(AnchorAssigningObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new AnchorAssigningObjectGraphVisitor(args.InnerVisitor, args.EventEmitter, args.GetPreProcessingPhaseObjectGraphVisitor<AnchorAssigner>()));
			lazyComponentRegistrationList2.Add(typeof(DefaultValuesObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new DefaultValuesObjectGraphVisitor(this.defaultValuesHandlingConfiguration, args.InnerVisitor, new DefaultObjectFactory()));
			lazyComponentRegistrationList2.Add(typeof(CommentsObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new CommentsObjectGraphVisitor(args.InnerVisitor));
			this.emissionPhaseObjectGraphVisitorFactories = lazyComponentRegistrationList2;
			this.eventEmitterFactories = new LazyComponentRegistrationList<IEventEmitter, IEventEmitter>
			{
				{
					typeof(TypeAssigningEventEmitter),
					(IEventEmitter inner) => new TypeAssigningEventEmitter(inner, false, this.tagMappings, this.quoteNecessaryStrings, this.quoteYaml1_1Strings, this.defaultScalarStyle, this.yamlFormatter)
				}
			};
			this.objectFactory = new DefaultObjectFactory();
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new FullObjectGraphTraversalStrategy(typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.objectFactory));
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00002072 File Offset: 0x00000272
		protected override SerializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00010FEA File Offset: 0x0000F1EA
		public SerializerBuilder WithQuotingNecessaryStrings(bool quoteYaml1_1Strings = false)
		{
			this.quoteNecessaryStrings = true;
			this.quoteYaml1_1Strings = quoteYaml1_1Strings;
			return this;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00010FFB File Offset: 0x0000F1FB
		public SerializerBuilder WithDefaultScalarStyle(ScalarStyle style)
		{
			this.defaultScalarStyle = style;
			return this;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00011008 File Offset: 0x0000F208
		public SerializerBuilder WithMaximumRecursion(int maximumRecursion)
		{
			if (maximumRecursion <= 0)
			{
				string text = "maximumRecursion";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(79, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The maximum recursion specified (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(maximumRecursion);
				defaultInterpolatedStringHandler.AppendLiteral(") is invalid. It should be a positive integer.");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.maximumRecursion = maximumRecursion;
			return this;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001105D File Offset: 0x0000F25D
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory) where TEventEmitter : IEventEmitter
		{
			Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where;
			if ((where = SerializerBuilder.<>c__18<TEventEmitter>.<>9__18_0) == null)
			{
				where = (SerializerBuilder.<>c__18<TEventEmitter>.<>9__18_0 = new Action<IRegistrationLocationSelectionSyntax<IEventEmitter>>(SerializerBuilder.<>c__18<TEventEmitter>.<>9.<WithEventEmitter>b__18_0));
			}
			return this.WithEventEmitter<TEventEmitter>(eventEmitterFactory, where);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00011088 File Offset: 0x0000F288
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory, Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			SerializerBuilder.<>c__DisplayClass19_0<TEventEmitter> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass19_0<TEventEmitter>();
			CS$<>8__locals1.eventEmitterFactory = eventEmitterFactory;
			if (CS$<>8__locals1.eventEmitterFactory == null)
			{
				throw new ArgumentNullException("eventEmitterFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.eventEmitterFactories.CreateRegistrationLocationSelector(typeof(TEventEmitter), new Func<IEventEmitter, IEventEmitter>(CS$<>8__locals1.<WithEventEmitter>b__0)));
			return this.Self;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000110F0 File Offset: 0x0000F2F0
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(WrapperFactory<IEventEmitter, IEventEmitter, TEventEmitter> eventEmitterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			SerializerBuilder.<>c__DisplayClass20_0<TEventEmitter> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass20_0<TEventEmitter>();
			CS$<>8__locals1.eventEmitterFactory = eventEmitterFactory;
			if (CS$<>8__locals1.eventEmitterFactory == null)
			{
				throw new ArgumentNullException("eventEmitterFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.eventEmitterFactories.CreateTrackingRegistrationLocationSelector(typeof(TEventEmitter), new Func<IEventEmitter, IEventEmitter, IEventEmitter>(CS$<>8__locals1.<WithEventEmitter>b__0)));
			return this.Self;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00011158 File Offset: 0x0000F358
		public SerializerBuilder WithoutEventEmitter<[Nullable(0)] TEventEmitter>() where TEventEmitter : IEventEmitter
		{
			return this.WithoutEventEmitter(typeof(TEventEmitter));
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001116A File Offset: 0x0000F36A
		public SerializerBuilder WithoutEventEmitter(Type eventEmitterType)
		{
			if (eventEmitterType == null)
			{
				throw new ArgumentNullException("eventEmitterType");
			}
			this.eventEmitterFactories.Remove(eventEmitterType);
			return this;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00011190 File Offset: 0x0000F390
		public override SerializerBuilder WithTagMapping(TagName tag, Type type)
		{
			if (tag.IsEmpty)
			{
				throw new ArgumentException("Non-specific tags cannot be maped");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			TagName tagName;
			if (this.tagMappings.TryGetValue(type, ref tagName))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Type already has a registered tag '");
				defaultInterpolatedStringHandler.AppendFormatted<TagName>(tagName);
				defaultInterpolatedStringHandler.AppendLiteral("' for type '");
				defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "type");
			}
			this.tagMappings.Add(type, tag);
			return this;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00011238 File Offset: 0x0000F438
		public SerializerBuilder WithoutTagMapping(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!this.tagMappings.Remove(type))
			{
				throw new KeyNotFoundException("Tag for type '" + type.FullName + "' is not registered");
			}
			return this;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00011278 File Offset: 0x0000F478
		public SerializerBuilder EnsureRoundtrip()
		{
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new RoundtripObjectGraphTraversalStrategy(typeConverters, typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.settings, this.objectFactory));
			this.WithEventEmitter<TypeAssigningEventEmitter>(([Nullable(1)] IEventEmitter inner) => new TypeAssigningEventEmitter(inner, true, this.tagMappings, this.quoteNecessaryStrings, this.quoteYaml1_1Strings, this.defaultScalarStyle, this.yamlFormatter), delegate(IRegistrationLocationSelectionSyntax<IEventEmitter> loc)
			{
				loc.InsteadOf<TypeAssigningEventEmitter>();
			});
			return base.WithTypeInspector<ReadableAndWritablePropertiesTypeInspector>(([Nullable(1)] ITypeInspector inner) => new ReadableAndWritablePropertiesTypeInspector(inner), delegate(IRegistrationLocationSelectionSyntax<ITypeInspector> loc)
			{
				loc.OnBottom();
			});
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001130D File Offset: 0x0000F50D
		public SerializerBuilder DisableAliases()
		{
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigner));
			this.emissionPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigningObjectGraphVisitor));
			return this;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0001133A File Offset: 0x0000F53A
		[Obsolete("The default behavior is now to always emit default values, thefore calling this method has no effect. This behavior is now controlled by ConfigureDefaultValuesHandling.", true)]
		public SerializerBuilder EmitDefaults()
		{
			return this.ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00011343 File Offset: 0x0000F543
		public SerializerBuilder ConfigureDefaultValuesHandling(DefaultValuesHandling configuration)
		{
			this.defaultValuesHandlingConfiguration = configuration;
			return this;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00011350 File Offset: 0x0000F550
		public SerializerBuilder JsonCompatible()
		{
			this.emitterSettings = this.emitterSettings.WithMaxSimpleKeyLength(int.MaxValue).WithoutAnchorName();
			return base.WithTypeConverter(new GuidConverter(true), delegate(IRegistrationLocationSelectionSyntax<IYamlTypeConverter> w)
			{
				w.InsteadOf<GuidConverter>();
			}).WithTypeConverter(new DateTimeConverter(1, null, true, Array.Empty<string>())).WithTypeConverter(new DateOnlyConverter(null, true, Array.Empty<string>())).WithTypeConverter(new TimeOnlyConverter(null, true, Array.Empty<string>())).WithEventEmitter<JsonEventEmitter>(([Nullable(1)] IEventEmitter inner) => new JsonEventEmitter(inner, this.yamlFormatter), delegate(IRegistrationLocationSelectionSyntax<IEventEmitter> loc)
			{
				loc.InsteadOf<TypeAssigningEventEmitter>();
			});
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00011407 File Offset: 0x0000F607
		public SerializerBuilder WithNewLine(string newLine)
		{
			this.emitterSettings = this.emitterSettings.WithNewLine(newLine);
			return this;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001141C File Offset: 0x0000F61C
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where;
			if ((where = SerializerBuilder.<>c__31<TObjectGraphVisitor>.<>9__31_0) == null)
			{
				where = (SerializerBuilder.<>c__31<TObjectGraphVisitor>.<>9__31_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>>(SerializerBuilder.<>c__31<TObjectGraphVisitor>.<>9.<WithPreProcessingPhaseObjectGraphVisitor>b__31_0));
			}
			return this.WithPreProcessingPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitor, where);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00011444 File Offset: 0x0000F644
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			SerializerBuilder.<>c__DisplayClass32_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass32_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitor = objectGraphVisitor;
			if (CS$<>8__locals1.objectGraphVisitor == null)
			{
				throw new ArgumentNullException("objectGraphVisitor");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.preProcessingPhaseObjectGraphVisitorFactories.CreateRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>(CS$<>8__locals1.<WithPreProcessingPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000114AC File Offset: 0x0000F6AC
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<IObjectGraphVisitor<Nothing>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			SerializerBuilder.<>c__DisplayClass33_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass33_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitorFactory = objectGraphVisitorFactory;
			if (CS$<>8__locals1.objectGraphVisitorFactory == null)
			{
				throw new ArgumentNullException("objectGraphVisitorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.preProcessingPhaseObjectGraphVisitorFactories.CreateTrackingRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IObjectGraphVisitor<Nothing>, IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>(CS$<>8__locals1.<WithPreProcessingPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001150F File Offset: 0x0000F70F
		public SerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			return this.WithoutPreProcessingPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00011521 File Offset: 0x0000F721
		public SerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00011544 File Offset: 0x0000F744
		public SerializerBuilder WithObjectGraphTraversalStrategyFactory(ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory)
		{
			this.objectGraphTraversalStrategyFactory = objectGraphTraversalStrategyFactory;
			return this;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0001154E File Offset: 0x0000F74E
		public SerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where;
			if ((where = SerializerBuilder.<>c__37<TObjectGraphVisitor>.<>9__37_0) == null)
			{
				where = (SerializerBuilder.<>c__37<TObjectGraphVisitor>.<>9__37_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>>(SerializerBuilder.<>c__37<TObjectGraphVisitor>.<>9.<WithEmissionPhaseObjectGraphVisitor>b__37_0));
			}
			return this.WithEmissionPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitorFactory, where);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00011578 File Offset: 0x0000F778
		public SerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			SerializerBuilder.<>c__DisplayClass38_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass38_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitorFactory = objectGraphVisitorFactory;
			if (CS$<>8__locals1.objectGraphVisitorFactory == null)
			{
				throw new ArgumentNullException("objectGraphVisitorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.emissionPhaseObjectGraphVisitorFactories.CreateRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>>(CS$<>8__locals1.<WithEmissionPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000115DC File Offset: 0x0000F7DC
		public SerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			SerializerBuilder.<>c__DisplayClass39_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass39_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitorFactory = objectGraphVisitorFactory;
			if (CS$<>8__locals1.objectGraphVisitorFactory == null)
			{
				throw new ArgumentNullException("objectGraphVisitorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.emissionPhaseObjectGraphVisitorFactories.CreateTrackingRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IObjectGraphVisitor<IEmitter>, EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>>(CS$<>8__locals1.<WithEmissionPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001163F File Offset: 0x0000F83F
		public SerializerBuilder WithoutEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			return this.WithoutEmissionPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00011651 File Offset: 0x0000F851
		public SerializerBuilder WithoutEmissionPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.emissionPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00011674 File Offset: 0x0000F874
		public SerializerBuilder WithIndentedSequences()
		{
			this.emitterSettings = this.emitterSettings.WithIndentedSequences();
			return this;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00011688 File Offset: 0x0000F888
		public ISerializer Build()
		{
			return Serializer.FromValueSerializer(this.BuildValueSerializer(), this.emitterSettings);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001169C File Offset: 0x0000F89C
		public IValueSerializer BuildValueSerializer()
		{
			IEnumerable<IYamlTypeConverter> typeConverters = base.BuildTypeConverters();
			ITypeInspector typeInspector = this.BuildTypeInspector();
			IObjectGraphTraversalStrategy traversalStrategy = this.objectGraphTraversalStrategyFactory(typeInspector, this.typeResolver, typeConverters, this.maximumRecursion);
			IEventEmitter eventEmitter = this.eventEmitterFactories.BuildComponentChain(new WriterEventEmitter());
			return new SerializerBuilder.ValueSerializer(traversalStrategy, eventEmitter, typeConverters, this.preProcessingPhaseObjectGraphVisitorFactories.Clone(), this.emissionPhaseObjectGraphVisitorFactories.Clone());
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00011700 File Offset: 0x0000F900
		internal ITypeInspector BuildTypeInspector()
		{
			ITypeInspector typeInspector = new ReadablePropertiesTypeInspector(this.typeResolver, this.includeNonPublicProperties);
			if (!this.ignoreFields)
			{
				typeInspector = new CompositeTypeInspector(new ITypeInspector[]
				{
					new ReadableFieldsTypeInspector(this.typeResolver),
					typeInspector
				});
			}
			return this.typeInspectorFactories.BuildComponentChain(typeInspector);
		}

		// Token: 0x040001C7 RID: 455
		private ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory;

		// Token: 0x040001C8 RID: 456
		private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

		// Token: 0x040001C9 RID: 457
		private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;

		// Token: 0x040001CA RID: 458
		private readonly LazyComponentRegistrationList<IEventEmitter, IEventEmitter> eventEmitterFactories;

		// Token: 0x040001CB RID: 459
		private readonly IDictionary<Type, TagName> tagMappings = new Dictionary<Type, TagName>();

		// Token: 0x040001CC RID: 460
		private readonly IObjectFactory objectFactory;

		// Token: 0x040001CD RID: 461
		private int maximumRecursion = 50;

		// Token: 0x040001CE RID: 462
		private EmitterSettings emitterSettings = EmitterSettings.Default;

		// Token: 0x040001CF RID: 463
		private DefaultValuesHandling defaultValuesHandlingConfiguration;

		// Token: 0x040001D0 RID: 464
		private ScalarStyle defaultScalarStyle;

		// Token: 0x040001D1 RID: 465
		private bool quoteNecessaryStrings;

		// Token: 0x040001D2 RID: 466
		private bool quoteYaml1_1Strings;

		// Token: 0x020000BD RID: 189
		[Nullable(0)]
		private class ValueSerializer : IValueSerializer
		{
			// Token: 0x06000454 RID: 1108 RVA: 0x00011853 File Offset: 0x0000FA53
			public ValueSerializer(IObjectGraphTraversalStrategy traversalStrategy, IEventEmitter eventEmitter, IEnumerable<IYamlTypeConverter> typeConverters, LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories, LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories)
			{
				this.traversalStrategy = traversalStrategy;
				this.eventEmitter = eventEmitter;
				this.typeConverters = typeConverters;
				this.preProcessingPhaseObjectGraphVisitorFactories = preProcessingPhaseObjectGraphVisitorFactories;
				this.emissionPhaseObjectGraphVisitorFactories = emissionPhaseObjectGraphVisitorFactories;
			}

			// Token: 0x06000455 RID: 1109 RVA: 0x00011880 File Offset: 0x0000FA80
			[NullableContext(2)]
			public void SerializeValue([Nullable(1)] IEmitter emitter, object value, Type type)
			{
				Type type2 = type ?? ((value != null) ? value.GetType() : typeof(object));
				Type staticType = type ?? typeof(object);
				ObjectDescriptor graph = new ObjectDescriptor(value, type2, staticType);
				List<IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitors = this.preProcessingPhaseObjectGraphVisitorFactories.BuildComponentList(this.typeConverters);
				foreach (IObjectGraphVisitor<Nothing> visitor in preProcessingPhaseObjectGraphVisitors)
				{
					this.traversalStrategy.Traverse<Nothing>(graph, visitor, default(Nothing));
				}
				IObjectGraphVisitor<IEmitter> visitor2 = this.emissionPhaseObjectGraphVisitorFactories.BuildComponentChain(new EmittingObjectGraphVisitor(this.eventEmitter), (IObjectGraphVisitor<IEmitter> inner) => new EmissionPhaseObjectGraphVisitorArgs(inner, this.eventEmitter, preProcessingPhaseObjectGraphVisitors, this.typeConverters, new ObjectSerializer(base.<SerializeValue>g__NestedObjectSerializer|0)));
				this.traversalStrategy.Traverse<IEmitter>(graph, visitor2, emitter);
			}

			// Token: 0x040001D3 RID: 467
			private readonly IObjectGraphTraversalStrategy traversalStrategy;

			// Token: 0x040001D4 RID: 468
			private readonly IEventEmitter eventEmitter;

			// Token: 0x040001D5 RID: 469
			private readonly IEnumerable<IYamlTypeConverter> typeConverters;

			// Token: 0x040001D6 RID: 470
			private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

			// Token: 0x040001D7 RID: 471
			private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;
		}
	}
}
