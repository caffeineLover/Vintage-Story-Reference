using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModernWpf.Controls
{
	// Token: 0x0200006E RID: 110
	internal class SplitVector<T, SplitVectorID>
	{
		// Token: 0x06000548 RID: 1352 RVA: 0x00013872 File Offset: 0x00011A72
		public SplitVector(SplitVectorID id, Func<T, int> indexOfFunction)
		{
			this.m_indexesInOriginalVector = new List<int>();
			base..ctor();
			this.m_vectorID = id;
			this.m_indexFunctionFromDataSource = indexOfFunction;
			this.m_vector = new ObservableCollection<T>();
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001389E File Offset: 0x00011A9E
		public SplitVectorID GetVectorIDForItem()
		{
			return this.m_vectorID;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000138A6 File Offset: 0x00011AA6
		public IList GetVector()
		{
			return this.m_vector;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x000138B0 File Offset: 0x00011AB0
		public void OnRawDataRemove(int indexInOriginalVector, SplitVectorID vectorID)
		{
			if (object.Equals(this.m_vectorID, vectorID))
			{
				this.RemoveAt(indexInOriginalVector);
			}
			for (int i = 0; i < this.m_indexesInOriginalVector.Count; i++)
			{
				if (this.m_indexesInOriginalVector[i] > indexInOriginalVector)
				{
					List<int> indexesInOriginalVector = this.m_indexesInOriginalVector;
					int num = i;
					int num2 = indexesInOriginalVector[num];
					indexesInOriginalVector[num] = num2 - 1;
				}
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001391C File Offset: 0x00011B1C
		public void OnRawDataInsert(int preferIndex, int indexInOriginalVector, T value, SplitVectorID vectorID)
		{
			for (int i = 0; i < this.m_indexesInOriginalVector.Count; i++)
			{
				if (this.m_indexesInOriginalVector[i] > indexInOriginalVector)
				{
					List<int> indexesInOriginalVector = this.m_indexesInOriginalVector;
					int num = i;
					int num2 = indexesInOriginalVector[num];
					indexesInOriginalVector[num] = num2 + 1;
				}
			}
			if (object.Equals(this.m_vectorID, vectorID))
			{
				this.InsertAt(preferIndex, indexInOriginalVector, value);
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00013989 File Offset: 0x00011B89
		public void InsertAt(int preferIndex, int indexInOriginalVector, T value)
		{
			this.m_vector.Insert(preferIndex, value);
			this.m_indexesInOriginalVector.Insert(preferIndex, indexInOriginalVector);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x000139A8 File Offset: 0x00011BA8
		public void Replace(int indexInOriginalVector, T value)
		{
			int num = this.IndexFromIndexInOriginalVector(indexInOriginalVector);
			Collection<T> vector = this.m_vector;
			vector.RemoveAt(num);
			vector.Insert(num, value);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x000139D1 File Offset: 0x00011BD1
		public void Clear()
		{
			this.m_vector.Clear();
			this.m_indexesInOriginalVector.Clear();
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000139EC File Offset: 0x00011BEC
		public void RemoveAt(int indexInOriginalVector)
		{
			int num = this.IndexFromIndexInOriginalVector(indexInOriginalVector);
			this.m_vector.RemoveAt(num);
			this.m_indexesInOriginalVector.RemoveAt(num);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00013A1C File Offset: 0x00011C1C
		public int IndexOf(T value)
		{
			int num = this.m_indexFunctionFromDataSource.Invoke(value);
			return this.IndexFromIndexInOriginalVector(num);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00013A3D File Offset: 0x00011C3D
		public int IndexToIndexInOriginalVector(int index)
		{
			return this.m_indexesInOriginalVector[index];
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00013A4C File Offset: 0x00011C4C
		public int IndexFromIndexInOriginalVector(int indexInOriginalVector)
		{
			int num = this.m_indexesInOriginalVector.IndexOf(indexInOriginalVector);
			if (num != -1)
			{
				return num;
			}
			return -1;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00013A6D File Offset: 0x00011C6D
		private int Size()
		{
			return this.m_indexesInOriginalVector.Count;
		}

		// Token: 0x04000281 RID: 641
		private SplitVectorID m_vectorID;

		// Token: 0x04000282 RID: 642
		private Collection<T> m_vector;

		// Token: 0x04000283 RID: 643
		private List<int> m_indexesInOriginalVector;

		// Token: 0x04000284 RID: 644
		private Func<T, int> m_indexFunctionFromDataSource;
	}
}
