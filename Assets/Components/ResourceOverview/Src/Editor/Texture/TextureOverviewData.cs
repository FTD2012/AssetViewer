using UnityEditor;
using System;

namespace ResourceFormat
{
    public class TextureOverviewData : OverviewData
    {
        /// Don't modify variable name
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
            }
            return false;
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