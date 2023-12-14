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
    public partial class Frm_Novo_Projeto : Form
    {
        int Alfa, BarraX, BarraY, BarraTotal, RDtamanhos = 10;

        double BarraValor;

        public string ImgL, Nomep , Propriedades;
        public int NX, NY;
        public Boolean AtivarTudo = false;
        public Image Imm;
        public Color Cb;

        Bitmap IMG;
        Graphics g;

        public Frm_Novo_Projeto()
        {
            Controls.Clear();
            InitializeComponent();
            TxTNomeP.Focus();
            GraphicsPath forma0 = new GraphicsPath();
            forma0.AddEllipse(0, 0, BtnFechar.Width, BtnFechar.Height);
            BtnFechar.Region = new Region(forma0);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            BarraX = BarraDeDados.Width;
            BarraY = BarraDeDados.Height;
            BarraValor = BarraX / 100.0;
            BarraTotal = 0;
            IMG = new Bitmap(BarraX, BarraY);
        }

        private void BarradeTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void Verificar_Tick(object sender, EventArgs e)
        {
            Prever.Size = new Size(Convert.ToInt32(NumeroLargura.Value) / RDtamanhos, Convert.ToInt32(NumeroAltura.Value) / RDtamanhos);
            LblPerfil.Text = TxTNomeP.Text + ". Tamanho: Largura: " + NumeroLargura.Value.ToString() + ", Altura: " + NumeroAltura.Value.ToString();
            LblValorL.Text = NumeroLargura.Value.ToString();
            LblValorA.Text = NumeroAltura.Value.ToString();
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

        private void BtnTransparente_Click(object sender, EventArgs e)
        {
            BtnOKNovo_Click(sender, e);
        }

        private void RDm_CheckedChanged(object sender, EventArgs e)
        {
            RDtamanhos = 10;
        }

        private void RDg_CheckedChanged(object sender, EventArgs e)
        {
            RDtamanhos = 5;
        }

        private void RDp_CheckedChanged(object sender, EventArgs e)
        {
            RDtamanhos = 20;
        }

        private void BtnAbrir_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Super imagem";
            op.Filter = "Imagem Boa (*.png)|*.png|Imagem (*.jpg)|*.jpg|Imagem (*.jpeg)|*.jpeg|Imagem Animada (*.gif)|*.gif|Arquivos (*.Nasca)|*.Nasca";
            if (op.ShowDialog() == DialogResult.OK)
            {
                int x, y;
                Prever.Image = (Image)Image.FromFile(op.FileName).Clone();
                Prever.SizeMode = PictureBoxSizeMode.AutoSize;
                x = Prever.Size.Width;
                y = Prever.Size.Height;
                NumeroLargura.Value = x;
                NumeroAltura.Value = y;
                Prever.SizeMode = PictureBoxSizeMode.Zoom;
                ImgL = op.FileName;
                if(TxTNomeP.Text == "")
                {
                    TxTNomeP.Text = op.SafeFileName;
                }
            }
        }

        private void ListadeElementos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListadeElementos.Text == "Cinema")
            {
                NumeroLargura.Value = 1080;
                NumeroAltura.Value = 720;
            }
            else if (ListadeElementos.Text == "Minhatura")
            {
                NumeroLargura.Value = 720;
                NumeroAltura.Value = 350;
            }
            else if (ListadeElementos.Text == "Icone")
            {
                NumeroLargura.Value = 500;
                NumeroAltura.Value = 500;
            }
            else if (ListadeElementos.Text == "16 x 9")
            {
                NumeroLargura.Value = 1920;
                NumeroAltura.Value = 1080;
            }
            else if (ListadeElementos.Text == "21 x 9")
            {
                NumeroLargura.Value = 2190;
                NumeroAltura.Value = 1080;
            }
            else if (ListadeElementos.Text == "3 x 4")
            {
                NumeroLargura.Value = 300;
                NumeroAltura.Value = 400;
            }
            else if (ListadeElementos.Text == "4K")
            {
                NumeroLargura.Value = 4000;
                NumeroAltura.Value = 3000;
            }
            else if (ListadeElementos.Text == "3GP")
            {
                NumeroLargura.Value = 560;
                NumeroAltura.Value = 320;
            }
            else if (ListadeElementos.Text == "Comum")
            {
                NumeroLargura.Value = 1280;
                NumeroAltura.Value = 720;
            }
        }

        private void BtnCorDeFundo_Click(object sender, EventArgs e)
        {
            if(Cord.ShowDialog() == DialogResult.OK)
            {
                Prever.BackColor = Cord.Color;
                HSANovo.Value = 255;
            }
        }

        private void HSANovo_Scroll(object sender, ScrollEventArgs e)
        {
            LblValorHSANovo.Text = HSANovo.Value.ToString();
            Alfa = HSANovo.Value;
            Prever.BackColor = Color.FromArgb(Alfa, Cord.Color);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void Frm_Novo_Projeto_Load(object sender, EventArgs e)
        {
            BarraDeDados.Visible = false;
            BarraTotal = 0;
            LblValorHSANovo.Visible = false;
            BtnOKNovo.Visible = false;
            HSANovo.Visible = false;
            HSANovo.Value = 255;
            LblValorHSANovo.Text = "255";
            Prever.BackColor = Color.White;
            Prever.Image = null;
            TxTNomeP.Text = "";
            label4.ForeColor = Color.White;
        }

        private void BtnFechar_Click(object sender, EventArgs e)                           
        {
            this.DialogResult = DialogResult.No;                                                          
        }                             

        private void TT_Tick(object sender, EventArgs e)
        {
            g = Graphics.FromImage(IMG);

            g.Clear(Color.FromArgb(85, 0, 115));

            g.FillRectangle(Brushes.DeepPink, new Rectangle(0, 0, (int)(BarraTotal * BarraValor), BarraY));

            g.DrawString(BarraTotal + "%", new Font("Arial", BarraY / 2), Brushes.White, new PointF(BarraX - 40, BarraY / 10));

            BarraDeDados.Image = IMG;

            BarraTotal++;
            if (BarraTotal > 100)
            {
                g.Dispose();
                TT.Stop();
                TT.Enabled = false;
                Imm = null;
                NX = Convert.ToInt32(NumeroLargura.Value);
                NY = Convert.ToInt32(NumeroAltura.Value);
                Cb = Prever.BackColor;
                Propriedades = TxTNomeP.Text + ": " + NumeroLargura.Value.ToString() + " X " + NumeroAltura.Value.ToString();
                Nomep = TxTNomeP.Text;
                AtivarTudo = true;
                ////////////////////////////////////////////=========
                this.DialogResult = DialogResult.OK;
            }
            if(BarraTotal > 50)
            {
                PnlNotfi.Visible = false;
            }
        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            if (TxTNomeP.Text != "")
            {
                BarraDeDados.Visible = true;
                TT.Enabled = true;
                Verificar.Enabled = false;
                if (PnlBarradetitulo.BackColor == Color.Red)
                {
                    lblNotifi.Text = "Agora sim! Tudo OK";
                    label4.ForeColor = Color.Wheat;
                    splitter1.BackColor = Color.FromArgb(85, 0, 115);
                    splitter2.BackColor = splitter1.BackColor;
                    splitter3.BackColor = splitter2.BackColor;
                    splitter4.BackColor = splitter3.BackColor;
                    PnlBarradetitulo.BackColor = Color.Black;
                }
            }
            else
            {
                PnlNotfi.Visible = true;
                lblNotifi.Text = "Por favor verifique se o nome está OK";
                label4.ForeColor = Color.Red;
                splitter1.BackColor = label4.ForeColor;
                splitter2.BackColor = splitter1.BackColor;
                splitter3.BackColor = splitter2.BackColor;
                splitter4.BackColor = splitter3.BackColor;
                PnlBarradetitulo.BackColor = splitter4.BackColor;
                TxTNomeP.Focus();
            }
        }
    }                                                                                                  
}