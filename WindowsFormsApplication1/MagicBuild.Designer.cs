namespace WindowsFormsApplication1
{
    partial class MagicBuild
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
            this.Letuchka = new System.Windows.Forms.Button();
            this.ProgrammText = new System.Windows.Forms.TextBox();
            this.Tokens = new System.Windows.Forms.TextBox();
            this.Kurva_perdole = new System.Windows.Forms.TextBox();
            this.Output = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Letuchka
            // 
            this.Letuchka.Location = new System.Drawing.Point(172, 131);
            this.Letuchka.Name = "Letuchka";
            this.Letuchka.Size = new System.Drawing.Size(211, 43);
            this.Letuchka.TabIndex = 0;
            this.Letuchka.Text = "Щелк!";
            this.Letuchka.UseVisualStyleBackColor = true;
            this.Letuchka.Click += new System.EventHandler(this.Ckick);
            // 
            // ProgrammText
            // 
            this.ProgrammText.Location = new System.Drawing.Point(21, 27);
            this.ProgrammText.Name = "ProgrammText";
            this.ProgrammText.Size = new System.Drawing.Size(225, 20);
            this.ProgrammText.TabIndex = 1;
            // 
            // Tokens
            // 
            this.Tokens.Location = new System.Drawing.Point(21, 73);
            this.Tokens.Name = "Tokens";
            this.Tokens.Size = new System.Drawing.Size(225, 20);
            this.Tokens.TabIndex = 2;
            // 
            // Kurva_perdole
            // 
            this.Kurva_perdole.Location = new System.Drawing.Point(305, 27);
            this.Kurva_perdole.Name = "Kurva_perdole";
            this.Kurva_perdole.Size = new System.Drawing.Size(225, 20);
            this.Kurva_perdole.TabIndex = 3;
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(305, 73);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(225, 20);
            this.Output.TabIndex = 4;
            // 
            // MagicBuild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 186);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Kurva_perdole);
            this.Controls.Add(this.Tokens);
            this.Controls.Add(this.ProgrammText);
            this.Controls.Add(this.Letuchka);
            this.Name = "MagicBuild";
            this.Text = "Магическое Окно";
            this.Load += new System.EventHandler(this.MagicBuild_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Letuchka;
        private System.Windows.Forms.TextBox ProgrammText;
        private System.Windows.Forms.TextBox Tokens;
        private System.Windows.Forms.TextBox Kurva_perdole;
        private System.Windows.Forms.TextBox Output;
    }
}

