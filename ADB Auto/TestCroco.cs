using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADB_Auto
{
    public partial class TestCroco : Form
    {
        public TestCroco()
        {
            InitializeComponent();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            indic2.Visible = false;
            indic3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            indic1.Visible = false;
            indic3.Visible = false;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            indic1.Visible = false;
            indic2.Visible = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 0)
            {
                indic1.Visible = true;
            }

            if (tabControl1.SelectedIndex == 1)
            {
                indic2.Visible = true;
            }

            if (tabControl1.SelectedIndex == 2)
            {
                indic3.Visible = true;
            }
        }

        private void TestCroco_Load(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                indic1.Visible = true;
            }

            if (tabControl1.SelectedIndex == 1)
            {
                indic2.Visible = true;
            }

            if (tabControl1.SelectedIndex == 2)
            {
                indic3.Visible = true;
            }
        }
    }
}
