using BancoApi.DTOs;
using BancoApi.Services;
using Microsoft.AspNetCore.Mvc;
namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentaService _service;
        public CuentasController(ICuentaService service)
        {
            _service = service;
        }
        [HttpPost("clientes")]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteRequest request)
        {
            var res = await _service.CrearClienteAsync(request);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CrearCuenta([FromBody] CrearCuentaRequest request)
        {
            try
            {
                var res = await _service.CrearCuentaAsync(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [HttpGet("{numeroCuenta}/saldo")]
        public async Task<IActionResult> ConsultarSaldo(string numeroCuenta)
        {
            try
            {
                var res = await _service.ObtenerSaldoAsync(numeroCuenta);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }
        [HttpPost("deposito")]
        public async Task<IActionResult> Depositar([FromBody] TransaccionRequest request)
        {
            try
            {
                var res = await _service.DepositarAsync(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [HttpPost("retiro")]
        public async Task<IActionResult> Retirar([FromBody] TransaccionRequest request)
        {
            try
            {
                var res = await _service.RetirarAsync(request);
                return Ok(res);
            }
            catch (InvalidOperationException ex)
            {
                // Devolver error 400 Bad Request cuando no hay saldo
                return BadRequest(new { error = "TransaccionRechazada", mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [HttpPost("aplicar-interes")]
        public async Task<IActionResult> AplicarInteres([FromBody] AplicarInteresRequest request)
        {
            try
            {
                var res = await _service.AplicarInteresAsync(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [HttpGet("{numeroCuenta}/historial")]
        public async Task<IActionResult> ObtenerHistorial(string numeroCuenta)
        {
            try
            {
                var res = await _service.ObtenerHistorialAsync(numeroCuenta);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }
    }
}