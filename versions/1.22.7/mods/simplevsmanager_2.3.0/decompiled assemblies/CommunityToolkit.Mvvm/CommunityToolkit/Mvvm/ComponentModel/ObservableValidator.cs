using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.ComponentModel
{
	// Token: 0x02000037 RID: 55
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ObservableValidator : ObservableObject, INotifyDataErrorInfo
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600014F RID: 335 RVA: 0x00005EB0 File Offset: 0x000040B0
		// (remove) Token: 0x06000150 RID: 336 RVA: 0x00005EE8 File Offset: 0x000040E8
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		// Token: 0x06000151 RID: 337 RVA: 0x00005F1D File Offset: 0x0000411D
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected ObservableValidator()
		{
			this.validationContext = new ValidationContext(this);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005F3C File Offset: 0x0000413C
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected ObservableValidator([Nullable(new byte[]
		{
			2,
			1,
			2
		})] IDictionary<object, object> items)
		{
			this.validationContext = new ValidationContext(this, items);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005F5C File Offset: 0x0000415C
		[NullableContext(2)]
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected ObservableValidator(IServiceProvider serviceProvider, [Nullable(new byte[]
		{
			2,
			1,
			2
		})] IDictionary<object, object> items)
		{
			this.validationContext = new ValidationContext(this, serviceProvider, items);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005F7D File Offset: 0x0000417D
		protected ObservableValidator(ValidationContext validationContext)
		{
			ArgumentNullException.ThrowIfNull(validationContext, "validationContext");
			this.validationContext = validationContext;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00005FA2 File Offset: 0x000041A2
		[Display(AutoGenerateField = false)]
		public bool HasErrors
		{
			get
			{
				return this.totalErrors > 0;
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005FAD File Offset: 0x000041AD
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool SetProperty<[Nullable(2)] T>([NotNullIfNotNull("newValue")] ref T field, T newValue, bool validate, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			bool flag = base.SetProperty<T>(ref field, newValue, propertyName);
			if (flag && validate)
			{
				this.ValidateProperty(newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005FD8 File Offset: 0x000041D8
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool SetProperty<[Nullable(2)] T>([NotNullIfNotNull("newValue")] ref T field, T newValue, IEqualityComparer<T> comparer, bool validate, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			bool flag = base.SetProperty<T>(ref field, newValue, comparer, propertyName);
			if (flag && validate)
			{
				this.ValidateProperty(newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006010 File Offset: 0x00004210
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool SetProperty<[Nullable(2)] T>(T oldValue, T newValue, Action<T> callback, bool validate, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			bool flag = base.SetProperty<T>(oldValue, newValue, callback, propertyName);
			if (flag && validate)
			{
				this.ValidateProperty(newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006048 File Offset: 0x00004248
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool SetProperty<[Nullable(2)] T>(T oldValue, T newValue, IEqualityComparer<T> comparer, Action<T> callback, bool validate, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			bool flag = base.SetProperty<T>(oldValue, newValue, comparer, callback, propertyName);
			if (flag && validate)
			{
				this.ValidateProperty(newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000609C File Offset: 0x0000429C
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool SetProperty<TModel, [Nullable(2)] T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, bool validate, [CallerMemberName] string propertyName = null) where TModel : class
		{
			ArgumentNullException.ThrowIfNull(model, "model");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			bool flag = base.SetProperty<TModel, T>(oldValue, newValue, model, callback, propertyName);
			if (flag && validate)
			{
				this.ValidateProperty(newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000060F4 File Offset: 0x000042F4
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool SetProperty<TModel, [Nullable(2)] T>(T oldValue, T newValue, IEqualityComparer<T> comparer, TModel model, Action<TModel, T> callback, bool validate, [CallerMemberName] string propertyName = null) where TModel : class
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(model, "model");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			bool flag = base.SetProperty<TModel, T>(oldValue, newValue, comparer, model, callback, propertyName);
			if (flag && validate)
			{
				this.ValidateProperty(newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006158 File Offset: 0x00004358
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool TrySetProperty<[Nullable(2)] T>(ref T field, T newValue, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			return this.TryValidateProperty(newValue, propertyName, out errors) && base.SetProperty<T>(ref field, newValue, propertyName);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006183 File Offset: 0x00004383
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool TrySetProperty<[Nullable(2)] T>(ref T field, T newValue, IEqualityComparer<T> comparer, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			return this.TryValidateProperty(newValue, propertyName, out errors) && base.SetProperty<T>(ref field, newValue, comparer, propertyName);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000061BB File Offset: 0x000043BB
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool TrySetProperty<[Nullable(2)] T>(T oldValue, T newValue, Action<T> callback, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			return this.TryValidateProperty(newValue, propertyName, out errors) && base.SetProperty<T>(oldValue, newValue, callback, propertyName);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000061F4 File Offset: 0x000043F4
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool TrySetProperty<[Nullable(2)] T>(T oldValue, T newValue, IEqualityComparer<T> comparer, Action<T> callback, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			return this.TryValidateProperty(newValue, propertyName, out errors) && base.SetProperty<T>(oldValue, newValue, comparer, callback, propertyName);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006248 File Offset: 0x00004448
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool TrySetProperty<TModel, [Nullable(2)] T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string propertyName = null) where TModel : class
		{
			ArgumentNullException.ThrowIfNull(model, "model");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			return this.TryValidateProperty(newValue, propertyName, out errors) && base.SetProperty<TModel, T>(oldValue, newValue, model, callback, propertyName);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000062A0 File Offset: 0x000044A0
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected bool TrySetProperty<TModel, [Nullable(2)] T>(T oldValue, T newValue, IEqualityComparer<T> comparer, TModel model, Action<TModel, T> callback, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string propertyName = null) where TModel : class
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(model, "model");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			return this.TryValidateProperty(newValue, propertyName, out errors) && base.SetProperty<TModel, T>(oldValue, newValue, comparer, model, callback, propertyName);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006304 File Offset: 0x00004504
		[NullableContext(2)]
		protected void ClearErrors(string propertyName = null)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				this.ClearAllErrors();
				return;
			}
			this.ClearErrorsForProperty(propertyName);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000631C File Offset: 0x0000451C
		public IEnumerable<ValidationResult> GetErrors([Nullable(2)] string propertyName = null)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return this.<GetErrors>g__GetAllErrors|28_0();
			}
			List<ValidationResult> result;
			if (this.errors.TryGetValue(propertyName, ref result))
			{
				return result;
			}
			return Array.Empty<ValidationResult>();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000634F File Offset: 0x0000454F
		IEnumerable INotifyDataErrorInfo.GetErrors([Nullable(2)] string propertyName)
		{
			return this.GetErrors(propertyName);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006358 File Offset: 0x00004558
		[RequiresUnreferencedCode("This method requires the generated CommunityToolkit.Mvvm.ComponentModel.__Internals.__ObservableValidatorExtensions type not to be removed to use the fast path. If this type is removed by the linker, or if the target recipient was created dynamically and was missed by the source generator, a slower fallback path using a compiled LINQ expression will be used. This will have more overhead in the first invocation of this method for any given recipient type. Additionally, due to the usage of validation APIs, the type of the current instance cannot be statically discovered.")]
		protected void ValidateAllProperties()
		{
			ObservableValidator.EntityValidatorMap.GetValue(base.GetType(), (Type t) => ObservableValidator.<ValidateAllProperties>g__GetValidationAction|30_0(t)).Invoke(this);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006390 File Offset: 0x00004590
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		protected internal void ValidateProperty([Nullable(2)] object value, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(propertyName, "propertyName");
			List<ValidationResult> list;
			if (!this.errors.TryGetValue(propertyName, ref list))
			{
				list = new List<ValidationResult>();
				this.errors.Add(propertyName, list);
			}
			bool flag = false;
			if (list.Count > 0)
			{
				list.Clear();
				flag = true;
			}
			this.validationContext.MemberName = propertyName;
			this.validationContext.DisplayName = this.GetDisplayNameForProperty(propertyName);
			bool flag2 = Validator.TryValidateProperty(value, this.validationContext, list);
			if (flag2)
			{
				if (flag)
				{
					this.totalErrors--;
					if (this.totalErrors == 0)
					{
						this.OnPropertyChanged(ObservableValidator.HasErrorsChangedEventArgs);
					}
				}
			}
			else if (!flag)
			{
				this.totalErrors++;
				if (this.totalErrors == 1)
				{
					this.OnPropertyChanged(ObservableValidator.HasErrorsChangedEventArgs);
				}
			}
			if (flag || !flag2)
			{
				EventHandler<DataErrorsChangedEventArgs> errorsChanged = this.ErrorsChanged;
				if (errorsChanged == null)
				{
					return;
				}
				errorsChanged.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006478 File Offset: 0x00004678
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		private bool TryValidateProperty([Nullable(2)] object value, string propertyName, out IReadOnlyCollection<ValidationResult> errors)
		{
			List<ValidationResult> list;
			if (!this.errors.TryGetValue(propertyName, ref list))
			{
				list = new List<ValidationResult>();
				this.errors.Add(propertyName, list);
			}
			bool flag = list.Count > 0;
			List<ValidationResult> list2 = new List<ValidationResult>();
			this.validationContext.MemberName = propertyName;
			this.validationContext.DisplayName = this.GetDisplayNameForProperty(propertyName);
			bool flag2 = Validator.TryValidateProperty(value, this.validationContext, list2);
			if (flag2 && flag)
			{
				list.Clear();
				this.totalErrors--;
				if (this.totalErrors == 0)
				{
					this.OnPropertyChanged(ObservableValidator.HasErrorsChangedEventArgs);
				}
				EventHandler<DataErrorsChangedEventArgs> errorsChanged = this.ErrorsChanged;
				if (errorsChanged != null)
				{
					errorsChanged.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
				}
			}
			errors = list2;
			return flag2;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000652C File Offset: 0x0000472C
		private void ClearAllErrors()
		{
			if (this.totalErrors == 0)
			{
				return;
			}
			foreach (KeyValuePair<string, List<ValidationResult>> keyValuePair in this.errors)
			{
				bool flag = keyValuePair.Value.Count > 0;
				keyValuePair.Value.Clear();
				if (flag)
				{
					EventHandler<DataErrorsChangedEventArgs> errorsChanged = this.ErrorsChanged;
					if (errorsChanged != null)
					{
						errorsChanged.Invoke(this, new DataErrorsChangedEventArgs(keyValuePair.Key));
					}
				}
			}
			this.totalErrors = 0;
			this.OnPropertyChanged(ObservableValidator.HasErrorsChangedEventArgs);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000065D0 File Offset: 0x000047D0
		private void ClearErrorsForProperty(string propertyName)
		{
			List<ValidationResult> list;
			if (!this.errors.TryGetValue(propertyName, ref list) || list.Count == 0)
			{
				return;
			}
			list.Clear();
			this.totalErrors--;
			if (this.totalErrors == 0)
			{
				this.OnPropertyChanged(ObservableValidator.HasErrorsChangedEventArgs);
			}
			EventHandler<DataErrorsChangedEventArgs> errorsChanged = this.ErrorsChanged;
			if (errorsChanged == null)
			{
				return;
			}
			errorsChanged.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006634 File Offset: 0x00004834
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		private string GetDisplayNameForProperty(string propertyName)
		{
			string text;
			ObservableValidator.DisplayNamesMap.GetValue(base.GetType(), (Type t) => ObservableValidator.<GetDisplayNameForProperty>g__GetDisplayNames|35_0(t)).TryGetValue(propertyName, ref text);
			return text ?? propertyName;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000066A4 File Offset: 0x000048A4
		[CompilerGenerated]
		[MethodImpl(8)]
		private IEnumerable<ValidationResult> <GetErrors>g__GetAllErrors|28_0()
		{
			return Enumerable.SelectMany<List<ValidationResult>, ValidationResult>(this.errors.Values, (List<ValidationResult> errors) => errors);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000066D8 File Offset: 0x000048D8
		[CompilerGenerated]
		[RequiresUnreferencedCode("The type of the current instance cannot be statically discovered.")]
		internal static Action<object> <ValidateAllProperties>g__GetValidationAction|30_0(Type type)
		{
			Type type2 = type.Assembly.GetType("CommunityToolkit.Mvvm.ComponentModel.__Internals.__ObservableValidatorExtensions");
			if (type2 != null)
			{
				MethodInfo method = type2.GetMethod("CreateAllPropertiesValidator", new Type[]
				{
					type
				});
				if (method != null)
				{
					return (Action<object>)method.Invoke(null, new object[1]);
				}
			}
			return ObservableValidator.<ValidateAllProperties>g__GetValidationActionFallback|30_1(type);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000672C File Offset: 0x0000492C
		[CompilerGenerated]
		internal static Action<object> <ValidateAllProperties>g__GetValidationActionFallback|30_1(Type type)
		{
			ValueTuple<string, MethodInfo>[] array = Enumerable.ToArray<ValueTuple<string, MethodInfo>>(Enumerable.Select(Enumerable.Where(Enumerable.Select(Enumerable.Where<PropertyInfo>(type.GetProperties(20), (PropertyInfo property) => property.GetIndexParameters().Length == 0 && Enumerable.Any<ValidationAttribute>(CustomAttributeExtensions.GetCustomAttributes<ValidationAttribute>(property, true))), (PropertyInfo property) => new <>f__AnonymousType0<PropertyInfo, MethodInfo>(property, property.GetMethod)), <>h__TransparentIdentifier0 => <>h__TransparentIdentifier0.getMethod != null), <>h__TransparentIdentifier0 => new ValueTuple<string, MethodInfo>(<>h__TransparentIdentifier0.property.Name, <>h__TransparentIdentifier0.getMethod)));
			if (array.Length == 0)
			{
				return delegate(object _)
				{
				};
			}
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
			UnaryExpression inst0 = Expression.Convert(parameterExpression, type);
			MethodInfo validateMethod = typeof(ObservableValidator).GetMethod("ValidateProperty", 36);
			return Expression.Lambda<Action<object>>(Expression.Block(Enumerable.Select<ValueTuple<string, MethodInfo>, MethodCallExpression>(array, ([TupleElementNames(new string[]
			{
				"Name",
				"GetMethod"
			})] ValueTuple<string, MethodInfo> property) => Expression.Call(inst0, validateMethod, new Expression[]
			{
				Expression.Convert(Expression.Call(inst0, property.Item2), typeof(object)),
				Expression.Constant(property.Item1)
			}))), new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006868 File Offset: 0x00004A68
		[CompilerGenerated]
		internal static Dictionary<string, string> <GetDisplayNameForProperty>g__GetDisplayNames|35_0(Type type)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (PropertyInfo propertyInfo in type.GetProperties(20))
			{
				DisplayAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<DisplayAttribute>(propertyInfo);
				if (customAttribute != null)
				{
					string name = customAttribute.GetName();
					if (name != null)
					{
						dictionary.Add(propertyInfo.Name, name);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x04000068 RID: 104
		private static readonly ConditionalWeakTable<Type, Action<object>> EntityValidatorMap = new ConditionalWeakTable<Type, Action<object>>();

		// Token: 0x04000069 RID: 105
		private static readonly ConditionalWeakTable<Type, Dictionary<string, string>> DisplayNamesMap = new ConditionalWeakTable<Type, Dictionary<string, string>>();

		// Token: 0x0400006A RID: 106
		private static readonly PropertyChangedEventArgs HasErrorsChangedEventArgs = new PropertyChangedEventArgs("HasErrors");

		// Token: 0x0400006B RID: 107
		private readonly ValidationContext validationContext;

		// Token: 0x0400006C RID: 108
		private readonly Dictionary<string, List<ValidationResult>> errors = new Dictionary<string, List<ValidationResult>>();

		// Token: 0x0400006D RID: 109
		private int totalErrors;
	}
}
