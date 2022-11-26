using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp1
{
    public partial class Log : Form
    {
        public bool Logued;
        public string? NombreUsuario { get; set; }
        public string? Clave { get; set; }
        public string? Dni { get; set; }
        public string? Mail { get; set; }
        public string? Apellido { get; set; }

        public Banco Banco;

        public TransfDelegado? TransfEvento;

        public Log(Banco b)
        {
            Logued = false;
            InitializeComponent();
            Banco = b;
        }

        public delegate void TransfDelegado(string nombreUsuario, int dni, string clave);

        private void ButtonLogin(object sender, EventArgs e)
        {
            NombreUsuario = textBoxLogUser.Text;
            Dni = textBox1.Text;
            Clave = textBoxLogPass.Text;
            bool nombreOk = true;
            bool claveOk = true;
            int dniInt = 0;

            if (!Dni.Equals(""))
            {
                dniInt = Convert.ToInt32(Dni);
            }

            if (NombreUsuario.Equals(""))
            {
                MessageBox.Show("No se ingreso el nombre de usuario");
                nombreOk = false;
            }

            if (Clave.Equals(""))
            {
                MessageBox.Show("No se ingreso la clave");
                claveOk = false;
            }

            if (nombreOk && claveOk)
            {
                TransfEvento(NombreUsuario, dniInt, Clave);
                Logued = true;
            }
            else
            {
                MessageBox.Show("Error al iniciar sesion");
            }
        }

        private void ButtonAgregarUsuario(object sender, EventArgs e)
        {
            Dni = textBoxDni.Text;
            NombreUsuario = textBoxNombre.Text;
            Apellido = textBoxApellido.Text;
            Mail = textBoxMail.Text;
            Clave = textBoxPass.Text;
            bool isAdmin = false;
            bool bloqueado = false;
            int dniInt = 0;
            bool dniOk = true;
            bool nombreOk = true;
            bool apellidoOk = true;
            bool mailOk = true;
            bool claveOk = true;

            if (!Dni.Equals(""))
            {
                dniInt = Convert.ToInt32(Dni);
            }

            if (Dni.Equals(""))
            {
                MessageBox.Show("No se ingreso ningun dni");
                dniOk = false;
            }

            if (NombreUsuario.Equals(""))
            {
                MessageBox.Show("No se ingreso ningun nombre");
                nombreOk = false;
            }

            if (Apellido.Equals(""))
            {
                MessageBox.Show("No se ingreso ningun apellido");
                apellidoOk = false;
            }

            if (Mail.Equals(""))
            {
                MessageBox.Show("No se ingreso ningun mail");
                mailOk = false;
            }

            if (Clave.Equals(""))
            {
                MessageBox.Show("No se ingreso ninguna contraseña");
                claveOk = false;
            }

            if (dniOk && nombreOk && apellidoOk && mailOk && claveOk)
            {
                if (Banco.AltaDeUsuario(dniInt, NombreUsuario, Apellido, Mail, Clave, isAdmin, bloqueado))
                {
                    MessageBox.Show("Usuario Agregado con éxito");
                }
                else
                {
                    MessageBox.Show("Error al agregar el usuario");
                }
            }
        }

        private void TextBoxDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
