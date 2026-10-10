using BancoApi.Models;
namespace BancoApi.DTOs
{
    public record CrearClienteRequest(string Nombre, DateTime FechaNacimiento, string Sexo, decimal Ingresos);
    public record ClienteResponse(int Id, string Nombre, DateTime FechaNacimiento, string Sexo, decimal
    Ingresos);
    public record CrearCuentaRequest(int ClienteId, decimal SaldoInicial);
    public record CuentaResponse(int Id, string NumeroCuenta, decimal Saldo, int ClienteId);
    public record ConsultarSaldoResponse(string NumeroCuenta, decimal SaldoActual);
    public record TransaccionRequest(string NumeroCuenta, decimal Monto);
    public record AplicarInteresRequest(string NumeroCuenta, decimal TasaPorcentaje);
    public record TransaccionDetalleDTO(int Id, string Tipo, decimal Monto, decimal SaldoResultante, DateTime
    Fecha);
    public record ResumenCuentaResponse(string NumeroCuenta, decimal SaldoFinal, List<TransaccionDetalleDTO>
    Transacciones);
}
