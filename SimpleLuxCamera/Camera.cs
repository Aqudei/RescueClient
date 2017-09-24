using Luxand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;

namespace SimpleLuxCamera
{
    public class Camera : WindowsFormsHost
    {


        public string LastCapturePath
        {
            get { return (string)GetValue(LastCapturePathProperty); }
            set { SetValue(LastCapturePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastCapturePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastCapturePathProperty =
            DependencyProperty.Register("LastCapturePath", typeof(string), typeof(Camera), new PropertyMetadata(null));

        System.Windows.Forms.PictureBox cameraImage = new System.Windows.Forms.PictureBox();

        private string[] cameraList;
        private int cameraHandle;
        private bool camIsGood;
        private BackgroundWorker worker;
        private int cameraCount;
        private bool startCapture;
        private bool isPause;
        private bool libIsGood;

        public Camera() : base()
        {
            Child = cameraImage;
            cameraImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            var lic = File.ReadAllText("lux.txt");
            if (FSDK.FSDKE_OK == FSDK.ActivateLibrary(lic))
            {
                libIsGood = true;
                FSDK.InitializeLibrary();
                FSDKCam.InitializeCapturing();
                Directory.CreateDirectory("Captures");
            }
        }

        public void CaptureImage()
        {
            if (camIsGood)
                startCapture = true;
        }

        public void InitializeCamera()
        {
            if (libIsGood)
            {
                FSDKCam.GetCameraList(out cameraList, out cameraCount);
                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
            }
        }

        public void StartCamera()
        {
            if (cameraCount > 0)
            {
                camIsGood = FSDK.FSDKE_OK == FSDKCam.OpenVideoCamera(ref cameraList[0], ref cameraHandle);
                if (camIsGood)
                {
                    worker.RunWorkerAsync();
                }
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
                cameraImage.Image = (Image)e.UserState;
            if (e.ProgressPercentage == 1)
                LastCapturePath = e.UserState.ToString();
        }


        public void Pause()
        {
            isPause = true;
        }

        public void Resume()
        {
            isPause = false;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int image = 0;
            while (camIsGood)
            {
                if (isPause)
                {
                    Thread.Sleep(4);
                    continue;
                }
                FSDKCam.GrabFrame(cameraHandle, ref image);
                var cimg = new FSDK.CImage(image);
                worker.ReportProgress(0, cimg.Copy().ToCLRImage());
                if (startCapture)
                {
                    var imgPath = Path.GetFullPath(Path.Combine("Captures", Guid.NewGuid().ToString() + ".jpg"));
                    cimg.Save(imgPath);
                    worker.ReportProgress(1, imgPath);
                    startCapture = false;
                    isPause = true;
                }
                cimg.Dispose();
                FSDK.FreeImage(image);
                GC.Collect();
                Thread.Sleep(16);
            }

            FSDKCam.CloseVideoCamera(cameraHandle);
        }

        public void EndCapturing()
        {
            camIsGood = false;
        }
    }
}
