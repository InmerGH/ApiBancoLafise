using BancoApi.Data;
using BancoApi.DTOs;
using BancoApi.Models;
using Microsoft.EntityFrameworkCore;
namespace BancoApi.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly BancoDbContext _context;
        public CuentaService(BancoDbContext context)
        {
            _context = context;
        }
        public async Task<ClienteResponse> CrearClienteAsync(CrearClienteRequest request)
        {
            var cliente = new Cliente
            {
                Nombre = request.Nombre,
                FechaNacimiento = request.FechaNacimiento,
                Sexo = request.Sexo,
                Ingresos = request.Ingresos
            };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return new ClienteResponse(cliente.Id, cliente.Nombre, cliente.FechaNacimiento, cliente.Sexo,
            cliente.Ingresos);
        }
        public async Task<CuentaResponse> CrearCuentaAsync(CrearCuentaRequest request)
        {
            var cliente = await _context.Clientes.FindAsync(request.ClienteId);
            if (cliente == null)
                throw new KeyNotFoundException("El cliente no existe.");
            if (request.SaldoInicial < 0)
                throw new InvalidOperationException("El saldo inicial no puede ser negativo.");
            string numeroCuenta = $"CTA-{Random.Shared.Next(100000, 999999)}";
            var cuenta = new CuentaBancaria
            {
                ClienteId = request.ClienteId,
                NumeroCuenta = numeroCuenta,
                Saldo = request.SaldoInicial
            };
            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();
            if (request.SaldoInicial > 0)
            {
                _context.Transacciones.Add(new Transaccion
                {
                    CuentaBancariaId = cuenta.Id,
                    Tipo = TipoTransaccion.Deposito,
                    Monto = request.SaldoInicial,
                    SaldoResultante = request.SaldoInicial
                });
                await _context.SaveChangesAsync();
            }
            return new CuentaResponse(cuenta.Id, cuenta.NumeroCuenta, cuenta.Saldo, cuenta.ClienteId);
        }
        public async Task<ConsultarSaldoResponse> ObtenerSaldoAsync(string numeroCuenta)
        {
            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
            if (cuenta == null) throw new KeyNotFoundException("Cuenta no encontrada.");
            return new ConsultarSaldoResponse(cuenta.NumeroCuenta, cuenta.Saldo);
        }
        public async Task<ResumenCuentaResponse> DepositarAsync(TransaccionRequest request)
        {
            if (request.Monto <= 0) throw new ArgumentException("Monto debe ser mayor a cero.");
            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == request.NumeroCuenta);
            if (cuenta == null) throw new KeyNotFoundException("Cuenta no encontrada.");
            cuenta.Saldo += request.Monto;
            _context.Transacciones.Add(new Transaccion
            {
                CuentaBancariaId = cuenta.Id,
                Tipo = TipoTransaccion.Deposito,
                Monto = request.Monto,
                SaldoResultante = cuenta.Saldo
            });
            await _context.SaveChangesAsync();
            return await ObtenerHistorialAsync(cuenta.NumeroCuenta);
        }
        public async Task<ResumenCuentaResponse> RetirarAsync(TransaccionRequest request)
        {
            if (request.Monto <= 0) throw new ArgumentException("Monto debe ser mayor a cero.");
            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == request.NumeroCuenta);
            if (cuenta == null) throw new KeyNotFoundException("Cuenta no encontrada.");
            // REGLA CLAVE: Validar saldo suficiente
            if (cuenta.Saldo < request.Monto)
            {
                throw new InvalidOperationException("Transacción rechazada: Fondos insuficientes.");
            }
            cuenta.Saldo -= request.Monto;
            _context.Transacciones.Add(new Transaccion
            {
                CuentaBancariaId = cuenta.Id,
                Tipo = TipoTransaccion.Retiro,
                Monto = request.Monto,
                SaldoResultante = cuenta.Saldo
            });
            await _context.SaveChangesAsync();
            return await ObtenerHistorialAsync(cuenta.NumeroCuenta);
        }
        public async Task<ResumenCuentaResponse> AplicarInteresAsync(AplicarInteresRequest request)
        {
            if (request.TasaPorcentaje <= 0) throw new ArgumentException("Tasa inválida.");
            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == request.NumeroCuenta);
            if (cuenta == null) throw new KeyNotFoundException("Cuenta no encontrada.");
            decimal interes = cuenta.Saldo * (request.TasaPorcentaje / 100m);
            cuenta.Saldo += interes;
            _context.Transacciones.Add(new Transaccion
            {
                CuentaBancariaId = cuenta.Id,
                Tipo = TipoTransaccion.Interes,
                Monto = interes,
                SaldoResultante = cuenta.Saldo
            });
            await _context.SaveChangesAsync();
            return await ObtenerHistorialAsync(cuenta.NumeroCuenta);
        }
        public async Task<ResumenCuentaResponse> ObtenerHistorialAsync(string numeroCuenta)
        {
            var cuenta = await _context.Cuentas
            .Include(c => c.Transacciones)
            .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
            if (cuenta == null) throw new KeyNotFoundException("Cuenta no encontrada.");
            var detalles = cuenta.Transacciones
            .OrderByDescending(t => t.Fecha)
            .Select(t => new TransaccionDetalleDTO(t.Id, t.Tipo.ToString(), t.Monto, t.SaldoResultante,
            t.Fecha))
            .ToList();
            return new ResumenCuentaResponse(cuenta.NumeroCuenta, cuenta.Saldo, detalles);
        }
    }
}
