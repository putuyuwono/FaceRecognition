using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace OpenScreenProjectFaceRS
{
    public class FaceRecogReportNew
    {
        public String msg_no;
        public String msg_time;
        public String msg_type;
        public String sensor_type;
        public String sensor_id;
        public String user_id;
        public String gender;
        public String age;
        public String height;
        public String nationality;
        public String screen_id;
        public String screen_loc_x;
        public String screen_loc_y;
        public String distance;
        public String attention;

        public FaceRecogReportNew()
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
