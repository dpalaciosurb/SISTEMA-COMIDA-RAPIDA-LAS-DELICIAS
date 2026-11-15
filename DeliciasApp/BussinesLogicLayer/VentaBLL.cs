using DeliciasApp.DataAccessLayer;
using DeliciasApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliciasApp.BussinesLogicLayer
{
    public class VentaBLL
    {
        private readonly VentaData _ventaData;

        public VentaBLL()
        {
            _ventaData = new VentaData();
        }

        public bool RegistrarVenta(VentaModel modelo)
        {
            return _ventaData.RegistrarVenta(modelo);
        }
    }
}
