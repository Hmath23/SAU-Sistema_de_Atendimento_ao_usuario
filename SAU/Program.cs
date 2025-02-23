﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAU
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new frmLogin());

            frmLogin frm = new frmLogin();
            frm.ShowDialog();
            if (frm.LoginSucesso)
            {
                Application.Run(new frmPrincipal());
            }
        }
    }
}
