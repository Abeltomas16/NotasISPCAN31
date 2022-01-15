
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.App;
using Xamarin.Forms;

namespace NotasISPCAN.Droid
{
    [Activity(Label = "Notas", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            Registra();
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void Registra()
        {
            MessagingCenter.Subscribe<object>(this, "pdfgerado", (info) =>
            {
                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                string NOTIFICATION_CHANEL = "50";
                string conteudo = $"guardado em /notas/{info}";
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this, NOTIFICATION_CHANEL)
                    .SetAutoCancel(true)
                    .SetDefaults(1)
                    .SetSmallIcon(Resource.Drawable.icon_about)
                    .SetContentTitle("Pdf gerado")
                    .SetContentText(conteudo)
                    .SetContentInfo("Info");
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANEL, "info", NotificationImportance.Max);
                    notificationChannel.EnableLights(true);
                    notificationChannel.EnableVibration(true);
                    notificationChannel.SetVibrationPattern(new long[] { 0, 1000, 500, 1000 });
                    notificationChannel.Description = "Pdf gerado";
                    notificationManager.CreateNotificationChannel(notificationChannel);
                    notificationManager.Notify(1, builder.Build());
                }
            });
        }
    }
}