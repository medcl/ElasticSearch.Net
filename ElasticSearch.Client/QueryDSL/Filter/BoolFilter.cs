using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A filter that matches documents matching boolean combinations of other queries. Similar in concept to Boolean query, except that the clauses are other filters. Can be placed within queries that accept a filter.
    /// </summary>
    [JsonObject("bool")]
    [JsonConverter(typeof(BoolFilterConvert))]
    public class BoolFilter:IFilter
    {
        public bool Cache=false;

        public BoolFilter()
        {
            
        }


        internal List<IFilter> ShouldFilters;
        internal List<IFilter> MustFilters;
        internal List<IFilter> MustNotFilters;

        public BoolFilter Must(IFilter query)
        {
            if (MustFilters == null)
            {
                MustFilters = new List<IFilter>();
            }
            MustFilters.Add(query);
            return this;
        }

        public BoolFilter Should(IFilter query)
        {
            if (ShouldFilters == null)
            {
                ShouldFilters = new List<IFilter>();
            }
            ShouldFilters.Add(query);
            return this;
        }

        public BoolFilter MustNot(IFilter query)
        {
            if (MustNotFilters == null)
            {
                MustNotFilters = new List<IFilter>();
            }
            MustNotFilters.Add(query);
            return this;
        }

        public void SetCache(bool cache)
        {
            Cache = cache;
        }
    }
}