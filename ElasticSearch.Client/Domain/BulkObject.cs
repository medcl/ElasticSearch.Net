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
		//TODO: replace dictionary to list
		public Dictionary<string, object> Fields = new Dictionary<string, object>();
		public string JsonData;
		public string ParentId;
		public BulkObject(){}
		public BulkObject(string index,string type,string id,string jsonData,string parentKey=null)
		{
			Index = index.Trim().ToLower();
			Type = type.Trim();
			Id = id;
			JsonData = jsonData;
		}
		public BulkObject(string index, string type, string id, Dictionary<string, object> fields)
		{
			Index = index.Trim().ToLower();
			Type = type.Trim();
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
					//stringBuilder.AppendLine("{ \"index\" : { \"_index\" : \"" + bulkObject.Index + "\", \"_type\" : \"" +
						//					 bulkObject.Type + "\", \"_id\" : \"" + bulkObject.Id + "\" } }");
					stringBuilder.Append("{ \"index\" : { \"_index\" : \"" + bulkObject.Index.ToLower());
					if (!string.IsNullOrEmpty(bulkObject.ParentId))
					{
						stringBuilder.Append("\", \"_parent\" : \"" + bulkObject.ParentId);
					}
					stringBuilder.Append("\", \"_type\" : \"" + bulkObject.Type);
					stringBuilder.Append("\", \"_id\" : \"" + bulkObject.Id + "\" } }");
					stringBuilder.Append("\n");    

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
	}
}