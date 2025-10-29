using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeoutMiddleware : IMiddleware
    {

        private readonly ILogger<RequestTimeoutMiddleware> _logger;
        private readonly Stopwatch _stopwatch;

        public RequestTimeoutMiddleware(ILogger<RequestTimeoutMiddleware> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();

            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;

            var elapsedSeconds = elapsedMilliseconds / 1000.0;

            if (elapsedSeconds > 4.0)
            {
                _logger.LogWarning($"Request [{context.Request.Method}] at [{context.Request.Path}] took [{elapsedSeconds}] seconds which exceeds the threshold.");
            }
        }
    }
}