using System.Collections.Generic;
using ElasticSearch.Client.Mapping;
using Newtonsoft.Json;

namespace ElasticSearch.Client.Domain
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
		public TemplateIndexSetting IndexSetting { set; get; }

		[JsonProperty("mappings")] 
		public Dictionary<string, TypeSetting> Mappings;

		public void AddTypeSetting(TypeSetting typeSetting)
		{
			if (Mappings == null) { Mappings = new Dictionary<string, TypeSetting>(); }
			Mappings[typeSetting.TypeName] = typeSetting;
		}
	}
}