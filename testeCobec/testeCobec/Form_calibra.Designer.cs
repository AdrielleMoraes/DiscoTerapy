namespace testeCobec
{
    partial class Form_calibra
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btPause = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbEMG = new System.Windows.Forms.Label();
            this.btReceive = new System.Windows.Forms.Button();
            this.btConnectEMG = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.lbQAtual = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.lbQDireita = new System.Windows.Forms.Label();
            this.btQDireita = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lbQEsquerda = new System.Windows.Forms.Label();
            this.btQEsq = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lbQBaixo = new System.Windows.Forms.Label();
            this.btQBaixo = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbQCima = new System.Windows.Forms.Label();
            this.btQCima = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbQRef = new System.Windows.Forms.Label();
            this.btQRef = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus,
            this.toolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 518);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1040, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1040, 518);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1032, 489);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "EMG";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chart1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(241, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(788, 483);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // chart1
            // 
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(3, 18);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(782, 462);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btPause);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btReceive);
            this.groupBox1.Controls.Add(this.btConnectEMG);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 483);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // btPause
            // 
            this.btPause.Enabled = false;
            this.btPause.Location = new System.Drawing.Point(7, 78);
            this.btPause.Name = "btPause";
            this.btPause.Size = new System.Drawing.Size(120, 23);
            this.btPause.TabIndex = 8;
            this.btPause.Text = "Stop";
            this.btPause.UseVisualStyleBackColor = true;
            this.btPause.Click += new System.EventHandler(this.btPause_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbEMG);
            this.groupBox3.Location = new System.Drawing.Point(6, 123);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(226, 354);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Resultados";
            // 
            // lbEMG
            // 
            this.lbEMG.AutoSize = true;
            this.lbEMG.Location = new System.Drawing.Point(24, 36);
            this.lbEMG.Name = "lbEMG";
            this.lbEMG.Size = new System.Drawing.Size(70, 17);
            this.lbEMG.TabIndex = 5;
            this.lbEMG.Text = "Label Info";
            // 
            // btReceive
            // 
            this.btReceive.Enabled = false;
            this.btReceive.Location = new System.Drawing.Point(7, 48);
            this.btReceive.Name = "btReceive";
            this.btReceive.Size = new System.Drawing.Size(120, 23);
            this.btReceive.TabIndex = 7;
            this.btReceive.Text = "Receive";
            this.btReceive.UseVisualStyleBackColor = true;
            this.btReceive.Click += new System.EventHandler(this.btReceive_Click);
            // 
            // btConnectEMG
            // 
            this.btConnectEMG.Location = new System.Drawing.Point(6, 18);
            this.btConnectEMG.Name = "btConnectEMG";
            this.btConnectEMG.Size = new System.Drawing.Size(121, 23);
            this.btConnectEMG.TabIndex = 6;
            this.btConnectEMG.Text = "Connect";
            this.btConnectEMG.UseVisualStyleBackColor = true;
            this.btConnectEMG.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox9);
            this.tabPage2.Controls.Add(this.groupBox8);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1032, 489);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Quaternion";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.lbQAtual);
            this.groupBox9.Location = new System.Drawing.Point(8, 6);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(178, 145);
            this.groupBox9.TabIndex = 5;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Posição Atual";
            // 
            // lbQAtual
            // 
            this.lbQAtual.AutoSize = true;
            this.lbQAtual.Location = new System.Drawing.Point(6, 38);
            this.lbQAtual.Name = "lbQAtual";
            this.lbQAtual.Size = new System.Drawing.Size(62, 17);
            this.lbQAtual.TabIndex = 0;
            this.lbQAtual.Text = "lbQAtual";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lbQDireita);
            this.groupBox8.Controls.Add(this.btQDireita);
            this.groupBox8.Location = new System.Drawing.Point(195, 160);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(219, 144);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Direita";
            // 
            // lbQDireita
            // 
            this.lbQDireita.AutoSize = true;
            this.lbQDireita.Location = new System.Drawing.Point(6, 45);
            this.lbQDireita.Name = "lbQDireita";
            this.lbQDireita.Size = new System.Drawing.Size(46, 17);
            this.lbQDireita.TabIndex = 1;
            this.lbQDireita.Text = "label6";
            // 
            // btQDireita
            // 
            this.btQDireita.Location = new System.Drawing.Point(138, 115);
            this.btQDireita.Name = "btQDireita";
            this.btQDireita.Size = new System.Drawing.Size(75, 23);
            this.btQDireita.TabIndex = 0;
            this.btQDireita.Text = "Iniciar";
            this.btQDireita.UseVisualStyleBackColor = true;
            this.btQDireita.Click += new System.EventHandler(this.btQDireita_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.lbQEsquerda);
            this.groupBox7.Controls.Add(this.btQEsq);
            this.groupBox7.Location = new System.Drawing.Point(195, 310);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(219, 173);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Esquerda";
            // 
            // lbQEsquerda
            // 
            this.lbQEsquerda.AutoSize = true;
            this.lbQEsquerda.Location = new System.Drawing.Point(6, 41);
            this.lbQEsquerda.Name = "lbQEsquerda";
            this.lbQEsquerda.Size = new System.Drawing.Size(46, 17);
            this.lbQEsquerda.TabIndex = 1;
            this.lbQEsquerda.Text = "label5";
            // 
            // btQEsq
            // 
            this.btQEsq.Location = new System.Drawing.Point(138, 144);
            this.btQEsq.Name = "btQEsq";
            this.btQEsq.Size = new System.Drawing.Size(75, 23);
            this.btQEsq.TabIndex = 0;
            this.btQEsq.Text = "Iniciar";
            this.btQEsq.UseVisualStyleBackColor = true;
            this.btQEsq.Click += new System.EventHandler(this.btQEsq_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lbQBaixo);
            this.groupBox6.Controls.Add(this.btQBaixo);
            this.groupBox6.Location = new System.Drawing.Point(8, 310);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(178, 173);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Baixo";
            // 
            // lbQBaixo
            // 
            this.lbQBaixo.AutoSize = true;
            this.lbQBaixo.Location = new System.Drawing.Point(6, 41);
            this.lbQBaixo.Name = "lbQBaixo";
            this.lbQBaixo.Size = new System.Drawing.Size(46, 17);
            this.lbQBaixo.TabIndex = 1;
            this.lbQBaixo.Text = "label4";
            // 
            // btQBaixo
            // 
            this.btQBaixo.Location = new System.Drawing.Point(97, 144);
            this.btQBaixo.Name = "btQBaixo";
            this.btQBaixo.Size = new System.Drawing.Size(75, 23);
            this.btQBaixo.TabIndex = 0;
            this.btQBaixo.Text = "Iniciar";
            this.btQBaixo.UseVisualStyleBackColor = true;
            this.btQBaixo.Click += new System.EventHandler(this.btQBaixo_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbQCima);
            this.groupBox5.Controls.Add(this.btQCima);
            this.groupBox5.Location = new System.Drawing.Point(8, 160);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(178, 144);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Cima";
            // 
            // lbQCima
            // 
            this.lbQCima.AutoSize = true;
            this.lbQCima.Location = new System.Drawing.Point(6, 35);
            this.lbQCima.Name = "lbQCima";
            this.lbQCima.Size = new System.Drawing.Size(46, 17);
            this.lbQCima.TabIndex = 1;
            this.lbQCima.Text = "label2";
            // 
            // btQCima
            // 
            this.btQCima.Location = new System.Drawing.Point(97, 115);
            this.btQCima.Name = "btQCima";
            this.btQCima.Size = new System.Drawing.Size(75, 23);
            this.btQCima.TabIndex = 0;
            this.btQCima.Text = "Iniciar";
            this.btQCima.UseVisualStyleBackColor = true;
            this.btQCima.Click += new System.EventHandler(this.btQCima_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbQRef);
            this.groupBox4.Controls.Add(this.btQRef);
            this.groupBox4.Location = new System.Drawing.Point(195, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(219, 145);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Referência";
            // 
            // lbQRef
            // 
            this.lbQRef.AutoSize = true;
            this.lbQRef.Location = new System.Drawing.Point(6, 38);
            this.lbQRef.Name = "lbQRef";
            this.lbQRef.Size = new System.Drawing.Size(25, 17);
            this.lbQRef.TabIndex = 1;
            this.lbQRef.Text = "ref";
            // 
            // btQRef
            // 
            this.btQRef.Location = new System.Drawing.Point(138, 116);
            this.btQRef.Name = "btQRef";
            this.btQRef.Size = new System.Drawing.Size(75, 23);
            this.btQRef.TabIndex = 0;
            this.btQRef.Text = "Iniciar";
            this.btQRef.UseVisualStyleBackColor = true;
            this.btQRef.Click += new System.EventHandler(this.btQRef_Click);
            // 
            // Form_calibra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 540);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form_calibra";
            this.Text = "Form_calibra";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbEMG;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label lbQAtual;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label lbQDireita;
        private System.Windows.Forms.Button btQDireita;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label lbQEsquerda;
        private System.Windows.Forms.Button btQEsq;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lbQBaixo;
        private System.Windows.Forms.Button btQBaixo;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lbQCima;
        private System.Windows.Forms.Button btQCima;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lbQRef;
        private System.Windows.Forms.Button btQRef;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.Button btPause;
        private System.Windows.Forms.Button btReceive;
        private System.Windows.Forms.Button btConnectEMG;
    }
}