using WinFormsApp1;

public class PlazoFijo
{
    public int IdPlazoFijo { get; set; }
    public double Monto { get; set; }
    public DateTime FechaIni { get; set; }
    public DateTime FechaFin { get; set; }
    public double Tasa { get; set; }
    public bool IsPagado { get; set; }
    public int IdUsuario { get; set; }
    public Usuario Usuario { get; set; }
    public int CbuAPagar { get; set; }

    public PlazoFijo() { }

    public PlazoFijo(double monto, DateTime fechaIni, DateTime fechaFin, double tasa, bool isPagado, int idUsuario, int cbuAPagar)
    {
        Monto = monto;
        Tasa = tasa;
        FechaIni = fechaIni;
        FechaFin = fechaFin;
        IsPagado = isPagado;
        IdUsuario = idUsuario;
        CbuAPagar = cbuAPagar;
    }
}