﻿using DAYA.ArchRules.Utilities;
using DAYA.Cloud.Framework.V2.Application.Contracts;
using NetArchTest.Rules;

namespace DAYA.ArchRules.Application.Queries.Queries
{
    internal class QueryShouldBeImmutable : ArchRule
    {
        internal override void Check()
        {
            var result = Types.InAssembly(Data.ApplicationAssembly)
                .That()
                .Inherit(typeof(Query<>)).Or()
                .ImplementInterface(typeof(IQuery<>))
                .Should()
                .BeInitOnly();

            AssertArchTestResult(result);
        }
    }
}