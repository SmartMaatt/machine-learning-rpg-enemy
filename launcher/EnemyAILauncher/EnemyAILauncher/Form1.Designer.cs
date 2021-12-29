namespace EnemyAILauncher
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.ModeComboBox = new System.Windows.Forms.ComboBox();
            this.MapComboBox = new System.Windows.Forms.ComboBox();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.ProfileTextBox = new System.Windows.Forms.TextBox();
            this.ProfileComboBox = new System.Windows.Forms.ComboBox();
            this.NewProfileCheckBox = new System.Windows.Forms.CheckBox();
            this.ProfileGroupBox = new System.Windows.Forms.GroupBox();
            this.DeleteProfileBtn = new System.Windows.Forms.Button();
            this.OpenProfileFolderBtn = new System.Windows.Forms.Button();
            this.ModeGroupBox = new System.Windows.Forms.GroupBox();
            this.SubmitBtn = new System.Windows.Forms.Button();
            this.MapGroupBox = new System.Windows.Forms.GroupBox();
            this.ProfileGroupBox.SuspendLayout();
            this.ModeGroupBox.SuspendLayout();
            this.MapGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ModeComboBox
            // 
            this.ModeComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeComboBox.FormattingEnabled = true;
            this.ModeComboBox.Items.AddRange(new object[] {
            "Trening z graczem",
            "Trening dwóch robotów",
            "Rozgrywka z graczem",
            "Rozgrywka dwóch robotów"});
            this.ModeComboBox.Location = new System.Drawing.Point(10, 25);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(392, 24);
            this.ModeComboBox.TabIndex = 0;
            // 
            // MapComboBox
            // 
            this.MapComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MapComboBox.FormattingEnabled = true;
            this.MapComboBox.Items.AddRange(new object[] {
            "Trening",
            "Las"});
            this.MapComboBox.Location = new System.Drawing.Point(10, 25);
            this.MapComboBox.Name = "MapComboBox";
            this.MapComboBox.Size = new System.Drawing.Size(392, 24);
            this.MapComboBox.TabIndex = 0;
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.InfoLabel.Font = new System.Drawing.Font("Arial", 8F);
            this.InfoLabel.ForeColor = System.Drawing.Color.Red;
            this.InfoLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.InfoLabel.Location = new System.Drawing.Point(10, 427);
            this.InfoLabel.MaximumSize = new System.Drawing.Size(412, 0);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(34, 16);
            this.InfoLabel.TabIndex = 5;
            this.InfoLabel.Text = "Test";
            // 
            // ProfileTextBox
            // 
            this.ProfileTextBox.Location = new System.Drawing.Point(10, 56);
            this.ProfileTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.ProfileTextBox.MaxLength = 20;
            this.ProfileTextBox.Name = "ProfileTextBox";
            this.ProfileTextBox.Size = new System.Drawing.Size(392, 22);
            this.ProfileTextBox.TabIndex = 1;
            this.ProfileTextBox.Visible = false;
            // 
            // ProfileComboBox
            // 
            this.ProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProfileComboBox.FormattingEnabled = true;
            this.ProfileComboBox.Location = new System.Drawing.Point(10, 56);
            this.ProfileComboBox.Name = "ProfileComboBox";
            this.ProfileComboBox.Size = new System.Drawing.Size(392, 24);
            this.ProfileComboBox.TabIndex = 2;
            // 
            // NewProfileCheckBox
            // 
            this.NewProfileCheckBox.AutoSize = true;
            this.NewProfileCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.NewProfileCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.NewProfileCheckBox.Location = new System.Drawing.Point(10, 25);
            this.NewProfileCheckBox.Name = "NewProfileCheckBox";
            this.NewProfileCheckBox.Size = new System.Drawing.Size(392, 21);
            this.NewProfileCheckBox.TabIndex = 3;
            this.NewProfileCheckBox.Text = "Nowy Profil";
            this.NewProfileCheckBox.UseVisualStyleBackColor = true;
            this.NewProfileCheckBox.CheckedChanged += new System.EventHandler(this.NewProfileCheckBox_CheckedChanged);
            // 
            // ProfileGroupBox
            // 
            this.ProfileGroupBox.Controls.Add(this.DeleteProfileBtn);
            this.ProfileGroupBox.Controls.Add(this.OpenProfileFolderBtn);
            this.ProfileGroupBox.Controls.Add(this.NewProfileCheckBox);
            this.ProfileGroupBox.Controls.Add(this.ProfileComboBox);
            this.ProfileGroupBox.Controls.Add(this.ProfileTextBox);
            this.ProfileGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ProfileGroupBox.Location = new System.Drawing.Point(10, 80);
            this.ProfileGroupBox.Margin = new System.Windows.Forms.Padding(0);
            this.ProfileGroupBox.Name = "ProfileGroupBox";
            this.ProfileGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.ProfileGroupBox.Size = new System.Drawing.Size(412, 140);
            this.ProfileGroupBox.TabIndex = 3;
            this.ProfileGroupBox.TabStop = false;
            this.ProfileGroupBox.Text = "2.Wybierz model";
            // 
            // DeleteProfileBtn
            // 
            this.DeleteProfileBtn.Location = new System.Drawing.Point(90, 90);
            this.DeleteProfileBtn.Name = "DeleteProfileBtn";
            this.DeleteProfileBtn.Size = new System.Drawing.Size(75, 30);
            this.DeleteProfileBtn.TabIndex = 5;
            this.DeleteProfileBtn.Text = "Usuń";
            this.DeleteProfileBtn.UseVisualStyleBackColor = true;
            // 
            // OpenProfileFolderBtn
            // 
            this.OpenProfileFolderBtn.Location = new System.Drawing.Point(10, 90);
            this.OpenProfileFolderBtn.Name = "OpenProfileFolderBtn";
            this.OpenProfileFolderBtn.Size = new System.Drawing.Size(75, 30);
            this.OpenProfileFolderBtn.TabIndex = 4;
            this.OpenProfileFolderBtn.Text = "Folder";
            this.OpenProfileFolderBtn.UseVisualStyleBackColor = true;
            // 
            // ModeGroupBox
            // 
            this.ModeGroupBox.Controls.Add(this.ModeComboBox);
            this.ModeGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ModeGroupBox.Location = new System.Drawing.Point(10, 10);
            this.ModeGroupBox.Name = "ModeGroupBox";
            this.ModeGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.ModeGroupBox.Size = new System.Drawing.Size(412, 70);
            this.ModeGroupBox.TabIndex = 2;
            this.ModeGroupBox.TabStop = false;
            this.ModeGroupBox.Text = "1.Wybierz tryb";
            // 
            // SubmitBtn
            // 
            this.SubmitBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SubmitBtn.Location = new System.Drawing.Point(322, 292);
            this.SubmitBtn.Name = "SubmitBtn";
            this.SubmitBtn.Size = new System.Drawing.Size(100, 45);
            this.SubmitBtn.TabIndex = 4;
            this.SubmitBtn.Text = "Rozpocznij";
            this.SubmitBtn.UseVisualStyleBackColor = true;
            // 
            // MapGroupBox
            // 
            this.MapGroupBox.Controls.Add(this.MapComboBox);
            this.MapGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.MapGroupBox.Location = new System.Drawing.Point(10, 220);
            this.MapGroupBox.Name = "MapGroupBox";
            this.MapGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.MapGroupBox.Size = new System.Drawing.Size(412, 69);
            this.MapGroupBox.TabIndex = 6;
            this.MapGroupBox.TabStop = false;
            this.MapGroupBox.Text = "3.Wybierz mapę";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 453);
            this.Controls.Add(this.MapGroupBox);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.SubmitBtn);
            this.Controls.Add(this.ProfileGroupBox);
            this.Controls.Add(this.ModeGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.Text = "EnemyAI Launcher";
            this.ProfileGroupBox.ResumeLayout(false);
            this.ProfileGroupBox.PerformLayout();
            this.ModeGroupBox.ResumeLayout(false);
            this.MapGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.TextBox ProfileTextBox;
        private System.Windows.Forms.ComboBox ProfileComboBox;
        private System.Windows.Forms.CheckBox NewProfileCheckBox;
        private System.Windows.Forms.GroupBox ProfileGroupBox;
        private System.Windows.Forms.GroupBox ModeGroupBox;
        private System.Windows.Forms.Button SubmitBtn;
        private System.Windows.Forms.Button DeleteProfileBtn;
        private System.Windows.Forms.Button OpenProfileFolderBtn;
        private System.Windows.Forms.GroupBox MapGroupBox;
        private System.Windows.Forms.ComboBox ModeComboBox;
        private System.Windows.Forms.ComboBox MapComboBox;
    }
}

