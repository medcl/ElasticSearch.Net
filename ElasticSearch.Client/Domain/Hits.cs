using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.Domain
{
	public class Hits
	{
		[JsonProperty("_source")] 
		public Dictionary<string, object> Source = new Dictionary<string, object>();
        
        [JsonProperty("highlight")]
        public Dictionary<string, string[]> Highlight = new Dictionary<string, string[]>();

        [JsonProperty("fields")]
        public Dictionary<string, object> Fields;

		[JsonProperty("_id")] 
		public string Id;
		[JsonProperty("_index")] 
		public string Index;
		[JsonProperty("_score")] 
		public string Score;
		[JsonProperty("_type")] 
		public string Type;
		[JsonProperty("_version")]
		public int Version;

		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("_index:");
			stringBuilder.Append(Index);
			stringBuilder.Append(" _type:");
			stringBuilder.Append(Type);
			stringBuilder.Append(" _id:");
			stringBuilder.Append(Id);
			stringBuilder.Append(" _score:");
			stringBuilder.Append(Score);
			stringBuilder.Append(" _version:");
			stringBuilder.Append(Version);
			stringBuilder.Append(" _source:");
			foreach (var field in Source)
			{
				stringBuilder.Append(field.Key);
				stringBuilder.Append(", ");
				stringBuilder.Append(field.Value);
			}	
            
            stringBuilder.Append(" _highlight:");
			foreach (var field in Highlight)
			{
				stringBuilder.Append(field.Key);
				stringBuilder.Append(", ");
				stringBuilder.Append(field.Value);
			}
			return stringBuilder.ToString();
		}
	}
}