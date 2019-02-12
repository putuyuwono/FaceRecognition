using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace OpenScreenProjectFaceRS
{
    public class FaceRecogReport
    {
        public int faceid;
        public int age;
        public String gender;
        public int posX;
        public int posY;
        public String msg_no;
        public String msg_time;
        public String msg_type;
        public String evt_type;
        public String sensor_id;

        public FaceRecogReport()
        {

        }

        public String ToJSON()
        {
            String jsonFormatString = null;
            jsonFormatString = new JavaScriptSerializer().Serialize(this);
            return jsonFormatString;
        }
    }
}
