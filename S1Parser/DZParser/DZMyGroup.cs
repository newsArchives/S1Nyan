﻿using System;
using System.Collections.Generic;

namespace S1Parser.DZParser
{
    public enum MyGroupTypes
    {
        MyReplys = -3,
        MyThreads = -2,
        MyFavorite = -1,
        None = 0,
    }

    public class DZMyGroup
    {
        private static S1ListItem _myGroup;

        internal static S1ListItem MyGroup
        {
            get
            {
                return _myGroup ?? (
                    _myGroup = new S1ListItem("我的", "", new List<S1ListItem>
                    {
                        MyS1ListItem(MyGroupTypes.MyFavorite),
                        MyS1ListItem(MyGroupTypes.MyThreads),
                        MyS1ListItem(MyGroupTypes.MyReplys),
                    }));
            }
        }

        private static S1ListItem MyS1ListItem(MyGroupTypes type)
        {
            var itemName = "";
            switch (type)
            {
                case MyGroupTypes.MyFavorite:
                    itemName = "我的收藏";
                    break;
                case MyGroupTypes.MyThreads:
                    itemName = "我的主题";
                    break;
                case MyGroupTypes.MyReplys:
                    itemName = "我回复的主题";
                    break;
            }
            return new S1ListItem(itemName, ((int) type).ToString(), null);
        }

        internal static string ParseSpecialUrl(string fid, int page)
        {
            switch (GetMyGroupType(fid))
            {
                case MyGroupTypes.MyFavorite:
                    return string.Format("?module=myfavthread&page={0}", page);
                case MyGroupTypes.MyThreads:
                    return string.Format("?module=mythread&page={0}", page);
                case MyGroupTypes.MyReplys:
                    return string.Format("?module=mythread&type=reply&page={0}", page);
            }

            return null;
        }

        private static MyGroupTypes GetMyGroupType(string fid)
        {
            int type = 0;
            int.TryParse(fid, out type);
            return (MyGroupTypes) type;
        }

        internal static IThreadList ThreadListFromJson(string json, string fid)
        {
            Type type = typeof(ForumVariables);
            switch (GetMyGroupType(fid))
            {
                case MyGroupTypes.MyFavorite:
                    type = typeof(MyFavoriteVariables);
                    break;
                case MyGroupTypes.MyThreads:
                case MyGroupTypes.MyReplys:
                    type = typeof(MyThreadVariables);
                    break;
            }
            return (IThreadList)json.Parse(type);
        }

        public static bool IsMyFavorite(string fid)
        {
            return GetMyGroupType(fid) == MyGroupTypes.MyFavorite;
        }
    }
}