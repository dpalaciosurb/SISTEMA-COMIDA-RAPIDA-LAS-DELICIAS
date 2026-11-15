using DeliciasApp.DataAccessLayer;
using DeliciasApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliciasApp.BussinesLogicLayer
{
    public class ProveedorBLL
    {

        private readonly ProveedorData _proveedorData;

        public ProveedorBLL()
        {
            _proveedorData = new ProveedorData();
        }

        public List<ProveedorModel> Listar()
        {
            return _proveedorData.Listar();
        }

        public bool Guardar(ProveedorModel modelo)
        {
            if (string.IsNullOrEmpty(modelo.IdProveedor) || modelo.IdProveedor == "Autogenerado")
                return _proveedorData.Guardar(modelo);
            else
                return _proveedorData.Editar(modelo);
        }

        public bool Eliminar(string idProveedor)
        {
            return _proveedorData.Eliminar(idProveedor);
        }


    }
}
