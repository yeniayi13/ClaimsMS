using ClaimsMS.Application.Claim.Command;
using ClaimsMS.Application.Resolution.Command;
using ClaimsMS.Common.Dtos.Claim.Request;
using ClaimsMS.Common.Dtos.Resolution.Request;
using ClaimsMS.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsMS.Controller
{
    [ApiController]
    [Route("claim/resolution")]
    public class ResolutionController : ControllerBase
    {
        private readonly ILogger<ResolutionController> _logger;
        private readonly IMediator _mediator;

        public ResolutionController(ILogger<ResolutionController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(logger));
        }

        // Crea una resolución para un reclamo
        [Authorize(Policy = "AdministradorOSoportePolicy")]
        [HttpPost("Add-Resolution/{claimId}")]
        public async Task<IActionResult> CreatedProduct([FromBody] CreateResolutionDto createResolutionDto, [FromRoute] Guid claimId)
        {
            try
            {
                if (createResolutionDto == null)
                {
                    throw new ArgumentNullException(nameof(createResolutionDto), "El objeto de creación de resolucion no puede ser nulo.");
                }

                var command = new CreateResolutionCommand(createResolutionDto, claimId);
                var resolutionId = await _mediator.Send(command);

                if (resolutionId == Guid.Empty)
                {
                    throw new InvalidOperationException("El reclamo no se pudo crear correctamente.");
                }

                return Ok(resolutionId);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Error: {Message}", e.Message);
                return StatusCode(400, $"Error de argumento nulo: {e.Message}");
            }
            catch (ClaimNotFoundException e)
            {
                _logger.LogError("Error: {Message}", e.Message);
                return StatusCode(404, $"Claim no encontrado: {e.Message}");
            }

            catch (ValidatorException e)
            {
                _logger.LogError("Error: {Message}", e.Message);
                return StatusCode(400, $"Error de validación: {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError("Error: {Message}", e.Message);
                return StatusCode(500, $"Operación inválida: {e.Message}");
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                _logger.LogError("Error de autenticación: {Message}", e.Message);
                return StatusCode(401, "Acceso denegado. Verifica tus credenciales.");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, "No tienes permisos para acceder a este recurso.");
            }
            catch (Exception e)
            {
                _logger.LogError("Error inesperado: {Message}", e.Message);
                return StatusCode(500, "Ocurrió un error inesperado al intentar crear el producto.");
            }
        }
    }
}
