namespace models;

public class Presupuestos{
    private int idPresupuesto;
    private string nombreDestinatario;
    List<PresupuestosDetalle> detalle;

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public List<PresupuestosDetalle> Detalle { get => detalle; set => detalle = value; }

    public Presupuestos(int idPresupuesto, string nombreDestinatario, List<PresupuestosDetalle> detalle)
    {
        this.idPresupuesto = idPresupuesto;
        this.nombreDestinatario = nombreDestinatario;
        this.detalle = detalle;
    }

    public int MontoPresupuesto(){
        return detalle.Select(d => d.Producto.Precio * d.Cantidad).Sum();
    }
    public double MontoPresupuestoConIVA(){
        return MontoPresupuesto() * 1.21;
    }

    public int CantidadProductor(){
        return detalle.Select(d => d.Cantidad).Sum();
    }
}