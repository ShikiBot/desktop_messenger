namespace _4yatClient
{
    partial class registration
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.name = new System.Windows.Forms.TextBox();
            this.pas1 = new System.Windows.Forms.TextBox();
            this.nicname = new System.Windows.Forms.Label();
            this.passwd = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.pas1Chk = new System.Windows.Forms.Label();
            this.pas2Chk = new System.Windows.Forms.Label();
            this.pas2 = new System.Windows.Forms.TextBox();
            this.passwd2 = new System.Windows.Forms.Label();
            this.ipp = new System.Windows.Forms.TextBox();
            this.server = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.name.Location = new System.Drawing.Point(68, 70);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(275, 38);
            this.name.TabIndex = 0;
            // 
            // pas1
            // 
            this.pas1.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pas1.Location = new System.Drawing.Point(403, 215);
            this.pas1.Name = "pas1";
            this.pas1.PasswordChar = '*';
            this.pas1.Size = new System.Drawing.Size(275, 38);
            this.pas1.TabIndex = 1;
            this.pas1.TextChanged += new System.EventHandler(this.pas1_TextChanged);
            // 
            // nicname
            // 
            this.nicname.AutoSize = true;
            this.nicname.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nicname.Location = new System.Drawing.Point(158, 41);
            this.nicname.Name = "nicname";
            this.nicname.Size = new System.Drawing.Size(70, 26);
            this.nicname.TabIndex = 2;
            this.nicname.Text = "label1";
            // 
            // passwd
            // 
            this.passwd.AutoSize = true;
            this.passwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.passwd.Location = new System.Drawing.Point(180, 108);
            this.passwd.Name = "passwd";
            this.passwd.Size = new System.Drawing.Size(70, 26);
            this.passwd.TabIndex = 3;
            this.passwd.Text = "label2";
            // 
            // ok
            // 
            this.ok.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ok.Location = new System.Drawing.Point(451, 339);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(127, 53);
            this.ok.TabIndex = 6;
            this.ok.Text = "button1";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // pas1Chk
            // 
            this.pas1Chk.AutoSize = true;
            this.pas1Chk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pas1Chk.Location = new System.Drawing.Point(12, 9);
            this.pas1Chk.Name = "pas1Chk";
            this.pas1Chk.Size = new System.Drawing.Size(18, 20);
            this.pas1Chk.TabIndex = 7;
            this.pas1Chk.Text = "x";
            // 
            // pas2Chk
            // 
            this.pas2Chk.AutoSize = true;
            this.pas2Chk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pas2Chk.Location = new System.Drawing.Point(31, 9);
            this.pas2Chk.Name = "pas2Chk";
            this.pas2Chk.Size = new System.Drawing.Size(26, 20);
            this.pas2Chk.TabIndex = 8;
            this.pas2Chk.Text = "✔";
            // 
            // pas2
            // 
            this.pas2.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pas2.Location = new System.Drawing.Point(403, 253);
            this.pas2.Name = "pas2";
            this.pas2.Size = new System.Drawing.Size(275, 38);
            this.pas2.TabIndex = 4;
            this.pas2.TextChanged += new System.EventHandler(this.pas2_TextChanged);
            // 
            // passwd2
            // 
            this.passwd2.AutoSize = true;
            this.passwd2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.passwd2.Location = new System.Drawing.Point(180, 173);
            this.passwd2.Name = "passwd2";
            this.passwd2.Size = new System.Drawing.Size(70, 26);
            this.passwd2.TabIndex = 5;
            this.passwd2.Text = "label2";
            // 
            // ipp
            // 
            this.ipp.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ipp.Location = new System.Drawing.Point(77, 367);
            this.ipp.Name = "ipp";
            this.ipp.Size = new System.Drawing.Size(275, 38);
            this.ipp.TabIndex = 18;
            // 
            // server
            // 
            this.server.AutoSize = true;
            this.server.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.server.Location = new System.Drawing.Point(114, 339);
            this.server.Name = "server";
            this.server.Size = new System.Drawing.Size(70, 26);
            this.server.TabIndex = 17;
            this.server.Text = "label2";
            // 
            // registration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ipp);
            this.Controls.Add(this.server);
            this.Controls.Add(this.pas2Chk);
            this.Controls.Add(this.pas1Chk);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.passwd2);
            this.Controls.Add(this.pas2);
            this.Controls.Add(this.passwd);
            this.Controls.Add(this.nicname);
            this.Controls.Add(this.pas1);
            this.Controls.Add(this.name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "registration";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox pas1;
        private System.Windows.Forms.Label nicname;
        private System.Windows.Forms.Label passwd;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label pas1Chk;
        private System.Windows.Forms.Label pas2Chk;
        private System.Windows.Forms.TextBox pas2;
        private System.Windows.Forms.Label passwd2;
        private System.Windows.Forms.TextBox ipp;
        private System.Windows.Forms.Label server;
    }
}

