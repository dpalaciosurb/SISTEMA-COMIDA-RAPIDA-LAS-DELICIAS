using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliciasApp.Models
{
    public class ProductoModel
    {
        public string IdProducto { get; set; } = string.Empty;
        public string IdCategoria { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public string IdProveedor { get; set; } = string.Empty;
        public string NombreProveedor { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; } = true;
    }
}
