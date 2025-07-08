using ClaimsMS.Application.Claim.Command;
using ClaimsMS.Common.Dtos.Claim.Request;
using ClaimsMS.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsMS.Controller
{
    [ApiController]
    [Route("claim")]
    public class ClaimController : ControllerBase
    {
        private readonly ILogger<ClaimController> _logger;
        private readonly IMediator _mediator;

        public ClaimController(ILogger<ClaimController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(logger));
        }
        //[Authorize(Policy = "SubastadorPolicy")]
        [HttpPost("Add-Claim/{auctionId}/{userId}")]
        public async Task<IActionResult> CreatedProduct([FromBody] CreateClaimDto createClaimDto, [FromRoute] Guid userId, [FromRoute] Guid auctionId)
        {
            try
            {
                if (createClaimDto == null)
                {
                    throw new ArgumentNullException(nameof(createClaimDto), "El objeto de creación de producto no puede ser nulo.");
                }

                var command = new CreateClaimCommand(createClaimDto, userId, auctionId);
                var categoryId = await _mediator.Send(command);

                if (categoryId == Guid.Empty)
                {
                    throw new InvalidOperationException("El reclamo no se pudo crear correctamente.");
                }

                return Ok(categoryId);
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
        [Authorize(Policy = "AdministradorOSoportePolicy")]
        [HttpPut("Update-Status/{claimId}")]
        public async Task<IActionResult> UpdateStatusClaim([FromRoute] Guid claimId)
        {
            try
            {
                var command = new UpdateStatusClaimCommand(claimId);
                var id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Error: {Message}", e.Message);
                return StatusCode(400, $"Argumento nulo: {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError("Operación inválida: {Message}", e.Message);
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
                return StatusCode(401, "No tienes permisos para realizar esta operación.");
            }
            catch (Exception e)
            {
                _logger.LogError("Error inesperado: {Message}", e.Message);
                return StatusCode(500, "Ocurrió un error inesperado al intentar registrar la tarjeta.");
            }
        }


    }
}
