
using System;
using System.Collections.Generic;

namespace Javeriana.Api.HealthChecks
{
    public class HealthCheckResponse
    {
        public string Status {get; set;}

        public IEnumerable<CheckInfo> Checks {get; set;}

        public TimeSpan Duration {get; set;}
    }
}