using Luxand;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;

namespace SimpleLuxCamera
{
    public class Camera : WindowsFormsHost
    {
        System.Windows.Forms.PictureBox cameraImage = new System.Windows.Forms.PictureBox();

        public Camera() : base()
        {
            var lic = File.ReadAllText("lux.txt");
            FSDK.ActivateLibrary(lic);
            FSDK.InitializeLibrary();
            FSDKCam.InitializeCapturing();
        }
    }
}
