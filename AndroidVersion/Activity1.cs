using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Microsoft.Xna.Framework;

namespace AndroidVersion
{
    [Activity(Label = "AndroidVersion"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        MyCrossPlatformGame game;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            game = new MyCrossPlatformGame();
            SetContentView((View)game.Services.GetService(typeof(View)));
            game.Run();
           
        }

        



    }

}

