using System.Collections.Generic;

namespace ElasticSearch.Client.Domain
{
	/// <summary>
	/// single document result
	/// </summary>
	public class Document
	{
		public Hits Hits;
		public string JsonString;
		public Dictionary<string, object> GetFields()
		{
			var dict = new Dictionary<string, object>();
			if (Hits != null)
			{
				foreach (var fileItem in Hits.Source)
				{
					if (dict.ContainsKey(fileItem.Key))
					{
						object value = dict[fileItem.Key];
						value = value + "," + fileItem.Value;
						dict[fileItem.Key] = value;
					}
					else
					{
						dict.Add(fileItem.Key, fileItem.Value);
					}
				}
			}
			return dict;
		}
	}
}