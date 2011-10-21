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
		
		/// <summary>
		///  Resolve Lucene Keyword
		/// </summary>
		/// <param name="searchKeyword"></param>
		/// <returns></returns>
		public static string ReplaceLuceneKeywordChar(this string searchKeyword)
		{
			if(searchKeyword.Length==1)
			{
				if (searchKeyword.StartsWith("!"))
				{
					searchKeyword = "*";
				}
			}

			searchKeyword = searchKeyword.Replace(@"\", @"\\");
			string strFilter = ":*?~!@^-+'\"\\{}[]()";
			char[] arrFilterChar = strFilter.ToCharArray();
			foreach (char c in arrFilterChar)
			{
				searchKeyword = searchKeyword.Replace("" + c, @"\" + c);
			}
			return searchKeyword;
		}
	}
}