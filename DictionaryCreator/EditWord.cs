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
    public partial class EditWord : Form
    {
        public Form1 form1;
        public string fWord, fDesc, fIPA, fEx;
        public int selected;

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox4.Text = "";
            form1.Activate();
        }

        public EditWord()
        {
            InitializeComponent();
        }

        private void EditWord_Load(object sender, EventArgs e)
        {
            textBox5.Text = fWord;
            textBox6.Text = fDesc;
            textBox7.Text = fIPA;
            textBox4.Text = fEx;
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
            WordDefinition oldword = new WordDefinition();
            oldword.Word = fWord;
            oldword.Description = fDesc;
            oldword.IPA = fIPA;
            oldword.Example = fEx;
            form1.editWords(oldword, word, this.selected);
            this.Hide();
            form1.Activate();
        }
    }
}
