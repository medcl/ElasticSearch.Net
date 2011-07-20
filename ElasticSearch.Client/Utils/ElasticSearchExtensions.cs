using System.Diagnostics.Contracts;

namespace ElasticSearch.Client.Utils
{
	public static class ElasticSearchExtensions
	{
		public static string Fill(this string formatString, params object[] args)
		{
			Contract.Assert(formatString != null);
			Contract.Assert(args != null);
			return string.Format(formatString, args);
		}
	}
}