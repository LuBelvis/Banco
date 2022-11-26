using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Banco banco;
        Log hijoLoguin;
        Menu hijoMain;
        internal string texto;
        Admin hijoAdmin;
        public bool touched;
        public Usuario? usuarioActual;

        public Form1()
        {
            InitializeComponent();
            banco = new Banco();
            hijoLoguin = new Log(banco);
            hijoLoguin.Logued = false;
            hijoLoguin.MdiParent = this;
            hijoLoguin.TransfEvento += TransfDelegado;
            hijoLoguin.Show();
            touched = false;

        }


        private void TransfDelegado(string NombreUsuario, int Dni, string Clave)
        {
            Usuario? usuarioDni = banco.BuscarUsuarioBloqueado(NombreUsuario, Dni);
            Usuario usuarioClave = banco.IniciarSesion(NombreUsuario, Dni, Clave);

            if (usuarioClave != null)
            {
                MessageBox.Show("Correcto, Usuario: " + NombreUsuario, "Log In", MessageBoxButtons.OK, MessageBoxIcon.Information);
                hijoLoguin.Close();
                if (usuarioClave.IsAdmin)
                {
                    hijoAdmin = new Admin(banco);
                    hijoAdmin.Usuario = NombreUsuario;
                    hijoAdmin.MdiParent = this;
                    hijoAdmin.TransfEvento += TransfDelegadoAdmin;
                    usuarioActual = banco.UsuarioActual;
                    hijoAdmin.Show();
                }
                else
                {
                    hijoMain = new Menu(banco);
                    hijoMain.Usuario = NombreUsuario;
                    hijoMain.MdiParent = this;
                    hijoMain.TransfEvento += TransfDelegadoMenu;
                    usuarioActual = banco.UsuarioActual;
                    hijoMain.Show();
                }
            }
            else
            {
                if (usuarioDni != null && usuarioDni.IsBloqueado)
                {
                    MessageBox.Show("Usuario bloqueado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Log in incorrecto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void TransfDelegadoMenu()
        {
            banco.CerrarSesion();
            MessageBox.Show("Hasta luego", "Adios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            hijoMain.Close();
            hijoLoguin = new Log(banco);
            hijoLoguin.MdiParent = this;
            hijoLoguin.TransfEvento += TransfDelegado;
            hijoLoguin.Show();

        }

        private void TransfDelegadoAdmin()
        {
            banco.CerrarSesion();
            MessageBox.Show("Hasta luego", "Adios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            hijoAdmin.Close();
            hijoLoguin = new Log(banco);
            hijoLoguin.MdiParent = this;
            hijoLoguin.TransfEvento += TransfDelegado;
            hijoLoguin.Show();

        }
    }
}
