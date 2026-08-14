using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Table
{
	// Token: 0x02000106 RID: 262
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Table : Element, IStateful, IContentDirectionAware, ISemanticAware
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00015B35 File Offset: 0x00013D35
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x00015B3D File Offset: 0x00013D3D
		public List<TableColumnDefinition> Columns { get; set; } = new List<TableColumnDefinition>();

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00015B46 File Offset: 0x00013D46
		// (set) Token: 0x060008CF RID: 2255 RVA: 0x00015B4E File Offset: 0x00013D4E
		public List<TableCell> Cells { get; set; } = new List<TableCell>();

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00015B57 File Offset: 0x00013D57
		// (set) Token: 0x060008D1 RID: 2257 RVA: 0x00015B5F File Offset: 0x00013D5F
		public bool ExtendLastCellsToTableBottom { get; set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00015B68 File Offset: 0x00013D68
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x00015B70 File Offset: 0x00013D70
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00015B79 File Offset: 0x00013D79
		// (set) Token: 0x060008D5 RID: 2261 RVA: 0x00015B81 File Offset: 0x00013D81
		private bool CacheInitialized { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00015B8A File Offset: 0x00013D8A
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x00015B92 File Offset: 0x00013D92
		private bool HasRelativeColumns { get; set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00015B9B File Offset: 0x00013D9B
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x00015BA3 File Offset: 0x00013DA3
		private int StartingRowsCount { get; set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00015BAC File Offset: 0x00013DAC
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x00015BB4 File Offset: 0x00013DB4
		private int RowsCount { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00015BBD File Offset: 0x00013DBD
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x00015BC5 File Offset: 0x00013DC5
		private int MaxRow { get; set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00015BCE File Offset: 0x00013DCE
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x00015BD6 File Offset: 0x00013DD6
		private int MaxRowSpan { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00015BDF File Offset: 0x00013DDF
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x00015BE7 File Offset: 0x00013DE7
		private TableCell[][] CellsCache { get; set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x00015BF0 File Offset: 0x00013DF0
		private bool IsRendered
		{
			get
			{
				return this.CurrentRow > this.StartingRowsCount;
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00015C00 File Offset: 0x00013E00
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		internal override IEnumerable<Element> GetChildren()
		{
			return this.Cells;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00015C08 File Offset: 0x00013E08
		private void Initialize()
		{
			if (this.CacheInitialized)
			{
				return;
			}
			this.HasRelativeColumns = Enumerable.Any<TableColumnDefinition>(this.Columns, (TableColumnDefinition x) => x.RelativeSize > 0f);
			this.StartingRowsCount = Enumerable.Max(Enumerable.DefaultIfEmpty<int>(Enumerable.Select<TableCell, int>(this.Cells, (TableCell x) => x.Row), 0));
			this.RowsCount = Enumerable.Max(Enumerable.DefaultIfEmpty<int>(Enumerable.Select<TableCell, int>(this.Cells, (TableCell x) => x.Row + x.RowSpan - 1), 0));
			this.Cells = Enumerable.ToList<TableCell>(Enumerable.ThenBy<TableCell, int>(Enumerable.OrderBy<TableCell, int>(this.Cells, (TableCell x) => x.Row), (TableCell x) => x.Column));
			this.BuildCache();
			this.CacheInitialized = true;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00015D2C File Offset: 0x00013F2C
		private void BuildCache()
		{
			if (this.CellsCache != null)
			{
				return;
			}
			if (this.Cells.Count == 0)
			{
				this.MaxRow = 0;
				this.MaxRowSpan = 1;
				this.CellsCache = Array.Empty<TableCell[]>();
				return;
			}
			Dictionary<int, TableCell[]> groups = Enumerable.ToDictionary<IGrouping<int, TableCell>, int, TableCell[]>(Enumerable.GroupBy<TableCell, int>(this.Cells, (TableCell x) => x.Row + x.RowSpan - 1), (IGrouping<int, TableCell> x) => x.Key, (IGrouping<int, TableCell> x) => Enumerable.ToArray<TableCell>(Enumerable.OrderBy<TableCell, int>(x, (TableCell x) => x.Column)));
			this.MaxRow = Enumerable.Max<KeyValuePair<int, TableCell[]>>(groups, (KeyValuePair<int, TableCell[]> x) => x.Key);
			this.MaxRowSpan = Enumerable.Max<TableCell>(this.Cells, (TableCell x) => x.RowSpan);
			this.CellsCache = Enumerable.ToArray<TableCell[]>(Enumerable.Select<int, TableCell[]>(Enumerable.Range(0, this.MaxRow + 1), delegate(int x)
			{
				TableCell[] result;
				if (!groups.TryGetValue(x, ref result))
				{
					return Array.Empty<TableCell>();
				}
				return result;
			}));
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00015E6C File Offset: 0x0001406C
		internal override SpacePlan Measure(Size availableSpace)
		{
			this.Initialize();
			if (!Enumerable.Any<TableCell>(this.Cells))
			{
				return SpacePlan.Empty();
			}
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			if (this.HasRelativeColumns && availableSpace.Width < 0.01f)
			{
				return SpacePlan.Wrap("Insufficient space to render columns of relative size.");
			}
			this.UpdateColumnsWidth(availableSpace.Width);
			ICollection<TableCellRenderingCommand> collection = this.PlanLayout(availableSpace);
			if (!Enumerable.Any<TableCellRenderingCommand>(collection))
			{
				return SpacePlan.Wrap("Insufficient space to render (even partially) a single row.");
			}
			float width = Enumerable.Sum<TableColumnDefinition>(this.Columns, (TableColumnDefinition x) => x.Width);
			float height = Enumerable.Max<TableCellRenderingCommand>(collection, (TableCellRenderingCommand x) => x.Offset.Y + x.Size.Height);
			Size size = new Size(width, height);
			if (size.Width > availableSpace.Width + 0.01f)
			{
				return SpacePlan.Wrap("The content requires more horizontal space than available.");
			}
			if (this.CalculateCurrentRow(collection) <= this.StartingRowsCount)
			{
				return SpacePlan.PartialRender(size);
			}
			return SpacePlan.FullRender(size);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00015F7C File Offset: 0x0001417C
		internal override void Draw(Size availableSpace)
		{
			this.Initialize();
			this.RegisterSemanticTree();
			if (this.IsRendered)
			{
				return;
			}
			this.UpdateColumnsWidth(availableSpace.Width);
			ICollection<TableCellRenderingCommand> collection = this.PlanLayout(availableSpace);
			foreach (TableCellRenderingCommand tableCellRenderingCommand in Enumerable.OrderBy<TableCellRenderingCommand, int>(collection, (TableCellRenderingCommand x) => x.Cell.ZIndex))
			{
				SpacePlanType type = tableCellRenderingCommand.Measurement.Type;
				bool flag = type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
				if (flag)
				{
					tableCellRenderingCommand.Cell.IsRendered = true;
				}
				if (tableCellRenderingCommand.Measurement.Type != SpacePlanType.Wrap)
				{
					Position vector = (this.ContentDirection == ContentDirection.LeftToRight) ? tableCellRenderingCommand.Offset : new Position(availableSpace.Width - tableCellRenderingCommand.Offset.X - tableCellRenderingCommand.Size.Width, tableCellRenderingCommand.Offset.Y);
					base.Canvas.Translate(vector);
					tableCellRenderingCommand.Cell.Draw(tableCellRenderingCommand.Size);
					base.Canvas.Translate(vector.Reverse());
				}
			}
			this.CurrentRow = this.CalculateCurrentRow(collection);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x000160C8 File Offset: 0x000142C8
		private int CalculateCurrentRow(ICollection<TableCellRenderingCommand> commands)
		{
			if (!Enumerable.Any<TableCellRenderingCommand>(commands))
			{
				return this.CurrentRow;
			}
			List<TableCellRenderingCommand> list = Enumerable.ToList<TableCellRenderingCommand>(Enumerable.Where<TableCellRenderingCommand>(Enumerable.Where<TableCellRenderingCommand>(commands, (TableCellRenderingCommand x) => !x.Cell.IsRendered), delegate(TableCellRenderingCommand x)
			{
				SpacePlanType type = x.Measurement.Type;
				return type - SpacePlanType.Wrap <= 1;
			}));
			if (Enumerable.Any<TableCellRenderingCommand>(list))
			{
				return Enumerable.Min<TableCellRenderingCommand>(list, (TableCellRenderingCommand x) => x.Cell.Row + x.Cell.RowSpan - 1);
			}
			return Enumerable.Max<TableCellRenderingCommand>(commands, (TableCellRenderingCommand x) => x.Cell.Row + x.Cell.RowSpan - 1) + 1;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00016188 File Offset: 0x00014388
		private void UpdateColumnsWidth(float availableWidth)
		{
			float num = Enumerable.Sum<TableColumnDefinition>(this.Columns, (TableColumnDefinition x) => x.ConstantSize);
			float num2 = Enumerable.Sum<TableColumnDefinition>(this.Columns, (TableColumnDefinition x) => x.RelativeSize);
			float num3 = (num2 > 0f) ? ((availableWidth - num) / num2) : 0f;
			foreach (TableColumnDefinition tableColumnDefinition in this.Columns)
			{
				tableColumnDefinition.Width = tableColumnDefinition.ConstantSize + tableColumnDefinition.RelativeSize * num3;
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00016258 File Offset: 0x00014458
		private ICollection<TableCellRenderingCommand> PlanLayout(Size availableSpace)
		{
			Table.<>c__DisplayClass53_0 CS$<>8__locals1 = new Table.<>c__DisplayClass53_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.availableSpace = availableSpace;
			CS$<>8__locals1.columnOffsets = Table.<PlanLayout>g__GetColumnLeftOffsets|53_0(this.Columns);
			ICollection<TableCellRenderingCommand> collection = CS$<>8__locals1.<PlanLayout>g__GetRenderingCommands|1();
			if (!Enumerable.Any<TableCellRenderingCommand>(collection))
			{
				return collection;
			}
			if (this.ExtendLastCellsToTableBottom)
			{
				Table.<PlanLayout>g__AdjustLastCellSizes|53_3(Enumerable.Max<TableCellRenderingCommand>(collection, (TableCellRenderingCommand cell) => cell.Offset.Y + cell.Size.Height), collection);
			}
			return collection;
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x000162CD File Offset: 0x000144CD
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x000162D5 File Offset: 0x000144D5
		private int CurrentRow { get; set; }

		// Token: 0x060008ED RID: 2285 RVA: 0x000162E0 File Offset: 0x000144E0
		public void ResetState(bool hardReset = false)
		{
			foreach (TableCell tableCell in this.Cells)
			{
				tableCell.IsRendered = false;
			}
			this.CurrentRow = 1;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00016338 File Offset: 0x00014538
		public object GetState()
		{
			bool[] array = new bool[this.Cells.Count];
			for (int i = 0; i < this.Cells.Count; i++)
			{
				array[i] = this.Cells[i].IsRendered;
			}
			return new Table.TableState
			{
				CellsRenderingState = array,
				CurrentRow = this.CurrentRow
			};
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x000163A4 File Offset: 0x000145A4
		public void SetState(object state)
		{
			Table.TableState tableState = (Table.TableState)state;
			for (int i = 0; i < this.Cells.Count; i++)
			{
				this.Cells[i].IsRendered = tableState.CellsRenderingState[i];
			}
			this.CurrentRow = tableState.CurrentRow;
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x000163F3 File Offset: 0x000145F3
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x000163FB File Offset: 0x000145FB
		internal bool EnableAutomatedSemanticTagging { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00016404 File Offset: 0x00014604
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x0001640C File Offset: 0x0001460C
		private bool IsSemanticTaggingApplied { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00016415 File Offset: 0x00014615
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x0001641D File Offset: 0x0001461D
		[Nullable(2)]
		public SemanticTreeManager SemanticTreeManager { [NullableContext(2)] get; [NullableContext(2)] set; } = new SemanticTreeManager();

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00016426 File Offset: 0x00014626
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x0001642E File Offset: 0x0001462E
		internal bool TableRequiresAdvancedHeaderTagging { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00016437 File Offset: 0x00014637
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x0001643F File Offset: 0x0001463F
		internal Table.TablePartType PartType { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00016448 File Offset: 0x00014648
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00016450 File Offset: 0x00014650
		public List<TableCell> HeaderCells { get; set; } = new List<TableCell>();

		// Token: 0x060008FC RID: 2300 RVA: 0x0001645C File Offset: 0x0001465C
		private void RegisterSemanticTree()
		{
			if (this.SemanticTreeManager == null)
			{
				return;
			}
			if (this.SemanticTreeManager.IsCurrentContentArtifact())
			{
				return;
			}
			if (!this.EnableAutomatedSemanticTagging)
			{
				return;
			}
			if (this.IsSemanticTaggingApplied)
			{
				return;
			}
			this.IsSemanticTaggingApplied = true;
			foreach (IEnumerable<TableCell> enumerable in Enumerable.GroupBy<TableCell, int>(this.Cells, (TableCell x) => x.Row))
			{
				SemanticTreeNode node = new SemanticTreeNode
				{
					NodeId = this.SemanticTreeManager.GetNextNodeId(),
					Type = "TR"
				};
				this.SemanticTreeManager.AddNode(node);
				this.SemanticTreeManager.PushOnStack(node);
				foreach (TableCell tableCell in Enumerable.OrderBy<TableCell, int>(enumerable, (TableCell x) => x.Column))
				{
					tableCell.CreateProxy((Element x) => new SemanticTag
					{
						SemanticTreeManager = this.SemanticTreeManager,
						Canvas = base.Canvas,
						TagType = "TD",
						Child = x
					});
					SemanticTag semanticTag = tableCell.Child as SemanticTag;
					if (semanticTag != null)
					{
						if (this.PartType == Table.TablePartType.Header || tableCell.IsSemanticHorizontalHeader)
						{
							semanticTag.TagType = "TH";
						}
						semanticTag.RegisterCurrentSemanticNode();
						tableCell.SemanticNodeId = semanticTag.SemanticTreeNode.NodeId;
						Table.<RegisterSemanticTree>g__AssignCellAttributesForColumnAndRowSpans|87_0(tableCell, semanticTag);
					}
				}
				this.SemanticTreeManager.PopStack();
			}
			this.<RegisterSemanticTree>g__AssignCellAttributesForHeaderCellRoles|87_1();
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00016614 File Offset: 0x00014814
		public static bool DoesTableBodyRequireExtendedHeaderTagging(ICollection<TableCell> headerCells, ICollection<TableCell> bodyCells)
		{
			return Table.<DoesTableBodyRequireExtendedHeaderTagging>g__ContainsSpanningCells|88_0(headerCells) || Table.<DoesTableBodyRequireExtendedHeaderTagging>g__ContainsSpanningCells|88_0(bodyCells);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001665C File Offset: 0x0001485C
		[CompilerGenerated]
		internal static float[] <PlanLayout>g__GetColumnLeftOffsets|53_0(IList<TableColumnDefinition> columns)
		{
			float[] array = new float[columns.Count + 1];
			array[0] = 0f;
			foreach (int num in Enumerable.Range(1, array.Length - 1))
			{
				array[num] = columns[num - 1].Width + array[num - 1];
			}
			return array;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x000166D8 File Offset: 0x000148D8
		[CompilerGenerated]
		internal static void <PlanLayout>g__AdjustCellSizes|53_2(ICollection<TableCellRenderingCommand> commands, DynamicDictionary<int, float> rowBottomOffsets)
		{
			foreach (TableCellRenderingCommand tableCellRenderingCommand in commands)
			{
				int num = tableCellRenderingCommand.Cell.Row + tableCellRenderingCommand.Cell.RowSpan - 1;
				float height = rowBottomOffsets[num] - tableCellRenderingCommand.Offset.Y;
				tableCellRenderingCommand.Size = new Size(tableCellRenderingCommand.Size.Width, height);
				tableCellRenderingCommand.Offset = new Position(tableCellRenderingCommand.Offset.X, rowBottomOffsets[tableCellRenderingCommand.Cell.Row - 1]);
			}
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00016788 File Offset: 0x00014988
		[CompilerGenerated]
		internal static void <PlanLayout>g__AdjustLastCellSizes|53_3(float tableHeight, ICollection<TableCellRenderingCommand> commands)
		{
			int num = Enumerable.Max<TableCell>(Enumerable.Select<TableCellRenderingCommand, TableCell>(commands, (TableCellRenderingCommand x) => x.Cell), (TableCell x) => x.Column + x.ColumnSpan - 1);
			using (IEnumerator<int> enumerator = Enumerable.Range(1, num).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int column = enumerator.Current;
					TableCellRenderingCommand tableCellRenderingCommand = Enumerable.FirstOrDefault<TableCellRenderingCommand>(Enumerable.OrderByDescending<TableCellRenderingCommand, int>(Enumerable.Where<TableCellRenderingCommand>(commands, (TableCellRenderingCommand x) => x.Cell.Column <= column && column < x.Cell.Column + x.Cell.ColumnSpan), (TableCellRenderingCommand x) => x.Cell.Row + x.Cell.RowSpan));
					if (tableCellRenderingCommand != null)
					{
						tableCellRenderingCommand.Size = new Size(tableCellRenderingCommand.Size.Width, tableHeight - tableCellRenderingCommand.Offset.Y);
					}
				}
			}
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x000168BC File Offset: 0x00014ABC
		[CompilerGenerated]
		internal static void <RegisterSemanticTree>g__AssignCellAttributesForColumnAndRowSpans|87_0(TableCell tableCell, SemanticTag semanticTag)
		{
			if (tableCell.ColumnSpan > 1)
			{
				semanticTag.SemanticTreeNode.Attributes.Add(new SemanticTreeNode.Attribute
				{
					Owner = "Table",
					Name = "ColSpan",
					Value = tableCell.ColumnSpan
				});
			}
			if (tableCell.RowSpan > 1)
			{
				semanticTag.SemanticTreeNode.Attributes.Add(new SemanticTreeNode.Attribute
				{
					Owner = "Table",
					Name = "RowSpan",
					Value = tableCell.RowSpan
				});
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00016953 File Offset: 0x00014B53
		[CompilerGenerated]
		private void <RegisterSemanticTree>g__AssignCellAttributesForHeaderCellRoles|87_1()
		{
			if (this.PartType == Table.TablePartType.Footer)
			{
				return;
			}
			if (this.TableRequiresAdvancedHeaderTagging)
			{
				this.<RegisterSemanticTree>g__AssignCellAttributesForHeaderCellRolesOfComplexTables|87_3();
				return;
			}
			this.<RegisterSemanticTree>g__AssignCellAttributesForHeaderCellRolesOfSimpleTables|87_2();
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00016974 File Offset: 0x00014B74
		[CompilerGenerated]
		private void <RegisterSemanticTree>g__AssignCellAttributesForHeaderCellRolesOfSimpleTables|87_2()
		{
			foreach (TableCell tableCell in this.Cells)
			{
				SemanticTag semanticTag = tableCell.Child as SemanticTag;
				if (semanticTag != null && !(semanticTag.TagType != "TH"))
				{
					bool flag = this.PartType == Table.TablePartType.Header;
					bool isSemanticHorizontalHeader = tableCell.IsSemanticHorizontalHeader;
					string text;
					if (flag)
					{
						if (!isSemanticHorizontalHeader)
						{
							text = "Column";
						}
						else
						{
							text = "Both";
						}
					}
					else if (!isSemanticHorizontalHeader)
					{
						text = null;
					}
					else
					{
						text = "Row";
					}
					string text2 = text;
					if (text2 != null)
					{
						semanticTag.SemanticTreeNode.Attributes.Add(new SemanticTreeNode.Attribute
						{
							Owner = "Table",
							Name = "Scope",
							Value = text2
						});
					}
				}
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00016A60 File Offset: 0x00014C60
		[CompilerGenerated]
		private void <RegisterSemanticTree>g__AssignCellAttributesForHeaderCellRolesOfComplexTables|87_3()
		{
			Table.<>c__DisplayClass87_0 CS$<>8__locals1;
			CS$<>8__locals1.semanticHorizontalHeaders = Enumerable.ToList<TableCell>(Enumerable.Where<TableCell>(this.Cells, (TableCell x) => x.IsSemanticHorizontalHeader));
			foreach (TableCell tableCell in this.Cells)
			{
				SemanticTag semanticTag = tableCell.Child as SemanticTag;
				if (semanticTag != null)
				{
					int[] array = Enumerable.ToArray<int>(this.<RegisterSemanticTree>g__GetRelatedHeadersFor|87_8(tableCell, ref CS$<>8__locals1));
					if (Enumerable.Any<int>(array))
					{
						semanticTag.SemanticTreeNode.Attributes.Add(new SemanticTreeNode.Attribute
						{
							Owner = "Table",
							Name = "Headers",
							Value = array
						});
					}
				}
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00016B40 File Offset: 0x00014D40
		[CompilerGenerated]
		private IEnumerable<int> <RegisterSemanticTree>g__GetRelatedHeadersFor|87_8(TableCell cell, ref Table.<>c__DisplayClass87_0 A_2)
		{
			bool flag = this.PartType == Table.TablePartType.Header;
			IEnumerable<TableCell> enumerable = Enumerable.AsEnumerable<TableCell>(flag ? this.Cells : this.HeaderCells);
			if (flag)
			{
				enumerable = Enumerable.Where<TableCell>(enumerable, (TableCell x) => x.Row < cell.Row);
			}
			IEnumerable<int> enumerable2 = Enumerable.Select<TableCell, int>(Enumerable.Where<TableCell>(enumerable, (TableCell x) => x.Column < cell.Column + cell.ColumnSpan && cell.Column < x.Column + x.ColumnSpan), (TableCell x) => x.SemanticNodeId);
			if (flag)
			{
				return enumerable2;
			}
			IEnumerable<int> enumerable3 = Enumerable.Select<TableCell, int>(Enumerable.Where<TableCell>(Enumerable.Where<TableCell>(A_2.semanticHorizontalHeaders, (TableCell x) => x.Column < cell.Column), (TableCell x) => x.Row < cell.Row + cell.RowSpan && cell.Row < x.Row + x.RowSpan), (TableCell x) => x.SemanticNodeId);
			return Enumerable.Concat<int>(enumerable2, enumerable3);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00016C1F File Offset: 0x00014E1F
		[CompilerGenerated]
		internal static bool <DoesTableBodyRequireExtendedHeaderTagging>g__ContainsSpanningCells|88_0(IEnumerable<TableCell> cells)
		{
			return Enumerable.Any<TableCell>(cells, (TableCell x) => x.RowSpan > 1 || x.ColumnSpan > 1);
		}

		// Token: 0x020001D7 RID: 471
		[NullableContext(0)]
		public struct TableState
		{
			// Token: 0x040006E0 RID: 1760
			[Nullable(1)]
			public bool[] CellsRenderingState;

			// Token: 0x040006E1 RID: 1761
			public int CurrentRow;
		}

		// Token: 0x020001D8 RID: 472
		[NullableContext(0)]
		internal enum TablePartType
		{
			// Token: 0x040006E3 RID: 1763
			Header,
			// Token: 0x040006E4 RID: 1764
			Body,
			// Token: 0x040006E5 RID: 1765
			Footer
		}
	}
}
