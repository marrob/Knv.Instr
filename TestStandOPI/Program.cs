﻿
namespace TestStandOPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;


    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new App().MainForm);
        }



        public class App
        {
            public Form MainForm;

            public App()
            {

                MainForm = new MainForm();

            }
        
        }
    }
}
