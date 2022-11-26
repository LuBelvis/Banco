using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1;
using static Pago;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static WinFormsApp1.Log;
using static WinFormsApp1.Menu;

namespace WinFormsApp1
{
    public partial class Menu : Form
    {


        public object[] Argumentos;

        public string Usuario;
        public Banco MiBanco;
        public string Nombre;
        public double Monto;
        public string Pago;
        public TransfDelegadoMenu? TransfEvento;


        public Menu(Banco banco)
        {

            InitializeComponent();

            MiBanco = banco;
            CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual().ToArray();
            PlazosFijos.DataSource = MiBanco.MostrarPlazosFijosUsuarioActual().ToArray();
            TarjetasDeCredito.DataSource = MiBanco.MostrarTarjetasDeCreditoUsuarioActual().ToArray();
            PagosRealizados.DataSource = MiBanco.MostrarPagosRealizadoUsuarioActual().ToArray();
            PagosPendientes.DataSource = MiBanco.MostrarPagosPendienteUsuarioActual().ToArray();
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


        private void CrearNuevoPago(object sender, EventArgs e)
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
                if (MiBanco.AltaPagoUsuarioActual(detalle, montoDouble, isPagado, metodo))
                {
                    nombrePago.Clear();
                    montoPago.Clear();
                    PagosRealizados.DataSource = MiBanco.MostrarPagosRealizadoUsuarioActual().ToArray();
                    PagosPendientes.DataSource = MiBanco.MostrarPagosPendienteUsuarioActual().ToArray();
                    MessageBox.Show("Pago creado");
                }
            }
        }

        private void CrearCajaAhorro(object sender, EventArgs e)
        {
            if (MiBanco.AltaCajaAhorroUsuarioActual())
            {
                MessageBox.Show("Nueva caja creada con exito");
                CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual().ToArray();
            }
            else
            {
                MessageBox.Show("No se pudo crear la caja");
            }
        }

        private void ButtonDepositar(object sender, EventArgs e)
        {
            label16.Visible = false;
            label17.Visible = false;
            textBoxtransf.Visible = false;
            textBoxCBU.Visible = false;
            button20.Visible = false;
            label15.Visible = false;
            textBox4.Visible = false;
            button19.Visible = false;

            dataGridView8.Visible = false;
            monthCalendar1.Visible = false;
            button21.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            label24.Visible = false;
            label25.Visible = false;

            label14.Visible = true;
            montodep.Visible = true;
            button18.Visible = true;

        }

        private void ButtonRetirar(object sender, EventArgs e)
        {
            label16.Visible = false;
            label17.Visible = false;
            textBoxtransf.Visible = false;
            textBoxCBU.Visible = false;
            button20.Visible = false;
            label14.Visible = false;
            montodep.Visible = false;
            button18.Visible = false;

            dataGridView8.Visible = false;
            monthCalendar1.Visible = false;
            button21.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            label24.Visible = false;
            label25.Visible = false;

            label15.Visible = true;
            textBox4.Visible = true;
            button19.Visible = true;
        }

        private void ButtonTransferir(object sender, EventArgs e)
        {
            label15.Visible = false;
            textBox4.Visible = false;
            button19.Visible = false;
            label14.Visible = false;
            montodep.Visible = false;
            button18.Visible = false;

            dataGridView8.Visible = false;
            monthCalendar1.Visible = false;
            button21.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            label24.Visible = false;
            label25.Visible = false;

            label16.Visible = true;
            label17.Visible = true;
            textBoxtransf.Visible = true;
            textBoxCBU.Visible = true;
            button20.Visible = true;

        }

        private void VerDetalle(object sender, EventArgs e)
        {
            int id = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());

            monthCalendar1.Visible = true;
            button21.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            label24.Visible = true;
            label25.Visible = true;

            montodep.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
            textBoxCBU.Visible = false;
            textBoxtransf.Visible = false;
            button20.Visible = false;
            button19.Visible = false;
            button18.Visible = false;

