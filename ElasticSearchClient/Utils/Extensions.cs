using System.Diagnostics.Contracts;

namespace ElasticSearch.Client.Utils
{
	public static class Extensions
	{
		public static string F(this string format, params object[] args)
		{
			Contract.Assert(format != null);
			Contract.Assert(args != null);
			return string.Format(format, args);
		}
	}
}