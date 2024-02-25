using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Bumptech.Glide;

namespace phoenixpulsecu
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        WebView? webview;
        ImageView? imageViewLoading;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Make the app full screen by hiding the status bar
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            // Hide the title bar as well
            RequestWindowFeature(WindowFeatures.NoTitle);

            // This line is for hiding the navigation bar and making the app truly full screen
            // It uses immersive mode which allows for temporary hiding of both the navigation and status bar
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                SystemUiFlags.LayoutStable |
                SystemUiFlags.LayoutHideNavigation |
                SystemUiFlags.LayoutFullscreen |
                SystemUiFlags.HideNavigation |
                SystemUiFlags.Fullscreen |
                SystemUiFlags.ImmersiveSticky);


            SetContentView(Resource.Layout.activity_main);
            webview = FindViewById<WebView>(Resource.Id.webView1);
            imageViewLoading = FindViewById<ImageView>(Resource.Id.imageViewLoading);

            Glide.With(this).Load(Resource.Drawable.loading).Into(imageViewLoading);


            if (webview == null) return;

            webview.Settings.JavaScriptEnabled = true;
            webview.SetWebViewClient(new CustomWebViewClient(this));
            webview.LoadUrl("https://phoenixpulsecu.com/secure/customer_login");
        }

        public override void OnBackPressed()
        {
            if (webview != null && webview.CanGoBack())
            {
                webview.GoBack();
            }
            else
            {
                ShowExitConfirmationDialog();
            }
        }

        private void ShowExitConfirmationDialog()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage("Do you want to exit?");
            builder.SetPositiveButton("Yes", (sender, args) => { Finish(); });
            builder.SetNegativeButton("No", (sender, args) => { /* Do nothing */ });
            builder.SetCancelable(false);
            builder.Show();
        }

        // Method to update label visibility
        public void SetLabelLoadingVisibility(Android.Views.ViewStates visibility)
        {
            RunOnUiThread(() => {
                if(visibility == Android.Views.ViewStates.Visible)
                {
                    imageViewLoading.Visibility = visibility;
                    webview.Visibility = Android.Views.ViewStates.Gone;
                }
                else { 
                    imageViewLoading.Visibility = Android.Views.ViewStates.Gone;
                    webview.Visibility = Android.Views.ViewStates.Visible;
                }
            });
        }

        private class CustomWebViewClient : WebViewClient
        {
            private MainActivity _activity;

            public CustomWebViewClient(MainActivity activity)
            {
                _activity = activity;
            }

            public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);
                _activity.SetLabelLoadingVisibility(Android.Views.ViewStates.Visible);
            }

            public override void OnPageFinished(WebView view, string url)
            {
                base.OnPageFinished(view, url);
                _activity.SetLabelLoadingVisibility(Android.Views.ViewStates.Gone);
            }
        }
    }
}
