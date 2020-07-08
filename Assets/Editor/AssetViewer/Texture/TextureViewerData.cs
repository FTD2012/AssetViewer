using UnityEditor;
using System;
using UnityEngine;

namespace AssetViewer
{
    public class TextureViewerData : ViewerData
    {
        /// Don't modify variable name
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
        public int Width;
        public int Height;
        public FilterMode FilterMode;

        private TextureViewerMode _mode;

        public TextureViewerData(string mode, TextureInfo texInfo)
        {
            _mode = (TextureViewerMode)Enum.Parse(typeof(TextureViewerMode), mode);
            ReadWriteEnable = texInfo.ReadWriteEnable;
            MipmapEnable = texInfo.MipmapEnable;
            ImportType = texInfo.ImportType;
            StandaloneFormat = texInfo.StandaloneFormat;
            AndroidFormat = texInfo.AndroidFormat;
            IosFormat = texInfo.IosFormat;
            WidthAndHeight = texInfo.Width == texInfo.Height;
            SizeIndex = ViewerConst.GetTextureSizeIndex(texInfo.Width, texInfo.Height);
            SizeStr = ViewerConst.TextureSizeStr[SizeIndex];
            Width = texInfo.Width;
            Height = texInfo.Height;
            FilterMode = texInfo.FilterMode;
        }

        public override bool IsMatch(BaseInfo texInfo)
        {
            return isMatch((TextureInfo)texInfo);
        }

        private bool isMatch(TextureInfo texInfo)
        {
            switch (_mode)
            {
                case TextureViewerMode.ReadWrite:
                    return ReadWriteEnable == texInfo.ReadWriteEnable;
                case TextureViewerMode.MipMap:
                    return MipmapEnable == texInfo.MipmapEnable;
                case TextureViewerMode.Type:
                    return ImportType == texInfo.ImportType;
                case TextureViewerMode.Resolution:
                    return SizeIndex == ViewerConst.GetTextureSizeIndex(texInfo.Width, texInfo.Height);
                case TextureViewerMode.WidthVSHeight:
                    return WidthAndHeight == (texInfo.Width == texInfo.Height);
                case TextureViewerMode.StandaloneFormat:
                    return StandaloneFormat == texInfo.StandaloneFormat;
                case TextureViewerMode.AndroidFormat:
                    return AndroidFormat == texInfo.AndroidFormat;
                case TextureViewerMode.iOSFormat:
                    return IosFormat == texInfo.IosFormat;
                case TextureViewerMode.FilterMode:
                    return FilterMode == texInfo.FilterMode;
            }
            return false;
        }

        public override int GetMatchHealthCount(object obj)
        {
            int count = 0;
            foreach (TextureInfo texInfo in _object)
            {
                switch (_mode)
                {
                    case TextureViewerMode.ReadWrite:
                        count += texInfo.ReadWriteEnable == (bool)obj ? 1 : 0;
                        break;
                    case TextureViewerMode.MipMap:
                        count += texInfo.MipmapEnable == (bool)obj ? 1 : 0;
                        break;
                    case TextureViewerMode.Resolution:
                        count += texInfo.Width * texInfo.Height >= (int)obj ? 1 : 0;
                        break;
                    case TextureViewerMode.FilterMode:
                        count += texInfo.FilterMode == (FilterMode)obj ? 1 : 0;
                        break;
                    case TextureViewerMode.Type:
                        count += texInfo.ImportType == (TextureImporterType)obj ? 1 : 0;
                        break;
                    case TextureViewerMode.WidthVSHeight:
                        count += (texInfo.Width == texInfo.Height) == (bool)obj ? 1 : 0;
                        break;
                    case TextureViewerMode.StandaloneFormat:
                        count += texInfo.StandaloneFormat == (TextureImporterFormat)obj ? 1 : 0;
                        break;
                    case TextureViewerMode.AndroidFormat:
                        count += texInfo.AndroidFormat == (TextureImporterFormat)obj ? 1 : 0;
                        break;
                    case TextureViewerMode.iOSFormat:
                        count += texInfo.IosFormat == (TextureImporterFormat)obj ? 1 : 0;
                        break;
                }
            }
            return count;
        }

        public override void AddObject(BaseInfo texInfo)
        {
            addObject((TextureInfo)texInfo);
        }

        private void addObject(TextureInfo texInfo)
        {
            if (_mode == TextureViewerMode.AndroidFormat)
            {
                Memory += texInfo.AndroidSize;
            }
            else if (_mode == TextureViewerMode.iOSFormat)
            {
                Memory += texInfo.IosSize;
            }
            else if (_mode == TextureViewerMode.StandaloneFormat)
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
    }
}