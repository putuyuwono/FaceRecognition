using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OpenScreenProjectFaceRS
{
    public class Utils
    {
        public static PointF CalculateMedian(ref PointF p1, ref PointF p2)
        {
            PointF median = PointF.Empty;

            if (p1 != null && p1 != PointF.Empty && p2 != null && p2 != PointF.Empty)
            {
                float dX = p2.X - p1.X;
                float dY = p2.Y - p1.Y;

                median.X = p1.X + dX;
                median.Y = p1.Y + dY;
            }

            return median;
        }

        public static bool PointIsInsideRect(ref PointF p, ref Rectangle rect)
        {
            bool status = false;

            if (p != null && p != PointF.Empty && rect != null && rect != Rectangle.Empty)
            {
                if (p.X >= rect.X && p.X <= rect.X + rect.Width && p.Y >= rect.Y && p.Y <= rect.Y + rect.Height)
                {
                    status = true;
                }
            }

            return status;
        }
    }
}
