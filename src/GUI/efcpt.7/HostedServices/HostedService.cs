using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ErikEJ.EFCorePowerTools.HostedServices;

public abstract class HostedService : BackgroundService
{
    public sealed override Task StartAsync(CancellationToken cancellationToken)
    {
        return Environment.ExitCode != 0 ? Task.CompletedTask : base.StartAsync(cancellationToken);
    }
}
