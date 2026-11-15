using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliciasApp.Models
{
    public class VentaModel
    {
        public string IdVenta { get; set; } = string.Empty;
        public string IdUsuario { get; set; } = string.Empty;
        public string IdCliente { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public decimal Total { get; set; }
        public List<DetalleVentaModel> Detalles { get; set; } = new List<DetalleVentaModel>();
    }

    public class DetalleVentaModel
    {
        public int IdDetalle { get; set; }
        public string IdVenta { get; set; } = string.Empty;
        public string IdProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
