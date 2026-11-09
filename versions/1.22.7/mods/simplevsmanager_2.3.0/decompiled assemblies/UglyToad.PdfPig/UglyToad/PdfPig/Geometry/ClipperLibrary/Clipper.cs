using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x0200018D RID: 397
	internal class Clipper : ClipperBase
	{
		// Token: 0x06000975 RID: 2421 RVA: 0x0002C3D0 File Offset: 0x0002A5D0
		public Clipper(int InitOptions = 0)
		{
			this.m_Scanbeam = null;
			this.m_Maxima = null;
			this.m_ActiveEdges = null;
			this.m_SortedEdges = null;
			this.m_IntersectList = new List<ClipperIntersectNode>();
			this.m_IntersectNodeComparer = new ClipperIntersectNodeSort();
			this.m_ExecuteLocked = false;
			this.m_UsingPolyTree = false;
			this.m_PolyOuts = new List<ClipperOutRec>();
			this.m_Joins = new List<ClipperJoin>();
			this.m_GhostJoins = new List<ClipperJoin>();
			this.ReverseSolution = ((1 & InitOptions) != 0);
			this.StrictlySimple = ((2 & InitOptions) != 0);
			base.PreserveCollinear = ((4 & InitOptions) != 0);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0002C468 File Offset: 0x0002A668
		private void InsertMaxima(long X)
		{
			ClipperMaxima clipperMaxima = new ClipperMaxima
			{
				X = X
			};
			if (this.m_Maxima == null)
			{
				this.m_Maxima = clipperMaxima;
				this.m_Maxima.Next = null;
				this.m_Maxima.Previous = null;
				return;
			}
			if (X < this.m_Maxima.X)
			{
				clipperMaxima.Next = this.m_Maxima;
				clipperMaxima.Previous = null;
				this.m_Maxima = clipperMaxima;
				return;
			}
			ClipperMaxima clipperMaxima2 = this.m_Maxima;
			while (clipperMaxima2.Next != null && X >= clipperMaxima2.Next.X)
			{
				clipperMaxima2 = clipperMaxima2.Next;
			}
			if (X == clipperMaxima2.X)
			{
				return;
			}
			clipperMaxima.Next = clipperMaxima2.Next;
			clipperMaxima.Previous = clipperMaxima2;
			if (clipperMaxima2.Next != null)
			{
				clipperMaxima2.Next.Previous = clipperMaxima;
			}
			clipperMaxima2.Next = clipperMaxima;
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0002C531 File Offset: 0x0002A731
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x0002C539 File Offset: 0x0002A739
		public bool ReverseSolution { get; set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0002C542 File Offset: 0x0002A742
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x0002C54A File Offset: 0x0002A74A
		public bool StrictlySimple { get; set; }

		// Token: 0x0600097B RID: 2427 RVA: 0x0002C553 File Offset: 0x0002A753
		public bool Execute(ClipperClipType clipType, List<List<ClipperIntPoint>> solution, ClipperPolyFillType FillType = ClipperPolyFillType.EvenOdd)
		{
			return this.Execute(clipType, solution, FillType, FillType);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0002C55F File Offset: 0x0002A75F
		public bool Execute(ClipperClipType clipType, ClipperPolyTree polytree, ClipperPolyFillType FillType = ClipperPolyFillType.EvenOdd)
		{
			return this.Execute(clipType, polytree, FillType, FillType);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0002C56C File Offset: 0x0002A76C
		public bool Execute(ClipperClipType clipType, List<List<ClipperIntPoint>> solution, ClipperPolyFillType subjFillType, ClipperPolyFillType clipFillType)
		{
			if (this.m_ExecuteLocked)
			{
				return false;
			}
			if (this.m_HasOpenPaths)
			{
				throw new ClipperException("Error: PolyTree struct is needed for open path clipping.");
			}
			this.m_ExecuteLocked = true;
			solution.Clear();
			this.m_SubjFillType = subjFillType;
			this.m_ClipFillType = clipFillType;
			this.m_ClipType = clipType;
			this.m_UsingPolyTree = false;
			bool flag;
			try
			{
				flag = this.ExecuteInternal();
				if (flag)
				{
					this.BuildResult(solution);
				}
			}
			finally
			{
				this.DisposeAllPolyPts();
				this.m_ExecuteLocked = false;
			}
			return flag;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0002C5F4 File Offset: 0x0002A7F4
		public bool Execute(ClipperClipType clipType, ClipperPolyTree polytree, ClipperPolyFillType subjFillType, ClipperPolyFillType clipFillType)
		{
			if (this.m_ExecuteLocked)
			{
				return false;
			}
			this.m_ExecuteLocked = true;
			this.m_SubjFillType = subjFillType;
			this.m_ClipFillType = clipFillType;
			this.m_ClipType = clipType;
			this.m_UsingPolyTree = true;
			bool flag;
			try
			{
				flag = this.ExecuteInternal();
				if (flag)
				{
					this.BuildResult2(polytree);
				}
			}
			finally
			{
				this.DisposeAllPolyPts();
				this.m_ExecuteLocked = false;
			}
			return flag;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0002C664 File Offset: 0x0002A864
		internal void FixHoleLinkage(ClipperOutRec outRec)
		{
			if (outRec.FirstLeft == null || (outRec.IsHole != outRec.FirstLeft.IsHole && outRec.FirstLeft.Pts != null))
			{
				return;
			}
			ClipperOutRec firstLeft = outRec.FirstLeft;
			while (firstLeft != null && (firstLeft.IsHole == outRec.IsHole || firstLeft.Pts == null))
			{
				firstLeft = firstLeft.FirstLeft;
			}
			outRec.FirstLeft = firstLeft;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0002C6CC File Offset: 0x0002A8CC
		private bool ExecuteInternal()
		{
			bool result;
			try
			{
				this.Reset();
				this.m_SortedEdges = null;
				this.m_Maxima = null;
				long botY;
				if (!base.PopScanbeam(out botY))
				{
					result = false;
				}
				else
				{
					this.InsertLocalMinimaIntoAEL(botY);
					long num;
					while (base.PopScanbeam(out num) || base.LocalMinimaPending())
					{
						this.ProcessHorizontals();
						this.m_GhostJoins.Clear();
						if (!this.ProcessIntersections(num))
						{
							return false;
						}
						this.ProcessEdgesAtTopOfScanbeam(num);
						botY = num;
						this.InsertLocalMinimaIntoAEL(botY);
					}
					foreach (ClipperOutRec clipperOutRec in this.m_PolyOuts)
					{
						if (clipperOutRec.Pts != null && !clipperOutRec.IsOpen && (clipperOutRec.IsHole ^ this.ReverseSolution) == this.Area(clipperOutRec) > 0.0)
						{
							this.ReversePolyPtLinks(clipperOutRec.Pts);
						}
					}
					this.JoinCommonEdges();
					foreach (ClipperOutRec clipperOutRec2 in this.m_PolyOuts)
					{
						if (clipperOutRec2.Pts != null)
						{
							if (clipperOutRec2.IsOpen)
							{
								this.FixupOutPolyline(clipperOutRec2);
							}
							else
							{
								this.FixupOutPolygon(clipperOutRec2);
							}
						}
					}
					if (this.StrictlySimple)
					{
						this.DoSimplePolygons();
					}
					result = true;
				}
			}
			finally
			{
				this.m_Joins.Clear();
				this.m_GhostJoins.Clear();
			}
			return result;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0002C88C File Offset: 0x0002AA8C
		private void DisposeAllPolyPts()
		{
			for (int i = 0; i < this.m_PolyOuts.Count; i++)
			{
				base.DisposeOutRec(i);
			}
			this.m_PolyOuts.Clear();
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0002C8C4 File Offset: 0x0002AAC4
		private void AddJoin(ClipperOutPt Op1, ClipperOutPt Op2, ClipperIntPoint OffPt)
		{
			ClipperJoin clipperJoin = new ClipperJoin
			{
				OutPt1 = Op1,
				OutPt2 = Op2,
				OffPt = OffPt
			};
			this.m_Joins.Add(clipperJoin);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0002C8F8 File Offset: 0x0002AAF8
		private void AddGhostJoin(ClipperOutPt Op, ClipperIntPoint OffPt)
		{
			ClipperJoin clipperJoin = new ClipperJoin
			{
				OutPt1 = Op,
				OffPt = OffPt
			};
			this.m_GhostJoins.Add(clipperJoin);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0002C928 File Offset: 0x0002AB28
		private void InsertLocalMinimaIntoAEL(long botY)
		{
			ClipperLocalMinima clipperLocalMinima;
			while (base.PopLocalMinima(botY, out clipperLocalMinima))
			{
				ClipperTEdge leftBound = clipperLocalMinima.LeftBound;
				ClipperTEdge rightBound = clipperLocalMinima.RightBound;
				ClipperOutPt clipperOutPt = null;
				if (leftBound == null)
				{
					this.InsertEdgeIntoAEL(rightBound, null);
					this.SetWindingCount(rightBound);
					if (this.IsContributing(rightBound))
					{
						clipperOutPt = this.AddOutPt(rightBound, rightBound.Bot);
					}
				}
				else if (rightBound == null)
				{
					this.InsertEdgeIntoAEL(leftBound, null);
					this.SetWindingCount(leftBound);
					if (this.IsContributing(leftBound))
					{
						clipperOutPt = this.AddOutPt(leftBound, leftBound.Bot);
					}
					base.InsertScanbeam(leftBound.Top.Y);
				}
				else
				{
					this.InsertEdgeIntoAEL(leftBound, null);
					this.InsertEdgeIntoAEL(rightBound, leftBound);
					this.SetWindingCount(leftBound);
					rightBound.WindCnt = leftBound.WindCnt;
					rightBound.WindCnt2 = leftBound.WindCnt2;
					if (this.IsContributing(leftBound))
					{
						clipperOutPt = this.AddLocalMinPoly(leftBound, rightBound, leftBound.Bot);
					}
					base.InsertScanbeam(leftBound.Top.Y);
				}
				if (rightBound != null)
				{
					if (ClipperBase.IsHorizontal(rightBound))
					{
						if (rightBound.NextInLML != null)
						{
							base.InsertScanbeam(rightBound.NextInLML.Top.Y);
						}
						this.AddEdgeToSEL(rightBound);
					}
					else
					{
						base.InsertScanbeam(rightBound.Top.Y);
					}
				}
				if (leftBound != null && rightBound != null)
				{
					if (clipperOutPt != null && ClipperBase.IsHorizontal(rightBound) && this.m_GhostJoins.Count > 0 && rightBound.WindDelta != 0)
					{
						for (int i = 0; i < this.m_GhostJoins.Count; i++)
						{
							ClipperJoin clipperJoin = this.m_GhostJoins[i];
							if (this.HorzSegmentsOverlap(clipperJoin.OutPt1.Pt.X, clipperJoin.OffPt.X, rightBound.Bot.X, rightBound.Top.X))
							{
								this.AddJoin(clipperJoin.OutPt1, clipperOutPt, clipperJoin.OffPt);
							}
						}
					}
					if (leftBound.OutIdx >= 0 && leftBound.PrevInAEL != null && leftBound.PrevInAEL.Curr.X == leftBound.Bot.X && leftBound.PrevInAEL.OutIdx >= 0 && ClipperBase.SlopesEqual(leftBound.PrevInAEL.Curr, leftBound.PrevInAEL.Top, leftBound.Curr, leftBound.Top, this.m_UseFullRange) && leftBound.WindDelta != 0 && leftBound.PrevInAEL.WindDelta != 0)
					{
						ClipperOutPt op = this.AddOutPt(leftBound.PrevInAEL, leftBound.Bot);
						this.AddJoin(clipperOutPt, op, leftBound.Top);
					}
					if (leftBound.NextInAEL != rightBound)
					{
						if (rightBound.OutIdx >= 0 && rightBound.PrevInAEL.OutIdx >= 0 && ClipperBase.SlopesEqual(rightBound.PrevInAEL.Curr, rightBound.PrevInAEL.Top, rightBound.Curr, rightBound.Top, this.m_UseFullRange) && rightBound.WindDelta != 0 && rightBound.PrevInAEL.WindDelta != 0)
						{
							ClipperOutPt op2 = this.AddOutPt(rightBound.PrevInAEL, rightBound.Bot);
							this.AddJoin(clipperOutPt, op2, rightBound.Top);
						}
						ClipperTEdge nextInAEL = leftBound.NextInAEL;
						if (nextInAEL != null)
						{
							while (nextInAEL != rightBound)
							{
								this.IntersectEdges(rightBound, nextInAEL, leftBound.Curr);
								nextInAEL = nextInAEL.NextInAEL;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0002CC70 File Offset: 0x0002AE70
		private void InsertEdgeIntoAEL(ClipperTEdge edge, ClipperTEdge startEdge)
		{
			if (this.m_ActiveEdges == null)
			{
				edge.PrevInAEL = null;
				edge.NextInAEL = null;
				this.m_ActiveEdges = edge;
				return;
			}
			if (startEdge == null && this.E2InsertsBeforeE1(this.m_ActiveEdges, edge))
			{
				edge.PrevInAEL = null;
				edge.NextInAEL = this.m_ActiveEdges;
				this.m_ActiveEdges.PrevInAEL = edge;
				this.m_ActiveEdges = edge;
				return;
			}
			if (startEdge == null)
			{
				startEdge = this.m_ActiveEdges;
			}
			while (startEdge.NextInAEL != null && !this.E2InsertsBeforeE1(startEdge.NextInAEL, edge))
			{
				startEdge = startEdge.NextInAEL;
			}
			edge.NextInAEL = startEdge.NextInAEL;
			if (startEdge.NextInAEL != null)
			{
				startEdge.NextInAEL.PrevInAEL = edge;
			}
			edge.PrevInAEL = startEdge;
			startEdge.NextInAEL = edge;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0002CD30 File Offset: 0x0002AF30
		private bool E2InsertsBeforeE1(ClipperTEdge e1, ClipperTEdge e2)
		{
			if (e2.Curr.X != e1.Curr.X)
			{
				return e2.Curr.X < e1.Curr.X;
			}
			if (e2.Top.Y > e1.Top.Y)
			{
				return e2.Top.X < Clipper.TopX(e1, e2.Top.Y);
			}
			return e1.Top.X > Clipper.TopX(e2, e1.Top.Y);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002CDC3 File Offset: 0x0002AFC3
		private bool IsEvenOddFillType(ClipperTEdge edge)
		{
			if (edge.PolyTyp == ClipperPolyType.Subject)
			{
				return this.m_SubjFillType == ClipperPolyFillType.EvenOdd;
			}
			return this.m_ClipFillType == ClipperPolyFillType.EvenOdd;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0002CDE0 File Offset: 0x0002AFE0
		private bool IsEvenOddAltFillType(ClipperTEdge edge)
		{
			if (edge.PolyTyp == ClipperPolyType.Subject)
			{
				return this.m_ClipFillType == ClipperPolyFillType.EvenOdd;
			}
			return this.m_SubjFillType == ClipperPolyFillType.EvenOdd;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0002CE00 File Offset: 0x0002B000
		private bool IsContributing(ClipperTEdge edge)
		{
			ClipperPolyFillType clipperPolyFillType;
			if (edge.PolyTyp == ClipperPolyType.Subject)
			{
				clipperPolyFillType = this.m_SubjFillType;
				ClipperPolyFillType clipFillType = this.m_ClipFillType;
			}
			else
			{
				clipperPolyFillType = this.m_ClipFillType;
				ClipperPolyFillType subjFillType = this.m_SubjFillType;
			}
			if (clipperPolyFillType == ClipperPolyFillType.EvenOdd || clipperPolyFillType != ClipperPolyFillType.NonZero)
			{
				if (edge.WindDelta == 0 && edge.WindCnt != 1)
				{
					return false;
				}
			}
			else if (Math.Abs(edge.WindCnt) != 1)
			{
				return false;
			}
			switch (this.m_ClipType)
			{
			case ClipperClipType.Intersection:
				return edge.WindCnt2 != 0;
			case ClipperClipType.Union:
				return edge.WindCnt2 == 0;
			case ClipperClipType.Difference:
				if (edge.PolyTyp == ClipperPolyType.Subject)
				{
					return edge.WindCnt2 == 0;
				}
				return edge.WindCnt2 != 0;
			case ClipperClipType.Xor:
				return edge.WindDelta != 0 || edge.WindCnt2 == 0;
			default:
				return true;
			}
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0002CEC4 File Offset: 0x0002B0C4
		private void SetWindingCount(ClipperTEdge edge)
		{
			ClipperTEdge clipperTEdge = edge.PrevInAEL;
			while (clipperTEdge != null && (clipperTEdge.PolyTyp != edge.PolyTyp || clipperTEdge.WindDelta == 0))
			{
				clipperTEdge = clipperTEdge.PrevInAEL;
			}
			if (clipperTEdge == null)
			{
				if (edge.PolyTyp != ClipperPolyType.Subject)
				{
					ClipperPolyFillType clipFillType = this.m_ClipFillType;
				}
				else
				{
					ClipperPolyFillType subjFillType = this.m_SubjFillType;
				}
				if (edge.WindDelta == 0)
				{
					edge.WindCnt = 1;
				}
				else
				{
					edge.WindCnt = edge.WindDelta;
				}
				edge.WindCnt2 = 0;
				clipperTEdge = this.m_ActiveEdges;
			}
			else if (edge.WindDelta == 0 && this.m_ClipType != ClipperClipType.Union)
			{
				edge.WindCnt = 1;
				edge.WindCnt2 = clipperTEdge.WindCnt2;
				clipperTEdge = clipperTEdge.NextInAEL;
			}
			else if (this.IsEvenOddFillType(edge))
			{
				if (edge.WindDelta == 0)
				{
					bool flag = true;
					for (ClipperTEdge prevInAEL = clipperTEdge.PrevInAEL; prevInAEL != null; prevInAEL = prevInAEL.PrevInAEL)
					{
						if (prevInAEL.PolyTyp == clipperTEdge.PolyTyp && prevInAEL.WindDelta != 0)
						{
							flag = !flag;
						}
					}
					edge.WindCnt = (flag ? 0 : 1);
				}
				else
				{
					edge.WindCnt = edge.WindDelta;
				}
				edge.WindCnt2 = clipperTEdge.WindCnt2;
				clipperTEdge = clipperTEdge.NextInAEL;
			}
			else
			{
				if (clipperTEdge.WindCnt * clipperTEdge.WindDelta < 0)
				{
					if (Math.Abs(clipperTEdge.WindCnt) > 1)
					{
						if (clipperTEdge.WindDelta * edge.WindDelta < 0)
						{
							edge.WindCnt = clipperTEdge.WindCnt;
						}
						else
						{
							edge.WindCnt = clipperTEdge.WindCnt + edge.WindDelta;
						}
					}
					else
					{
						edge.WindCnt = ((edge.WindDelta == 0) ? 1 : edge.WindDelta);
					}
				}
				else if (edge.WindDelta == 0)
				{
					edge.WindCnt = ((clipperTEdge.WindCnt < 0) ? (clipperTEdge.WindCnt - 1) : (clipperTEdge.WindCnt + 1));
				}
				else if (clipperTEdge.WindDelta * edge.WindDelta < 0)
				{
					edge.WindCnt = clipperTEdge.WindCnt;
				}
				else
				{
					edge.WindCnt = clipperTEdge.WindCnt + edge.WindDelta;
				}
				edge.WindCnt2 = clipperTEdge.WindCnt2;
				clipperTEdge = clipperTEdge.NextInAEL;
			}
			if (this.IsEvenOddAltFillType(edge))
			{
				while (clipperTEdge != edge)
				{
					if (clipperTEdge.WindDelta != 0)
					{
						edge.WindCnt2 = ((edge.WindCnt2 == 0) ? 1 : 0);
					}
					clipperTEdge = clipperTEdge.NextInAEL;
				}
				return;
			}
			while (clipperTEdge != edge)
			{
				edge.WindCnt2 += clipperTEdge.WindDelta;
				clipperTEdge = clipperTEdge.NextInAEL;
			}
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0002D114 File Offset: 0x0002B314
		private void AddEdgeToSEL(ClipperTEdge edge)
		{
			if (this.m_SortedEdges == null)
			{
				this.m_SortedEdges = edge;
				edge.PrevInSEL = null;
				edge.NextInSEL = null;
				return;
			}
			edge.NextInSEL = this.m_SortedEdges;
			edge.PrevInSEL = null;
			this.m_SortedEdges.PrevInSEL = edge;
			this.m_SortedEdges = edge;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0002D168 File Offset: 0x0002B368
		internal bool PopEdgeFromSEL(out ClipperTEdge e)
		{
			e = this.m_SortedEdges;
			if (e == null)
			{
				return false;
			}
			ClipperTEdge clipperTEdge = e;
			this.m_SortedEdges = e.NextInSEL;
			if (this.m_SortedEdges != null)
			{
				this.m_SortedEdges.PrevInSEL = null;
			}
			clipperTEdge.NextInSEL = null;
			clipperTEdge.PrevInSEL = null;
			return true;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002D1B4 File Offset: 0x0002B3B4
		private void CopyAELToSEL()
		{
			ClipperTEdge clipperTEdge = this.m_ActiveEdges;
			this.m_SortedEdges = clipperTEdge;
			while (clipperTEdge != null)
			{
				clipperTEdge.PrevInSEL = clipperTEdge.PrevInAEL;
				clipperTEdge.NextInSEL = clipperTEdge.NextInAEL;
				clipperTEdge = clipperTEdge.NextInAEL;
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0002D1F4 File Offset: 0x0002B3F4
		private void SwapPositionsInSEL(ClipperTEdge edge1, ClipperTEdge edge2)
		{
			if (edge1.NextInSEL == null && edge1.PrevInSEL == null)
			{
				return;
			}
			if (edge2.NextInSEL == null && edge2.PrevInSEL == null)
			{
				return;
			}
			if (edge1.NextInSEL == edge2)
			{
				ClipperTEdge nextInSEL = edge2.NextInSEL;
				if (nextInSEL != null)
				{
					nextInSEL.PrevInSEL = edge1;
				}
				ClipperTEdge prevInSEL = edge1.PrevInSEL;
				if (prevInSEL != null)
				{
					prevInSEL.NextInSEL = edge2;
				}
				edge2.PrevInSEL = prevInSEL;
				edge2.NextInSEL = edge1;
				edge1.PrevInSEL = edge2;
				edge1.NextInSEL = nextInSEL;
			}
			else if (edge2.NextInSEL == edge1)
			{
				ClipperTEdge nextInSEL2 = edge1.NextInSEL;
				if (nextInSEL2 != null)
				{
					nextInSEL2.PrevInSEL = edge2;
				}
				ClipperTEdge prevInSEL2 = edge2.PrevInSEL;
				if (prevInSEL2 != null)
				{
					prevInSEL2.NextInSEL = edge1;
				}
				edge1.PrevInSEL = prevInSEL2;
				edge1.NextInSEL = edge2;
				edge2.PrevInSEL = edge1;
				edge2.NextInSEL = nextInSEL2;
			}
			else
			{
				ClipperTEdge nextInSEL3 = edge1.NextInSEL;
				ClipperTEdge prevInSEL3 = edge1.PrevInSEL;
				edge1.NextInSEL = edge2.NextInSEL;
				if (edge1.NextInSEL != null)
				{
					edge1.NextInSEL.PrevInSEL = edge1;
				}
				edge1.PrevInSEL = edge2.PrevInSEL;
				if (edge1.PrevInSEL != null)
				{
					edge1.PrevInSEL.NextInSEL = edge1;
				}
				edge2.NextInSEL = nextInSEL3;
				if (edge2.NextInSEL != null)
				{
					edge2.NextInSEL.PrevInSEL = edge2;
				}
				edge2.PrevInSEL = prevInSEL3;
				if (edge2.PrevInSEL != null)
				{
					edge2.PrevInSEL.NextInSEL = edge2;
				}
			}
			if (edge1.PrevInSEL == null)
			{
				this.m_SortedEdges = edge1;
				return;
			}
			if (edge2.PrevInSEL == null)
			{
				this.m_SortedEdges = edge2;
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0002D364 File Offset: 0x0002B564
		private void AddLocalMaxPoly(ClipperTEdge e1, ClipperTEdge e2, ClipperIntPoint pt)
		{
			this.AddOutPt(e1, pt);
			if (e2.WindDelta == 0)
			{
				this.AddOutPt(e2, pt);
			}
			if (e1.OutIdx == e2.OutIdx)
			{
				e1.OutIdx = -1;
				e2.OutIdx = -1;
				return;
			}
			if (e1.OutIdx < e2.OutIdx)
			{
				this.AppendPolygon(e1, e2);
				return;
			}
			this.AppendPolygon(e2, e1);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0002D3C8 File Offset: 0x0002B5C8
		private ClipperOutPt AddLocalMinPoly(ClipperTEdge e1, ClipperTEdge e2, ClipperIntPoint pt)
		{
			ClipperOutPt clipperOutPt;
			ClipperTEdge clipperTEdge;
			ClipperTEdge prevInAEL;
			if (ClipperBase.IsHorizontal(e2) || e1.Dx > e2.Dx)
			{
				clipperOutPt = this.AddOutPt(e1, pt);
				e2.OutIdx = e1.OutIdx;
				e1.Side = ClipperEdgeSide.Left;
				e2.Side = ClipperEdgeSide.Right;
				clipperTEdge = e1;
				if (clipperTEdge.PrevInAEL == e2)
				{
					prevInAEL = e2.PrevInAEL;
				}
				else
				{
					prevInAEL = clipperTEdge.PrevInAEL;
				}
			}
			else
			{
				clipperOutPt = this.AddOutPt(e2, pt);
				e1.OutIdx = e2.OutIdx;
				e1.Side = ClipperEdgeSide.Right;
				e2.Side = ClipperEdgeSide.Left;
				clipperTEdge = e2;
				if (clipperTEdge.PrevInAEL == e1)
				{
					prevInAEL = e1.PrevInAEL;
				}
				else
				{
					prevInAEL = clipperTEdge.PrevInAEL;
				}
			}
			if (prevInAEL != null && prevInAEL.OutIdx >= 0 && prevInAEL.Top.Y < pt.Y && clipperTEdge.Top.Y < pt.Y)
			{
				long num = Clipper.TopX(prevInAEL, pt.Y);
				long num2 = Clipper.TopX(clipperTEdge, pt.Y);
				if (num == num2 && clipperTEdge.WindDelta != 0 && prevInAEL.WindDelta != 0 && ClipperBase.SlopesEqual(new ClipperIntPoint(num, pt.Y), prevInAEL.Top, new ClipperIntPoint(num2, pt.Y), clipperTEdge.Top, this.m_UseFullRange))
				{
					ClipperOutPt op = this.AddOutPt(prevInAEL, pt);
					this.AddJoin(clipperOutPt, op, clipperTEdge.Top);
				}
			}
			return clipperOutPt;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0002D520 File Offset: 0x0002B720
		private ClipperOutPt AddOutPt(ClipperTEdge e, ClipperIntPoint pt)
		{
			if (e.OutIdx < 0)
			{
				ClipperOutRec clipperOutRec = base.CreateOutRec();
				clipperOutRec.IsOpen = (e.WindDelta == 0);
				ClipperOutPt clipperOutPt = new ClipperOutPt();
				clipperOutRec.Pts = clipperOutPt;
				clipperOutPt.Index = clipperOutRec.Idx;
				clipperOutPt.Pt = pt;
				clipperOutPt.Next = clipperOutPt;
				clipperOutPt.Prev = clipperOutPt;
				if (!clipperOutRec.IsOpen)
				{
					this.SetHoleState(e, clipperOutRec);
				}
				e.OutIdx = clipperOutRec.Idx;
				return clipperOutPt;
			}
			ClipperOutRec clipperOutRec2 = this.m_PolyOuts[e.OutIdx];
			ClipperOutPt pts = clipperOutRec2.Pts;
			bool flag = e.Side == ClipperEdgeSide.Left;
			if (flag && pt == pts.Pt)
			{
				return pts;
			}
			if (!flag && pt == pts.Prev.Pt)
			{
				return pts.Prev;
			}
			ClipperOutPt clipperOutPt2 = new ClipperOutPt
			{
				Index = clipperOutRec2.Idx,
				Pt = pt,
				Next = pts,
				Prev = pts.Prev
			};
			clipperOutPt2.Prev.Next = clipperOutPt2;
			pts.Prev = clipperOutPt2;
			if (flag)
			{
				clipperOutRec2.Pts = clipperOutPt2;
			}
			return clipperOutPt2;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002D640 File Offset: 0x0002B840
		private ClipperOutPt GetLastOutPt(ClipperTEdge e)
		{
			ClipperOutRec clipperOutRec = this.m_PolyOuts[e.OutIdx];
			if (e.Side == ClipperEdgeSide.Left)
			{
				return clipperOutRec.Pts;
			}
			return clipperOutRec.Pts.Prev;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0002D67C File Offset: 0x0002B87C
		internal void SwapPoints(ref ClipperIntPoint pt1, ref ClipperIntPoint pt2)
		{
			ClipperIntPoint clipperIntPoint = new ClipperIntPoint(pt1);
			pt1 = pt2;
			pt2 = clipperIntPoint;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002D6A9 File Offset: 0x0002B8A9
		private bool HorzSegmentsOverlap(long seg1a, long seg1b, long seg2a, long seg2b)
		{
			if (seg1a > seg1b)
			{
				base.Swap(ref seg1a, ref seg1b);
			}
			if (seg2a > seg2b)
			{
				base.Swap(ref seg2a, ref seg2b);
			}
			return seg1a < seg2b && seg2a < seg1b;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0002D6D4 File Offset: 0x0002B8D4
		private void SetHoleState(ClipperTEdge e, ClipperOutRec outRec)
		{
			ClipperTEdge prevInAEL = e.PrevInAEL;
			ClipperTEdge clipperTEdge = null;
			while (prevInAEL != null)
			{
				if (prevInAEL.OutIdx >= 0 && prevInAEL.WindDelta != 0)
				{
					if (clipperTEdge == null)
					{
						clipperTEdge = prevInAEL;
					}
					else if (clipperTEdge.OutIdx == prevInAEL.OutIdx)
					{
						clipperTEdge = null;
					}
				}
				prevInAEL = prevInAEL.PrevInAEL;
			}
			if (clipperTEdge == null)
			{
				outRec.FirstLeft = null;
				outRec.IsHole = false;
				return;
			}
			outRec.FirstLeft = this.m_PolyOuts[clipperTEdge.OutIdx];
			outRec.IsHole = !outRec.FirstLeft.IsHole;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0002D75B File Offset: 0x0002B95B
		private double GetDx(ClipperIntPoint pt1, ClipperIntPoint pt2)
		{
			if (pt1.Y == pt2.Y)
			{
				return -3.4E+38;
			}
			return (double)(pt2.X - pt1.X) / (double)(pt2.Y - pt1.Y);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0002D794 File Offset: 0x0002B994
		private bool FirstIsBottomPt(ClipperOutPt btmPt1, ClipperOutPt btmPt2)
		{
			ClipperOutPt clipperOutPt = btmPt1.Prev;
			while (clipperOutPt.Pt == btmPt1.Pt && clipperOutPt != btmPt1)
			{
				clipperOutPt = clipperOutPt.Prev;
			}
			double num = Math.Abs(this.GetDx(btmPt1.Pt, clipperOutPt.Pt));
			clipperOutPt = btmPt1.Next;
			while (clipperOutPt.Pt == btmPt1.Pt && clipperOutPt != btmPt1)
			{
				clipperOutPt = clipperOutPt.Next;
			}
			double num2 = Math.Abs(this.GetDx(btmPt1.Pt, clipperOutPt.Pt));
			clipperOutPt = btmPt2.Prev;
			while (clipperOutPt.Pt == btmPt2.Pt && clipperOutPt != btmPt2)
			{
				clipperOutPt = clipperOutPt.Prev;
			}
			double num3 = Math.Abs(this.GetDx(btmPt2.Pt, clipperOutPt.Pt));
			clipperOutPt = btmPt2.Next;
			while (clipperOutPt.Pt == btmPt2.Pt && clipperOutPt != btmPt2)
			{
				clipperOutPt = clipperOutPt.Next;
			}
			double num4 = Math.Abs(this.GetDx(btmPt2.Pt, clipperOutPt.Pt));
			if (Math.Max(num, num2) == Math.Max(num3, num4) && Math.Min(num, num2) == Math.Min(num3, num4))
			{
				return this.Area(btmPt1) > 0.0;
			}
			return (num >= num3 && num >= num4) || (num2 >= num3 && num2 >= num4);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0002D8EC File Offset: 0x0002BAEC
		private ClipperOutPt GetBottomPt(ClipperOutPt pp)
		{
			ClipperOutPt clipperOutPt = null;
			ClipperOutPt next;
			for (next = pp.Next; next != pp; next = next.Next)
			{
				if (next.Pt.Y > pp.Pt.Y)
				{
					pp = next;
					clipperOutPt = null;
				}
				else if (next.Pt.Y == pp.Pt.Y && next.Pt.X <= pp.Pt.X)
				{
					if (next.Pt.X < pp.Pt.X)
					{
						clipperOutPt = null;
						pp = next;
					}
					else if (next.Next != pp && next.Prev != pp)
					{
						clipperOutPt = next;
					}
				}
			}
			if (clipperOutPt != null)
			{
				while (clipperOutPt != next)
				{
					if (!this.FirstIsBottomPt(next, clipperOutPt))
					{
						pp = clipperOutPt;
					}
					clipperOutPt = clipperOutPt.Next;
					while (clipperOutPt.Pt != pp.Pt)
					{
						clipperOutPt = clipperOutPt.Next;
					}
				}
			}
			return pp;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002D9D4 File Offset: 0x0002BBD4
		private ClipperOutRec GetLowermostRec(ClipperOutRec outRec1, ClipperOutRec outRec2)
		{
			if (outRec1.BottomPt == null)
			{
				outRec1.BottomPt = this.GetBottomPt(outRec1.Pts);
			}
			if (outRec2.BottomPt == null)
			{
				outRec2.BottomPt = this.GetBottomPt(outRec2.Pts);
			}
			ClipperOutPt bottomPt = outRec1.BottomPt;
			ClipperOutPt bottomPt2 = outRec2.BottomPt;
			if (bottomPt.Pt.Y > bottomPt2.Pt.Y)
			{
				return outRec1;
			}
			if (bottomPt.Pt.Y < bottomPt2.Pt.Y)
			{
				return outRec2;
			}
			if (bottomPt.Pt.X < bottomPt2.Pt.X)
			{
				return outRec1;
			}
			if (bottomPt.Pt.X > bottomPt2.Pt.X)
			{
				return outRec2;
			}
			if (bottomPt.Next == bottomPt)
			{
				return outRec2;
			}
			if (bottomPt2.Next == bottomPt2)
			{
				return outRec1;
			}
			if (this.FirstIsBottomPt(bottomPt, bottomPt2))
			{
				return outRec1;
			}
			return outRec2;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0002DAAE File Offset: 0x0002BCAE
		private bool OutRec1RightOfOutRec2(ClipperOutRec outRec1, ClipperOutRec outRec2)
		{
			for (;;)
			{
				outRec1 = outRec1.FirstLeft;
				if (outRec1 == outRec2)
				{
					break;
				}
				if (outRec1 == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002DAC4 File Offset: 0x0002BCC4
		private ClipperOutRec GetOutRec(int idx)
		{
			ClipperOutRec clipperOutRec;
			for (clipperOutRec = this.m_PolyOuts[idx]; clipperOutRec != this.m_PolyOuts[clipperOutRec.Idx]; clipperOutRec = this.m_PolyOuts[clipperOutRec.Idx])
			{
			}
			return clipperOutRec;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002DB08 File Offset: 0x0002BD08
		private void AppendPolygon(ClipperTEdge e1, ClipperTEdge e2)
		{
			ClipperOutRec clipperOutRec = this.m_PolyOuts[e1.OutIdx];
			ClipperOutRec clipperOutRec2 = this.m_PolyOuts[e2.OutIdx];
			ClipperOutRec clipperOutRec3;
			if (this.OutRec1RightOfOutRec2(clipperOutRec, clipperOutRec2))
			{
				clipperOutRec3 = clipperOutRec2;
			}
			else if (this.OutRec1RightOfOutRec2(clipperOutRec2, clipperOutRec))
			{
				clipperOutRec3 = clipperOutRec;
			}
			else
			{
				clipperOutRec3 = this.GetLowermostRec(clipperOutRec, clipperOutRec2);
			}
			ClipperOutPt pts = clipperOutRec.Pts;
			ClipperOutPt prev = pts.Prev;
			ClipperOutPt pts2 = clipperOutRec2.Pts;
			ClipperOutPt prev2 = pts2.Prev;
			if (e1.Side == ClipperEdgeSide.Left)
			{
				if (e2.Side == ClipperEdgeSide.Left)
				{
					this.ReversePolyPtLinks(pts2);
					pts2.Next = pts;
					pts.Prev = pts2;
					prev.Next = prev2;
					prev2.Prev = prev;
					clipperOutRec.Pts = prev2;
				}
				else
				{
					prev2.Next = pts;
					pts.Prev = prev2;
					pts2.Prev = prev;
					prev.Next = pts2;
					clipperOutRec.Pts = pts2;
				}
			}
			else if (e2.Side == ClipperEdgeSide.Right)
			{
				this.ReversePolyPtLinks(pts2);
				prev.Next = prev2;
				prev2.Prev = prev;
				pts2.Next = pts;
				pts.Prev = pts2;
			}
			else
			{
				prev.Next = pts2;
				pts2.Prev = prev;
				pts.Prev = prev2;
				prev2.Next = pts;
			}
			clipperOutRec.BottomPt = null;
			if (clipperOutRec3 == clipperOutRec2)
			{
				if (clipperOutRec2.FirstLeft != clipperOutRec)
				{
					clipperOutRec.FirstLeft = clipperOutRec2.FirstLeft;
				}
				clipperOutRec.IsHole = clipperOutRec2.IsHole;
			}
			clipperOutRec2.Pts = null;
			clipperOutRec2.BottomPt = null;
			clipperOutRec2.FirstLeft = clipperOutRec;
			int outIdx = e1.OutIdx;
			int outIdx2 = e2.OutIdx;
			e1.OutIdx = -1;
			e2.OutIdx = -1;
			for (ClipperTEdge clipperTEdge = this.m_ActiveEdges; clipperTEdge != null; clipperTEdge = clipperTEdge.NextInAEL)
			{
				if (clipperTEdge.OutIdx == outIdx2)
				{
					clipperTEdge.OutIdx = outIdx;
					clipperTEdge.Side = e1.Side;
					break;
				}
			}
			clipperOutRec2.Idx = clipperOutRec.Idx;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002DCF0 File Offset: 0x0002BEF0
		private void ReversePolyPtLinks(ClipperOutPt pp)
		{
			if (pp == null)
			{
				return;
			}
			ClipperOutPt clipperOutPt = pp;
			do
			{
				ClipperOutPt next = clipperOutPt.Next;
				clipperOutPt.Next = clipperOutPt.Prev;
				clipperOutPt.Prev = next;
				clipperOutPt = next;
			}
			while (clipperOutPt != pp);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0002DD24 File Offset: 0x0002BF24
		private static void SwapSides(ClipperTEdge edge1, ClipperTEdge edge2)
		{
			ClipperEdgeSide side = edge1.Side;
			edge1.Side = edge2.Side;
			edge2.Side = side;
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0002DD4C File Offset: 0x0002BF4C
		private static void SwapPolyIndexes(ClipperTEdge edge1, ClipperTEdge edge2)
		{
			int outIdx = edge1.OutIdx;
			edge1.OutIdx = edge2.OutIdx;
			edge2.OutIdx = outIdx;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0002DD74 File Offset: 0x0002BF74
		private void IntersectEdges(ClipperTEdge e1, ClipperTEdge e2, ClipperIntPoint pt)
		{
			bool flag = e1.OutIdx >= 0;
			bool flag2 = e2.OutIdx >= 0;
			if (e1.WindDelta == 0 || e2.WindDelta == 0)
			{
				if (e1.WindDelta == 0 && e2.WindDelta == 0)
				{
					return;
				}
				if (e1.PolyTyp == e2.PolyTyp && e1.WindDelta != e2.WindDelta && this.m_ClipType == ClipperClipType.Union)
				{
					if (e1.WindDelta == 0)
					{
						if (flag2)
						{
							this.AddOutPt(e1, pt);
							if (flag)
							{
								e1.OutIdx = -1;
								return;
							}
						}
					}
					else if (flag)
					{
						this.AddOutPt(e2, pt);
						if (flag2)
						{
							e2.OutIdx = -1;
							return;
						}
					}
				}
				else if (e1.PolyTyp != e2.PolyTyp)
				{
					if (e1.WindDelta == 0 && Math.Abs(e2.WindCnt) == 1 && (this.m_ClipType != ClipperClipType.Union || e2.WindCnt2 == 0))
					{
						this.AddOutPt(e1, pt);
						if (flag)
						{
							e1.OutIdx = -1;
							return;
						}
					}
					else if (e2.WindDelta == 0 && Math.Abs(e1.WindCnt) == 1 && (this.m_ClipType != ClipperClipType.Union || e1.WindCnt2 == 0))
					{
						this.AddOutPt(e2, pt);
						if (flag2)
						{
							e2.OutIdx = -1;
						}
					}
				}
				return;
			}
			else
			{
				if (e1.PolyTyp == e2.PolyTyp)
				{
					if (this.IsEvenOddFillType(e1))
					{
						int windCnt = e1.WindCnt;
						e1.WindCnt = e2.WindCnt;
						e2.WindCnt = windCnt;
					}
					else
					{
						if (e1.WindCnt + e2.WindDelta == 0)
						{
							e1.WindCnt = -e1.WindCnt;
						}
						else
						{
							e1.WindCnt += e2.WindDelta;
						}
						if (e2.WindCnt - e1.WindDelta == 0)
						{
							e2.WindCnt = -e2.WindCnt;
						}
						else
						{
							e2.WindCnt -= e1.WindDelta;
						}
					}
				}
				else
				{
					if (!this.IsEvenOddFillType(e2))
					{
						e1.WindCnt2 += e2.WindDelta;
					}
					else
					{
						e1.WindCnt2 = ((e1.WindCnt2 == 0) ? 1 : 0);
					}
					if (!this.IsEvenOddFillType(e1))
					{
						e2.WindCnt2 -= e1.WindDelta;
					}
					else
					{
						e2.WindCnt2 = ((e2.WindCnt2 == 0) ? 1 : 0);
					}
				}
				if (e1.PolyTyp == ClipperPolyType.Subject)
				{
					ClipperPolyFillType subjFillType = this.m_SubjFillType;
					ClipperPolyFillType clipFillType = this.m_ClipFillType;
				}
				else
				{
					ClipperPolyFillType clipFillType2 = this.m_ClipFillType;
					ClipperPolyFillType subjFillType2 = this.m_SubjFillType;
				}
				if (e2.PolyTyp == ClipperPolyType.Subject)
				{
					ClipperPolyFillType subjFillType3 = this.m_SubjFillType;
					ClipperPolyFillType clipFillType3 = this.m_ClipFillType;
				}
				else
				{
					ClipperPolyFillType clipFillType4 = this.m_ClipFillType;
					ClipperPolyFillType subjFillType4 = this.m_SubjFillType;
				}
				int num = Math.Abs(e1.WindCnt);
				int num2 = Math.Abs(e2.WindCnt);
				if (!flag || !flag2)
				{
					if (flag)
					{
						if (num2 == 0 || num2 == 1)
						{
							this.AddOutPt(e1, pt);
							Clipper.SwapSides(e1, e2);
							Clipper.SwapPolyIndexes(e1, e2);
							return;
						}
					}
					else if (flag2)
					{
						if (num == 0 || num == 1)
						{
							this.AddOutPt(e2, pt);
							Clipper.SwapSides(e1, e2);
							Clipper.SwapPolyIndexes(e1, e2);
							return;
						}
					}
					else if ((num == 0 || num == 1) && (num2 == 0 || num2 == 1))
					{
						long num3 = (long)Math.Abs(e1.WindCnt2);
						long num4 = (long)Math.Abs(e2.WindCnt2);
						if (e1.PolyTyp != e2.PolyTyp)
						{
							this.AddLocalMinPoly(e1, e2, pt);
							return;
						}
						if (num == 1 && num2 == 1)
						{
							switch (this.m_ClipType)
							{
							case ClipperClipType.Intersection:
								if (num3 > 0L && num4 > 0L)
								{
									this.AddLocalMinPoly(e1, e2, pt);
									return;
								}
								break;
							case ClipperClipType.Union:
								if (num3 <= 0L && num4 <= 0L)
								{
									this.AddLocalMinPoly(e1, e2, pt);
									return;
								}
								break;
							case ClipperClipType.Difference:
								if ((e1.PolyTyp == ClipperPolyType.Clip && num3 > 0L && num4 > 0L) || (e1.PolyTyp == ClipperPolyType.Subject && num3 <= 0L && num4 <= 0L))
								{
									this.AddLocalMinPoly(e1, e2, pt);
									return;
								}
								break;
							case ClipperClipType.Xor:
								this.AddLocalMinPoly(e1, e2, pt);
								return;
							default:
								return;
							}
						}
						else
						{
							Clipper.SwapSides(e1, e2);
						}
					}
					return;
				}
				if ((num != 0 && num != 1) || (num2 != 0 && num2 != 1) || (e1.PolyTyp != e2.PolyTyp && this.m_ClipType != ClipperClipType.Xor))
				{
					this.AddLocalMaxPoly(e1, e2, pt);
					return;
				}
				this.AddOutPt(e1, pt);
				this.AddOutPt(e2, pt);
				Clipper.SwapSides(e1, e2);
				Clipper.SwapPolyIndexes(e1, e2);
				return;
			}
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0002E198 File Offset: 0x0002C398
		private void ProcessHorizontals()
		{
			ClipperTEdge horzEdge;
			while (this.PopEdgeFromSEL(out horzEdge))
			{
				this.ProcessHorizontal(horzEdge);
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0002E1B8 File Offset: 0x0002C3B8
		private void GetHorzDirection(ClipperTEdge HorzEdge, out ClipperDirection Dir, out long Left, out long Right)
		{
			if (HorzEdge.Bot.X < HorzEdge.Top.X)
			{
				Left = HorzEdge.Bot.X;
				Right = HorzEdge.Top.X;
				Dir = ClipperDirection.LeftToRight;
				return;
			}
			Left = HorzEdge.Top.X;
			Right = HorzEdge.Bot.X;
			Dir = ClipperDirection.RightToLeft;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0002E21C File Offset: 0x0002C41C
		private void ProcessHorizontal(ClipperTEdge horzEdge)
		{
			bool flag = horzEdge.WindDelta == 0;
			ClipperDirection clipperDirection;
			long num;
			long num2;
			this.GetHorzDirection(horzEdge, out clipperDirection, out num, out num2);
			ClipperTEdge clipperTEdge = horzEdge;
			ClipperTEdge clipperTEdge2 = null;
			while (clipperTEdge.NextInLML != null && ClipperBase.IsHorizontal(clipperTEdge.NextInLML))
			{
				clipperTEdge = clipperTEdge.NextInLML;
			}
			if (clipperTEdge.NextInLML == null)
			{
				clipperTEdge2 = this.GetMaximaPair(clipperTEdge);
			}
			ClipperMaxima clipperMaxima = this.m_Maxima;
			if (clipperMaxima != null)
			{
				if (clipperDirection == ClipperDirection.LeftToRight)
				{
					while (clipperMaxima != null && clipperMaxima.X <= horzEdge.Bot.X)
					{
						clipperMaxima = clipperMaxima.Next;
					}
					if (clipperMaxima != null && clipperMaxima.X >= clipperTEdge.Top.X)
					{
						clipperMaxima = null;
					}
				}
				else
				{
					while (clipperMaxima.Next != null && clipperMaxima.Next.X < horzEdge.Bot.X)
					{
						clipperMaxima = clipperMaxima.Next;
					}
					if (clipperMaxima.X <= clipperTEdge.Top.X)
					{
						clipperMaxima = null;
					}
				}
			}
			ClipperOutPt clipperOutPt = null;
			for (;;)
			{
				bool flag2 = horzEdge == clipperTEdge;
				ClipperTEdge nextInAEL;
				for (ClipperTEdge clipperTEdge3 = this.GetNextInAEL(horzEdge, clipperDirection); clipperTEdge3 != null; clipperTEdge3 = nextInAEL)
				{
					if (clipperMaxima != null)
					{
						if (clipperDirection == ClipperDirection.LeftToRight)
						{
							while (clipperMaxima != null)
							{
								if (clipperMaxima.X >= clipperTEdge3.Curr.X)
								{
									break;
								}
								if (horzEdge.OutIdx >= 0 && !flag)
								{
									this.AddOutPt(horzEdge, new ClipperIntPoint(clipperMaxima.X, horzEdge.Bot.Y));
								}
								clipperMaxima = clipperMaxima.Next;
							}
						}
						else
						{
							while (clipperMaxima != null && clipperMaxima.X > clipperTEdge3.Curr.X)
							{
								if (horzEdge.OutIdx >= 0 && !flag)
								{
									this.AddOutPt(horzEdge, new ClipperIntPoint(clipperMaxima.X, horzEdge.Bot.Y));
								}
								clipperMaxima = clipperMaxima.Previous;
							}
						}
					}
					if ((clipperDirection == ClipperDirection.LeftToRight && clipperTEdge3.Curr.X > num2) || (clipperDirection == ClipperDirection.RightToLeft && clipperTEdge3.Curr.X < num) || (clipperTEdge3.Curr.X == horzEdge.Top.X && horzEdge.NextInLML != null && clipperTEdge3.Dx < horzEdge.NextInLML.Dx))
					{
						break;
					}
					if (horzEdge.OutIdx >= 0 && !flag)
					{
						clipperOutPt = this.AddOutPt(horzEdge, clipperTEdge3.Curr);
						for (ClipperTEdge clipperTEdge4 = this.m_SortedEdges; clipperTEdge4 != null; clipperTEdge4 = clipperTEdge4.NextInSEL)
						{
							if (clipperTEdge4.OutIdx >= 0 && this.HorzSegmentsOverlap(horzEdge.Bot.X, horzEdge.Top.X, clipperTEdge4.Bot.X, clipperTEdge4.Top.X))
							{
								ClipperOutPt lastOutPt = this.GetLastOutPt(clipperTEdge4);
								this.AddJoin(lastOutPt, clipperOutPt, clipperTEdge4.Top);
							}
						}
						this.AddGhostJoin(clipperOutPt, horzEdge.Bot);
					}
					if (clipperTEdge3 == clipperTEdge2 && flag2)
					{
						goto Block_28;
					}
					if (clipperDirection == ClipperDirection.LeftToRight)
					{
						ClipperIntPoint pt = new ClipperIntPoint(clipperTEdge3.Curr.X, horzEdge.Curr.Y);
						this.IntersectEdges(horzEdge, clipperTEdge3, pt);
					}
					else
					{
						ClipperIntPoint pt2 = new ClipperIntPoint(clipperTEdge3.Curr.X, horzEdge.Curr.Y);
						this.IntersectEdges(clipperTEdge3, horzEdge, pt2);
					}
					nextInAEL = this.GetNextInAEL(clipperTEdge3, clipperDirection);
					base.SwapPositionsInAEL(horzEdge, clipperTEdge3);
				}
				if (horzEdge.NextInLML == null || !ClipperBase.IsHorizontal(horzEdge.NextInLML))
				{
					goto IL_39F;
				}
				base.UpdateEdgeIntoAEL(ref horzEdge);
				if (horzEdge.OutIdx >= 0)
				{
					this.AddOutPt(horzEdge, horzEdge.Bot);
				}
				this.GetHorzDirection(horzEdge, out clipperDirection, out num, out num2);
			}
			Block_28:
			if (horzEdge.OutIdx >= 0)
			{
				this.AddLocalMaxPoly(horzEdge, clipperTEdge2, horzEdge.Top);
			}
			base.DeleteFromAEL(horzEdge);
			base.DeleteFromAEL(clipperTEdge2);
			return;
			IL_39F:
			if (horzEdge.OutIdx >= 0 && clipperOutPt == null)
			{
				clipperOutPt = this.GetLastOutPt(horzEdge);
				for (ClipperTEdge clipperTEdge5 = this.m_SortedEdges; clipperTEdge5 != null; clipperTEdge5 = clipperTEdge5.NextInSEL)
				{
					if (clipperTEdge5.OutIdx >= 0 && this.HorzSegmentsOverlap(horzEdge.Bot.X, horzEdge.Top.X, clipperTEdge5.Bot.X, clipperTEdge5.Top.X))
					{
						ClipperOutPt lastOutPt2 = this.GetLastOutPt(clipperTEdge5);
						this.AddJoin(lastOutPt2, clipperOutPt, clipperTEdge5.Top);
					}
				}
				this.AddGhostJoin(clipperOutPt, horzEdge.Top);
			}
			if (horzEdge.NextInLML != null)
			{
				if (horzEdge.OutIdx < 0)
				{
					base.UpdateEdgeIntoAEL(ref horzEdge);
					return;
				}
				clipperOutPt = this.AddOutPt(horzEdge, horzEdge.Top);
				base.UpdateEdgeIntoAEL(ref horzEdge);
				if (horzEdge.WindDelta == 0)
				{
					return;
				}
				ClipperTEdge prevInAEL = horzEdge.PrevInAEL;
				ClipperTEdge nextInAEL2 = horzEdge.NextInAEL;
				if (prevInAEL != null && prevInAEL.Curr.X == horzEdge.Bot.X && prevInAEL.Curr.Y == horzEdge.Bot.Y && prevInAEL.WindDelta != 0 && prevInAEL.OutIdx >= 0 && prevInAEL.Curr.Y > prevInAEL.Top.Y && ClipperBase.SlopesEqual(horzEdge, prevInAEL, this.m_UseFullRange))
				{
					ClipperOutPt op = this.AddOutPt(prevInAEL, horzEdge.Bot);
					this.AddJoin(clipperOutPt, op, horzEdge.Top);
					return;
				}
				if (nextInAEL2 != null && nextInAEL2.Curr.X == horzEdge.Bot.X && nextInAEL2.Curr.Y == horzEdge.Bot.Y && nextInAEL2.WindDelta != 0 && nextInAEL2.OutIdx >= 0 && nextInAEL2.Curr.Y > nextInAEL2.Top.Y && ClipperBase.SlopesEqual(horzEdge, nextInAEL2, this.m_UseFullRange))
				{
					ClipperOutPt op2 = this.AddOutPt(nextInAEL2, horzEdge.Bot);
					this.AddJoin(clipperOutPt, op2, horzEdge.Top);
					return;
				}
			}
			else
			{
				if (horzEdge.OutIdx >= 0)
				{
					this.AddOutPt(horzEdge, horzEdge.Top);
				}
				base.DeleteFromAEL(horzEdge);
			}
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0002E806 File Offset: 0x0002CA06
		private ClipperTEdge GetNextInAEL(ClipperTEdge e, ClipperDirection Direction)
		{
			if (Direction != ClipperDirection.LeftToRight)
			{
				return e.PrevInAEL;
			}
			return e.NextInAEL;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0002E819 File Offset: 0x0002CA19
		private bool IsMaxima(ClipperTEdge e, double Y)
		{
			return e != null && (double)e.Top.Y == Y && e.NextInLML == null;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0002E838 File Offset: 0x0002CA38
		private bool IsIntermediate(ClipperTEdge e, double Y)
		{
			return (double)e.Top.Y == Y && e.NextInLML != null;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0002E854 File Offset: 0x0002CA54
		internal ClipperTEdge GetMaximaPair(ClipperTEdge e)
		{
			if (e.Next.Top == e.Top && e.Next.NextInLML == null)
			{
				return e.Next;
			}
			if (e.Prev.Top == e.Top && e.Prev.NextInLML == null)
			{
				return e.Prev;
			}
			return null;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0002E8BC File Offset: 0x0002CABC
		internal ClipperTEdge GetMaximaPairEx(ClipperTEdge e)
		{
			ClipperTEdge maximaPair = this.GetMaximaPair(e);
			if (maximaPair == null || maximaPair.OutIdx == -2 || (maximaPair.NextInAEL == maximaPair.PrevInAEL && !ClipperBase.IsHorizontal(maximaPair)))
			{
				return null;
			}
			return maximaPair;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0002E8F8 File Offset: 0x0002CAF8
		private bool ProcessIntersections(long topY)
		{
			if (this.m_ActiveEdges == null)
			{
				return true;
			}
			try
			{
				this.BuildIntersectList(topY);
				if (this.m_IntersectList.Count == 0)
				{
					return true;
				}
				if (this.m_IntersectList.Count != 1 && !this.FixupIntersectionOrder())
				{
					return false;
				}
				this.ProcessIntersectList();
			}
			catch
			{
				this.m_SortedEdges = null;
				this.m_IntersectList.Clear();
				throw new ClipperException("ProcessIntersections error");
			}
			this.m_SortedEdges = null;
			return true;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0002E984 File Offset: 0x0002CB84
		private void BuildIntersectList(long topY)
		{
			if (this.m_ActiveEdges == null)
			{
				return;
			}
			ClipperTEdge clipperTEdge = this.m_ActiveEdges;
			this.m_SortedEdges = clipperTEdge;
			while (clipperTEdge != null)
			{
				clipperTEdge.PrevInSEL = clipperTEdge.PrevInAEL;
				clipperTEdge.NextInSEL = clipperTEdge.NextInAEL;
				clipperTEdge.Curr.X = Clipper.TopX(clipperTEdge, topY);
				clipperTEdge = clipperTEdge.NextInAEL;
			}
			bool flag = true;
			while (flag && this.m_SortedEdges != null)
			{
				flag = false;
				clipperTEdge = this.m_SortedEdges;
				while (clipperTEdge.NextInSEL != null)
				{
					ClipperTEdge nextInSEL = clipperTEdge.NextInSEL;
					if (clipperTEdge.Curr.X > nextInSEL.Curr.X)
					{
						ClipperIntPoint clipperIntPoint;
						this.IntersectPoint(clipperTEdge, nextInSEL, out clipperIntPoint);
						if (clipperIntPoint.Y < topY)
						{
							clipperIntPoint = new ClipperIntPoint(Clipper.TopX(clipperTEdge, topY), topY);
						}
						ClipperIntersectNode clipperIntersectNode = new ClipperIntersectNode
						{
							Edge1 = clipperTEdge,
							Edge2 = nextInSEL,
							Pt = clipperIntPoint
						};
						this.m_IntersectList.Add(clipperIntersectNode);
						this.SwapPositionsInSEL(clipperTEdge, nextInSEL);
						flag = true;
					}
					else
					{
						clipperTEdge = nextInSEL;
					}
				}
				if (clipperTEdge.PrevInSEL == null)
				{
					break;
				}
				clipperTEdge.PrevInSEL.NextInSEL = null;
			}
			this.m_SortedEdges = null;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0002EA99 File Offset: 0x0002CC99
		private bool EdgesAdjacent(ClipperIntersectNode inode)
		{
			return inode.Edge1.NextInSEL == inode.Edge2 || inode.Edge1.PrevInSEL == inode.Edge2;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0002EAC4 File Offset: 0x0002CCC4
		private bool FixupIntersectionOrder()
		{
			this.m_IntersectList.Sort(this.m_IntersectNodeComparer);
			this.CopyAELToSEL();
			int count = this.m_IntersectList.Count;
			for (int i = 0; i < count; i++)
			{
				if (!this.EdgesAdjacent(this.m_IntersectList[i]))
				{
					int num = i + 1;
					while (num < count && !this.EdgesAdjacent(this.m_IntersectList[num]))
					{
						num++;
					}
					if (num == count)
					{
						return false;
					}
					ClipperIntersectNode clipperIntersectNode = this.m_IntersectList[i];
					this.m_IntersectList[i] = this.m_IntersectList[num];
					this.m_IntersectList[num] = clipperIntersectNode;
				}
				this.SwapPositionsInSEL(this.m_IntersectList[i].Edge1, this.m_IntersectList[i].Edge2);
			}
			return true;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0002EBA0 File Offset: 0x0002CDA0
		private void ProcessIntersectList()
		{
			for (int i = 0; i < this.m_IntersectList.Count; i++)
			{
				ClipperIntersectNode clipperIntersectNode = this.m_IntersectList[i];
				this.IntersectEdges(clipperIntersectNode.Edge1, clipperIntersectNode.Edge2, clipperIntersectNode.Pt);
				base.SwapPositionsInAEL(clipperIntersectNode.Edge1, clipperIntersectNode.Edge2);
			}
			this.m_IntersectList.Clear();
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002EC05 File Offset: 0x0002CE05
		internal static long Round(double value)
		{
			if (value >= 0.0)
			{
				return (long)(value + 0.5);
			}
			return (long)(value - 0.5);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0002EC2C File Offset: 0x0002CE2C
		private static long TopX(ClipperTEdge edge, long currentY)
		{
			if (currentY == edge.Top.Y)
			{
				return edge.Top.X;
			}
			return edge.Bot.X + Clipper.Round(edge.Dx * (double)(currentY - edge.Bot.Y));
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0002EC7C File Offset: 0x0002CE7C
		private void IntersectPoint(ClipperTEdge edge1, ClipperTEdge edge2, out ClipperIntPoint ip)
		{
			ip = default(ClipperIntPoint);
			if (edge1.Dx == edge2.Dx)
			{
				ip.Y = edge1.Curr.Y;
				ip.X = Clipper.TopX(edge1, ip.Y);
				return;
			}
			if (edge1.Delta.X == 0L)
			{
				ip.X = edge1.Bot.X;
				if (ClipperBase.IsHorizontal(edge2))
				{
					ip.Y = edge2.Bot.Y;
				}
				else
				{
					double num = (double)edge2.Bot.Y - (double)edge2.Bot.X / edge2.Dx;
					ip.Y = Clipper.Round((double)ip.X / edge2.Dx + num);
				}
			}
			else if (edge2.Delta.X == 0L)
			{
				ip.X = edge2.Bot.X;
				if (ClipperBase.IsHorizontal(edge1))
				{
					ip.Y = edge1.Bot.Y;
				}
				else
				{
					double num2 = (double)edge1.Bot.Y - (double)edge1.Bot.X / edge1.Dx;
					ip.Y = Clipper.Round((double)ip.X / edge1.Dx + num2);
				}
			}
			else
			{
				double num2 = (double)edge1.Bot.X - (double)edge1.Bot.Y * edge1.Dx;
				double num = (double)edge2.Bot.X - (double)edge2.Bot.Y * edge2.Dx;
				double num3 = (num - num2) / (edge1.Dx - edge2.Dx);
				ip.Y = Clipper.Round(num3);
				if (Math.Abs(edge1.Dx) < Math.Abs(edge2.Dx))
				{
					ip.X = Clipper.Round(edge1.Dx * num3 + num2);
				}
				else
				{
					ip.X = Clipper.Round(edge2.Dx * num3 + num);
				}
			}
			if (ip.Y < edge1.Top.Y || ip.Y < edge2.Top.Y)
			{
				if (edge1.Top.Y > edge2.Top.Y)
				{
					ip.Y = edge1.Top.Y;
				}
				else
				{
					ip.Y = edge2.Top.Y;
				}
				if (Math.Abs(edge1.Dx) < Math.Abs(edge2.Dx))
				{
					ip.X = Clipper.TopX(edge1, ip.Y);
				}
				else
				{
					ip.X = Clipper.TopX(edge2, ip.Y);
				}
			}
			if (ip.Y > edge1.Curr.Y)
			{
				ip.Y = edge1.Curr.Y;
				if (Math.Abs(edge1.Dx) > Math.Abs(edge2.Dx))
				{
					ip.X = Clipper.TopX(edge2, ip.Y);
					return;
				}
				ip.X = Clipper.TopX(edge1, ip.Y);
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0002EF64 File Offset: 0x0002D164
		private void ProcessEdgesAtTopOfScanbeam(long topY)
		{
			ClipperTEdge clipperTEdge = this.m_ActiveEdges;
			while (clipperTEdge != null)
			{
				bool flag = this.IsMaxima(clipperTEdge, (double)topY);
				if (flag)
				{
					ClipperTEdge maximaPairEx = this.GetMaximaPairEx(clipperTEdge);
					flag = (maximaPairEx == null || !ClipperBase.IsHorizontal(maximaPairEx));
				}
				if (flag)
				{
					if (this.StrictlySimple)
					{
						this.InsertMaxima(clipperTEdge.Top.X);
					}
					ClipperTEdge prevInAEL = clipperTEdge.PrevInAEL;
					this.DoMaxima(clipperTEdge);
					if (prevInAEL == null)
					{
						clipperTEdge = this.m_ActiveEdges;
					}
					else
					{
						clipperTEdge = prevInAEL.NextInAEL;
					}
				}
				else
				{
					if (this.IsIntermediate(clipperTEdge, (double)topY) && ClipperBase.IsHorizontal(clipperTEdge.NextInLML))
					{
						base.UpdateEdgeIntoAEL(ref clipperTEdge);
						if (clipperTEdge.OutIdx >= 0)
						{
							this.AddOutPt(clipperTEdge, clipperTEdge.Bot);
						}
						this.AddEdgeToSEL(clipperTEdge);
					}
					else
					{
						clipperTEdge.Curr.X = Clipper.TopX(clipperTEdge, topY);
						clipperTEdge.Curr.Y = topY;
					}
					if (this.StrictlySimple)
					{
						ClipperTEdge prevInAEL2 = clipperTEdge.PrevInAEL;
						if (clipperTEdge.OutIdx >= 0 && clipperTEdge.WindDelta != 0 && prevInAEL2 != null && prevInAEL2.OutIdx >= 0 && prevInAEL2.Curr.X == clipperTEdge.Curr.X && prevInAEL2.WindDelta != 0)
						{
							ClipperIntPoint clipperIntPoint = new ClipperIntPoint(clipperTEdge.Curr);
							ClipperOutPt op = this.AddOutPt(prevInAEL2, clipperIntPoint);
							ClipperOutPt op2 = this.AddOutPt(clipperTEdge, clipperIntPoint);
							this.AddJoin(op, op2, clipperIntPoint);
						}
					}
					clipperTEdge = clipperTEdge.NextInAEL;
				}
			}
			this.ProcessHorizontals();
			this.m_Maxima = null;
			for (clipperTEdge = this.m_ActiveEdges; clipperTEdge != null; clipperTEdge = clipperTEdge.NextInAEL)
			{
				if (this.IsIntermediate(clipperTEdge, (double)topY))
				{
					ClipperOutPt clipperOutPt = null;
					if (clipperTEdge.OutIdx >= 0)
					{
						clipperOutPt = this.AddOutPt(clipperTEdge, clipperTEdge.Top);
					}
					base.UpdateEdgeIntoAEL(ref clipperTEdge);
					ClipperTEdge prevInAEL3 = clipperTEdge.PrevInAEL;
					ClipperTEdge nextInAEL = clipperTEdge.NextInAEL;
					if (prevInAEL3 != null && prevInAEL3.Curr.X == clipperTEdge.Bot.X && prevInAEL3.Curr.Y == clipperTEdge.Bot.Y && clipperOutPt != null && prevInAEL3.OutIdx >= 0 && prevInAEL3.Curr.Y > prevInAEL3.Top.Y && ClipperBase.SlopesEqual(clipperTEdge.Curr, clipperTEdge.Top, prevInAEL3.Curr, prevInAEL3.Top, this.m_UseFullRange) && clipperTEdge.WindDelta != 0 && prevInAEL3.WindDelta != 0)
					{
						ClipperOutPt op3 = this.AddOutPt(prevInAEL3, clipperTEdge.Bot);
						this.AddJoin(clipperOutPt, op3, clipperTEdge.Top);
					}
					else if (nextInAEL != null && nextInAEL.Curr.X == clipperTEdge.Bot.X && nextInAEL.Curr.Y == clipperTEdge.Bot.Y && clipperOutPt != null && nextInAEL.OutIdx >= 0 && nextInAEL.Curr.Y > nextInAEL.Top.Y && ClipperBase.SlopesEqual(clipperTEdge.Curr, clipperTEdge.Top, nextInAEL.Curr, nextInAEL.Top, this.m_UseFullRange) && clipperTEdge.WindDelta != 0 && nextInAEL.WindDelta != 0)
					{
						ClipperOutPt op4 = this.AddOutPt(nextInAEL, clipperTEdge.Bot);
						this.AddJoin(clipperOutPt, op4, clipperTEdge.Top);
					}
				}
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002F2C0 File Offset: 0x0002D4C0
		private void DoMaxima(ClipperTEdge e)
		{
			ClipperTEdge maximaPairEx = this.GetMaximaPairEx(e);
			if (maximaPairEx == null)
			{
				if (e.OutIdx >= 0)
				{
					this.AddOutPt(e, e.Top);
				}
				base.DeleteFromAEL(e);
				return;
			}
			ClipperTEdge nextInAEL = e.NextInAEL;
			while (nextInAEL != null && nextInAEL != maximaPairEx)
			{
				this.IntersectEdges(e, nextInAEL, e.Top);
				base.SwapPositionsInAEL(e, nextInAEL);
				nextInAEL = e.NextInAEL;
			}
			if (e.OutIdx == -1 && maximaPairEx.OutIdx == -1)
			{
				base.DeleteFromAEL(e);
				base.DeleteFromAEL(maximaPairEx);
				return;
			}
			if (e.OutIdx >= 0 && maximaPairEx.OutIdx >= 0)
			{
				if (e.OutIdx >= 0)
				{
					this.AddLocalMaxPoly(e, maximaPairEx, e.Top);
				}
				base.DeleteFromAEL(e);
				base.DeleteFromAEL(maximaPairEx);
				return;
			}
			if (e.WindDelta == 0)
			{
				if (e.OutIdx >= 0)
				{
					this.AddOutPt(e, e.Top);
					e.OutIdx = -1;
				}
				base.DeleteFromAEL(e);
				if (maximaPairEx.OutIdx >= 0)
				{
					this.AddOutPt(maximaPairEx, e.Top);
					maximaPairEx.OutIdx = -1;
				}
				base.DeleteFromAEL(maximaPairEx);
				return;
			}
			throw new ClipperException("DoMaxima error");
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0002F3DC File Offset: 0x0002D5DC
		public static void ReversePaths(List<List<ClipperIntPoint>> polys)
		{
			foreach (List<ClipperIntPoint> list in polys)
			{
				list.Reverse();
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0002F428 File Offset: 0x0002D628
		public static bool Orientation(List<ClipperIntPoint> poly)
		{
			return Clipper.Area(poly) >= 0.0;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0002F440 File Offset: 0x0002D640
		private int PointCount(ClipperOutPt pts)
		{
			if (pts == null)
			{
				return 0;
			}
			int num = 0;
			ClipperOutPt clipperOutPt = pts;
			do
			{
				num++;
				clipperOutPt = clipperOutPt.Next;
			}
			while (clipperOutPt != pts);
			return num;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002F468 File Offset: 0x0002D668
		private void BuildResult(List<List<ClipperIntPoint>> polyg)
		{
			polyg.Clear();
			polyg.Capacity = this.m_PolyOuts.Count;
			for (int i = 0; i < this.m_PolyOuts.Count; i++)
			{
				ClipperOutRec clipperOutRec = this.m_PolyOuts[i];
				if (clipperOutRec.Pts != null)
				{
					ClipperOutPt prev = clipperOutRec.Pts.Prev;
					int num = this.PointCount(prev);
					if (num >= 2)
					{
						List<ClipperIntPoint> list = new List<ClipperIntPoint>(num);
						for (int j = 0; j < num; j++)
						{
							list.Add(prev.Pt);
							prev = prev.Prev;
						}
						polyg.Add(list);
					}
				}
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0002F504 File Offset: 0x0002D704
		private void BuildResult2(ClipperPolyTree polytree)
		{
			polytree.Clear();
			polytree.AllPolys.Capacity = this.m_PolyOuts.Count;
			for (int i = 0; i < this.m_PolyOuts.Count; i++)
			{
				ClipperOutRec clipperOutRec = this.m_PolyOuts[i];
				int num = this.PointCount(clipperOutRec.Pts);
				if ((!clipperOutRec.IsOpen || num >= 2) && (clipperOutRec.IsOpen || num >= 3))
				{
					this.FixHoleLinkage(clipperOutRec);
					ClipperPolyNode clipperPolyNode = new ClipperPolyNode();
					polytree.AllPolys.Add(clipperPolyNode);
					clipperOutRec.PolyNode = clipperPolyNode;
					clipperPolyNode.Polygon.Capacity = num;
					ClipperOutPt prev = clipperOutRec.Pts.Prev;
					for (int j = 0; j < num; j++)
					{
						clipperPolyNode.Polygon.Add(prev.Pt);
						prev = prev.Prev;
					}
				}
			}
			polytree.Children.Capacity = this.m_PolyOuts.Count;
			for (int k = 0; k < this.m_PolyOuts.Count; k++)
			{
				ClipperOutRec clipperOutRec2 = this.m_PolyOuts[k];
				if (clipperOutRec2.PolyNode != null)
				{
					if (clipperOutRec2.IsOpen)
					{
						clipperOutRec2.PolyNode.IsOpen = true;
						polytree.AddChild(clipperOutRec2.PolyNode);
					}
					else if (clipperOutRec2.FirstLeft != null && clipperOutRec2.FirstLeft.PolyNode != null)
					{
						clipperOutRec2.FirstLeft.PolyNode.AddChild(clipperOutRec2.PolyNode);
					}
					else
					{
						polytree.AddChild(clipperOutRec2.PolyNode);
					}
				}
			}
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0002F690 File Offset: 0x0002D890
		private void FixupOutPolyline(ClipperOutRec outrec)
		{
			ClipperOutPt clipperOutPt = outrec.Pts;
			ClipperOutPt prev = clipperOutPt.Prev;
			while (clipperOutPt != prev)
			{
				clipperOutPt = clipperOutPt.Next;
				if (clipperOutPt.Pt == clipperOutPt.Prev.Pt)
				{
					if (clipperOutPt == prev)
					{
						prev = clipperOutPt.Prev;
					}
					ClipperOutPt prev2 = clipperOutPt.Prev;
					prev2.Next = clipperOutPt.Next;
					clipperOutPt.Next.Prev = prev2;
					clipperOutPt = prev2;
				}
			}
			if (clipperOutPt == clipperOutPt.Prev)
			{
				outrec.Pts = null;
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0002F70C File Offset: 0x0002D90C
		private void FixupOutPolygon(ClipperOutRec outRec)
		{
			ClipperOutPt clipperOutPt = null;
			outRec.BottomPt = null;
			ClipperOutPt clipperOutPt2 = outRec.Pts;
			bool flag = base.PreserveCollinear || this.StrictlySimple;
			while (clipperOutPt2.Prev != clipperOutPt2 && clipperOutPt2.Prev != clipperOutPt2.Next)
			{
				if (clipperOutPt2.Pt == clipperOutPt2.Next.Pt || clipperOutPt2.Pt == clipperOutPt2.Prev.Pt || (ClipperBase.SlopesEqual(clipperOutPt2.Prev.Pt, clipperOutPt2.Pt, clipperOutPt2.Next.Pt, this.m_UseFullRange) && (!flag || !base.Pt2IsBetweenPt1AndPt3(clipperOutPt2.Prev.Pt, clipperOutPt2.Pt, clipperOutPt2.Next.Pt))))
				{
					clipperOutPt = null;
					clipperOutPt2.Prev.Next = clipperOutPt2.Next;
					clipperOutPt2.Next.Prev = clipperOutPt2.Prev;
					clipperOutPt2 = clipperOutPt2.Prev;
				}
				else
				{
					if (clipperOutPt2 == clipperOutPt)
					{
						outRec.Pts = clipperOutPt2;
						return;
					}
					if (clipperOutPt == null)
					{
						clipperOutPt = clipperOutPt2;
					}
					clipperOutPt2 = clipperOutPt2.Next;
				}
			}
			outRec.Pts = null;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002F828 File Offset: 0x0002DA28
		private ClipperOutPt DupOutPt(ClipperOutPt outPt, bool InsertAfter)
		{
			ClipperOutPt clipperOutPt = new ClipperOutPt
			{
				Pt = outPt.Pt,
				Index = outPt.Index
			};
			if (InsertAfter)
			{
				clipperOutPt.Next = outPt.Next;
				clipperOutPt.Prev = outPt;
				outPt.Next.Prev = clipperOutPt;
				outPt.Next = clipperOutPt;
			}
			else
			{
				clipperOutPt.Prev = outPt.Prev;
				clipperOutPt.Next = outPt;
				outPt.Prev.Next = clipperOutPt;
				outPt.Prev = clipperOutPt;
			}
			return clipperOutPt;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0002F8A8 File Offset: 0x0002DAA8
		private bool GetOverlap(long a1, long a2, long b1, long b2, out long Left, out long Right)
		{
			if (a1 < a2)
			{
				if (b1 < b2)
				{
					Left = Math.Max(a1, b1);
					Right = Math.Min(a2, b2);
				}
				else
				{
					Left = Math.Max(a1, b2);
					Right = Math.Min(a2, b1);
				}
			}
			else if (b1 < b2)
			{
				Left = Math.Max(a2, b1);
				Right = Math.Min(a1, b2);
			}
			else
			{
				Left = Math.Max(a2, b2);
				Right = Math.Min(a1, b1);
			}
			return Left < Right;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0002F928 File Offset: 0x0002DB28
		private bool JoinHorz(ClipperOutPt op1, ClipperOutPt op1b, ClipperOutPt op2, ClipperOutPt op2b, ClipperIntPoint Pt, bool DiscardLeft)
		{
			ClipperDirection clipperDirection = (op1.Pt.X > op1b.Pt.X) ? ClipperDirection.RightToLeft : ClipperDirection.LeftToRight;
			ClipperDirection clipperDirection2 = (op2.Pt.X > op2b.Pt.X) ? ClipperDirection.RightToLeft : ClipperDirection.LeftToRight;
			if (clipperDirection == clipperDirection2)
			{
				return false;
			}
			if (clipperDirection == ClipperDirection.LeftToRight)
			{
				while (op1.Next.Pt.X <= Pt.X && op1.Next.Pt.X >= op1.Pt.X && op1.Next.Pt.Y == Pt.Y)
				{
					op1 = op1.Next;
				}
				if (DiscardLeft && op1.Pt.X != Pt.X)
				{
					op1 = op1.Next;
				}
				op1b = this.DupOutPt(op1, !DiscardLeft);
				if (op1b.Pt != Pt)
				{
					op1 = op1b;
					op1.Pt = Pt;
					op1b = this.DupOutPt(op1, !DiscardLeft);
				}
			}
			else
			{
				while (op1.Next.Pt.X >= Pt.X && op1.Next.Pt.X <= op1.Pt.X && op1.Next.Pt.Y == Pt.Y)
				{
					op1 = op1.Next;
				}
				if (!DiscardLeft && op1.Pt.X != Pt.X)
				{
					op1 = op1.Next;
				}
				op1b = this.DupOutPt(op1, DiscardLeft);
				if (op1b.Pt != Pt)
				{
					op1 = op1b;
					op1.Pt = Pt;
					op1b = this.DupOutPt(op1, DiscardLeft);
				}
			}
			if (clipperDirection2 == ClipperDirection.LeftToRight)
			{
				while (op2.Next.Pt.X <= Pt.X && op2.Next.Pt.X >= op2.Pt.X && op2.Next.Pt.Y == Pt.Y)
				{
					op2 = op2.Next;
				}
				if (DiscardLeft && op2.Pt.X != Pt.X)
				{
					op2 = op2.Next;
				}
				op2b = this.DupOutPt(op2, !DiscardLeft);
				if (op2b.Pt != Pt)
				{
					op2 = op2b;
					op2.Pt = Pt;
					op2b = this.DupOutPt(op2, !DiscardLeft);
				}
			}
			else
			{
				while (op2.Next.Pt.X >= Pt.X && op2.Next.Pt.X <= op2.Pt.X && op2.Next.Pt.Y == Pt.Y)
				{
					op2 = op2.Next;
				}
				if (!DiscardLeft && op2.Pt.X != Pt.X)
				{
					op2 = op2.Next;
				}
				op2b = this.DupOutPt(op2, DiscardLeft);
				if (op2b.Pt != Pt)
				{
					op2 = op2b;
					op2.Pt = Pt;
					op2b = this.DupOutPt(op2, DiscardLeft);
				}
			}
			if (clipperDirection == ClipperDirection.LeftToRight == DiscardLeft)
			{
				op1.Prev = op2;
				op2.Next = op1;
				op1b.Next = op2b;
				op2b.Prev = op1b;
			}
			else
			{
				op1.Next = op2;
				op2.Prev = op1;
				op1b.Prev = op2b;
				op2b.Next = op1b;
			}
			return true;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0002FC8C File Offset: 0x0002DE8C
		private bool JoinPoints(ClipperJoin j, ClipperOutRec outRec1, ClipperOutRec outRec2)
		{
			ClipperOutPt clipperOutPt = j.OutPt1;
			ClipperOutPt clipperOutPt2 = j.OutPt2;
			bool flag = j.OutPt1.Pt.Y == j.OffPt.Y;
			if (flag && j.OffPt == j.OutPt1.Pt && j.OffPt == j.OutPt2.Pt)
			{
				if (outRec1 != outRec2)
				{
					return false;
				}
				ClipperOutPt clipperOutPt3 = j.OutPt1.Next;
				while (clipperOutPt3 != clipperOutPt && clipperOutPt3.Pt == j.OffPt)
				{
					clipperOutPt3 = clipperOutPt3.Next;
				}
				bool flag2 = clipperOutPt3.Pt.Y > j.OffPt.Y;
				ClipperOutPt clipperOutPt4 = j.OutPt2.Next;
				while (clipperOutPt4 != clipperOutPt2 && clipperOutPt4.Pt == j.OffPt)
				{
					clipperOutPt4 = clipperOutPt4.Next;
				}
				bool flag3 = clipperOutPt4.Pt.Y > j.OffPt.Y;
				if (flag2 == flag3)
				{
					return false;
				}
				if (flag2)
				{
					clipperOutPt3 = this.DupOutPt(clipperOutPt, false);
					clipperOutPt4 = this.DupOutPt(clipperOutPt2, true);
					clipperOutPt.Prev = clipperOutPt2;
					clipperOutPt2.Next = clipperOutPt;
					clipperOutPt3.Next = clipperOutPt4;
					clipperOutPt4.Prev = clipperOutPt3;
					j.OutPt1 = clipperOutPt;
					j.OutPt2 = clipperOutPt3;
					return true;
				}
				clipperOutPt3 = this.DupOutPt(clipperOutPt, true);
				clipperOutPt4 = this.DupOutPt(clipperOutPt2, false);
				clipperOutPt.Next = clipperOutPt2;
				clipperOutPt2.Prev = clipperOutPt;
				clipperOutPt3.Prev = clipperOutPt4;
				clipperOutPt4.Next = clipperOutPt3;
				j.OutPt1 = clipperOutPt;
				j.OutPt2 = clipperOutPt3;
				return true;
			}
			else if (flag)
			{
				ClipperOutPt clipperOutPt3 = clipperOutPt;
				while (clipperOutPt.Prev.Pt.Y == clipperOutPt.Pt.Y && clipperOutPt.Prev != clipperOutPt3)
				{
					if (clipperOutPt.Prev == clipperOutPt2)
					{
						break;
					}
					clipperOutPt = clipperOutPt.Prev;
				}
				while (clipperOutPt3.Next.Pt.Y == clipperOutPt3.Pt.Y && clipperOutPt3.Next != clipperOutPt && clipperOutPt3.Next != clipperOutPt2)
				{
					clipperOutPt3 = clipperOutPt3.Next;
				}
				if (clipperOutPt3.Next == clipperOutPt || clipperOutPt3.Next == clipperOutPt2)
				{
					return false;
				}
				ClipperOutPt clipperOutPt4 = clipperOutPt2;
				while (clipperOutPt2.Prev.Pt.Y == clipperOutPt2.Pt.Y && clipperOutPt2.Prev != clipperOutPt4)
				{
					if (clipperOutPt2.Prev == clipperOutPt3)
					{
						break;
					}
					clipperOutPt2 = clipperOutPt2.Prev;
				}
				while (clipperOutPt4.Next.Pt.Y == clipperOutPt4.Pt.Y && clipperOutPt4.Next != clipperOutPt2 && clipperOutPt4.Next != clipperOutPt)
				{
					clipperOutPt4 = clipperOutPt4.Next;
				}
				if (clipperOutPt4.Next == clipperOutPt2 || clipperOutPt4.Next == clipperOutPt)
				{
					return false;
				}
				long num;
				long num2;
				if (!this.GetOverlap(clipperOutPt.Pt.X, clipperOutPt3.Pt.X, clipperOutPt2.Pt.X, clipperOutPt4.Pt.X, out num, out num2))
				{
					return false;
				}
				ClipperIntPoint pt;
				bool discardLeft;
				if (clipperOutPt.Pt.X >= num && clipperOutPt.Pt.X <= num2)
				{
					pt = clipperOutPt.Pt;
					discardLeft = (clipperOutPt.Pt.X > clipperOutPt3.Pt.X);
				}
				else if (clipperOutPt2.Pt.X >= num && clipperOutPt2.Pt.X <= num2)
				{
					pt = clipperOutPt2.Pt;
					discardLeft = (clipperOutPt2.Pt.X > clipperOutPt4.Pt.X);
				}
				else if (clipperOutPt3.Pt.X >= num && clipperOutPt3.Pt.X <= num2)
				{
					pt = clipperOutPt3.Pt;
					discardLeft = (clipperOutPt3.Pt.X > clipperOutPt.Pt.X);
				}
				else
				{
					pt = clipperOutPt4.Pt;
					discardLeft = (clipperOutPt4.Pt.X > clipperOutPt2.Pt.X);
				}
				j.OutPt1 = clipperOutPt;
				j.OutPt2 = clipperOutPt2;
				return this.JoinHorz(clipperOutPt, clipperOutPt3, clipperOutPt2, clipperOutPt4, pt, discardLeft);
			}
			else
			{
				ClipperOutPt clipperOutPt3 = clipperOutPt.Next;
				while (clipperOutPt3.Pt == clipperOutPt.Pt && clipperOutPt3 != clipperOutPt)
				{
					clipperOutPt3 = clipperOutPt3.Next;
				}
				bool flag4 = clipperOutPt3.Pt.Y > clipperOutPt.Pt.Y || !ClipperBase.SlopesEqual(clipperOutPt.Pt, clipperOutPt3.Pt, j.OffPt, this.m_UseFullRange);
				if (flag4)
				{
					clipperOutPt3 = clipperOutPt.Prev;
					while (clipperOutPt3.Pt == clipperOutPt.Pt && clipperOutPt3 != clipperOutPt)
					{
						clipperOutPt3 = clipperOutPt3.Prev;
					}
					if (clipperOutPt3.Pt.Y > clipperOutPt.Pt.Y || !ClipperBase.SlopesEqual(clipperOutPt.Pt, clipperOutPt3.Pt, j.OffPt, this.m_UseFullRange))
					{
						return false;
					}
				}
				ClipperOutPt clipperOutPt4 = clipperOutPt2.Next;
				while (clipperOutPt4.Pt == clipperOutPt2.Pt && clipperOutPt4 != clipperOutPt2)
				{
					clipperOutPt4 = clipperOutPt4.Next;
				}
				bool flag5 = clipperOutPt4.Pt.Y > clipperOutPt2.Pt.Y || !ClipperBase.SlopesEqual(clipperOutPt2.Pt, clipperOutPt4.Pt, j.OffPt, this.m_UseFullRange);
				if (flag5)
				{
					clipperOutPt4 = clipperOutPt2.Prev;
					while (clipperOutPt4.Pt == clipperOutPt2.Pt && clipperOutPt4 != clipperOutPt2)
					{
						clipperOutPt4 = clipperOutPt4.Prev;
					}
					if (clipperOutPt4.Pt.Y > clipperOutPt2.Pt.Y || !ClipperBase.SlopesEqual(clipperOutPt2.Pt, clipperOutPt4.Pt, j.OffPt, this.m_UseFullRange))
					{
						return false;
					}
				}
				if (clipperOutPt3 == clipperOutPt || clipperOutPt4 == clipperOutPt2 || clipperOutPt3 == clipperOutPt4 || (outRec1 == outRec2 && flag4 == flag5))
				{
					return false;
				}
				if (flag4)
				{
					clipperOutPt3 = this.DupOutPt(clipperOutPt, false);
					clipperOutPt4 = this.DupOutPt(clipperOutPt2, true);
					clipperOutPt.Prev = clipperOutPt2;
					clipperOutPt2.Next = clipperOutPt;
					clipperOutPt3.Next = clipperOutPt4;
					clipperOutPt4.Prev = clipperOutPt3;
					j.OutPt1 = clipperOutPt;
					j.OutPt2 = clipperOutPt3;
					return true;
				}
				clipperOutPt3 = this.DupOutPt(clipperOutPt, true);
				clipperOutPt4 = this.DupOutPt(clipperOutPt2, false);
				clipperOutPt.Next = clipperOutPt2;
				clipperOutPt2.Prev = clipperOutPt;
				clipperOutPt3.Prev = clipperOutPt4;
				clipperOutPt4.Next = clipperOutPt3;
				j.OutPt1 = clipperOutPt;
				j.OutPt2 = clipperOutPt3;
				return true;
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0003029C File Offset: 0x0002E49C
		public static int PointInPolygon(ClipperIntPoint pt, List<ClipperIntPoint> path)
		{
			int num = 0;
			int count = path.Count;
			if (count < 3)
			{
				return 0;
			}
			ClipperIntPoint clipperIntPoint = path[0];
			for (int i = 1; i <= count; i++)
			{
				ClipperIntPoint clipperIntPoint2 = (i == count) ? path[0] : path[i];
				if (clipperIntPoint2.Y == pt.Y && (clipperIntPoint2.X == pt.X || (clipperIntPoint.Y == pt.Y && clipperIntPoint2.X > pt.X == clipperIntPoint.X < pt.X)))
				{
					return -1;
				}
				if (clipperIntPoint.Y < pt.Y != clipperIntPoint2.Y < pt.Y)
				{
					if (clipperIntPoint.X >= pt.X)
					{
						if (clipperIntPoint2.X > pt.X)
						{
							num = 1 - num;
						}
						else
						{
							double num2 = (double)(clipperIntPoint.X - pt.X) * (double)(clipperIntPoint2.Y - pt.Y) - (double)(clipperIntPoint2.X - pt.X) * (double)(clipperIntPoint.Y - pt.Y);
							if (num2 == 0.0)
							{
								return -1;
							}
							if (num2 > 0.0 == clipperIntPoint2.Y > clipperIntPoint.Y)
							{
								num = 1 - num;
							}
						}
					}
					else if (clipperIntPoint2.X > pt.X)
					{
						double num3 = (double)(clipperIntPoint.X - pt.X) * (double)(clipperIntPoint2.Y - pt.Y) - (double)(clipperIntPoint2.X - pt.X) * (double)(clipperIntPoint.Y - pt.Y);
						if (num3 == 0.0)
						{
							return -1;
						}
						if (num3 > 0.0 == clipperIntPoint2.Y > clipperIntPoint.Y)
						{
							num = 1 - num;
						}
					}
				}
				clipperIntPoint = clipperIntPoint2;
			}
			return num;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00030478 File Offset: 0x0002E678
		private static int PointInPolygon(ClipperIntPoint pt, ClipperOutPt op)
		{
			int num = 0;
			ClipperOutPt clipperOutPt = op;
			long x = pt.X;
			long y = pt.Y;
			long num2 = op.Pt.X;
			long num3 = op.Pt.Y;
			for (;;)
			{
				op = op.Next;
				long x2 = op.Pt.X;
				long y2 = op.Pt.Y;
				if (y2 == y && (x2 == x || (num3 == y && x2 > x == num2 < x)))
				{
					break;
				}
				if (num3 < y != y2 < y)
				{
					if (num2 >= x)
					{
						if (x2 > x)
						{
							num = 1 - num;
						}
						else
						{
							double num4 = (double)(num2 - x) * (double)(y2 - y) - (double)(x2 - x) * (double)(num3 - y);
							if (num4 == 0.0)
							{
								return -1;
							}
							if (num4 > 0.0 == y2 > num3)
							{
								num = 1 - num;
							}
						}
					}
					else if (x2 > x)
					{
						double num5 = (double)(num2 - x) * (double)(y2 - y) - (double)(x2 - x) * (double)(num3 - y);
						if (num5 == 0.0)
						{
							return -1;
						}
						if (num5 > 0.0 == y2 > num3)
						{
							num = 1 - num;
						}
					}
				}
				num2 = x2;
				num3 = y2;
				if (clipperOutPt == op)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000305AC File Offset: 0x0002E7AC
		private static bool Poly2ContainsPoly1(ClipperOutPt outPt1, ClipperOutPt outPt2)
		{
			ClipperOutPt clipperOutPt = outPt1;
			int num;
			for (;;)
			{
				num = Clipper.PointInPolygon(clipperOutPt.Pt, outPt2);
				if (num >= 0)
				{
					break;
				}
				clipperOutPt = clipperOutPt.Next;
				if (clipperOutPt == outPt1)
				{
					return true;
				}
			}
			return num > 0;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000305E0 File Offset: 0x0002E7E0
		private void FixupFirstLefts1(ClipperOutRec OldOutRec, ClipperOutRec NewOutRec)
		{
			foreach (ClipperOutRec clipperOutRec in this.m_PolyOuts)
			{
				ClipperOutRec clipperOutRec2 = Clipper.ParseFirstLeft(clipperOutRec.FirstLeft);
				if (clipperOutRec.Pts != null && clipperOutRec2 == OldOutRec && Clipper.Poly2ContainsPoly1(clipperOutRec.Pts, NewOutRec.Pts))
				{
					clipperOutRec.FirstLeft = NewOutRec;
				}
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00030660 File Offset: 0x0002E860
		private void FixupFirstLefts2(ClipperOutRec innerOutRec, ClipperOutRec outerOutRec)
		{
			ClipperOutRec firstLeft = outerOutRec.FirstLeft;
			foreach (ClipperOutRec clipperOutRec in this.m_PolyOuts)
			{
				if (clipperOutRec.Pts != null && clipperOutRec != outerOutRec && clipperOutRec != innerOutRec)
				{
					ClipperOutRec clipperOutRec2 = Clipper.ParseFirstLeft(clipperOutRec.FirstLeft);
					if (clipperOutRec2 == firstLeft || clipperOutRec2 == innerOutRec || clipperOutRec2 == outerOutRec)
					{
						if (Clipper.Poly2ContainsPoly1(clipperOutRec.Pts, innerOutRec.Pts))
						{
							clipperOutRec.FirstLeft = innerOutRec;
						}
						else if (Clipper.Poly2ContainsPoly1(clipperOutRec.Pts, outerOutRec.Pts))
						{
							clipperOutRec.FirstLeft = outerOutRec;
						}
						else if (clipperOutRec.FirstLeft == innerOutRec || clipperOutRec.FirstLeft == outerOutRec)
						{
							clipperOutRec.FirstLeft = firstLeft;
						}
					}
				}
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00030734 File Offset: 0x0002E934
		private void FixupFirstLefts3(ClipperOutRec OldOutRec, ClipperOutRec NewOutRec)
		{
			foreach (ClipperOutRec clipperOutRec in this.m_PolyOuts)
			{
				ClipperOutRec clipperOutRec2 = Clipper.ParseFirstLeft(clipperOutRec.FirstLeft);
				if (clipperOutRec.Pts != null && clipperOutRec2 == OldOutRec)
				{
					clipperOutRec.FirstLeft = NewOutRec;
				}
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x000307A0 File Offset: 0x0002E9A0
		private static ClipperOutRec ParseFirstLeft(ClipperOutRec FirstLeft)
		{
			while (FirstLeft != null && FirstLeft.Pts == null)
			{
				FirstLeft = FirstLeft.FirstLeft;
			}
			return FirstLeft;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x000307B8 File Offset: 0x0002E9B8
		private void JoinCommonEdges()
		{
			for (int i = 0; i < this.m_Joins.Count; i++)
			{
				ClipperJoin clipperJoin = this.m_Joins[i];
				ClipperOutRec outRec = this.GetOutRec(clipperJoin.OutPt1.Index);
				ClipperOutRec clipperOutRec = this.GetOutRec(clipperJoin.OutPt2.Index);
				if (outRec.Pts != null && clipperOutRec.Pts != null && !outRec.IsOpen && !clipperOutRec.IsOpen)
				{
					ClipperOutRec clipperOutRec2;
					if (outRec == clipperOutRec)
					{
						clipperOutRec2 = outRec;
					}
					else if (this.OutRec1RightOfOutRec2(outRec, clipperOutRec))
					{
						clipperOutRec2 = clipperOutRec;
					}
					else if (this.OutRec1RightOfOutRec2(clipperOutRec, outRec))
					{
						clipperOutRec2 = outRec;
					}
					else
					{
						clipperOutRec2 = this.GetLowermostRec(outRec, clipperOutRec);
					}
					if (this.JoinPoints(clipperJoin, outRec, clipperOutRec))
					{
						if (outRec == clipperOutRec)
						{
							outRec.Pts = clipperJoin.OutPt1;
							outRec.BottomPt = null;
							clipperOutRec = base.CreateOutRec();
							clipperOutRec.Pts = clipperJoin.OutPt2;
							this.UpdateOutPtIdxs(clipperOutRec);
							if (Clipper.Poly2ContainsPoly1(clipperOutRec.Pts, outRec.Pts))
							{
								clipperOutRec.IsHole = !outRec.IsHole;
								clipperOutRec.FirstLeft = outRec;
								if (this.m_UsingPolyTree)
								{
									this.FixupFirstLefts2(clipperOutRec, outRec);
								}
								if ((clipperOutRec.IsHole ^ this.ReverseSolution) == this.Area(clipperOutRec) > 0.0)
								{
									this.ReversePolyPtLinks(clipperOutRec.Pts);
								}
							}
							else if (Clipper.Poly2ContainsPoly1(outRec.Pts, clipperOutRec.Pts))
							{
								clipperOutRec.IsHole = outRec.IsHole;
								outRec.IsHole = !clipperOutRec.IsHole;
								clipperOutRec.FirstLeft = outRec.FirstLeft;
								outRec.FirstLeft = clipperOutRec;
								if (this.m_UsingPolyTree)
								{
									this.FixupFirstLefts2(outRec, clipperOutRec);
								}
								if ((outRec.IsHole ^ this.ReverseSolution) == this.Area(outRec) > 0.0)
								{
									this.ReversePolyPtLinks(outRec.Pts);
								}
							}
							else
							{
								clipperOutRec.IsHole = outRec.IsHole;
								clipperOutRec.FirstLeft = outRec.FirstLeft;
								if (this.m_UsingPolyTree)
								{
									this.FixupFirstLefts1(outRec, clipperOutRec);
								}
							}
						}
						else
						{
							clipperOutRec.Pts = null;
							clipperOutRec.BottomPt = null;
							clipperOutRec.Idx = outRec.Idx;
							outRec.IsHole = clipperOutRec2.IsHole;
							if (clipperOutRec2 == clipperOutRec)
							{
								outRec.FirstLeft = clipperOutRec.FirstLeft;
							}
							clipperOutRec.FirstLeft = outRec;
							if (this.m_UsingPolyTree)
							{
								this.FixupFirstLefts3(clipperOutRec, outRec);
							}
						}
					}
				}
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00030A1C File Offset: 0x0002EC1C
		private void UpdateOutPtIdxs(ClipperOutRec outrec)
		{
			ClipperOutPt clipperOutPt = outrec.Pts;
			do
			{
				clipperOutPt.Index = outrec.Idx;
				clipperOutPt = clipperOutPt.Prev;
			}
			while (clipperOutPt != outrec.Pts);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00030A4C File Offset: 0x0002EC4C
		private void DoSimplePolygons()
		{
			int i = 0;
			while (i < this.m_PolyOuts.Count)
			{
				ClipperOutRec clipperOutRec = this.m_PolyOuts[i++];
				ClipperOutPt clipperOutPt = clipperOutRec.Pts;
				if (clipperOutPt != null && !clipperOutRec.IsOpen)
				{
					do
					{
						for (ClipperOutPt clipperOutPt2 = clipperOutPt.Next; clipperOutPt2 != clipperOutRec.Pts; clipperOutPt2 = clipperOutPt2.Next)
						{
							if (clipperOutPt.Pt == clipperOutPt2.Pt && clipperOutPt2.Next != clipperOutPt && clipperOutPt2.Prev != clipperOutPt)
							{
								ClipperOutPt prev = clipperOutPt.Prev;
								ClipperOutPt prev2 = clipperOutPt2.Prev;
								clipperOutPt.Prev = prev2;
								prev2.Next = clipperOutPt;
								clipperOutPt2.Prev = prev;
								prev.Next = clipperOutPt2;
								clipperOutRec.Pts = clipperOutPt;
								ClipperOutRec clipperOutRec2 = base.CreateOutRec();
								clipperOutRec2.Pts = clipperOutPt2;
								this.UpdateOutPtIdxs(clipperOutRec2);
								if (Clipper.Poly2ContainsPoly1(clipperOutRec2.Pts, clipperOutRec.Pts))
								{
									clipperOutRec2.IsHole = !clipperOutRec.IsHole;
									clipperOutRec2.FirstLeft = clipperOutRec;
									if (this.m_UsingPolyTree)
									{
										this.FixupFirstLefts2(clipperOutRec2, clipperOutRec);
									}
								}
								else if (Clipper.Poly2ContainsPoly1(clipperOutRec.Pts, clipperOutRec2.Pts))
								{
									clipperOutRec2.IsHole = clipperOutRec.IsHole;
									clipperOutRec.IsHole = !clipperOutRec2.IsHole;
									clipperOutRec2.FirstLeft = clipperOutRec.FirstLeft;
									clipperOutRec.FirstLeft = clipperOutRec2;
									if (this.m_UsingPolyTree)
									{
										this.FixupFirstLefts2(clipperOutRec, clipperOutRec2);
									}
								}
								else
								{
									clipperOutRec2.IsHole = clipperOutRec.IsHole;
									clipperOutRec2.FirstLeft = clipperOutRec.FirstLeft;
									if (this.m_UsingPolyTree)
									{
										this.FixupFirstLefts1(clipperOutRec, clipperOutRec2);
									}
								}
								clipperOutPt2 = clipperOutPt;
							}
						}
						clipperOutPt = clipperOutPt.Next;
					}
					while (clipperOutPt != clipperOutRec.Pts);
				}
			}
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00030C14 File Offset: 0x0002EE14
		public static double Area(List<ClipperIntPoint> poly)
		{
			int count = poly.Count;
			if (count < 3)
			{
				return 0.0;
			}
			double num = 0.0;
			int i = 0;
			int num2 = count - 1;
			while (i < count)
			{
				num += ((double)poly[num2].X + (double)poly[i].X) * ((double)poly[num2].Y - (double)poly[i].Y);
				num2 = i;
				i++;
			}
			return -num * 0.5;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00030C98 File Offset: 0x0002EE98
		internal double Area(ClipperOutRec outRec)
		{
			return this.Area(outRec.Pts);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00030CA8 File Offset: 0x0002EEA8
		internal double Area(ClipperOutPt op)
		{
			ClipperOutPt clipperOutPt = op;
			if (op == null)
			{
				return 0.0;
			}
			double num = 0.0;
			do
			{
				num += (double)(op.Prev.Pt.X + op.Pt.X) * (double)(op.Prev.Pt.Y - op.Pt.Y);
				op = op.Next;
			}
			while (op != clipperOutPt);
			return num * 0.5;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00030D24 File Offset: 0x0002EF24
		public static List<List<ClipperIntPoint>> SimplifyPolygon(List<ClipperIntPoint> poly, ClipperPolyFillType fillType = ClipperPolyFillType.EvenOdd)
		{
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>();
			Clipper clipper = new Clipper(0);
			clipper.StrictlySimple = true;
			clipper.AddPath(poly, ClipperPolyType.Subject, true);
			clipper.Execute(ClipperClipType.Union, list, fillType, fillType);
			return list;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00030D5C File Offset: 0x0002EF5C
		public static List<List<ClipperIntPoint>> SimplifyPolygons(List<List<ClipperIntPoint>> polys, ClipperPolyFillType fillType = ClipperPolyFillType.EvenOdd)
		{
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>();
			Clipper clipper = new Clipper(0);
			clipper.StrictlySimple = true;
			clipper.AddPaths(polys, ClipperPolyType.Subject, true);
			clipper.Execute(ClipperClipType.Union, list, fillType, fillType);
			return list;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00030D94 File Offset: 0x0002EF94
		private static double DistanceFromLineSqrd(ClipperIntPoint pt, ClipperIntPoint ln1, ClipperIntPoint ln2)
		{
			double num = (double)(ln1.Y - ln2.Y);
			double num2 = (double)(ln2.X - ln1.X);
			double num3 = num * (double)ln1.X + num2 * (double)ln1.Y;
			num3 = num * (double)pt.X + num2 * (double)pt.Y - num3;
			return num3 * num3 / (num * num + num2 * num2);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00030DF4 File Offset: 0x0002EFF4
		private static bool SlopesNearCollinear(ClipperIntPoint pt1, ClipperIntPoint pt2, ClipperIntPoint pt3, double distSqrd)
		{
			if (Math.Abs(pt1.X - pt2.X) > Math.Abs(pt1.Y - pt2.Y))
			{
				if (pt1.X > pt2.X == pt1.X < pt3.X)
				{
					return Clipper.DistanceFromLineSqrd(pt1, pt2, pt3) < distSqrd;
				}
				if (pt2.X > pt1.X == pt2.X < pt3.X)
				{
					return Clipper.DistanceFromLineSqrd(pt2, pt1, pt3) < distSqrd;
				}
				return Clipper.DistanceFromLineSqrd(pt3, pt1, pt2) < distSqrd;
			}
			else
			{
				if (pt1.Y > pt2.Y == pt1.Y < pt3.Y)
				{
					return Clipper.DistanceFromLineSqrd(pt1, pt2, pt3) < distSqrd;
				}
				if (pt2.Y > pt1.Y == pt2.Y < pt3.Y)
				{
					return Clipper.DistanceFromLineSqrd(pt2, pt1, pt3) < distSqrd;
				}
				return Clipper.DistanceFromLineSqrd(pt3, pt1, pt2) < distSqrd;
			}
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00030EE8 File Offset: 0x0002F0E8
		private static bool PointsAreClose(ClipperIntPoint pt1, ClipperIntPoint pt2, double distSqrd)
		{
			double num = (double)pt1.X - (double)pt2.X;
			double num2 = (double)pt1.Y - (double)pt2.Y;
			return num * num + num2 * num2 <= distSqrd;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00030F20 File Offset: 0x0002F120
		private static ClipperOutPt ExcludeOp(ClipperOutPt op)
		{
			ClipperOutPt prev = op.Prev;
			prev.Next = op.Next;
			op.Next.Prev = prev;
			prev.Index = 0;
			return prev;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00030F54 File Offset: 0x0002F154
		public static List<ClipperIntPoint> CleanPolygon(List<ClipperIntPoint> path, double distance = 1.415)
		{
			int num = path.Count;
			if (num == 0)
			{
				return new List<ClipperIntPoint>();
			}
			ClipperOutPt[] array = new ClipperOutPt[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new ClipperOutPt();
			}
			for (int j = 0; j < num; j++)
			{
				array[j].Pt = path[j];
				array[j].Next = array[(j + 1) % num];
				array[j].Next.Prev = array[j];
				array[j].Index = 0;
			}
			double distSqrd = distance * distance;
			ClipperOutPt clipperOutPt = array[0];
			while (clipperOutPt.Index == 0 && clipperOutPt.Next != clipperOutPt.Prev)
			{
				if (Clipper.PointsAreClose(clipperOutPt.Pt, clipperOutPt.Prev.Pt, distSqrd))
				{
					clipperOutPt = Clipper.ExcludeOp(clipperOutPt);
					num--;
				}
				else if (Clipper.PointsAreClose(clipperOutPt.Prev.Pt, clipperOutPt.Next.Pt, distSqrd))
				{
					Clipper.ExcludeOp(clipperOutPt.Next);
					clipperOutPt = Clipper.ExcludeOp(clipperOutPt);
					num -= 2;
				}
				else if (Clipper.SlopesNearCollinear(clipperOutPt.Prev.Pt, clipperOutPt.Pt, clipperOutPt.Next.Pt, distSqrd))
				{
					clipperOutPt = Clipper.ExcludeOp(clipperOutPt);
					num--;
				}
				else
				{
					clipperOutPt.Index = 1;
					clipperOutPt = clipperOutPt.Next;
				}
			}
			if (num < 3)
			{
				num = 0;
			}
			List<ClipperIntPoint> list = new List<ClipperIntPoint>(num);
			for (int k = 0; k < num; k++)
			{
				list.Add(clipperOutPt.Pt);
				clipperOutPt = clipperOutPt.Next;
			}
			return list;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x000310D8 File Offset: 0x0002F2D8
		public static List<List<ClipperIntPoint>> CleanPolygons(List<List<ClipperIntPoint>> polys, double distance = 1.415)
		{
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>(polys.Count);
			for (int i = 0; i < polys.Count; i++)
			{
				list.Add(Clipper.CleanPolygon(polys[i], distance));
			}
			return list;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00031118 File Offset: 0x0002F318
		internal static List<List<ClipperIntPoint>> Minkowski(List<ClipperIntPoint> pattern, List<ClipperIntPoint> path, bool IsSum, bool IsClosed)
		{
			int num = IsClosed ? 1 : 0;
			int count = pattern.Count;
			int count2 = path.Count;
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>(count2);
			if (IsSum)
			{
				for (int i = 0; i < count2; i++)
				{
					List<ClipperIntPoint> list2 = new List<ClipperIntPoint>(count);
					foreach (ClipperIntPoint clipperIntPoint in pattern)
					{
						list2.Add(new ClipperIntPoint(path[i].X + clipperIntPoint.X, path[i].Y + clipperIntPoint.Y));
					}
					list.Add(list2);
				}
			}
			else
			{
				for (int j = 0; j < count2; j++)
				{
					List<ClipperIntPoint> list3 = new List<ClipperIntPoint>(count);
					foreach (ClipperIntPoint clipperIntPoint2 in pattern)
					{
						list3.Add(new ClipperIntPoint(path[j].X - clipperIntPoint2.X, path[j].Y - clipperIntPoint2.Y));
					}
					list.Add(list3);
				}
			}
			List<List<ClipperIntPoint>> list4 = new List<List<ClipperIntPoint>>((count2 + num) * (count + 1));
			for (int k = 0; k < count2 - 1 + num; k++)
			{
				for (int l = 0; l < count; l++)
				{
					List<ClipperIntPoint> list5 = new List<ClipperIntPoint>(4);
					list5.Add(list[k % count2][l % count]);
					list5.Add(list[(k + 1) % count2][l % count]);
					list5.Add(list[(k + 1) % count2][(l + 1) % count]);
					list5.Add(list[k % count2][(l + 1) % count]);
					List<ClipperIntPoint> list6 = list5;
					if (!Clipper.Orientation(list6))
					{
						list6.Reverse();
					}
					list4.Add(list6);
				}
			}
			return list4;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0003133C File Offset: 0x0002F53C
		public static List<List<ClipperIntPoint>> MinkowskiSum(List<ClipperIntPoint> pattern, List<ClipperIntPoint> path, bool pathIsClosed)
		{
			List<List<ClipperIntPoint>> list = Clipper.Minkowski(pattern, path, true, pathIsClosed);
			Clipper clipper = new Clipper(0);
			clipper.AddPaths(list, ClipperPolyType.Subject, true);
			clipper.Execute(ClipperClipType.Union, list, ClipperPolyFillType.NonZero, ClipperPolyFillType.NonZero);
			return list;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00031370 File Offset: 0x0002F570
		private static List<ClipperIntPoint> TranslatePath(List<ClipperIntPoint> path, ClipperIntPoint delta)
		{
			List<ClipperIntPoint> list = new List<ClipperIntPoint>(path.Count);
			for (int i = 0; i < path.Count; i++)
			{
				list.Add(new ClipperIntPoint(path[i].X + delta.X, path[i].Y + delta.Y));
			}
			return list;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x000313CC File Offset: 0x0002F5CC
		public static List<List<ClipperIntPoint>> MinkowskiSum(List<ClipperIntPoint> pattern, List<List<ClipperIntPoint>> paths, bool pathIsClosed)
		{
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>();
			Clipper clipper = new Clipper(0);
			for (int i = 0; i < paths.Count; i++)
			{
				List<List<ClipperIntPoint>> ppg = Clipper.Minkowski(pattern, paths[i], true, pathIsClosed);
				clipper.AddPaths(ppg, ClipperPolyType.Subject, true);
				if (pathIsClosed)
				{
					List<ClipperIntPoint> pg = Clipper.TranslatePath(paths[i], pattern[0]);
					clipper.AddPath(pg, ClipperPolyType.Clip, true);
				}
			}
			clipper.Execute(ClipperClipType.Union, list, ClipperPolyFillType.NonZero, ClipperPolyFillType.NonZero);
			return list;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00031440 File Offset: 0x0002F640
		public static List<List<ClipperIntPoint>> MinkowskiDiff(List<ClipperIntPoint> poly1, List<ClipperIntPoint> poly2)
		{
			List<List<ClipperIntPoint>> list = Clipper.Minkowski(poly1, poly2, false, true);
			Clipper clipper = new Clipper(0);
			clipper.AddPaths(list, ClipperPolyType.Subject, true);
			clipper.Execute(ClipperClipType.Union, list, ClipperPolyFillType.NonZero, ClipperPolyFillType.NonZero);
			return list;
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00031474 File Offset: 0x0002F674
		public static List<List<ClipperIntPoint>> PolyTreeToPaths(ClipperPolyTree polytree)
		{
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>
			{
				Capacity = polytree.Total
			};
			Clipper.AddPolyNodeToPaths(polytree, Clipper.NodeType.ntAny, list);
			return list;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0003149C File Offset: 0x0002F69C
		internal static void AddPolyNodeToPaths(ClipperPolyNode polynode, Clipper.NodeType nt, List<List<ClipperIntPoint>> paths)
		{
			bool flag = true;
			if (nt != Clipper.NodeType.ntOpen)
			{
				if (nt == Clipper.NodeType.ntClosed)
				{
					flag = !polynode.IsOpen;
				}
				if (polynode.Polygon.Count > 0 && flag)
				{
					paths.Add(polynode.Polygon);
				}
				foreach (ClipperPolyNode polynode2 in polynode.Children)
				{
					Clipper.AddPolyNodeToPaths(polynode2, nt, paths);
				}
				return;
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00031524 File Offset: 0x0002F724
		public static List<List<ClipperIntPoint>> OpenPathsFromPolyTree(ClipperPolyTree polytree)
		{
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>
			{
				Capacity = polytree.ChildCount
			};
			for (int i = 0; i < polytree.ChildCount; i++)
			{
				if (polytree.Children[i].IsOpen)
				{
					list.Add(polytree.Children[i].Polygon);
				}
			}
			return list;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00031580 File Offset: 0x0002F780
		public static List<List<ClipperIntPoint>> ClosedPathsFromPolyTree(ClipperPolyTree polytree)
		{
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>
			{
				Capacity = polytree.Total
			};
			Clipper.AddPolyNodeToPaths(polytree, Clipper.NodeType.ntClosed, list);
			return list;
		}

		// Token: 0x04000485 RID: 1157
		public const int ioReverseSolution = 1;

		// Token: 0x04000486 RID: 1158
		public const int ioStrictlySimple = 2;

		// Token: 0x04000487 RID: 1159
		public const int ioPreserveCollinear = 4;

		// Token: 0x04000488 RID: 1160
		private ClipperClipType m_ClipType;

		// Token: 0x04000489 RID: 1161
		private ClipperMaxima m_Maxima;

		// Token: 0x0400048A RID: 1162
		private ClipperTEdge m_SortedEdges;

		// Token: 0x0400048B RID: 1163
		private readonly List<ClipperIntersectNode> m_IntersectList;

		// Token: 0x0400048C RID: 1164
		private readonly IComparer<ClipperIntersectNode> m_IntersectNodeComparer;

		// Token: 0x0400048D RID: 1165
		private bool m_ExecuteLocked;

		// Token: 0x0400048E RID: 1166
		private ClipperPolyFillType m_ClipFillType;

		// Token: 0x0400048F RID: 1167
		private ClipperPolyFillType m_SubjFillType;

		// Token: 0x04000490 RID: 1168
		private readonly List<ClipperJoin> m_Joins;

		// Token: 0x04000491 RID: 1169
		private readonly List<ClipperJoin> m_GhostJoins;

		// Token: 0x04000492 RID: 1170
		private bool m_UsingPolyTree;

		// Token: 0x0200027C RID: 636
		internal enum NodeType
		{
			// Token: 0x040008A7 RID: 2215
			ntAny,
			// Token: 0x040008A8 RID: 2216
			ntOpen,
			// Token: 0x040008A9 RID: 2217
			ntClosed
		}
	}
}
