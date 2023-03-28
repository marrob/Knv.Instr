
namespace Knv.Instr.Tool.Commands
{
    using System;
    using System.Windows.Forms;

    class UpdatesCommands : ToolStripButton
    {
        public UpdatesCommands()
        {
            //    Image = Resources.Delete32x32;
            DisplayStyle = ToolStripItemDisplayStyle.Text;
            //    Size = new System.Drawing.Size(50, 50);
            Text = "Updates";
            //   _diagnosticsView = diagnosticsView;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            //var hiw = new MJBL180509.Tool.UpdatesForm();
            //hiw.ShowDialog();
        }
    }
}
