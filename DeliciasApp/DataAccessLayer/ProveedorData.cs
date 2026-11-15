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
    public class ProveedorData
    {

        private readonly string _cadenaSQL = "Integrated Security=SSPI; Persist Security Info=False; Initial Catalog=DeliciasDB; Data Source=(localdb)\\MSSQLLocalDB";

        public List<ProveedorModel> Listar()
        {
            var lista = new List<ProveedorModel>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_ListarProveedores", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ProveedorModel
                            {
                                IdProveedor = dr["idProveedor"].ToString(),
                                NombreProveedor = dr["nombreProveedor"].ToString(),
                                Contacto = dr["contacto"] != DBNull.Value ? dr["contacto"].ToString() : "",
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

        public bool Guardar(ProveedorModel modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_InsertarProveedor", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombreProveedor", modelo.NombreProveedor);
                    cmd.Parameters.AddWithValue("@contacto", (object)modelo.Contacto ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@telefono", (object)modelo.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@direccion", (object)modelo.Direccion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@activo", modelo.Activo);

                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }

        public bool Editar(ProveedorModel modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_ActualizarProveedor", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProveedor", modelo.IdProveedor);
                    cmd.Parameters.AddWithValue("@nombreProveedor", modelo.NombreProveedor);
                    cmd.Parameters.AddWithValue("@contacto", (object)modelo.Contacto ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@telefono", (object)modelo.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@direccion", (object)modelo.Direccion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@activo", modelo.Activo);

                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }

        public bool Eliminar(string idProveedor)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_EliminarProveedor", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0; // Borrará siempre al proveedor y sus dependencias
                }
            }
        }

    }
}
