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
		public string JsonData;
		public BulkObject(){}
		public BulkObject(string index,string type,string id,string jsonData)
		{
			Index = index;
			Type = type;
			Id = id;
			JsonData = jsonData;
		}
		public BulkObject(string index, string type, string id, Dictionary<string, object> fields)
		{
			Index = index;
			Type = type;
			Id = id;
			Fields = fields;
		}
	}

	internal static class BulkObjectExtension
	{
		internal static string GetJson(this IList<BulkObject> bulkObjects)
		{
			StringBuilder stringBuilder = new StringBuilder();

			foreach (var bulkObject in bulkObjects)
			{
					stringBuilder.AppendLine("{ \"index\" : { \"_index\" : \"" + bulkObject.Index + "\", \"_type\" : \"" +
											 bulkObject.Type + "\", \"_id\" : \"" + bulkObject.Id + "\" } }");
					if(bulkObject.Fields.Count>0)
					{
						stringBuilder.AppendLine(JsonSerializer.Get(bulkObject.Fields));
					}
					else
					{
						stringBuilder.AppendLine(bulkObject.JsonData);
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