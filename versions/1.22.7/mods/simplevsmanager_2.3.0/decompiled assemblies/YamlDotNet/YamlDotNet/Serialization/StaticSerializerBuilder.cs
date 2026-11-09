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
	// Token: 0x0200003C RID: 60
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	public sealed class StaticSerializerBuilder : StaticBuilderSkeleton<StaticSerializerBuilder>
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x0000593C File Offset: 0x00003B3C
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
					(IEventEmitter inner) => new TypeAssigningEventEmitter(inner, this.tagMappings, this.quoteNecessaryStrings, this.quoteYaml1_1Strings, this.defaultScalarStyle, this.yamlFormatter, this.enumNamingConvention, this.BuildTypeInspector())
				}
			};
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new FullObjectGraphTraversalStrategy(typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.factory));
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00005B3B File Offset: 0x00003D3B
		protected override StaticSerializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00005B3E File Offset: 0x00003D3E
		public StaticSerializerBuilder WithQuotingNecessaryStrings(bool quoteYaml1_1Strings = false)
		{
			this.quoteNecessaryStrings = true;
			this.quoteYaml1_1Strings = quoteYaml1_1Strings;
			return this;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00005B4F File Offset: 0x00003D4F
		public StaticSerializerBuilder WithQuotingNecessaryStrings()
		{
			this.quoteNecessaryStrings = true;
			return this;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00005B59 File Offset: 0x00003D59
		public StaticSerializerBuilder WithDefaultScalarStyle(ScalarStyle style)
		{
			this.defaultScalarStyle = style;
			return this;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005B64 File Offset: 0x00003D64
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

		// Token: 0x060001AC RID: 428 RVA: 0x00005BB9 File Offset: 0x00003DB9
		public StaticSerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory) where TEventEmitter : IEventEmitter
		{
			Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where;
			if ((where = StaticSerializerBuilder.<>c__20<TEventEmitter>.<>9__20_0) == null)
			{
				where = (StaticSerializerBuilder.<>c__20<TEventEmitter>.<>9__20_0 = new Action<IRegistrationLocationSelectionSyntax<IEventEmitter>>(StaticSerializerBuilder.<>c__20<TEventEmitter>.<>9.<WithEventEmitter>b__20_0));
			}
			return this.WithEventEmitter<TEventEmitter>(eventEmitterFactory, where);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00005BE1 File Offset: 0x00003DE1
		public StaticSerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, ITypeInspector, TEventEmitter> eventEmitterFactory) where TEventEmitter : IEventEmitter
		{
			Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where;
			if ((where = StaticSerializerBuilder.<>c__21<TEventEmitter>.<>9__21_0) == null)
			{
				where = (StaticSerializerBuilder.<>c__21<TEventEmitter>.<>9__21_0 = new Action<IRegistrationLocationSelectionSyntax<IEventEmitter>>(StaticSerializerBuilder.<>c__21<TEventEmitter>.<>9.<WithEventEmitter>b__21_0));
			}
			return this.WithEventEmitter<TEventEmitter>(eventEmitterFactory, where);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00005C0C File Offset: 0x00003E0C
		public StaticSerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory, Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			StaticSerializerBuilder.<>c__DisplayClass22_0<TEventEmitter> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass22_0<TEventEmitter>();
			CS$<>8__locals1.eventEmitterFactory = eventEmitterFactory;
			return this.WithEventEmitter<TEventEmitter>(new Func<IEventEmitter, ITypeInspector, TEventEmitter>(CS$<>8__locals1.<WithEventEmitter>b__0), where);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00005C3C File Offset: 0x00003E3C
		public StaticSerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, ITypeInspector, TEventEmitter> eventEmitterFactory, Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			StaticSerializerBuilder.<>c__DisplayClass23_0<TEventEmitter> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass23_0<TEventEmitter>();
			CS$<>8__locals1.eventEmitterFactory = eventEmitterFactory;
			CS$<>8__locals1.<>4__this = this;
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

		// Token: 0x060001B0 RID: 432 RVA: 0x00005CAC File Offset: 0x00003EAC
		public StaticSerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(WrapperFactory<IEventEmitter, IEventEmitter, TEventEmitter> eventEmitterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			StaticSerializerBuilder.<>c__DisplayClass24_0<TEventEmitter> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass24_0<TEventEmitter>();
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

		// Token: 0x060001B1 RID: 433 RVA: 0x00005D14 File Offset: 0x00003F14
		public StaticSerializerBuilder WithoutEventEmitter<[Nullable(0)] TEventEmitter>() where TEventEmitter : IEventEmitter
		{
			return this.WithoutEventEmitter(typeof(TEventEmitter));
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005D26 File Offset: 0x00003F26
		public StaticSerializerBuilder WithoutEventEmitter(Type eventEmitterType)
		{
			if (eventEmitterType == null)
			{
				throw new ArgumentNullException("eventEmitterType");
			}
			this.eventEmitterFactories.Remove(eventEmitterType);
			return this;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00005D4C File Offset: 0x00003F4C
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

		// Token: 0x060001B4 RID: 436 RVA: 0x00005DF4 File Offset: 0x00003FF4
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

		// Token: 0x060001B5 RID: 437 RVA: 0x00005E34 File Offset: 0x00004034
		public StaticSerializerBuilder EnsureRoundtrip()
		{
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new RoundtripObjectGraphTraversalStrategy(typeConverters, typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.settings, this.factory));
			this.WithEventEmitter<TypeAssigningEventEmitter>((IEventEmitter inner) => new TypeAssigningEventEmitter(inner, this.tagMappings, this.quoteNecessaryStrings, false, ScalarStyle.Plain, YamlFormatter.Default, this.enumNamingConvention, this.BuildTypeInspector()), delegate(IRegistrationLocationSelectionSyntax<IEventEmitter> loc)
			{
				loc.InsteadOf<TypeAssigningEventEmitter>();
			});
			return base.WithTypeInspector<ReadableAndWritablePropertiesTypeInspector>((ITypeInspector inner) => new ReadableAndWritablePropertiesTypeInspector(inner), delegate(IRegistrationLocationSelectionSyntax<ITypeInspector> loc)
			{
				loc.OnBottom();
			});
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00005EC9 File Offset: 0x000040C9
		public StaticSerializerBuilder DisableAliases()
		{
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigner));
			this.emissionPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigningObjectGraphVisitor));
			return this;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00005EF6 File Offset: 0x000040F6
		[Obsolete("The default behavior is now to always emit default values, thefore calling this method has no effect. This behavior is now controlled by ConfigureDefaultValuesHandling.", true)]
		public StaticSerializerBuilder EmitDefaults()
		{
			return this.ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005EFF File Offset: 0x000040FF
		public StaticSerializerBuilder ConfigureDefaultValuesHandling(DefaultValuesHandling configuration)
		{
			this.defaultValuesHandlingConfiguration = configuration;
			return this;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005F0C File Offset: 0x0000410C
		public StaticSerializerBuilder JsonCompatible()
		{
			this.emitterSettings = this.emitterSettings.WithMaxSimpleKeyLength(int.MaxValue).WithoutAnchorName().WithUtf16SurrogatePairs();
			return base.WithTypeConverter(new GuidConverter(true), delegate(IRegistrationLocationSelectionSyntax<IYamlTypeConverter> w)
			{
				w.InsteadOf<GuidConverter>();
			}).WithTypeConverter(new DateTime8601Converter(ScalarStyle.DoubleQuoted)).WithTypeConverter(new DateOnlyConverter(null, true, Array.Empty<string>())).WithTypeConverter(new TimeOnlyConverter(null, true, Array.Empty<string>())).WithEventEmitter<JsonEventEmitter>((IEventEmitter inner) => new JsonEventEmitter(inner, this.yamlFormatter, this.enumNamingConvention, this.BuildTypeInspector()), delegate(IRegistrationLocationSelectionSyntax<IEventEmitter> loc)
			{
				loc.InsteadOf<TypeAssigningEventEmitter>();
			});
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00005FC1 File Offset: 0x000041C1
		public StaticSerializerBuilder WithNewLine(string newLine)
		{
			this.emitterSettings = this.emitterSettings.WithNewLine(newLine);
			return this;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00005FD6 File Offset: 0x000041D6
		public StaticSerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where;
			if ((where = StaticSerializerBuilder.<>c__35<TObjectGraphVisitor>.<>9__35_0) == null)
			{
				where = (StaticSerializerBuilder.<>c__35<TObjectGraphVisitor>.<>9__35_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>>(StaticSerializerBuilder.<>c__35<TObjectGraphVisitor>.<>9.<WithPreProcessingPhaseObjectGraphVisitor>b__35_0));
			}
			return this.WithPreProcessingPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitor, where);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00005FFE File Offset: 0x000041FE
		public StaticSerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<IEnumerable<IYamlTypeConverter>, TObjectGraphVisitor> objectGraphVisitorFactory) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where;
			if ((where = StaticSerializerBuilder.<>c__36<TObjectGraphVisitor>.<>9__36_0) == null)
			{
				where = (StaticSerializerBuilder.<>c__36<TObjectGraphVisitor>.<>9__36_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>>(StaticSerializerBuilder.<>c__36<TObjectGraphVisitor>.<>9.<WithPreProcessingPhaseObjectGraphVisitor>b__36_0));
			}
			return this.WithPreProcessingPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitorFactory, where);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00006028 File Offset: 0x00004228
		public StaticSerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			StaticSerializerBuilder.<>c__DisplayClass37_0<TObjectGraphVisitor> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass37_0<TObjectGraphVisitor>();
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

		// Token: 0x060001BE RID: 446 RVA: 0x00006090 File Offset: 0x00004290
		public StaticSerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<IEnumerable<IYamlTypeConverter>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			StaticSerializerBuilder.<>c__DisplayClass38_0<TObjectGraphVisitor> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass38_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitorFactory = objectGraphVisitorFactory;
			if (CS$<>8__locals1.objectGraphVisitorFactory == null)
			{
				throw new ArgumentNullException("objectGraphVisitorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.preProcessingPhaseObjectGraphVisitorFactories.CreateRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>(CS$<>8__locals1.<WithPreProcessingPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000060F4 File Offset: 0x000042F4
		public StaticSerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<IObjectGraphVisitor<Nothing>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			StaticSerializerBuilder.<>c__DisplayClass39_0<TObjectGraphVisitor> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass39_0<TObjectGraphVisitor>();
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

		// Token: 0x060001C0 RID: 448 RVA: 0x00006158 File Offset: 0x00004358
		public StaticSerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
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
			where.Invoke(this.preProcessingPhaseObjectGraphVisitorFactories.CreateTrackingRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IObjectGraphVisitor<Nothing>, IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>(CS$<>8__locals1.<WithPreProcessingPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000061BB File Offset: 0x000043BB
		public StaticSerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			return this.WithoutPreProcessingPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000061CD File Offset: 0x000043CD
		public StaticSerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000061F0 File Offset: 0x000043F0
		public StaticSerializerBuilder WithObjectGraphTraversalStrategyFactory(ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory)
		{
			this.objectGraphTraversalStrategyFactory = objectGraphTraversalStrategyFactory;
			return this;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000061FA File Offset: 0x000043FA
		public StaticSerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where;
			if ((where = StaticSerializerBuilder.<>c__44<TObjectGraphVisitor>.<>9__44_0) == null)
			{
				where = (StaticSerializerBuilder.<>c__44<TObjectGraphVisitor>.<>9__44_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>>(StaticSerializerBuilder.<>c__44<TObjectGraphVisitor>.<>9.<WithEmissionPhaseObjectGraphVisitor>b__44_0));
			}
			return this.WithEmissionPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitorFactory, where);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00006224 File Offset: 0x00004424
		public StaticSerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			StaticSerializerBuilder.<>c__DisplayClass45_0<TObjectGraphVisitor> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass45_0<TObjectGraphVisitor>();
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

		// Token: 0x060001C6 RID: 454 RVA: 0x00006288 File Offset: 0x00004488
		public StaticSerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			StaticSerializerBuilder.<>c__DisplayClass46_0<TObjectGraphVisitor> CS$<>8__locals1 = new StaticSerializerBuilder.<>c__DisplayClass46_0<TObjectGraphVisitor>();
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

		// Token: 0x060001C7 RID: 455 RVA: 0x000062EB File Offset: 0x000044EB
		public StaticSerializerBuilder WithoutEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			return this.WithoutEmissionPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000062FD File Offset: 0x000044FD
		public StaticSerializerBuilder WithoutEmissionPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.emissionPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00006320 File Offset: 0x00004520
		public StaticSerializerBuilder WithIndentedSequences()
		{
			this.emitterSettings = this.emitterSettings.WithIndentedSequences();
			return this;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00006334 File Offset: 0x00004534
		public ISerializer Build()
		{
			return Serializer.FromValueSerializer(this.BuildValueSerializer(), this.emitterSettings);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00006348 File Offset: 0x00004548
		public IValueSerializer BuildValueSerializer()
		{
			IEnumerable<IYamlTypeConverter> typeConverters = base.BuildTypeConverters();
			ITypeInspector typeInspector = this.BuildTypeInspector();
			IObjectGraphTraversalStrategy traversalStrategy = this.objectGraphTraversalStrategyFactory(typeInspector, this.typeResolver, typeConverters, this.maximumRecursion);
			IEventEmitter eventEmitter = this.eventEmitterFactories.BuildComponentChain(new WriterEventEmitter());
			return new StaticSerializerBuilder.ValueSerializer(traversalStrategy, eventEmitter, typeConverters, this.preProcessingPhaseObjectGraphVisitorFactories.Clone(), this.emissionPhaseObjectGraphVisitorFactories.Clone());
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000063AC File Offset: 0x000045AC
		public ITypeInspector BuildTypeInspector()
		{
			ITypeInspector typeInspector = this.context.GetTypeInspector();
			return this.typeInspectorFactories.BuildComponentChain(typeInspector);
		}

		// Token: 0x0400005E RID: 94
		private readonly StaticContext context;

		// Token: 0x0400005F RID: 95
		private readonly StaticObjectFactory factory;

		// Token: 0x04000060 RID: 96
		private ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory;

		// Token: 0x04000061 RID: 97
		private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

		// Token: 0x04000062 RID: 98
		private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;

		// Token: 0x04000063 RID: 99
		private readonly LazyComponentRegistrationList<IEventEmitter, IEventEmitter> eventEmitterFactories;

		// Token: 0x04000064 RID: 100
		private readonly Dictionary<Type, TagName> tagMappings = new Dictionary<Type, TagName>();

		// Token: 0x04000065 RID: 101
		private int maximumRecursion = 50;

		// Token: 0x04000066 RID: 102
		private EmitterSettings emitterSettings = EmitterSettings.Default;

		// Token: 0x04000067 RID: 103
		private DefaultValuesHandling defaultValuesHandlingConfiguration;

		// Token: 0x04000068 RID: 104
		private bool quoteNecessaryStrings;

		// Token: 0x04000069 RID: 105
		private bool quoteYaml1_1Strings;

		// Token: 0x0400006A RID: 106
		private ScalarStyle defaultScalarStyle;

		// Token: 0x0200014A RID: 330
		[Nullable(0)]
		private class ValueSerializer : IValueSerializer
		{
			// Token: 0x06000764 RID: 1892 RVA: 0x00018847 File Offset: 0x00016A47
			public ValueSerializer(IObjectGraphTraversalStrategy traversalStrategy, IEventEmitter eventEmitter, IEnumerable<IYamlTypeConverter> typeConverters, LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories, LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories)
			{
				this.traversalStrategy = traversalStrategy;
				this.eventEmitter = eventEmitter;
				this.typeConverters = typeConverters;
				this.preProcessingPhaseObjectGraphVisitorFactories = preProcessingPhaseObjectGraphVisitorFactories;
				this.emissionPhaseObjectGraphVisitorFactories = emissionPhaseObjectGraphVisitorFactories;
			}

			// Token: 0x06000765 RID: 1893 RVA: 0x00018874 File Offset: 0x00016A74
			[NullableContext(2)]
			public void SerializeValue([Nullable(1)] IEmitter emitter, object value, Type type)
			{
				StaticSerializerBuilder.ValueSerializer.<>c__DisplayClass6_0 CS$<>8__locals1 = new StaticSerializerBuilder.ValueSerializer.<>c__DisplayClass6_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.emitter = emitter;
				Type type2 = type ?? ((value != null) ? value.GetType() : typeof(object));
				Type staticType = type ?? typeof(object);
				ObjectDescriptor graph = new ObjectDescriptor(value, type2, staticType);
				CS$<>8__locals1.preProcessingPhaseObjectGraphVisitors = this.preProcessingPhaseObjectGraphVisitorFactories.BuildComponentList(this.typeConverters);
				foreach (IObjectGraphVisitor<Nothing> visitor in CS$<>8__locals1.preProcessingPhaseObjectGraphVisitors)
				{
					this.traversalStrategy.Traverse<Nothing>(graph, visitor, default(Nothing), new ObjectSerializer(CS$<>8__locals1.<SerializeValue>g__NestedObjectSerializer|0));
				}
				IObjectGraphVisitor<IEmitter> visitor2 = this.emissionPhaseObjectGraphVisitorFactories.BuildComponentChain(new EmittingObjectGraphVisitor(this.eventEmitter), (IObjectGraphVisitor<IEmitter> inner) => new EmissionPhaseObjectGraphVisitorArgs(inner, CS$<>8__locals1.<>4__this.eventEmitter, CS$<>8__locals1.preProcessingPhaseObjectGraphVisitors, CS$<>8__locals1.<>4__this.typeConverters, new ObjectSerializer(base.<SerializeValue>g__NestedObjectSerializer|0)));
				this.traversalStrategy.Traverse<IEmitter>(graph, visitor2, CS$<>8__locals1.emitter, new ObjectSerializer(CS$<>8__locals1.<SerializeValue>g__NestedObjectSerializer|0));
			}

			// Token: 0x04000282 RID: 642
			private readonly IObjectGraphTraversalStrategy traversalStrategy;

			// Token: 0x04000283 RID: 643
			private readonly IEventEmitter eventEmitter;

			// Token: 0x04000284 RID: 644
			private readonly IEnumerable<IYamlTypeConverter> typeConverters;

			// Token: 0x04000285 RID: 645
			private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

			// Token: 0x04000286 RID: 646
			private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;
		}
	}
}
