using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenScreenProjectFaceRS
{
    public class FaceObject
    {
        public int ID;
        public String gender;
        public int age;
        public Rectangle boundingBox;
        public Point cellLocation;
        public float distance;
        public DateTime registerTime;
        public DateTime lastSeen;
        public bool isVisible = true;
        public PointF noseTip;
        public PointF leftEye;
        public PointF rightEye;
        public bool landmarkDetected = false;
        public float pitch = -99.0f;//up-down
        public float yaw = -99.0f;//right-left
        public float roll = -99.0f;//bend right-left
        public bool isAligned = false;
        
        public FaceObject()
        {
            noseTip = PointF.Empty;
            leftEye = PointF.Empty;
            rightEye = PointF.Empty;
        }

        public bool isSameCellLocation(FaceObject fObj)
        {
            bool status = false;
            if (this.cellLocation.X == fObj.cellLocation.X && this.cellLocation.Y == fObj.cellLocation.Y)
            {
                status = true;
            }
            return status;
        }

        public void UpdateLocation(FaceObject fObj)
        {
            this.boundingBox = fObj.boundingBox;
            this.cellLocation = fObj.cellLocation;
            this.distance = fObj.distance;
            this.lastSeen = fObj.lastSeen;
        }

        public String GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ID: " + this.ID);
            sb.Append("Loc: " + this.cellLocation.X + "," + this.cellLocation.Y);
            return sb.ToString();
        }

        public bool IsRegisterable()
        {
            bool registerable = false;

            if (landmarkDetected)
            {
                Rectangle midFace = GetMidFaceROI();
                registerable = Utils.PointIsInsideRect(ref this.noseTip, ref midFace);
            }

            return registerable;
        }

        public Rectangle GetMidFaceROI()
        {
            Rectangle result = Rectangle.Empty;

            if (this.boundingBox != null && this.boundingBox != Rectangle.Empty)
            {
                int factor = 3;
                int width = (this.boundingBox.Width / factor);
                int height = (this.boundingBox.Height / factor);
                result = new Rectangle();
                result.Width = width;
                result.Height = height;
                result.X = this.boundingBox.X + width;
                result.Y = this.boundingBox.Y + height;
            }

            return result;
        }
    }
}
