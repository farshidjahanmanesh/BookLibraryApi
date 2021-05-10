using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookLibrary.Application.MediatR.PipeLines
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Stopwatch pipeLineTimer = new Stopwatch();
            pipeLineTimer.Start();
            var response = await next();
            pipeLineTimer.Stop();
            if (pipeLineTimer.Elapsed.TotalSeconds > 5)
            {
                _logger.LogWarning($"need check {typeof(TRequest).Name} becuase It took a long time");
            }
            return response;
        }
    }
}
