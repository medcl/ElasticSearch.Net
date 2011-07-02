using System.Threading;

namespace ElasticSearch.Client.Utils
{
	internal class AggregateCounter
	{
		private const int c_lsFree = 0;
		private const int c_lsOwned = 1;
		private readonly int[] _data;
		private int _lock = c_lsFree;
		private int _countThisMinute;
		private int _countThisSecond;
		private int _cursor;

		public AggregateCounter(int count)
		{
			_data = new int[count];
		}

		public void IncrementCounter()
		{
			Interlocked.Increment(ref _countThisSecond);
		}

		public void IncrementCounterBy(int value)
		{
			Interlocked.Add(ref _countThisSecond, value);
		}

		private bool EnterLock()
		{
			Thread.BeginCriticalRegion();

			// If resource available, set it to in-use and return
			if (Interlocked.Exchange(ref _lock, c_lsOwned) == c_lsFree)
			{
				return true;
			}
			else
			{
				Thread.EndCriticalRegion();
				return false;
			}
		}

		private void ExitLock()
		{
			// Mark the resource as available
			Interlocked.Exchange(ref _lock, c_lsFree);
			Thread.EndCriticalRegion();
		}

		/// <summary>
		/// Grabs the accumulated amount in the counter and clears it.
		/// This needs to be called by a 1 second timer.
		/// </summary>
		/// <returns>The total aggregated over the last min</returns>
		public int Tick()
		{
			if (EnterLock())
			{
				try
				{
					int totalThisSecond = Interlocked.Exchange(ref _countThisSecond, 0);
					int valueFrom1MinAgo = Interlocked.Exchange(ref _data[_cursor], totalThisSecond);

					_cursor++;
					if (_cursor >= _data.Length) _cursor = 0;

					_countThisMinute -= valueFrom1MinAgo;
					_countThisMinute += totalThisSecond;

					return _countThisMinute;
				}
				finally
				{
					ExitLock();
				}
			}

			return -1;
		}
	}
}