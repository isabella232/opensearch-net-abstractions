﻿// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

using System;
using System.Collections.Concurrent;
using Elastic.Managed.Ephemeral;
using Elasticsearch.Net;
using Nest;

namespace Elastic.Xunit.ExampleComplex
{
	internal static class EphemeralClusterExtensions
	{
		private static readonly ConcurrentDictionary<IEphemeralCluster, IElasticClient> Clients = new ConcurrentDictionary<IEphemeralCluster, IElasticClient>();

		public static IElasticClient GetOrAddClient(this IEphemeralCluster cluster)
		{
			return Clients.GetOrAdd(cluster, (c) =>
			{
				var connectionPool = new StaticConnectionPool(c.NodesUris());
				var settings = new ConnectionSettings(connectionPool);
				var client = new ElasticClient(settings);
				return client;
			});
		}
	}

	public interface IMyCluster
	{
		IElasticClient Client { get; }
	}

	public abstract class MyClusterBase : XunitClusterBase, IMyCluster
	{
		protected MyClusterBase() : base(new XunitClusterConfiguration(MyRunOptions.TestVersion)
		{
			ShowElasticsearchOutputAfterStarted = false,
		})
		{

		}

		public IElasticClient Client => this.GetOrAddClient();
	}

	public class TestCluster : MyClusterBase
	{
		protected override void SeedCluster()
		{
			var pluginsResponse = this.Client.CatPlugins();
		}
	}

	public class TestGenericCluster : XunitClusterBase<XunitClusterConfiguration>, IMyCluster
	{
		public TestGenericCluster() : base(new XunitClusterConfiguration(MyRunOptions.TestVersion)) { }

		public IElasticClient Client => this.GetOrAddClient();

		protected override void SeedCluster()
		{
			var aliasesResponse = this.Client.CatAliases();
		}
	}
}
