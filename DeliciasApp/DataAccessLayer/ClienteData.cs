using DeliciasApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliciasApp.DataAccessLayer
{
    public class ClienteData
    {

        private readonly string _cadenaSQL = "Integrated Security=SSPI; Persist Security Info=False; Initial Catalog=DeliciasDB; Data Source=(localdb)\\MSSQLLocalDB";

        public List<ClienteModel> Listar()
        {
            var lista = new List<ClienteModel>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_ListarClientes", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ClienteModel
                            {
                                IdCliente = dr["idCliente"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Telefono = dr["telefono"] != DBNull.Value ? dr["telefono"].ToString() : "",
                                Direccion = dr["direccion"] != DBNull.Value ? dr["direccion"].ToString() : "",
                                Activo = Convert.ToBoolean(dr["activo"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public bool Guardar(ClienteModel modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_InsertarCliente", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", modelo.Nombre);
                    cmd.Parameters.AddWithValue("@telefono", (object)modelo.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@direccion", (object)modelo.Direccion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@activo", modelo.Activo);

                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }

        public bool Editar(ClienteModel modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_ActualizarCliente", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCliente", modelo.IdCliente);
                    cmd.Parameters.AddWithValue("@nombre", modelo.Nombre);
                    cmd.Parameters.AddWithValue("@telefono", (object)modelo.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@direccion", (object)modelo.Direccion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@activo", modelo.Activo);

                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }

        public bool Eliminar(string idCliente)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_EliminarCliente", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }

    }
}
