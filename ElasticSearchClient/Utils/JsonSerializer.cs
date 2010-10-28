using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ElasticSearch.Utils
{
	public class JsonSerializer
	{
		static LogWrapper _logger= LogWrapper.GetLogger();
		
		static JsonSerializerSettings SerializationSettings = new JsonSerializerSettings()
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			NullValueHandling = NullValueHandling.Ignore,
			Converters = new List<JsonConverter> { new IsoDateTimeConverter() }
		};

		public static string Get<T>(T t)
		{
			var json = JsonConvert.SerializeObject(t, Formatting.None, SerializationSettings);
			return json;
		}
		public static T Get<T>(string json) where T : class
		{
			try
			{
				var t = JsonConvert.DeserializeObject(json,typeof(T), SerializationSettings);
				return t as T;
			}
			catch (System.Exception e)
			{
				_logger.Error(e);
				throw;
			}
			
		}
	}


	/**
     * @Author : Hu Changwei, CN,
     * Simple Json builder
     * email : koqiui@163.com
     * personal site : http://koqiui.pip.verisignlabs.com/
     */

	internal delegate string ToJSONDelegate(object value, bool useSingleQuote);

	/**
 * @author: Hu Changwei
 * used for Simple Json builder
 * predefined  ToJSON convertor Delegates
 */
}