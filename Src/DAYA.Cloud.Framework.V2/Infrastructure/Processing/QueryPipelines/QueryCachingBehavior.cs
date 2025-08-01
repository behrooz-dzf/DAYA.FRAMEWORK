﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CacheQ;
using DAYA.Cloud.Framework.V2.Application.Contracts;
using MediatR;

namespace DAYA.Cloud.Framework.V2.Infrastructure.Processing.QueryPipelines;

internal class QueryCachingBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
	where TRequest : IQuery<TResult>
{
	private readonly ICachePolicy<TRequest> _cachePolicy;
	private readonly ICacheManager _cacheManager;

	public QueryCachingBehavior(
		IEnumerable<ICachePolicy<TRequest>> cachePolicy,
		ICacheManager cacheManager)
	{
		_cachePolicy = cachePolicy.SingleOrDefault();
		_cacheManager = cacheManager;
	}

	public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
	{
		if (_cachePolicy is null)
		{
			return await next();
		}
		return await ReadOrUpdateCache(request, next);
	}

	private async Task<TResult> ReadOrUpdateCache(TRequest request, RequestHandlerDelegate<TResult> next)
	{
		if (_cacheManager.TryGetValue(
						_cachePolicy,
						request,
						out TResult cachedResult))
		{
			return await Task.FromResult(cachedResult);
		}

		// Read From Handler
		TResult result = await next();

		// Update Cache
		_cacheManager.SetItem(_cachePolicy, request, result);

		return result;
	}
}