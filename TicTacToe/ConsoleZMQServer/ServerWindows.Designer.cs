namespace ConsoleZMQServer
{
    partial class ServerWindows
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
            this.buttonRunServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOffServer = new System.Windows.Forms.Button();
            this.consoleBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxO = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRunServer
            // 
            this.buttonRunServer.Location = new System.Drawing.Point(52, 53);
            this.buttonRunServer.Name = "buttonRunServer";
            this.buttonRunServer.Size = new System.Drawing.Size(141, 48);
            this.buttonRunServer.TabIndex = 0;
            this.buttonRunServer.Text = "Запустить";
            this.buttonRunServer.UseVisualStyleBackColor = true;
            this.buttonRunServer.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Нажмите для старта";
            // 
            // buttonOffServer
            // 
            this.buttonOffServer.Location = new System.Drawing.Point(671, 23);
            this.buttonOffServer.Name = "buttonOffServer";
            this.buttonOffServer.Size = new System.Drawing.Size(98, 48);
            this.buttonOffServer.TabIndex = 2;
            this.buttonOffServer.Text = "Отключить";
            this.buttonOffServer.UseVisualStyleBackColor = true;
            this.buttonOffServer.Click += new System.EventHandler(this.buttonOffServer_Click);
            // 
            // consoleBox
            // 
            this.consoleBox.Location = new System.Drawing.Point(52, 144);
            this.consoleBox.Multiline = true;
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.Size = new System.Drawing.Size(377, 143);
            this.consoleBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Логи";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(775, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 49);
            this.button1.TabIndex = 5;
            this.button1.Text = "Закрыть";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Крестики:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Нолики:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxO);
            this.groupBox1.Controls.Add(this.textBoxX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(626, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 133);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Текущая игра";
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(87, 37);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(153, 22);
            this.textBoxX.TabIndex = 8;
            // 
            // textBoxO
            // 
            this.textBoxO.Location = new System.Drawing.Point(87, 66);
            this.textBoxO.Name = "textBoxO";
            this.textBoxO.Size = new System.Drawing.Size(153, 22);
            this.textBoxO.TabIndex = 9;
            // 
            // ServerWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 439);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.consoleBox);
            this.Controls.Add(this.buttonOffServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRunServer);
            this.Name = "ServerWindows";
            this.Text = "ServerWindows";
            this.Load += new System.EventHandler(this.ServerWindows_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRunServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOffServer;
        private System.Windows.Forms.TextBox consoleBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxO;
        private System.Windows.Forms.TextBox textBoxX;
    }
}