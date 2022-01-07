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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ModeComboBox = new System.Windows.Forms.ComboBox();
            this.MapComboBox = new System.Windows.Forms.ComboBox();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.ProfileTextBox = new System.Windows.Forms.TextBox();
            this.ProfileComboBox = new System.Windows.Forms.ComboBox();
            this.NewProfileCheckBox = new System.Windows.Forms.CheckBox();
            this.ProfileGroupBox = new System.Windows.Forms.GroupBox();
            this.DiagnosticsBtn = new System.Windows.Forms.Button();
            this.DeleteProfileBtn = new System.Windows.Forms.Button();
            this.OpenProfileFolderBtn = new System.Windows.Forms.Button();
            this.ModeGroupBox = new System.Windows.Forms.GroupBox();
            this.SubmitBtn = new System.Windows.Forms.Button();
            this.MapGroupBox = new System.Windows.Forms.GroupBox();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.BuildVenvBtn = new System.Windows.Forms.Button();
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
            this.ModeComboBox.Location = new System.Drawing.Point(11, 25);
            this.ModeComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(388, 24);
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
            this.MapComboBox.Location = new System.Drawing.Point(11, 25);
            this.MapComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MapComboBox.Name = "MapComboBox";
            this.MapComboBox.Size = new System.Drawing.Size(388, 24);
            this.MapComboBox.TabIndex = 0;
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.InfoLabel.Font = new System.Drawing.Font("Arial", 8F);
            this.InfoLabel.ForeColor = System.Drawing.Color.Red;
            this.InfoLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.InfoLabel.Location = new System.Drawing.Point(11, 377);
            this.InfoLabel.MaximumSize = new System.Drawing.Size(412, 0);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 16);
            this.InfoLabel.TabIndex = 5;
            // 
            // ProfileTextBox
            // 
            this.ProfileTextBox.Location = new System.Drawing.Point(11, 57);
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
            this.ProfileComboBox.Location = new System.Drawing.Point(11, 55);
            this.ProfileComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ProfileComboBox.Name = "ProfileComboBox";
            this.ProfileComboBox.Size = new System.Drawing.Size(392, 24);
            this.ProfileComboBox.TabIndex = 2;
            // 
            // NewProfileCheckBox
            // 
            this.NewProfileCheckBox.AutoSize = true;
            this.NewProfileCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.NewProfileCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.NewProfileCheckBox.Location = new System.Drawing.Point(11, 25);
            this.NewProfileCheckBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NewProfileCheckBox.Name = "NewProfileCheckBox";
            this.NewProfileCheckBox.Size = new System.Drawing.Size(388, 21);
            this.NewProfileCheckBox.TabIndex = 3;
            this.NewProfileCheckBox.Text = "Nowy Profil";
            this.NewProfileCheckBox.UseVisualStyleBackColor = true;
            this.NewProfileCheckBox.CheckedChanged += new System.EventHandler(this.NewProfileCheckBox_CheckedChanged);
            // 
            // ProfileGroupBox
            // 
            this.ProfileGroupBox.Controls.Add(this.DiagnosticsBtn);
            this.ProfileGroupBox.Controls.Add(this.DeleteProfileBtn);
            this.ProfileGroupBox.Controls.Add(this.OpenProfileFolderBtn);
            this.ProfileGroupBox.Controls.Add(this.NewProfileCheckBox);
            this.ProfileGroupBox.Controls.Add(this.ProfileComboBox);
            this.ProfileGroupBox.Controls.Add(this.ProfileTextBox);
            this.ProfileGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ProfileGroupBox.Location = new System.Drawing.Point(11, 80);
            this.ProfileGroupBox.Margin = new System.Windows.Forms.Padding(0);
            this.ProfileGroupBox.Name = "ProfileGroupBox";
            this.ProfileGroupBox.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.ProfileGroupBox.Size = new System.Drawing.Size(410, 140);
            this.ProfileGroupBox.TabIndex = 3;
            this.ProfileGroupBox.TabStop = false;
            this.ProfileGroupBox.Text = "2.Wybierz model";
            // 
            // DiagnosticsBtn
            // 
            this.DiagnosticsBtn.Location = new System.Drawing.Point(122, 90);
            this.DiagnosticsBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DiagnosticsBtn.Name = "DiagnosticsBtn";
            this.DiagnosticsBtn.Size = new System.Drawing.Size(100, 30);
            this.DiagnosticsBtn.TabIndex = 6;
            this.DiagnosticsBtn.Text = "Statystyka";
            this.DiagnosticsBtn.UseVisualStyleBackColor = true;
            this.DiagnosticsBtn.Click += new System.EventHandler(this.DiagnosticsBtn_Click);
            // 
            // DeleteProfileBtn
            // 
            this.DeleteProfileBtn.Location = new System.Drawing.Point(228, 90);
            this.DeleteProfileBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DeleteProfileBtn.Name = "DeleteProfileBtn";
            this.DeleteProfileBtn.Size = new System.Drawing.Size(75, 30);
            this.DeleteProfileBtn.TabIndex = 5;
            this.DeleteProfileBtn.Text = "Usuń";
            this.DeleteProfileBtn.UseVisualStyleBackColor = true;
            this.DeleteProfileBtn.Click += new System.EventHandler(this.DeleteProfileBtn_Click);
            // 
            // OpenProfileFolderBtn
            // 
            this.OpenProfileFolderBtn.Location = new System.Drawing.Point(11, 90);
            this.OpenProfileFolderBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OpenProfileFolderBtn.Name = "OpenProfileFolderBtn";
            this.OpenProfileFolderBtn.Size = new System.Drawing.Size(105, 30);
            this.OpenProfileFolderBtn.TabIndex = 4;
            this.OpenProfileFolderBtn.Text = "Otwórz folder";
            this.OpenProfileFolderBtn.UseVisualStyleBackColor = true;
            this.OpenProfileFolderBtn.Click += new System.EventHandler(this.OpenProfileFolderBtn_Click);
            // 
            // ModeGroupBox
            // 
            this.ModeGroupBox.Controls.Add(this.ModeComboBox);
            this.ModeGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ModeGroupBox.Location = new System.Drawing.Point(11, 10);
            this.ModeGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ModeGroupBox.Name = "ModeGroupBox";
            this.ModeGroupBox.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.ModeGroupBox.Size = new System.Drawing.Size(410, 70);
            this.ModeGroupBox.TabIndex = 2;
            this.ModeGroupBox.TabStop = false;
            this.ModeGroupBox.Text = "1.Wybierz tryb";
            // 
            // SubmitBtn
            // 
            this.SubmitBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SubmitBtn.Location = new System.Drawing.Point(323, 292);
            this.SubmitBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SubmitBtn.Name = "SubmitBtn";
            this.SubmitBtn.Size = new System.Drawing.Size(100, 46);
            this.SubmitBtn.TabIndex = 4;
            this.SubmitBtn.Text = "Rozpocznij";
            this.SubmitBtn.UseVisualStyleBackColor = true;
            this.SubmitBtn.Click += new System.EventHandler(this.SubmitBtn_Click);
            // 
            // MapGroupBox
            // 
            this.MapGroupBox.Controls.Add(this.MapComboBox);
            this.MapGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.MapGroupBox.Location = new System.Drawing.Point(11, 220);
            this.MapGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MapGroupBox.Name = "MapGroupBox";
            this.MapGroupBox.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.MapGroupBox.Size = new System.Drawing.Size(410, 69);
            this.MapGroupBox.TabIndex = 6;
            this.MapGroupBox.TabStop = false;
            this.MapGroupBox.Text = "3.Wybierz mapę";
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RefreshBtn.Location = new System.Drawing.Point(217, 292);
            this.RefreshBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(100, 46);
            this.RefreshBtn.TabIndex = 7;
            this.RefreshBtn.Text = "Odśwież";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // BuildVenvBtn
            // 
            this.BuildVenvBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BuildVenvBtn.Location = new System.Drawing.Point(95, 292);
            this.BuildVenvBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BuildVenvBtn.Name = "BuildVenvBtn";
            this.BuildVenvBtn.Size = new System.Drawing.Size(116, 46);
            this.BuildVenvBtn.TabIndex = 8;
            this.BuildVenvBtn.Text = "Utwórz Venv";
            this.BuildVenvBtn.UseVisualStyleBackColor = true;
            this.BuildVenvBtn.Click += new System.EventHandler(this.BuildVenvBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 403);
            this.Controls.Add(this.BuildVenvBtn);
            this.Controls.Add(this.RefreshBtn);
            this.Controls.Add(this.MapGroupBox);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.SubmitBtn);
            this.Controls.Add(this.ProfileGroupBox);
            this.Controls.Add(this.ModeGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
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
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.Button DiagnosticsBtn;
        private System.Windows.Forms.Button BuildVenvBtn;
    }
}

