using System.Diagnostics.Contracts;

namespace ElasticSearch.Utils
{
	public static class Extensions
	{
		public static string F(this string format, params object[] args)
		{
			Contract.Ensures(format != null);
			Contract.Ensures(args != null);
			return string.Format(format, args);
		}
	}
}