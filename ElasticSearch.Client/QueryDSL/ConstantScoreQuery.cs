using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    [JsonObject("constant_score")]
    [JsonConverter(typeof(ConstantScoreQueryConvert))]
    public class ConstantScoreQuery:IQuery
    {
        public IQuery Query { get; set; }
        public IFilter Filter { get; set; }
        [DefaultValue(1.0)]
        public double Boost { get; set; }
        public ConstantScoreQuery(IQuery query,double boost=1.0)
        {
            Query = query;
            Boost = boost;
        } 
        
        public ConstantScoreQuery(IFilter filter,double boost=1.0)
        {
            Filter = filter;
            Boost = boost;
        }
    }
}