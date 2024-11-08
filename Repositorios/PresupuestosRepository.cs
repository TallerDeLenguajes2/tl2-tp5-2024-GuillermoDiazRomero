using Microsoft.Data.Sqlite;
using models;

namespace persistence;

public class PresupuestoRepository
{
    private string conexionString = "Data Source=db/Tienda.db;Cache=Shared";
    public void CrearPresupuesto(Presupuestos nuevoPresupuesto)
    {
        using (SqliteConnection connection = new SqliteConnection(conexionString))
        {
            var query = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@nomDes,@feCre)";
            connection.Open();
            var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@nomDes", nuevoPresupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@feCre", nuevoPresupuesto.FechaCreacion);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Presupuestos> Listar()
    {
        var presupuestos = new Dictionary<int, Presupuestos>();
        using (var connection = new SqliteConnection(conexionString))
        {
            connection.Open();

            var sqlQuery = @"SELECT p.idPresupuesto, p.NombreDestinatario, p.FechaCreacion, pr.idProducto, pr.Descripcion, pr.Precio, pd.Cantidad 
                             FROM Presupuestos p
                             LEFT JOIN PresupuestosDetalle pd USING (idPresupuesto)
                             LEFT JOIN Productos pr USING (idProducto)";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            using (var sqlReader = sqlCmd.ExecuteReader())
            {
                while(sqlReader.Read())
                {
                    int idPresupuesto = sqlReader.GetInt32(0);

                    if (!presupuestos.TryGetValue(idPresupuesto, out var presupuesto))
                        presupuesto = generarPresupuesto(sqlReader);

                    if (!sqlReader.IsDBNull(3))
                    {
                        var producto = new Productos(sqlReader.GetInt32(3), sqlReader.GetString(4), sqlReader.GetInt32(5));
                        var detalle = new PresupuestosDetalle(producto,sqlReader.GetInt32(6));

                        presupuesto.Detalle.Add(detalle);
                    }
                    presupuestos.TryAdd(idPresupuesto, presupuesto);
                }
            }
            connection.Close();
        }
        return new List<Presupuestos>(presupuestos.Values);
    }


    public List<PresupuestosDetalle> ObtenerDetalles(int idPresupuesto)
    {
        List<PresupuestosDetalle> lista = new List<PresupuestosDetalle>();
        using (SqliteConnection connection = new SqliteConnection(conexionString))
        {
            var query = "SELECT * FROM PresupuestosDetalles A INNER JOIN Productos B ON A.idProducto = @id = B.idProducto WHERE A.idProducto = @id";
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", idPresupuesto);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Productos prod = new Productos(Convert.ToInt32(reader["idProducto"]), Convert.ToString(reader["Descripcion"]) ?? "No tiene descripcion", Convert.ToInt32(reader["Precio"]));
                    PresupuestosDetalle presProd = new PresupuestosDetalle(prod, Convert.ToInt32(reader["Cantidad"]));
                    lista.Add(presProd);
                }
                connection.Close();
            }
        }
        return lista;
    }


    public void AgregarPresupuesto(int idPresupuesto, int idProducto, int cant)
    {
        using (SqliteConnection connection = new SqliteConnection(conexionString))
        {
            var query = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@pres,@prod,@cant)";
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@pres", idPresupuesto);
            command.Parameters.AddWithValue("@prod", idProducto);
            command.Parameters.AddWithValue("@cant", cant);
            command.ExecuteNonQuery();
            connection.Close();
        }

    }


    public void EliminarProducto(int idPresupuesto)
    {
        using (SqliteConnection connection = new SqliteConnection(conexionString))
        {
            var query = "DELETE FROM Presupuesto, PresupuestosDetalle WHERE idPresupuesto = @id";
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", idPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }


    private Presupuestos generarPresupuesto(SqliteDataReader reader)
    {
        try {
            return new Presupuestos(reader.GetInt32(0),
                                   reader.GetString(1),
                                   reader.GetString(2));
        } catch (Exception) {
            return new Presupuestos();
        }
    }

}