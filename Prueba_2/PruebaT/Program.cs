using System;
using System.Data.SqlClient;
using System.IO;

class Program
{
    static void Main()
    {
        string connectionString = "Server=DESKTOP-6CB7TP5\\SQLEXPRESS;Database=nuxiba;Integrated Security=True;";

        ListarTop10Usuarios(connectionString);
        GenerarArchivoCSV(connectionString);
        ActualizarSalario(1, 5000, connectionString); // Actualiza el salario del usuario con ID 1 a $5000
        AgregarUsuario("nuevo_login", "Nuevo", "Usuario", "Apellido", 3000, connectionString); // Agrega un nuevo usuario con un salario predeterminado de $3000
    }

    static void ListarTop10Usuarios(string connectionString)//Listar top 10 usuarios de la base antes creada 
    {
        string query = "SELECT TOP 10 Login, Nombre + ' ' + Paterno + ' ' + Materno AS NombreCompleto, Sueldo, FechaIngreso FROM usuarios INNER JOIN empleados ON usuarios.userId = empleados.userId ORDER BY Sueldo DESC";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Top 10 usuarios:");
            while (reader.Read())
            {
                Console.WriteLine($"Login: {reader["Login"]}, Nombre Completo: {reader["NombreCompleto"]}, Sueldo: {reader["Sueldo"]}, Fecha de Ingreso: {reader["FechaIngreso"]}");
            }
        }
    }

    static void GenerarArchivoCSV(string connectionString)//Generar un archivo csv con las siguienets campos con su información(Login, Nombre completo, sueldo, fecha Ingreso) 
    {
        string query = "SELECT Login, Nombre + ' ' + Paterno + ' ' + Materno AS NombreCompleto, Sueldo, FechaIngreso FROM usuarios INNER JOIN empleados ON usuarios.userId = empleados.userId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            using (StreamWriter writer = new StreamWriter("usuarios.csv"))
            {
                while (reader.Read())
                {
                    string linea = $"{reader["Login"]},{reader["NombreCompleto"]},{reader["Sueldo"]},{reader["FechaIngreso"]}";
                    writer.WriteLine(linea);
                }
            }
        }
    }

    static void ActualizarSalario(int userId, double nuevoSalario, string connectionString)//Poder actualizar el salario del algun usuario especifico
    {
        string query = "UPDATE empleados SET Sueldo = @NuevoSalario WHERE UserId = @UserId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NuevoSalario", nuevoSalario);
            command.Parameters.AddWithValue("@UserId", userId);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    static void AgregarUsuario(string login, string nombre, string paterno, string materno, double salarioPredeterminado, string connectionString)//Pder Tener una opcion para agregar un nuevo usuario y se pueda asiganar el salario y la fecha de ingreso por default el dia de hoy
    {
        string query = "INSERT INTO usuarios (Login, Nombre, Paterno, Materno) VALUES (@Login, @Nombre, @Paterno, @Materno); SELECT SCOPE_IDENTITY()";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Login", login);
            command.Parameters.AddWithValue("@Nombre", nombre);
            command.Parameters.AddWithValue("@Paterno", paterno);
            command.Parameters.AddWithValue("@Materno", materno);

            connection.Open();
            int userId = Convert.ToInt32(command.ExecuteScalar());

            query = "INSERT INTO empleados (UserId, Sueldo, FechaIngreso) VALUES (@UserId, @Sueldo, GETDATE())";
            command.CommandText = query;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Sueldo", salarioPredeterminado);
            command.ExecuteNonQuery();
        }
    }
}
