using DeliciasApp.DataAccessLayer;
using DeliciasApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliciasApp.BussinesLogicLayer
{
    public class ProductoBLL
    {

        private readonly ProductoData _productoData;

        public ProductoBLL()
        {
            _productoData = new ProductoData();
        }

        public List<ProductoModel> Listar()
        {
            return _productoData.Listar();
        }

        public bool Guardar(ProductoModel modelo)
        {
            if (string.IsNullOrEmpty(modelo.IdProducto) || modelo.IdProducto == "Autogenerado")
                return _productoData.Guardar(modelo);
            else
                return _productoData.Editar(modelo);
        }

        public bool Eliminar(string idProducto)
        {
            return _productoData.Eliminar(idProducto);
        }




    }
}
