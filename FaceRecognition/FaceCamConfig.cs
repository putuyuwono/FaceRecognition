using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenScreenProjectFaceRS
{
    public enum FaceCamDirection 
    {
        X, Y
    }

    public enum FaceCamOrientation
    {
        Positive, Negative
    }

    public class FaceCamConfig
    {
        private String screenID;
        private Point location; //Grid-cell location (x,y)
        private FaceCamDirection direction;
        private FaceCamOrientation orientation;
        private Size frameSize;
        private List<int> coordinateBound;
        private List<Size> faceSizeBound;

        public FaceCamConfig(String scrID, Point loc, FaceCamDirection direct, FaceCamOrientation orient, Size frSize, List<int> corBound, List<Size> sizeBound)
        {
            this.screenID = scrID;
            this.location = loc;
            this.direction = direct;
            this.orientation = orient;
            this.frameSize = frSize;

            this.coordinateBound = corBound;
            this.faceSizeBound = sizeBound;
        }

        public String ScreenID
        {
            get { return screenID; }
            private set { this.screenID = value; }
        }

        public List<int> CoordinateBound 
        {
            get { return this.coordinateBound; }
            private set { this.coordinateBound = value; }
        }

        public Size FrameSize 
        {
            get { return this.frameSize; }
            private set { this.frameSize = value; }
        }

        public Point Location
        {
            get { return this.location; }
            private set { this.location = value; }
        }

        public Point GetFaceLoc(Rectangle faceRect)
        {
            int shiftX = 0, shiftY = 0;
            int centralLine = this.frameSize.Width / 2;
            bool isRightSide = faceRect.X > centralLine ? true : false;
            Point faceCentroid = new Point(faceRect.X + faceRect.Width / 2, faceRect.Y + faceRect.Height / 2);
            
            if (direction == FaceCamDirection.X)
            {
                if (orientation == FaceCamOrientation.Positive)
                {
                    foreach (Size bound in faceSizeBound)
                    {
                        /* Smaller face size increase X
                         */
                        if (faceRect.Height < bound.Height && faceRect.Width < bound.Width)
                        {
                            shiftX++;
                        }
                    }

                    foreach (int vertiLine in coordinateBound)
                    {
                        /* Larger face centroid x-coor will decrease Y 
                         * Smaller face centroid x-coor will increase Y
                         */
                        if (isRightSide)
                        {
                            if (vertiLine > centralLine && faceCentroid.X > vertiLine)
                            {
                                shiftY--;
                            }
                        }
                        else // isLeftSide
                        {
                            if (vertiLine <= centralLine && faceCentroid.X < vertiLine)
                            {
                                shiftY++;
                            }
                        }
                    }
                }
                else // FaceCamOrientation.Negative
                {
                    foreach (Size bound in faceSizeBound)
                    {
                        /* Smaller face size decrease X
                         */
                        if (faceRect.Height < bound.Height && faceRect.Width < bound.Width)
                        {
                            shiftX--;
                        }
                    }

                    foreach (int vertiLine in coordinateBound)
                    {
                        /* Larger face centroid x-coor will increase Y
                         * Smaller face centroid x-coor will decrease Y
                         */
                        if (isRightSide)
                        {
                            if (vertiLine > centralLine && faceCentroid.X > vertiLine)
                            {
                                shiftY++;
                            }
                        }
                        else // isLeftSide
                        {
                            if (vertiLine <= centralLine && faceCentroid.X < vertiLine)
                            {
                                shiftY--;
                            }
                        }
                    }                    
                }
            }
            else // FaceCamDirection.Y
            {
                if (orientation == FaceCamOrientation.Positive)
                {
                    foreach (Size bound in faceSizeBound)
                    {
                        // Smaller face size increase Y
                        if (faceRect.Height < bound.Height && faceRect.Width < bound.Width)
                        {
                            shiftY++;
                        }
                    }
                    
                    foreach (int vertiLine in coordinateBound)
                    {
                        /* Larger face centroid x-coor will increase X 
                         * Smaller face centroid x-coor will decrease X
                         */
                        if (isRightSide)
                        {
                            if (vertiLine > centralLine && faceCentroid.X > vertiLine)
                            {
                                shiftX++;
                            }
                        }
                        else // isLeftSide
                        {
                            if (vertiLine <= centralLine && faceCentroid.X < vertiLine)
                            {
                                shiftX--;
                            }
                        }
                    }
                }
                else // FaceCamOrientation.Negative
                {
                    foreach (Size bound in faceSizeBound)
                    {
                        /* Smaller face size decrease Y
                         */
                        if (faceRect.Height < bound.Height && faceRect.Width < bound.Width)
                        {
                            shiftY--;
                        }
                    }
                                        
                    foreach (int vertiLine in coordinateBound)
                    {
                        /* Larger face centroid x-coor will decrease X
                         * Smaller face centroid x-coor will increase X
                         */
                        if (isRightSide)
                        {
                            if (vertiLine > centralLine && faceCentroid.X > vertiLine)
                            {
                                shiftX--;
                            }
                        }
                        else // isLeftSide
                        {
                            if (vertiLine <= centralLine && faceCentroid.X < vertiLine)
                            {
                                shiftX++;
                            }
                        }
                    }
                }
            }
            
            return new Point(this.location.X + shiftX, this.location.Y + shiftY);
        }
    }
}
