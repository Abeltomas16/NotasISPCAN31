using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Java.IO;
using NotasISPCAN.Droid.Services;
using NotasISPCAN.Relatorio.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndroid))]
namespace NotasISPCAN.Droid.Services
{
    public class SaveAndroid : ISave
    {
        public async Task SaveAndView(string fileName, String contentType, MemoryStream stream)
        {
            string exception = string.Empty;
            string root = null;

            if (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions((Android.App.Activity)Android.App.Application.Context, new String[] { Manifest.Permission.WriteExternalStorage }, 1);
            }

            if (Android.OS.Environment.IsExternalStorageEmulated)
                root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            else
                root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            Java.IO.File myDir = new Java.IO.File(root + "/notas");
            myDir.Mkdir();
            Java.IO.File file = new Java.IO.File(myDir, fileName);

            if (file.Exists()) file.Delete();

            try
            {
                FileOutputStream outs = new FileOutputStream(file);
                await outs.WriteAsync(stream.ToArray());

                outs.Flush();
                outs.Close();
            }
            catch (Exception e)
            {
                exception = e.ToString();
            }
        }
    }
}
