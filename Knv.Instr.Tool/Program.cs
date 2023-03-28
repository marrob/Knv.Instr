namespace Knv.Instr.Tool
{ 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using System.Threading;
    using System.Diagnostics;
    using Properties;
    using Common;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new App();
        }
    }

    public interface IApp
    {

    }

    class App : IApp
    {
        IMainForm MainForm = new MainForm();
        List<IModul> Moduls = new List<IModul>();
        SynchronizationContext SyncContext;

        public App()
        {
            if(Settings.Default.ApplictionSettingsSaveCounter == 0)
            {
                Settings.Default.Upgrade();
                Settings.Default.ApplictionSettingsUpgradeCounter++;
            }
            Settings.Default.ApplictionSettingsSaveCounter++;
            Settings.Default.PropertyChanged += (s, e) =>
            {
                Debug.WriteLine(GetType().Namespace + "." + GetType().Name + "." +
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "(): " +
                    e.PropertyName + ", NewValue: " + Settings.Default[e.PropertyName]);
            };

            Settings.Default.SettingsLoaded += (s, e) =>
            {
                 Debug.WriteLine("SettingsLoaded");
            };

            Settings.Default.SettingChanging += (s, e) =>
            {
                Debug.WriteLine(GetType().Namespace + "." + GetType().Name + "." +
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()");
            };

            MainForm.Version = Application.ProductVersion;
            MainForm.Text = AppConstants.SoftwareTitle + " - " + Application.ProductVersion;
            MainForm.Shown += (o, e) =>
            {
                SyncContext = SynchronizationContext.Current;
                var lastUsedModul = Moduls.FirstOrDefault(n => n.Name == Settings.Default.LastUsedPageName);
                if (lastUsedModul != null)
                {
                    MainForm.Tree.SelectedNode = lastUsedModul.TreeNode;
                    MainForm.Page = lastUsedModul.Page;
                }

                MainForm.LayoutRestore();
            };
            MainForm.FormClosed += (s, e) =>
            {
                MainForm.LayoutSave();
                Settings.Default.Save();
            };

            MainForm.Tree.AfterSelect += (s, e) =>
            {
                var currentModul = (IModul)e.Node.Tag;
                MainForm.Page = currentModul.Page;
                if (e.Action != TreeViewAction.Unknown)
                    Settings.Default.LastUsedPageName = currentModul.Name;
            };

            MainForm.DebugButton.Click += (s, e) =>
            {
                MainForm.Tree.Select();
                MainForm.Tree.SelectedNode = Moduls[2].TreeNode;
            };




            var helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDown.Items.AddRange(
                 new ToolStripItem[]
                 {
                     new Commands.HowIsWorkingCommand(),
                     new Commands.UpdatesCommands(),
                 });

            MainForm.MenuBar = new ToolStripItem[]
                {
                    helpMenu,
                };


            foreach (var modul in Moduls)
                MainForm.Tree.Nodes.Add(modul.TreeNode);
            





            Application.Run((MainForm)MainForm);
        }
    }
}
