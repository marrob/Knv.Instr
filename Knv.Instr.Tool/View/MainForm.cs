
namespace Knv.Instr.Tool
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using Properties;


    public interface IMainForm
    {
        event FormClosedEventHandler FormClosed;
        event FormClosingEventHandler FormClosing;
        event EventHandler Disposed;
        event EventHandler Shown;
        event KeyEventHandler KeyUp;
        ToolStripItem[] MenuBar { set; }
        TreeView Tree { get;  }
        UserControl Page { set; }
        ToolStripMenuItem DebugButton { get; }
        void CursorWait();
        void CursorDefault();
        string Text { get; set; }
        string Status { get; set; }
        string Version { get; set; }
        void LayoutSave();
        void LayoutRestore();
    }

    public partial class MainForm : Form, IMainForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public ToolStripItem[] MenuBar
        {
            set { menuStrip1.Items.AddRange(value); }
        }

        public UserControl Page
        {
            set
            {
                if (!splitContainer1.Panel2.Controls.Contains(value))
                {
                    splitContainer1.Panel2.Controls.Clear();
                    splitContainer1.Panel2.Controls.Add(value);
                }
            }
        }

        public TreeView Tree { get  { return treeView1;} }

        public string Status
        {
            get { return toolStripStatusLabel1.Text;  }
            set { toolStripStatusLabel1.Text = value; }
        }

        public string Version
        {
            get { return toolStripStatusLabelVersion.Text; }
            set { toolStripStatusLabelVersion.Text = value; }
        }

        public void CursorWait()
        {
            Cursor.Current = Cursors.WaitCursor;
        }

        public void CursorDefault()
        {
            Cursor.Current = Cursors.Default;
        }

        public void LayoutSave()
        {
            Settings.Default.MainFormLocation = Location;
            Settings.Default.MainFormWindowState = WindowState;
            Settings.Default.MainFormSize = Size;
            Settings.Default.MainFormSplitter = splitContainer1.SplitterDistance;
        }

        public void LayoutRestore()
        {
            Location = Settings.Default.MainFormLocation;
            WindowState = Settings.Default.MainFormWindowState;
            Size = Settings.Default.MainFormSize;
            splitContainer1.SplitterDistance = Settings.Default.MainFormSplitter;
        }

        public ToolStripMenuItem DebugButton { get { return debugButtonToolStripMenuItem; } }

    }
}
