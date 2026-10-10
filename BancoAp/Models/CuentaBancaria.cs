using Microsoft.AspNetCore.Mvc;

namespace BancoApi.Models
{
    public class CuentaBancaria
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}
