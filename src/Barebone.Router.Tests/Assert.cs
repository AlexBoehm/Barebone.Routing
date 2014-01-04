using System;
using System.Collections.Generic;
using System.Linq;

namespace Barebone.Router.Tests
{
	public class Assert : Xunit.Assert{
		public static void IsOrderedAsc(IEnumerable<IComparable> list){
			var last = list.Last();

			foreach (var item in list.Skip(1)) {				
				if (item.CompareTo(last) < 0) {
					throw new Exception("List ist not sorted");
				}
			}
		}
	}
}
