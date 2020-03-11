using EditorCommon;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ResourceFormat{
    public enum TextureOverviewMode
    {
        ReadWrite = 0,
        MipMap,
        Type,
        Resolution,
        WidthVSHeight,
        StandaloneFormat,
        AndroidFormat,
        iOSFormat,

        Count,
    }

    public class TextureOverviewData
    {
        /// Dont modify variable name
        public int Count;
        public int Memory;
        public bool ReadWriteEnable;
        public bool MipmapEnable;
        public TextureImporterType ImportType;
        public TextureImporterFormat StandaloneFormat;
        public TextureImporterFormat AndroidFormat;
        public TextureImporterFormat IosFormat;
        public int SizeIndex;
        public string SizeStr;
        public bool WidthAndHeight;
        public TextureOverviewMode Mode;

        protected List<object> _object = new List<object>();

        public static TextureOverviewData create(TextureOverviewMode mode, TextureInfo texInfo)
        {
            TextureOverviewData retData = new TextureOverviewData();
            retData.Mode = mode;
            retData.ReadWriteEnable = texInfo.ReadWriteEnable;
            retData.MipmapEnable = texInfo.MipmapEnable;
            retData.ImportType = texInfo.ImportType;
            retData.StandaloneFormat= texInfo.StandaloneFormat;
            retData.AndroidFormat = texInfo.AndroidFormat;
            retData.IosFormat = texInfo.IosFormat;
            retData.WidthAndHeight = texInfo.Width == texInfo.Height;
            retData.SizeIndex = OverviewTableConst.GetTextureSizeIndex(texInfo.Width, texInfo.Height);
            retData.SizeStr = OverviewTableConst.TextureSizeStr[retData.SizeIndex];

            return retData;
        }

        public static void switchDataTableMode(TextureOverviewMode mode, TableView tableView)
        {
            
        }

        public bool isMatch(TextureInfo texInfo)
        {
            switch (Mode)
            {
            case TextureOverviewMode.ReadWrite:
                return ReadWriteEnable == texInfo.ReadWriteEnable;
            case TextureOverviewMode.MipMap:
                return MipmapEnable == texInfo.MipmapEnable;
            case TextureOverviewMode.Type:
                return ImportType == texInfo.ImportType;
            case TextureOverviewMode.Resolution:
                return SizeIndex == OverviewTableConst.GetTextureSizeIndex(texInfo.Width, texInfo.Height);
            case TextureOverviewMode.WidthVSHeight:
                return WidthAndHeight == (texInfo.Width == texInfo.Height);
            case TextureOverviewMode.StandaloneFormat:
                return StandaloneFormat == texInfo.StandaloneFormat;
            case TextureOverviewMode.AndroidFormat:
                return AndroidFormat == texInfo.AndroidFormat;
            case TextureOverviewMode.iOSFormat:
                return IosFormat == texInfo.IosFormat;
            }
            return false;
        }

        public void addObject(TextureInfo texInfo)
        {
            if (Mode == TextureOverviewMode.AndroidFormat)
            {
                Memory += texInfo.AndroidSize;
            }
            else if (Mode == TextureOverviewMode.iOSFormat)
            {
                Memory += texInfo.IosSize;
            }
            else if (Mode == TextureOverviewMode.StandaloneFormat)
            {
                Memory += texInfo.StandaloneSize;
            }
            else
            {
                Memory += texInfo.MemSize;
            }

            _object.Add(texInfo);
            Count++;
        }

        public List<object> getObject()
        {
            return _object;
        }
    }
}