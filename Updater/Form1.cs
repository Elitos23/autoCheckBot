using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public partial class Form1 : Form
    {
        static private string lastInstaller = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Updater/updates/STARDOM_INST.msi";
        string newInstallerPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Updater/updates/STARDOM_INST.msi";
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //Enable paint via message to reduce flicker. 
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
        bool currentlyAnimating = false;
        //Indicate if update the animation.
        bool isAnimating = true;
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (isAnimating)
            {
                //Begin the animation.
                AnimateImage();
                //Update the frames. The cell would paint the next frame of the image late on.
                ImageAnimator.UpdateFrames();
            }
            base.OnPaintBackground(e);
        }
        //This method begins the animation.
        public void AnimateImage()
        {
            if (!currentlyAnimating)
            {
                ImageAnimator.Animate(this.BackgroundImage, new EventHandler(this.OnFrameChanged));
                currentlyAnimating = true;
            }
        }
        private void OnFrameChanged(object o, EventArgs e)
        {
            //Force to redraw the form.
            this.Invalidate();
        }
        static async Task<int> RunProcessAsync()
        {
            var tcs = new TaskCompletionSource<int>();

            var process = new Process
            {
                StartInfo = { FileName = "msiexec.exe", Arguments = "/x " + lastInstaller + " /qn" },
                EnableRaisingEvents = true
            };

            process.Exited += (sender3, args) =>
            {
                Console.WriteLine("кончил");
                tcs.SetResult(process.ExitCode);
                process.Dispose();

            };

            process.Start();

            return await tcs.Task;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            string pathUpdater = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Updater/";

            string updaterPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM//Updater/Updater.exe";
          
            string lastVersionPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Updater/updates/STARDOM_INST.msi";
            string exe = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/STARDAPP.exe";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom";

            


            bool iSdeleted = false;
            Process p = new Process();
            try
            {
                Update.Visible = false;


                /*p.StartInfo.FileName = "msiexec.exe";

                p.StartInfo.Arguments = "/x " + lastInstaller + " /qn";
                p.Start();*/
                Console.WriteLine("начал процесс");
                await RunProcessAsync();


            }
            catch
            {
                if (System.IO.File.Exists(exe))
                {

                    MessageBox.Show("Проблемы с деинсталяцией, обнови в ручную");

                }



            }


            while (iSdeleted == false)
            {
                if (Directory.GetFiles(path, "*.dll").Length == 0)
                {
                    iSdeleted = true;
                    if (File.Exists(lastInstaller))
                    {

                        File.Delete(lastInstaller);
                        Console.WriteLine("удалил старый инсталятор");
                    }
                    else
                    {

                        Console.WriteLine("Проблемка");
                    }

                    Console.WriteLine("Удалил старые файлы");
                }
                else
                {
                    System.Threading.Thread.Sleep(5000);
                    iSdeleted = false;

                }









            }


            if (iSdeleted == true)
            {



                WebClient webClient = new WebClient();


               

                    Console.WriteLine("На месте");

                    progressBar1.Visible = true;
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    webClient.DownloadFileAsync(new Uri("http://stardomaio.ru/STARDOM_INST.msi"), lastInstaller);
                    Console.WriteLine("Скачал");

                


            }
            else
            {


            }









        }


        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            string lastVersionPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\STARDOM\Updater\updates\STARDOM_INST.msi";
            string exefile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\STARDOM\Stardom\STDAPP.exe";
            System.Threading.Thread.Sleep(3000);

            Console.WriteLine("Устанвока");
            if (System.IO.File.Exists(lastVersionPath))
            {

                Console.WriteLine("На месте инсталятор");

                Installer.SetInternalUI(InstallUIOptions.Silent);
                Installer.InstallProduct(lastVersionPath, "ACTION=INSTALL ALLUSERS=2 MSIINSTALLPERUSER=");
                Console.WriteLine("хуй тебе");

                System.Diagnostics.Process.Start(exefile);
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }


        }
        private void Completed1(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("завершил");


        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
