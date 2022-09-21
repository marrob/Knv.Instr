using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NationalInstruments.Examples.FindResources
{
	/// <summary>
	/// CustomFilterForm lets the user enter the custom filter string to
	/// find the available resources with. Public property CustomFilter 
	/// returns the custom filter string.
	/// </summary>
	public class CustomFilterForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label customFilterLabel;
        private System.Windows.Forms.TextBox customFilterTextBox;
        private System.Windows.Forms.Button okButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CustomFilterForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CustomFilterForm));
            this.customFilterTextBox = new System.Windows.Forms.TextBox();
            this.customFilterLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // customFilterTextBox
            // 
            this.customFilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.customFilterTextBox.Location = new System.Drawing.Point(16, 24);
            this.customFilterTextBox.Name = "customFilterTextBox";
            this.customFilterTextBox.Size = new System.Drawing.Size(152, 20);
            this.customFilterTextBox.TabIndex = 0;
            this.customFilterTextBox.Text = "?*";
            // 
            // customFilterLabel
            // 
            this.customFilterLabel.Location = new System.Drawing.Point(16, 8);
            this.customFilterLabel.Name = "customFilterLabel";
            this.customFilterLabel.Size = new System.Drawing.Size(144, 16);
            this.customFilterLabel.TabIndex = 1;
            this.customFilterLabel.Text = "Enter Custom Filter String:";
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.Location = new System.Drawing.Point(56, 56);
            this.okButton.Name = "okButton";
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // CustomFilterForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(184, 78);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.customFilterLabel);
            this.Controls.Add(this.customFilterTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(384, 112);
            this.MinimumSize = new System.Drawing.Size(192, 112);
            this.Name = "CustomFilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Filter";
            this.ResumeLayout(false);

        }
		#endregion

		private void okButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		public string CustomFilter
		{
			get
			{
				return customFilterTextBox.Text;
			}
		}

	}
}
