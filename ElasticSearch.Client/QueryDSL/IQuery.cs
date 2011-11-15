using System;
using System.Linq.Expressions;

namespace ElasticSearch.Client.QueryDSL
{
	public interface IQuery
	{
		
	}

	public interface IQuery<T> : IQuery where T : class
	{
		Expression<Func<T, object>> Expression { get; }
	}

    public interface IFilter
    {
        
    }
}
