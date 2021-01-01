using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Rosterd.Admin.Api.Infrastructure.HealthChecks
{
    internal class PingHealthCheck : IHealthCheck
    {
        private readonly string _host;
        private readonly int _timeout;

        public PingHealthCheck(string host, int timeout)
        {
            _host = host;
            _timeout = timeout;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(_host, _timeout);
                if (reply.Status != IPStatus.Success)
                {
                    return HealthCheckResult.Unhealthy($"Ping check status [{ reply.Status }]. Host {_host} did not respond within {_timeout} ms.");
                }

                return reply.RoundtripTime >= _timeout ?
                    HealthCheckResult.Degraded($"Ping check for {_host} takes too long to respond. Expected {_timeout} ms but responded in {reply.RoundtripTime} ms.") :
                    HealthCheckResult.Healthy($"Ping check for {_host} is ok.");
            }
            catch
            {
                return HealthCheckResult.Unhealthy($"Error when trying to check ping for {_host}.");
            }
        }
    }
}
