using System.Collections.Generic;
using ElasticSearch.Mapping;
using Newtonsoft.Json;

namespace ElasticSearch.Client
{

	public class TemplateSetting
	{
		public TemplateSetting(string template)
		{
			Template = template;
		}
		
		[JsonProperty("template")]
		public string Template { set; get; }

		[JsonProperty("order")]
		public int Order { set; get; }

		[JsonProperty("settings")]
		public IndexSetting IndexSetting { set; get; }

		[JsonProperty("mappings")] 
		public Dictionary<string, TypeSetting> Mappings;

		public void AddTypeSetting(TypeSetting typeSetting)
		{
			if (Mappings == null) { Mappings = new Dictionary<string, TypeSetting>(); }
			Mappings[typeSetting.Type] = typeSetting;
		}
	}
}