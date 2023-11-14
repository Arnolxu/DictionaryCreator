using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DictionaryCreator
{
    public partial class Form1 : Form
    {
        private string filepath;
        private List<WordDefinition> words;

        public Form1()
        {
            InitializeComponent();
        }

        private void ShowData(string search = "")
        {
            dataGridView1.Rows.Clear();
            foreach (var record in words)
            {
                if (record.Word.Contains(search)
                 || record.Description.Contains(search)
                 || record.IPA.Contains(search)
                 || record.Example.Contains(search))
                    dataGridView1.Rows.Add(new string[] { record.Word, record.Description, record.IPA, record.Example });
            }
            UpdateButtonAvailability();
        }

        private void UpdateButtonAvailability()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                button4.Enabled = true;
                button5.Enabled = true;
            }
            else
            {
                button4.Enabled = false;
                button5.Enabled = false;
            }
        }

        private void LoadDictionary()
        {
            using (var reader = new StreamReader(filepath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                words = csv.GetRecords<WordDefinition>().ToList();
                ShowData();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to open a previous .csv dictionary file?", "Open previous file", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.DefaultExt = "csv";
                openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.CheckPathExists = true;
                openFileDialog1.ShowDialog();
                filepath = openFileDialog1.FileName;
                LoadDictionary();
                Activate();
            }
            else
            {
                words = new List<WordDefinition> { };
            }

            Text = filepath != null ? Path.GetFileName(filepath) + " - Dictionary Creator" : "Untitled - Dictionary Creator";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowData(textBox1.Text);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void SaveFile(string filename)
        {
            using (var writer = new StreamWriter(filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(words);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filepath != null)
            {
                SaveFile(filepath);
                MessageBox.Show("Saved your dictionary to " + filepath, "Saved dictionary");
            }
            else
            {
                button6_Click(sender, e);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = filepath != null ? Path.GetFileName(filepath) : "untitled_dictionary.csv";
            savefile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                SaveFile(savefile.FileName);
                MessageBox.Show("Saved your dictionary to " + savefile.FileName, "Saved dictionary");
            }
        }

        public void addWords(WordDefinition word)
        {
            words.Add(word);
            dataGridView1.Rows.Add(new string[] { word.Word, word.Description, word.IPA, word.Example });
            UpdateButtonAvailability();
        }

        public void editWords(WordDefinition oldword, WordDefinition word, int selected)
        {
            words = words.ReplaceAllWords<WordDefinition>(oldword, word);
            dataGridView1.Rows[selected].SetValues(new string[] { word.Word, word.Description, word.IPA, word.Example });
            UpdateButtonAvailability();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddWord addword = new AddWord();
            addword.form1 = this;
            addword.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditWord editword = new EditWord();
            editword.form1 = this;
            editword.fWord = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            editword.fDesc = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            editword.fIPA = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            editword.fEx = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            editword.selected = dataGridView1.SelectedRows[0].Index;
            editword.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            WordDefinition selected = new WordDefinition();
            selected.Word = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            selected.Description = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            selected.IPA = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            selected.Example = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();

            words = words.RemoveAllWords<WordDefinition>(selected);
            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            UpdateButtonAvailability();
        }

        private void OpenFile()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to open another file? Unsaved changes will be gone", "Open another file", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.DefaultExt = "csv";
                openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.CheckPathExists = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // hide all open forms
                    foreach (Form curf in Application.OpenForms)
                    {
                        curf.Hide();
                    }
                    filepath = openFileDialog1.FileName;
                    LoadDictionary();
                    Activate();
                    Show();
                    Text = filepath != null ? Path.GetFileName(filepath) + " - Dictionary Creator" : "Untitled - Dictionary Creator";
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S) && !textBox1.Focused)
            {
                if (filepath != null)
                {
                    SaveFile(filepath);
                    MessageBox.Show("Saved your dictionary to " + filepath, "Saved dictionary");
                }
                else
                {
                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.FileName = filepath != null ? Path.GetFileName(filepath) : "untitled_dictionary.csv";
                    savefile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        SaveFile(savefile.FileName);
                        MessageBox.Show("Saved your dictionary to " + savefile.FileName, "Saved dictionary");
                    }
                }
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.S) && !textBox1.Focused)
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = filepath != null ? Path.GetFileName(filepath) : "untitled_dictionary.csv";
                savefile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    SaveFile(savefile.FileName);
                    MessageBox.Show("Saved your dictionary to " + savefile.FileName, "Saved dictionary");
                }
            }
            else if (keyData == (Keys.Control | Keys.O) && !textBox1.Focused)
            {
                OpenFile();
            }
            else if (keyData == (Keys.Control | Keys.E) && !textBox1.Focused)
            {
                EditWord editword = new EditWord();
                editword.form1 = this;
                editword.fWord = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                editword.fDesc = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                editword.fIPA = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                editword.fEx = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                editword.selected = dataGridView1.SelectedRows[0].Index;
                editword.Show();
            }
            else if (keyData == (Keys.Control | Keys.A) && !textBox1.Focused)
            {
                AddWord addword = new AddWord();
                addword.form1 = this;
                addword.Show();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.A))
            {
                e.Handled = true;
            }
        }
    }
}
