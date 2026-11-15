using DeliciasApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliciasApp.DataAccessLayer
{
    public class VentaData
    {

        private readonly string _cadenaSQL = "Integrated Security=SSPI; Persist Security Info=False; Initial Catalog=DeliciasDB; Data Source=(localdb)\\MSSQLLocalDB";

        public bool RegistrarVenta(VentaModel modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                using (var transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // Insertar cabecera de la venta y generar código autoincremental VEN-XXXXXX
                        string queryVenta = @"
                            DECLARE @NuevoId VARCHAR(15);
                            DECLARE @Ultimo INT;
                            SELECT @Ultimo = ISNULL(MAX(CAST(SUBSTRING(idVenta, 5, LEN(idVenta)-4) AS INT)), 0) FROM Ventas WHERE idVenta LIKE 'VEN-%';
                            SET @NuevoId = 'VEN-' + RIGHT('00000' + CAST(@Ultimo + 1 AS VARCHAR), 6);

                            INSERT INTO Ventas (idVenta, idUsuario, idCliente, fechaHora, total)
                            VALUES (@NuevoId, @idUsuario, @idCliente, GETDATE(), @total);

                            SELECT @NuevoId;
                        ";

                        string idVentaGenerado = "";
                        using (var cmdVenta = new SqlCommand(queryVenta, conexion, transaccion))
                        {
                            cmdVenta.Parameters.AddWithValue("@idUsuario", modelo.IdUsuario);
                            cmdVenta.Parameters.AddWithValue("@idCliente", modelo.IdCliente);
                            cmdVenta.Parameters.AddWithValue("@total", modelo.Total);
                            idVentaGenerado = cmdVenta.ExecuteScalar().ToString();
                        }

                        // Insertar el detalle de cada producto y actualizar stock
                        foreach (var detalle in modelo.Detalles)
                        {
                            string queryDetalle = @"
                                INSERT INTO DetalleVenta (idVenta, idProducto, cantidad, subtotal)
                                VALUES (@idVenta, @idProducto, @cantidad, @subtotal);

                                UPDATE Productos 
                                SET stock = stock - @cantidad 
                                WHERE idProducto = @idProducto;
                            ";

                            using (var cmdDetalle = new SqlCommand(queryDetalle, conexion, transaccion))
                            {
                                cmdDetalle.Parameters.AddWithValue("@idVenta", idVentaGenerado);
                                cmdDetalle.Parameters.AddWithValue("@idProducto", detalle.IdProducto);
                                cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                                cmdDetalle.Parameters.AddWithValue("@subtotal", detalle.Subtotal);
                                cmdDetalle.ExecuteNonQuery();
                            }
                        }

                        transaccion.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaccion.Rollback();
                        return false;
                    }
                }
            }
        }

    }
}
