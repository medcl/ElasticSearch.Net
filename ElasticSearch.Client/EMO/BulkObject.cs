using System.Collections.Generic;
using System.Text;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.EMO
{
	public class BulkObject
	{
		public string Index;
		public string Type;
		public string Id;

		public Dictionary<string, object> Fields = new Dictionary<string, object>();
	}

	internal static class BulkObjectExtension
	{
		internal static string GetJson(this IList<BulkObject> bulkObjects)
		{
			StringBuilder stringBuilder = new StringBuilder();

			foreach (var bulkObject in bulkObjects)
			{
				if (bulkObject.Fields.Count > 0)
				{
					stringBuilder.AppendLine("{ \"index\" : { \"_index\" : \"" + bulkObject.Index + "\", \"_type\" : \"" +
											 bulkObject.Type + "\", \"_id\" : \"" + bulkObject.Id + "\" } }");
					stringBuilder.AppendLine(JsonSerializer.Get(bulkObject.Fields));
				}
			}
			return stringBuilder.ToString();
		}

		internal static string Fill(this string format, params object[] args)
		{
			return string.Format(format, args);
		}
	}
}