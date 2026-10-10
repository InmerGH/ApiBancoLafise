using BancoApi.DTOs;
namespace BancoApi.Services
{
    public interface ICuentaService
    {
        Task<ClienteResponse> CrearClienteAsync(CrearClienteRequest request);
        Task<CuentaResponse> CrearCuentaAsync(CrearCuentaRequest request);
        Task<ConsultarSaldoResponse> ObtenerSaldoAsync(string numeroCuenta);
        Task<ResumenCuentaResponse> DepositarAsync(TransaccionRequest request);
        Task<ResumenCuentaResponse> RetirarAsync(TransaccionRequest request);
        Task<ResumenCuentaResponse> AplicarInteresAsync(AplicarInteresRequest request);
        Task<ResumenCuentaResponse> ObtenerHistorialAsync(string numeroCuenta);
    }
}
