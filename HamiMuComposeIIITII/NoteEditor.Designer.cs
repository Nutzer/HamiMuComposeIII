namespace HamiMuComposeIIITII
{
    partial class NoteEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.info = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.spmode = new System.Windows.Forms.ComboBox();
            this.spbonus = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.linest = new System.Windows.Forms.NumericUpDown();
            this.unkn1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.unkn2 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.linest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unkn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unkn2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(26, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(34, 34);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // info
            // 
            this.info.AutoSize = true;
            this.info.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.info.Location = new System.Drawing.Point(73, 20);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(170, 24);
            this.info.TabIndex = 1;
            this.info.Text = "Note @ Y on Z line";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(24, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Button";
            // 
            // button
            // 
            this.button.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button.FormattingEnabled = true;
            this.button.Items.AddRange(new object[] {
            "X",
            "A",
            "B",
            "Y",
            "S",
            "X Hold",
            "A Hold",
            "B Hold",
            "Y Hold"});
            this.button.Location = new System.Drawing.Point(318, 71);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(82, 28);
            this.button.TabIndex = 3;
            this.button.SelectedIndexChanged += new System.EventHandler(this.button_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(24, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "SP Mode";
            // 
            // spmode
            // 
            this.spmode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.spmode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spmode.FormattingEnabled = true;
            this.spmode.Items.AddRange(new object[] {
            "None",
            "Start",
            "Stop"});
            this.spmode.Location = new System.Drawing.Point(318, 105);
            this.spmode.Name = "spmode";
            this.spmode.Size = new System.Drawing.Size(82, 28);
            this.spmode.TabIndex = 5;
            this.spmode.SelectedIndexChanged += new System.EventHandler(this.spmode_SelectedIndexChanged);
            // 
            // spbonus
            // 
            this.spbonus.AutoSize = true;
            this.spbonus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spbonus.Location = new System.Drawing.Point(28, 145);
            this.spbonus.Name = "spbonus";
            this.spbonus.Size = new System.Drawing.Size(108, 24);
            this.spbonus.TabIndex = 6;
            this.spbonus.Text = "Bonus Line";
            this.spbonus.UseVisualStyleBackColor = true;
            this.spbonus.CheckedChanged += new System.EventHandler(this.spbonus_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(24, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Line Start (0 for None)";
            // 
            // linest
            // 
            this.linest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linest.Location = new System.Drawing.Point(318, 183);
            this.linest.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.linest.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.linest.Name = "linest";
            this.linest.Size = new System.Drawing.Size(82, 26);
            this.linest.TabIndex = 8;
            this.linest.ValueChanged += new System.EventHandler(this.linest_ValueChanged);
            // 
            // unkn1
            // 
            this.unkn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.unkn1.Location = new System.Drawing.Point(318, 217);
            this.unkn1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.unkn1.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.unkn1.Name = "unkn1";
            this.unkn1.Size = new System.Drawing.Size(82, 26);
            this.unkn1.TabIndex = 10;
            this.unkn1.ValueChanged += new System.EventHandler(this.unkn1_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(24, 219);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Unknown 1";
            // 
            // unkn2
            // 
            this.unkn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.unkn2.Location = new System.Drawing.Point(318, 251);
            this.unkn2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.unkn2.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.unkn2.Name = "unkn2";
            this.unkn2.Size = new System.Drawing.Size(82, 26);
            this.unkn2.TabIndex = 12;
            this.unkn2.ValueChanged += new System.EventHandler(this.unkn2_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(24, 253);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Unknown 2";
            // 
            // NoteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 297);
            this.Controls.Add(this.unkn2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.unkn1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.linest);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.spbonus);
            this.Controls.Add(this.spmode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.info);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NoteEditor";
            this.Text = "NoteEditor";
            this.Load += new System.EventHandler(this.NoteEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.linest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unkn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unkn2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label info;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox spmode;
        private System.Windows.Forms.CheckBox spbonus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown linest;
        private System.Windows.Forms.NumericUpDown unkn1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown unkn2;
        private System.Windows.Forms.Label label6;
    }
}