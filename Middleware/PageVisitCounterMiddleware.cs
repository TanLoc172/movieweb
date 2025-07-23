// Trong Middleware/PageVisitCounterMiddleware.cs
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MovieWebsite.Middleware
{
    public class PageVisitCounterMiddleware
    {
        private readonly RequestDelegate _next;

        public PageVisitCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            // Chỉ đếm các request GET thông thường, bỏ qua các file tĩnh (css, js, images) và API calls
            if (context.Request.Method == "GET" && !context.Request.Path.Value.Contains("."))
            {
                var counter = await dbContext.AnalyticsCounters
                                .FirstOrDefaultAsync(c => c.CounterName == "TotalPageViews");

                if (counter == null)
                {
                    counter = new AnalyticsCounter { CounterName = "TotalPageViews", CountValue = 1 };
                    dbContext.AnalyticsCounters.Add(counter);
                }
                else
                {
                    counter.CountValue++;
                }

                await dbContext.SaveChangesAsync();
            }

            // Chuyển request cho middleware tiếp theo trong pipeline
            await _next(context);
        }
    }
}