using System.Linq;
using GreenFluxAssignment.Api.Contracts.Responses;
using GreenFluxAssignment.Domain.Exceptions;
using GreenFluxAssignment.Domain.Services;
using GreenFluxAssignment.Persistence.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GreenFluxAssignment.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            _logger.LogError(exception, exception.Message);

            var result = exception switch
            {
                EntityNotFoundException entityException => Handle(entityException),
                ConnectorsLimitException limitException => Handle(limitException),
                InvalidArgumentException argumentException => Handle(argumentException),
                ExcessiveCurrentException excessiveException => Handle(excessiveException),
                ConcurrencyConflictException concurrencyConflict => Handle(concurrencyConflict),
                _ => Handle(),
            };

            context.Result = result;
        }

        private ObjectResult Handle(EntityNotFoundException exception)
        {
            return new NotFoundObjectResult(new { message = exception.Message });
        }

        private ObjectResult Handle(ConnectorsLimitException exception)
        {
            return new UnprocessableEntityObjectResult(new { message = exception.Message });
        }

        private ObjectResult Handle(InvalidArgumentException exception)
        {
            return new BadRequestObjectResult(new { message = exception.Message });
        }

        private ObjectResult Handle(ExcessiveCurrentException exception)
        {
            var message = $"Excessive {exception.ExcessiveCurrent} current requested." +
                $" Remove existing connectors or increase the group capacity";
            var suggestions = ConnectorRemovalSuggestionService.SuggestConnectors(
                exception.Group,
                exception.ExcessiveCurrent)
                .Select(s => s.Select(cs => new ConnectorSuggestion
                {
                    ConnectorId = cs.ConnectorId,
                    StationId = cs.StationId,
                }));

            return new BadRequestObjectResult(new { message, suggestions });
        }

        private ObjectResult Handle(ConcurrencyConflictException concurrencyConflict)
        {
            string message = $"Cannot update Group with id {concurrencyConflict.Id}";
            return new ObjectResult(new { message })
            {
                StatusCode = StatusCodes.Status409Conflict,
            };
        }

        private ObjectResult Handle()
        {
            return new ObjectResult(new { message = "Internal Server Error." })
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
