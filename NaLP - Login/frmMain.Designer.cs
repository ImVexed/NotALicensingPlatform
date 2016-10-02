namespace NaLP___Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtRUsername = new System.Windows.Forms.TextBox();
            this.txtRKey = new System.Windows.Forms.TextBox();
            this.txtRPassword = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txtActivateKey = new System.Windows.Forms.TextBox();
            this.btnActivate = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(65, 18);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(147, 25);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.Text = "Username";
            this.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(65, 51);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(147, 25);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "Password";
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(101, 84);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(284, 151);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtUsername);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.btnLogin);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(276, 125);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Login";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtRUsername);
            this.tabPage2.Controls.Add(this.txtRKey);
            this.tabPage2.Controls.Add(this.txtRPassword);
            this.tabPage2.Controls.Add(this.btnRegister);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(276, 125);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Register";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtRUsername
            // 
            this.txtRUsername.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRUsername.Location = new System.Drawing.Point(65, 4);
            this.txtRUsername.Name = "txtRUsername";
            this.txtRUsername.Size = new System.Drawing.Size(147, 25);
            this.txtRUsername.TabIndex = 4;
            this.txtRUsername.Text = "Username";
            this.txtRUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRKey
            // 
            this.txtRKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRKey.Location = new System.Drawing.Point(65, 66);
            this.txtRKey.Name = "txtRKey";
            this.txtRKey.Size = new System.Drawing.Size(147, 25);
            this.txtRKey.TabIndex = 7;
            this.txtRKey.Text = "Key";
            this.txtRKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRPassword
            // 
            this.txtRPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRPassword.Location = new System.Drawing.Point(65, 35);
            this.txtRPassword.Name = "txtRPassword";
            this.txtRPassword.PasswordChar = '*';
            this.txtRPassword.Size = new System.Drawing.Size(147, 25);
            this.txtRPassword.TabIndex = 5;
            this.txtRPassword.Text = "Password";
            this.txtRPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(101, 97);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(75, 23);
            this.btnRegister.TabIndex = 6;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtActivateKey);
            this.tabPage3.Controls.Add(this.btnActivate);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(276, 125);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Activate Key";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hypothetically, a client registered, then his/her \r\nsubscription ran out, they ca" +
    "n still log in, but\r\nare barred from using functionality.";
            // 
            // txtActivateKey
            // 
            this.txtActivateKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtActivateKey.Location = new System.Drawing.Point(62, 58);
            this.txtActivateKey.Name = "txtActivateKey";
            this.txtActivateKey.Size = new System.Drawing.Size(147, 25);
            this.txtActivateKey.TabIndex = 9;
            this.txtActivateKey.Text = "Key";
            this.txtActivateKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(98, 91);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(75, 23);
            this.btnActivate.TabIndex = 8;
            this.btnActivate.Text = "Activate Key";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 151);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "NaLP - Login";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtRUsername;
        private System.Windows.Forms.TextBox txtRKey;
        private System.Windows.Forms.TextBox txtRPassword;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtActivateKey;
        private System.Windows.Forms.Button btnActivate;
    }
}

