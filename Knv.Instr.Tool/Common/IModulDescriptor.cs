
namespace Knv.Instr.Tool.Common
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    public interface IModul
    {
        string Name { get; }
        string IconName { get; }
        string ToolTip { get; }
        UserControl Page{ get; }
        TreeNode TreeNode { get; }
    }
}
