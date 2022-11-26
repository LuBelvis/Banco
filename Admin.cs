using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Admin : Form
    {
        public string Usuario;
        public Banco MiBanco;
        public string Nombre;
        public double Monto;
        public string Pago;
        public TransfDelegadoAdmin? TransfEvento;

        public Admin(Banco banco)
        {
            InitializeComponent();
            MiBanco = banco;
            CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorro().ToArray();
            PlazosFijos.DataSource = MiBanco.MostrarPlazosFijos().ToArray();
            TarjetasDeCredito.DataSource = MiBanco.MostrarTarjetasDeCredito().ToArray();
            PagosRealizados.DataSource = MiBanco.MostrarPagosRealizado().ToArray();
            PagosPendientes.DataSource = MiBanco.MostrarPagosPendiente().ToArray();
            todosUsuarios.DataSource = MiBanco.MostrarUsuarios().ToArray();
            bloqueados.DataSource = MiBanco.MostrarUsuariosBloqueados().ToArray();
            lblDescrNombre.Text = MiBanco.UsuarioActual.Nombre;
            lblDescrApell.Text = MiBanco.UsuarioActual.Apellido;
            lblDescrMail.Text = MiBanco.UsuarioActual.Mail;
            lblDescrDni.Text = MiBanco.UsuarioActual.Dni.ToString();
            lblDescrPass.Text = MiBanco.UsuarioActual.Clave;

            lblDescrNombre.Visible = true;
            lblDescrApell.Visible = true;
            lblDescrMail.Visible = true;
            lblDescrDni.Visible = true;
            lblDescrPass.Visible = true;
        }

        public delegate void TransfDelegadoAdmin();

        private void todosUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int IdUsuario = int.Parse(todosUsuarios.CurrentRow.Cells[1].Value.ToString());

            CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorroByIdUsuario(IdUsuario).ToArray();
            dataGridView8.DataSource = MovimientosByIdUsuario(IdUsuario).ToArray();
            TarjetasDeCredito.DataSource = MiBanco.MostrarTarjetasDeCreditoByIdUsuario(IdUsuario).ToArray();
            PagosRealizados.DataSource = MiBanco.MostrarPagosRealizadoByIdUsuario(IdUsuario).ToArray();
            PagosPendientes.DataSource = MiBanco.MostrarPagosPendienteByIdUsuario(IdUsuario).ToArray();
            PlazosFijos.DataSource = MiBanco.MostrarPlazosFijosByIdUsuario(IdUsuario).ToArray();
        }

        private void bloqueados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button28.Visible = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            label15.Visible = true;
            label17.Visible = true;
            label16.Visible = true;
            label28.Visible = true;
            label29.Visible = true;
            label30.Text = "Crear nuevo usuario";
            label30.Visible = true;
            textBox1.Visible = true;
            textBox6.Visible = true;
            textBox5.Visible = true;
            textBox4.Visible = true;
            textBox7.Visible = true;
            button17.Visible = true;
            button18.Visible = true;
            check.Visible = true;
        }
        //aceptar
        private void button17_Click(object sender, EventArgs e)
        {
            int Dni = int.Parse(textBox7.Text);
            string Nombre = textBox1.Text;
            string Apellido = textBox6.Text;
            string Mail = textBox5.Text;
            string Clave = textBox4.Text;
            bool IsAdmin = check.Checked;
            bool IsBloqueado = false;

            if (MiBanco.AltaDeUsuario(Dni, Nombre, Apellido, Mail, Clave, IsAdmin, IsBloqueado))
            {
                MessageBox.Show("Usuario: " + Nombre + " creado con exito");

                todosUsuarios.DataSource = MiBanco.MostrarUsuarios().ToArray();
            }
            else
            {
                MessageBox.Show("No se pudo crear el usuario");
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            int IdUsuario = int.Parse(bloqueados.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.DesbloquearUsuario(IdUsuario))
            {
                MessageBox.Show("Usuario desbloqueado");
                bloqueados.DataSource = MiBanco.MostrarUsuariosBloqueados().ToArray();
            }
            else
            {
                MessageBox.Show("No se pudo desbloquear el usuario");
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private List<Movimiento> MovimientosByIdUsuario(int IdUsuario)
        {
            List<Movimiento> resultado = new List<Movimiento>();

            foreach (CajaDeAhorro caja in MiBanco.MostrarCajasDeAhorroByIdUsuario(IdUsuario))
            {
                foreach (Movimiento movimiento in caja.Movimientos)
                {
                    resultado.Add(movimiento);
                }
            }

            return resultado;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            label15.Visible = false;
            label17.Visible = false;
            label16.Visible = false;
            label29.Visible = false;
            label28.Visible = false;
            label30.Visible = false;
            textBox1.Visible = false;
            textBox6.Visible = false;
            textBox5.Visible = false;
            textBox4.Visible = false;
            textBox7.Visible = false;
            button17.Visible = false;
            button18.Visible = false;
            check.Visible = false;

            textBox1.Clear();
            textBox6.Clear();
            textBox5.Clear();
            textBox4.Clear();
            textBox7.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int IdUsuario = int.Parse(todosUsuarios.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.EliminarUsuarioAdmin(IdUsuario))
            {
                MessageBox.Show("Usuario eliminado con exito");
                todosUsuarios.DataSource = MiBanco.MostrarUsuarios().ToArray();
                bloqueados.DataSource = MiBanco.MostrarUsuariosBloqueados().ToArray();
            }
            else
            {
                MessageBox.Show("No se pudo eliminar el usuario");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            label15.Visible = true;
            label17.Visible = true;
            label16.Visible = true;
            label28.Visible = true;
            label29.Visible = true;
            label30.Text = "Modificar usuario";
            label30.Visible = true;
            textBox1.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;
            button18.Visible = true;
            button19.Visible = true;
            check.Visible = true;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            int IdUsuario = int.Parse(todosUsuarios.CurrentRow.Cells[1].Value.ToString());
            string Nombre = textBox1.Text;
            string Apellido = textBox4.Text;
            string Mail = textBox5.Text;
            string Dni = textBox6.Text;
            string Clave = textBox7.Text;
            bool IsAdmin = check.Checked;
            bool IsBloqueado = false;

            int DniInt = 0;

            if (Dni != "")
            {
                DniInt = Convert.ToInt32(Dni);
            }

            if (MiBanco.ModificarUsuarioAdmin(IdUsuario, DniInt, Nombre, Apellido, Mail, Clave, IsAdmin, IsBloqueado))
            {
                txtNombre.Clear();
                txtApellido.Clear();
                txtMail.Clear();
                txtDni.Clear();
                txtPass.Clear();

                MessageBox.Show("Dato cambiado con exito");
            }
            else
            {
                MessageBox.Show("No se pudo cambiar el dato");
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CajarDeAhorro.CurrentRow.Cells[2].Value.ToString() != null)
            {
                label4.Text = CajarDeAhorro.CurrentRow.Cells[2].Value.ToString();
                label5.Text = CajarDeAhorro.CurrentRow.Cells[2].Value.ToString();
                label4.Visible = true;
                label5.Visible = true;

                tabAgregarTitular.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Dni = txtAgregarTitular.Text;

            if (!Dni.Equals(""))
            {
                int DniInt = 0;

                if (Dni != "")
                {
                    DniInt = Convert.ToInt32(Dni);
                }

                int id = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());

                if (MiBanco.ModificarCajaAhorro(DniInt, id, "agregar"))
                {
                    label13.Text = "Titular agregado con exito!";
                    label13.Visible = true;
                }
            }
        }

        private void btnModificarCaja_Click(object sender, EventArgs e)
        {
            tabAgregarTitular.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Dni = txtTitular.Text;

            if (!Dni.Equals(""))
            {
                int DniInt = 0;

                if (Dni != "")
                {
                    DniInt = Convert.ToInt32(Dni);
                }

                int IdCajaAhorro = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());

                if (MiBanco.ModificarCajaAhorro(DniInt, IdCajaAhorro, "eliminar"))
                {
                    label12.Text = "Titular eliminado con exito!";
                    label12.Visible = true;
                }
                else
                {
                    label12.Text = "No se puede dejar una caja sin titular!";
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tabAgregarTitular.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int id = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.BajaCajaAhorro(id))
            {

                button7.Visible = false;

                MessageBox.Show("Caja eliminada con exito");

                CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorro().ToArray();
                PlazosFijos.DataSource = MiBanco.MostrarPlazosFijos().ToArray();
                TarjetasDeCredito.DataSource = MiBanco.MostrarTarjetasDeCredito().ToArray();
            }
            else
            {
                MessageBox.Show("No se puede eliminar la caja");

            }
        }

        private void CajarDeAhorro_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int IdUsuario = int.Parse(todosUsuarios.CurrentRow.Cells[1].Value.ToString());
            if (MiBanco.AltaCajaAhorroAdmin(IdUsuario))
            {
                MessageBox.Show("Nueva caja creada con exito");
                CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorro().ToArray();
            }
            else
            {
                MessageBox.Show("No se pudo crear la caja");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int id = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());

            monthCalendar1.Visible = true;
            button21.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            label24.Visible = true;
            label25.Visible = true;

            dataGridView8.Visible = true;
            dataGridView8.DataSource = MiBanco.MostrarMovimientos(id);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int id = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());

            DateTime? fecha = monthCalendar1.SelectionStart.Date;
            string? detalle = textBox2.Text;
            string? monto = textBox3.Text;

            double montoDouble = 0;

            if (!monto.Equals(""))
            {
                montoDouble = Convert.ToDouble(monto);
            }


            dataGridView8.DataSource = MiBanco.BuscarMovimiento(id, detalle, fecha, montoDouble);
        }

        private void PlazosFijos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnBajaPlazo.Visible = true;
        }

        private void btnBajaPlazo_Click(object sender, EventArgs e)
        {
            int id = int.Parse(PlazosFijos.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.BajaPlazoFijo(id))
            {
                PlazosFijos.DataSource = MiBanco.MostrarPlazosFijos();

                MessageBox.Show("Plazo dado de baja con exito");
            }
            else
            {
                PlazosFijos.DataSource = MiBanco.MostrarPlazosFijos();

                MessageBox.Show("No se pudo dar de baja");
            }
        }

        private void btnPlazoFijo_Click(object sender, EventArgs e)
        {
            int IdUsuario = int.Parse(todosUsuarios.CurrentRow.Cells[1].Value.ToString());

            dgvCajasPlazo.DataSource = MiBanco.MostrarCajasDeAhorroByIdUsuario(IdUsuario);
            dgvCajasPlazo.Visible = true;
        }

        private void dgvCajasPlazo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = fechaInicio.AddYears(1);
            //pongo tasa 10%
            int tasa = 10;

            lblfechaInicio.Text = fechaInicio.ToString();
            lblFechaFin.Text = fechaFin.ToString();
            lblTasa.Text = tasa.ToString() + "%";

            label21.Visible = true;
            lblfechaInicio.Visible = true;
            label22.Visible = true;
            lblFechaFin.Visible = true;
            label23.Visible = true;
            lblTasa.Visible = true;
            lblMontoPlazo.Visible = true;
            txtMontoPlazo.Visible = true;
            btnMontoPlazo.Visible = true;
        }

        private void btnMontoPlazo_Click(object sender, EventArgs e)
        {
            int IdUsuario = int.Parse(todosUsuarios.CurrentRow.Cells[1].Value.ToString());
            int Id = int.Parse(dgvCajasPlazo.CurrentRow.Cells[1].Value.ToString());
            string monto = txtMontoPlazo.Text;
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = fechaInicio.AddYears(1);
            //pongo tasa 10%
            int tasa = 10;
            bool isPagado = false;
            bool montoOk = true;
            bool montoCorrecto = true;

            double montoDouble = 0;

            if (!monto.Equals(""))
            {
                montoDouble = Convert.ToDouble(monto);
            }

            if (monto.Equals(""))
            {
                MessageBox.Show("Debe ingresar un monto");
                montoOk = false;
            }
            else
            {
                if (montoDouble <= 999)
                {
                    MessageBox.Show("El monto minimo es $1000");
                    montoCorrecto = false;
                }
            }

            if (montoOk && montoCorrecto)
            {
                if (MiBanco.AltaPlazoFijo(Id, montoDouble, fechaInicio, fechaFin, tasa, isPagado, IdUsuario))
                {
                    MessageBox.Show("Plazo fijo creado con exito");

                    dgvCajasPlazo.Visible = false;
                    lblMontoPlazo.Visible = false;
                    txtMontoPlazo.Visible = false;
                    btnBajaPlazo.Visible = false;
                    lblfechaInicio.Visible = false;
                    lblFechaFin.Visible = false;
                    lblTasa.Visible = false;
                    label21.Visible = false;
                    label22.Visible = false;
                    label23.Visible = false;
                    btnMontoPlazo.Visible = false;
                    txtMontoPlazo.Clear();

                    PlazosFijos.DataSource = MiBanco.MostrarPlazosFijos();
                }
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            int id = int.Parse(TarjetasDeCredito.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.BajaTarjetaCredito(id))
            {
                TarjetasDeCredito.DataSource = MiBanco.MostrarTarjetasDeCredito();

                MessageBox.Show("Tarjeta dada de baja con exito");
            }
            else
            {
                MessageBox.Show("No se pudo eliminar la tarjeta");
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            int IdUsuario = int.Parse(todosUsuarios.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.AltaTarjetaCredito(IdUsuario))
            {
                TarjetasDeCredito.DataSource = MiBanco.MostrarTarjetasDeCredito();

                MessageBox.Show("Tarjeta creada con exito");
            }
            else
            {
                MessageBox.Show("No se pudo crear la tarjeta");
            }
        }

        private void TarjetasDeCredito_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button22.Visible = true;
            button24.Visible = true;
            txtLimite.Visible = true;
            lblLimite.Visible = true;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            int IdTarjeta = int.Parse(TarjetasDeCredito.CurrentRow.Cells[1].Value.ToString());
            string NuevoLimite = txtLimite.Text;
            double NuevoLimiteDouble = 0;
            bool LimiteOk = true;

            if (NuevoLimite != "")
            {
                NuevoLimiteDouble = Convert.ToInt32(NuevoLimite);
            }

            if (NuevoLimite == "")
            {
                MessageBox.Show("Ingrese el nuevo limite");
                LimiteOk = false;
            }

            if (LimiteOk && MiBanco.ModificarTarjetaCredito(IdTarjeta, NuevoLimiteDouble))
            {
                MessageBox.Show("Se cambio el limite con exito");
            }
            else
            {
                MessageBox.Show("No se pudo cambiar el limite");
            }
        }

        private void txtLimite_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void PagosRealizados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button12.Visible = true;
        }

        private void PagosPendientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button12.Visible = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int IdUsuario = int.Parse(todosUsuarios.CurrentRow.Cells[1].Value.ToString());

            if (PagosRealizados.CurrentRow.Cells[1].Value != null)
            {
                int IdPago = int.Parse(PagosRealizados.CurrentRow.Cells[1].Value.ToString());

                if (MiBanco.BajaPago(IdPago))
                {
                    MessageBox.Show("Pago eliminado correctamente");
                    PagosPendientes.DataSource = MiBanco.MostrarPagosPendienteByIdUsuario(IdUsuario).ToArray();
                    PagosRealizados.DataSource = MiBanco.MostrarPagosRealizadoByIdUsuario(IdUsuario).ToArray();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el pago");
                }
            }
            else
            {
                MessageBox.Show("No se selecciono ningun pago");
            }
        }

        private void montoPago_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string detalle = nombrePago.Text;
            string monto = montoPago.Text;
            double montoDouble = 0;
            bool isPagado = false;
            string metodo = "Pendiente";
            bool montoOk = true;
            bool detalleOk = true;

            if (!monto.Equals(""))
            {
                montoDouble = Convert.ToDouble(monto);
            }

            if (monto.Equals(""))
            {
                MessageBox.Show("Ingrese un monto");
                montoOk = false;
            }

            if (detalle.Equals(""))
            {
                MessageBox.Show("Ingrese el detalle");
                detalleOk = false;
            }

            if (montoOk && detalleOk)
            {
                int IdUsuario = int.Parse(todosUsuarios.CurrentRow.Cells[1].Value.ToString());

                if (MiBanco.AltaPago(IdUsuario, detalle, montoDouble, isPagado, metodo))
                {
                    nombrePago.Clear();
                    montoPago.Clear();
                    PagosPendientes.DataSource = MiBanco.MostrarPagosPendienteByIdUsuario(IdUsuario).ToArray();
                    PagosRealizados.DataSource = MiBanco.MostrarPagosRealizadoByIdUsuario(IdUsuario).ToArray();
                    MessageBox.Show("Pago creado");
                }
                else
                {
                    MessageBox.Show("No se pudo crear el pago");
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            lblNombre.Visible = true;
            lblApellido.Visible = true;
            lblMail.Visible = true;
            lblDni.Visible = true;
            lblPass.Visible = true;
            txtNombre.Visible = true;
            txtApellido.Visible = true;
            txtMail.Visible = true;
            txtDni.Visible = true;
            txtPass.Visible = true;
            btnMail.Visible = true;

            btnEliminar.Visible = false;
        }

        private void txtDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnMail_Click(object sender, EventArgs e)
        {
            string Nombre = txtNombre.Text;
            string Apellido = txtApellido.Text;
            string Mail = txtMail.Text;
            string Dni = txtDni.Text;
            string Clave = txtPass.Text;

            int DniInt = 0;

            if (Dni != "")
            {
                DniInt = Convert.ToInt32(Dni);
            }

            if (MiBanco.ModificarUsuarioUsuarioActual(DniInt, Nombre, Apellido, Mail, Clave))
            {
                txtNombre.Clear();
                txtApellido.Clear();
                txtMail.Clear();
                txtDni.Clear();
                txtPass.Clear();

                lblDescrNombre.Text = MiBanco.UsuarioActual.Nombre.ToString();
                lblDescrApell.Text = MiBanco.UsuarioActual.Apellido.ToString();
                lblDescrMail.Text = MiBanco.UsuarioActual.Mail.ToString();
                lblDescrDni.Text = MiBanco.UsuarioActual.Dni.ToString();
                lblDescrPass.Text = MiBanco.UsuarioActual.Clave.ToString();

                MessageBox.Show("Dato cambiado con exito");
            }
            else
            {
                MessageBox.Show("No se pudo cambiar el dato");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            lblNombre.Visible = false;
            lblApellido.Visible = false;
            lblMail.Visible = false;
            lblDni.Visible = false;
            lblPass.Visible = false;
            txtNombre.Visible = false;
            txtApellido.Visible = false;
            txtMail.Visible = false;
            txtDni.Visible = false;
            txtPass.Visible = false;
            btnMail.Visible = false;

            btnEliminar.Visible = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MiBanco.EliminarUsuarioUsuarioActual())
            {
                TransfEvento();
            }
            else
            {
                MessageBox.Show("No se pudo eliminar el usuario");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TransfEvento();
        }
    }
}
