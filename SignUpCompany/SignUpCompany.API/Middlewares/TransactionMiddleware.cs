using Microsoft.EntityFrameworkCore.Storage;
using SignUpCompany.Data;

namespace SignUpCompany.API.Middlewares
{
    public class TransactionMiddleware : IMiddleware
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TransactionMiddleware> _logger;

        public TransactionMiddleware(IServiceScopeFactory scopeFactory, ILogger<TransactionMiddleware> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            if (httpContext.Request.Method == HttpMethods.Get)
            {
                await next(httpContext);
                return;
            }

            await using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<CompanyDBContext>();

            IDbContextTransaction transaction = null;

            try
            {
                transaction = await context.Database.BeginTransactionAsync();

                await next(httpContext);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }

                _logger.LogError(ex, "An unhandled exception occurred during request execution.");
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                }
            }
        }
    }
}
