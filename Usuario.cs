public class Usuario
{

    public int IdUsuario { get; set; }
    public int Dni { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Mail { get; set; }
    public string Clave { get; set; }
    public int IntentosFallidos { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsBloqueado { get; set; }

    public List<PlazoFijo> PlazosFijos { get; set; }
    public List<Tarjeta> Tarjetas { get; set; }
    public List<Pago> Pagos { get; set; }
    public ICollection<CajaDeAhorro> Cajas { get; set; } = new List<CajaDeAhorro>();
    public List<CajaAhorroUsuario> CajaAhorroUsuarios { get; set; }

    public Usuario() { }


    public Usuario(int dni, string nombre, string apellido, string mail, string clave, bool isAdmin, bool isBloqueado)
    {
        Dni = dni;
        Nombre = nombre;
        Apellido = apellido;
        Mail = mail;
        Clave = clave;
        IsAdmin = isAdmin;
        IsBloqueado = isBloqueado;
        Cajas = new List<CajaDeAhorro>();
        Tarjetas = new List<Tarjeta>();
        Pagos = new List<Pago>();
        PlazosFijos = new List<PlazoFijo>();

    }
}
