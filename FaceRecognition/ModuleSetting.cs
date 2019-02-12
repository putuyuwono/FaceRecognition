using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;

namespace OpenScreenProjectFaceRS
{
    public class ModuleSetting
    {
        private static String configFilePath = @"setting/config.json";

        public String serveraddress1;
        public String serveraddress2;
        public String screen_id;
        public String location;
        public String directorient;
        public String centroidbound = "";
        public String facesizebound = "";
        public int registrationinterval;
        public int recogcompensaation;

        public ModuleSetting()
        {

        }

        private bool CheckSettings()
        {
            bool status = true;

            if (String.IsNullOrEmpty(serveraddress1)) status = false;
            if (String.IsNullOrEmpty(serveraddress2)) status = false;
            if (String.IsNullOrEmpty(screen_id)) status = false;
            if (String.IsNullOrEmpty(location)) status = false;
            if (String.IsNullOrEmpty(directorient)) status = false;
            if (String.IsNullOrEmpty(centroidbound)) status = false;
            if (String.IsNullOrEmpty(facesizebound)) status = false;
            if (registrationinterval <= 0) status = false;
            if (recogcompensaation <= 0) status = false;

            return status;
        }

        public String ToJSON()
        {
            String jsonFormatString = String.Empty;
            if (CheckSettings())
            {
                jsonFormatString = new JavaScriptSerializer().Serialize(this);
            }
            return jsonFormatString;
        }

        public static ModuleSetting Load()
        {
            ModuleSetting setting = null;
            try
            {
                String configContent = File.ReadAllText(configFilePath);
                setting = new JavaScriptSerializer().Deserialize<ModuleSetting>(configContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return setting;
        }

        public void Save()
        {
            if (this != null)
            {
                try
                {
                    String configContent = this.ToJSON();
                    if (!String.IsNullOrEmpty(configContent))
                    {
                        File.WriteAllText(configFilePath, configContent);   
                    }
                    else
                    {
                        throw new Exception("Invalid Configuration Setting");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
