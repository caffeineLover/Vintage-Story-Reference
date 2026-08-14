using System;
using System.Collections.Generic;
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
	// Token: 0x020000DC RID: 220
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	public sealed class StaticSerializerBuilder : StaticBuilderSkeleton<StaticSerializerBuilder>
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x00013154 File Offset: 0x00011354
		public StaticSerializerBuilder(StaticContext context) : base(new DynamicTypeResolver())
		{
			this.context = context;
			this.factory = context.GetFactory();
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
			LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> lazyComponentRegistrationList = new LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>();
			lazyComponentRegistrationList.Add(typeof(AnchorAssigner), (IEnumerable<IYamlTypeConverter> typeConverters) => new AnchorAssigner(typeConverters));
			this.preProcessingPhaseObjectGraphVisitorFactories = lazyComponentRegistrationList;
			LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> lazyComponentRegistrationList2 = new LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>>();
			lazyComponentRegistrationList2.Add(typeof(CustomSerializationObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new CustomSerializationObjectGraphVisitor(args.InnerVisitor, args.TypeConverters, args.NestedObjectSerializer));
			lazyComponentRegistrationList2.Add(typeof(AnchorAssigningObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new AnchorAssigningObjectGraphVisitor(args.InnerVisitor, args.EventEmitter, args.GetPreProcessingPhaseObjectGraphVisitor<AnchorAssigner>()));
			lazyComponentRegistrationList2.Add(typeof(DefaultValuesObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new DefaultValuesObjectGraphVisitor(this.defaultValuesHandlingConfiguration, args.InnerVisitor, this.factory));
			lazyComponentRegistrationList2.Add(typeof(CommentsObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new CommentsObjectGraphVisitor(args.InnerVisitor));
			this.emissionPhaseObjectGraphVisitorFactories = lazyComponentRegistrationList2;
			this.eventEmitterFactories = new LazyComponentRegistrationList<IEventEmitter, IEventEmitter>
			{
				{
					typeof(TypeAssigningEventEmitter),
					(IEventEmitter inner) => new TypeAssigningEventEmitter(inner, false, this.tagMappings, this.quoteNecessaryStrings, this.quoteYaml1_1Strings, this.defaultScalarStyle, this.yamlFormatter)
				}
			};
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new FullObjectGraphTraversalStrategy(typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.factory));
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		protected override StaticSerializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001334D File Offset: 0x0001154D
		public StaticSerializerBuilder WithQuotingNecessaryStrings(bool quoteYaml1_1Strings = false)
		{
			this.quoteNecessaryStrings = true;
			this.quoteYaml1_1Strings = quoteYaml1_1Strings;
			return this;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001335E File Offset: 0x0001155E
		public StaticSerializerBuilder WithQuotingNecessaryStrings()
		{
			this.quoteNecessaryStrings = true;
			return this;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00013368 File Offset: 0x00011568
		public StaticSerializerBuilder WithDefaultScalarStyle(ScalarStyle style)
		{
			this.defaultScalarStyle = style;
			return this;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00013374 File Offset: 0x00011574
		public StaticSerializerBuilder WithMaximumRecursion(int maximumRecursion)
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

		// Token: 0x0600051A RID: 1306 RVA: 0x000133C9 File Offset: 0x000115C9
		public StaticSerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory) where TEventEmitter : IEventEmitter
		{
			Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where;
			if ((where = StaticSerializerBuilder.<>c__20<TEventEmitter>.<>9__20_0) == null)
			{
				where = (StaticSerializerBuilder.<>c__20<TEventEmitter>.<>9__20_0 = new Action<IRegistrationLocationSelectionSyntax<IEventEmitter>>(StaticSerializerBuilder.<>c__20<TEventEmitter>.<>9.<WithEventEmitter>b__20_0));
			}
			return this.WithEventEmitter<TEventEmitter>(eventEmitterFactory, where);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000133F4 File Offset: 0x000115F4
		public StaticSerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory, Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			StaticSerializerBuilder.<>c__DisplayClass21_0<TEventEmitter> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass21_0<TEventEmitter>();
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

		// Token: 0x0600051C RID: 1308 RVA: 0x0001345C File Offset: 0x0001165C
		public StaticSerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(WrapperFactory<IEventEmitter, IEventEmitter, TEventEmitter> eventEmitterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			StaticSerializerBuilder.<>c__DisplayClass22_0<TEventEmitter> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass22_0<TEventEmitter>();
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

		// Token: 0x0600051D RID: 1309 RVA: 0x000134C4 File Offset: 0x000116C4
		public StaticSerializerBuilder WithoutEventEmitter<[Nullable(0)] TEventEmitter>() where TEventEmitter : IEventEmitter
		{
			return this.WithoutEventEmitter(typeof(TEventEmitter));
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000134D6 File Offset: 0x000116D6
		public StaticSerializerBuilder WithoutEventEmitter(Type eventEmitterType)
		{
			if (eventEmitterType == null)
			{
				throw new ArgumentNullException("eventEmitterType");
			}
			this.eventEmitterFactories.Remove(eventEmitterType);
			return this;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000134FC File Offset: 0x000116FC
		public override StaticSerializerBuilder WithTagMapping(TagName tag, Type type)
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

		// Token: 0x06000520 RID: 1312 RVA: 0x000135A4 File Offset: 0x000117A4
		public StaticSerializerBuilder WithoutTagMapping(Type type)
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

		// Token: 0x06000521 RID: 1313 RVA: 0x000135E4 File Offset: 0x000117E4
		public StaticSerializerBuilder EnsureRoundtrip()
		{
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new RoundtripObjectGraphTraversalStrategy(typeConverters, typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.settings, this.factory));
			this.WithEventEmitter<TypeAssigningEventEmitter>(([Nullable(1)] IEventEmitter inner) => new TypeAssigningEventEmitter(inner, true, this.tagMappings, this.quoteNecessaryStrings), delegate(IRegistrationLocationSelectionSyntax<IEventEmitter> loc)
			{
				loc.InsteadOf<TypeAssigningEventEmitter>();
			});
			return base.WithTypeInspector<ReadableAndWritablePropertiesTypeInspector>(([Nullable(1)] ITypeInspector inner) => new ReadableAndWritablePropertiesTypeInspector(inner), delegate(IRegistrationLocationSelectionSyntax<ITypeInspector> loc)
			{
				loc.OnBottom();
			});
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00013679 File Offset: 0x00011879
		public StaticSerializerBuilder DisableAliases()
		{
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigner));
			this.emissionPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigningObjectGraphVisitor));
			return this;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000136A6 File Offset: 0x000118A6
		[Obsolete("The default behavior is now to always emit default values, thefore calling this method has no effect. This behavior is now controlled by ConfigureDefaultValuesHandling.", true)]
		public StaticSerializerBuilder EmitDefaults()
		{
			return this.ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000136AF File Offset: 0x000118AF
		public StaticSerializerBuilder ConfigureDefaultValuesHandling(DefaultValuesHandling configuration)
		{
			this.defaultValuesHandlingConfiguration = configuration;
			return this;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000136BC File Offset: 0x000118BC
		public StaticSerializerBuilder JsonCompatible()
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

		// Token: 0x06000526 RID: 1318 RVA: 0x00013773 File Offset: 0x00011973
		public StaticSerializerBuilder WithNewLine(string newLine)
		{
			this.emitterSettings = this.emitterSettings.WithNewLine(newLine);
			return this;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00013788 File Offset: 0x00011988
		public StaticSerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where;
			if ((where = StaticSerializerBuilder.<>c__33<TObjectGraphVisitor>.<>9__33_0) == null)
			{
				where = (StaticSerializerBuilder.<>c__33<TObjectGraphVisitor>.<>9__33_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>>(StaticSerializerBuilder.<>c__33<TObjectGraphVisitor>.<>9.<WithPreProcessingPhaseObjectGraphVisitor>b__33_0));
			}
			return this.WithPreProcessingPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitor, where);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x000137B0 File Offset: 0x000119B0
		public StaticSerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			StaticSerializerBuilder.<>c__DisplayClass34_0<TObjectGraphVisitor> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass34_0<TObjectGraphVisitor>();
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

		// Token: 0x06000529 RID: 1321 RVA: 0x00013818 File Offset: 0x00011A18
		public StaticSerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<IObjectGraphVisitor<Nothing>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			StaticSerializerBuilder.<>c__DisplayClass35_0<TObjectGraphVisitor> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass35_0<TObjectGraphVisitor>();
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

		// Token: 0x0600052A RID: 1322 RVA: 0x0001387B File Offset: 0x00011A7B
		public StaticSerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			return this.WithoutPreProcessingPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001388D File Offset: 0x00011A8D
		public StaticSerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000138B0 File Offset: 0x00011AB0
		public StaticSerializerBuilder WithObjectGraphTraversalStrategyFactory(ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory)
		{
			this.objectGraphTraversalStrategyFactory = objectGraphTraversalStrategyFactory;
			return this;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000138BA File Offset: 0x00011ABA
		public StaticSerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where;
			if ((where = StaticSerializerBuilder.<>c__39<TObjectGraphVisitor>.<>9__39_0) == null)
			{
				where = (StaticSerializerBuilder.<>c__39<TObjectGraphVisitor>.<>9__39_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>>(StaticSerializerBuilder.<>c__39<TObjectGraphVisitor>.<>9.<WithEmissionPhaseObjectGraphVisitor>b__39_0));
			}
			return this.WithEmissionPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitorFactory, where);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000138E4 File Offset: 0x00011AE4
		public StaticSerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			StaticSerializerBuilder.<>c__DisplayClass40_0<TObjectGraphVisitor> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass40_0<TObjectGraphVisitor>();
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

		// Token: 0x0600052F RID: 1327 RVA: 0x00013948 File Offset: 0x00011B48
		public StaticSerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			StaticSerializerBuilder.<>c__DisplayClass41_0<TObjectGraphVisitor> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass41_0<TObjectGraphVisitor>();
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

		// Token: 0x06000530 RID: 1328 RVA: 0x000139AB File Offset: 0x00011BAB
		public StaticSerializerBuilder WithoutEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			return this.WithoutEmissionPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000139BD File Offset: 0x00011BBD
		public StaticSerializerBuilder WithoutEmissionPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.emissionPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000139E0 File Offset: 0x00011BE0
		public StaticSerializerBuilder WithIndentedSequences()
		{
			this.emitterSettings = this.emitterSettings.WithIndentedSequences();
			return this;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000139F4 File Offset: 0x00011BF4
		public ISerializer Build()
		{
			return Serializer.FromValueSerializer(this.BuildValueSerializer(), this.emitterSettings);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00013A08 File Offset: 0x00011C08
		public IValueSerializer BuildValueSerializer()
		{
			IEnumerable<IYamlTypeConverter> typeConverters = base.BuildTypeConverters();
			ITypeInspector typeInspector = this.BuildTypeInspector();
			IObjectGraphTraversalStrategy traversalStrategy = this.objectGraphTraversalStrategyFactory(typeInspector, this.typeResolver, typeConverters, this.maximumRecursion);
			IEventEmitter eventEmitter = this.eventEmitterFactories.BuildComponentChain(new WriterEventEmitter());
			return new StaticSerializerBuilder.ValueSerializer(traversalStrategy, eventEmitter, typeConverters, this.preProcessingPhaseObjectGraphVisitorFactories.Clone(), this.emissionPhaseObjectGraphVisitorFactories.Clone());
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00013A6C File Offset: 0x00011C6C
		internal ITypeInspector BuildTypeInspector()
		{
			ITypeInspector typeInspector = this.context.GetTypeInspector();
			return this.typeInspectorFactories.BuildComponentChain(typeInspector);
		}

		// Token: 0x0400024B RID: 587
		private readonly StaticContext context;

		// Token: 0x0400024C RID: 588
		private readonly StaticObjectFactory factory;

		// Token: 0x0400024D RID: 589
		private ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory;

		// Token: 0x0400024E RID: 590
		private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

		// Token: 0x0400024F RID: 591
		private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;

		// Token: 0x04000250 RID: 592
		private readonly LazyComponentRegistrationList<IEventEmitter, IEventEmitter> eventEmitterFactories;

		// Token: 0x04000251 RID: 593
		private readonly IDictionary<Type, TagName> tagMappings = new Dictionary<Type, TagName>();

		// Token: 0x04000252 RID: 594
		private int maximumRecursion = 50;

		// Token: 0x04000253 RID: 595
		private EmitterSettings emitterSettings = EmitterSettings.Default;

		// Token: 0x04000254 RID: 596
		private DefaultValuesHandling defaultValuesHandlingConfiguration;

		// Token: 0x04000255 RID: 597
		private bool quoteNecessaryStrings;

		// Token: 0x04000256 RID: 598
		private bool quoteYaml1_1Strings;

		// Token: 0x04000257 RID: 599
		private ScalarStyle defaultScalarStyle;

		// Token: 0x020000DD RID: 221
		[Nullable(0)]
		private class ValueSerializer : IValueSerializer
		{
			// Token: 0x0600053D RID: 1341 RVA: 0x00013B56 File Offset: 0x00011D56
			public ValueSerializer(IObjectGraphTraversalStrategy traversalStrategy, IEventEmitter eventEmitter, IEnumerable<IYamlTypeConverter> typeConverters, LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories, LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories)
			{
				this.traversalStrategy = traversalStrategy;
				this.eventEmitter = eventEmitter;
				this.typeConverters = typeConverters;
				this.preProcessingPhaseObjectGraphVisitorFactories = preProcessingPhaseObjectGraphVisitorFactories;
				this.emissionPhaseObjectGraphVisitorFactories = emissionPhaseObjectGraphVisitorFactories;
			}

			// Token: 0x0600053E RID: 1342 RVA: 0x00013B84 File Offset: 0x00011D84
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

			// Token: 0x04000258 RID: 600
			private readonly IObjectGraphTraversalStrategy traversalStrategy;

			// Token: 0x04000259 RID: 601
			private readonly IEventEmitter eventEmitter;

			// Token: 0x0400025A RID: 602
			private readonly IEnumerable<IYamlTypeConverter> typeConverters;

			// Token: 0x0400025B RID: 603
			private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

			// Token: 0x0400025C RID: 604
			private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;
		}
	}
}
