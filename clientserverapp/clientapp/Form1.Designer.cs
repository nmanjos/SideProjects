namespace clientapp
{
    partial class Form1
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
            this.rtxConsola = new System.Windows.Forms.RichTextBox();
            this.btSend = new System.Windows.Forms.Button();
            this.sendmessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rtxConsola
            // 
            this.rtxConsola.Location = new System.Drawing.Point(12, 62);
            this.rtxConsola.Name = "rtxConsola";
            this.rtxConsola.Size = new System.Drawing.Size(260, 187);
            this.rtxConsola.TabIndex = 0;
            this.rtxConsola.Text = "";
            // 
            // btSend
            // 
            this.btSend.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSend.Location = new System.Drawing.Point(197, 12);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(75, 44);
            this.btSend.TabIndex = 1;
            this.btSend.Text = "Send Request";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // sendmessage
            // 
            this.sendmessage.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.sendmessage.Location = new System.Drawing.Point(12, 12);
            this.sendmessage.Name = "sendmessage";
            this.sendmessage.Size = new System.Drawing.Size(179, 20);
            this.sendmessage.TabIndex = 2;
            this.sendmessage.TextChanged += new System.EventHandler(this.sendmessage_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.sendmessage);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.rtxConsola);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxConsola;
        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.TextBox sendmessage;
    }
}

