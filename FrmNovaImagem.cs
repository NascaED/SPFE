using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Show_Photo_Foto_Editor
{
    public partial class FrmNovaImagem : Form
    {
        public FrmNovaImagem()
        {
            InitializeComponent();
        }/*

        private void BtnAbrir_Click(object sender, EventArgs e)
        {
            if (AbrirCoisas.ShowDialog() == DialogResult.OK)
            {
                int x, y;
                Prever.Image = (Image)Image.FromFile(AbrirCoisas.FileName).Clone();
                Prever.SizeMode = PictureBoxSizeMode.AutoSize;
                x = Prever.Size.Width;
                y = Prever.Size.Height;
                NumeroLargura.Value = x;
                NumeroAltura.Value = y;
                LblPerfil.Text = AbrirCoisas.FileName.ToString() + " " + Prever.Size.Width.ToString() + " X " + Prever.Size.Height.ToString();
                Prever.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void BtnTransparente_Click(object sender, EventArgs e)
        {
            if (LblValorHSANovo.Visible == false)
            {
                LblValorHSANovo.Visible = true;
                BtnOKNovo.Visible = true;
                HSANovo.Visible = true;
            }
            else
            {
                LblValorHSANovo.Visible = false;
                BtnOKNovo.Visible = false;
                HSANovo.Visible = false;
            }
        }

        private void HSANovo_Scroll(object sender, ScrollEventArgs e)
        {
            int i;
            LblValorHSANovo.Text = HSANovo.Value.ToString();
            i = HSANovo.Value;
            Prever.BackColor = Color.FromArgb(i, Prever.BackColor);
        }

        private void BtnOKNovo_Click(object sender, EventArgs e)
        {
            if (LblValorHSANovo.Visible == false)
            {
                LblValorHSANovo.Visible = true;
                BtnOKNovo.Visible = true;
                HSANovo.Visible = true;
            }
            else
            {
                LblValorHSANovo.Visible = false;
                BtnOKNovo.Visible = false;
                HSANovo.Visible = false;
            }
        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            if (TxTNomeP.Text != "")
            {
                Canvas.Refresh();
                Canvas.Image = null;
                Canvas.Image = Prever.Image;
                Canvas.Size = new Size(Convert.ToInt32(NumeroLargura.Value), Convert.ToInt32(NumeroAltura.Value));
                Canvas.BackColor = Prever.BackColor;
                LblNomeP.Text = TxTNomeP.Text + " " + NumeroLargura.Value.ToString() + " x " + NumeroAltura.Value.ToString();

                PnlPrincipal.Visible = true;
                PnlBotaoLado2.Enabled = true;
                PnlBotaoLado1.Enabled = true;
                PnlNovo.Visible = false;
            }
            else
            {
                label4.ForeColor = Color.Red;
            }
        }

        private void BtnCorDeFundo_Click(object sender, EventArgs e)
        {
            if (Corescdg.ShowDialog() == DialogResult.OK)
            {
                Prever.BackColor = Corescdg.Color;
            }
        }*/
    }
}
