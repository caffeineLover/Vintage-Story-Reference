using System;
using System.Collections.Generic;

namespace ModernWpf.Controls
{
	// Token: 0x0200006F RID: 111
	internal class SplitDataSourceBase<T, SplitVectorID, AttachedDataType> where SplitVectorID : Enum
	{
		// Token: 0x06000555 RID: 1365 RVA: 0x00013A7A File Offset: 0x00011C7A
		public SplitVectorID GetVectorIDForItem(int index)
		{
			return this.m_flags[index];
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00013A88 File Offset: 0x00011C88
		public AttachedDataType AttachedData(int index)
		{
			return this.m_attachedData[index];
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00013A96 File Offset: 0x00011C96
		public void AttachedData(int index, AttachedDataType attachedData)
		{
			this.m_attachedData[index] = attachedData;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00013AA5 File Offset: 0x00011CA5
		public void ResetAttachedData()
		{
			this.ResetAttachedData(this.DefaultAttachedData());
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00013AB4 File Offset: 0x00011CB4
		public void ResetAttachedData(AttachedDataType attachedData)
		{
			for (int i = 0; i < this.RawDataSize(); i++)
			{
				this.m_attachedData[i] = attachedData;
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00013ADF File Offset: 0x00011CDF
		public SplitVector<T, SplitVectorID> GetVectorForItem(int index)
		{
			if (index >= 0 && index < this.RawDataSize())
			{
				return this.m_splitVectors[Convert.ToInt32(this.m_flags[index])];
			}
			return null;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00013B0D File Offset: 0x00011D0D
		public void MoveItemsToVector(SplitVectorID newVectorID)
		{
			this.MoveItemsToVector(0, this.RawDataSize(), newVectorID);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00013B20 File Offset: 0x00011D20
		public void MoveItemsToVector(int start, int end, SplitVectorID newVectorID)
		{
			for (int i = start; i < end; i++)
			{
				this.MoveItemToVector(i, newVectorID);
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00013B44 File Offset: 0x00011D44
		public void MoveItemToVector(int index, SplitVectorID newVectorID)
		{
			if (!object.Equals(this.m_flags[index], newVectorID))
			{
				SplitVector<T, SplitVectorID> vectorForItem = this.GetVectorForItem(index);
				if (vectorForItem != null)
				{
					vectorForItem.RemoveAt(index);
				}
				this.m_flags[index] = newVectorID;
				SplitVector<T, SplitVectorID> splitVector = this.m_splitVectors[Convert.ToInt32(newVectorID)];
				if (splitVector != null)
				{
					int preferIndex = this.GetPreferIndex(index, newVectorID);
					T at = this.GetAt(index);
					splitVector.InsertAt(preferIndex, index, at);
				}
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00013BBE File Offset: 0x00011DBE
		internal virtual int IndexOf(T value)
		{
			return 0;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00013BC4 File Offset: 0x00011DC4
		internal virtual T GetAt(int index)
		{
			return default(T);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00013BDA File Offset: 0x00011DDA
		internal virtual int Size()
		{
			return 0;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00013BE0 File Offset: 0x00011DE0
		internal virtual SplitVectorID DefaultVectorIDOnInsert()
		{
			return default(SplitVectorID);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00013BF8 File Offset: 0x00011DF8
		internal virtual AttachedDataType DefaultAttachedData()
		{
			return default(AttachedDataType);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00013C10 File Offset: 0x00011E10
		public int IndexOfImpl(T value, SplitVectorID vectorID)
		{
			int num = this.IndexOf(value);
			int result = -1;
			if (num != -1)
			{
				SplitVector<T, SplitVectorID> vectorForItem = this.GetVectorForItem(num);
				if (vectorForItem != null && object.Equals(vectorForItem.GetVectorIDForItem(), vectorID))
				{
					result = vectorForItem.IndexFromIndexInOriginalVector(num);
				}
			}
			return result;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00013C58 File Offset: 0x00011E58
		public void InitializeSplitVectors(List<SplitVector<T, SplitVectorID>> vectors)
		{
			foreach (SplitVector<T, SplitVectorID> splitVector in vectors)
			{
				this.m_splitVectors[Convert.ToInt32(splitVector.GetVectorIDForItem())] = splitVector;
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00013CB8 File Offset: 0x00011EB8
		public SplitVector<T, SplitVectorID> GetVector(SplitVectorID vectorID)
		{
			return this.m_splitVectors[Convert.ToInt32(vectorID)];
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00013CCC File Offset: 0x00011ECC
		public void OnClear()
		{
			foreach (SplitVector<T, SplitVectorID> splitVector in this.m_splitVectors)
			{
				if (splitVector != null)
				{
					splitVector.Clear();
				}
			}
			this.m_flags.Clear();
			this.m_attachedData.Clear();
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00013D14 File Offset: 0x00011F14
		public void OnRemoveAt(int startIndex, int count)
		{
			for (int i = startIndex + count - 1; i >= startIndex; i--)
			{
				this.OnRemoveAt(i);
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00013D38 File Offset: 0x00011F38
		public void OnInsertAt(int startIndex, int count)
		{
			for (int i = startIndex; i < startIndex + count; i++)
			{
				this.OnInsertAt(i);
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00013D5A File Offset: 0x00011F5A
		public int RawDataSize()
		{
			return this.m_flags.Count;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00013D68 File Offset: 0x00011F68
		public void SyncAndInitVectorFlagsWithID(SplitVectorID defaultID, AttachedDataType defaultAttachedData)
		{
			for (int i = 0; i < this.Size(); i++)
			{
				this.m_flags.Add(defaultID);
				this.m_attachedData.Add(defaultAttachedData);
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00013D9E File Offset: 0x00011F9E
		public void Clear()
		{
			this.OnClear();
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00013DA8 File Offset: 0x00011FA8
		private void OnRemoveAt(int index)
		{
			SplitVectorID splitVectorID = this.m_flags[index];
			foreach (SplitVector<T, SplitVectorID> splitVector in this.m_splitVectors)
			{
				if (splitVector != null)
				{
					splitVector.OnRawDataRemove(index, splitVectorID);
				}
			}
			this.m_flags.RemoveAt(index);
			this.m_attachedData.RemoveAt(index);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00013E00 File Offset: 0x00012000
		private void OnReplace(int index)
		{
			SplitVector<T, SplitVectorID> vectorForItem = this.GetVectorForItem(index);
			if (vectorForItem != null)
			{
				T at = this.GetAt(index);
				vectorForItem.Replace(index, at);
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00013E28 File Offset: 0x00012028
		private void OnInsertAt(int index)
		{
			SplitVectorID splitVectorID = this.DefaultVectorIDOnInsert();
			AttachedDataType attachedDataType = this.DefaultAttachedData();
			int preferIndex = this.GetPreferIndex(index, splitVectorID);
			T at = this.GetAt(index);
			foreach (SplitVector<T, SplitVectorID> splitVector in this.m_splitVectors)
			{
				if (splitVector != null)
				{
					splitVector.OnRawDataInsert(preferIndex, index, at, splitVectorID);
				}
			}
			this.m_flags.Insert(index, splitVectorID);
			this.m_attachedData.Insert(index, attachedDataType);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00013E9F File Offset: 0x0001209F
		private int GetPreferIndex(int index, SplitVectorID vectorID)
		{
			return this.RangeCount(0, index, vectorID);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00013EAC File Offset: 0x000120AC
		private int RangeCount(int start, int end, SplitVectorID vectorID)
		{
			int num = 0;
			for (int i = start; i < end; i++)
			{
				if (object.Equals(this.m_flags[i], vectorID))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00013EEA File Offset: 0x000120EA
		public SplitDataSourceBase()
		{
			this.m_flags = new List<SplitVectorID>();
			this.m_attachedData = new List<AttachedDataType>();
			this.m_splitVectors = new SplitVector<T, SplitVectorID>[SplitDataSourceBase<T, SplitVectorID, AttachedDataType>.SplitVectorSize];
			base..ctor();
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00013F18 File Offset: 0x00012118
		// Note: this type is marked as 'beforefieldinit'.
		static SplitDataSourceBase()
		{
			SplitDataSourceBase<T, SplitVectorID, AttachedDataType>.SplitVectorSize = Enum.GetNames(typeof(SplitVectorID)).Length;
		}

		// Token: 0x04000285 RID: 645
		private static readonly int SplitVectorSize;

		// Token: 0x04000286 RID: 646
		private List<SplitVectorID> m_flags;

		// Token: 0x04000287 RID: 647
		private List<AttachedDataType> m_attachedData;

		// Token: 0x04000288 RID: 648
		private SplitVector<T, SplitVectorID>[] m_splitVectors;
	}
}
