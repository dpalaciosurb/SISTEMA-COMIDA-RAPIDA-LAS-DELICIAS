using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeliciasApp.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (usuario == "admin" && password == "admin123")
            {
                SesionActual.NombreUsuario = "Dereck Palacios";
                SesionActual.Rol = "Administrador";

                frmMenu menu = new frmMenu();
                this.Hide();
                menu.FormClosed += (s, args) => this.Close();
                menu.Show();
            }
            else if (usuario == "cajero" && password == "cajero123")
            {
                SesionActual.NombreUsuario = "Cajero de Turno";
                SesionActual.Rol = "Cajero";

                frmMenu menu = new frmMenu();
                this.Hide();
                menu.FormClosed += (s, args) => this.Close();
                menu.Show();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
