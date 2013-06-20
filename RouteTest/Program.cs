using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RouteTest
{
	class MainClass{
		public static void Main(string[] args){
			Parse("/test/{foo}/{assetId}-{caption}+++{foobar}.foo.html");
		}

		private static void Parse(string path){
			PathParser.Parse(path);
		}
	}
}
