using Microsoft.Data.Sqlite;
using models;


namespace persistence;

public class ProductoRepository
{
    private string conexionString = "Data Source=db/Tienda.db;Cache=Shared";

    public void CrearProducto(Productos prod)
    {
        using (SqliteConnection conexion = new SqliteConnection(conexionString))
        {
            var query = "INSERT INTO Productos (Descripcion, Precio) VALUES (@dscr, @precio)";
            conexion.Open();
            var command = new SqliteCommand(query, conexion);
            command.Parameters.Add(new SqliteParameter("@dscr", prod.Descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", prod.Precio));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public void ModificarProducto(int id, Productos prod)
    {
        using (SqliteConnection conexion = new SqliteConnection(conexionString))
        {
            var query = @"UPDATE Productos
                        SET Descripcion = @dscr, Precio = @precio
                        WHERE idProducto = @id";
            conexion.Open();
            var command = new SqliteCommand(query, conexion);
            command.Parameters.Add(new SqliteParameter("@id", id));
            command.Parameters.Add(new SqliteParameter("@dscr", prod.Descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", prod.Precio));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public List<Productos> ListarProductos()
    {
        List<Productos> listado = new List<Productos>();

        using (SqliteConnection conexion = new SqliteConnection(conexionString))
        {
            var query = "SELECT * FROM Productos";
            conexion.Open();
            SqliteCommand command = new SqliteCommand(query, conexion);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Productos prod = new Productos(Convert.ToInt32(reader["idProducto"]), Convert.ToString(reader["Descripcion"]) ?? "No tiene descripcion", Convert.ToInt32(reader["Precio"]));
                    listado.Add(prod);
                }
                conexion.Close();
            }
        }
        return listado;
    }

    public Productos DetallesProducto(int id)
    {
        Productos prod;
        using (SqliteConnection conexion = new SqliteConnection(conexionString))
        {
            var query = "SELECT * FROM Productos WHERE idProducto = @id";
            conexion.Open();
            var command = new SqliteCommand(query, conexion);
            command.Parameters.AddWithValue("@id", id);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                reader.Read();
                prod = new Productos(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]) ?? "No tiene descripcion", Convert.ToInt32(reader[2]));
                conexion.Close();
            }
        }
        return prod;

    }

    public void EliminarProducto(int id)
    {
        using (SqliteConnection conexion = new SqliteConnection(conexionString))
        {
            var query = "DELETE FROM Productos WHERE idProductos = (@id)";
            var command = new SqliteCommand(query, conexion);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }
}