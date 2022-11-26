using WinFormsApp1;

public class CajaDeAhorro
{
    public int IdCajaAhorro { get; set; }
    public int Cbu { get; set; }
    public double Saldo { get; set; }
    public List<Movimiento> Movimientos { get; set; }
    public ICollection<Usuario> Titulares { get; set; } = new List<Usuario>();
    public List<CajaAhorroUsuario> CajaAhorroUsuarios { get; set; }

    public CajaDeAhorro() { }

    public CajaDeAhorro(int cbu, double saldo)
    {
        Cbu = cbu;
        Saldo = saldo;
    }
}