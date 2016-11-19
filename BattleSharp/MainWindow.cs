using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BattleSharp
{
    public partial class MainWindow : Form
    {
        readonly UnityBootstrapper.Interface bootstrap  = new UnityBootstrapper.Interface();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void injectButton_Click(object sender, EventArgs e)
        {
            UnityBootstrapper.Interface.Inject(Process.GetProcessesByName("Battlerite")[0].Id, bootstrap);
        }
    }
}
