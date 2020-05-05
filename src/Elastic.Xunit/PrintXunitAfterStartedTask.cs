﻿// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

using Elastic.Managed.ConsoleWriters;
using Elastic.Managed.Ephemeral;
using Elastic.Managed.Ephemeral.Tasks;

namespace Elastic.Xunit
{
	/// <summary>
	/// A task that writes a diagnostic message to indicate that tests will now run
	/// </summary>
	public class PrintXunitAfterStartedTask : ClusterComposeTask
	{
		/// <inheritdoc />
		public override void Run(IEphemeralCluster<EphemeralClusterConfiguration> cluster)
		{
			var name = cluster.GetType().Name;
			cluster.Writer.WriteDiagnostic($"All good! kicking off [{name}] tests now");
		}
	}
}