            dataGridView8.Visible = true;
            dataGridView8.DataSource = MiBanco.MostrarMovimientosUsuarioActual(id);
        }

        private void DarDeBaja(object sender, EventArgs e)
        {
            int id = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.BajaCajaAhorroUsuarioActual(id))
            {

                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                button9.Visible = false;
                button10.Visible = false;

                MessageBox.Show("Caja eliminada con exito");

                CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual().ToArray();
                PlazosFijos.DataSource = MiBanco.MostrarPlazosFijosUsuarioActual().ToArray();
                TarjetasDeCredito.DataSource = MiBanco.MostrarTarjetasDeCreditoUsuarioActual().ToArray();
            }
            else
            {
                MessageBox.Show("No se puede eliminar la caja");

            }
        }

        private void Modificar(object sender, EventArgs e)
        {
            if (CajarDeAhorro.CurrentRow.Cells[2].Value.ToString() != null)
            {
                label4.Visible = true;
                label5.Visible = true;

                label4.Text = CajarDeAhorro.CurrentRow.Cells[2].Value.ToString();
                label5.Text = CajarDeAhorro.CurrentRow.Cells[2].Value.ToString();
                tabAgregarTitular.Visible = true;
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button5.Visible = true;
            button6.Visible = true;
            button10.Visible = true;
            button9.Visible = true;
            button8.Visible = true;
            button7.Visible = true;
        }

        private void GrillaPagosRealizados(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView6.Visible = false;
            dataGridView7.Visible = false;
            label6.Visible = false;
            label7.Visible = false;

            dataGridView6.ClearSelection();
            dataGridView7.ClearSelection();
        }


        private void BtnModificarCaja_Click(object sender, EventArgs e)
        {
            tabAgregarTitular.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
        }
        //agregar titular Cancelar
        private void ButtonAgregarUsuarioCaja(object sender, EventArgs e)
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
        //eliminar titular
        private void ButtonEliminarUsuarioCaja(object sender, EventArgs e)
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
        //Cancelar 
        private void BtnCancelar_Click_1(object sender, EventArgs e)
        {
            tabAgregarTitular.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
        }

        public delegate void TransfDelegadoMenu();

        private void Button3_Click(object sender, EventArgs e)
        {
            TransfEvento();
        }

        private void ButtonConfirmarDepositar(object sender, EventArgs e)
        {
            double monto = double.Parse(montodep.Text);
            int id = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.Depositar(id, monto))
            {
                montodep.Clear();
                montodep.Visible = false;
                label14.Visible = false;
                button18.Visible = false;

                CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual().ToArray();
                MessageBox.Show("El saldo de la caja es: " + MiBanco.BuscarCajaPorId(id).Saldo);
            }

        }

        private void ButtonConfirmarRetirar(object sender, EventArgs e)
        {
            double monto = double.Parse(textBox4.Text);
            int id = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.Retirar(id, monto))
            {
                textBox4.Clear();
                textBox4.Visible = false;
                label15.Visible = false;
                button19.Visible = false;

                CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual().ToArray();
                MessageBox.Show("El saldo de la caja es: " + MiBanco.BuscarCajaPorId(id).Saldo);
            }
            else
            {
                MessageBox.Show("El saldo de la caja no es suficiente");
            }
        }

        private void Button20_Click(object sender, EventArgs e)
        {
            double monto = double.Parse(textBoxtransf.Text);
            int idOrigen = int.Parse(CajarDeAhorro.CurrentRow.Cells[1].Value.ToString());
            int cbuDestino = int.Parse(textBoxCBU.Text);

            textBoxCBU.Clear();
            textBoxtransf.Clear();
            textBoxCBU.Visible = false;
            textBoxtransf.Visible = false;
            label16.Visible = false;
            label17.Visible = false;


            if (MiBanco.Transferir(idOrigen, cbuDestino, monto))
            {
                textBoxCBU.Clear();
                textBoxtransf.Clear();

                MessageBox.Show("Transferencia realizada con exito");
                CajarDeAhorro.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual().ToArray();
            }
            else
            {
                MessageBox.Show("El CBU destino no existe, o no tienes el saldo suficiente");
            }

        }

        private void Button14_Click(object sender, EventArgs e)
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

        private void Button16_Click(object sender, EventArgs e)
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

        private void BtnModificarUsuario(object sender, EventArgs e)
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

        private void button17_Click(object sender, EventArgs e)
        {
            dgvCajasPlazo.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual().ToArray();
            dgvCajasPlazo.Visible = true;
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            int IdCajaAhorro = int.Parse(dgvCajasPlazo.CurrentRow.Cells[1].Value.ToString());
            string monto = txtMontoPlazo.Text;
            DateTime fechaInicio = DateTime.Now.Date;
            DateTime fechaFin = fechaInicio.AddYears(1).Date;
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

                if (MiBanco.AltaPlazoFijoUsuarioActual(IdCajaAhorro, montoDouble, fechaInicio, fechaFin, tasa, isPagado))
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

                    PlazosFijos.DataSource = MiBanco.MostrarPlazosFijosUsuarioActual();
                }
                else
                {
                    MessageBox.Show("No se pudo crear el plazo fijo");
                }
            }
        }

        private void btnBajaPlazo_Click(object sender, EventArgs e)
        {
            int IdPlazoFijo = int.Parse(PlazosFijos.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.BajaPlazoFijoUsuarioActual(IdPlazoFijo))
            {
                PlazosFijos.DataSource = MiBanco.MostrarPlazosFijosUsuarioActual();

                MessageBox.Show("Plazo dado de baja con exito");
            }
            else
            {
                PlazosFijos.DataSource = MiBanco.MostrarPlazosFijosUsuarioActual();

                MessageBox.Show("No se pudo dar de baja");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnBajaPlazo.Visible = true;
        }

        private void ButtonCrearTarjetaCredito(object sender, EventArgs e)
        {
            if (MiBanco.AltaTarjetaCreditoUsuarioActual())
            {
                TarjetasDeCredito.DataSource = MiBanco.MostrarTarjetasDeCreditoUsuarioActual();

                MessageBox.Show("Tarjeta creada con exito");
            }
            else
            {
                MessageBox.Show("No se pudo crear la tarjeta");
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            int id = int.Parse(TarjetasDeCredito.CurrentRow.Cells[1].Value.ToString());

            if (MiBanco.BajaTarjetaCredito(id))
            {
                TarjetasDeCredito.DataSource = MiBanco.MostrarTarjetasDeCreditoUsuarioActual();

                MessageBox.Show("Tarjeta dada de baja con exito");
            }
            else
            {
                MessageBox.Show("No se pudo eliminar la tarjeta");
            }

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button22.Visible = true;
            button17.Visible = true;
        }

        private void ButtonFiltrarDetalle(object sender, EventArgs e)
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

        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void montodep_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxtransf_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxCBU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtAgregarTitular_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTitular_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtMontoPlazo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void montoPago_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ButtonPagarPagoConTarjeta(object sender, EventArgs e)
        {
            if (MiBanco.MostrarPagosPendiente().Count > 0)
            {
                int idPago = int.Parse(PagosPendientes.CurrentRow.Cells[1].Value.ToString());
                int idTarjetaCredito = int.Parse(dataGridView7.CurrentRow.Cells[1].Value.ToString());
                int idCajaAhorro = 0;

                if (MiBanco.PagarPago(idCajaAhorro, idTarjetaCredito, idPago))
                {
                    PagosPendientes.DataSource = MiBanco.MostrarPagosPendienteUsuarioActual().ToArray();
                    PagosRealizados.DataSource = MiBanco.MostrarPagosRealizadoUsuarioActual().ToArray();
                    dataGridView7.DataSource = MiBanco.MostrarTarjetasDeCreditoUsuarioActual();
                    MessageBox.Show("Se realizo el pago con exito");
                }
                else
                {
                    MessageBox.Show("No se pudo realizar el pago");
                }

            }
        }

        private void ButtonBajaPago(object sender, EventArgs e)
        {

            if (PagosRealizados.CurrentRow.Cells[1].Value != null)
            {
                button26.Visible = false;
                int idPago = int.Parse(PagosRealizados.CurrentRow.Cells[1].Value.ToString());

                if (MiBanco.BajaPagoUsuarioActual(idPago))
                {
                    MessageBox.Show("Pago eliminado correctamente");
                    PagosPendientes.DataSource = MiBanco.MostrarPagosPendienteUsuarioActual().ToArray();
                    PagosRealizados.DataSource = MiBanco.MostrarPagosRealizadoUsuarioActual().ToArray();
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

        private void ButtonMostrarPagarTarjeta(object sender, EventArgs e)
        {
            dataGridView1.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual().ToArray();
            dataGridView1.Visible = true;
            btnPagarTarjeta.Visible = false;
        }

        private void ButtonPagarTarjeta(object sender, EventArgs e)
        {
            int idTarjetaCredito = int.Parse(TarjetasDeCredito.CurrentRow.Cells[1].Value.ToString());
            int idCajaAhorro = int.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            bool tarjetaOk = true;
            bool cajaOk = true;

            if (TarjetasDeCredito.CurrentRow.Cells[1].Value == null)
            {
                MessageBox.Show("No se selecciono ninguna tarjeta");
                tarjetaOk = false;
            }

            if (dataGridView1.CurrentRow.Cells[1].Value == null)
            {
                MessageBox.Show("No se selecciono ninguna caja de ahorro");
                cajaOk = false;
            }

            if (tarjetaOk && cajaOk)
            {
                if (MiBanco.PagarTarjeta(idTarjetaCredito, idCajaAhorro))
                {
                    MessageBox.Show("Tarjeta pagada con exito");
                }
            }
        }

        private void PagosPendientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label27.Visible = true;
            button15.Visible = true;
            button25.Visible = true;
            button12.Visible = false;
        }

        private void PagosRealizados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button12.Visible = true;

            button13.Visible = false;
            button26.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            dataGridView6.Visible = false;
            dataGridView7.Visible = false;

            dataGridView6.ClearSelection();
            dataGridView7.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnPagarTarjeta.Visible = true;
        }

        private void dgvCajasPlazo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DateTime fechaInicio = DateTime.Now.Date;
            DateTime fechaFin = fechaInicio.AddYears(1).Date;
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

        private void button15_Click(object sender, EventArgs e)
        {
            dataGridView6.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual();

            label6.Visible = true;
            dataGridView6.Visible = true;
            button13.Visible = true;

            label7.Visible = false;
            dataGridView7.ClearSelection();
            dataGridView7.Visible = false;
            button26.Visible = false;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            dataGridView7.DataSource = MiBanco.MostrarTarjetasDeCreditoUsuarioActual();

            label7.Visible = true;
            dataGridView7.Visible = true;
            button26.Visible = true;

            dataGridView6.ClearSelection();
            label6.Visible = false;
            dataGridView6.Visible = false;
            button13.Visible = false;
        }

        private void ButtonPagarPagoConCaja(object sender, EventArgs e)
        {
            if (MiBanco.MostrarPagosPendiente().Count > 0)
            {

                int idPago = int.Parse(PagosPendientes.CurrentRow.Cells[1].Value.ToString());
                int idCajaAhorro = int.Parse(dataGridView6.CurrentRow.Cells[1].Value.ToString());
                int idTarjetaCredito = 0;


                if (MiBanco.PagarPago(idCajaAhorro, idTarjetaCredito, idPago))
                {
                    PagosPendientes.DataSource = MiBanco.MostrarPagosPendienteUsuarioActual().ToArray();
                    PagosRealizados.DataSource = MiBanco.MostrarPagosRealizadoUsuarioActual().ToArray();
                    dataGridView6.DataSource = MiBanco.MostrarCajasDeAhorroUsuarioActual();
                    MessageBox.Show("Se realizo el pago con exito");
                }
                else
                {
                    MessageBox.Show("No se pudo realizar el pago");
                }
            }
        }
    }
}

