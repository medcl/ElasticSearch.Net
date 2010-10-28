using Newtonsoft.Json;

namespace ElasticSearch.Client
{
	/// <summary>
	/// operation result
	/// </summary>
	public class OperateResult
	{
		[JsonProperty("ok")]
		public bool Success;

		[JsonProperty("error")] 
		public string ErrorMessage;

		[JsonProperty("acknowledged")]
		public bool Acknowledged;

		[JsonIgnore]
		public string JsonString { set; get; }
	}
}