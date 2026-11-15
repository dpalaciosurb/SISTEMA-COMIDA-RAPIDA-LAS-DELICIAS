using DeliciasApp.BussinesLogicLayer;
using DeliciasApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeliciasApp.Forms
{
    public partial class frmProductos : Form
    {
        private readonly ProductoBLL _productoBLL;
        private readonly ProveedorBLL _proveedorBLL;
        private bool _esEdicion = false;
        private readonly string _cadenaSQL = "Integrated Security=SSPI; Persist Security Info=False; Initial Catalog=DeliciasDB; Data Source=(localdb)\\MSSQLLocalDB";
        public frmProductos()
        {
            InitializeComponent();
            _productoBLL = new ProductoBLL();
            _proveedorBLL = new ProveedorBLL();
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            CargarCombos();
            CargarDatosGrid();
            LimpiarFormulario();
        }

        private void CargarCombos()
        {
            try
            {
                // Cargar Proveedores desde BLL
                cboProveedor.DataSource = _proveedorBLL.Listar();
                cboProveedor.DisplayMember = "NombreProveedor";
                cboProveedor.ValueMember = "IdProveedor";

                // Cargar Categorias directo de SQL
                using (var con = new SqlConnection(_cadenaSQL))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT idCategoria, nombreCategoria FROM Categorias WHERE activo = 1", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cboCategoria.DataSource = dt;
                    cboCategoria.DisplayMember = "nombreCategoria";
                    cboCategoria.ValueMember = "idCategoria";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar listas desplegables: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosGrid()
        {
            try
            {
                dgvProductos.DataSource = null;
                dgvProductos.DataSource = _productoBLL.Listar();
                dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                if (dgvProductos.Columns["IdCategoria"] != null) dgvProductos.Columns["IdCategoria"].Visible = false;
                if (dgvProductos.Columns["IdProveedor"] != null) dgvProductos.Columns["IdProveedor"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            txtIdProducto.Text = "Autogenerado";
            txtNombreProducto.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            if (cboCategoria.Items.Count > 0) cboCategoria.SelectedIndex = 0;
            if (cboProveedor.Items.Count > 0) cboProveedor.SelectedIndex = 0;
            chkActivo.Checked = true;

            _esEdicion = false;
            btnGuardar.Text = "Guardar / Actualizar";
            txtNombreProducto.Focus();
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];
                txtIdProducto.Text = fila.Cells["IdProducto"].Value?.ToString();
                txtNombreProducto.Text = fila.Cells["NombreProducto"].Value?.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value?.ToString();
                txtStock.Text = fila.Cells["Stock"].Value?.ToString();
                cboCategoria.SelectedValue = fila.Cells["IdCategoria"].Value;
                cboProveedor.SelectedValue = fila.Cells["IdProveedor"].Value;
                chkActivo.Checked = Convert.ToBoolean(fila.Cells["Activo"].Value);

                _esEdicion = true;
                btnGuardar.Text = "Actualizar";
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreProducto.Text))
            {
                MessageBox.Show("Ingrese el nombre del producto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreProducto.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("Ingrese un precio válido mayor a 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Ingrese una cantidad de stock válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return;
            }

            var modelo = new ProductoModel
            {
                IdProducto = _esEdicion ? txtIdProducto.Text : string.Empty,
                IdCategoria = cboCategoria.SelectedValue.ToString(),
                IdProveedor = cboProveedor.SelectedValue.ToString(),
                NombreProducto = txtNombreProducto.Text.Trim(),
                Precio = precio,
                Stock = stock,
                Activo = chkActivo.Checked
            };

            if (_productoBLL.Guardar(modelo))
            {
                MessageBox.Show(_esEdicion ? "Producto actualizado correctamente." : "Producto guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!_esEdicion || string.IsNullOrWhiteSpace(txtIdProducto.Text) || txtIdProducto.Text == "Autogenerado")
            {
                MessageBox.Show("Seleccione un producto de la tabla para eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show("¿Está seguro de eliminar este producto?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion == DialogResult.Yes)
            {
                if (_productoBLL.Eliminar(txtIdProducto.Text))
                {
                    MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatosGrid();
                    LimpiarFormulario();
                }
                else
                {
                    MessageBox.Show("No se puede eliminar el producto porque tiene ventas asociadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
