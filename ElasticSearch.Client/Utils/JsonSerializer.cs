using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ElasticSearch.Client.Utils
{
	public class JsonSerializer
	{
		static LogWrapper _logger= LogWrapper.GetLogger();
		
		static JsonSerializerSettings SerializationSettings = new JsonSerializerSettings()
		{
//			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			NullValueHandling = NullValueHandling.Ignore,
			DefaultValueHandling = DefaultValueHandling.Ignore,
			Converters = new List<JsonConverter> { new IsoDateTimeConverter() }
		};




		public static string Get<T>(T t)
		{
			var json = JsonConvert.SerializeObject(t, Formatting.None, SerializationSettings);
			return json;
		}
		public static string Get<T>(T t,bool pretty)
		{
			var json = JsonConvert.SerializeObject(t, Formatting.Indented, SerializationSettings);
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
	internal delegate string ToJSONDelegate(object value, bool useSingleQuote);
	}