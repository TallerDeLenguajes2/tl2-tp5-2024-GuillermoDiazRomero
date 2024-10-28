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

    public List<Presupuestos> ListarPresupuestos()
    {
        List<Presupuestos> Lista = new List<Presupuestos>();
        using (SqliteConnection connection = new SqliteConnection(conexionString))
        {
            var query = "SELECT * FROM Presupuestos";
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Presupuestos presupuestos = new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]), Convert.ToString(reader["NombreDestinatario"]) ?? "NULL", Convert.ToString(reader["FechaCreacion"]) ?? "NULL");
                    Lista.Add(presupuestos);
                }
            }
            connection.Close();
        }
        return Lista;
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


    public void AgregarPresupuesto(int idPresupuesto, int idProducto, int cant){
        using (SqliteConnection connection = new SqliteConnection(conexionString)){
            var query = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@pres,@prod,@cant)";
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@pres",idPresupuesto);
            command.Parameters.AddWithValue("@prod",idProducto);
            command.Parameters.AddWithValue("@cant",cant);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }


    public void EliminarProducto(int idPresupuesto){
        using (SqliteConnection connection = new SqliteConnection(conexionString)){
            var query = "DELETE FROM Presupuesto, PresupuestosDetalle WHERE idPresupuesto = @id";
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", idPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

}