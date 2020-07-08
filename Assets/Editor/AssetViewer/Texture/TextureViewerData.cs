using UnityEditor;
using System;
using UnityEngine;

namespace AssetViewer
{
    public class TextureOverviewData : OverviewData
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

        private TextureOverviewMode _mode;

        public TextureOverviewData(string mode, TextureInfo texInfo)
        {
            _mode = (TextureOverviewMode)Enum.Parse(typeof(TextureOverviewMode), mode);
            ReadWriteEnable = texInfo.ReadWriteEnable;
            MipmapEnable = texInfo.MipmapEnable;
            ImportType = texInfo.ImportType;
            StandaloneFormat = texInfo.StandaloneFormat;
            AndroidFormat = texInfo.AndroidFormat;
            IosFormat = texInfo.IosFormat;
            WidthAndHeight = texInfo.Width == texInfo.Height;
            SizeIndex = OverviewTableConst.GetTextureSizeIndex(texInfo.Width, texInfo.Height);
            SizeStr = OverviewTableConst.TextureSizeStr[SizeIndex];
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
                case TextureOverviewMode.FilterMode:
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
                    case TextureOverviewMode.ReadWrite:
                        count += texInfo.ReadWriteEnable == (bool)obj ? 1 : 0;
                        break;
                    case TextureOverviewMode.MipMap:
                        count += texInfo.MipmapEnable == (bool)obj ? 1 : 0;
                        break;
                    case TextureOverviewMode.Resolution:
                        count += texInfo.Width * texInfo.Height >= (int)obj ? 1 : 0;
                        break;
                    case TextureOverviewMode.FilterMode:
                        count += texInfo.FilterMode == (FilterMode)obj ? 1 : 0;
                        break;
                    case TextureOverviewMode.Type:
                        count += texInfo.ImportType == (TextureImporterType)obj ? 1 : 0;
                        break;
                    case TextureOverviewMode.WidthVSHeight:
                        count += (texInfo.Width == texInfo.Height) == (bool)obj ? 1 : 0;
                        break;
                    case TextureOverviewMode.StandaloneFormat:
                        count += texInfo.StandaloneFormat == (TextureImporterFormat)obj ? 1 : 0;
                        break;
                    case TextureOverviewMode.AndroidFormat:
                        count += texInfo.AndroidFormat == (TextureImporterFormat)obj ? 1 : 0;
                        break;
                    case TextureOverviewMode.iOSFormat:
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
            if (_mode == TextureOverviewMode.AndroidFormat)
            {
                Memory += texInfo.AndroidSize;
            }
            else if (_mode == TextureOverviewMode.iOSFormat)
            {
                Memory += texInfo.IosSize;
            }
            else if (_mode == TextureOverviewMode.StandaloneFormat)
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