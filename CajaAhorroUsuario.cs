using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CajaAhorroUsuario
{
    public int IdUsuario { get; set; }
    public Usuario Usuario { get; set; }
    public int IdCajaAhorro { get; set; }
    public CajaDeAhorro CajaDeAhorro { get; set; }

    public CajaAhorroUsuario() { }

    public CajaAhorroUsuario(int idUsuario, int idCajaAhorro)
    {
        IdUsuario = idUsuario;
        IdCajaAhorro = idCajaAhorro;
    }

















}

