﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using S1Nyan.App.Resources;
using S1Nyan.Model;
using S1Nyan.ViewModel;
using S1Parser.Action;

namespace S1Nyan.Views
{
    public partial class ThreadView : PhoneApplicationPage
    {
        public ThreadView()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();

            SettingView.UpdateOrientation(this);
            Loaded += (o, e) => SettingView.UpdateOrientation(this);
        }

        /// <summary>
        /// Gets the view's ViewModel.
        /// </summary>
        public ThreadViewModel Vm
        {
            get
            {
                return (ThreadViewModel)DataContext;
            }
        }

        [DataContract]
        public class PageInfoItem
        {
            [DataMember]
            public string id;
            [DataMember]
            public string title;
            [DataMember]
            public int page;
        }

        [DataContract]
        public class ThreadViewPageInfo
        {
            [DataMember]
            public List<PageInfoItem> Stack;

            public ThreadViewPageInfo()
            {
                Stack = new List<PageInfoItem>();
            }
        }

        private const string ThreadViewPageInfoKey = "ThreadViewPageInfo";

        private string idParam = null, titleParam = null;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ImageResourceManager.Reset();
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (idParam == null)
                {   //tombstone
                    var stack = GetInfoStack();
                    if (stack.Count > 0)
                    {
                        var item = stack[stack.Count - 1];
                        Vm.OnChangeTID(item.id, item.title, item.page);
                    }
                }
                return;
            }
            string pageParam = idParam = titleParam = null;
            int page = 1;
            if (NavigationContext.QueryString.TryGetValue("ID", out idParam))
            {
                if (NavigationContext.QueryString.TryGetValue("Title", out titleParam))
                    titleParam = HttpUtility.HtmlDecode(titleParam);
                if (NavigationContext.QueryString.TryGetValue("Page", out pageParam))
                {
                    int.TryParse(pageParam, out page);
                }
                Vm.OnChangeTID(idParam, titleParam, page);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {   //tombstone
            base.OnNavigatedFrom(e);
            ImageResourceManager.Reset();
            if (e.NavigationMode == NavigationMode.Back)
            {
                var stack = GetInfoStack();
                if (stack.Count > 0) stack.RemoveAt(stack.Count - 1);
                Vm.Cleanup();
            }
            else if (e.NavigationMode == NavigationMode.New)
            {
                var stack = GetInfoStack();
                var item = new PageInfoItem();
                item.id = idParam;
                item.page = Vm.CurrentPage;
                item.title = Vm.Title;
                stack.Add(item);
            }
        }

        public static List<PageInfoItem> GetInfoStack()
        {
            ThreadViewPageInfo info = null;
            if (PhoneApplicationService.Current.State.ContainsKey(ThreadViewPageInfoKey))
            {
                info = PhoneApplicationService.Current.State[ThreadViewPageInfoKey] as ThreadViewPageInfo;
            }
            if (info == null)
            {
                info = new ThreadViewPageInfo();
                PhoneApplicationService.Current.State[ThreadViewPageInfoKey] = info;
            }

            return info.Stack;
        }

        ApplicationBarIconButton navBarButton;
        ApplicationBarIconButton replyButton;
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton refreshBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.sync.rest.png", UriKind.Relative));
            refreshBarButton.Text = AppResources.AppBarButtonRefresh;
            refreshBarButton.Click += (o, e) => Vm.RefreshThread();

            navBarButton = new ApplicationBarIconButton(navIcon);
            navBarButton.Text = AppResources.AppBarButtonNavigator;
            navBarButton.Click += ToggleNavigator;

            replyButton = new ApplicationBarIconButton(replyIcon);
            replyButton.Text = AppResources.AppBarButtonReply;
            replyButton.Click += OnReplyButton;

            ApplicationBarIconButton nextBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.next.rest.png", UriKind.Relative));
            nextBarButton.Text = AppResources.AppBarButtonNextPage;
            nextBarButton.Click += (o, e) => Vm.CurrentPage++;

            ApplicationBar.Buttons.Add(refreshBarButton);
            ApplicationBar.Buttons.Add(replyButton);
            ApplicationBar.Buttons.Add(nextBarButton);
            ApplicationBar.Buttons.Add(navBarButton);

            ApplicationBar.MenuItems.Add(SettingView.GetSettingMenuItem());

            nextBarButton.IsEnabled = false;

            Vm.PageChanged = (current, total) =>
            {
                if (current > 1 && total > 1)
                    FirstPage.IsEnabled = true;
                else
                    FirstPage.IsEnabled = false;
                if (current < total && total > 1)
                {
                    nextBarButton.IsEnabled = true;
                    LastPage.IsEnabled = true;
                }
                else
                {
                    nextBarButton.IsEnabled = false;
                    LastPage.IsEnabled = false;
                }
            };
        }

        private async void OnReplyButton(object sender, EventArgs e)
        {
#if UseLocalhost
            var replyLink = "post.php?action=reply&fid=2&tid=1";
#else
            var replyLink = Vm.TheThread.ReplyLink;
#endif
            var v = SettingView.VerifyString;
            if (v == null) return;
            var s = await new S1WebClient().Reply(v,
                reletivePostUrl: replyLink,
                content: "Reply test @" + DateTime.Now.ToShortTimeString(),
                signature: SettingView.GetSignature());
        }

        static Uri replyIcon = new Uri("/Assets/AppBar/appbar.reply.email.png", UriKind.Relative);
        static Uri replyFullIcon = new Uri("/Assets/AppBar/appbar.quill.png", UriKind.Relative);
        static Uri navIcon = new Uri("/Assets/AppBar/appbar.stairs.up.horizontal.png", UriKind.Relative);
        static Uri navIconRevert = new Uri("/Assets/AppBar/appbar.stairs.up.revert.horizontal.png", UriKind.Relative);
        bool IsNavigatorVisible { get { return Navigator.Visibility == Visibility.Visible; } }
        private void ToggleNavigator(object sender, EventArgs e)
        {
            if (navBarButton == null) return;
            if (IsNavigatorVisible)
            {
                navBarButton.IconUri = navIcon;
                HideNavi.Begin();
                HideNavi.Completed += (o, ee) => Navigator.Visibility = Visibility.Collapsed;
            }
            else
            {
                navBarButton.IconUri = navIconRevert;
                Navigator.Visibility = Visibility.Visible;
                ShowNavi.Begin();
            }
        }

    }
}