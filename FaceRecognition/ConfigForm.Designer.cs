namespace OpenScreenProjectFaceRS
{
    partial class ConfigForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.cfgServerAddress1 = new System.Windows.Forms.TextBox();
            this.cfgLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cfgDirectionOrientation = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cfgServerAddress2 = new System.Windows.Forms.TextBox();
            this.cfgCentroidBound = new System.Windows.Forms.TextBox();
            this.cfgFaceSizeBound = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cfgScreenID = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cfgCompensation = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.cfgInterval = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cfgCompensation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cfgInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Address 1";
            // 
            // cfgServerAddress1
            // 
            this.cfgServerAddress1.Location = new System.Drawing.Point(91, 19);
            this.cfgServerAddress1.Name = "cfgServerAddress1";
            this.cfgServerAddress1.Size = new System.Drawing.Size(150, 20);
            this.cfgServerAddress1.TabIndex = 1;
            // 
            // cfgLocation
            // 
            this.cfgLocation.Location = new System.Drawing.Point(103, 48);
            this.cfgLocation.Name = "cfgLocation";
            this.cfgLocation.Size = new System.Drawing.Size(49, 20);
            this.cfgLocation.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Screen Loc. (x,y)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Direc/ Orient.";
            // 
            // cfgDirectionOrientation
            // 
            this.cfgDirectionOrientation.FormattingEnabled = true;
            this.cfgDirectionOrientation.Items.AddRange(new object[] {
            "X+",
            "X-",
            "Y+",
            "Y-"});
            this.cfgDirectionOrientation.Location = new System.Drawing.Point(103, 74);
            this.cfgDirectionOrientation.Name = "cfgDirectionOrientation";
            this.cfgDirectionOrientation.Size = new System.Drawing.Size(49, 21);
            this.cfgDirectionOrientation.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Centroid Bound";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Face Size Bound";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Server Address 2";
            // 
            // cfgServerAddress2
            // 
            this.cfgServerAddress2.Location = new System.Drawing.Point(91, 47);
            this.cfgServerAddress2.Name = "cfgServerAddress2";
            this.cfgServerAddress2.Size = new System.Drawing.Size(150, 20);
            this.cfgServerAddress2.TabIndex = 12;
            // 
            // cfgCentroidBound
            // 
            this.cfgCentroidBound.Location = new System.Drawing.Point(103, 102);
            this.cfgCentroidBound.Name = "cfgCentroidBound";
            this.cfgCentroidBound.Size = new System.Drawing.Size(138, 20);
            this.cfgCentroidBound.TabIndex = 13;
            // 
            // cfgFaceSizeBound
            // 
            this.cfgFaceSizeBound.Location = new System.Drawing.Point(103, 128);
            this.cfgFaceSizeBound.Name = "cfgFaceSizeBound";
            this.cfgFaceSizeBound.Size = new System.Drawing.Size(138, 20);
            this.cfgFaceSizeBound.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(158, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "e.g.: 4,3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(152, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 16;
            // 
            // btSave
            // 
            this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btSave.Location = new System.Drawing.Point(202, 374);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(60, 23);
            this.btSave.TabIndex = 20;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cfgServerAddress1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cfgServerAddress2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 79);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cfgScreenID);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cfgLocation);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cfgDirectionOrientation);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cfgFaceSizeBound);
            this.groupBox2.Controls.Add(this.cfgCentroidBound);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(12, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 181);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Camera Info";
            // 
            // cfgScreenID
            // 
            this.cfgScreenID.Location = new System.Drawing.Point(103, 19);
            this.cfgScreenID.Name = "cfgScreenID";
            this.cfgScreenID.Size = new System.Drawing.Size(138, 20);
            this.cfgScreenID.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 21);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "Screen ID";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(158, 77);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "e.g.: X+";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.cfgCompensation);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.cfgInterval);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(12, 284);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(250, 84);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Recognition Info";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(154, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "(times)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(154, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "(sec)";
            // 
            // cfgCompensation
            // 
            this.cfgCompensation.Location = new System.Drawing.Point(106, 48);
            this.cfgCompensation.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cfgCompensation.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cfgCompensation.Name = "cfgCompensation";
            this.cfgCompensation.Size = new System.Drawing.Size(49, 20);
            this.cfgCompensation.TabIndex = 20;
            this.cfgCompensation.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Compensation";
            // 
            // cfgInterval
            // 
            this.cfgInterval.Location = new System.Drawing.Point(106, 19);
            this.cfgInterval.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cfgInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cfgInterval.Name = "cfgInterval";
            this.cfgInterval.Size = new System.Drawing.Size(49, 20);
            this.cfgInterval.TabIndex = 18;
            this.cfgInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Registration Interval";
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 406);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btSave);
            this.Name = "ConfigForm";
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cfgCompensation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cfgInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox cfgServerAddress1;
        private System.Windows.Forms.TextBox cfgLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cfgDirectionOrientation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox cfgServerAddress2;
        private System.Windows.Forms.TextBox cfgCentroidBound;
        private System.Windows.Forms.TextBox cfgFaceSizeBound;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown cfgInterval;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown cfgCompensation;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox cfgScreenID;
        private System.Windows.Forms.Label label14;
    }
}