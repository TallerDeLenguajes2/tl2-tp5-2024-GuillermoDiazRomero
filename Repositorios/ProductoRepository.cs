using Microsoft.Data.Sqlite;
using models;


namespace persistence;

public class ProductoRepository
{
    public void CrearProducto(Productos prod)
    {
        string connectionString = "Data Source=(local);Initial Catalog=Northwind;" + "IntegratedSecurity = true";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            var query = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";
            connection.Open();
            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@Descripcion", prod.Descripcion));
            command.Parameters.Add(new SqliteParameter("@Precio", prod.Precio));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void ModificarProducto(int id, Productos Producto)
    {
        //Buscar Producto
    }

    public List<Productos> ListarProductos()
    {
        List<Productos> listado = new List<Productos>();

        return listado;
    }

    public Productos DetallesProducto(int id)
    {

    }

}