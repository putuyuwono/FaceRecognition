using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenScreenProjectFaceRS
{
    public partial class ConfigForm : Form
    {
        private ModuleSetting setting;

        public ConfigForm()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void LoadConfigFile()
        {
            try
            {
                setting = ModuleSetting.Load();
                this.DisplayConfigContent();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void DisplayConfigContent()
        {
            if (setting != null)
            {
                cfgServerAddress1.Text = setting.serveraddress1;
                cfgServerAddress2.Text = setting.serveraddress2;
                cfgScreenID.Text = setting.screen_id;
                cfgLocation.Text = setting.location;
                cfgDirectionOrientation.SelectedIndex = cfgDirectionOrientation.FindString(setting.directorient);
                cfgCentroidBound.Text = setting.centroidbound;
                cfgFaceSizeBound.Text = setting.facesizebound;
                cfgInterval.Value = setting.registrationinterval;
                cfgCompensation.Value = setting.recogcompensaation;
            }
        }

        private bool ValidateConfigutation()
        {
            bool isValid = false;

            try
            {
                setting = new ModuleSetting();
                setting.serveraddress1 = cfgServerAddress1.Text;
                setting.serveraddress2 = cfgServerAddress2.Text;
                setting.screen_id = cfgScreenID.Text;
                setting.location = cfgLocation.Text;
                setting.directorient = cfgDirectionOrientation.Text;
                setting.centroidbound = cfgCentroidBound.Text;
                setting.facesizebound = cfgFaceSizeBound.Text;
                setting.registrationinterval = (int)cfgInterval.Value;
                setting.recogcompensaation = (int)cfgCompensation.Value;

                isValid = true;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            return isValid;
        }

        private void ShowError(String message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            this.LoadConfigFile();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            bool isValid = ValidateConfigutation();
            if (isValid)
            {
                try
                {
                    setting.Save();
                    this.Close();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }
    }
}
