using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000025 RID: 37
	[NullableContext(1)]
	[Nullable(0)]
	public class ExperimentalModDebugDialog : Window, INotifyPropertyChanged, IComponentConnector
	{
		// Token: 0x06000450 RID: 1104 RVA: 0x000210F3 File Offset: 0x0001F2F3
		public ExperimentalModDebugDialog(string modId, IReadOnlyList<ExperimentalModDebugLogLine> logLines) : this("Log entries mentioning '" + modId + "'", "No log entries referencing '" + modId + "' were found.", logLines)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(modId, "modId");
			this.ModId = modId;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00021130 File Offset: 0x0001F330
		public ExperimentalModDebugDialog(string headerText, string emptyMessage, IReadOnlyList<ExperimentalModDebugLogLine> logLines)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(headerText, "headerText");
			ArgumentException.ThrowIfNullOrWhiteSpace(emptyMessage, "emptyMessage");
			ArgumentNullException.ThrowIfNull(logLines, "logLines");
			this.ModId = null;
			this.InitializeComponent();
			this.HeaderText = headerText;
			this.EmptyMessage = emptyMessage;
			this.LogLines = logLines;
			this._filteredLogLines = CollectionViewSource.GetDefaultView(this.LogLines);
			this._filteredLogLines.Filter = new Predicate<object>(this.FilterLogLine);
			base.DataContext = this;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x000211B5 File Offset: 0x0001F3B5
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x000211BD File Offset: 0x0001F3BD
		[Nullable(2)]
		public string ModId { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x000211C6 File Offset: 0x0001F3C6
		public IReadOnlyList<ExperimentalModDebugLogLine> LogLines { get; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x000211CE File Offset: 0x0001F3CE
		public ICollectionView FilteredLogLines
		{
			get
			{
				return this._filteredLogLines ?? CollectionViewSource.GetDefaultView(this.LogLines);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x000211E5 File Offset: 0x0001F3E5
		public string HeaderText { get; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x000211ED File Offset: 0x0001F3ED
		public string EmptyMessage { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x000211F5 File Offset: 0x0001F3F5
		public bool HasLogLines
		{
			get
			{
				return this.LogLines.Count > 0;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00021205 File Offset: 0x0001F405
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x0002120D File Offset: 0x0001F40D
		public bool ShowOnlyHighlighted
		{
			get
			{
				return this._showOnlyHighlighted;
			}
			set
			{
				if (this._showOnlyHighlighted != value)
				{
					this._showOnlyHighlighted = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged != null)
					{
						propertyChanged.Invoke(this, new PropertyChangedEventArgs("ShowOnlyHighlighted"));
					}
					ICollectionView filteredLogLines = this._filteredLogLines;
					if (filteredLogLines == null)
					{
						return;
					}
					filteredLogLines.Refresh();
				}
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600045B RID: 1115 RVA: 0x0002124C File Offset: 0x0001F44C
		// (remove) Token: 0x0600045C RID: 1116 RVA: 0x00021284 File Offset: 0x0001F484
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600045D RID: 1117 RVA: 0x000212BC File Offset: 0x0001F4BC
		private bool FilterLogLine(object obj)
		{
			ExperimentalModDebugLogLine experimentalModDebugLogLine = obj as ExperimentalModDebugLogLine;
			return experimentalModDebugLogLine == null || !this.ShowOnlyHighlighted || experimentalModDebugLogLine.IsHighlighted;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000212E5 File Offset: 0x0001F4E5
		private void CloseButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000212F4 File Offset: 0x0001F4F4
		private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DataGrid dataGrid = sender as DataGrid;
			if (dataGrid == null)
			{
				return;
			}
			ExperimentalModDebugLogLine experimentalModDebugLogLine = dataGrid.SelectedItem as ExperimentalModDebugLogLine;
			if (experimentalModDebugLogLine == null)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(experimentalModDebugLogLine.FilePath) || experimentalModDebugLogLine.LineNumber <= 0)
			{
				return;
			}
			if (!File.Exists(experimentalModDebugLogLine.FilePath))
			{
				MessageBox.Show("The log file could not be found:\n\n" + experimentalModDebugLogLine.FilePath, "File Not Found", 0, 48);
				return;
			}
			ExperimentalModDebugDialog.OpenFileAtLine(experimentalModDebugLogLine.FilePath, experimentalModDebugLogLine.LineNumber);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00021370 File Offset: 0x0001F570
		private static void OpenFileAtLine(string filePath, int lineNumber)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return;
			}
			try
			{
				filePath = Path.GetFullPath(filePath);
			}
			catch (Exception)
			{
				return;
			}
			List<ValueTuple<string, string>> list = new List<ValueTuple<string, string>>();
			string text = "code";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(6, 2);
			defaultInterpolatedStringHandler.AppendLiteral("-g \"");
			defaultInterpolatedStringHandler.AppendFormatted(filePath);
			defaultInterpolatedStringHandler.AppendLiteral("\":");
			defaultInterpolatedStringHandler.AppendFormatted<int>(lineNumber);
			list.Add(new ValueTuple<string, string>(text, defaultInterpolatedStringHandler.ToStringAndClear()));
			string text2 = "notepad++";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(5, 2);
			defaultInterpolatedStringHandler2.AppendLiteral("\"");
			defaultInterpolatedStringHandler2.AppendFormatted(filePath);
			defaultInterpolatedStringHandler2.AppendLiteral("\" -n");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(lineNumber);
			list.Add(new ValueTuple<string, string>(text2, defaultInterpolatedStringHandler2.ToStringAndClear()));
			string text3 = "subl";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
			defaultInterpolatedStringHandler3..ctor(3, 2);
			defaultInterpolatedStringHandler3.AppendLiteral("\"");
			defaultInterpolatedStringHandler3.AppendFormatted(filePath);
			defaultInterpolatedStringHandler3.AppendLiteral("\":");
			defaultInterpolatedStringHandler3.AppendFormatted<int>(lineNumber);
			list.Add(new ValueTuple<string, string>(text3, defaultInterpolatedStringHandler3.ToStringAndClear()));
			string text4 = "gvim";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
			defaultInterpolatedStringHandler4..ctor(4, 2);
			defaultInterpolatedStringHandler4.AppendLiteral("+");
			defaultInterpolatedStringHandler4.AppendFormatted<int>(lineNumber);
			defaultInterpolatedStringHandler4.AppendLiteral(" \"");
			defaultInterpolatedStringHandler4.AppendFormatted(filePath);
			defaultInterpolatedStringHandler4.AppendLiteral("\"");
			list.Add(new ValueTuple<string, string>(text4, defaultInterpolatedStringHandler4.ToStringAndClear()));
			list.Add(new ValueTuple<string, string>(string.Empty, "\"" + filePath + "\""));
			foreach (ValueTuple<string, string> valueTuple in list)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				try
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo();
					if (string.IsNullOrEmpty(item))
					{
						processStartInfo.FileName = filePath;
						processStartInfo.UseShellExecute = true;
					}
					else
					{
						processStartInfo.FileName = item;
						processStartInfo.Arguments = item2;
						processStartInfo.UseShellExecute = false;
					}
					Process.Start(processStartInfo);
					break;
				}
				catch (Win32Exception)
				{
				}
				catch (FileNotFoundException)
				{
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000215B0 File Offset: 0x0001F7B0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/experimentalmoddebugdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000215E0 File Offset: 0x0001F7E0
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				((DataGrid)target).MouseDoubleClick += new MouseButtonEventHandler(this.DataGrid_OnMouseDoubleClick);
				return;
			}
			if (connectionId != 2)
			{
				this._contentLoaded = true;
				return;
			}
			((Button)target).Click += new RoutedEventHandler(this.CloseButton_OnClick);
		}

		// Token: 0x0400024F RID: 591
		[Nullable(2)]
		private readonly ICollectionView _filteredLogLines;

		// Token: 0x04000250 RID: 592
		private bool _showOnlyHighlighted;

		// Token: 0x04000256 RID: 598
		private bool _contentLoaded;
	}
}
