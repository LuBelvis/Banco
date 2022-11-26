using WinFormsApp1;

public class Tarjeta
{

    public int IdTarjeta { get; set; }
    public int Numero { get; set; }
    public int CodigoV { get; set; }
    public double Limite { get; set; }
    public double Consumos { get; set; }
    public int IdUsuario { get; set; }
    public Usuario usuario { get; set; }

    public Tarjeta() { }

    public Tarjeta(int numero, int codigoV, double limite, double consumos, int idUsuario)
    {
        Numero = numero;
        CodigoV = codigoV;
        Limite = limite;
        Consumos = consumos;
        IdUsuario = idUsuario;
    }
}