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
    public class ProductoData
    {

        private readonly string _cadenaSQL = "Integrated Security=SSPI; Persist Security Info=False; Initial Catalog=DeliciasDB; Data Source=(localdb)\\MSSQLLocalDB";

        public List<ProductoModel> Listar()
        {
            var lista = new List<ProductoModel>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_ListarProductos", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ProductoModel
                            {
                                IdProducto = dr["idProducto"].ToString(),
                                IdCategoria = dr["idCategoria"].ToString(),
                                NombreCategoria = dr["nombreCategoria"].ToString(),
                                IdProveedor = dr["idProveedor"].ToString(),
                                NombreProveedor = dr["nombreProveedor"].ToString(),
                                NombreProducto = dr["nombreProducto"].ToString(),
                                Precio = Convert.ToDecimal(dr["precio"]),
                                Stock = Convert.ToInt32(dr["stock"]),
                                Activo = Convert.ToBoolean(dr["activo"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public bool Guardar(ProductoModel modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_InsertarProducto", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCategoria", modelo.IdCategoria);
                    cmd.Parameters.AddWithValue("@idProveedor", modelo.IdProveedor);
                    cmd.Parameters.AddWithValue("@nombreProducto", modelo.NombreProducto);
                    cmd.Parameters.AddWithValue("@precio", modelo.Precio);
                    cmd.Parameters.AddWithValue("@stock", modelo.Stock);
                    cmd.Parameters.AddWithValue("@activo", modelo.Activo);

                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }

        public bool Editar(ProductoModel modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_ActualizarProducto", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", modelo.IdProducto);
                    cmd.Parameters.AddWithValue("@idCategoria", modelo.IdCategoria);
                    cmd.Parameters.AddWithValue("@idProveedor", modelo.IdProveedor);
                    cmd.Parameters.AddWithValue("@nombreProducto", modelo.NombreProducto);
                    cmd.Parameters.AddWithValue("@precio", modelo.Precio);
                    cmd.Parameters.AddWithValue("@stock", modelo.Stock);
                    cmd.Parameters.AddWithValue("@activo", modelo.Activo);

                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }

        public bool Eliminar(string idProducto)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                using (var cmd = new SqlCommand("sp_EliminarProducto", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }



    }
}
