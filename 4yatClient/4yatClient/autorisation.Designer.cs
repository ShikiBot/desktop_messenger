namespace _4yatClient
{
    partial class autorisation
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
            this.ok = new System.Windows.Forms.Button();
            this.passwd = new System.Windows.Forms.Label();
            this.nicname = new System.Windows.Forms.Label();
            this.pas1 = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.ok2 = new System.Windows.Forms.Button();
            this.server = new System.Windows.Forms.Label();
            this.ipp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ok.Location = new System.Drawing.Point(478, 348);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(127, 53);
            this.ok.TabIndex = 20;
            this.ok.Text = "button1";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // passwd
            // 
            this.passwd.AutoSize = true;
            this.passwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.passwd.Location = new System.Drawing.Point(207, 117);
            this.passwd.Name = "passwd";
            this.passwd.Size = new System.Drawing.Size(70, 26);
            this.passwd.TabIndex = 10;
            this.passwd.Text = "label2";
            // 
            // nicname
            // 
            this.nicname.AutoSize = true;
            this.nicname.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nicname.Location = new System.Drawing.Point(185, 50);
            this.nicname.Name = "nicname";
            this.nicname.Size = new System.Drawing.Size(70, 26);
            this.nicname.TabIndex = 9;
            this.nicname.Text = "label1";
            // 
            // pas1
            // 
            this.pas1.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pas1.Location = new System.Drawing.Point(173, 146);
            this.pas1.Name = "pas1";
            this.pas1.PasswordChar = '*';
            this.pas1.Size = new System.Drawing.Size(275, 38);
            this.pas1.TabIndex = 8;
            // 
            // name
            // 
            this.name.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.name.Location = new System.Drawing.Point(95, 79);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(275, 38);
            this.name.TabIndex = 7;
            // 
            // ok2
            // 
            this.ok2.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ok2.Location = new System.Drawing.Point(305, 268);
            this.ok2.Name = "ok2";
            this.ok2.Size = new System.Drawing.Size(127, 53);
            this.ok2.TabIndex = 30;
            this.ok2.Text = "button1";
            this.ok2.UseVisualStyleBackColor = true;
            this.ok2.Click += new System.EventHandler(this.ok2_Click);
            // 
            // server
            // 
            this.server.AutoSize = true;
            this.server.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.server.Location = new System.Drawing.Point(300, 178);
            this.server.Name = "server";
            this.server.Size = new System.Drawing.Size(70, 26);
            this.server.TabIndex = 15;
            this.server.Text = "label2";
            // 
            // ipp
            // 
            this.ipp.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ipp.Location = new System.Drawing.Point(263, 206);
            this.ipp.Name = "ipp";
            this.ipp.Size = new System.Drawing.Size(275, 38);
            this.ipp.TabIndex = 16;
            // 
            // autorisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ipp);
            this.Controls.Add(this.server);
            this.Controls.Add(this.ok2);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.passwd);
            this.Controls.Add(this.nicname);
            this.Controls.Add(this.pas1);
            this.Controls.Add(this.name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "autorisation";
            this.Text = "autorisation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label passwd;
        private System.Windows.Forms.Label nicname;
        private System.Windows.Forms.TextBox pas1;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Button ok2;
        private System.Windows.Forms.Label server;
        private System.Windows.Forms.TextBox ipp;
    }
}