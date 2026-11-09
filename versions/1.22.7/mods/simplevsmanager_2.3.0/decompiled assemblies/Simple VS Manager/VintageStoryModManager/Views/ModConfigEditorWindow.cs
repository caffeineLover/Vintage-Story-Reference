using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Win32;
using VintageStoryModManager.Services;
using VintageStoryModManager.ViewModels;
using YamlDotNet.Core;

namespace VintageStoryModManager.Views
{
	// Token: 0x02000018 RID: 24
	[NullableContext(1)]
	[Nullable(0)]
	public class ModConfigEditorWindow : Window, IComponentConnector, IStyleConnector
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x0001D139 File Offset: 0x0001B339
		public ModConfigEditorWindow(ModConfigEditorViewModel viewModel)
		{
			this.InitializeComponent();
			if (viewModel == null)
			{
				throw new ArgumentNullException("viewModel");
			}
			base.DataContext = viewModel;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001D15D File Offset: 0x0001B35D
		private void Window_OnLoaded(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(6, new Action(this.EnsureFilePathDoesNotOverlapButtons));
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001D178 File Offset: 0x0001B378
		private void EnsureFilePathDoesNotOverlapButtons()
		{
			if (this.FilePathTextBlock == null || this.ActionButtonsPanel == null)
			{
				return;
			}
			base.UpdateLayout();
			for (int i = 0; i < 3; i++)
			{
				Rect elementBounds = this.GetElementBounds(this.FilePathTextBlock);
				Rect elementBounds2 = this.GetElementBounds(this.ActionButtonsPanel);
				if (elementBounds.Right <= elementBounds2.Left)
				{
					break;
				}
				double num = elementBounds.Right - elementBounds2.Left;
				if (num <= 0.0)
				{
					break;
				}
				double num2 = num + 24.0;
				base.Width = Math.Max(base.Width + num2, base.MinWidth);
				base.UpdateLayout();
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0001D21B File Offset: 0x0001B41B
		private Rect GetElementBounds(FrameworkElement element)
		{
			if (!element.IsLoaded)
			{
				element.UpdateLayout();
			}
			return element.TransformToAncestor(this).TransformBounds(new Rect(element.RenderSize));
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001D244 File Offset: 0x0001B444
		private void SaveButton_OnClick(object sender, RoutedEventArgs e)
		{
			ModConfigEditorViewModel modConfigEditorViewModel = base.DataContext as ModConfigEditorViewModel;
			if (modConfigEditorViewModel == null)
			{
				return;
			}
			try
			{
				modConfigEditorViewModel.Save();
				base.DialogResult = new bool?(true);
			}
			catch (Exception ex) when (ex is InvalidOperationException || ex is IOException || ex is UnauthorizedAccessException || ex is JsonException || ex is YamlException)
			{
				ModManagerMessageBox.Show(this, "Failed to save the configuration:\n" + ex.Message, "Edit Config", 0, 16, null, null, null);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001D2EC File Offset: 0x0001B4EC
		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001D2FC File Offset: 0x0001B4FC
		private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
		{
			ModConfigEditorViewModel modConfigEditorViewModel = base.DataContext as ModConfigEditorViewModel;
			if (modConfigEditorViewModel == null)
			{
				return;
			}
			if (!modConfigEditorViewModel.HasConfigurations)
			{
				this.AddButton_OnClick(sender, e);
				return;
			}
			string initialDirectory = ModConfigEditorWindow.GetInitialDirectory(modConfigEditorViewModel.FilePath);
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = "Select configuration file",
				Filter = "Config files (*.json;*.yaml;*.yml)|*.json;*.yaml;*.yml|All files (*.*)|*.*",
				CheckFileExists = true,
				Multiselect = false
			};
			if (!string.IsNullOrWhiteSpace(initialDirectory) && Directory.Exists(initialDirectory))
			{
				openFileDialog.InitialDirectory = initialDirectory;
			}
			try
			{
				string fileName = Path.GetFileName(modConfigEditorViewModel.FilePath);
				if (!string.IsNullOrWhiteSpace(fileName))
				{
					openFileDialog.FileName = fileName;
				}
			}
			catch (Exception)
			{
			}
			if (!openFileDialog.ShowDialog(this).GetValueOrDefault())
			{
				return;
			}
			try
			{
				modConfigEditorViewModel.ReplaceConfigurationFile(openFileDialog.FileName);
				base.Dispatcher.BeginInvoke(6, new Action(this.EnsureFilePathDoesNotOverlapButtons));
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is JsonException || ex is YamlException || ex is ArgumentException || ex is NotSupportedException)
			{
				ModManagerMessageBox.Show(this, "Failed to open the configuration file:\n" + ex.Message, "Edit Config", 0, 16, null, null, null);
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001D46C File Offset: 0x0001B66C
		private void AddButton_OnClick(object sender, RoutedEventArgs e)
		{
			ModConfigEditorViewModel modConfigEditorViewModel = base.DataContext as ModConfigEditorViewModel;
			if (modConfigEditorViewModel == null)
			{
				return;
			}
			string initialDirectory = ModConfigEditorWindow.GetInitialDirectory(modConfigEditorViewModel.FilePath);
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = "Add configuration file",
				Filter = "Config files (*.json;*.yaml;*.yml)|*.json;*.yaml;*.yml|All files (*.*)|*.*",
				CheckFileExists = true,
				Multiselect = false
			};
			if (!string.IsNullOrWhiteSpace(initialDirectory) && Directory.Exists(initialDirectory))
			{
				openFileDialog.InitialDirectory = initialDirectory;
			}
			if (!openFileDialog.ShowDialog(this).GetValueOrDefault())
			{
				return;
			}
			try
			{
				modConfigEditorViewModel.AddConfiguration(openFileDialog.FileName, true);
				base.Dispatcher.BeginInvoke(6, new Action(this.EnsureFilePathDoesNotOverlapButtons));
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is JsonException || ex is YamlException || ex is ArgumentException || ex is NotSupportedException)
			{
				ModManagerMessageBox.Show(this, "Failed to add the configuration file:\n" + ex.Message, "Edit Config", 0, 16, null, null, null);
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001D59C File Offset: 0x0001B79C
		private void ClearButton_OnClick(object sender, RoutedEventArgs e)
		{
			ModConfigEditorViewModel modConfigEditorViewModel = base.DataContext as ModConfigEditorViewModel;
			if (modConfigEditorViewModel == null)
			{
				return;
			}
			if (modConfigEditorViewModel.RemoveSelectedConfiguration())
			{
				base.Dispatcher.BeginInvoke(6, new Action(this.EnsureFilePathDoesNotOverlapButtons));
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001D5DC File Offset: 0x0001B7DC
		private void TreeView_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject == null)
			{
				return;
			}
			ScrollViewer scrollViewer = ModConfigEditorWindow.FindAncestorScrollViewer(dependencyObject);
			if (scrollViewer == null)
			{
				return;
			}
			if (e.Delta > 0)
			{
				scrollViewer.LineUp();
			}
			else if (e.Delta < 0)
			{
				scrollViewer.LineDown();
			}
			e.Handled = true;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001D630 File Offset: 0x0001B830
		[NullableContext(2)]
		private static ScrollViewer FindAncestorScrollViewer(DependencyObject current)
		{
			while (current != null)
			{
				ScrollViewer scrollViewer = current as ScrollViewer;
				if (scrollViewer != null)
				{
					return scrollViewer;
				}
				current = VisualTreeHelper.GetParent(current);
			}
			return null;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001D658 File Offset: 0x0001B858
		private void TreeViewItem_OnExpanded(object sender, RoutedEventArgs e)
		{
			TreeViewItem item = sender as TreeViewItem;
			if (item == null)
			{
				return;
			}
			if (!(item.DataContext is ModConfigArrayNodeViewModel))
			{
				return;
			}
			base.Dispatcher.BeginInvoke(4, delegate()
			{
				this.ExpandArrayChildren(item);
			});
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001D6B4 File Offset: 0x0001B8B4
		private void TreeViewItem_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem treeViewItem = sender as TreeViewItem;
			if (treeViewItem == null)
			{
				return;
			}
			if (!(treeViewItem.DataContext is ModConfigArrayNodeViewModel))
			{
				return;
			}
			DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
			if (dependencyObject != null)
			{
				if (ModConfigEditorWindow.FindAncestor<ToggleButton>(dependencyObject) != null)
				{
					return;
				}
				if (ModConfigEditorWindow.FindAncestor<TextBoxBase>(dependencyObject) != null)
				{
					return;
				}
			}
			treeViewItem.IsSelected = true;
			treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
			e.Handled = true;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001D718 File Offset: 0x0001B918
		private void ExpandArrayChildren(TreeViewItem arrayItem)
		{
			ModConfigEditorWindow.<>c__DisplayClass13_0 CS$<>8__locals1 = new ModConfigEditorWindow.<>c__DisplayClass13_0();
			CS$<>8__locals1.arrayItem = arrayItem;
			CS$<>8__locals1.<>4__this = this;
			if (CS$<>8__locals1.arrayItem.ItemContainerGenerator.Status != 2)
			{
				CS$<>8__locals1.arrayItem.UpdateLayout();
				if (CS$<>8__locals1.arrayItem.ItemContainerGenerator.Status != 2)
				{
					CS$<>8__locals1.arrayItem.ItemContainerGenerator.StatusChanged += new EventHandler(CS$<>8__locals1.<ExpandArrayChildren>g__OnStatusChanged|0);
					return;
				}
			}
			foreach (object obj in CS$<>8__locals1.arrayItem.Items)
			{
				TreeViewItem treeViewItem = CS$<>8__locals1.arrayItem.ItemContainerGenerator.ContainerFromItem(obj) as TreeViewItem;
				if (treeViewItem != null && treeViewItem.DataContext is ModConfigContainerNodeViewModel)
				{
					treeViewItem.IsExpanded = true;
					this.ExpandArrayChildren(treeViewItem);
				}
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001D804 File Offset: 0x0001BA04
		[return: Nullable(2)]
		private static T FindAncestor<[Nullable(0)] T>(DependencyObject current) where T : DependencyObject
		{
			while (current != null)
			{
				T t = current as T;
				if (t != null)
				{
					return t;
				}
				DependencyObject parent = VisualTreeHelper.GetParent(current);
				if (parent == null)
				{
					break;
				}
				current = parent;
			}
			return default(T);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001D844 File Offset: 0x0001BA44
		[NullableContext(2)]
		private static string GetInitialDirectory(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return null;
			}
			string result;
			try
			{
				string directoryName = Path.GetDirectoryName(filePath);
				if (string.IsNullOrWhiteSpace(directoryName))
				{
					result = null;
				}
				else
				{
					string text = directoryName;
					while (!string.IsNullOrWhiteSpace(text))
					{
						if (string.Equals(Path.GetFileName(text.TrimEnd(new char[]
						{
							Path.DirectorySeparatorChar,
							Path.AltDirectorySeparatorChar
						})), "ModConfig", 5))
						{
							return text;
						}
						string directoryName2 = Path.GetDirectoryName(text);
						if (string.IsNullOrWhiteSpace(directoryName2) || string.Equals(directoryName2, text, 5))
						{
							break;
						}
						text = directoryName2;
					}
					result = directoryName;
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001D8E4 File Offset: 0x0001BAE4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/modconfigeditorwindow.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001D914 File Offset: 0x0001BB14
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((ModConfigEditorWindow)target).Loaded += new RoutedEventHandler(this.Window_OnLoaded);
				return;
			case 4:
				this.ActionButtonsPanel = (StackPanel)target;
				return;
			case 5:
				((Button)target).Click += new RoutedEventHandler(this.AddButton_OnClick);
				return;
			case 6:
				((Button)target).Click += new RoutedEventHandler(this.ClearButton_OnClick);
				return;
			case 7:
				((Button)target).Click += new RoutedEventHandler(this.BrowseButton_OnClick);
				return;
			case 8:
				((Button)target).Click += new RoutedEventHandler(this.SaveButton_OnClick);
				return;
			case 9:
				((Button)target).Click += new RoutedEventHandler(this.CancelButton_OnClick);
				return;
			case 10:
				this.FilePathTextBlock = (TextBlock)target;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001DA08 File Offset: 0x0001BC08
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 2)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = TreeViewItem.ExpandedEvent;
				eventSetter.Handler = new RoutedEventHandler(this.TreeViewItem_OnExpanded);
				((Style)target).Setters.Add(eventSetter);
				eventSetter = new EventSetter();
				eventSetter.Event = UIElement.PreviewMouseLeftButtonDownEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.TreeViewItem_OnPreviewMouseLeftButtonDown);
				((Style)target).Setters.Add(eventSetter);
				return;
			}
			if (connectionId != 3)
			{
				return;
			}
			((TreeView)target).PreviewMouseWheel += new MouseWheelEventHandler(this.TreeView_OnPreviewMouseWheel);
		}

		// Token: 0x040001DE RID: 478
		[Nullable(0)]
		internal StackPanel ActionButtonsPanel;

		// Token: 0x040001DF RID: 479
		[Nullable(0)]
		internal TextBlock FilePathTextBlock;

		// Token: 0x040001E0 RID: 480
		private bool _contentLoaded;
	}
}
