using AdvertApi.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace AdvertApi.HealthCheck
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorageService _advertStorageService;

        public StorageHealthCheck(IAdvertStorageService advertStorageService)
        {
            _advertStorageService = advertStorageService;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var storageStatus = await _advertStorageService.CheckHealthAsync();
            return new HealthCheckResult(storageStatus ? HealthStatus.Healthy : HealthStatus.Unhealthy);
        }
    }
}
