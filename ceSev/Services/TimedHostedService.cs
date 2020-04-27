using ceSev.Task;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ceSev
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");
            CreateTryTaskWorkHis();
            //CreateTaskWorkHis();
            CreateDataByApi();
        }

        private void CreateTryTaskWorkHis()
        {
            TaskWork taskWork = new TaskWork();
            taskWork.CreteDataTryHis();
        }

        private void CreateTaskWorkHis()
        {
            TaskWork taskWork = new TaskWork();
            taskWork.CreteDataWorkHis();
        }

        private void CreateDataByApi()
        {
            TaskWork taskWork = new TaskWork();
            taskWork.CreteData();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        System.Threading.Tasks.Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            _logger.LogInformation("Timed Background Service is starting.");

            //_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            //_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));


            return System.Threading.Tasks.Task.CompletedTask;
        }

        System.Threading.Tasks.Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();

            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}

