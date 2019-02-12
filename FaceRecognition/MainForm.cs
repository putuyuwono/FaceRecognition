using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenScreenProjectFaceRS
{
    public partial class MainForm : Form
    {
        private Thread processingThread;
        private PXCMSenseManager senseManager;
        private PXCMFaceConfiguration.RecognitionConfiguration recogConfig;
        private PXCMFaceData faceData;
        private PXCMFaceData.RecognitionData recogData;
        private Int32 numFacesDetected;
        private bool isDBLoaded;
        private const int MAX_USERS = 20;
        private const string DB_NAME = "RECOGDB";
        private const string DB_FILE = "RECOGDB.bin";
        private bool showGrid;
        private bool doRegister;
        private bool isAutoRegister;
        private int[] failedCount;
        private float facePitchThreshold = 12;
        private float faceYawThreshold = 15;
        private float faceRollThreshold = 15;
        private int RecogCompensation = 3;
        private DateTime lastRegisterTime;
        private int RegistrationInterval = 3;
        private const int LandmarkAllignment = -3;
        private readonly object bitmapLock = new object();
        private Bitmap bitmap;
        private Bitmap originalBitmap;
        private FaceCamConfig faceCamConfig;
        private List<Color> faceRectColor;
        private ModuleSetting setting;
        private bool settingIsLoaded;
        private int msgCount;
        private String logFile;
        private List<FaceObject> allRecogFaces;
        private bool isRecordingEnabled = false;
        private bool isRecording = false;
        private DateTime startRec;
        private int frameWritten;
        private VideoWriter videoWriterRes;
        private String saveFaceImgRootDir;
        private UdpClient agClient = null;
        private const String agServerAddr = "127.0.0.1";
        private const int agServerPort = 9999;

        private String reportServer1 = String.Empty;
        private String reportServer2 = String.Empty;
        //private String screenID = String.Empty;

        public MainForm(PXCMSession session)
        {
            InitializeComponent();

            InitUI();

            InitAttr();

            ConfigureCameraInfo();

            ConfigureRecogInfo();

            ConfigureRealSense(session);

            InitThread();
        }

        private void InitUI()
        {
            this.CenterToScreen();
            this.Size = new Size(678, 721);

            pictureBox.Location = new Point(12, 37);
            pictureBox.Size = new Size(640, 480);

            btRegister.Location = new Point(526, 488);
            btRegister.Size = new Size(122, 23);

            btRecord.Location = new Point(20, 488);
            btRecord.Size = new Size(75, 23);
            lblTimer.Location = new Point(20, 460);

            groupBox.Location = new Point(12, 524);
            groupBox.Size = new Size(640, 146);

            rtbReportLog.Location = new Point(7, 20);
            rtbReportLog.Size = new Size(627, 120);
        }

        private void InitAttr()
        {
            settingIsLoaded = false;
            try
            {
                setting = ModuleSetting.Load();
                settingIsLoaded = true;
                
                reportServer1 = setting.serveraddress1;
                reportServer2 = setting.serveraddress2;
                //screenID = setting.screen_id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            numFacesDetected = 0;
            showGrid = false;
            doRegister = false;
            isAutoRegister = true;
            isDBLoaded = false;
            lastRegisterTime = DateTime.Now;
            failedCount = new int[5] { 0, 0, 0, 0, 0 };

            isRecordingEnabled = true;
            ConfigureRecording();

            autoRegisterToolStripMenuItem.Checked = isAutoRegister;
            btRegister.Enabled = !isAutoRegister;
            showGridToolStripMenuItem.Checked = showGrid;

            faceRectColor = new List<Color>();
            faceRectColor.Add(Color.Coral);
            faceRectColor.Add(Color.Lime);
            faceRectColor.Add(Color.Cyan);
            faceRectColor.Add(Color.Beige);
            faceRectColor.Add(Color.Yellow);

            msgCount = 1;
            logFile = Application.StartupPath + "\\openscreen.log";
            //saveFaceImgRootDir = Application.StartupPath + "\\faceimg\\";
            saveFaceImgRootDir = "c:\\AG_Image\\";
            allRecogFaces = new List<FaceObject>();

            try
            {
                agClient = new UdpClient(agServerAddr, agServerPort);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }


            autoRegTimer.Start();
            pictureBox.Paint += PictureBox_Paint;
        }

        private void ConfigureCameraInfo()
        {
            if (settingIsLoaded)
            {
                bool isValid = true;

                String scrID = setting.screen_id;

                Point camLoc = Point.Empty;
                String location = setting.location;
                String[] tLoc = location.Split(',');
                try
                {
                    int x = Int32.Parse(tLoc[0].Trim());
                    int y = Int32.Parse(tLoc[1].Trim());
                    camLoc = new Point(x, y);
                }
                catch (Exception ex)
                {
                    isValid = false;
                    ShowError("Error parsing camera location setting: " + ex.Message);
                }


                String dirOri = setting.directorient;
                FaceCamDirection camDirec;
                char dir = dirOri[0];
                if (dir == 'X')
                {
                    camDirec = FaceCamDirection.X;
                }
                else
                {
                    camDirec = FaceCamDirection.Y;
                }

                FaceCamOrientation camOrien;
                char ori = dirOri[1];
                if (ori == '+')
                {
                    camOrien = FaceCamOrientation.Positive;
                }
                else
                {
                    camOrien = FaceCamOrientation.Negative;
                }

                List<int> cBound = new List<int>();
                String centroidBound = setting.centroidbound;
                String[] tCen = centroidBound.Split(',');
                try
                {
                    foreach (String s in tCen)
                    {
                        int limit = Int32.Parse(s);
                        cBound.Add(limit);
                    }
                }
                catch (Exception ex)
                {
                    isValid = false;
                    ShowError("Error parsing centroid bound: " + ex.Message);
                }

                List<Size> sizeBound = new List<Size>();
                String fsizeBound = setting.facesizebound;
                String[] tFsize = fsizeBound.Split(',');
                try
                {
                    foreach (String s in tFsize)
                    {
                        int fsize = Int32.Parse(s);
                        sizeBound.Add(new Size(fsize, fsize));
                    }
                }
                catch (Exception ex)
                {
                    isValid = false;
                    ShowError("Error parsing face size bound: " + ex.Message);
                }

                Size frameSize = new Size(640, 480);
                if (isValid)
                {
                    faceCamConfig = new FaceCamConfig(scrID, camLoc, camDirec, camOrien, frameSize, cBound, sizeBound);
                }
                else
                {
                    //Using default face cam setting;
                    camLoc = new Point(1, 0);
                    camDirec = FaceCamDirection.Y;
                    camOrien = FaceCamOrientation.Positive;

                    cBound.Add(50);
                    cBound.Add(590);
                    sizeBound.Add(new Size(200, 200));

                    faceCamConfig = new FaceCamConfig(scrID, camLoc, camDirec, camOrien, frameSize, cBound, sizeBound);
                }
            }
        }

        private void ConfigureRecogInfo()
        {
            bool isValid = true;

            if (isValid)
            {
                try
                {
                    RegistrationInterval = setting.registrationinterval;
                    RecogCompensation = setting.recogcompensaation;
                }
                catch (Exception ex)
                {
                    ShowError("Error parsing recognition info: " + ex.Message);
                }
            }
            else
            {
                RegistrationInterval = 3;
                RecogCompensation = 3;
            }
        }

        private void ConfigureRecording()
        {
            enableRecordingToolStripMenuItem.Checked = isRecordingEnabled;
            btRecord.Visible = isRecordingEnabled;
        }

        private void ConfigureRealSense(PXCMSession session)
        {
            PXCMFaceModule faceModule;
            PXCMFaceConfiguration faceConfig;

            // Start the SenseManager and session  
            senseManager = session.CreateSenseManager();

            // Enable the color stream
            senseManager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 640, 480, 30);

            // Enable the face module
            senseManager.EnableFace();
            faceModule = senseManager.QueryFace();
            faceConfig = faceModule.CreateActiveConfiguration();

            // Configure for 3D face tracking (if camera cannot support depth it will revert to 2D tracking)
            faceConfig.SetTrackingMode(PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH);

            // Enable landmarks detection
            faceConfig.landmarks.isEnabled = true;

            // Enable pose detection
            faceConfig.pose.isEnabled = true;

            // Enable facial recognition
            recogConfig = faceConfig.QueryRecognition();
            recogConfig.Enable();

            // Create a recognition database storage
            PXCMFaceConfiguration.RecognitionConfiguration.RecognitionStorageDesc recognitionDesc = new PXCMFaceConfiguration.RecognitionConfiguration.RecognitionStorageDesc();
            recognitionDesc.maxUsers = MAX_USERS;
            recogConfig.CreateStorage(DB_NAME, out recognitionDesc);
            recogConfig.UseStorage(DB_NAME);
            isDBLoaded = LoadDB();
            recogConfig.SetRegistrationMode(PXCMFaceConfiguration.RecognitionConfiguration.RecognitionRegistrationMode.REGISTRATION_MODE_CONTINUOUS);

            // Apply changes and initialize
            faceConfig.ApplyChanges();
            senseManager.Init();
            faceData = faceModule.CreateOutput();

            // Mirror image
            senseManager.QueryCaptureManager().QueryDevice().SetMirrorMode(PXCMCapture.Device.MirrorMode.MIRROR_MODE_HORIZONTAL);

            //IVCam Setting
            senseManager.QueryCaptureManager().QueryDevice().SetIVCAMFilterOption(7);
            senseManager.QueryCaptureManager().QueryDevice().SetIVCAMAccuracy(PXCMCapture.Device.IVCAMAccuracy.IVCAM_ACCURACY_COARSE);
            senseManager.QueryCaptureManager().QueryDevice().SetIVCAMLaserPower(16);
            senseManager.QueryCaptureManager().QueryDevice().SetIVCAMMotionRangeTradeOff(100);

            // Release resources
            faceConfig.Dispose();
            faceModule.Dispose();
        }

        private bool LoadDB()
        {
            bool status = false;
            if (File.Exists(DB_FILE))
            {
                Byte[] buffer = File.ReadAllBytes(DB_FILE);
                recogConfig.SetDatabaseBuffer(buffer);
                status = true;
            }

            return status;
        }

        private void SaveDB()
        {
            PXCMFaceData.RecognitionModuleData rmd = faceData.QueryRecognitionModule();
            Int32 nbytes = rmd.QueryDatabaseSize();
            Byte[] buffer = new Byte[nbytes];
            rmd.QueryDatabaseBuffer(buffer);
            File.WriteAllBytes(DB_FILE, buffer);
        }

        private void InitThread()
        {
            processingThread = new Thread(RunProcess);
            processingThread.Start();
        }

        private void RunProcess()
        {
            while (senseManager.AcquireFrame(true) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                PXCMCapture.Sample sample = senseManager.QuerySample();

                if (sample != null && faceData != null)
                {
                    DisplayImage(sample.color);

                    faceData.Update();

                    Analyze(faceData);

                    if (showGrid) DrawFaceCamGrid();

                    UpdatePictureBox();
                }
                sample.color.Dispose();
                senseManager.ReleaseFrame();
            }
        }

        private void DrawBitmap(Bitmap image)
        {
            lock (bitmapLock)
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
                bitmap = new Bitmap(image);

                if (originalBitmap != null)
                {
                    originalBitmap.Dispose();
                }
                originalBitmap = new Bitmap(image);
            }
        }

        private void DisplayImage(PXCMImage image)
        {
            PXCMImage.ImageData data;
            if (image.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out data) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return;
            }

            //Function for drawing image data in Form
            this.DrawBitmap(data.ToBitmap(0, image.info.width, image.info.height));
            image.ReleaseAccess(data);
        }
        
        private void Analyze(PXCMFaceData data)
        {
            numFacesDetected = data.QueryNumberOfDetectedFaces();
            bool isRegisteringFace = false;
            for (int i = 0; i < numFacesDetected; i++)
            {
                PXCMFaceData.Face face = faceData.QueryFaceByIndex(i);
                if (face != null)
                {
                    FaceObject fObj = new FaceObject();
                    CreateFaceObject(face, ref fObj);

                    //Check landmarks alignment
                    //GetFaceLandmarks(face, out fObj.noseTip, out fObj.leftEye, out fObj.rightEye, out fObj.landmarkDetected);
                    GetFacePose(face, out fObj.pitch, out fObj.yaw, out fObj.roll);
                    CheckFaceAlignment(ref fObj);

                    if (fObj.ID < 0) //Face is NOT recognized
                    {
                        // TODO: Check if the face is registerable

                        if (doRegister && fObj.isAligned)
                        {
                            recogData = face.QueryRecognition();
                            if (recogData != null)
                            {
                                if (isAutoRegister)
                                {
                                    if (failedCount[i] >= RecogCompensation)
                                    {
                                        fObj.ID = recogData.RegisterUser();
                                        isRegisteringFace = true;
                                        failedCount[i] = 0;
                                        Console.WriteLine("Registrering Face: " + fObj.ID);
                                    }
                                    else
                                    {
                                        failedCount[i]++;
                                        Console.WriteLine("Registration Failed (" + i + "): " + failedCount[i] + "/" + RecogCompensation);
                                    }
                                }
                                else //Button Triggered Registration
                                {
                                    fObj.ID = recogData.RegisterUser();
                                }
                            }

                            fObj.registerTime = DateTime.Now;
                            fObj.lastSeen = DateTime.Now;
                        }
                    }
                    else // Face is RECOGNIZED
                    {
                        failedCount[i] = 0;
                    }

                    DrawFace(fObj);
                    CheckFace(fObj);
                }
            }

            if (isRegisteringFace) lastRegisterTime = DateTime.Now;

            if (doRegister) doRegister = false;

            CheckMissingFace();
        }

        private void GetFacePose(PXCMFaceData.Face face, out float pitch, out float yaw, out float roll)
        {
            pitch = -99.0f; yaw = -99.0f; roll = -99.0f;

            PXCMFaceData.PoseData poseData = face.QueryPose();
            if (poseData != null)
            {
                PXCMFaceData.PoseEulerAngles angles;
                poseData.QueryPoseAngles(out angles);

                pitch = angles.pitch;
                yaw = angles.yaw;
                roll = angles.roll;
            }

        }

        private void GetFaceLandmarks(PXCMFaceData.Face face, out PointF nose, out PointF leftEye, out PointF rightEye, out bool landmarkDetected)
        {
            nose = PointF.Empty;
            leftEye = PointF.Empty;
            rightEye = PointF.Empty;
            landmarkDetected = false;

            PXCMFaceData.LandmarksData landmarksData = face.QueryLandmarks();

            if (landmarksData != null)
            {
                int counter = 0;
                PXCMFaceData.LandmarkPoint noseTip;
                landmarksData.QueryPoint(29, out noseTip);
                if (noseTip != null && noseTip.confidenceImage != 0)
                {
                    nose = new PointF(noseTip.image.x + LandmarkAllignment, noseTip.image.y + LandmarkAllignment);
                    counter++;
                }

                PXCMFaceData.LandmarkPoint lEye;
                landmarksData.QueryPoint(76, out lEye);
                if (lEye != null && lEye.confidenceImage != 0)
                {
                    leftEye = new PointF(lEye.image.x + LandmarkAllignment, lEye.image.y + LandmarkAllignment);
                    counter++;
                }

                PXCMFaceData.LandmarkPoint rEye;
                landmarksData.QueryPoint(77, out rEye);
                if (rEye != null && rEye.confidenceImage != 0)
                {
                    rightEye = new PointF(rEye.image.x + LandmarkAllignment, rEye.image.y + LandmarkAllignment);
                    counter++;
                }

                if (counter == 3)
                {
                    landmarkDetected = true;
                }
            }            
        }

        private void CheckFaceAlignment(ref FaceObject fObj)
        {
            int counter = 0;

            if (Math.Abs(fObj.pitch) <= facePitchThreshold)
            {
                counter++;
            }

            if (Math.Abs(fObj.yaw) <= faceYawThreshold)
            {
                counter++;
            }

            if (Math.Abs(fObj.roll) <= faceRollThreshold)
            {
                counter++;
            }

            if (counter == 3)
            {
                fObj.isAligned = true;
            }
            else
            {
                fObj.isAligned = false;
            }
        }

        private void CreateFaceObject(PXCMFaceData.Face face, ref FaceObject fObj)
        {
            PXCMFaceData.DetectionData detection = face.QueryDetection();
            if (detection != null)
            {
                PXCMRectI32 faceRect = new PXCMRectI32();
                detection.QueryBoundingRect(out faceRect);

                PXCMFaceData.RecognitionData recogData = face.QueryRecognition();
                int userID = -1;
                if (recogData != null)
                {
                    userID = recogData.QueryUserID();
                }

                //Registered Face
                if (userID > 0)
                {
                    //Get Face by USER-ID
                    fObj = this.GetRecognizedFaceByID(userID);

                    //Due to pre-loaded DB, FaceObject can be null
                    if (fObj == null) 
                    {
                        fObj = new FaceObject();
                    }
                }

                float faceDistance;
                detection.QueryFaceAverageDepth(out faceDistance);
                faceDistance /= 1000;

                Rectangle rect = new Rectangle(faceRect.x, faceRect.y, faceRect.w, faceRect.h);
                //Point faceLoc = faceCamConfig.GetFaceLoc(rect);
                Point faceLoc = faceCamConfig.Location;

                fObj.ID = userID;
                fObj.boundingBox = rect;
                fObj.cellLocation = faceLoc;
                fObj.registerTime = DateTime.Now;
                fObj.lastSeen = DateTime.Now;
                fObj.distance = faceDistance;
            }
        }

        private void CheckMissingFace()
        {
            foreach (FaceObject f in allRecogFaces)
            {
                if (f.cellLocation == Point.Empty)
                {
                    continue;    
                }

                DateTime now = DateTime.Now;
                TimeSpan ts = now.Subtract(f.lastSeen);
                if (ts.Seconds > 2)
                {
                    f.cellLocation = Point.Empty;
                    f.isVisible = false;
                    Console.WriteLine("FaceID: " + f.ID + " has gone missing");
                }
            }            
        }

        private void CheckFace(FaceObject fObj)
        {
            if (fObj.ID < 0) return;

            bool isNewFace = true;
            foreach (FaceObject f in allRecogFaces)
            {
                if (f.ID == fObj.ID)
                {
                    if (!f.isSameCellLocation(fObj))
                    {
                        Console.WriteLine("Recognized Face: " + fObj.ID + " is moving to another location");
                        GenerateAndSendReport(f.ID, f.gender, f.age, f.cellLocation.X, f.cellLocation.Y, fObj.distance.ToString(), "move");
                        f.UpdateLocation(fObj);
                    }

                    isNewFace = false;
                    break;
                }
            }

            if (isNewFace)
            {
                allRecogFaces.Add(fObj);
                lock (bitmapLock)
                {
                    int numOfPic = 3;
                    this.SaveFaceImage(fObj, numOfPic);
                    String request = this.AGCreateRequest(fObj.ID, numOfPic);
                    this.AGSendRequest(request);
                }

                Console.WriteLine("New Face: " + fObj.ID + " has been added.");
                GenerateAndSendReport(fObj.ID, fObj.gender, fObj.age, fObj.cellLocation.X, fObj.cellLocation.Y, fObj.distance.ToString(), "new");
            }
            else
            {
                if (fObj.isVisible == false)
                {
                    Console.WriteLine("Recognized Face: " + fObj.ID + " is visible.");
                    fObj.isVisible = true;
                    GenerateAndSendReport(fObj.ID, fObj.gender, fObj.age, fObj.cellLocation.X, fObj.cellLocation.Y, fObj.distance.ToString(), "old");
                }
            }
        }

        private FaceObject GetRecognizedFaceByID(int ID)
        {
            FaceObject fObj = null;
            foreach (FaceObject f in allRecogFaces)
            {
                if (f.ID == ID)
                {
                    fObj = f;
                    break;
                }
            }
            return fObj;
        }

        private Rectangle ResizeRect(Rectangle rect, int times)
        {
            Size oldSize = rect.Size;
            Size newSize = new Size(oldSize.Width * times, oldSize.Height * times);
                        
            int x = rect.X - (newSize.Width - oldSize.Width) / 2;
            int y = rect.Y - (newSize.Height - oldSize.Height) / 2;

            Rectangle result = new Rectangle(x, y, newSize.Width, newSize.Height);
            return result;
        }

        private void DrawFace(FaceObject fObj)
        {
            if (bitmap == null) return;

            StringBuilder userLabel = new StringBuilder("ID:");
            Color faceColor = Color.Red;
            if (fObj.ID > -1)
            {
                faceColor = GetFaceRectColor(fObj.ID);
                userLabel.Append(fObj.ID);
                //userLabel.Append(" " + fObj.gender + fObj.age.ToString());
                //userLabel.Append(" " + fObj.cellLocation.X + "," + fObj.cellLocation.Y);
            }
            else
            {
                userLabel.Append("-");
            }

            StringBuilder userInfo = new StringBuilder("G/A:");
            if (!String.IsNullOrEmpty(fObj.gender) && fObj.age > 0)
            {
                userInfo.Append(fObj.gender + fObj.age);
            }
            else
            {
                userInfo.Append("-");
            }
            userInfo.Append("A:" + fObj.isAligned.ToString());
            userInfo.Append("PYR: " + fObj.pitch.ToString("0.00") + ";" + fObj.yaw.ToString("0.00") + ";" + fObj.roll.ToString("0.00"));
            //userInfo.Append(" Loc:" + fObj.cellLocation.X + "," + fObj.cellLocation.Y);

            //if (fObj.distance > 0)
            //{
            //    userLabel.Append(" " + fObj.distance.ToString("0.00") + "m");
            //}
            //else
            //{
            //    userLabel.Append(" >1m");
            //}

            lock (bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                using (Pen facePen = new Pen(faceColor, 3.0f))
                using (Pen redPen = new Pen(Color.Red, 3.0f))
                using (Brush faceBrush = new SolidBrush(faceColor))
                using (Brush blackBrush = new SolidBrush(Color.Black))
                using (Font font = new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold))
                {
                    Rectangle idLabelRect = new Rectangle(fObj.boundingBox.X, fObj.boundingBox.Y - 25, fObj.boundingBox.Width, 25);
                    graphics.FillRectangle(faceBrush, idLabelRect);
                    PointF idLabelLoc = new PointF(fObj.boundingBox.X + 5, fObj.boundingBox.Y - 20);
                    graphics.DrawString(userLabel.ToString(), font, blackBrush, idLabelLoc);

                    Rectangle infoLabelRect = new Rectangle(fObj.boundingBox.X, fObj.boundingBox.Y + fObj.boundingBox.Height, fObj.boundingBox.Width, 25);
                    graphics.FillRectangle(faceBrush, infoLabelRect);
                    PointF infoLabelLoc = new PointF(infoLabelRect.X + 5, infoLabelRect.Y + 5);
                    graphics.DrawString(userInfo.ToString(), font, blackBrush, infoLabelLoc);

                    graphics.DrawRectangle(facePen, fObj.boundingBox);                    
                }
            }
        }

        private void SaveFaceImage(FaceObject fObj, int numbers)
        {
            //Rectangle crop = this.ResizeRect(fObj.boundingBox, 2);
            Bitmap faceImg = originalBitmap.Clone(fObj.boundingBox, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            for (int i = 0; i < numbers; i++)
            {
                String filename = saveFaceImgRootDir + fObj.ID + "_0" + i.ToString() + ".jpg";
                
                try
                {
                    ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 75L);
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    faceImg.Save(filename, myImageCodecInfo, myEncoderParameters);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            Console.WriteLine("Image saved: " + fObj.ID);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private Color GetFaceRectColor(int faceID)
        {
            int code = faceID % faceRectColor.Count;
            return faceRectColor[code];
        }

        private void DrawFaceCamGrid()
        {
            lock (bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                using (Pen pen = new Pen(Color.Cyan, 1.0f))
                {
                    List<int> coordinateBound = faceCamConfig.CoordinateBound;
                    Size frameSize = faceCamConfig.FrameSize;

                    foreach (int vBound in coordinateBound)
                    {
                        graphics.DrawLine(pen, new Point(vBound, 0), new Point(vBound, frameSize.Height));
                    }
                }
            }
        }

        private void ReleaseResources()
        {
            // Stop the worker thread
            processingThread.Abort();

            // Release resources
            faceData.Dispose();
            senseManager.Dispose();
        }

        private void GenerateAndSendReport(int faceID, string gender, int age, int posX, int posY, String distance, String eventType)
        {
            String msgNo = this.GenerateMsgNo(msgCount++);
            String msgTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            FaceRecogReport report = new FaceRecogReport()
            {
                faceid = faceID,
                age = age,
                gender = gender,
                posX = posX,
                posY = posY,
                msg_no = msgNo,
                msg_time = msgTime,
                msg_type = "face_report",
                evt_type = eventType,
                sensor_id = "face_cam"
            };

            String serverReport1 = "OpenScreenReport=" + report.ToJSON();
            this.SendReport(reportServer1, serverReport1);

            FaceRecogReportNew report2 = new FaceRecogReportNew()
            {
                msg_no = msgNo,
                msg_time = msgTime,
                msg_type = "id_report",
                sensor_type = "web_cam",
                sensor_id = "2",
                user_id = faceID.ToString(),
                gender = gender,
                age = age.ToString(),
                height = "175",
                nationality = "Korea",
                screen_id = faceCamConfig.ScreenID,
                screen_loc_x = faceCamConfig.Location.X.ToString(),
                screen_loc_y = faceCamConfig.Location.Y.ToString(),
                distance = distance,
                attention = "N"
            };

            String serverReport2 = "OpenScreenReport=" + report2.ToJSON();
            this.SendReport(reportServer2, serverReport2);
        }

        private async void SendReport(String server, String message)
        {
            String response = String.Empty;
            if (String.IsNullOrEmpty(server))
            {
                response = "Server Address is null/empty";
            }
            else
            {
                try
                {
                    String sendLog = "ReportAddress: " + server + "\n" + message + "\n";
                    AppendReport(sendLog);
                    response = await ReportManager.PostAsync(server, message);
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
            }
            String responseLog = "ReportAddress: " + server + "\nReponse: " + response + "\n";
            AppendReport(responseLog);
            //WriteLog(message);
        }

        private void WriteLog(String logMessage)
        {
            lock (this)
            {
                using (StreamWriter file = new StreamWriter(logFile, true))
                {
                    file.WriteLine(logMessage);
                }
            }
        }

        public String GenerateMsgNo(int msgNo)
        {
            int msg_limit = 9999;
            if (msgNo > msg_limit)
            {
                msgNo = (msgNo % msg_limit) + 1;
            }

            StringBuilder sb = new StringBuilder();
            if (msgNo < 10)
            {
                sb.Append("000");
            }
            else if (msgNo < 100)
            {
                sb.Append("00");
            }
            else if (msgNo < 1000)
            {
                sb.Append("0");
            }
            sb.Append(msgNo);

            return sb.ToString();
        }

        private void ShowError(String message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        delegate void AppendTrackReport(String reportText);
        private void AppendReport(String reportText)
        {
            if (this.rtbReportLog.InvokeRequired)
            {
                AppendTrackReport a = new AppendTrackReport(AppendReport);
                this.Invoke(a, new object[] { reportText });
            }
            else
            {
                this.rtbReportLog.AppendText(reportText);
            }
        }

        private delegate void UpdatePictureBoxDelegate();
        public void UpdatePictureBox()
        {
            pictureBox.Invoke(new UpdatePictureBoxDelegate(() => pictureBox.Invalidate()));
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            lock (bitmapLock)
            {
                if (bitmap != null)
                {
                    e.Graphics.DrawImage(bitmap, pictureBox.ClientRectangle);

                    if (isRecording)
                    {
                        StartRecording(bitmap);
                    }
                    else
                    {
                        StopRecording();
                    }
                }
            }
        }

        private void btRegister_Click(object sender, EventArgs e)
        {
            doRegister = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDB();
            ReleaseResources();
        }

        private void autoRegTimer_Tick(object sender, EventArgs e)
        {
            if (isAutoRegister)
            {
                TimeSpan idleTime = DateTime.Now.Subtract(lastRegisterTime);
                if (idleTime.Seconds > RegistrationInterval)
                {
                    doRegister = !doRegister;
                    lastRegisterTime = DateTime.Now;
                }
            }
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            if (configForm.ShowDialog() == DialogResult.OK)
            {
                setting = ModuleSetting.Load();
                ConfigureCameraInfo();
                ConfigureRecogInfo();
            }
        }

        private void autoRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoRegisterToolStripMenuItem.Checked = !autoRegisterToolStripMenuItem.Checked;
            isAutoRegister = autoRegisterToolStripMenuItem.Checked;
            btRegister.Enabled = !isAutoRegister;
        }

        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGridToolStripMenuItem.Checked = !showGridToolStripMenuItem.Checked;
            showGrid = showGridToolStripMenuItem.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseResources();
            this.Close();
        }

        private void enableRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isRecordingEnabled = !isRecordingEnabled;

            ConfigureRecording();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = "Duration: " + DateTime.Now.Subtract(startRec).ToString();
        }

        private void btRecord_Click(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                btRecord.Text = "Finish";
                isRecording = true;
                lblTimer.Visible = true;

                ConfigRecorder();

                timer.Start();
                startRec = DateTime.Now;
            }
            else
            {
                btRecord.Text = "Record";
                isRecording = false;
                lblTimer.Visible = false;

                videoWriterRes.Dispose();

                timer.Stop();
                lblTimer.Text = "Duration: 00:00:00.000";
            }
        }

        #region Recording Functions
        private void ConfigRecorder()
        {
            frameWritten = 0;
            String resultFilepath = Application.StartupPath + "\\" + DateTime.Now.ToString("yyMMddhhmmss") + "-result.avi";
            videoWriterRes = new VideoWriter(resultFilepath, 10, new Size(640, 480), true);
        }

        private void StartRecording(Bitmap bmp)
        {
            if (videoWriterRes != null)
            {
                Image<Bgr, Byte> img = new Image<Bgr, byte>(bmp);
                videoWriterRes.Write(img.Mat);
                frameWritten++;
            }
        }

        private void StopRecording()
        {
            if (frameWritten > 0)
            {
                videoWriterRes.Dispose();
                frameWritten = 0;
            }
        }
        #endregion

        #region Age-Gender Estimation IPC methods

        private String AGCreateRequest(int PID, int numOfPic)
        {
            return "2001#" + PID.ToString() + "#" + numOfPic.ToString(); 
        }

        private void AGSendRequest(String request)
        {
            if (agClient != null)
            {
                byte[] bRequest = Encoding.ASCII.GetBytes(request);
                agClient.Send(bRequest, bRequest.Length);

                try
                {
                    agClient.BeginReceive(new AsyncCallback(AGGetResponse), null);
                }
                catch { }
            }
        }

        private void AGGetResponse(IAsyncResult res)
        {
            IPEndPoint RemoteIpEndPoint = null;
            byte[] received = agClient.EndReceive(res, ref RemoteIpEndPoint);
            String response = Encoding.UTF8.GetString(received);
            
            //Process codes
            //Console.WriteLine("AG-Response: " + response);
            AGParseRespone(response);
            agClient.BeginReceive(new AsyncCallback(AGGetResponse), null);
        }

        private void AGParseRespone(String serverResponse)
        {
            if (!String.IsNullOrEmpty(serverResponse))
            {
                String[] temp = serverResponse.Split('#');
                if (temp.Length == 3)
                {
                    String code = temp[0];
                    String PID = temp[1];
                    String agegender = temp[2];

                    int faceID = 0;
                    Int32.TryParse(PID, out faceID);
                    if (code.Equals("2002"))
                    {
                        FaceObject fObj = this.GetRecognizedFaceByID(faceID);
                        if (fObj != null)
                        {
                            fObj.gender = agegender.Substring(0, 1);
                            fObj.age = Int32.Parse(agegender.Substring(1, agegender.Length - 1));
                            
                            GenerateAndSendReport(fObj.ID, fObj.gender, fObj.age, fObj.cellLocation.X, fObj.cellLocation.Y, fObj.distance.ToString(), "age");
                        }
                    }
                }
            }
        }
        #endregion
    }
}
