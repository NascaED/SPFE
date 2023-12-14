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
    public partial class Frm_Principal : Form
    {
        Frm_Novo_Projeto novinho = new Frm_Novo_Projeto();
        Frm_MSG mms = new Frm_MSG("Abrir os filtros?", "Filtros é uma super ferramenta\npor isso ela é separada do modo principal\n\nSe você abrir o Filtro agora não será mais possivel continuar o desenho do seu projeto!", true, "Conhecer melhor!", Properties.Resources.info);
        

        int lx, ly, sw, sh, AA, RR, GG, BB, Efeito, Tamanho = 2;

        Point Atual = new Point(), Antigo = new Point();

        string Nome;

        OpenFileDialog AbrirCoisas = new OpenFileDialog();

        Bitmap Coselecionada, IMG;
        Graphics Graf, G;
        Pen P;

        public Frm_Principal()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
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
            if(Salvarap.Visible == true)
            {
                Frm_MSG MSG = new Frm_MSG("Salvar??", "Se você sair agora sua\nimagem não sera salva!", true, "Salvar Agora!", Properties.Resources.questão);
                if (MSG.ShowDialog() == DialogResult.OK)
                {
                    Application.Exit();
                }
                else if(MSG.DialogResult == DialogResult.Abort)
                {
                    BtnSalvar_Click(sender, e);
                }
            }
            else
            {
                Application.Exit();
            }
        }                                                                                          
                                                                                                   
        private void BarradeTitulo_MouseDown(object sender, MouseEventArgs e)                      
        {                                                                                   
            ReleaseCapture();                                                                      
            SendMessage(this.Handle, 0x112, 0xf012, 0);                                            
        }                                                                                          
                                                                                                   
        protected override void OnPaint(PaintEventArgs e)                                          
        {                                                                                          
            GraphicsPath forma0 = new GraphicsPath();                                              
            GraphicsPath forma1 = new GraphicsPath();                                              
            GraphicsPath forma2 = new GraphicsPath();                                              
            GraphicsPath forma3 = new GraphicsPath();
            GraphicsPath forma4 = new GraphicsPath();
            forma0.AddEllipse(0, 0, BtnFechar.Width, BtnFechar.Height);                                               
            forma1.AddEllipse(0, 0, BtnQuadricular.Width, BtnQuadricular.Height);                  
            forma2.AddEllipse(0, 0, BtnMaximizar.Width, BtnMaximizar.Height);                                         
            forma3.AddEllipse(0, 0, BtnMinimizar.Width, BtnMinimizar.Height);
            forma4.AddEllipse(0, 0, Img3DT.Width, Img3DT.Height);
            BtnFechar.Region = new Region(forma0);                                                                         
            BtnQuadricular.Region = new Region(forma1);                                            
            BtnMaximizar.Region = new Region(forma2);                                                                                         //
            BtnMinimizar.Region = new Region(forma3);
            Img3DT.Region = new Region(forma4);
        }                                                                                              
                                                                                                       
        private int tolerance = 12;                                                                    
        private const int WM_NCHITTEST = 132;                                                          
        private const int HTBOTTOMRIGHT = 17;                                                          
        private Rectangle sizeGripRectangle;                                                           
        protected override void WndProc(ref Message m)                                                 
        {                                                                                              
            switch (m.Msg)                                                                             
            {                                                                                          
                case WM_NCHITTEST:                                                                     
                    base.WndProc(ref m);                                                               
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))                                           
                        m.Result = new IntPtr(HTBOTTOMRIGHT);                                           
                    break;                                                                              
                default:                                                                                
                    base.WndProc(ref m);                                                                
                    break;                                                                              
            }                                                                                           
        }

        private void Verificar_Tick(object sender, EventArgs e)
        {
            Graf = Canvas.CreateGraphics();
            P = new Pen(CorAtual.BackColor, Tamanho);
            if (Efeito == 0)
            {
                P.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Round);
            }
            else if (Efeito == 1)
            {
                P.SetLineCap(LineCap.Square, LineCap.Flat, DashCap.Round);
            }
            else if (Efeito == 2)
            {
                P.SetLineCap(LineCap.Triangle, LineCap.Round, DashCap.Triangle);
            }
            else if (Efeito == 3)
            {
                P.SetLineCap(LineCap.ArrowAnchor, LineCap.Round, DashCap.Flat);
            }
            else if (Efeito == 4)
            {
                P.SetLineCap(LineCap.SquareAnchor, LineCap.Custom, DashCap.Round);
            }

            pictureBox2.Width = Tamanho;
            pictureBox2.Height = Tamanho;

            GraphicsPath xx = new GraphicsPath();
            xx.AddEllipse(0, 0, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Region = new Region(xx);

            AA = HSA.Value;
            RR = HSR.Value;
            GG = HSG.Value;
            BB = HSB.Value;
            HSR.BackColor = Color.FromArgb(RR, 0, 0);
            HSG.BackColor = Color.FromArgb(0, GG, 0);
            HSB.BackColor = Color.FromArgb(0, 0, BB);
            CorAtual.BackColor = Color.FromArgb(AA, RR, GG, BB);
            PreverCor3D.BackColor = Color.FromArgb(AA, RR, GG, BB);
            label1.Text = RR.ToString() +";"+ GG.ToString() + ";" + BB.ToString();
            Img3DT.BackgroundImage = prever3d.Image;
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
        
        private void BtnFerramentas_Click(object sender, EventArgs e)
        {
            if(PNLFerramentasOP.Visible == false)
            {
                PNLFerramentasOP.BringToFront();
                PNLFerramentasOP.Visible = true;
            }
            else
            {
                PNLFerramentasOP.Visible = false;
            }
        }

        private void BtnTiposDeCores_Click(object sender, EventArgs e)
        {
            if(PnlTipos.Visible == false)
            {
                PnlTipos.Visible = true;
                PnlTipos.BringToFront();
            }
            else
            {
                PnlTipos.Visible = false;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            PnlTipos.Visible = false;
            Pnl3DT.Visible = false;
            Cores.Image = Properties.Resources.Cor_redondo;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            PnlTipos.Visible = false;
            Pnl3DT.Visible = false;
            Cores.Image = Properties.Resources.Circuloquadrado;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            PnlTipos.Visible = false;
            Pnl3DT.Visible = false;
            Cores.Image = Properties.Resources.Todas_as_cores_1;
        }

        private void Btn3DT_Click(object sender, EventArgs e)
        {
            Img3DT.BackColor = CorAtual.BackColor;

            if (Pnl3DT.Visible == false)
            {
                Pnl3DT.Visible = true;
                Pnl3DT.BringToFront();
                PnlTipos.Visible = false;
            }
            else
            {
                Pnl3DT.Visible = false;
            }
        }

        private void BtnAbrirTexturas_Click(object sender, EventArgs e)
        {
            AbrirCoisas.Filter = "Imagem Boa (*.png)|*.png|Imagem (*.jpg)|*.jpg|Imagem (*.jpeg)|*.jpeg|Imagem Animada (*.gif)|*.gif|Arquivos (*.Nasca)|*.Nasca";
            if (AbrirCoisas.ShowDialog() == DialogResult.OK)
            {
                prever3d.ImageLocation = AbrirCoisas.FileName;
            }
        }

        private void BtnPinceis_Click(object sender, EventArgs e)
        {
            if(PnlPincels.Visible == false)
            {
                PnlPincels.Visible = true;
                PnlPincels.BringToFront();
            }
            else
            {
                PnlPincels.Visible = false;
            }
        }

        private void TamanhoDoPem_Scroll(object sender, EventArgs e)
        {
            Tamanho = TamanhoDoPem.Value;
            LblVerNumero.Text = TamanhoDoPem.Value.ToString();
        }

        private void Img3DT_MouseUp(object sender, MouseEventArgs e)
        {
            VerCor.Enabled = false;
            corantiga3D.BackColor = Color.FromArgb(AA, RR, GG, BB);
        }

        private void BtnVer_Click(object sender, EventArgs e)
        {
            Img3DT.BackColor = Color.Red;
        }

        private void BtnVerde_Click(object sender, EventArgs e)
        {
            Img3DT.BackColor = Color.Lime;
        }

        private void BtnAz_Click(object sender, EventArgs e)
        {
            Img3DT.BackColor = Color.Blue;
        }

        private void BtnOKk_Click(object sender, EventArgs e)
        {
            PnlPincels.Visible = false;
        }

        private void LinhaReta_Click(object sender, EventArgs e)
        {
            Efeito = 0;
            lblEfeitoAtual.Text = "Efeito atual: Linha Comum";
        }

        private void BtnLinhasRetas_Click(object sender, EventArgs e)
        {
            Efeito = 1;
            lblEfeitoAtual.Text = "Efeito atual: Linhas Ligadas";
        }

        private void Btnefx_Click(object sender, EventArgs e)
        {
            Efeito = 3;
            lblEfeitoAtual.Text = "Efeito atual: Cubos de Linhas";
        }

        private void BtneFx2_Click(object sender, EventArgs e)
        {
            Efeito = 4;
            lblEfeitoAtual.Text = "Efeito atual: The Linhas";
        }

        private void BtnFechar_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Fechar App";
        }

        private void BtnQuadricular_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Quadricular App";
        }

        private void BtnMinimizar_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Minimizar App";
        }

        private void BtnNovo_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Criar novo projeto";
        }

        private void PnlMenuTop_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Barra de ferramentas";
        }

        private void PnlBarradetitulo_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Barra de titulo";
        }

        private void LblNomeP_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Nome e propriedades do projeto";
        }

        private void BtnFerramentas_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Ferramentas do App";
        }

        private void BtnPinceis_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Configurações dos pinceis";
        }

        private void CapturaDeTela_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Fundo do canvas";
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Código da cor atual";
        }

        private void BtnAbrirTexturas_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Abrir imagem para a textura na bola 3D";
        }

        private void BtnTiposDeCores_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Tipos de cores selecionadas";
        }

        private void BtnSalvar_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Botão para salvar coisas";
        }

        private void Salvarap_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Indicador de quando salvar";
        }

        private void label6_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Escolher cor no tipo de circulo";
        }

        private void label7_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Um circulo com quadrados retangulos dentro dele";
        }

        private void Btn3DT_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Textura 3D";
        }

        private void PnlFerramentas_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Barra lateral de opções de ferramentas";
        }

        private void PnlDock_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Dock de ferramentas";
        }

        private void PnlStatus_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Barra de informações legais";
        }

        private void Pnl3DT_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Modo 3D";
        }

        private void BtnMaximizar_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Maximizar App";
        }

        private void PnlJanela_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Janela";
        }

        private void PnlPincels_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Ferramentas do pincel";
        }

        private void Limparimg3D_Click(object sender, EventArgs e)
        {
            prever3d.Image = null;
        }

        private void Limparimg3D_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Remover a imagem da bola 3D";
        }

        private void lblprever_Click(object sender, EventArgs e)
        {
            PNLFerramentasOP.Visible = false;
            if(PnlPreverCanvas.Visible == false)
            {
                PnlPreverCanvas.Visible = true;
                PnlPreverCanvas.BringToFront();
                lblprever.Text = "Ocultar Prever          *";
            }
            else
            {
                PnlPreverCanvas.Visible = false;
                lblprever.Text = "Mostrar Prever";
            }
        }

        private void BtnOKprever_Click(object sender, EventArgs e)
        {
            PnlPreverCanvas.Visible = false;
            lblprever.Text = "Mostrar Prever";
        }

        private void Img3DT_MouseDown(object sender, MouseEventArgs e)
        {
            VerCor.Enabled = true;
            CorAntigona.BackColor = corantiga3D.BackColor;
        }

        private void lblAtivarFiltros_Click(object sender, EventArgs e)
        {
            PNLFerramentasOP.Visible = false;
            if (BtnFiltros.Visible == false)
            {
                BtnFiltros.Visible = true;
                BtnFiltros.BringToFront();
                lblAtivarFiltros.Text = "Desativar Filtros          *";
            }
            else
            {
                BtnFiltros.Visible = false;
                lblAtivarFiltros.Text = "Ativar Filtros";
            }
        }

        private void BtnFiltros_Click(object sender, EventArgs e)
        {
            if(mms.ShowDialog() == DialogResult.OK)
            {
                FRM_Filtros ff = new FRM_Filtros(Nome, novinho.Propriedades, IMG);
                ff.Show();
                this.Hide();
            }
            else if(mms.DialogResult == DialogResult.Abort)
            {
                
            }
        }

        private void Img3DT_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                var filenames = data as string[];
                if (filenames.Length > 1)
                {
                    Img3DT.Image = Image.FromFile(filenames[0]);
                }
            }
        }

        private void Img3DT_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void BtnLinhasREtasEF_Click(object sender, EventArgs e)
        {
            Efeito = 2;
            lblEfeitoAtual.Text = "Efeito atual: Tiag";
        }

        private void VerCor_Tick(object sender, EventArgs e)
        {
            Coselecionada = new Bitmap(1, 1);
            Graf = Graphics.FromImage(Coselecionada);
            Graf.CopyFromScreen(new Point(MousePosition.X, MousePosition.Y), new Point(0, 0), Coselecionada.Size);
            CorAtual.BackColor = Coselecionada.GetPixel(0, 0);
            HSA.Value = CorAtual.BackColor.A;
            HSR.Value = CorAtual.BackColor.R;
            HSG.Value = CorAtual.BackColor.G;
            HSB.Value = CorAtual.BackColor.B;
        }

        private void Cores_MouseDown(object sender, MouseEventArgs e)
        {
            VerCor.Enabled = true;
        }

        private void Cores_MouseUp(object sender, MouseEventArgs e)
        {
            VerCor.Enabled = false;
        }

        private void Criar()
        {
            if (novinho.ShowDialog() == DialogResult.OK)
            {
                Canvas.Refresh();
                Canvas.Image = null;
                Canvas.ImageLocation = "";
                PnlFerramentas.Enabled = novinho.AtivarTudo;
                PnlCapituradeTela.Visible = novinho.AtivarTudo;
                PnlStatus.Enabled = novinho.AtivarTudo;
                PnlDock.Enabled = novinho.AtivarTudo;
                LblNomeP.Text = novinho.Propriedades;
                Nome = novinho.Nomep;
                Canvas.Size = new Size(novinho.NX, novinho.NY);
                Canvas.Image = novinho.Imm;
                Canvas.ImageLocation = novinho.ImgL;
                Canvas.BackColor = novinho.Cb;
                LblTitulo.Text = "Show Photo Foto Editor (" + novinho.Nomep + ").";
            }
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            if (Salvarap.Visible == true)
            {
                Frm_MSG MSG = new Frm_MSG("Salvar??", "Se você criar um novo projeto agora\na imagem não sera salva!", true, "Salvar agora!", Properties.Resources.questão);
                if (MSG.ShowDialog() == DialogResult.OK)
                {
                    Criar();
                }
                else if (MSG.DialogResult == DialogResult.Abort)
                {
                    BtnSalvar_Click(sender, e);
                }
            }
            else
            {
                Criar();
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        #region Canvas
        private void Desenhando_Tick(object sender, EventArgs e)
        {
            lblAtivarFiltros.Enabled = true;

            if (Canvas.Size.Width >= PnlCapituradeTela.Size.Width && Canvas.Size.Height >= PnlCapituradeTela.Size.Height)
            {
                IMG = new Bitmap(PnlCapituradeTela.Size.Width, PnlCapituradeTela.Size.Height);
                G = Graphics.FromImage(IMG);
                Rectangle Retangulo = PnlCapituradeTela.RectangleToScreen(PnlCapituradeTela.ClientRectangle);
                G.CopyFromScreen(Retangulo.Location, Point.Empty, PnlCapituradeTela.Size);
                IMGPrever.Image = IMG;
                G.Dispose();
            }
            else if (Canvas.Size.Width <= PnlCapituradeTela.Size.Width && Canvas.Size.Height >= PnlCapituradeTela.Size.Height)
            {
                IMG = new Bitmap(Canvas.Size.Width, PnlCapituradeTela.Size.Height);
                G = Graphics.FromImage(IMG);
                Rectangle Retangulo = PnlCapituradeTela.RectangleToScreen(PnlCapituradeTela.ClientRectangle);
                G.CopyFromScreen(Retangulo.Location, Point.Empty, PnlCapituradeTela.Size);
                IMGPrever.Image = IMG;
                G.Dispose();
            }
            else if (Canvas.Size.Width <= PnlCapituradeTela.Size.Width && Canvas.Size.Height <= PnlCapituradeTela.Size.Height)
            {
                IMG = new Bitmap(Canvas.Size.Width, Canvas.Size.Height);
                G = Graphics.FromImage(IMG);
                Rectangle Retangulo = Canvas.RectangleToScreen(Canvas.ClientRectangle);
                G.CopyFromScreen(Retangulo.Location, Point.Empty, Canvas.Size);
                IMGPrever.Image = IMG;
                G.Dispose();
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            Canvas.Image = IMG;
            Desenhando.Enabled = false;
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            PNLFerramentasOP.Visible = false;
            Canvas.BringToFront();
            Desenhando.Enabled = true;
            Salvarap.Visible = true;
            Antigo = e.Location;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            LblStatus.Text = "Canvas";
            LblMousePosisao.Text = e.Location.X.ToString() + " X " + e.Location.Y.ToString();
          
            if (e.Button == MouseButtons.Left)
            {
                Atual = e.Location;
                Graf.DrawLine(P, Antigo, Atual);
                Antigo = Atual;
            }
        }
        #endregion

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if(Salvarap.Visible == true)
            {
                SaveFileDialog SalvarCoisas = new SaveFileDialog();
                SalvarCoisas.Filter = "Imagem Boa |*.png|Imagem |*.ico|Imagem Animada |*.gif|Imagem Comum |*.jpeg|Arquivos Nasca |*.Nasca";
                if (Canvas.Size.Width >= PnlCapituradeTela.Size.Width && Canvas.Size.Height >= PnlCapituradeTela.Size.Height)
                {
                    IMG = new Bitmap(PnlCapituradeTela.Size.Width, PnlCapituradeTela.Size.Height);
                    Graf = Graphics.FromImage(IMG);
                    Rectangle Retangulo = PnlCapituradeTela.RectangleToScreen(PnlCapituradeTela.ClientRectangle);
                    Graf.CopyFromScreen(Retangulo.Location, Point.Empty, PnlCapituradeTela.Size);
                    Graf.Dispose();
                }
                else if (Canvas.Size.Width <= PnlCapituradeTela.Size.Width && Canvas.Size.Height >= PnlCapituradeTela.Size.Height)
                {
                    IMG = new Bitmap(Canvas.Size.Width, PnlCapituradeTela.Size.Height);
                    Graf = Graphics.FromImage(IMG);
                    Rectangle Retangulo = PnlCapituradeTela.RectangleToScreen(PnlCapituradeTela.ClientRectangle);
                    Graf.CopyFromScreen(Retangulo.Location, Point.Empty, PnlCapituradeTela.Size);
                    Graf.Dispose();
                }
                else if (Canvas.Size.Width <= PnlCapituradeTela.Size.Width && Canvas.Size.Height <= PnlCapituradeTela.Size.Height)
                {
                    IMG = new Bitmap(Canvas.Size.Width, Canvas.Size.Height);
                    Graf = Graphics.FromImage(IMG);
                    Rectangle Retangulo = Canvas.RectangleToScreen(Canvas.ClientRectangle);
                    Graf.CopyFromScreen(Retangulo.Location, Point.Empty, Canvas.Size);
                    Graf.Dispose();
                }

                SalvarCoisas.FileName = Nome;

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
                        Frm_MSG MSG = new Frm_MSG("Desculpe", string.Format("Não deu para salvar por algum erro externo... Erro {0}", ex.Message), false, "" , Properties.Resources.questão);
                        MSG.ShowDialog();
                    }
                }
            }
            else
            {
                Frm_MSG MSG = new Frm_MSG("Salvar", "Não tem nada para\nsalvar no momento", false, "" , Properties.Resources.info);
                MSG.ShowDialog();
            }
        }
    }
}