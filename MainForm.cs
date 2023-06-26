
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace TuringMachine
{
    public class MainForm : Form
    {
        public static Coms work = new Coms();
        private List<string> pointers = new List<string>();
        private List<string> LineList = new List<string>();
        private List<string> Alph = new List<string>();
        private int pointerPosition = MainForm.work.CountEmpty / 2;
        private IContainer components = (IContainer)null;
        private Button ButtonFront;
        private Button ButtonBack;
        private TextBox textBoxLine;
        private TextBox textBoxPointer;
        private Button ButtonStep;
        private SaveFileDialog saveFileDialog;
        private DataGridView dataGridView1;
        private Button ButtonDeleteColumns;
        private Label label2;
        private TextBox textBoxAlph;
        private Button ButtonAddColumns;
        private Button buttonEraseLine;
        private Button ButtonEnterLine;
        private GroupBox groupBox1;
        private Button load;
        private Button save;
        private Button Auto;
        private GroupBox groupBox3;
        private GroupBox groupBox2;

        public MainForm()
        {
            this.InitializeComponent();
            this.MaximizeBox = false;
            MainForm.work.CountEmpty = 57;
            for (int index = 0; index < MainForm.work.CountEmpty; ++index)
                this.textBoxLine.Text += "_";
            this.ButtonFront_Click((object)null, (EventArgs)null);
            int num = (int)MessageBox.Show("Машина Тьюринга\nпример команды: 1 s R\n1 - номер состояния\ns - измененный символ\nR - переместить ленту вправо\nL - переместить ленту влево\nH - завершить программу");
        }

        public bool Сheck(string text)
        {
            for (int index = 0; index < MainForm.work.CountEmpty; ++index)
            {
                if (text.Length < MainForm.work.CountEmpty * 2 || text[index] != '_')
                    return false;
            }
            return true;
        }

        public void Erase()
        {
            this.ButtonBack.Enabled = false;
            this.ButtonFront.Enabled = false;
            this.textBoxPointer.Text = "";
            this.textBoxLine.Text = "";
            for (int index = 0; index < MainForm.work.CountEmpty; ++index)
                this.textBoxLine.Text += "_";
            this.Alph.Clear();
            this.LineList.Clear();
        }

        private void CreatingLinePointer()
        {
            this.textBoxPointer.Text = "";
            this.pointers.Clear();
            for (int index = 0; index < this.textBoxLine.TextLength; ++index)
                this.pointers.Add("_");
        }

        private void RecoverPointer(bool left)
        {
            if (left)
            {
                ++this.pointerPosition;
                this.pointers[this.pointerPosition] = "↑";
                for (int index = 0; index < this.textBoxLine.TextLength; ++index)
                    this.textBoxPointer.Text += this.pointers[index];
            }
            else
            {
                --this.pointerPosition;
                this.pointers[this.pointerPosition] = "↑";
                for (int index = 0; index < this.textBoxLine.TextLength; ++index)
                    this.textBoxPointer.Text += this.pointers[index];
            }
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.CreatingLinePointer();
                --this.pointerPosition;
                this.pointers[this.pointerPosition] = "↑";
                for (int index = 0; index < this.textBoxLine.TextLength; ++index)
                    this.textBoxPointer.Text += this.pointers[index];
            }
            catch (ArgumentOutOfRangeException ex)
            {
                int num = (int)MessageBox.Show("Указатель находится в крайнем левом положении.\nПеремещение ленты влево невозможно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.RecoverPointer(true);
            }
        }

        private void ButtonFront_Click(object sender, EventArgs e)
        {
            try
            {
                this.CreatingLinePointer();
                ++this.pointerPosition;
                this.pointers[this.pointerPosition] = "↑";
                for (int index = 0; index < this.textBoxLine.TextLength; ++index)
                    this.textBoxPointer.Text += this.pointers[index];
            }
            catch (ArgumentOutOfRangeException ex)
            {
                int num = (int)MessageBox.Show("Указатель находится в крайнем правом положении.\nПеремещение ленты правом невозможно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.RecoverPointer(false);
            }
        }

        public void CreateTable()
        {
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.Columns.Add("zero", "");
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.Columns.Add("1", "q1");
            for (int index = 0; index < this.Alph.Count; ++index)
                this.dataGridView1.Rows.Add(new object[1]
                {
          (object) this.Alph[index]
                });
            this.dataGridView1.Columns[0].ReadOnly = true;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AllowUserToAddRows = false;
        }

        private void buttonEraseLine_Click(object sender, EventArgs e)
        {
            this.ButtonEnterLine.Enabled = true;
            this.ButtonStep.Enabled = false;
            this.textBoxLine.Text = "";
            for (int index = 0; index < MainForm.work.CountEmpty; ++index)
                this.textBoxLine.Text += "_";
            this.pointerPosition = MainForm.work.CountEmpty - 1;
            this.LineList.Clear();
            MainForm.work.Command = (string)null;
            MainForm.work.Direction = (string)null;
            MainForm.work.NextColumn = (string)null;
            MainForm.work.ReplaceOnIt = (string)null;
        }

        public void UpdateLine()
        {
            this.textBoxLine.Text = "";
            for (int index = 0; index < this.LineList.Count; ++index)
                this.textBoxLine.Text += this.LineList[index];
        }

        private void ButtonStep_Click(object sender, EventArgs e)
        {
            if (MainForm.work.NextColumn == null)
                MainForm.work.NextColumn = "1";
            MainForm.work.CurrentContentCell = this.LineList[this.pointerPosition];
            try
            {
                MainForm.work.CurrentContentCell = this.LineList[this.pointerPosition];
                this.dataGridView1.CurrentCell = this.dataGridView1[MainForm.work.NextColumn, this.Alph.IndexOf(MainForm.work.CurrentContentCell)];
                MainForm.work.Command = (string)this.dataGridView1[MainForm.work.NextColumn, this.Alph.IndexOf(MainForm.work.CurrentContentCell)].Value;
                try
                {
                    for (int x = 0; x < dataGridView1.ColumnCount; ++x)
                    {
                        for (int y = 0; y < dataGridView1.RowCount; ++y)
                        {
                            if (dataGridView1.Rows[y].Cells[x].Value == null)
                                dataGridView1.Rows[y].Cells[x].Value = " ";
                        }
                    }
                    MainForm.work.SplittedCommand = ((IEnumerable<string>)MainForm.work.Command.Split(' ')).ToList<string>();
                }
                catch
                {
                    int num = (int)MessageBox.Show("Введите состояние");
                }
                if (this.dataGridView1.Columns.Contains(MainForm.work.SplittedCommand[0]))
                {
                    MainForm.work.NextColumn = MainForm.work.SplittedCommand[0];
                }
                else
                {
                    int num1 = (int)MessageBox.Show("Состояние не найдено", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                if (this.Alph.Contains(MainForm.work.SplittedCommand[1]))
                {
                    MainForm.work.ReplaceOnIt = MainForm.work.SplittedCommand[1];
                }
                else
                {
                    int num2 = (int)MessageBox.Show("Алфавит не содержит этого символа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                MainForm.work.Direction = MainForm.work.SplittedCommand[2];
                this.LineList[this.pointerPosition] = MainForm.work.ReplaceOnIt;
                this.UpdateLine();
                if (MainForm.work.Direction == "R")
                    this.ButtonFront_Click((object)null, (EventArgs)null);
                else if (MainForm.work.Direction == "L")
                    this.ButtonBack_Click((object)null, (EventArgs)null);
                else if (MainForm.work.Direction == "H")
                {
                    int num3 = (int)MessageBox.Show("Конец программы");
                }
                else
                {
                    int num4 = (int)MessageBox.Show("Невозможно переместить ленту", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch
            {
                int num = (int)MessageBox.Show("Пожалуйста, проверьте вводимые данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ButtonAddColumns_Click(object sender, EventArgs e)
        {
            string name = this.dataGridView1.Columns[this.dataGridView1.Columns.Count - 1].Name;
            DataGridViewColumnCollection columns = this.dataGridView1.Columns;
            int num = int.Parse(name) + 1;
            string columnName = num.ToString();
            num = int.Parse(name) + 1;
            string headerText = "q" + num.ToString();
            columns.Add(columnName, headerText);
        }

        private void ButtonDeleteColumns_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Columns.Count <= 2)
                return;
            this.dataGridView1.Columns.RemoveAt(this.dataGridView1.Columns.Count - 1);
        }

        private void ButtonEnterLine_Click_1(object sender, EventArgs e)
        {
            if (this.textBoxLine.Text.Length > 57)
            {
                List<string> stringList = new List<string>();
                for (int index = 0; index < this.textBoxLine.Text.Length; ++index)
                    stringList.Add(this.textBoxLine.Text[index].ToString());
                int num = 0;
                num = this.textBoxLine.Text.Length - 57;
                while (stringList.Count != 57)
                    stringList.RemoveAt(stringList.Count - 1);
                this.textBoxLine.Clear();
                for (int index = 0; index < stringList.Count; ++index)
                    this.textBoxLine.Text += stringList[index];
            }
            this.Alph.Clear();
            int num1 = 0;
            char ch;
            for (int index = 0; index < this.textBoxLine.TextLength; ++index)
            {
                List<string> lineList = this.LineList;
                ch = this.textBoxLine.Text[index];
                string str = ch.ToString();
                lineList.Add(str);
            }
            this.Alph.Add("_");
            for (int index = 0; index < this.textBoxAlph.TextLength; ++index)
            {
                List<string> alph = this.Alph;
                ch = this.textBoxAlph.Text[index];
                string str = ch.ToString();
                alph.Add(str);
            }
            for (int index = 0; index < this.textBoxLine.TextLength; ++index)
            {
                if (!this.Alph.Contains(this.LineList[index]))
                    ++num1;
            }
            if (num1 == 0 && this.Alph.Distinct<string>().ToList<string>().Count == this.Alph.Count)
            {
                this.pointerPosition = (MainForm.work.CountEmpty - 1) / 2;
                this.CreatingLinePointer();
                for (int index = 0; index < this.textBoxPointer.TextLength; ++index)
                    this.textBoxPointer.Text += this.pointers[index];
                this.ButtonBack.Enabled = true;
                this.ButtonFront.Enabled = true;
                if (this.dataGridView1.RowCount == 0)
                    this.CreateTable();
                else if (MessageBox.Show("Удалить состояния?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.CreateTable();
                this.ButtonEnterLine.Enabled = false;
                this.ButtonAddColumns.Enabled = true;
                this.ButtonDeleteColumns.Enabled = true;
                this.ButtonStep.Enabled = true;
                this.ButtonFront_Click((object)null, (EventArgs)null);
            }
            else if (num1 != 0)
            {
                int num2 = (int)MessageBox.Show("Лента содержит символы, не входящие в алфавит.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Erase();
            }
            else if (this.textBoxLine.TextLength == MainForm.work.CountEmpty * 2)
            {
                int num3 = (int)MessageBox.Show("Лента пуста. Введите хотя бы один символ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Erase();
            }
            else if (this.Alph.Distinct<string>().ToList<string>().Count != this.Alph.Count)
            {
                int num4 = (int)MessageBox.Show("Алфавит содержит повторяющиеся символы. Пожалуйста, убедитесь, что есть только одна копия каждого.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Erase();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ButtonFront = new System.Windows.Forms.Button();
            this.ButtonBack = new System.Windows.Forms.Button();
            this.textBoxLine = new System.Windows.Forms.TextBox();
            this.textBoxPointer = new System.Windows.Forms.TextBox();
            this.ButtonStep = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ButtonDeleteColumns = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAlph = new System.Windows.Forms.TextBox();
            this.ButtonAddColumns = new System.Windows.Forms.Button();
            this.buttonEraseLine = new System.Windows.Forms.Button();
            this.ButtonEnterLine = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.load = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.Auto = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonFront
            // 
            this.ButtonFront.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ButtonFront.Enabled = false;
            this.ButtonFront.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonFront.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ButtonFront.Location = new System.Drawing.Point(652, 138);
            this.ButtonFront.Name = "ButtonFront";
            this.ButtonFront.Size = new System.Drawing.Size(262, 38);
            this.ButtonFront.TabIndex = 1;
            this.ButtonFront.Text = "Сдвинуть в право ";
            this.ButtonFront.UseVisualStyleBackColor = false;
            this.ButtonFront.Click += new System.EventHandler(this.ButtonFront_Click);
            // 
            // ButtonBack
            // 
            this.ButtonBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ButtonBack.Enabled = false;
            this.ButtonBack.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonBack.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ButtonBack.Location = new System.Drawing.Point(2, 135);
            this.ButtonBack.Name = "ButtonBack";
            this.ButtonBack.Size = new System.Drawing.Size(262, 38);
            this.ButtonBack.TabIndex = 2;
            this.ButtonBack.Text = "Сдвинуть влево";
            this.ButtonBack.UseVisualStyleBackColor = false;
            this.ButtonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // textBoxLine
            // 
            this.textBoxLine.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxLine.Location = new System.Drawing.Point(11, 47);
            this.textBoxLine.Name = "textBoxLine";
            this.textBoxLine.Size = new System.Drawing.Size(903, 39);
            this.textBoxLine.TabIndex = 3;
            this.textBoxLine.TabStop = false;
            // 
            // textBoxPointer
            // 
            this.textBoxPointer.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPointer.Location = new System.Drawing.Point(12, 92);
            this.textBoxPointer.Name = "textBoxPointer";
            this.textBoxPointer.ReadOnly = true;
            this.textBoxPointer.Size = new System.Drawing.Size(902, 39);
            this.textBoxPointer.TabIndex = 13;
            // 
            // ButtonStep
            // 
            this.ButtonStep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ButtonStep.Enabled = false;
            this.ButtonStep.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonStep.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ButtonStep.Location = new System.Drawing.Point(652, 3);
            this.ButtonStep.Name = "ButtonStep";
            this.ButtonStep.Size = new System.Drawing.Size(262, 38);
            this.ButtonStep.TabIndex = 15;
            this.ButtonStep.Text = "Следующий шаг ";
            this.ButtonStep.UseVisualStyleBackColor = false;
            this.ButtonStep.Click += new System.EventHandler(this.ButtonStep_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 88);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.Size = new System.Drawing.Size(628, 253);
            this.dataGridView1.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxAlph);
            this.groupBox2.Font = new System.Drawing.Font("Palatino Linotype", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(280, 180);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(660, 347);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            // 
            // ButtonDeleteColumns
            // 
            this.ButtonDeleteColumns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ButtonDeleteColumns.Enabled = false;
            this.ButtonDeleteColumns.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonDeleteColumns.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ButtonDeleteColumns.Location = new System.Drawing.Point(10, 180);
            this.ButtonDeleteColumns.Name = "ButtonDeleteColumns";
            this.ButtonDeleteColumns.Size = new System.Drawing.Size(250, 50);
            this.ButtonDeleteColumns.TabIndex = 12;
            this.ButtonDeleteColumns.Text = "Удалить колонку";
            this.ButtonDeleteColumns.UseVisualStyleBackColor = false;
            this.ButtonDeleteColumns.Click += new System.EventHandler(this.ButtonDeleteColumns_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(6, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 28);
            this.label2.TabIndex = 9;
            this.label2.Text = "Алфавит:";
            // 
            // textBoxAlph
            // 
            this.textBoxAlph.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxAlph.Location = new System.Drawing.Point(121, 27);
            this.textBoxAlph.Name = "textBoxAlph";
            this.textBoxAlph.Size = new System.Drawing.Size(396, 32);
            this.textBoxAlph.TabIndex = 8;
            // 
            // ButtonAddColumns
            // 
            this.ButtonAddColumns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ButtonAddColumns.Enabled = false;
            this.ButtonAddColumns.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonAddColumns.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ButtonAddColumns.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonAddColumns.Location = new System.Drawing.Point(10, 124);
            this.ButtonAddColumns.Name = "ButtonAddColumns";
            this.ButtonAddColumns.Size = new System.Drawing.Size(250, 50);
            this.ButtonAddColumns.TabIndex = 11;
            this.ButtonAddColumns.Text = "Добавить колонку";
            this.ButtonAddColumns.UseVisualStyleBackColor = false;
            this.ButtonAddColumns.Click += new System.EventHandler(this.ButtonAddColumns_Click);
            // 
            // buttonEraseLine
            // 
            this.buttonEraseLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonEraseLine.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonEraseLine.Location = new System.Drawing.Point(10, 68);
            this.buttonEraseLine.Name = "buttonEraseLine";
            this.buttonEraseLine.Size = new System.Drawing.Size(250, 50);
            this.buttonEraseLine.TabIndex = 14;
            this.buttonEraseLine.Text = "Удалить ленту";
            this.buttonEraseLine.UseVisualStyleBackColor = false;
            this.buttonEraseLine.Click += new System.EventHandler(this.buttonEraseLine_Click);
            // 
            // ButtonEnterLine
            // 
            this.ButtonEnterLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ButtonEnterLine.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonEnterLine.Location = new System.Drawing.Point(10, 12);
            this.ButtonEnterLine.Name = "ButtonEnterLine";
            this.ButtonEnterLine.Size = new System.Drawing.Size(250, 50);
            this.ButtonEnterLine.TabIndex = 5;
            this.ButtonEnterLine.Text = "Создать ленту";
            this.ButtonEnterLine.UseVisualStyleBackColor = false;
            this.ButtonEnterLine.Click += new System.EventHandler(this.ButtonEnterLine_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.load);
            this.groupBox1.Controls.Add(this.save);
            this.groupBox1.Controls.Add(this.ButtonEnterLine);
            this.groupBox1.Controls.Add(this.buttonEraseLine);
            this.groupBox1.Controls.Add(this.ButtonAddColumns);
            this.groupBox1.Controls.Add(this.ButtonDeleteColumns);
            this.groupBox1.Font = new System.Drawing.Font("Palatino Linotype", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(1, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 347);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // load
            // 
            this.load.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.load.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold);
            this.load.Location = new System.Drawing.Point(10, 235);
            this.load.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(250, 50);
            this.load.TabIndex = 16;
            this.load.Text = "Занрузить состояние";
            this.load.UseVisualStyleBackColor = false;
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // save
            // 
            this.save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.save.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold);
            this.save.Location = new System.Drawing.Point(10, 289);
            this.save.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(250, 50);
            this.save.TabIndex = 15;
            this.save.Text = "Сохранить состояние";
            this.save.UseVisualStyleBackColor = false;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // Auto
            // 
            this.Auto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Auto.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Auto.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Auto.Location = new System.Drawing.Point(2, 1);
            this.Auto.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Auto.Name = "Auto";
            this.Auto.Size = new System.Drawing.Size(262, 38);
            this.Auto.TabIndex = 18;
            this.Auto.Text = "Автоматический проход";
            this.Auto.UseVisualStyleBackColor = false;
            this.Auto.Click += new System.EventHandler(this.Auto_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.LightGray;
            this.groupBox3.Controls.Add(this.Auto);
            this.groupBox3.Controls.Add(this.ButtonBack);
            this.groupBox3.Location = new System.Drawing.Point(10, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(903, 177);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(927, 539);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ButtonStep);
            this.Controls.Add(this.textBoxPointer);
            this.Controls.Add(this.textBoxLine);
            this.Controls.Add(this.ButtonFront);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Turing Machine";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void save_Click(object sender, EventArgs e)
        {
            List<string> states = new List<string>();
            SaveFileDialog savef = new SaveFileDialog();
            savef.DefaultExt = ".txt";
            savef.Filter = "TuringMachine|*.txt";
            states.Add(textBoxAlph.Text);
            states.Add(Convert.ToString(dataGridView1.RowCount));
            states.Add(Convert.ToString(dataGridView1.ColumnCount - 1));
            for (int x = 0; x < dataGridView1.ColumnCount; ++x)
            {
                for (int y = 0; y < dataGridView1.RowCount; ++y)
                {
                    if (dataGridView1.Rows[y].Cells[x].Value == null)
                        dataGridView1.Rows[y].Cells[x].Value = " ";
                }
            }
            for (int y = 1; y < dataGridView1.ColumnCount; y++)
            {
                for (int j = 0; j < dataGridView1.RowCount; j++)
                {
                    states.Add(this.dataGridView1.Rows[j].Cells[y].Value.ToString());
                }
            }

            if (savef.ShowDialog() == System.Windows.Forms.DialogResult.OK && savef.FileName.Length > 0)
            {
                using (StreamWriter fs = new StreamWriter(savef.FileName, true))
                {
                    for (int i = 0; i < states.Count; i++)
                    {
                        fs.WriteLine(states[i]);
                    }
                    fs.Close();
                }
            }
        }

        private void load_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var openf = new OpenFileDialog();
            openf.Filter = "Text files(*.txt)|*.txt";
            openf.DefaultExt = ".txt";
            if (openf.ShowDialog() == System.Windows.Forms.DialogResult.OK && openf.FileName.Length > 0)
            {
                string filename = openf.FileName;
                var table = System.IO.File.ReadAllLines(filename);
                string alpha = table[0];
                textBoxAlph.Text = alpha;
                Alph.Clear();
                this.Alph.Add("_");
                for (int index = 0; index < this.textBoxAlph.TextLength; ++index)
                {
                    List<string> alph = this.Alph;
                    char ch = this.textBoxAlph.Text[index];
                    string str = ch.ToString();
                    alph.Add(str);
                }
                CreateTable();
                int rows = Convert.ToInt32(table[1]);
                int cols = Convert.ToInt32(table[2]);
                for (int i = 0; i < cols - 1; i++)
                {
                    ButtonAddColumns_Click(sender, e);
                }
                int x = 3;
                for (int y = 1; y < dataGridView1.ColumnCount; y++)
                {
                    for (int j = 0; j < dataGridView1.RowCount; j++)
                    {
                        this.dataGridView1.Rows[j].Cells[y].Value = table[x];
                        x++;
                    }
                }
                this.ButtonBack.Enabled = false;
                this.ButtonFront.Enabled = false;
                this.textBoxPointer.Text = "";
                this.textBoxLine.Text = "";
                for (int index = 0; index < MainForm.work.CountEmpty; ++index)
                    this.textBoxLine.Text += "_";
            }
        }
        Timer timer = new Timer();
        public void Auto_Click(object sender, EventArgs e)
        {
            timer.Interval = 100;
            timer.Tick += new EventHandler(MyTimer_Tick);
            timer.Start();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            MakeMove();
        }
        private int MakeMove()
        {

            if (MainForm.work.NextColumn == null)
                MainForm.work.NextColumn = "1";
            MainForm.work.CurrentContentCell = this.LineList[this.pointerPosition];
            try
            {
                MainForm.work.CurrentContentCell = this.LineList[this.pointerPosition];
                this.dataGridView1.CurrentCell = this.dataGridView1[MainForm.work.NextColumn, this.Alph.IndexOf(MainForm.work.CurrentContentCell)];
                MainForm.work.Command = (string)this.dataGridView1[MainForm.work.NextColumn, this.Alph.IndexOf(MainForm.work.CurrentContentCell)].Value;
                try
                {
                    for (int x = 0; x < dataGridView1.ColumnCount; ++x)
                    {
                        for (int y = 0; y < dataGridView1.RowCount; ++y)
                        {
                            if (dataGridView1.Rows[y].Cells[x].Value == null)
                                dataGridView1.Rows[y].Cells[x].Value = " ";
                        }
                    }
                    MainForm.work.SplittedCommand = ((IEnumerable<string>)MainForm.work.Command.Split(' ')).ToList<string>();
                }
                catch
                {
                    timer.Enabled = false;
                    MessageBox.Show("Input state.");
                    return -1;
                }
                if (this.dataGridView1.Columns.Contains(MainForm.work.SplittedCommand[0]))
                {
                    MainForm.work.NextColumn = MainForm.work.SplittedCommand[0];
                }
                else
                {
                    timer.Enabled = false;
                    MessageBox.Show("Состояние не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return -1;
                }
                if (this.Alph.Contains(MainForm.work.SplittedCommand[1]))
                {
                    MainForm.work.ReplaceOnIt = MainForm.work.SplittedCommand[1];
                }
                else
                {
                    timer.Enabled = false;
                    MessageBox.Show("Алфавит не содержит этого символа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return -1;
                }
                MainForm.work.Direction = MainForm.work.SplittedCommand[2];
                this.LineList[this.pointerPosition] = MainForm.work.ReplaceOnIt;
                this.UpdateLine();
                if (MainForm.work.Direction == "R")
                    this.ButtonFront_Click((object)null, (EventArgs)null);
                else if (MainForm.work.Direction == "L")
                    this.ButtonBack_Click((object)null, (EventArgs)null);
                else if (MainForm.work.Direction == "H")
                {
                    timer.Enabled = false;
                    MessageBox.Show("Конец программы");
                    return -1;
                }
                else
                {
                    timer.Enabled = false;
                    MessageBox.Show("Невозможно переместить ленту", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return -1;
                }
                return 0;
            }
            catch
            {
                timer.Enabled = false;
                MessageBox.Show("Пожалуйста, проверьте вводимые данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return -1;
            }
        }
    }
}
