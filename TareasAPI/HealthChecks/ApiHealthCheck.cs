using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Javeriana.Api.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        public long Threshold { get; set; } = 1024L * 1024L *100L;//; * 1024L;

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            var allocated = GC.GetTotalMemory(forceFullCollection: false);
            var data = new Dictionary<string, object>()
            {
                { "AllocatedBytes", allocated },
                { "Gen0Collections", GC.CollectionCount(0) },
                { "Gen1Collections", GC.CollectionCount(1) },
                { "Gen2Collections", GC.CollectionCount(2) },
            };

            var status = (allocated < Threshold) 
                ? HealthStatus.Healthy 
                : HealthStatus.Degraded;

            return Task.FromResult(new HealthCheckResult(status,
                description: "Se reporta degradado cuando la memoria reservada es superior a :" + Threshold + " Bytes",
                exception: null,
                data: data));
        }
    }
}