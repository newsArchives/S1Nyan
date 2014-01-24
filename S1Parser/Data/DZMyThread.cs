﻿// Json Mapping Automatically Generated By JsonToolkit Library for C#
// Diego Trinciarelli 2011
// To use this code you will need to reference Newtonsoft's Json Parser, downloadable from codeplex.
// http://json.codeplex.com/
// 

using System;
using Newtonsoft.Json;

namespace S1Parser
{
    public class MyThreadList : IThreadListItem
    {
        [JsonProperty("tid")]
        public string Id { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("subject")]
        public string Title { get; set; }

        [JsonProperty("lastposter")]
        public string LastPoster { get; set; }

        [JsonProperty("replies")]
        public string Subtle { get; set; }

        [JsonProperty("dbdateline"), JsonConverter(typeof (DateTimeConverter))]
        public DateTime AuthorDate { get; set; }

        [JsonProperty("dblastpost"), JsonConverter(typeof (DateTimeConverter))]
        public DateTime LastPostDate { get; set; }

        public string Fid;
        public string Posttableid;
        public string Typeid;
        public string Sortid;
        public string Readperm;
        public string Price;
        public string Authorid;
        public string Dateline;
        public string Lastpost;
        public string Views;
        public string Displayorder;
        public string Highlight;
        public string Digest;
        public string Rate;
        public string Special;
        public string Attachment;
        public string Moderated;
        public string Closed;
        public string Stickreply;
        public string Recommends;
        public string Recommend_add;
        public string Recommend_sub;
        public string Heats;
        public string Status;
        public string Isgroup;
        public string Favtimes;
        public string Sharetimes;
        public string Stamp;
        public string Icon;
        public string Pushedaid;
        public string Cover;
        public string Replycredit;
        public string Relatebytag;
        public string Maxposition;
        public string Bgcolor;
        public string Comments;
        public string Lastposterenc;
        public string Multipage;
        public string Pages;
        public string Recommendicon;
        public string New;
        public string Heatlevel;
        public string Moved;
        public string Icontid;
        public string Folder;
        public string Weeknew;
        public string Istoday;
        public string id;
        public string Rushreply;

        //Empty Constructor
        public MyThreadList()
        {
        }

    }


    public class MyThreadVariables : UserVariables, IThreadList
    {
        public MyThreadList[] Data;

        [JsonProperty("perpage")]
        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPage
        {
            get { return 1; }
        }

        public IThreadListItem[] ThreadList
        {
            get { return Data; }
        }

    }

}

//Json Mapping End
