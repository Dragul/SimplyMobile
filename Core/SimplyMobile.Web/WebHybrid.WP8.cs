﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using SimplyMobile.Text;

namespace SimplyMobile.Web
{
    public partial class WebHybrid
    {
        private WebBrowser webView;

        public WebHybrid(WebBrowser webView) : this(webView, null)
        {

        }

        public WebHybrid(WebBrowser webView, IJsonSerializer serializer)
        {
            this.webView = webView;
            this.Serializer = serializer;
            this.Initialize();
        }

        private void Initialize()
        {
            this.registeredActions = new Dictionary<string, Action<string>>();

            this.webView.IsScriptEnabled = true;
            this.webView.Navigating += webView_Navigating;
            this.webView.LoadCompleted += webView_LoadCompleted;
            this.webView.ScriptNotify += WebViewOnScriptNotify;
        }

        private void WebViewOnScriptNotify(object sender, NotifyEventArgs notifyEventArgs)
        {
            System.Diagnostics.Debug.WriteLine(notifyEventArgs.Value);

            Action<string> action;
            var values = notifyEventArgs.Value.Split('/');
            var name = values.FirstOrDefault();

            if (name != null && this.registeredActions.TryGetValue(name, out action))
            {
                var data = Uri.UnescapeDataString(values.ElementAt(1));
                action.Invoke(data);
            }
        }

        void webView_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //this.InjectNativeFunctionScript();
        }

        void webView_Navigating(object sender, NavigatingEventArgs e)
        {
            e.Cancel = this.CheckRequest(e.Uri.AbsoluteUri);
        }

        partial void Inject(string script)
        {
            //var context = this.webView.DataContext;
            this.webView.InvokeScript(string.Format("javascript: {0}", script));
            //this.webView.InvokeScript(script);
        }
    }
}
