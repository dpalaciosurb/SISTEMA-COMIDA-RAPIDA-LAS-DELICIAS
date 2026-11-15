using DeliciasApp.DataAccessLayer;
using DeliciasApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliciasApp.BussinesLogicLayer
{
    public class ClienteBLL
    {

        private readonly ClienteData _clienteData;

        public ClienteBLL()
        {
            _clienteData = new ClienteData();
        }

        public List<ClienteModel> Listar()
        {
            return _clienteData.Listar();
        }

        public bool Guardar(ClienteModel modelo)
        {
            if (string.IsNullOrEmpty(modelo.IdCliente) || modelo.IdCliente == "Autogenerado")
                return _clienteData.Guardar(modelo);
            else
                return _clienteData.Editar(modelo);
        }

        public bool Eliminar(string idCliente)
        {
            return _clienteData.Eliminar(idCliente);
        }

    }
}
