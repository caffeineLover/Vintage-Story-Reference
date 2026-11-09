using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VintageStoryModManager.Views
{
	// Token: 0x02000014 RID: 20
	[NullableContext(1)]
	[Nullable(0)]
	public class BulkUpdateChangelogWindow : Window, IComponentConnector
	{
		// Token: 0x06000124 RID: 292 RVA: 0x00006090 File Offset: 0x00004290
		public BulkUpdateChangelogWindow(IEnumerable<BulkUpdateChangelogWindow.BulkUpdateChangelogItem> items)
		{
			this.InitializeComponent();
			List<BulkUpdateChangelogWindow.BulkUpdateChangelogItem> list = ((items != null) ? Enumerable.ToList<BulkUpdateChangelogWindow.BulkUpdateChangelogItem>(items) : null) ?? new List<BulkUpdateChangelogWindow.BulkUpdateChangelogItem>();
			this.ChangelogItems = new ReadOnlyCollection<BulkUpdateChangelogWindow.BulkUpdateChangelogItem>(list);
			base.DataContext = this;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000060D2 File Offset: 0x000042D2
		public IReadOnlyList<BulkUpdateChangelogWindow.BulkUpdateChangelogItem> ChangelogItems { get; }

		// Token: 0x06000126 RID: 294 RVA: 0x000060DA File Offset: 0x000042DA
		private void CloseButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
			base.Close();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000060F0 File Offset: 0x000042F0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/bulkupdatechangelogwindow.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006120 File Offset: 0x00004320
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				((Button)target).Click += new RoutedEventHandler(this.CloseButton_OnClick);
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x040000CE RID: 206
		private bool _contentLoaded;

		// Token: 0x02000119 RID: 281
		[Nullable(0)]
		public sealed class BulkUpdateChangelogItem : IEquatable<BulkUpdateChangelogWindow.BulkUpdateChangelogItem>
		{
			// Token: 0x06001155 RID: 4437 RVA: 0x0005E531 File Offset: 0x0005C731
			public BulkUpdateChangelogItem(string Title, string Changelog)
			{
				this.Title = Title;
				this.Changelog = Changelog;
				base..ctor();
			}

			// Token: 0x1700041E RID: 1054
			// (get) Token: 0x06001156 RID: 4438 RVA: 0x0005E547 File Offset: 0x0005C747
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(BulkUpdateChangelogWindow.BulkUpdateChangelogItem);
				}
			}

			// Token: 0x1700041F RID: 1055
			// (get) Token: 0x06001157 RID: 4439 RVA: 0x0005E553 File Offset: 0x0005C753
			// (set) Token: 0x06001158 RID: 4440 RVA: 0x0005E55B File Offset: 0x0005C75B
			public string Title { get; set; }

			// Token: 0x17000420 RID: 1056
			// (get) Token: 0x06001159 RID: 4441 RVA: 0x0005E564 File Offset: 0x0005C764
			// (set) Token: 0x0600115A RID: 4442 RVA: 0x0005E56C File Offset: 0x0005C76C
			public string Changelog { get; set; }

			// Token: 0x0600115B RID: 4443 RVA: 0x0005E578 File Offset: 0x0005C778
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("BulkUpdateChangelogItem");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600115C RID: 4444 RVA: 0x0005E5C4 File Offset: 0x0005C7C4
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Title = ");
				builder.Append(this.Title);
				builder.Append(", Changelog = ");
				builder.Append(this.Changelog);
				return true;
			}

			// Token: 0x0600115D RID: 4445 RVA: 0x0005E5FE File Offset: 0x0005C7FE
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(BulkUpdateChangelogWindow.BulkUpdateChangelogItem left, BulkUpdateChangelogWindow.BulkUpdateChangelogItem right)
			{
				return !(left == right);
			}

			// Token: 0x0600115E RID: 4446 RVA: 0x0005E60A File Offset: 0x0005C80A
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(BulkUpdateChangelogWindow.BulkUpdateChangelogItem left, BulkUpdateChangelogWindow.BulkUpdateChangelogItem right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600115F RID: 4447 RVA: 0x0005E61E File Offset: 0x0005C81E
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Title>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Changelog>k__BackingField);
			}

			// Token: 0x06001160 RID: 4448 RVA: 0x0005E65E File Offset: 0x0005C85E
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as BulkUpdateChangelogWindow.BulkUpdateChangelogItem);
			}

			// Token: 0x06001161 RID: 4449 RVA: 0x0005E66C File Offset: 0x0005C86C
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(BulkUpdateChangelogWindow.BulkUpdateChangelogItem other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Title>k__BackingField, other.<Title>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Changelog>k__BackingField, other.<Changelog>k__BackingField));
			}

			// Token: 0x06001163 RID: 4451 RVA: 0x0005E6CD File Offset: 0x0005C8CD
			[CompilerGenerated]
			private BulkUpdateChangelogItem(BulkUpdateChangelogWindow.BulkUpdateChangelogItem original)
			{
				this.Title = original.<Title>k__BackingField;
				this.Changelog = original.<Changelog>k__BackingField;
			}

			// Token: 0x06001164 RID: 4452 RVA: 0x0005E6ED File Offset: 0x0005C8ED
			[CompilerGenerated]
			public void Deconstruct(out string Title, out string Changelog)
			{
				Title = this.Title;
				Changelog = this.Changelog;
			}
		}
	}
}
