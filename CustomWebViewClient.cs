using Android.Graphics;
using Android.Views;
using Android.Webkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phoenixpulsecu
{
    public class CustomWebViewClient : WebViewClient
    {
        private IPageLoadListener mListener;

        public CustomWebViewClient(IPageLoadListener listener)
        {
            mListener = listener;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            mListener?.OnPageLoaded();
        }

        public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
        {
            base.OnReceivedError(view, request, error);
            mListener?.OnPageError();
        }
    }

}
