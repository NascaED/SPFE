using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHOW_PHOTO_FOTO_EDITOR
{
    public partial class Frm_MSG : Form
    {
        Boolean Opcoes = false;

        public Frm_MSG(string Titulo, string Conteudo, Boolean Op, string BtnMeio, Image ico)
        {
            InitializeComponent();
            BtnOK.Focus();
            LblTitulo.Text = Titulo;
            LblConteudo.Text = Conteudo;
            Opcoes = Op;
            IMGicone.Image = ico;
            this.Size = new Size(100 + LblConteudo.Size.Width, 119 + LblConteudo.Size.Height);
            if (this.Size.Height >= 138)
            {
                this.Size = new Size(100 + LblConteudo.Size.Width, 80 + LblConteudo.Size.Height);
            }

            if (Opcoes == true)
            {
                BtnOpcoes.Visible = true;
            }
            else
            {
                BtnOpcoes.Visible = false;
            }

            BtnOpcoes.Text = BtnMeio;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void BtnOpcoes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsPath forma0 = new GraphicsPath();
            forma0.AddEllipse(0, 0, BtnFechar.Width, BtnFechar.Height);
            BtnFechar.Region = new Region(forma0);
        }

        private void MoverJanela_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
