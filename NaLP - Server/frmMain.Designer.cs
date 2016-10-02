namespace NaLP___Server
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tPageLog = new System.Windows.Forms.TabPage();
            this.rTxtLog = new System.Windows.Forms.RichTextBox();
            this.tPageConnected = new System.Windows.Forms.TabPage();
            this.dgvConnections = new System.Windows.Forms.DataGridView();
            this.clmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctxtConnections = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemKick = new System.Windows.Forms.ToolStripMenuItem();
            this.itemBan = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tPageLog.SuspendLayout();
            this.tPageConnected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConnections)).BeginInit();
            this.ctxtConnections.SuspendLayout();
            this.SuspendLayout();
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(161, 13);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(57, 20);
            this.numPort.TabIndex = 0;
            this.numPort.Value = new decimal(new int[] {
            1337,
            0,
            0,
            0});
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(126, 15);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(224, 11);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(303, 11);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tPageLog);
            this.tabMain.Controls.Add(this.tPageConnected);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabMain.Location = new System.Drawing.Point(0, 39);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(505, 445);
            this.tabMain.TabIndex = 4;
            // 
            // tPageLog
            // 
            this.tPageLog.Controls.Add(this.rTxtLog);
            this.tPageLog.Location = new System.Drawing.Point(4, 22);
            this.tPageLog.Name = "tPageLog";
            this.tPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tPageLog.Size = new System.Drawing.Size(497, 419);
            this.tPageLog.TabIndex = 0;
            this.tPageLog.Text = "  Log";
            this.tPageLog.UseVisualStyleBackColor = true;
            // 
            // rTxtLog
            // 
            this.rTxtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rTxtLog.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.rTxtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rTxtLog.Location = new System.Drawing.Point(3, 3);
            this.rTxtLog.Name = "rTxtLog";
            this.rTxtLog.ReadOnly = true;
            this.rTxtLog.Size = new System.Drawing.Size(491, 413);
            this.rTxtLog.TabIndex = 0;
            this.rTxtLog.Text = "";
            // 
            // tPageConnected
            // 
            this.tPageConnected.Controls.Add(this.dgvConnections);
            this.tPageConnected.Location = new System.Drawing.Point(4, 22);
            this.tPageConnected.Name = "tPageConnected";
            this.tPageConnected.Padding = new System.Windows.Forms.Padding(3);
            this.tPageConnected.Size = new System.Drawing.Size(497, 419);
            this.tPageConnected.TabIndex = 1;
            this.tPageConnected.Text = "Connected";
            this.tPageConnected.UseVisualStyleBackColor = true;
            // 
            // dgvConnections
            // 
            this.dgvConnections.AllowUserToAddRows = false;
            this.dgvConnections.AllowUserToDeleteRows = false;
            this.dgvConnections.AllowUserToResizeRows = false;
            this.dgvConnections.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConnections.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvConnections.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvConnections.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvConnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConnections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmName,
            this.clmIP,
            this.clmTime});
            this.dgvConnections.ContextMenuStrip = this.ctxtConnections;
            this.dgvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvConnections.Location = new System.Drawing.Point(3, 3);
            this.dgvConnections.MultiSelect = false;
            this.dgvConnections.Name = "dgvConnections";
            this.dgvConnections.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvConnections.RowHeadersVisible = false;
            this.dgvConnections.Size = new System.Drawing.Size(491, 413);
            this.dgvConnections.TabIndex = 0;
            // 
            // clmName
            // 
            this.clmName.HeaderText = "Name";
            this.clmName.Name = "clmName";
            this.clmName.ReadOnly = true;
            // 
            // clmIP
            // 
            this.clmIP.HeaderText = "IP";
            this.clmIP.Name = "clmIP";
            this.clmIP.ReadOnly = true;
            // 
            // clmTime
            // 
            this.clmTime.HeaderText = "Time left on current key";
            this.clmTime.Name = "clmTime";
            // 
            // ctxtConnections
            // 
            this.ctxtConnections.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemKick,
            this.itemBan});
            this.ctxtConnections.Name = "ctxtConnections";
            this.ctxtConnections.Size = new System.Drawing.Size(161, 48);
            // 
            // itemKick
            // 
            this.itemKick.Name = "itemKick";
            this.itemKick.Size = new System.Drawing.Size(160, 22);
            this.itemKick.Text = "Kick";
            this.itemKick.Click += new System.EventHandler(this.itemKick_Click);
            // 
            // itemBan
            // 
            this.itemBan.Name = "itemBan";
            this.itemBan.Size = new System.Drawing.Size(160, 22);
            this.itemBan.Text = "Ban (IP + HWID)";
            this.itemBan.Click += new System.EventHandler(this.itemBan_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 484);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.numPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Not a Licensing Platform";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tPageLog.ResumeLayout(false);
            this.tPageConnected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConnections)).EndInit();
            this.ctxtConnections.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tPageLog;
        private System.Windows.Forms.TabPage tPageConnected;
        private System.Windows.Forms.RichTextBox rTxtLog;
        private System.Windows.Forms.ContextMenuStrip ctxtConnections;
        private System.Windows.Forms.ToolStripMenuItem itemKick;
        private System.Windows.Forms.ToolStripMenuItem itemBan;
        public System.Windows.Forms.DataGridView dgvConnections;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTime;
    }
}