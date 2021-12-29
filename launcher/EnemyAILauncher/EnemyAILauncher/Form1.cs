using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace EnemyAILauncher
{
    public partial class Form1 : Form
    {
        private string modelsDirectoryName = "results";
        private string modelsDirectoryPath = null;

        private string noProfile = "Brak wytrenowanych model!";
        private string brainName = "enemyAI.onnx";

        public Form1()
        {
            InitializeComponent();
            ModelsDirectoryCheck();
            ComboBoxSetup();
        }

        private void NewProfileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ProfileComboBox.Visible = !NewProfileCheckBox.Checked;
            DeleteProfileBtn.Visible = !NewProfileCheckBox.Checked;
            OpenProfileFolderBtn.Visible = !NewProfileCheckBox.Checked;
            ProfileTextBox.Visible = NewProfileCheckBox.Checked;

            if(NewProfileCheckBox.Checked)
            {
                ProfileGroupBox.Height = 80;
            }
            else
            {
                ProfileGroupBox.Height = 110;
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    //ProcessStartInfo pro = new ProcessStartInfo();
        //    //pro.FileName = "cmd.exe";
        //    //Process proStart = new Process();
        //    //proStart.StartInfo = pro;
        //    //proStart.Start();

        //    var startInfo = new ProcessStartInfo
        //    {
        //        FileName = "cmd.exe",
        //        Arguments = @"/k venv\Scripts\activate&mlagents-learn --run-id=test"
        //    };

        //    var process = new Process { StartInfo = startInfo };

        //    process.Start();
        //    //process.StandardInput.WriteLine(@"echo dupa");
        //}
    
        private void ModelsDirectoryCheck()
        {
            modelsDirectoryPath = Path.Combine(Environment.CurrentDirectory, modelsDirectoryName);
            Directory.CreateDirectory(modelsDirectoryPath);
        }

        private void ComboBoxSetup()
        {
            ModeComboBox.SelectedIndex = 0;
            MapComboBox.SelectedIndex = 0;
            GetBrainModelProfiles();
        }

        private void GetBrainModelProfiles()
        {
            List<string> tmpBrainProfilesList = GetDirectories(modelsDirectoryPath);

            if (tmpBrainProfilesList.Count != 0)
            {
                foreach (string profile in tmpBrainProfilesList)
                {
                    if (File.Exists(Path.Combine(modelsDirectoryPath, profile, brainName)))
                    {
                        ProfileComboBox.Items.Add(new DirectoryInfo(profile).Name);
                    }
                }
            }
            else
            {
                ProfileComboBox.Items.Add(noProfile);
            }
            ProfileComboBox.SelectedIndex = 0;
        }

        private List<string> GetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
        }

    }
}
