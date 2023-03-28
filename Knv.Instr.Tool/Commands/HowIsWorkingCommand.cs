
namespace Knv.Instr.Tool.Commands
{
    using System;
    using System.Windows.Forms;


    class HowIsWorkingCommand : ToolStripButton
    {
        readonly IMainForm _mainForm;

        public HowIsWorkingCommand()
        {
            //    Image = Resources.Delete32x32;
            DisplayStyle = ToolStripItemDisplayStyle.Text;
            //    Size = new System.Drawing.Size(50, 50);
            Text = "How is Working?";

            //   _diagnosticsView = diagnosticsView;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            //var hiw = new Tool.View.HowIsWorkingForm();
            //hiw.ShowDialog();
        }
    }
}
