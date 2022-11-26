using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WinFormsApp1;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class Banco
{

    private MyContext contexto;
    public Usuario? UsuarioActual { get; set; }
    int nuevoNumTarjeta;
    int nuevoCbu;
    public Banco()
    {
        InicializarAtributos();
        nuevoNumTarjeta = CrearNumeroTarjeta();
        nuevoCbu = CrearCbu();
        PagarPlazoFijo();

    }

    private void InicializarAtributos()
    {
        try
        {
            contexto = new MyContext();
            contexto.Usuarios
                .Include(u => u.PlazosFijos)
                .Include(u => u.Tarjetas)
                .Include(u => u.Pagos)
                .Include(u => u.Cajas)
                .Include(u => u.CajaAhorroUsuarios).Load();
            contexto.Cajas
                .Include(c => c.Movimientos)
                .Include(c => c.Titulares)
                .Include(c => c.CajaAhorroUsuarios).Load();
            contexto.Plazos.Load();
            contexto.Tarjetas.Load();
            contexto.Pagos.Load();
            contexto.Movimientos.Load();
            contexto.CajasUsuario.Load();
        }
        catch (Exception e)
        {
            Console.Write(e.ToString());
        }
    }


    // MOSTRAR DATOS

    public List<Usuario> MostrarUsuarios()
    {
        return contexto.Usuarios.ToList();
    }

    public List<Usuario> MostrarUsuariosBloqueados()
    {
        return contexto.Usuarios.Where(u => !u.IsBloqueado).ToList();
    }

    public List<CajaDeAhorro> MostrarCajasDeAhorroUsuarioActual()
    {
        return UsuarioActual.Cajas.ToList();
    }

    public List<CajaDeAhorro> MostrarCajasDeAhorro()
    {
        return contexto.Cajas.ToList();
    }

    public List<CajaDeAhorro> MostrarCajasDeAhorroByIdUsuario(int IdTitular)
    {
        List<CajaDeAhorro>? resultado = new List<CajaDeAhorro>();

        foreach (CajaDeAhorro caja in contexto.Cajas)
        {
            foreach (Usuario titular in caja.Titulares)
            {
                if (titular.IdUsuario == IdTitular)
                {
                    resultado.Add(caja);
                }
            }
        }

        contexto.SaveChanges();
        return resultado;
    }

    public List<Movimiento> MostrarMovimientosUsuarioActual(int IdCajaAhorro)
    {
        List<Movimiento> resultado = new List<Movimiento>();

        foreach (CajaDeAhorro caja in UsuarioActual.Cajas)
        {
            if (caja.Movimientos.Count > 0 && caja.IdCajaAhorro == IdCajaAhorro)
            {
                foreach (Movimiento movimiento in caja.Movimientos)
                {
                    resultado.Add(movimiento);
                }
            }
        }

        contexto.SaveChanges();
        return resultado;
    }

    public List<Movimiento> MostrarMovimientos(int IdCajaAhorro)
    {
        return contexto.Movimientos.Where(m => m.IdCajaAhorro == IdCajaAhorro).ToList();
    }

    public List<Pago> MostrarPagosPendiente()
    {
        return contexto.Pagos.Where(p => !p.IsPagado).ToList();
    }

    public List<Pago> MostrarPagosRealizado()
    {
        return contexto.Pagos.Where(p => p.IsPagado).ToList();
    }

    public List<Pago> MostrarPagosPendienteByIdUsuario(int IdUsuario)
    {
        return UsuarioActual.Pagos.Where(p => p.IdUsuario == IdUsuario).ToList();
    }

    public List<Pago> MostrarPagosRealizadoByIdUsuario(int IdUsuario)
    {
        return contexto.Pagos.Where(p => (p.IsPagado) && (p.IdUsuario == IdUsuario)).ToList();
    }

    public List<Pago> MostrarPagosPendienteUsuarioActual()
    {
        return UsuarioActual.Pagos.Where(p => !p.IsPagado).ToList();
    }

    public List<Pago> MostrarPagosRealizadoUsuarioActual()
    {
        return UsuarioActual.Pagos.Where(p => p.IsPagado).ToList();
    }

    public List<PlazoFijo> MostrarPlazosFijosUsuarioActual()
    {

        return UsuarioActual.PlazosFijos.ToList();
    }

    public List<PlazoFijo> MostrarPlazosFijos()
    {
        return contexto.Plazos.ToList();
    }

    public List<PlazoFijo> MostrarPlazosFijosByIdUsuario(int IdUsuario)
    {
        return contexto.Plazos.Where(p => p.IdUsuario == IdUsuario).ToList();
    }

    public List<Tarjeta> MostrarTarjetasDeCreditoUsuarioActual()
    {
        return UsuarioActual.Tarjetas.ToList();
    }

    public List<Tarjeta> MostrarTarjetasDeCredito()
    {
        return contexto.Tarjetas.ToList();
    }

    public List<Tarjeta> MostrarTarjetasDeCreditoByIdUsuario(int IdUsuario)
    {
        return contexto.Tarjetas.Where(t => t.IdUsuario == IdUsuario).ToList();
    }


    // METODOS COMPLEMENTARIOS 


    private Usuario? BuscarUsuarioPorDni(int Dni)
    {
        return contexto.Usuarios.Where(u => u.Dni == Dni).FirstOrDefault();
    }

    public Usuario? BuscarUsuarioPorId(int IdUsuario)
    {
        return contexto.Usuarios.Where(u => u.IdUsuario == IdUsuario).FirstOrDefault();
    }

    public CajaDeAhorro? BuscarCajaPorIdUsuarioActual(int IdCajaAhorro)
    {
        return UsuarioActual.Cajas.Where(c => c.IdCajaAhorro == IdCajaAhorro).FirstOrDefault();
    }

    public CajaDeAhorro? BuscarCajaPorId(int IdCajaAhorro)
    {
        return contexto.Cajas.Where(c => c.IdCajaAhorro == IdCajaAhorro).FirstOrDefault();
    }

    public CajaDeAhorro? BuscarCajaPorCbu(int Cbu)
    {
        return contexto.Cajas.Where(c => c.Cbu == Cbu).FirstOrDefault();
    }

    public PlazoFijo? BuscarPlazoPorIdUsuarioActual(int IdPlazoFijo)
    {
        return UsuarioActual.PlazosFijos.Where(p => p.IdPlazoFijo == IdPlazoFijo).FirstOrDefault();
    }

    public PlazoFijo? BuscarPlazoPorId(int IdPlazoFijo)
    {
        return contexto.Plazos.Where(p => p.IdPlazoFijo == IdPlazoFijo).FirstOrDefault();
    }

    public Usuario BuscarUsuarioBloqueado(string Nombre, int Dni)
    {
        return contexto.Usuarios.Where(u => (u.Nombre.Equals(Nombre)) && (u.Dni == Dni)).FirstOrDefault();
    }


    //OPERACIONES DEL USUARIO


    public Usuario IniciarSesion(string Nombre, int Dni, string Clave)
    {
        return UsuarioActual = contexto.Usuarios.Where(u => (u.Nombre.Equals(Nombre)) && (u.Dni == Dni) && (u.Clave.Equals(Clave))).FirstOrDefault();
    }

    public bool DesbloquearUsuario(int IdUsuario)
    {
        bool resultado = false;

        foreach (Usuario usuario in contexto.Usuarios)
        {
            if (usuario.IdUsuario == IdUsuario)
            {
                usuario.IsBloqueado = false;

                resultado = true;
            }
        }

        contexto.SaveChanges();
        return resultado;
    }

    public void CerrarSesion()
    {
        UsuarioActual = null;
    }

    public List<Movimiento> BuscarMovimiento(int IdCajaAhorro, string? Detalle, DateTime? Fecha, double? Monto)
    {

        List<Movimiento> resultado = new List<Movimiento>();
        CajaDeAhorro cajaDeAhorro = BuscarCajaPorIdUsuarioActual(IdCajaAhorro);

        foreach (Movimiento movimiento in cajaDeAhorro.Movimientos)
        {
            if (movimiento.Detalle.Equals(Detalle) || movimiento.Fecha == Fecha || movimiento.Monto == Monto)
            {
                resultado.Add(movimiento);
            }
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool AltaDeUsuario(int Dni, string Nombre, string Apellido, string Mail, string Clave, bool IsAdmin, bool Bloqueado)
    {
        bool resultado = false;
        bool dniOk = true;
        bool mailOk = true;

        foreach (Usuario usuario in contexto.Usuarios)
        {
            if (usuario.Dni == Dni)
                dniOk = false;
        }

        foreach (Usuario usuario in contexto.Usuarios)
        {
            if (usuario.Mail.Equals(Mail))
            {
                mailOk = false;
            }
        }

        if (dniOk && mailOk)
        {
            try
            {
                Usuario nuevo = new Usuario(Dni, Nombre, Apellido, Mail, Clave, IsAdmin, Bloqueado);
                contexto.Usuarios.Add(nuevo);

                resultado = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                resultado = false;
            }
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool ModificarUsuarioAdmin(int IdUsuario, int Dni, string Nombre, string Apellido, string Mail, string Clave, bool Admin, bool Bloqueado)
    {
        bool resultado = false;

        try
        {
            foreach (Usuario usuario in contexto.Usuarios)
            {
                if (usuario.IdUsuario == IdUsuario)
                {
                    usuario.Dni = Dni;
                    usuario.Nombre = Nombre;
                    usuario.Apellido = Apellido;
                    usuario.Mail = Mail;
                    usuario.Clave = Clave;
                    usuario.IsAdmin = Admin;
                    contexto.Usuarios.Update(usuario);
                    resultado = true;
                }
                else if (usuario.IdUsuario == IdUsuario && usuario.IsAdmin == true)
                {
                    usuario.IsBloqueado = Bloqueado;
                    contexto.Usuarios.Update(usuario);
                    resultado = true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool ModificarUsuarioUsuarioActual(int Dni, string Nombre, string Apellido, string Mail, string Clave)
    {
        bool resultado;

        if (Dni == 0)
        {
            Dni = UsuarioActual.Dni;
        }

        if (Nombre.Equals(""))
        {
            Nombre = UsuarioActual.Nombre;
        }

        if (Apellido.Equals(""))
        {
            Apellido = UsuarioActual.Apellido;
        }

        if (Mail.Equals(""))
        {
            Mail = UsuarioActual.Mail;
        }

        if (Clave.Equals(""))
        {
            Clave = UsuarioActual.Clave;
        }

        try
        {
            UsuarioActual.Dni = Dni;
            UsuarioActual.Nombre = Nombre;
            UsuarioActual.Apellido = Apellido;
            UsuarioActual.Mail = Mail;
            UsuarioActual.Clave = Clave;
            contexto.Usuarios.Update(UsuarioActual);

            resultado = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }


    public bool EliminarUsuarioAdmin(int IdUsuario)
    {
        bool resultado = false;
        bool resultado1 = false;
        bool resultado2 = false;
        bool resultado3 = false;
        bool resultado4 = false;

        Usuario? usuario = BuscarUsuarioPorId(IdUsuario);

        try
        {
            foreach (CajaDeAhorro caja in usuario.Cajas)
            {
                if (BajaCajaAhorro(caja.IdCajaAhorro))
                {
                    resultado1 = true;
                }
            }

            foreach (PlazoFijo plazoFijos in usuario.PlazosFijos)
            {
                if (BajaPlazoFijo(plazoFijos.IdPlazoFijo))
                {
                    resultado2 = true;
                }
            }

            foreach (Tarjeta tarjetas in usuario.Tarjetas)
            {
                if (BajaTarjetaCredito(tarjetas.IdTarjeta))
                {
                    resultado3 = true;
                }
            }

            foreach (Pago pagos in usuario.Pagos)
            {
                if (BajaPago(pagos.IdPago))
                {
                    resultado4 = true;
                }
            }

            if (resultado1 && resultado2 && resultado3 && resultado4)
            {
                contexto.Usuarios.Remove(usuario);
                resultado = true;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool EliminarUsuarioUsuarioActual()
    {
        bool resultado = false;
        bool resultado1 = false;
        bool resultado2 = false;
        bool resultado3 = false;
        bool resultado4 = false;

        try
        {
            foreach (CajaDeAhorro caja in UsuarioActual.Cajas)
            {
                if (UsuarioActual.Cajas.Count == 0)
                {
                    BajaCajaAhorro(caja.IdCajaAhorro);
                    resultado1 = true;
                }
            }

            foreach (PlazoFijo plazoFijos in UsuarioActual.PlazosFijos)
            {
                if (UsuarioActual.PlazosFijos.Count == 0)
                {
                    BajaPlazoFijo(plazoFijos.IdPlazoFijo);
                    resultado2 = true;
                }
            }

            foreach (Tarjeta tarjetas in UsuarioActual.Tarjetas)
            {
                if (UsuarioActual.Tarjetas.Count == 0)
                {
                    BajaTarjetaCredito(tarjetas.IdTarjeta);
                    resultado3 = true;
                }
            }

            foreach (Pago pagos in UsuarioActual.Pagos)
            {
                if (UsuarioActual.Pagos.Count == 0)
                {
                    BajaPago(pagos.IdPago);
                    resultado4 = true;
                }
            }

            if (resultado1 && resultado2 && resultado3 && resultado4)
            {
                contexto.Usuarios.Remove(UsuarioActual);
                resultado = true;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool AltaCajaAhorroUsuarioActual()
    {
        bool resultado;

        double Saldo = 0;
        int cbuNuevaCaja = nuevoCbu;

        try
        {
            CajaDeAhorro nuevaCaja = new CajaDeAhorro(cbuNuevaCaja, Saldo);
            contexto.Cajas.Add(nuevaCaja);
            UsuarioActual.Cajas.Add(nuevaCaja);
            contexto.Usuarios.Update(UsuarioActual);
            nuevoCbu++;

            resultado = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool AltaCajaAhorroAdmin(int idUsuario)
    {
        bool resultado;

        double Saldo = 0;
        int cbuNuevaCaja = nuevoCbu;
        Usuario usuario = BuscarUsuarioPorId(idUsuario);
        try
        {
            CajaDeAhorro nuevaCaja = new CajaDeAhorro(cbuNuevaCaja, Saldo);
            contexto.Cajas.Add(nuevaCaja);
            usuario.Cajas.Add(nuevaCaja);
            contexto.Usuarios.Update(usuario);
            nuevoCbu++;

            resultado = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool ModificarCajaAhorro(int DniUsuario, int IdCajaAhorro, string accion)
    {
        bool resultado = false;

        CajaDeAhorro? caja = BuscarCajaPorId(IdCajaAhorro);
        Usuario? titular = BuscarUsuarioPorDni(DniUsuario);

        try
        {
            if (accion.Equals("agregar"))
            {
                caja.Titulares.Add(titular);
                titular.Cajas.Add(caja);

                resultado = true;
            }
            else if (accion.Equals("eliminar") && caja.Titulares.Count > 1)
            {
                caja.Titulares.Remove(titular);
                titular.Cajas.Remove(caja);

                resultado = true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool BajaCajaAhorro(int IdCajaAhorro)
    {
        bool resultado = false;

        try
        {
            foreach (Usuario usuario in contexto.Usuarios)
            {
                foreach (CajaDeAhorro caja in usuario.Cajas)
                {
                    if (caja.IdCajaAhorro == IdCajaAhorro)
                    {
                        if (caja.Saldo == 0)
                        {
                            contexto.Cajas.Remove(caja);
                            UsuarioActual.Cajas.Remove(caja);

                            resultado = true;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool BajaCajaAhorroUsuarioActual(int IdCajaAhorro)
    {
        try
        {
            foreach (CajaDeAhorro caja in UsuarioActual.Cajas)
            {
                if (caja.IdCajaAhorro == IdCajaAhorro)
                {
                    if (caja.Saldo == 0)
                    {
                        contexto.Cajas.Remove(caja);
                        UsuarioActual.Cajas.Remove(caja);


                        contexto.SaveChanges();
                        return true;
                    }
                }
            }

            contexto.SaveChanges();
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());

            contexto.SaveChanges();
            return false;
        }
    }

    public bool AltaPago(int IdUsuario, string Detalle, double Monto, bool IsPagado, string Metodo)
    {
        bool resultado;

        try
        {
            Pago nuevo = new Pago(Detalle, Monto, IsPagado, Metodo, IdUsuario);
            contexto.Pagos.Add(nuevo);
            UsuarioActual.Pagos.Add(nuevo);
            resultado = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool AltaPagoUsuarioActual(string Detalle, double Monto, bool IsPagado, string Metodo)
    {
        bool resultado;

        try
        {
            Pago nuevo = new Pago(Detalle, Monto, IsPagado, Metodo, UsuarioActual.IdUsuario);
            contexto.Pagos.Add(nuevo);
            UsuarioActual.Pagos.Add(nuevo);
            resultado = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool ModificarPago(int IdPago, string Detalle, double Monto, bool IsPagado, string Metodo)
    {
        bool resultado = false;

        try
        {
            foreach (Pago pago in contexto.Pagos)
            {
                if (pago.IdPago == IdPago)
                {
                    pago.Detalle = Detalle;
                    pago.Monto = Monto;
                    pago.Metodo = Metodo;
                    pago.IsPagado = IsPagado;
                    contexto.Pagos.Update(pago);
                    resultado = true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;

    }


    public bool BajaPago(int IdPago)
    {
        bool resultado = false;

        try
        {
            foreach (Pago pago in contexto.Pagos)
            {
                if (pago.IdPago == IdPago)
                {
                    if (pago.IsPagado)
                    {
                        contexto.Pagos.Remove(pago);
                        resultado = true;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool BajaPagoUsuarioActual(int IdPago)
    {
        bool resultado = false;

        try
        {
            foreach (Pago pago in UsuarioActual.Pagos)
            {
                if (pago.IdPago == IdPago)
                {
                    if (pago.IsPagado)
                    {
                        UsuarioActual.Pagos.Remove(pago);
                        contexto.Pagos.Remove(pago);
                        resultado = true;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool AltaPlazoFijo(int IdCajaAhorro, double Monto, DateTime FechaIni, DateTime FechaFin, double Tasa, bool IsPagado, int IdUsuario)
    {
        bool resultado = false;
        string DetalleMovimiento = "Alta Plazo Fijo";

        try
        {
            CajaDeAhorro? cajaAPagar = BuscarCajaPorIdUsuarioActual(IdCajaAhorro);

            if (cajaAPagar.Saldo >= Monto)
            {
                PlazoFijo plazoFijo = new PlazoFijo(Monto, FechaIni, FechaFin, Tasa, IsPagado, IdUsuario, cajaAPagar.Cbu);

                contexto.Plazos.Add(plazoFijo);
                UsuarioActual.PlazosFijos.Add(plazoFijo);

                Movimiento? movimiento = AltaMovimiento(DetalleMovimiento, Monto, IdCajaAhorro);

                cajaAPagar.Saldo -= Monto;

                cajaAPagar.Movimientos.Add(movimiento);

                resultado = true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool AltaPlazoFijoUsuarioActual(int IdCajaAhorro, double Monto, DateTime FechaIni, DateTime FechaFin, double Tasa, bool IsPagado)
    {
        bool resultado = false;
        string DetalleMovimiento = "Alta Plazo Fijo";

        try
        {
            CajaDeAhorro? cajaAPagar = BuscarCajaPorIdUsuarioActual(IdCajaAhorro);

            if (cajaAPagar.Saldo >= Monto)
            {
                PlazoFijo plazoFijo = new PlazoFijo(Monto, FechaIni, FechaFin, Tasa, IsPagado, UsuarioActual.IdUsuario, cajaAPagar.Cbu);

                contexto.Plazos.Add(plazoFijo);
                UsuarioActual.PlazosFijos.Add(plazoFijo);

                Movimiento? movimiento = AltaMovimiento(DetalleMovimiento, Monto, cajaAPagar.IdCajaAhorro);

                cajaAPagar.Saldo -= Monto;

                cajaAPagar.Movimientos.Add(movimiento);

                resultado = true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool PagarPlazoFijo()
    {
        bool resultado = false;
        PlazoFijo plazo = null;

        try
        {
            foreach (PlazoFijo p in contexto.Plazos)
            {
                TimeSpan comparacionDeFechas = DateTime.Now.Subtract(p.FechaFin);
                if (!p.IsPagado && comparacionDeFechas.Days >= 0)
                {
                    plazo = p;
                    contexto.SaveChanges();
                }
            }

            foreach (CajaDeAhorro caja in contexto.Cajas)
            {
                if (plazo != null && caja.Cbu == plazo.CbuAPagar)
                {
                    caja.Saldo += (plazo.Monto + ((plazo.Monto * plazo.Tasa) / 365) * (365));
                    plazo.IsPagado = true;
                    contexto.Update(plazo);
                    contexto.Update(caja);
                    resultado = true;
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool BajaPlazoFijo(int IdPlazoFijo)
    {
        bool resultado = false;

        try
        {
            PlazoFijo plazoFijo = BuscarPlazoPorId(IdPlazoFijo);

            if (plazoFijo.IsPagado)
            {

                TimeSpan comparacionDeFechas = DateTime.Now.Subtract(plazoFijo.FechaFin);

                if (comparacionDeFechas.Days >= 30)
                {

                    contexto.Plazos.Remove(plazoFijo);
                    UsuarioActual.PlazosFijos.Remove(plazoFijo);

                    resultado = true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool BajaPlazoFijoUsuarioActual(int IdPlazoFijo)
    {
        bool resultado = false;

        try
        {
            PlazoFijo? plazoFijo = BuscarPlazoPorIdUsuarioActual(IdPlazoFijo);

            if (plazoFijo.IsPagado)
            {

                TimeSpan comparacionDeFechas = DateTime.Now.Subtract(plazoFijo.FechaFin);

                if (comparacionDeFechas.Days >= 30)
                {

                    contexto.Plazos.Remove(plazoFijo);
                    UsuarioActual.PlazosFijos.Remove(plazoFijo);

                    resultado = true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool AltaTarjetaCredito(int IdUsuario)
    {
        bool resultado;

        Random codigoV = new Random();
        int numeroTarjeta = nuevoNumTarjeta;
        int codigoTarjeta = codigoV.Next(100, 999);
        double limiteTarjeta = 25000;
        double consumoTarjeta = 0;

        try
        {
            Tarjeta nuevaTarjeta = new Tarjeta(numeroTarjeta, codigoTarjeta, limiteTarjeta, consumoTarjeta, IdUsuario);
            UsuarioActual.Tarjetas.Add(nuevaTarjeta);
            nuevoNumTarjeta++;
            contexto.Tarjetas.Add(nuevaTarjeta);
            contexto.SaveChanges();
            resultado = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool AltaTarjetaCreditoUsuarioActual()
    {
        bool resultado;

        Random codigoV = new Random();
        int numeroTarjeta = nuevoNumTarjeta;
        int codigoTarjeta = codigoV.Next(100, 999);
        double limiteTarjeta = 25000;
        double consumoTarjeta = 0;

        try
        {
            Tarjeta nuevaTarjeta = new Tarjeta(numeroTarjeta, codigoTarjeta, limiteTarjeta, consumoTarjeta, UsuarioActual.IdUsuario);
            UsuarioActual.Tarjetas.Add(nuevaTarjeta);
            nuevoNumTarjeta++;
            contexto.Tarjetas.Add(nuevaTarjeta);
            contexto.SaveChanges();
            resultado = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool ModificarTarjetaCredito(int IdTarjeta, double NuevoLimite)
    {
        bool resultado = false;

        try
        {
            foreach (Tarjeta tarjeta in contexto.Tarjetas)
            {
                if (tarjeta.IdTarjeta == IdTarjeta)
                {
                    tarjeta.Limite = NuevoLimite;
                    contexto.Tarjetas.Update(tarjeta);
                    resultado = true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool BajaTarjetaCredito(int IdTarjeta)
    {
        bool resultado = false;

        try
        {
            foreach (Tarjeta tarjeta in contexto.Tarjetas)
            {
                if (tarjeta.IdTarjeta == IdTarjeta && tarjeta.Consumos == 0)
                {
                    contexto.Tarjetas.Remove(tarjeta);

                    resultado = true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool Depositar(int IdCajaAhorro, double Monto)
    {

        bool resultado = false;

        string detalleMovimiento = "Deposito";

        try
        {
            if (Monto > 0)
            {
                foreach (CajaDeAhorro caja in contexto.Cajas)
                {
                    if (caja.IdCajaAhorro == IdCajaAhorro)
                    {
                        caja.Saldo += Monto;

                        Movimiento movimiento = AltaMovimiento(detalleMovimiento, Monto, caja.IdCajaAhorro);
                        caja.Movimientos.Add(movimiento);
                        contexto.Cajas.Update(caja);
                        resultado = true;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool Retirar(int IdCajaAhorro, double Monto)
    {
        bool resultado = false;

        string detalleMovimiento = "Retiro";

        try
        {
            foreach (CajaDeAhorro caja in contexto.Cajas)
            {
                if (caja.IdCajaAhorro == IdCajaAhorro)
                {
                    if (caja.Saldo >= Monto)
                    {
                        caja.Saldo -= Monto;

                        Movimiento movimiento = AltaMovimiento(detalleMovimiento, Monto, caja.IdCajaAhorro);
                        caja.Movimientos.Add(movimiento);
                        contexto.Cajas.Update(caja);
                        resultado = true;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool Transferir(int IdCajaOrigen, int CbuCajaDestino, double Monto)
    {
        bool resultado = false;

        try
        {
            CajaDeAhorro? cajaOrigen = BuscarCajaPorIdUsuarioActual(IdCajaOrigen);
            CajaDeAhorro? cajaDestino = BuscarCajaPorCbu(CbuCajaDestino);
            string detalleMovimiento = "Transferir";

            if (cajaDestino != null && cajaOrigen.Saldo >= Monto)
            {
                cajaOrigen.Saldo -= Monto;
                cajaDestino.Saldo += Monto;

                Movimiento movimientoCajaOrigen = AltaMovimiento(detalleMovimiento, Monto, cajaOrigen.IdCajaAhorro);
                Movimiento movimientoCajaDestino = AltaMovimiento(detalleMovimiento, Monto, cajaDestino.IdCajaAhorro);

                cajaOrigen.Movimientos.Add(movimientoCajaOrigen);
                cajaDestino.Movimientos.Add(movimientoCajaDestino);
                contexto.Movimientos.Add(movimientoCajaOrigen);
                contexto.Movimientos.Add(movimientoCajaDestino);

                resultado = true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool PagarTarjeta(int IdTarjeta, int IdCajaAhorro)
    {

        bool resultado = false;

        try
        {
            foreach (CajaDeAhorro caja in UsuarioActual.Cajas)
            {
                if (caja.IdCajaAhorro == IdCajaAhorro)
                {
                    foreach (Tarjeta tarjeta in contexto.Tarjetas)
                    {
                        if (tarjeta.IdTarjeta == IdTarjeta)
                        {
                            if (caja.Saldo >= tarjeta.Consumos)
                            {
                                string detalleMovimiento = "Pago Tarjeta";
                                Movimiento movimiento = AltaMovimiento(detalleMovimiento, tarjeta.Consumos, caja.IdCajaAhorro);

                                caja.Saldo -= tarjeta.Consumos;
                                tarjeta.Consumos = 0;

                                caja.Movimientos.Add(movimiento);
                                contexto.Movimientos.Add(movimiento);

                                resultado = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public bool PagarPago(int IdCajaAhorro, int IdTarjetaCredito, int IdPago)
    {
        bool resultado = false;

        Pago pago = null;
        bool pagoOk = false;
        string metodo = "";

        try
        {
            foreach (Pago p in contexto.Pagos)
            {
                if (p.IdPago == IdPago)
                {
                    pago = p;
                    pagoOk = true;
                    contexto.SaveChanges();
                }
            }

            if (IdCajaAhorro != 0 && pagoOk)
            {
                foreach (CajaDeAhorro caja in contexto.Cajas)
                {
                    if (caja.IdCajaAhorro == IdCajaAhorro)
                    {
                        if (caja.Saldo >= pago.Monto)
                        {
                            metodo = "Caja de Ahorro";

                            caja.Saldo -= pago.Monto;

                            string detalleMovimiento = "Pagar pago";
                            Movimiento movimiento = AltaMovimiento(detalleMovimiento, pago.Monto, caja.IdCajaAhorro);
                            caja.Movimientos.Add(movimiento);
                            contexto.Movimientos.Add(movimiento);

                            resultado = true;
                        }
                    }
                }
            }
            else if (IdTarjetaCredito != 0 && pagoOk)
            {
                foreach (Tarjeta tarjeta in contexto.Tarjetas)
                {
                    if (tarjeta.IdTarjeta == IdTarjetaCredito)
                    {
                        if (tarjeta.Limite >= (tarjeta.Consumos + pago.Monto))
                        {
                            metodo = "Tarjeta";

                            tarjeta.Consumos += pago.Monto;
                            contexto.Tarjetas.Update(tarjeta);
                            resultado = true;
                        }
                    }
                }
            }

            ModificarPago(pago.IdPago, pago.Detalle, pago.Monto, true, metodo);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            resultado = false;
        }

        contexto.SaveChanges();
        return resultado;
    }

    public int CrearCbu()
    {
        int cbu = 100;

        try
        {
            foreach (CajaDeAhorro caja in contexto.Cajas)
            {
                if (caja.Cbu > 99)
                {
                    cbu = caja.Cbu + 1;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        contexto.SaveChanges();
        return cbu;
    }

    public int CrearNumeroTarjeta()
    {
        int numeroTarjeta = 10000000;

        try
        {
            foreach (Tarjeta tarjeta in contexto.Tarjetas)
            {
                if (tarjeta.Numero > 9999999)
                {
                    numeroTarjeta = tarjeta.Numero + 1;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        contexto.SaveChanges();
        return numeroTarjeta;
    }

    public Movimiento AltaMovimiento(string DetalleMovimiento, double Monto, int IdCajaAhorro)
    {
        DateTime FechaMovimiento = DateTime.Now.Date;
        Movimiento nuevoMovimiento = null;

        try
        {
            nuevoMovimiento = new Movimiento(DetalleMovimiento, Monto, FechaMovimiento, IdCajaAhorro);

            contexto.Movimientos.Add(nuevoMovimiento);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return nuevoMovimiento;
        }

        return nuevoMovimiento;
    }

    public void Cerrar()
    {
        contexto.Dispose();
    }
}