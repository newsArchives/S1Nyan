﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using S1Parser.DZParser;
using S1Parser.SimpleParser;

namespace S1Parser.PaserFactory
{
    public class DZParserFactory : IParserFactory
    {
        internal const int ThreadsPerPage = 50;
        internal const int PostsPerPage = 30;

        public IResourceService ResourceService { get; set; }
        internal const string DZMobilePath = "api/mobile/";

        public DZParserFactory(IResourceService resourceService)
        {
            ResourceService = resourceService;
            S1Resource.ParserFactory = this;
        }

        public string Path
        {
            get { return DZMobilePath; }
        }

        public string GetThreadOriginalUrl(string tid)
        {
            return  S1Resource.SiteBase + string.Format("thread-{0}-1-1.html", tid);
        }

        public async Task<IList<S1ListItem>> GetMainListData()
        {
            Stream s = await GetMainListStream();
            return new SimpleListParser(s).GetData();
        }

        public async Task<Stream> GetMainListStream()
        {
            Stream s = await ResourceService.GetResourceStream(GetMainUri());
            return s;
        }

        public IList<S1ListItem> ParseMainListData(Stream s)
        {
            return new DZListParser(s).GetData();
        }

        public IList<S1ListItem> ParseMainListData(string s)
        {
            return new DZListParser(s).GetData();
        }

        public async Task<S1ThreadList> GetThreadListData(string fid, int page)
        {
            Stream s = await ResourceService.GetResourceStream(GetThreadListUri(fid, page));
            return new DZThreadListParser(s).GetData(fid, page);
        }

        public async Task<S1Post> GetPostData(string tid, int page)
        {
            Stream s = await ResourceService.GetResourceStream(GetThreadUri(tid, page));
            return new DZPostParser(s).GetData();
        }

        protected virtual Uri GetMainUri()
        {
            //return new Uri("FakeData/main.json", UriKind.Relative);
            return new Uri(S1Resource.ForumBase + "?module=forumnav");
        }

        protected virtual Uri GetThreadListUri(string fid, int page)
        {
#if UseLocalhost
            return new Uri(S1Resource.ForumBase + string.Format("?module=forumdisplay&fid=2"));
#else
            string url = DZMyGroup.ParseSpecialUrl(fid, page) ?? String.Format("?module=forumdisplay&fid={0}&page={1}&tpp={2}", fid, page, ThreadsPerPage);
            return new Uri(S1Resource.ForumBase + url);
#endif
        }

        protected virtual Uri GetThreadUri(string tid, int page)
        {
#if UseLocalhost
            return new Uri(S1Resource.ForumBase + string.Format("?module=viewthread&tid=1&ppp=50"));
#else
            return new Uri(S1Resource.ForumBase + string.Format("?module=viewthread&tid={0}&page={1}&ppp={2}", tid, page, PostsPerPage));
#endif
        }

    }
}
