using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000024 RID: 36
	[NullableContext(1)]
	[Nullable(0)]
	public class DeleteGameProfilesDialog : Window, IComponentConnector
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x00020EFC File Offset: 0x0001F0FC
		public DeleteGameProfilesDialog(IEnumerable<string> profileNames, string activeProfileName)
		{
			if (profileNames == null)
			{
				throw new ArgumentNullException("profileNames");
			}
			this.InitializeComponent();
			foreach (string text in profileNames)
			{
				bool isDefault = string.Equals(text, "Default", 5);
				bool isActive = string.Equals(text, activeProfileName, 5);
				DeleteGameProfilesDialog.GameProfileDeletionOption gameProfileDeletionOption = new DeleteGameProfilesDialog.GameProfileDeletionOption(text, isActive, isDefault);
				gameProfileDeletionOption.PropertyChanged += new PropertyChangedEventHandler(this.OnProfilePropertyChanged);
				this.Profiles.Add(gameProfileDeletionOption);
			}
			this.UpdateDeleteButtonState();
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00020FA4 File Offset: 0x0001F1A4
		public ObservableCollection<DeleteGameProfilesDialog.GameProfileDeletionOption> Profiles { get; } = new ObservableCollection<DeleteGameProfilesDialog.GameProfileDeletionOption>();

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00020FAC File Offset: 0x0001F1AC
		public IReadOnlyList<string> SelectedProfileNames
		{
			get
			{
				return Enumerable.ToArray<string>(Enumerable.Select<DeleteGameProfilesDialog.GameProfileDeletionOption, string>(Enumerable.Where<DeleteGameProfilesDialog.GameProfileDeletionOption>(this.Profiles, (DeleteGameProfilesDialog.GameProfileDeletionOption profile) => profile.IsSelectable && profile.IsSelected), (DeleteGameProfilesDialog.GameProfileDeletionOption profile) => profile.Name));
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0002100C File Offset: 0x0001F20C
		private void OnProfilePropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsSelected")
			{
				this.UpdateDeleteButtonState();
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00021026 File Offset: 0x0001F226
		private void UpdateDeleteButtonState()
		{
			if (this.DeleteButton == null)
			{
				return;
			}
			this.DeleteButton.IsEnabled = Enumerable.Any<DeleteGameProfilesDialog.GameProfileDeletionOption>(this.Profiles, (DeleteGameProfilesDialog.GameProfileDeletionOption profile) => profile.IsSelectable && profile.IsSelected);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00021066 File Offset: 0x0001F266
		private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00021074 File Offset: 0x0001F274
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/deletegameprofilesdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000210A4 File Offset: 0x0001F2A4
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				this.ProfilesItemsControl = (ItemsControl)target;
				return;
			}
			if (connectionId != 2)
			{
				this._contentLoaded = true;
				return;
			}
			this.DeleteButton = (Button)target;
			this.DeleteButton.Click += new RoutedEventHandler(this.DeleteButton_OnClick);
		}

		// Token: 0x0400024C RID: 588
		[Nullable(0)]
		internal ItemsControl ProfilesItemsControl;

		// Token: 0x0400024D RID: 589
		[Nullable(0)]
		internal Button DeleteButton;

		// Token: 0x0400024E RID: 590
		private bool _contentLoaded;

		// Token: 0x020001D0 RID: 464
		[Nullable(0)]
		public sealed class GameProfileDeletionOption : INotifyPropertyChanged
		{
			// Token: 0x06001383 RID: 4995 RVA: 0x00072244 File Offset: 0x00070444
			public GameProfileDeletionOption(string name, bool isActive, bool isDefault)
			{
				this.Name = name;
				this.IsActive = isActive;
				this.IsDefault = isDefault;
			}

			// Token: 0x17000440 RID: 1088
			// (get) Token: 0x06001384 RID: 4996 RVA: 0x00072261 File Offset: 0x00070461
			public string Name { get; }

			// Token: 0x17000441 RID: 1089
			// (get) Token: 0x06001385 RID: 4997 RVA: 0x00072269 File Offset: 0x00070469
			public bool IsActive { get; }

			// Token: 0x17000442 RID: 1090
			// (get) Token: 0x06001386 RID: 4998 RVA: 0x00072271 File Offset: 0x00070471
			public bool IsDefault { get; }

			// Token: 0x17000443 RID: 1091
			// (get) Token: 0x06001387 RID: 4999 RVA: 0x00072279 File Offset: 0x00070479
			public bool IsSelectable
			{
				get
				{
					return !this.IsDefault;
				}
			}

			// Token: 0x17000444 RID: 1092
			// (get) Token: 0x06001388 RID: 5000 RVA: 0x00072284 File Offset: 0x00070484
			// (set) Token: 0x06001389 RID: 5001 RVA: 0x0007228C File Offset: 0x0007048C
			public bool IsSelected
			{
				get
				{
					return this._isSelected;
				}
				set
				{
					bool flag = this.IsSelectable && value;
					if (this._isSelected == flag)
					{
						return;
					}
					this._isSelected = flag;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged == null)
					{
						return;
					}
					propertyChanged.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
				}
			}

			// Token: 0x1400000D RID: 13
			// (add) Token: 0x0600138A RID: 5002 RVA: 0x000722D0 File Offset: 0x000704D0
			// (remove) Token: 0x0600138B RID: 5003 RVA: 0x00072308 File Offset: 0x00070508
			[Nullable(2)]
			[method: NullableContext(2)]
			[Nullable(2)]
			public event PropertyChangedEventHandler PropertyChanged;

			// Token: 0x04000C44 RID: 3140
			private bool _isSelected;
		}
	}
}
