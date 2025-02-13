using ScottPlot;

namespace almacen_lunare
{
    public partial class menu : Form
    {
        public menu()
        {

            InitializeComponent();
            Createplot();
        }
        bool Isopen = false;
        private void formsPlot1_Load(object sender, EventArgs e)
        {
           
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            
        }

       

        public void Createplot()
        {
            double[] xs1 = { 1, 2, 3, 4 };
            double[] ys1 = { 5, 10, 7, 13 };
            var bars1 = formsPlot1.Plot.Add.Bars(xs1, ys1);
            bars1.LegendText = "Alpha";

            double[] xs2 = { 6, 7, 8, 9 };
            double[] ys2 = { 7, 12, 9, 15 };
            var bars2 = formsPlot1.Plot.Add.Bars(xs2, ys2);
            bars2.LegendText = "Beta";

            formsPlot1.Plot.ShowLegend(Alignment.UpperLeft);
            formsPlot1.Plot.Axes.Margins(bottom: 0);

            formsPlot1.Refresh();
        }

        private void extendmenu_Click(object sender, EventArgs e)
        {

            if (!Isopen)
            {
                Isopen = true;
                panel6.Enabled = true;
                panel6.Visible = true;
            }
            else
            {
                panel6.Enabled = false;
                panel6.Visible = false;
                Isopen = false;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
