namespace models;

public class PresupuestosDetalle{
    private Productos producto;
    private int cantidad;

    public PresupuestosDetalle(Productos producto, int cantidad)
    {
        this.producto = producto;
        this.cantidad = cantidad;
    }

    public Productos Producto { get => producto; set => producto = value; }
    public int Cantidad { get => cantidad; set => cantidad = value; }
}