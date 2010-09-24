using System.Xml.Serialization;

namespace ElasticSearchClient
{
	[XmlRoot("ElasticSearchConfig")]
	public static class ElasticSearchConfig
	{
		/// <summary>
		/// ElasticSearchServicesHost
		/// </summary>
		[XmlElement("ElasticSearchHost")]
		public static string ElasticSearchHost { set; get; }
	}
}