using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliciasApp.Models
{
    public class UsuarioModel
    {
        public string IdUsuario { get; set; } = string.Empty;
        public string IdRol { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }
}
