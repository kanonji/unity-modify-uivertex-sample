using System.Collections;
using System.Collections.Generic;

namespace Kanonji
{
	public class CycleSequence<T> : IEnumerable<T>
	{
		protected List<T> list;

		public CycleSequence(List<T> reel)
		{
			list = reel;
		}

		public IEnumerator<T> GetEnumerator()
		{
			while (true)
			{
				foreach (T rl in list)
				{
					yield return rl;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}