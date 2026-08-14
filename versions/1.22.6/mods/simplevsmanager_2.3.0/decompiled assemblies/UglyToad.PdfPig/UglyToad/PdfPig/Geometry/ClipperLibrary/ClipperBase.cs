using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x0200018E RID: 398
	internal class ClipperBase
	{
		// Token: 0x060009DC RID: 2524 RVA: 0x000315A8 File Offset: 0x0002F7A8
		internal static bool NearZero(double val)
		{
			return val > -1E-20 && val < 1E-20;
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x000315C4 File Offset: 0x0002F7C4
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x000315CC File Offset: 0x0002F7CC
		public bool PreserveCollinear { get; set; }

		// Token: 0x060009DF RID: 2527 RVA: 0x000315D8 File Offset: 0x0002F7D8
		public void Swap(ref long val1, ref long val2)
		{
			long num = val1;
			val1 = val2;
			val2 = num;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x000315EF File Offset: 0x0002F7EF
		internal static bool IsHorizontal(ClipperTEdge e)
		{
			return e.Delta.Y == 0L;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00031600 File Offset: 0x0002F800
		internal bool PointIsVertex(ClipperIntPoint pt, ClipperOutPt pp)
		{
			ClipperOutPt clipperOutPt = pp;
			while (!(clipperOutPt.Pt == pt))
			{
				clipperOutPt = clipperOutPt.Next;
				if (clipperOutPt == pp)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0003162C File Offset: 0x0002F82C
		internal bool PointOnLineSegment(ClipperIntPoint pt, ClipperIntPoint linePt1, ClipperIntPoint linePt2, bool UseFullRange)
		{
			if (UseFullRange)
			{
				return (pt.X == linePt1.X && pt.Y == linePt1.Y) || (pt.X == linePt2.X && pt.Y == linePt2.Y) || (pt.X > linePt1.X == pt.X < linePt2.X && pt.Y > linePt1.Y == pt.Y < linePt2.Y && ClipperInt128.Int128Mul(pt.X - linePt1.X, linePt2.Y - linePt1.Y) == ClipperInt128.Int128Mul(linePt2.X - linePt1.X, pt.Y - linePt1.Y));
			}
			return (pt.X == linePt1.X && pt.Y == linePt1.Y) || (pt.X == linePt2.X && pt.Y == linePt2.Y) || (pt.X > linePt1.X == pt.X < linePt2.X && pt.Y > linePt1.Y == pt.Y < linePt2.Y && (pt.X - linePt1.X) * (linePt2.Y - linePt1.Y) == (linePt2.X - linePt1.X) * (pt.Y - linePt1.Y));
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x000317B8 File Offset: 0x0002F9B8
		internal bool PointOnPolygon(ClipperIntPoint pt, ClipperOutPt pp, bool UseFullRange)
		{
			ClipperOutPt clipperOutPt = pp;
			while (!this.PointOnLineSegment(pt, clipperOutPt.Pt, clipperOutPt.Next.Pt, UseFullRange))
			{
				clipperOutPt = clipperOutPt.Next;
				if (clipperOutPt == pp)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x000317F0 File Offset: 0x0002F9F0
		internal static bool SlopesEqual(ClipperTEdge e1, ClipperTEdge e2, bool UseFullRange)
		{
			if (UseFullRange)
			{
				return ClipperInt128.Int128Mul(e1.Delta.Y, e2.Delta.X) == ClipperInt128.Int128Mul(e1.Delta.X, e2.Delta.Y);
			}
			return e1.Delta.Y * e2.Delta.X == e1.Delta.X * e2.Delta.Y;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0003186C File Offset: 0x0002FA6C
		internal static bool SlopesEqual(ClipperIntPoint pt1, ClipperIntPoint pt2, ClipperIntPoint pt3, bool UseFullRange)
		{
			if (UseFullRange)
			{
				return ClipperInt128.Int128Mul(pt1.Y - pt2.Y, pt2.X - pt3.X) == ClipperInt128.Int128Mul(pt1.X - pt2.X, pt2.Y - pt3.Y);
			}
			return (pt1.Y - pt2.Y) * (pt2.X - pt3.X) - (pt1.X - pt2.X) * (pt2.Y - pt3.Y) == 0L;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x000318FC File Offset: 0x0002FAFC
		internal static bool SlopesEqual(ClipperIntPoint pt1, ClipperIntPoint pt2, ClipperIntPoint pt3, ClipperIntPoint pt4, bool UseFullRange)
		{
			if (UseFullRange)
			{
				return ClipperInt128.Int128Mul(pt1.Y - pt2.Y, pt3.X - pt4.X) == ClipperInt128.Int128Mul(pt1.X - pt2.X, pt3.Y - pt4.Y);
			}
			return (pt1.Y - pt2.Y) * (pt3.X - pt4.X) - (pt1.X - pt2.X) * (pt3.Y - pt4.Y) == 0L;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0003198C File Offset: 0x0002FB8C
		internal ClipperBase()
		{
			this.m_MinimaList = null;
			this.m_CurrentLM = null;
			this.m_UseFullRange = false;
			this.m_HasOpenPaths = false;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x000319BC File Offset: 0x0002FBBC
		public virtual void Clear()
		{
			this.DisposeLocalMinimaList();
			for (int i = 0; i < this.m_edges.Count; i++)
			{
				for (int j = 0; j < this.m_edges[i].Count; j++)
				{
					this.m_edges[i][j] = null;
				}
				this.m_edges[i].Clear();
			}
			this.m_edges.Clear();
			this.m_UseFullRange = false;
			this.m_HasOpenPaths = false;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00031A40 File Offset: 0x0002FC40
		private void DisposeLocalMinimaList()
		{
			while (this.m_MinimaList != null)
			{
				ClipperLocalMinima next = this.m_MinimaList.Next;
				this.m_MinimaList = null;
				this.m_MinimaList = next;
			}
			this.m_CurrentLM = null;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00031A78 File Offset: 0x0002FC78
		private void RangeTest(ClipperIntPoint Pt, ref bool useFullRange)
		{
			if (useFullRange)
			{
				if (Pt.X > 4611686018427387903L || Pt.Y > 4611686018427387903L || -Pt.X > 4611686018427387903L || -Pt.Y > 4611686018427387903L)
				{
					throw new ClipperException("Coordinate outside allowed range");
				}
			}
			else if (Pt.X > 1073741823L || Pt.Y > 1073741823L || -Pt.X > 1073741823L || -Pt.Y > 1073741823L)
			{
				useFullRange = true;
				this.RangeTest(Pt, ref useFullRange);
			}
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00031B1F File Offset: 0x0002FD1F
		private void InitEdge(ClipperTEdge e, ClipperTEdge eNext, ClipperTEdge ePrev, ClipperIntPoint pt)
		{
			e.Next = eNext;
			e.Prev = ePrev;
			e.Curr = pt;
			e.OutIdx = -1;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00031B40 File Offset: 0x0002FD40
		private void InitEdge2(ClipperTEdge e, ClipperPolyType polyType)
		{
			if (e.Curr.Y >= e.Next.Curr.Y)
			{
				e.Bot = e.Curr;
				e.Top = e.Next.Curr;
			}
			else
			{
				e.Top = e.Curr;
				e.Bot = e.Next.Curr;
			}
			this.SetDx(e);
			e.PolyTyp = polyType;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00031BB4 File Offset: 0x0002FDB4
		private ClipperTEdge FindNextLocMin(ClipperTEdge E)
		{
			ClipperTEdge clipperTEdge;
			for (;;)
			{
				if (!(E.Bot != E.Prev.Bot) && !(E.Curr == E.Top))
				{
					if (E.Dx != -3.4E+38 && E.Prev.Dx != -3.4E+38)
					{
						break;
					}
					while (E.Prev.Dx == -3.4E+38)
					{
						E = E.Prev;
					}
					clipperTEdge = E;
					while (E.Dx == -3.4E+38)
					{
						E = E.Next;
					}
					if (E.Top.Y != E.Prev.Bot.Y)
					{
						goto Block_7;
					}
				}
				else
				{
					E = E.Next;
				}
			}
			return E;
			Block_7:
			if (clipperTEdge.Prev.Bot.X < E.Bot.X)
			{
				E = clipperTEdge;
			}
			return E;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00031C9C File Offset: 0x0002FE9C
		private ClipperTEdge ProcessBound(ClipperTEdge E, bool LeftBoundIsForward)
		{
			ClipperTEdge clipperTEdge = E;
			if (clipperTEdge.OutIdx == -2)
			{
				E = clipperTEdge;
				if (LeftBoundIsForward)
				{
					while (E.Top.Y == E.Next.Bot.Y)
					{
						E = E.Next;
					}
					while (E != clipperTEdge)
					{
						if (E.Dx != -3.4E+38)
						{
							break;
						}
						E = E.Prev;
					}
				}
				else
				{
					while (E.Top.Y == E.Prev.Bot.Y)
					{
						E = E.Prev;
					}
					while (E != clipperTEdge && E.Dx == -3.4E+38)
					{
						E = E.Next;
					}
				}
				if (E == clipperTEdge)
				{
					if (LeftBoundIsForward)
					{
						clipperTEdge = E.Next;
					}
					else
					{
						clipperTEdge = E.Prev;
					}
				}
				else
				{
					if (LeftBoundIsForward)
					{
						E = clipperTEdge.Next;
					}
					else
					{
						E = clipperTEdge.Prev;
					}
					ClipperLocalMinima newLm = new ClipperLocalMinima
					{
						Next = null,
						Y = E.Bot.Y,
						LeftBound = null,
						RightBound = E
					};
					E.WindDelta = 0;
					clipperTEdge = this.ProcessBound(E, LeftBoundIsForward);
					this.InsertLocalMinima(newLm);
				}
				return clipperTEdge;
			}
			ClipperTEdge clipperTEdge2;
			if (E.Dx == -3.4E+38)
			{
				if (LeftBoundIsForward)
				{
					clipperTEdge2 = E.Prev;
				}
				else
				{
					clipperTEdge2 = E.Next;
				}
				if (clipperTEdge2.Dx == -3.4E+38)
				{
					if (clipperTEdge2.Bot.X != E.Bot.X && clipperTEdge2.Top.X != E.Bot.X)
					{
						this.ReverseHorizontal(E);
					}
				}
				else if (clipperTEdge2.Bot.X != E.Bot.X)
				{
					this.ReverseHorizontal(E);
				}
			}
			clipperTEdge2 = E;
			if (LeftBoundIsForward)
			{
				while (clipperTEdge.Top.Y == clipperTEdge.Next.Bot.Y && clipperTEdge.Next.OutIdx != -2)
				{
					clipperTEdge = clipperTEdge.Next;
				}
				if (clipperTEdge.Dx == -3.4E+38 && clipperTEdge.Next.OutIdx != -2)
				{
					ClipperTEdge clipperTEdge3 = clipperTEdge;
					while (clipperTEdge3.Prev.Dx == -3.4E+38)
					{
						clipperTEdge3 = clipperTEdge3.Prev;
					}
					if (clipperTEdge3.Prev.Top.X > clipperTEdge.Next.Top.X)
					{
						clipperTEdge = clipperTEdge3.Prev;
					}
				}
				while (E != clipperTEdge)
				{
					E.NextInLML = E.Next;
					if (E.Dx == -3.4E+38 && E != clipperTEdge2 && E.Bot.X != E.Prev.Top.X)
					{
						this.ReverseHorizontal(E);
					}
					E = E.Next;
				}
				if (E.Dx == -3.4E+38 && E != clipperTEdge2 && E.Bot.X != E.Prev.Top.X)
				{
					this.ReverseHorizontal(E);
				}
				clipperTEdge = clipperTEdge.Next;
			}
			else
			{
				while (clipperTEdge.Top.Y == clipperTEdge.Prev.Bot.Y && clipperTEdge.Prev.OutIdx != -2)
				{
					clipperTEdge = clipperTEdge.Prev;
				}
				if (clipperTEdge.Dx == -3.4E+38 && clipperTEdge.Prev.OutIdx != -2)
				{
					ClipperTEdge clipperTEdge3 = clipperTEdge;
					while (clipperTEdge3.Next.Dx == -3.4E+38)
					{
						clipperTEdge3 = clipperTEdge3.Next;
					}
					if (clipperTEdge3.Next.Top.X == clipperTEdge.Prev.Top.X || clipperTEdge3.Next.Top.X > clipperTEdge.Prev.Top.X)
					{
						clipperTEdge = clipperTEdge3.Next;
					}
				}
				while (E != clipperTEdge)
				{
					E.NextInLML = E.Prev;
					if (E.Dx == -3.4E+38 && E != clipperTEdge2 && E.Bot.X != E.Next.Top.X)
					{
						this.ReverseHorizontal(E);
					}
					E = E.Prev;
				}
				if (E.Dx == -3.4E+38 && E != clipperTEdge2 && E.Bot.X != E.Next.Top.X)
				{
					this.ReverseHorizontal(E);
				}
				clipperTEdge = clipperTEdge.Prev;
			}
			return clipperTEdge;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x000320E8 File Offset: 0x000302E8
		public bool AddPath(List<ClipperIntPoint> pg, ClipperPolyType polyType, bool Closed)
		{
			if (!Closed && polyType == ClipperPolyType.Clip)
			{
				throw new ClipperException("AddPath: Open paths must be subject.");
			}
			int i = pg.Count - 1;
			if (Closed)
			{
				while (i > 0)
				{
					if (!(pg[i] == pg[0]))
					{
						break;
					}
					i--;
				}
			}
			while (i > 0 && pg[i] == pg[i - 1])
			{
				i--;
			}
			if ((Closed && i < 2) || (!Closed && i < 1))
			{
				return false;
			}
			List<ClipperTEdge> list = new List<ClipperTEdge>(i + 1);
			for (int j = 0; j <= i; j++)
			{
				list.Add(new ClipperTEdge());
			}
			bool flag = true;
			list[1].Curr = pg[1];
			this.RangeTest(pg[0], ref this.m_UseFullRange);
			this.RangeTest(pg[i], ref this.m_UseFullRange);
			this.InitEdge(list[0], list[1], list[i], pg[0]);
			this.InitEdge(list[i], list[0], list[i - 1], pg[i]);
			for (int k = i - 1; k >= 1; k--)
			{
				this.RangeTest(pg[k], ref this.m_UseFullRange);
				this.InitEdge(list[k], list[k + 1], list[k - 1], pg[k]);
			}
			ClipperTEdge clipperTEdge = list[0];
			ClipperTEdge clipperTEdge2 = clipperTEdge;
			ClipperTEdge clipperTEdge3 = clipperTEdge;
			for (;;)
			{
				if (clipperTEdge2.Curr == clipperTEdge2.Next.Curr && (Closed || clipperTEdge2.Next != clipperTEdge))
				{
					if (clipperTEdge2 == clipperTEdge2.Next)
					{
						break;
					}
					if (clipperTEdge2 == clipperTEdge)
					{
						clipperTEdge = clipperTEdge2.Next;
					}
					clipperTEdge2 = this.RemoveEdge(clipperTEdge2);
					clipperTEdge3 = clipperTEdge2;
				}
				else
				{
					if (clipperTEdge2.Prev == clipperTEdge2.Next)
					{
						break;
					}
					if (Closed && ClipperBase.SlopesEqual(clipperTEdge2.Prev.Curr, clipperTEdge2.Curr, clipperTEdge2.Next.Curr, this.m_UseFullRange) && (!this.PreserveCollinear || !this.Pt2IsBetweenPt1AndPt3(clipperTEdge2.Prev.Curr, clipperTEdge2.Curr, clipperTEdge2.Next.Curr)))
					{
						if (clipperTEdge2 == clipperTEdge)
						{
							clipperTEdge = clipperTEdge2.Next;
						}
						clipperTEdge2 = this.RemoveEdge(clipperTEdge2);
						clipperTEdge2 = clipperTEdge2.Prev;
						clipperTEdge3 = clipperTEdge2;
					}
					else
					{
						clipperTEdge2 = clipperTEdge2.Next;
						if (clipperTEdge2 == clipperTEdge3 || (!Closed && clipperTEdge2.Next == clipperTEdge))
						{
							break;
						}
					}
				}
			}
			if ((!Closed && clipperTEdge2 == clipperTEdge2.Next) || (Closed && clipperTEdge2.Prev == clipperTEdge2.Next))
			{
				return false;
			}
			if (!Closed)
			{
				this.m_HasOpenPaths = true;
				clipperTEdge.Prev.OutIdx = -2;
			}
			clipperTEdge2 = clipperTEdge;
			do
			{
				this.InitEdge2(clipperTEdge2, polyType);
				clipperTEdge2 = clipperTEdge2.Next;
				if (flag && clipperTEdge2.Curr.Y != clipperTEdge.Curr.Y)
				{
					flag = false;
				}
			}
			while (clipperTEdge2 != clipperTEdge);
			if (!flag)
			{
				this.m_edges.Add(list);
				ClipperTEdge clipperTEdge4 = null;
				if (clipperTEdge2.Prev.Bot == clipperTEdge2.Prev.Top)
				{
					clipperTEdge2 = clipperTEdge2.Next;
				}
				for (;;)
				{
					clipperTEdge2 = this.FindNextLocMin(clipperTEdge2);
					if (clipperTEdge2 == clipperTEdge4)
					{
						break;
					}
					if (clipperTEdge4 == null)
					{
						clipperTEdge4 = clipperTEdge2;
					}
					ClipperLocalMinima clipperLocalMinima = new ClipperLocalMinima
					{
						Next = null,
						Y = clipperTEdge2.Bot.Y
					};
					bool flag2;
					if (clipperTEdge2.Dx < clipperTEdge2.Prev.Dx)
					{
						clipperLocalMinima.LeftBound = clipperTEdge2.Prev;
						clipperLocalMinima.RightBound = clipperTEdge2;
						flag2 = false;
					}
					else
					{
						clipperLocalMinima.LeftBound = clipperTEdge2;
						clipperLocalMinima.RightBound = clipperTEdge2.Prev;
						flag2 = true;
					}
					clipperLocalMinima.LeftBound.Side = ClipperEdgeSide.Left;
					clipperLocalMinima.RightBound.Side = ClipperEdgeSide.Right;
					if (!Closed)
					{
						clipperLocalMinima.LeftBound.WindDelta = 0;
					}
					else if (clipperLocalMinima.LeftBound.Next == clipperLocalMinima.RightBound)
					{
						clipperLocalMinima.LeftBound.WindDelta = -1;
					}
					else
					{
						clipperLocalMinima.LeftBound.WindDelta = 1;
					}
					clipperLocalMinima.RightBound.WindDelta = -clipperLocalMinima.LeftBound.WindDelta;
					clipperTEdge2 = this.ProcessBound(clipperLocalMinima.LeftBound, flag2);
					if (clipperTEdge2.OutIdx == -2)
					{
						clipperTEdge2 = this.ProcessBound(clipperTEdge2, flag2);
					}
					ClipperTEdge clipperTEdge5 = this.ProcessBound(clipperLocalMinima.RightBound, !flag2);
					if (clipperTEdge5.OutIdx == -2)
					{
						clipperTEdge5 = this.ProcessBound(clipperTEdge5, !flag2);
					}
					if (clipperLocalMinima.LeftBound.OutIdx == -2)
					{
						clipperLocalMinima.LeftBound = null;
					}
					else if (clipperLocalMinima.RightBound.OutIdx == -2)
					{
						clipperLocalMinima.RightBound = null;
					}
					this.InsertLocalMinima(clipperLocalMinima);
					if (!flag2)
					{
						clipperTEdge2 = clipperTEdge5;
					}
				}
				return true;
			}
			if (Closed)
			{
				return false;
			}
			clipperTEdge2.Prev.OutIdx = -2;
			ClipperLocalMinima clipperLocalMinima2 = new ClipperLocalMinima
			{
				Next = null,
				Y = clipperTEdge2.Bot.Y,
				LeftBound = null,
				RightBound = clipperTEdge2
			};
			clipperLocalMinima2.RightBound.Side = ClipperEdgeSide.Right;
			clipperLocalMinima2.RightBound.WindDelta = 0;
			for (;;)
			{
				if (clipperTEdge2.Bot.X != clipperTEdge2.Prev.Top.X)
				{
					this.ReverseHorizontal(clipperTEdge2);
				}
				if (clipperTEdge2.Next.OutIdx == -2)
				{
					break;
				}
				clipperTEdge2.NextInLML = clipperTEdge2.Next;
				clipperTEdge2 = clipperTEdge2.Next;
			}
			this.InsertLocalMinima(clipperLocalMinima2);
			this.m_edges.Add(list);
			return true;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00032688 File Offset: 0x00030888
		public bool AddPaths(List<List<ClipperIntPoint>> ppg, ClipperPolyType polyType, bool closed)
		{
			bool result = false;
			for (int i = 0; i < ppg.Count; i++)
			{
				if (this.AddPath(ppg[i], polyType, closed))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x000326BC File Offset: 0x000308BC
		internal bool Pt2IsBetweenPt1AndPt3(ClipperIntPoint pt1, ClipperIntPoint pt2, ClipperIntPoint pt3)
		{
			if (pt1 == pt3 || pt1 == pt2 || pt3 == pt2)
			{
				return false;
			}
			if (pt1.X != pt3.X)
			{
				return pt2.X > pt1.X == pt2.X < pt3.X;
			}
			return pt2.Y > pt1.Y == pt2.Y < pt3.Y;
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00032731 File Offset: 0x00030931
		private ClipperTEdge RemoveEdge(ClipperTEdge e)
		{
			e.Prev.Next = e.Next;
			e.Next.Prev = e.Prev;
			ClipperTEdge next = e.Next;
			e.Prev = null;
			return next;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00032764 File Offset: 0x00030964
		private void SetDx(ClipperTEdge e)
		{
			e.Delta.X = e.Top.X - e.Bot.X;
			e.Delta.Y = e.Top.Y - e.Bot.Y;
			if (e.Delta.Y == 0L)
			{
				e.Dx = -3.4E+38;
				return;
			}
			e.Dx = (double)e.Delta.X / (double)e.Delta.Y;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x000327F4 File Offset: 0x000309F4
		private void InsertLocalMinima(ClipperLocalMinima newLm)
		{
			if (this.m_MinimaList == null)
			{
				this.m_MinimaList = newLm;
				return;
			}
			if (newLm.Y >= this.m_MinimaList.Y)
			{
				newLm.Next = this.m_MinimaList;
				this.m_MinimaList = newLm;
				return;
			}
			ClipperLocalMinima clipperLocalMinima = this.m_MinimaList;
			while (clipperLocalMinima.Next != null && newLm.Y < clipperLocalMinima.Next.Y)
			{
				clipperLocalMinima = clipperLocalMinima.Next;
			}
			newLm.Next = clipperLocalMinima.Next;
			clipperLocalMinima.Next = newLm;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00032876 File Offset: 0x00030A76
		internal bool PopLocalMinima(long Y, out ClipperLocalMinima current)
		{
			current = this.m_CurrentLM;
			if (this.m_CurrentLM != null && this.m_CurrentLM.Y == Y)
			{
				this.m_CurrentLM = this.m_CurrentLM.Next;
				return true;
			}
			return false;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000328AA File Offset: 0x00030AAA
		private void ReverseHorizontal(ClipperTEdge e)
		{
			this.Swap(ref e.Top.X, ref e.Bot.X);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x000328C8 File Offset: 0x00030AC8
		internal virtual void Reset()
		{
			this.m_CurrentLM = this.m_MinimaList;
			if (this.m_CurrentLM == null)
			{
				return;
			}
			this.m_Scanbeam = null;
			for (ClipperLocalMinima clipperLocalMinima = this.m_MinimaList; clipperLocalMinima != null; clipperLocalMinima = clipperLocalMinima.Next)
			{
				this.InsertScanbeam(clipperLocalMinima.Y);
				ClipperTEdge clipperTEdge = clipperLocalMinima.LeftBound;
				if (clipperTEdge != null)
				{
					clipperTEdge.Curr = clipperTEdge.Bot;
					clipperTEdge.OutIdx = -1;
				}
				clipperTEdge = clipperLocalMinima.RightBound;
				if (clipperTEdge != null)
				{
					clipperTEdge.Curr = clipperTEdge.Bot;
					clipperTEdge.OutIdx = -1;
				}
			}
			this.m_ActiveEdges = null;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00032954 File Offset: 0x00030B54
		public static ClipperIntRect GetBounds(List<List<ClipperIntPoint>> paths)
		{
			int i = 0;
			int count = paths.Count;
			while (i < count && paths[i].Count == 0)
			{
				i++;
			}
			if (i == count)
			{
				return new ClipperIntRect(0L, 0L, 0L, 0L);
			}
			ClipperIntRect clipperIntRect = new ClipperIntRect
			{
				Left = paths[i][0].X
			};
			clipperIntRect.Right = clipperIntRect.Left;
			clipperIntRect.Top = paths[i][0].Y;
			clipperIntRect.Bottom = clipperIntRect.Top;
			while (i < count)
			{
				for (int j = 0; j < paths[i].Count; j++)
				{
					if (paths[i][j].X < clipperIntRect.Left)
					{
						clipperIntRect.Left = paths[i][j].X;
					}
					else if (paths[i][j].X > clipperIntRect.Right)
					{
						clipperIntRect.Right = paths[i][j].X;
					}
					if (paths[i][j].Y < clipperIntRect.Top)
					{
						clipperIntRect.Top = paths[i][j].Y;
					}
					else if (paths[i][j].Y > clipperIntRect.Bottom)
					{
						clipperIntRect.Bottom = paths[i][j].Y;
					}
				}
				i++;
			}
			return clipperIntRect;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00032AF4 File Offset: 0x00030CF4
		internal void InsertScanbeam(long Y)
		{
			if (this.m_Scanbeam == null)
			{
				this.m_Scanbeam = new ClipperScanbeam
				{
					Next = null,
					Y = Y
				};
				return;
			}
			if (Y > this.m_Scanbeam.Y)
			{
				ClipperScanbeam scanbeam = new ClipperScanbeam
				{
					Y = Y,
					Next = this.m_Scanbeam
				};
				this.m_Scanbeam = scanbeam;
				return;
			}
			ClipperScanbeam clipperScanbeam = this.m_Scanbeam;
			while (clipperScanbeam.Next != null && Y <= clipperScanbeam.Next.Y)
			{
				clipperScanbeam = clipperScanbeam.Next;
			}
			if (Y == clipperScanbeam.Y)
			{
				return;
			}
			ClipperScanbeam next = new ClipperScanbeam
			{
				Y = Y,
				Next = clipperScanbeam.Next
			};
			clipperScanbeam.Next = next;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00032BA2 File Offset: 0x00030DA2
		internal bool PopScanbeam(out long Y)
		{
			if (this.m_Scanbeam == null)
			{
				Y = 0L;
				return false;
			}
			Y = this.m_Scanbeam.Y;
			this.m_Scanbeam = this.m_Scanbeam.Next;
			return true;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00032BD1 File Offset: 0x00030DD1
		internal bool LocalMinimaPending()
		{
			return this.m_CurrentLM != null;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00032BDC File Offset: 0x00030DDC
		internal ClipperOutRec CreateOutRec()
		{
			ClipperOutRec clipperOutRec = new ClipperOutRec
			{
				Idx = -1,
				IsHole = false,
				IsOpen = false,
				FirstLeft = null,
				Pts = null,
				BottomPt = null,
				PolyNode = null
			};
			this.m_PolyOuts.Add(clipperOutRec);
			clipperOutRec.Idx = this.m_PolyOuts.Count - 1;
			return clipperOutRec;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00032C40 File Offset: 0x00030E40
		internal void DisposeOutRec(int index)
		{
			this.m_PolyOuts[index].Pts = null;
			this.m_PolyOuts[index] = null;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00032C64 File Offset: 0x00030E64
		internal void UpdateEdgeIntoAEL(ref ClipperTEdge e)
		{
			if (e.NextInLML == null)
			{
				throw new ClipperException("UpdateEdgeIntoAEL: invalid call");
			}
			ClipperTEdge prevInAEL = e.PrevInAEL;
			ClipperTEdge nextInAEL = e.NextInAEL;
			e.NextInLML.OutIdx = e.OutIdx;
			if (prevInAEL != null)
			{
				prevInAEL.NextInAEL = e.NextInLML;
			}
			else
			{
				this.m_ActiveEdges = e.NextInLML;
			}
			if (nextInAEL != null)
			{
				nextInAEL.PrevInAEL = e.NextInLML;
			}
			e.NextInLML.Side = e.Side;
			e.NextInLML.WindDelta = e.WindDelta;
			e.NextInLML.WindCnt = e.WindCnt;
			e.NextInLML.WindCnt2 = e.WindCnt2;
			e = e.NextInLML;
			e.Curr = e.Bot;
			e.PrevInAEL = prevInAEL;
			e.NextInAEL = nextInAEL;
			if (!ClipperBase.IsHorizontal(e))
			{
				this.InsertScanbeam(e.Top.Y);
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00032D68 File Offset: 0x00030F68
		internal void SwapPositionsInAEL(ClipperTEdge edge1, ClipperTEdge edge2)
		{
			if (edge1.NextInAEL == edge1.PrevInAEL || edge2.NextInAEL == edge2.PrevInAEL)
			{
				return;
			}
			if (edge1.NextInAEL == edge2)
			{
				ClipperTEdge nextInAEL = edge2.NextInAEL;
				if (nextInAEL != null)
				{
					nextInAEL.PrevInAEL = edge1;
				}
				ClipperTEdge prevInAEL = edge1.PrevInAEL;
				if (prevInAEL != null)
				{
					prevInAEL.NextInAEL = edge2;
				}
				edge2.PrevInAEL = prevInAEL;
				edge2.NextInAEL = edge1;
				edge1.PrevInAEL = edge2;
				edge1.NextInAEL = nextInAEL;
			}
			else if (edge2.NextInAEL == edge1)
			{
				ClipperTEdge nextInAEL2 = edge1.NextInAEL;
				if (nextInAEL2 != null)
				{
					nextInAEL2.PrevInAEL = edge2;
				}
				ClipperTEdge prevInAEL2 = edge2.PrevInAEL;
				if (prevInAEL2 != null)
				{
					prevInAEL2.NextInAEL = edge1;
				}
				edge1.PrevInAEL = prevInAEL2;
				edge1.NextInAEL = edge2;
				edge2.PrevInAEL = edge1;
				edge2.NextInAEL = nextInAEL2;
			}
			else
			{
				ClipperTEdge nextInAEL3 = edge1.NextInAEL;
				ClipperTEdge prevInAEL3 = edge1.PrevInAEL;
				edge1.NextInAEL = edge2.NextInAEL;
				if (edge1.NextInAEL != null)
				{
					edge1.NextInAEL.PrevInAEL = edge1;
				}
				edge1.PrevInAEL = edge2.PrevInAEL;
				if (edge1.PrevInAEL != null)
				{
					edge1.PrevInAEL.NextInAEL = edge1;
				}
				edge2.NextInAEL = nextInAEL3;
				if (edge2.NextInAEL != null)
				{
					edge2.NextInAEL.PrevInAEL = edge2;
				}
				edge2.PrevInAEL = prevInAEL3;
				if (edge2.PrevInAEL != null)
				{
					edge2.PrevInAEL.NextInAEL = edge2;
				}
			}
			if (edge1.PrevInAEL == null)
			{
				this.m_ActiveEdges = edge1;
				return;
			}
			if (edge2.PrevInAEL == null)
			{
				this.m_ActiveEdges = edge2;
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00032ED4 File Offset: 0x000310D4
		internal void DeleteFromAEL(ClipperTEdge e)
		{
			ClipperTEdge prevInAEL = e.PrevInAEL;
			ClipperTEdge nextInAEL = e.NextInAEL;
			if (prevInAEL == null && nextInAEL == null && e != this.m_ActiveEdges)
			{
				return;
			}
			if (prevInAEL != null)
			{
				prevInAEL.NextInAEL = nextInAEL;
			}
			else
			{
				this.m_ActiveEdges = nextInAEL;
			}
			if (nextInAEL != null)
			{
				nextInAEL.PrevInAEL = prevInAEL;
			}
			e.NextInAEL = null;
			e.PrevInAEL = null;
		}

		// Token: 0x04000495 RID: 1173
		internal const double Horizontal = -3.4E+38;

		// Token: 0x04000496 RID: 1174
		internal const int Skip = -2;

		// Token: 0x04000497 RID: 1175
		internal const int Unassigned = -1;

		// Token: 0x04000498 RID: 1176
		internal const double Tolerance = 1E-20;

		// Token: 0x04000499 RID: 1177
		public const long loRange = 1073741823L;

		// Token: 0x0400049A RID: 1178
		public const long hiRange = 4611686018427387903L;

		// Token: 0x0400049B RID: 1179
		internal ClipperLocalMinima m_MinimaList;

		// Token: 0x0400049C RID: 1180
		internal ClipperLocalMinima m_CurrentLM;

		// Token: 0x0400049D RID: 1181
		internal List<List<ClipperTEdge>> m_edges = new List<List<ClipperTEdge>>();

		// Token: 0x0400049E RID: 1182
		internal ClipperScanbeam m_Scanbeam;

		// Token: 0x0400049F RID: 1183
		internal List<ClipperOutRec> m_PolyOuts;

		// Token: 0x040004A0 RID: 1184
		internal ClipperTEdge m_ActiveEdges;

		// Token: 0x040004A1 RID: 1185
		internal bool m_UseFullRange;

		// Token: 0x040004A2 RID: 1186
		internal bool m_HasOpenPaths;
	}
}
