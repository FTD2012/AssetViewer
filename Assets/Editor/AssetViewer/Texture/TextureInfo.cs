using EditorCommon;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AssetViewer
{
    public enum TextureType
    {
        Default,
        NormalMap,
        Lightmap
    }

    public enum TextureAlphaMode
    {
        FormTexture,
        None,
    }

    public class TextureInfo : BaseInfo
    {
        public bool ReadWriteEnable = false;
        public bool MipmapEnable = false;
        public TextureImporterFormat StandaloneFormat;
        public TextureImporterFormat AndroidFormat;
        public TextureImporterFormat IosFormat;
        public TextureImporterType ImportType;
        public TextureWrapMode WrapMode;
        public FilterMode FilterMode;
        public TextureImporterShape ImportShape;
        public int StandaloneSize;
        public int AndroidSize;
        public int IosSize;
        public int Width;
        public int Height;
        public bool StandaloneOverriden;
        public bool AndroidOverriden;
        public bool IosOverriden;

        private static int _loadCount = 0;
        private static Dictionary<string, TextureInfo> _dictTexInfo = new Dictionary<string, TextureInfo>();

        public static TextureInfo CreateTextureInfo(string assetPath)
        {
            if (!EditorPath.IsTexture(assetPath))
            {
                return null;
            }

            TextureInfo textureInfo = null;
            if (!_dictTexInfo.TryGetValue(assetPath, out textureInfo))
            {
                textureInfo = new TextureInfo();
                _dictTexInfo.Add(assetPath, textureInfo);
            }

            TextureImporter textureImport = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
            if (textureImport == null || texture == null)
            {
                return null;
            }

            textureInfo.Path = textureImport.assetPath;
            textureInfo.ImportType = textureImport.textureType;
            textureInfo.ImportShape = textureImport.textureShape;
            textureInfo.ReadWriteEnable = textureImport.isReadable;
            textureInfo.MipmapEnable = textureImport.mipmapEnabled;
            textureInfo.WrapMode = textureImport.wrapMode;
            textureInfo.FilterMode = textureImport.filterMode;
            textureInfo.StandaloneFormat = EditorTool.GetPlatformTextureSettings(textureImport, EditorConst.PlatformStandalone);
            textureInfo.AndroidFormat = EditorTool.GetPlatformTextureSettings(textureImport, EditorConst.PlatformAndroid);
            textureInfo.IosFormat = EditorTool.GetPlatformTextureSettings(textureImport, EditorConst.PlatformIos);
            textureInfo.StandaloneOverriden = EditorTool.IsTextureOverriden(textureImport, EditorConst.PlatformStandalone);
            textureInfo.AndroidOverriden = EditorTool.IsTextureOverriden(textureImport, EditorConst.PlatformAndroid);
            textureInfo.IosOverriden = EditorTool.IsTextureOverriden(textureImport, EditorConst.PlatformIos);
            textureInfo.StandaloneSize = EditorTool.CalculateTextureSizeBytes(texture, textureInfo.StandaloneFormat);
            textureInfo.AndroidSize = EditorTool.CalculateTextureSizeBytes(texture, textureInfo.AndroidFormat);
            textureInfo.IosSize = EditorTool.CalculateTextureSizeBytes(texture, textureInfo.IosFormat);
            textureInfo.MemSize = Mathf.Max(textureInfo.StandaloneSize, textureInfo.AndroidSize, textureInfo.IosSize);
            textureInfo.Width = texture.width;
            textureInfo.Height = texture.height;

            if (Selection.activeObject != texture)
            {
                Resources.UnloadAsset(texture);
            }

            if (++_loadCount % 256 == 0)
            {
                Resources.UnloadUnusedAssets();
            }

            return textureInfo;
        }

        public static List<TextureInfo> GetInfoByDirectory(string dir)
        {
            List<TextureInfo> texInfoList = new List<TextureInfo>();
            List<string> list = new List<string>();
            EditorPath.ScanDirectoryFile(dir, true, list);
            for (int i = 0; i < list.Count; ++i)
            {
                string assetPath = EditorPath.FormatAssetPath(list[i]);
                string name = System.IO.Path.GetFileName(assetPath);
                EditorUtility.DisplayProgressBar("获取贴图数据", name, (i * 1.0f) / list.Count);
                TextureInfo texInfo = CreateTextureInfo(assetPath);
                if (texInfo != null)
                {
                    texInfoList.Add(texInfo);
                }
            }
            EditorUtility.ClearProgressBar();
            return texInfoList;
        }
    }
}
