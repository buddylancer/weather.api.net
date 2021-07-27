using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            string request = CountryCode.Text + "," + ZipCode.Text;
            var response = ConsoleApplication.Controller.ExecuteRequest(request);
            Result.Text = response;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
