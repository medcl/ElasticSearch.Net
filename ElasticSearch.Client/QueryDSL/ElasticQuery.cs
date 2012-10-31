using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// a queryDSL wrapper 
	/// </summary>
	[JsonConverter(typeof(ElasticQueryConverterer))]
	public class ElasticQuery
	{
		public ElasticQuery(int from, int size, bool explatin = false)
		{
			From = from;
			Size = size;
			Explain = explatin;
		}

		public ElasticQuery(IQuery query, SortItem sortItem=null, int from=0, int size=5)
		{
			Query = query;
		    if (sortItem != null)
		    {
		        SortItems = new List<SortItem>() { sortItem };
		    }
		    From = from;
			Size = size;
		}

		[DefaultValue(0)]
		public int From { get; private set; }
		[DefaultValue(10)]
		public int Size { get; private set; }
		[DefaultValue(false)]
		public bool Explain { set; get; }

		public List<string> Fields;

		public List<SortItem> SortItems;

		public IQuery Query;

        public IFacet Facets;

	    public Hightlight Hightlight;

        public ElasticQuery SetHightlight(Hightlight hightlight)
        {
            Hightlight = hightlight;
            return this;
        }

        public ElasticQuery AddHighlightField(HightlightField field)
        {
            if(Hightlight==null){Hightlight=new Hightlight();}
            if (field != null) Hightlight.AddField(field);
            return this;
        }

	    public ElasticQuery SetQuery(IQuery query)
		{
			Query = query;
			return this;
		}

		public ElasticQuery SetSortItems(List<SortItem> sortItems)
		{
			SortItems = sortItems;
			return this;
		}

		public ElasticQuery AddField(string field)
		{
			if (Fields == null) { Fields = new List<string>(); }
			Fields.Add(field);
			return this;
		}

		public ElasticQuery AddSortItem(SortItem sortItem)
		{
			if (SortItems == null) { SortItems = new List<SortItem>(); }
			SortItems.Add(sortItem);
			return this;
		}

		public ElasticQuery AddSortItem(string field, SortType type = SortType.Desc)
		{
			if (SortItems == null) { SortItems = new List<SortItem>(); }
			SortItems.Add(new SortItem(field, type));
			return this;
		}

		public void SetFacets()
		{

		}
        
        public ElasticQuery SetFacets(IFacet facet)
        {
            Facets = facet;
            return this;
		}

	    public ElasticQuery AddFields(string[] fields)
	    {
            if (Fields == null) { Fields = new List<string>(); }
            Fields.AddRange(fields);
	    	return this;
	    }
	}

    public interface IFacet
    {
        
    }

    [JsonConverter(typeof(TermsFacetConverterer))]
    public class TermsFacet : IFacet
    {
        internal List<TermsFacetItem> facetItems;
        public TermsFacet(string facetName, string field, int size = 10)
        {
            facetItems = new List<TermsFacetItem>();
            var item = new TermsFacetItem();
            item.FacetName = facetName;
            item.Field = field;
            item.Size = size;
            facetItems.Add(item);
        }

        public TermsFacet AddTermFacet(string facetName, string field, int size = 10)
        {
            var item = new TermsFacetItem();
            item.FacetName = facetName;
            item.Field = field;
            item.Size = size;
            facetItems.Add(item);
            return this;
        }

        public class TermsFacetItem
        {
            public string Field;
            public int Size;
            public string FacetName { get; set; }
        }
    }


	public class Filter
	{
		public IQuery Query;
	}


    [JsonConverter(typeof(HighlightConverterer))]
    public class Hightlight
    {
        public string order;
        public List<HightlightField> fields;
        public string fragment_size;
        public string number_of_fragments;
        public string tag_schema;
        public Hightlight AddField(HightlightField field)
        {
            if(fields==null){fields=new List<HightlightField>();}
            fields.Add(field);
            return this;
        }
    }

    public class HightlightField
    {
        public string name;
        public string fragment_size;
        public string number_of_fragments;
        public string fragment_offset;
        public string tag_schema;
        public string pre_tags;
        public string post_tags;
        public string order;

        public HightlightField(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// require_field_match can be set to true which will cause a field to be highlighted only if a query matched that field. false means that terms are highlighted on all requested fields regardless if the query matches specifically on them.
        /// </summary>
        public bool require_field_match;

        /// <summary>
        /// When highlighting a field that is mapped with term vectors, boundary_chars can be configured to define what constitutes a boundary for highlighting. It’s a single string with each boundary character defined in it. It defaults to .,!? \t\n.The boundary_max_size allows to control how far to look for boundary characters, and defaults to 20
        /// </summary>
        public string boundary_chars;
        

        public int boundary_max_size;
    }
}
