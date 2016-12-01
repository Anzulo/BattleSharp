using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BattleSharp
{
    public partial class MainWindow : Form
    {
        readonly UnityBootstrapper.Interface bootstrap = new UnityBootstrapper.Interface();

        static string steamPath = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam").GetValue("SteamPath").ToString();
        static string gamePath = steamPath + @"\steamapps\common\Battlerite\Battlerite_Data\Managed\";
        static string exePath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(MainWindow)).Location);

        public MainWindow()
        {
            InitializeComponent();
            UnityBootstrapper.Interface.Inject(Process.GetProcessesByName("Battlerite")[0].Id, bootstrap, exePath + @"\" + "BattleSharpController" + ".dll", "BattleSharpControllerGenericNamespace" + ".Loader:Load()");
        }

        private void InjectButtonClick(object sender, EventArgs e)
        {
            UnityBootstrapper.Interface.Inject(Process.GetProcessesByName("Battlerite")[0].Id, bootstrap, exePath + @"\" + "BattleSharpController" + ".dll", "BattleSharpControllerGenericNamespace" + ".Loader:Load()");
        }
    }
}