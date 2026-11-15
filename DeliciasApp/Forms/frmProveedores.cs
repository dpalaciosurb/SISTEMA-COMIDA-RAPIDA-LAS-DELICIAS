using DeliciasApp.BussinesLogicLayer;
using DeliciasApp.Models;
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
    public partial class frmProveedores : Form
    {
        private readonly ProveedorBLL _proveedorBLL;
        private bool _esEdicion = false;
        public frmProveedores()
        {
            InitializeComponent();
            _proveedorBLL = new ProveedorBLL();
        }

        private void frmProveedores_Load(object sender, EventArgs e)
        {
            CargarDatosGrid();
            LimpiarFormulario();
        }

        private void CargarDatosGrid()
        {
            try
            {
                dgvProveedores.DataSource = null;
                dgvProveedores.DataSource = _proveedorBLL.Listar();
                dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            txtIdProveedor.Text = "Autogenerado";
            txtNombreProveedor.Clear();
            txtContacto.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            chkActivo.Checked = true;

            _esEdicion = false;
            btnGuardar.Text = "Guardar / Actualizar";
            txtNombreProveedor.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreProveedor.Text))
            {
                MessageBox.Show("Ingrese el nombre de la empresa o proveedor.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreProveedor.Focus();
                return;
            }

            var modelo = new ProveedorModel
            {
                IdProveedor = _esEdicion ? txtIdProveedor.Text : string.Empty,
                NombreProveedor = txtNombreProveedor.Text.Trim(),
                Contacto = txtContacto.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Direccion = txtDireccion.Text.Trim(),
                Activo = chkActivo.Checked
            };

            if (_proveedorBLL.Guardar(modelo))
            {
                MessageBox.Show(_esEdicion ? "Proveedor actualizado correctamente." : "Proveedor guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatosGrid();
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show("No se pudo procesar el registro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!_esEdicion || string.IsNullOrWhiteSpace(txtIdProveedor.Text) || txtIdProveedor.Text == "Autogenerado")
            {
                MessageBox.Show("Seleccione un proveedor de la tabla para eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show("¿Está seguro de eliminar este proveedor?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion == DialogResult.Yes)
            {
                if (_proveedorBLL.Eliminar(txtIdProveedor.Text))
                {
                    MessageBox.Show("Proveedor eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatosGrid();
                    LimpiarFormulario();
                }
                else
                {
                    MessageBox.Show("No se puede eliminar el proveedor porque tiene productos asignados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var confirmacion = MessageBox.Show("¿Está seguro de cancelar la operación y volver al menú?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProveedores.Rows[e.RowIndex];
                txtIdProveedor.Text = fila.Cells["IdProveedor"].Value?.ToString();
                txtNombreProveedor.Text = fila.Cells["NombreProveedor"].Value?.ToString();
                txtContacto.Text = fila.Cells["Contacto"].Value?.ToString();
                txtTelefono.Text = fila.Cells["Telefono"].Value?.ToString();
                txtDireccion.Text = fila.Cells["Direccion"].Value?.ToString();
                chkActivo.Checked = Convert.ToBoolean(fila.Cells["Activo"].Value);

                _esEdicion = true;
                btnGuardar.Text = "Actualizar";
            }
        }
    }
}
