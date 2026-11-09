using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CommunityToolkit.Mvvm.ComponentModel
{
	// Token: 0x02000036 RID: 54
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ObservableRecipient : ObservableObject
	{
		// Token: 0x06000141 RID: 321 RVA: 0x00005CBA File Offset: 0x00003EBA
		protected ObservableRecipient() : this(WeakReferenceMessenger.Default)
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005CC7 File Offset: 0x00003EC7
		protected ObservableRecipient(IMessenger messenger)
		{
			ArgumentNullException.ThrowIfNull(messenger, "messenger");
			this.Messenger = messenger;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00005CE1 File Offset: 0x00003EE1
		protected IMessenger Messenger { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00005CE9 File Offset: 0x00003EE9
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00005CF1 File Offset: 0x00003EF1
		public bool IsActive
		{
			get
			{
				return this.isActive;
			}
			[RequiresUnreferencedCode("When this property is set to true, the OnActivated() method will be invoked, which will register all necessary message handlers for this recipient. This method requires the generated CommunityToolkit.Mvvm.Messaging.__Internals.__IMessengerExtensions type not to be removed to use the fast path. If this type is removed by the linker, or if the target recipient was created dynamically and was missed by the source generator, a slower fallback path using a compiled LINQ expression will be used. This will have more overhead in the first invocation of this method for any given recipient type. Alternatively, OnActivated() can be manually overwritten, and registration can be done individually for each required message for this recipient.")]
			[RequiresDynamicCode("When this property is set to true, the OnActivated() method will be invoked, which will register all necessary message handlers for this recipient. This method requires the generated CommunityToolkit.Mvvm.Messaging.__Internals.__IMessengerExtensions type not to be removed to use the fast path. If that is present, the method is AOT safe, as the only methods being invoked to register the messages will be the ones produced by the source generator. If it isn't, this method will need to dynamically create the generic methods to register messages, which might not be available at runtime. Alternatively, OnActivated() can be manually overwritten, and registration can be done individually for each required message for this recipient.")]
			set
			{
				if (this.SetProperty<bool>(ref this.isActive, value, true, "IsActive"))
				{
					if (value)
					{
						this.OnActivated();
						return;
					}
					this.OnDeactivated();
				}
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005D18 File Offset: 0x00003F18
		[RequiresUnreferencedCode("This method requires the generated CommunityToolkit.Mvvm.Messaging.__Internals.__IMessengerExtensions type not to be removed to use the fast path. If this type is removed by the linker, or if the target recipient was created dynamically and was missed by the source generator, a slower fallback path using a compiled LINQ expression will be used. This will have more overhead in the first invocation of this method for any given recipient type. Alternatively, OnActivated() can be manually overwritten, and registration can be done individually for each required message for this recipient.")]
		[RequiresDynamicCode("This method requires the generated CommunityToolkit.Mvvm.Messaging.__Internals.__IMessengerExtensions type not to be removed to use the fast path. If that is present, the method is AOT safe, as the only methods being invoked to register the messages will be the ones produced by the source generator. If it isn't, this method will need to dynamically create the generic methods to register messages, which might not be available at runtime. Alternatively, OnActivated() can be manually overwritten, and registration can be done individually for each required message for this recipient.")]
		protected virtual void OnActivated()
		{
			this.Messenger.RegisterAll(this);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005D26 File Offset: 0x00003F26
		protected virtual void OnDeactivated()
		{
			this.Messenger.UnregisterAll(this);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005D34 File Offset: 0x00003F34
		protected virtual void Broadcast<[Nullable(2)] T>(T oldValue, T newValue, [Nullable(2)] string propertyName)
		{
			PropertyChangedMessage<T> message = new PropertyChangedMessage<T>(this, propertyName, oldValue, newValue);
			this.Messenger.Send(message);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005D58 File Offset: 0x00003F58
		protected bool SetProperty<[Nullable(2)] T>([NotNullIfNotNull("newValue")] ref T field, T newValue, bool broadcast, [Nullable(2)] [CallerMemberName] string propertyName = null)
		{
			T oldValue = field;
			bool flag = base.SetProperty<T>(ref field, newValue, propertyName);
			if (flag && broadcast)
			{
				this.Broadcast<T>(oldValue, newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005D88 File Offset: 0x00003F88
		protected bool SetProperty<[Nullable(2)] T>([NotNullIfNotNull("newValue")] ref T field, T newValue, IEqualityComparer<T> comparer, bool broadcast, [Nullable(2)] [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			T oldValue = field;
			bool flag = base.SetProperty<T>(ref field, newValue, comparer, propertyName);
			if (flag && broadcast)
			{
				this.Broadcast<T>(oldValue, newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005DC2 File Offset: 0x00003FC2
		protected bool SetProperty<[Nullable(2)] T>(T oldValue, T newValue, Action<T> callback, bool broadcast, [Nullable(2)] [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(callback, "callback");
			bool flag = base.SetProperty<T>(oldValue, newValue, callback, propertyName);
			if (flag && broadcast)
			{
				this.Broadcast<T>(oldValue, newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005DEA File Offset: 0x00003FEA
		protected bool SetProperty<[Nullable(2)] T>(T oldValue, T newValue, IEqualityComparer<T> comparer, Action<T> callback, bool broadcast, [Nullable(2)] [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			bool flag = base.SetProperty<T>(oldValue, newValue, comparer, callback, propertyName);
			if (flag && broadcast)
			{
				this.Broadcast<T>(oldValue, newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005E20 File Offset: 0x00004020
		protected bool SetProperty<TModel, [Nullable(2)] T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, bool broadcast, [Nullable(2)] [CallerMemberName] string propertyName = null) where TModel : class
		{
			ArgumentNullException.ThrowIfNull(model, "model");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			bool flag = base.SetProperty<TModel, T>(oldValue, newValue, model, callback, propertyName);
			if (flag && broadcast)
			{
				this.Broadcast<T>(oldValue, newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005E5C File Offset: 0x0000405C
		protected bool SetProperty<TModel, [Nullable(2)] T>(T oldValue, T newValue, IEqualityComparer<T> comparer, TModel model, Action<TModel, T> callback, bool broadcast, [Nullable(2)] [CallerMemberName] string propertyName = null) where TModel : class
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(model, "model");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			bool flag = base.SetProperty<TModel, T>(oldValue, newValue, comparer, model, callback, propertyName);
			if (flag && broadcast)
			{
				this.Broadcast<T>(oldValue, newValue, propertyName);
			}
			return flag;
		}

		// Token: 0x04000067 RID: 103
		private bool isActive;
	}
}
