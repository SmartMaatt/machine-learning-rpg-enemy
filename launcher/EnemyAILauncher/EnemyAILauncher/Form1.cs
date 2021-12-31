using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EnemyAILauncher
{
    public partial class Form1 : Form
    {
        private string modelsDirectoryName = "results";
        private string modelsDirectoryPath = null;

        private string noProfile = "Brak wytrenowanych model!";
        private string emptyProfileName = "Nazwa nowego profilu nie może być pusta!";
        private string nameDoesntMatchTemplate = "Nazwa profilu może posiadać tylko duże i małe litery oraz cyfry!";
        private string brainName = "enemyAI.onnx";

        private string configFile = "level.conf";
        private string[] modesNames = { "Training", "SelfPlayTraining", "Play", "SelfPlay" };
        private string[] mapsNames = { "Training", "Forest" };

        private Regex newProfileTemplate = new Regex(@"^\w+$");

        public Form1()
        {
            InitializeComponent();
            ModelsDirectoryCheck();
            ComboBoxSetup();
        }

        private void NewProfileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ProfilePanelSwitch(NewProfileCheckBox.Checked);
        }

        private void ProfilePanelSwitch(bool newProfile)
        {
            NewProfileCheckBox.Checked = newProfile;
            ProfileComboBox.Visible = !newProfile;
            DeleteProfileBtn.Visible = !newProfile;
            OpenProfileFolderBtn.Visible = !newProfile;
            DiagnosticsBtn.Visible = !newProfile;
            ProfileTextBox.Visible = newProfile;

            if (newProfile)
            {
                ProfileGroupBox.Height = 80;
            }
            else
            {
                ProfileGroupBox.Height = 110;
            }
        }

        private void OpenProfileFolderBtn_Click(object sender, EventArgs e)
        {
            if (ProfileComboBox.Text != noProfile)
            {
                Process.Start("explorer.exe", Path.Combine(modelsDirectoryPath, ProfileComboBox.Text));
            }
            else
            {
                SetInfo("Brak profilu do wyświetlenia!");
            }
        }

        private void DiagnosticsBtn_Click(object sender, EventArgs e)
        {
            if (ProfileComboBox.Text != noProfile)
            {
                ExecuteProgram("cmd.exe", "venv\\Scripts\\activate&tensorboard --logdir results --port 6006");
                ExecuteProgram("http://localhost:6006", "");
            }
            else
            {
                SetInfo("Brak profilu do wyświetlenia!");
            }
        }

        private void DeleteProfileBtn_Click(object sender, EventArgs e)
        {
            if (ProfileComboBox.Text != noProfile)
            {
                DialogResult dialogResult = MessageBox.Show($"Chcesz usunąć profil {ProfileComboBox.Text}?", "Usuwanie profilu", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DeleteDirectory(Path.Combine(modelsDirectoryPath, ProfileComboBox.Text));
                    DeleteProfileFromComboBox(ProfileComboBox.SelectedIndex);
                }
            }
            else
            {
                SetInfo("Brak profilu do usunięcia!");
            }
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            ComboBoxSetup();
            ProfilePanelSwitch(false);
            ProfileTextBox.Text = "";
            SetInfo("");
        }

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            AppModes mode = (AppModes)ModeComboBox.SelectedIndex;
            string profileName = GetProfileName();
            Maps map = (Maps)MapComboBox.SelectedIndex;
            bool newProfle = NewProfileCheckBox.Checked;

            if (profileName == noProfile)
            {
                SetInfo("Brak profilu do odtworzenia!");
                return;
            }

            if (profileName == nameDoesntMatchTemplate)
            {
                SetInfo(nameDoesntMatchTemplate);
                return;
            }

            if (profileName == emptyProfileName)
            {
                SetInfo(emptyProfileName);
                return;
            }

            if (mode == AppModes.PLAYER_TRAINING || mode == AppModes.SELF_PLAY_TRAINING)
            {
                string venv = "venv\\Scripts\\activate";
                string pythonCommand = "mlagents-learn";
                int timeScale = 0;

                if (mode == AppModes.PLAYER_TRAINING)
                {
                    pythonCommand += " Config/enemyAIPlayerTraining.yaml";
                    timeScale = 1;
                }
                else if (mode == AppModes.SELF_PLAY_TRAINING)
                {
                    pythonCommand += " Config/enemyAISelfPlayTraining.yaml";
                    timeScale = 5;
                }

                pythonCommand += $" --env=Maps/{mapsNames[(int)map]}/Enemy-AI-Project";
                pythonCommand += $" --run-id={profileName}";
                pythonCommand += $" --time-scale={timeScale} --width=1280 --height=720";

                if (!newProfle)
                {
                    pythonCommand += " --resume";
                }

                WriteConfigFile(modesNames[(int)mode], profileName, 10, newProfle);
                if(ExecuteProgram("cmd.exe", venv + "&" + pythonCommand))
                {
                    ExitProgram();
                }
            }
            else if (mode == AppModes.PLAYER_GAME || mode == AppModes.SELF_PLAY_GAME)
            {
                if (newProfle)
                {
                    SetInfo("Utwórz profil w trybie treningu by móc rozpocząć tryb gry!");
                    return;
                }

                WriteConfigFile(modesNames[(int)mode], profileName, 1, newProfle);
                if(ExecuteProgram($"Maps\\{mapsNames[(int)map]}\\Enemy-AI-Project.exe", ""))
                {
                    ExitProgram();
                }
            }
        }

        private string GetProfileName()
        {
            if (NewProfileCheckBox.Checked)
            {
                if (newProfileTemplate.IsMatch(ProfileTextBox.Text))
                {
                    return ProfileTextBox.Text;
                }
                else if (ProfileTextBox.Text == "")
                {
                    return emptyProfileName;
                }
                else
                {
                    return nameDoesntMatchTemplate;
                }
            }
            else
            {
                return ProfileComboBox.Text;
            }
        }

        private void WriteConfigFile(string levelType, string mlBrainSession, int timeScale, bool newProfile)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, configFile)))
            {
                sw.WriteLine($"LevelType={levelType}");
                sw.WriteLine($"MLBrainSessionName={mlBrainSession}");
                sw.WriteLine($"TimeScale={timeScale.ToString()}");
                sw.WriteLine($"NewProfile={newProfile.ToString()}");
            }
        }

        private bool ExecuteProgram(string programPath, string arguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = programPath,
                    Arguments = $"/k {arguments}"
                };

                var process = new Process { StartInfo = startInfo };
                process.Start();
                return true;
            }
            catch (Exception ex)
            {
                SetInfo(ex.Message);
                return false;
            }
        }

        private void ExitProgram()
        {
            this.Close();
        }

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
            ProfileComboBox.Items.Clear();

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

        private void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        private bool CheckIfProfileExists(string profileName)
        {
            return ProfileComboBox.Items.Contains(profileName);
        }

        private void DeleteProfileFromComboBox(int index)
        {
            ProfileComboBox.Items.RemoveAt(index);
            if (ProfileComboBox.Items.Count == 0)
            {
                ProfileComboBox.Items.Add(noProfile);
            }

            ProfileComboBox.SelectedIndex = 0;
        }

        private void SetInfo(string text)
        {
            InfoLabel.Text = text;
        }
    }

    public enum AppModes
    {
        PLAYER_TRAINING = 0,
        SELF_PLAY_TRAINING = 1,
        PLAYER_GAME = 2,
        SELF_PLAY_GAME = 3
    }

    public enum Maps
    {
        TRAINING_MAP = 0,
        FOREST = 1
    }
}
