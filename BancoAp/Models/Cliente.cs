using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BancoApi.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public decimal Ingresos { get; set; }

        public ICollection<CuentaBancaria> Cuentas { get; set; } = new List<CuentaBancaria>();


    }
}
