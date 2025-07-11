using ClaimsMS.Application.Claim.Command;
using ClaimsMS.Application.Claim.Handler.Queries;
using ClaimsMS.Application.Claim.Queries;
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

        // Crea un reclamo para un producto en una subasta
        [Authorize(Policy = "PostorPolicy")]
        [HttpPost("Add-Claim/{auctionId}/{userId}")]
        public async Task<IActionResult> CreatedClaim([FromBody] CreateClaimDto createClaimDto, [FromRoute] Guid userId, [FromRoute] Guid auctionId)
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


        [Authorize(Policy = "PostorPolicy")]
        [HttpPost("Add-ClaimDelivery/{deliveryId}/{userId}")]
        public async Task<IActionResult> CreatedClaimDelivery([FromBody] CreateClaimDeliveryDto createClaimDto, [FromRoute] Guid userId, [FromRoute] Guid deliveryId)
        {
            try
            {
                if (createClaimDto == null)
                {
                    throw new ArgumentNullException(nameof(createClaimDto), "El objeto de creación de producto no puede ser nulo.");
                }

                var command = new CreateClaimDeliveryCommand(createClaimDto, userId, deliveryId);
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

        // Obtiene los reclamos filtrados por estado, usuario y subasta
        [HttpGet("Get-Claim-Filtered")]
        public async Task<IActionResult> GetFilteredClaim(
           [FromQuery] Guid? userId = null,
           [FromQuery] Guid? auction = null,
           [FromQuery] string? status = null)
        {
            try
            {
             
               

                var query = new GetClaimByStatusFilteredQuery( userId, status, auction);
                var bids = await _mediator.Send(query);

                if (bids == null || !bids.Any())
                {
                    return Ok(null);
                }

                return Ok(bids);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Error de parámetros de entrada: {Message}", ex.Message);
                return BadRequest($"Parámetro inválido: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                _logger.LogError("Tiempo de espera excedido: {Message}", ex.Message);
                return StatusCode(408, $"Tiempo de espera excedido: {ex.Message}");
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                _logger.LogError("Error de autenticación: {Message}", e.Message);
                return StatusCode(401, "Acceso denegado. Verifica tus credenciales.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Error de comunicación con el servidor: {Message}", ex.Message);
                return StatusCode(503, $"Servicio no disponible: {ex.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, "No tienes permisos para acceder a este recurso.");
            }
            catch (ClaimNotFoundException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, $"Ocurrió un error inesperado al obtener los productos, {e.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inesperado en GetAvailableProducts: {Message}", ex.Message);
                return StatusCode(500, $"Ocurrió un error inesperado al obtener los productos, {ex.Message}");
            }
        }


        // Obtiene los reclamos de entregas filtrados por estado, usuario y subasta
        [HttpGet("Get-ClaimDelivery-Filtered")]
        public async Task<IActionResult> GetFilteredClaimDelivery(
           [FromQuery] Guid? userId = null,
           [FromQuery] Guid? deliveryId = null,
           [FromQuery] string? status = null)
        {
            try
            {



                var query = new GetClaimDeliveryByStatusFilteredQuery(userId, status, deliveryId);
                var bids = await _mediator.Send(query);

                if (bids == null || !bids.Any())
                {
                    return Ok(null);
                }

                return Ok(bids);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Error de parámetros de entrada: {Message}", ex.Message);
                return BadRequest($"Parámetro inválido: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                _logger.LogError("Tiempo de espera excedido: {Message}", ex.Message);
                return StatusCode(408, $"Tiempo de espera excedido: {ex.Message}");
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                _logger.LogError("Error de autenticación: {Message}", e.Message);
                return StatusCode(401, "Acceso denegado. Verifica tus credenciales.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Error de comunicación con el servidor: {Message}", ex.Message);
                return StatusCode(503, $"Servicio no disponible: {ex.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, "No tienes permisos para acceder a este recurso.");
            }
            catch (ClaimNotFoundException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, $"Ocurrió un error inesperado al obtener los productos, {e.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inesperado en GetAvailableProducts: {Message}", ex.Message);
                return StatusCode(500, $"Ocurrió un error inesperado al obtener los productos, {ex.Message}");
            }
        }

        [HttpGet("Get-ClaimDelivery-ById/{id}")]
        public async Task<IActionResult> GetClaimByIdDelivery(
           [FromRoute] Guid id)
        {
            try
            {

                var query = new GetClaimDeliveryByIdQuery(id);
                var bids = await _mediator.Send(query);

                if (bids == null )
                {
                    return Ok(null);
                }

                return Ok(bids);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Error de parámetros de entrada: {Message}", ex.Message);
                return BadRequest($"Parámetro inválido: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                _logger.LogError("Tiempo de espera excedido: {Message}", ex.Message);
                return StatusCode(408, $"Tiempo de espera excedido: {ex.Message}");
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                _logger.LogError("Error de autenticación: {Message}", e.Message);
                return StatusCode(401, "Acceso denegado. Verifica tus credenciales.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Error de comunicación con el servidor: {Message}", ex.Message);
                return StatusCode(503, $"Servicio no disponible: {ex.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, "No tienes permisos para acceder a este recurso.");
            }
            catch (ClaimNotFoundException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, $"Ocurrió un error inesperado al obtener los productos, {e.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inesperado en GetAvailableProducts: {Message}", ex.Message);
                return StatusCode(500, $"Ocurrió un error inesperado al obtener los productos, {ex.Message}");
            }
        }


        [HttpGet("Get-Claim-ById/{id}")]
        public async Task<IActionResult> GetClaimById(
           [FromRoute] Guid id)
        {
            try
            {

                var query = new GetClaimByIdQuery(id);
                var bids = await _mediator.Send(query);

                if (bids == null)
                {
                    return Ok(null);
                }

                return Ok(bids);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Error de parámetros de entrada: {Message}", ex.Message);
                return BadRequest($"Parámetro inválido: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                _logger.LogError("Tiempo de espera excedido: {Message}", ex.Message);
                return StatusCode(408, $"Tiempo de espera excedido: {ex.Message}");
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                _logger.LogError("Error de autenticación: {Message}", e.Message);
                return StatusCode(401, "Acceso denegado. Verifica tus credenciales.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Error de comunicación con el servidor: {Message}", ex.Message);
                return StatusCode(503, $"Servicio no disponible: {ex.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, "No tienes permisos para acceder a este recurso.");
            }
            catch (ClaimNotFoundException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, $"Ocurrió un error inesperado al obtener los productos, {e.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inesperado en GetAvailableProducts: {Message}", ex.Message);
                return StatusCode(500, $"Ocurrió un error inesperado al obtener los productos, {ex.Message}");
            }
        }


        // Obtiene todos lo reclamos
        [HttpGet("GetAll-Claim")]
        public async Task<IActionResult> GetAllClaim()
        {
            try
            {

                var query = new GetAllClaimQuery();
                var bids = await _mediator.Send(query);

                if (bids == null)
                {
                    return Ok(null);
                }

                return Ok(bids);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Error de parámetros de entrada: {Message}", ex.Message);
                return BadRequest($"Parámetro inválido: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                _logger.LogError("Tiempo de espera excedido: {Message}", ex.Message);
                return StatusCode(408, $"Tiempo de espera excedido: {ex.Message}");
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                _logger.LogError("Error de autenticación: {Message}", e.Message);
                return StatusCode(401, "Acceso denegado. Verifica tus credenciales.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Error de comunicación con el servidor: {Message}", ex.Message);
                return StatusCode(503, $"Servicio no disponible: {ex.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, "No tienes permisos para acceder a este recurso.");
            }
            catch (ClaimNotFoundException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, $"Ocurrió un error inesperado al obtener los productos, {e.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inesperado en GetAvailableProducts: {Message}", ex.Message);
                return StatusCode(500, $"Ocurrió un error inesperado al obtener los productos, {ex.Message}");
            }
        }

        //Obtiene todos los reclamos de entregas
        [HttpGet("GetAll-ClaimDelivery")]
        public async Task<IActionResult> GetAllClaimDelivery()
        {
            try
            {

                var query = new GetAllClaimDeliveryQuery();
                var bids = await _mediator.Send(query);

                if (bids == null)
                {
                    return Ok(null);
                }

                return Ok(bids);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Error de parámetros de entrada: {Message}", ex.Message);
                return BadRequest($"Parámetro inválido: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                _logger.LogError("Tiempo de espera excedido: {Message}", ex.Message);
                return StatusCode(408, $"Tiempo de espera excedido: {ex.Message}");
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                _logger.LogError("Error de autenticación: {Message}", e.Message);
                return StatusCode(401, "Acceso denegado. Verifica tus credenciales.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Error de comunicación con el servidor: {Message}", ex.Message);
                return StatusCode(503, $"Servicio no disponible: {ex.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, "No tienes permisos para acceder a este recurso.");
            }
            catch (ClaimNotFoundException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, $"Ocurrió un error inesperado al obtener los productos, {e.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inesperado en GetAvailableProducts: {Message}", ex.Message);
                return StatusCode(500, $"Ocurrió un error inesperado al obtener los productos, {ex.Message}");
            }
        }

        //Actualiza el estado a revisado
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

        [Authorize(Policy = "AdministradorOSoportePolicy")]
        [HttpPut("Update-StatusClaimDelivery/{claimId}")]
        public async Task<IActionResult> UpdateStatusClaimDelivery([FromRoute] Guid claimId)
        {
            try
            {
                var command = new UpdateStatusClaimDeliveryCommand(claimId);
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
