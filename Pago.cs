using WinFormsApp1;

public class Pago
{

    public int IdPago { get; set; }
    public string Detalle { get; set; }
    public double Monto { get; set; }
    public bool IsPagado { get; set; }
    public string? Metodo { get; set; }
    public int IdUsuario { get; set; }
    public Usuario Usuario { get; set; }

    public Pago() { }

    public Pago(string detalle, double monto, bool isPagado, string metodo, int idUsuario)
    {
        Detalle = detalle;
        Monto = monto;
        IsPagado = isPagado;
        Metodo = metodo;
        IdUsuario = idUsuario;
    }
}
