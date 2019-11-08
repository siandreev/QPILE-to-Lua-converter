using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "specify the path to the file with QPILE code")
            {
                try
                {
                    string path = textBox1.Text;
                    string newPath = textBox2.Text;
                    var reader = Logic.CreateReader(path);
                    string output = Logic.Convert(reader);
                    output = Logic.DeleteSpaces(output);
                    Logic.WriteIntoFile(output, newPath);
                }
                catch { }
            }
            else
            {
                string text = richTextBox1.Text;
                string output = Logic.Convert(text);
                // richTextBox2.Text = (output.Replace("\n\n","\n")).Replace("\n\n", "\n");
                richTextBox2.Text = Logic.DeleteSpaces(output);
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
