﻿// Json Mapping Automatically Generated By JsonToolkit Library for C#
// Diego Trinciarelli 2011
// To use this code you will need to reference Newtonsoft's Json Parser, downloadable from codeplex.
// http://json.codeplex.com/
// 

using System;
using Newtonsoft.Json;

namespace S1Parser
{
    public class FavoriteList : IThreadListItem
    {

        public string Favid;
        public string Uid;
        public string Idtype;
        public string Spaceuid;
        public string Icon;
        public string Url;

        public string Id { get; set; }
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Subtle { get; set; }

        [JsonProperty("dateline"), JsonConverter(typeof (DateTimeConverter))]
        public DateTime AuthorDate { get; set; }

        public string Author { get; set; }

        public string LastPoster { get; set; }

        public DateTime LastPostDate { get; set; }

        //Empty Constructor
        public FavoriteList()
        {
        }

    }

    public class MyFavoriteVariables : UserVariables, IThreadList
    {
        public int Count;

        public FavoriteList[] List;

        [JsonProperty("perpage")]
        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPage
        {
            get { return Count/ItemsPerPage; }
        }

        public IThreadListItem[] ThreadList
        {
            get { return List; }
        }

        //Empty Constructor
        public MyFavoriteVariables()
        {
        }

    }

}

//Json Mapping End