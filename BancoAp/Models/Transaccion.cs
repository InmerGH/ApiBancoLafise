namespace BancoApi.Models
{
    public enum TipoTransaccion
    {
        Deposito = 1,
        Retiro = 2,
        Interes = 3
    }
    public class Transaccion
    {
        public int Id { get; set; }
        public TipoTransaccion Tipo { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoResultante { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public int CuentaBancariaId { get; set; }
        public CuentaBancaria? CuentaBancaria { get; set; }
    }
}

