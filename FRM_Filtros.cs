using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHOW_PHOTO_FOTO_EDITOR
{
    public partial class FRM_Filtros : Form
    {
        Pen p;

        Graphics g;

        int lx, ly, sw, sh, WIDTH = 300, HEIGHT = 300, HAND = 150, x, y, cx, cy, u, tx, ty, lim = 20, tt;

        string nomi;

        Bitmap IMG, bb, bmp;

        Image i,i2;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public FRM_Filtros(string Nome, string projeto, Image canvas)
        {
            InitializeComponent();

            bmp = new Bitmap(WIDTH + 1, HEIGHT + 1);
            cx = WIDTH / 2;
            cy = HEIGHT / 2;
            u = 0;

            i = canvas;
            i2 = canvas;
            nomi = Nome;
            lblInfo.Text = projeto;
            PreverFiltro.BackgroundImage = canvas;
            lblNome.Text = "Bem-Vindo(a) " + Nome + "!";

            GraphicsPath forma0 = new GraphicsPath();
            GraphicsPath forma1 = new GraphicsPath();
            GraphicsPath forma2 = new GraphicsPath();
            GraphicsPath forma3 = new GraphicsPath();
            forma0.AddEllipse(0, 0, BtnFechar.Width, BtnFechar.Height);
            forma1.AddEllipse(0, 0, BtnQuadricular.Width, BtnQuadricular.Height);
            forma2.AddEllipse(0, 0, BtnMaximizar.Width, BtnMaximizar.Height);
            forma3.AddEllipse(0, 0, BtnMinimizar.Width, BtnMinimizar.Height);
            BtnFechar.Region = new Region(forma0);
            BtnQuadricular.Region = new Region(forma1);
            BtnMaximizar.Region = new Region(forma2);
            BtnMinimizar.Region = new Region(forma3);
        }

        private void Carregamento_Tick(object sender, EventArgs e)
        {
            p = new Pen(Color.FromArgb(213, 0, 178), 1f);
            g = Graphics.FromImage(bmp);

            int tu = (u - lim) % 360;

            if (u >= 0 && u <= 180)
            {
                x = cx + (int)(HAND * Math.Sin(Math.PI * u / 180));
                y = cy - (int)(HAND * Math.Cos(Math.PI * u / 180));
            }
            else
            {
                x = cx - (int)(HAND * -Math.Sin(Math.PI * u / 180));
                y = cy - (int)(HAND * Math.Cos(Math.PI * u / 180));
            }

            if (tu >= 0 && tu <= 180)
            {
                tx = cx + (int)(HAND * Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));
            }
            else
            {
                tx = cx - (int)(HAND * -Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));
            }

            g.DrawEllipse(p, 0, 0, WIDTH, HEIGHT);
            g.DrawEllipse(p, 80, 80, WIDTH - 160, HEIGHT - 160);

            g.DrawLine(p, new Point(cx, 0), new Point(cx, HEIGHT));
            g.DrawLine(p, new Point(0, cy), new Point(WIDTH, cy));

            g.DrawLine(new Pen(Color.Black, 1f), new Point(cx, cy), new Point(tx, ty));
            g.DrawLine(p, new Point(cx, cy), new Point(x, y));

            Img_EFX.Image = bmp;

            p.Dispose();
            g.Dispose();

            u++;
            if (u == tt)
            {
                bmp = new Bitmap(WIDTH + 1, HEIGHT + 1);
                cx = WIDTH / 2;
                cy = HEIGHT / 2;
                u = 0;
                Carregamento.Enabled = false;
                Img_EFX.Image = bb;
            }
        }

        private void BtnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;
            BtnMaximizar.Visible = false;
            BtnQuadricular.Visible = true;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            BordaBa.Visible = false;
            BordaCi.Visible = false;
            BordaDi.Visible = false;
            BordaEs.Visible = false;
        }

        private void BtnQuadricular_Click(object sender, EventArgs e)
        {
            TopMost = false;
            BtnMaximizar.Visible = true;
            BtnQuadricular.Visible = false;
            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
            BordaBa.Visible = true;
            BordaCi.Visible = true;
            BordaDi.Visible = true;
            BordaEs.Visible = true;
        }
        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            if (Salvarap.Visible == true)
            {
                Frm_MSG MSG = new Frm_MSG("Salvar??", "Se você sair agora sua\nimagem não sera salva!", true, "Salvar Agora!", Properties.Resources.questão);
                if (MSG.ShowDialog() == DialogResult.OK)
                {
                    Application.Exit();
                }
                else if (MSG.DialogResult == DialogResult.Abort)
                {
                    BtnSalvar_Click(sender, e);
                }
            }
            else
            {
                Application.Exit();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            this.PnlJanela.Region = region;
            this.Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(mover);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        void SemFX()
        {
            btnSemFiltros.BackgroundImage = i2;
        }

        void LuzFX()
        {
            Bitmap bmpInverted = new Bitmap(i);
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{1+0.9f, 0, 0, 0, 0},
                new float[]{0, 1+1.5f, 0, 0, 0},
                new float[]{0, 0, 1+1.3f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            ia.SetColorMatrix(cmPicture);
            Graphics g = Graphics.FromImage(bmpInverted);
            g.DrawImage(i, new Rectangle(0, 0, i.Width, i.Height), 0, 0, i.Width, i.Height, GraphicsUnit.Pixel, ia);

            g.Dispose();
            BtnLuz.BackgroundImage = bmpInverted;
        }

        void C()
        {
            ZerarBarrinhas(); PeB(); LuzFX(); Filto2(); SemFX(); Fof(); filtroLegal(); Win();
            if (PnlC.Visible == false)
            {
                PnlC.Visible = true;
            }
            else
            {
                PnlC.Visible = false;
            }
        }

        void Filto2()
        {
            Bitmap bmpInverted = new Bitmap(i);

            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{.393f, .349f+0.5f, .272f, 0, 0},
                new float[]{.769f+0.3f, .686f, .534f, 0, 0},
                new float[]{.189f, .168f, .131f+0.5f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            ia.SetColorMatrix(cmPicture);
            Graphics g = Graphics.FromImage(bmpInverted);
            g.DrawImage(i, new Rectangle(0, 0, i.Width, i.Height), 0, 0, i.Width, i.Height, GraphicsUnit.Pixel, ia);

            g.Dispose();
            BtnFiltoAzulado.BackgroundImage = bmpInverted;
        }

        void PeB()
        {
            Bitmap bmpInverted = new Bitmap(i);

            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{0.299f, 0.299f, 0.299f, 0, 0},
                new float[]{0.587f, 0.587f, 0.587f, 0, 0},
                new float[]{0.114f, 0.114f, 0.114f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 0}
            });
            ia.SetColorMatrix(cmPicture);
            Graphics g = Graphics.FromImage(bmpInverted);

            g.DrawImage(i, new Rectangle(0, 0, i.Width, i.Height), 0, 0, i.Width, i.Height, GraphicsUnit.Pixel, ia);

            g.Dispose();
            PeBFX.BackgroundImage = bmpInverted;
        }

        void Win()
        {
             Bitmap bmpInverted = new Bitmap(i);
             ImageAttributes ia = new ImageAttributes();
             ColorMatrix cmPicture = new ColorMatrix(new float[][]
             {
                 new float[]{1,0,0,0,0},
                 new float[]{0,1,0,0,0},
                 new float[]{0,0,1,0,0},
                 new float[]{0, 0, 0, 1, 0},
                 new float[]{0, 0, 1, 0, 1}
             });
             ia.SetColorMatrix(cmPicture);
             Graphics g = Graphics.FromImage(bmpInverted);
             g.DrawImage(i, new Rectangle(0, 0, i.Width, i.Height), 0, 0, i.Width, i.Height, GraphicsUnit.Pixel, ia);

             g.Dispose();
             BtnFiltroWin.BackgroundImage = bmpInverted;
        }

        void filtroLegal()
        {
             Bitmap bmpInverted = new Bitmap(i);
             ImageAttributes ia = new ImageAttributes();
             ColorMatrix cmPicture = new ColorMatrix(new float[][]
             {
                 new float[]{.393f+0.3f, .349f, .272f, 0, 0},
                 new float[]{.769f, .686f+0.2f, .534f, 0, 0},
                 new float[]{.189f, .168f, .131f+0.9f, 0, 0},
                 new float[]{0, 0, 0, 1, 0},
                 new float[]{0, 0, 0, 0, 1}
             });
             ia.SetColorMatrix(cmPicture);
             Graphics g = Graphics.FromImage(bmpInverted);
             g.DrawImage(i, new Rectangle(0, 0, i.Width, i.Height), 0, 0, i.Width, i.Height, GraphicsUnit.Pixel, ia);

             g.Dispose();
             BtnFiltrodiferenciado.BackgroundImage = bmpInverted;
        }

        void Timg()
        {
             pictureBox1.Image = PreverFiltro.Image;
             pictureBox2.Image = PreverFiltro.Image;
             pictureBox3.Image = PreverFiltro.Image;
             pictureBox4.Image = PreverFiltro.Image;
             pictureBox5.Image = PreverFiltro.Image;
             pictureBox6.Image = PreverFiltro.Image;
             pictureBox7.Image = PreverFiltro.Image;
             pictureBox8.Image = PreverFiltro.Image;
             pictureBox9.Image = PreverFiltro.Image;
            pictureBox10.Image = PreverFiltro.Image;
            pictureBox11.Image = PreverFiltro.Image;
            pictureBox12.Image = PreverFiltro.Image;
            pictureBox13.Image = PreverFiltro.Image;
            pictureBox14.Image = PreverFiltro.Image;
            pictureBox15.Image = PreverFiltro.Image;
            pictureBox16.Image = PreverFiltro.Image;
            pictureBox17.Image = PreverFiltro.Image;
            pictureBox18.Image = PreverFiltro.Image;
            pictureBox19.Image = PreverFiltro.Image;
            pictureBox20.Image = PreverFiltro.Image;
            pictureBox21.Image = PreverFiltro.Image;
            pictureBox22.Image = PreverFiltro.Image;
            pictureBox23.Image = PreverFiltro.Image;
            pictureBox24.Image = PreverFiltro.Image;
            pictureBox25.Image = PreverFiltro.Image;
            pictureBox26.Image = PreverFiltro.Image;
            pictureBox27.Image = PreverFiltro.Image;
            pictureBox28.Image = PreverFiltro.Image;
            pictureBox29.Image = PreverFiltro.Image;
            pictureBox30.Image = PreverFiltro.Image;
            pictureBox31.Image = PreverFiltro.Image;
            pictureBox32.Image = PreverFiltro.Image;
            PnlTimelineOP.Top = 10;
            PnlTimeLine.Top = 8;
            PnlCamada1.Top = 67;
            Pnltempinho.Top = 64;
            PnlTimeLine.Visible = true;
            PnlTimelineOP.Visible = true;
            Pnltempinho.Size = new Size(PnlTimeLine.Width, PnlTimeLine.Height);
            Salvarap.Visible = true;
        }

        void Fof()
        {
            Bitmap bmpInverted = new Bitmap(i);
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{1+0.3f, 0, 0, 0, 0},
                new float[]{0, 1+0.7f, 0, 0, 0},
                new float[]{0, 0, 1+1.3f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            ia.SetColorMatrix(cmPicture);
            Graphics g = Graphics.FromImage(bmpInverted);
            g.DrawImage(i, new Rectangle(0, 0, i.Width, i.Height), 0, 0, i.Width, i.Height, GraphicsUnit.Pixel, ia);

            g.Dispose();
            BtnLuizinha.BackgroundImage = bmpInverted;
        }

        void Cores()
        {
            float changered = redbar.Value * 0.1f;
            float changegreen = greenbar.Value * 0.1f;
            float changeblue = bluebar.Value * 0.1f;

            reload();

            Bitmap bmpInverted = new Bitmap(i);

            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{1+changered, 0, 0, 0, 0},
                new float[]{0, 1+changegreen, 0, 0, 0},
                new float[]{0, 0, 1+changeblue, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            ia.SetColorMatrix(cmPicture);
            Graphics g = Graphics.FromImage(bmpInverted);   

            g.DrawImage(i, new Rectangle(0, 0, i.Width, i.Height), 0, 0, i.Width, i.Height, GraphicsUnit.Pixel, ia);

            g.Dispose();
            PreverFX.Image = bmpInverted;
            bb = new Bitmap(PreverFX.Image);
        }

        void reload()
        {
            PreverFX.Image = i;
        }

        private void PnlBarradetitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private int tolerance = 12;
        private const int WM_NCHITTEST = 132;

        private void ImgRR_Click(object sender, EventArgs e)
        {
            bb = new Bitmap(ImgRR.Image);
            Img_EFX.Image = bb;
            ImgRR.Visible = false;
            ImgGG.Visible = false;
            ImgBB.Visible = false;
            BtnRGB.Visible = true;
        }

        private void ImgGG_Click(object sender, EventArgs e)
        {
            bb = new Bitmap(ImgGG.Image);
            Img_EFX.Image = bb;
            ImgRR.Visible = false;
            ImgGG.Visible = false;
            ImgBB.Visible = false;
            BtnRGB.Visible = true;
        }

        private void ImgBB_Click(object sender, EventArgs e)
        {
            bb = new Bitmap(ImgBB.Image);
            Img_EFX.Image = bb;
            ImgRR.Visible = false;
            ImgGG.Visible = false;
            ImgBB.Visible = false;
            BtnRGB.Visible = true;
        }

        void ZerarBarrinhas()
        {
            redbar.Value = 0;
            greenbar.Value = 0;
            bluebar.Value = 0;
            LblRR.Text = "0";
            LblGG.Text = "0";
            LblBB.Text = "0";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            C();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            C();
        }

        private void btnFecharPnlC_Click(object sender, EventArgs e)
        {
            PnlC.Visible = false;
        }

        private void redbar_Scroll(object sender, EventArgs e)
        {
            Cores();
            LblRR.Text = redbar.Value.ToString();
        }

        private void greenbar_Scroll(object sender, EventArgs e)
        {
            LblGG.Text = greenbar.Value.ToString();
            Cores();
        }

        private void PeBFX_Click(object sender, EventArgs e)
        {
            lblInfoFiltro.Text = "Efeito Preto e Branco";
            ZerarBarrinhas();
            PreverFX.Image = PeBFX.BackgroundImage;
            i = PeBFX.BackgroundImage;
            bb = new Bitmap(i);
        }

        private void BtnLuz_Click(object sender, EventArgs e)
        {
            lblInfoFiltro.Text = "Luz timida";
            ZerarBarrinhas();
            PreverFX.Image = BtnLuz.BackgroundImage;
            i = BtnLuz.BackgroundImage;
            bb = new Bitmap(i);
        }

        private void btnSemFiltros_Click(object sender, EventArgs e)
        {
            lblInfoFiltro.Text = "Tira TODOS os efeitos";
            ZerarBarrinhas();
            PreverFX.Image = btnSemFiltros.BackgroundImage;
            i = btnSemFiltros.BackgroundImage;
            bb = new Bitmap(i);
        }

        private void BtnLuizinha_Click(object sender, EventArgs e)
        {
            lblInfoFiltro.Text = "Luz nervozinha";
            ZerarBarrinhas();
            PreverFX.Image = BtnLuizinha.BackgroundImage;
            i = BtnLuizinha.BackgroundImage;
            bb = new Bitmap(i);
        }

        private void BtnFiltrodiferenciado_Click(object sender, EventArgs e)
        {
            lblInfoFiltro.Text = "Tons diferenciados";
            ZerarBarrinhas();
            PreverFX.Image = BtnFiltrodiferenciado.BackgroundImage;
            i = BtnFiltrodiferenciado.BackgroundImage;
            bb = new Bitmap(i);
        }

        private void BtnFiltroWin_Click(object sender, EventArgs e)
        {
            lblInfoFiltro.Text = "Azulão";
            ZerarBarrinhas();
            PreverFX.Image = BtnFiltroWin.BackgroundImage;
            i = BtnFiltroWin.BackgroundImage;
            bb = new Bitmap(i);
        }

        private void BtnFiltoAzulado_Click(object sender, EventArgs e)
        {
            lblInfoFiltro.Text = "'Prof' O filtro do louco";
            ZerarBarrinhas();
            PreverFX.Image = BtnFiltoAzulado.BackgroundImage;
            i = BtnFiltoAzulado.BackgroundImage;
            bb = new Bitmap(i);
        }

        private void BtnO_Click(object sender, EventArgs e)
        {
            OOOO();
        }

        private void bluebar_Scroll(object sender, EventArgs e)
        {
            Cores();
            LblBB.Text = bluebar.Value.ToString();
        }

        private const int mover = 17;

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            PreverFiltro.Enabled = true;
            BtnPause.Visible = true;
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            BtnPause.Visible = false;
            PreverFiltro.Enabled = false;
        }

        private Rectangle sizeGripRectangle;

        private void BtnEfX_Click(object sender, EventArgs e)
        {
            if(PnlEFX.Visible == false)
            {
                PnlEFX.Visible = true;
            }
            else
            {
                PnlEFX.Visible = false;
            }
        }

        private void BtnFXPeB_Click(object sender, EventArgs e)
        {
            PnlC.Visible = false;
            tt = 20;
            Carregamento.Enabled = true;

            bb = new Bitmap(i);

            int width = bb.Width;
            int height = bb.Height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color p = bb.GetPixel(x, y);

                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    r = 255 - r;
                    g = 255 - g;
                    b = 255 - b;

                    bb.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            Img_EFX.Image = bb;
        }

        private void BtnEspelho_Click(object sender, EventArgs e)
        {
            PnlC.Visible = false;
            tt = 60;
            Carregamento.Enabled = true;

            Bitmap mimg;
            mimg = new Bitmap(i);

            int width = mimg.Width;
            int height = mimg.Height;

            bb = new Bitmap(width * 2, height);

            for (int y = 0; y < height; y++)
            {
                for (int lx = 0, rx = width * 2 - 1; lx < width; lx++, rx--)
                {
                    Color p = mimg.GetPixel(lx, y);

                    bb.SetPixel(lx, y, p);
                    bb.SetPixel(rx, y, p);
                }
            }
            Img_EFX.Image = bb;
        }

        private void BtnSepia_Click(object sender, EventArgs e)
        {
            PnlC.Visible = false;
            tt = 40;
            Carregamento.Enabled = true;

            bb = new Bitmap(i);

            int width = bb.Width;
            int height = bb.Height;

            Color p;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    p = bb.GetPixel(x, y);

                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                    int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                    int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);

                    if (tr > 255)
                    {
                        r = 255;
                    }
                    else
                    {
                        r = tr;
                    }

                    if (tg > 255)
                    {
                        g = 255;
                    }
                    else
                    {
                        g = tg;
                    }

                    if (tb > 255)
                    {
                        b = 255;
                    }
                    else
                    {
                        b = tb;
                    }

                    bb.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            Img_EFX.Image = bb;
        }

        void OOOO()
        {
            if(BtnC2.Enabled == true)
            {
                PnlTimelineOP.BackColor = Color.Black;
                PnlTimeLine.BackColor = Color.Black;
                BtnO.ForeColor = Color.Red;
                PreverFiltro.Image = null;
                BtnC2.Enabled =  false;
                BtnGO2.Enabled = false;
                BtnA2.Enabled =  false;
                Barra2.Enabled = false;
            }
            else
            {
                PnlTimelineOP.BackColor = Color.FromArgb(42, 42, 42);
                PnlTimeLine.BackColor = Color.FromArgb(64, 64, 64);
                BtnO.ForeColor = Color.White;
                PreverFiltro.Image = pictureBox1.Image;
                BtnC2.Enabled =  true;
                BtnGO2.Enabled = true;
                BtnA2.Enabled =  true;
                Barra2.Enabled = true;
            }
        }

        private void BtnRGB_Click(object sender, EventArgs e)
        {
            BtnRGB.Visible = false;

            bb = new Bitmap(i);

            int width = bb.Width;
            int height = bb.Height;

            Bitmap rbmp = new Bitmap(bb);
            Bitmap gbmp = new Bitmap(bb);
            Bitmap bbmp = new Bitmap(bb);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color p = bb.GetPixel(x, y);

                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    rbmp.SetPixel(x, y, Color.FromArgb(r, 0, 0));

                    gbmp.SetPixel(x, y, Color.FromArgb(0, g, 0));

                    bbmp.SetPixel(x, y, Color.FromArgb(0, 0, b));

                }
            }

            ImgRR.Visible = true;
            ImgGG.Visible = true;
            ImgBB.Visible = true;

            ImgRR.Image = rbmp;
            ImgGG.Image = gbmp;
            ImgBB.Image = bbmp;
        }

        void Chuvisco()
        {
            PnlC.Visible = false;
            if (EFXTempo.Enabled == false)
            {
                EFXTempo.Enabled = true;
            }
            else
            {
                EFXTempo.Enabled = false;
                Img_EFX.Image = null;
            }
        }

        private void BtnAplicarFX_Click(object sender, EventArgs e)
        {
            Salvarap.Visible = true;
            PnlC.Visible = false;
            IMG = bb;
            i = IMG;
            PreverFiltro.BackgroundImage = IMG;
            Img_EFX.Image = null;
            PreverFX.Image = null;
        }

        private void Verificador_Tick(object sender, EventArgs e)
        {
            img11.Image = PreverFiltro.BackgroundImage;
            img22.Image = PreverFiltro.BackgroundImage;
            img33.Image = PreverFiltro.BackgroundImage;

            if (Img_EFX.Image != null || PreverFX.Image != null)
            {
                BtnAplicarFX.Enabled = true;
            }
            else
            {
                BtnAplicarFX.Enabled = false;
            }
        }

        private void EFXTempo_Tick(object sender, EventArgs e)
        {
            int width = 40, height = 20;
            Bitmap cccc;
            cccc = new Bitmap(width, height);

            Random rand = new Random();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int r = rand.Next(256);
                    int g = rand.Next(256);
                    int b = rand.Next(256);

                    int avg = (r + g + b) / 8;

                    cccc.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                }
            }
            Img_EFX.Image = cccc;
        }

        private void PnlTempo_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void PnlTempo_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var data = e.Data.GetData(DataFormats.FileDrop);
                if (data != null)
                {
                    var filenames = data as string[];
                    if (filenames.Length > 0)
                    {
                        PreverFiltro.Image = Image.FromFile(filenames[0]);
                        Timg();
                    }
                }
            }
            catch (Exception ex)
            {
                Chuvisco();
                Frm_MSG MSG = new Frm_MSG("Desculpe", string.Format("Não deu para carregar o arquivo...\nTente apenas arquivos de imagem por favor...\n\n\nErro {0}", ex.Message), false, "", Properties.Resources.questão);
                if (MSG.ShowDialog()== DialogResult.OK)
                {
                    Chuvisco();
                }
                else
                {
                    Chuvisco();
                }
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (Salvarap.Visible == true)
            {
                SaveFileDialog SalvarCoisas = new SaveFileDialog();
                SalvarCoisas.Filter = "Imagem Boa |*.png|Imagem |*.ico|Imagem Animada |*.gif|Imagem Comum |*.jpeg|Arquivos Nasca |*.Nasca";

                SalvarCoisas.FileName = nomi;

                if (SalvarCoisas.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (SalvarCoisas.FileName.Contains(".png"))
                        {
                            IMG.Save(SalvarCoisas.FileName, ImageFormat.Png);
                        }
                        else if (SalvarCoisas.FileName.Contains(".ico"))
                        {
                            IMG.Save(SalvarCoisas.FileName, ImageFormat.Icon);
                        }
                        else if (SalvarCoisas.FileName.Contains(".jpeg"))
                        {
                            IMG.Save(SalvarCoisas.FileName, ImageFormat.Jpeg);
                        }
                        else if (SalvarCoisas.FileName.Contains(".gif"))
                        {
                            IMG.Save(SalvarCoisas.FileName, ImageFormat.Gif);
                        }
                        else if (SalvarCoisas.FileName.Contains(".Nasca"))
                        {
                            IMG.Save(SalvarCoisas.FileName);
                        }
                        Salvarap.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        Frm_MSG MSG = new Frm_MSG("Desculpe", string.Format("Não deu para salvar por algum erro externo... Erro {0}", ex.Message), false, "", Properties.Resources.questão);
                        MSG.ShowDialog();
                    }
                }
            }
            else
            {
                Frm_MSG MSG = new Frm_MSG("Salvar", "Não tem nada para\nsalvar no momento", false, "", Properties.Resources.info);
                MSG.ShowDialog();
            }
        }
    }
}
