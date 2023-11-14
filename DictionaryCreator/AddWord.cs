using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DictionaryCreator
{
    public partial class AddWord : Form
    {
        public Form1 form1;
        public AddWord()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == String.Empty)
            {
                MessageBox.Show("Word can't be empty!", "Error while adding word");
                return;
            }
            WordDefinition word = new WordDefinition();
            word.Word = textBox5.Text;
            word.Description = textBox6.Text == String.Empty ? "-" : textBox6.Text;
            word.IPA = textBox7.Text == String.Empty ? "-" : textBox7.Text;
            word.Example = textBox4.Text == String.Empty ? "-" : textBox4.Text;
            form1.addWords(word);
            this.Hide();
            form1.Activate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox4.Text = "";
            form1.Activate();
        }
    }
}
