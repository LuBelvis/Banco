public class Movimiento
{
    public int IdMovimiento { get; set; }
    public string Detalle { get; set; }
    public double Monto { get; set; }
    public DateTime Fecha { get; set; }
    public int IdCajaAhorro { get; set; }
    public CajaDeAhorro CajaDeAhorro { get; set; }

    public Movimiento() { }

    public Movimiento(string detalle, double monto, DateTime fecha, int idCaja)
    {
        Detalle = detalle;
        Monto = monto;
        Fecha = fecha;
        IdCajaAhorro = idCaja;
    }
}