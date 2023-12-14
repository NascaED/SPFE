using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHOW_PHOTO_FOTO_EDITOR
{
    public partial class Frm_Inicio : Form
    {
        int BarraX, BarraY, BarraTotal;

        double BarraValor;

        Graphics g;
        Bitmap IMG;

        public Frm_Inicio()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            BarraX = BarraDeDados.Width;
            BarraY = BarraDeDados.Height;
            BarraValor = BarraX / 100.0;
            BarraTotal = 0;
            IMG = new Bitmap(BarraX, BarraY);
        }

        private void Velocidade_Tick(object sender, EventArgs e)
        {
            g = Graphics.FromImage(IMG);

            g.Clear(Color.Purple);

            g.FillRectangle(Brushes.DeepPink, new Rectangle(0, 0, (int)(BarraTotal * BarraValor), BarraY));

            g.DrawString(BarraTotal + "%", new Font("Arial", BarraY - 7), Brushes.White, new PointF(BarraX - 40, BarraY / 10));

            BarraDeDados.Image = IMG;

            BarraTotal++;
            if (BarraTotal > 100)
            {
                g.Dispose();
                Velocidade.Enabled = false;
                Frm_Principal pp = new Frm_Principal();
                pp.Show();
                this.Hide();
            }
        }
    }
}
