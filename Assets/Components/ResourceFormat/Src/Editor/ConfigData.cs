using System.Collections.Generic;
using EditorCommon;

namespace ResourceFormat
{
    public class ConfigData
    {
        private static List<AnimationImportData> _aniSelectData = null;
        private static List<ModelImportData> _modelSelectData = null;
        private static List<TextureImportData> _texSelectData = null;

        public static List<TextureImportData> TextureImportData
        {
            get
            {
                if (_texSelectData == null)
                {
                    _texSelectData = EditorTool.LoadJsonData<List<TextureImportData>>(FormatConfig.TextureImportPath);
                }

                if (_texSelectData == null)
                {
                    _texSelectData = new List<TextureImportData>();
                }

                return _texSelectData;
            }
        }

        public static List<ModelImportData> ModelSelectData
        {
            get
            {
                if (_modelSelectData == null)
                {
                    _modelSelectData = EditorTool.LoadJsonData<List<ModelImportData>>(FormatConfig.ModelImportPath);
                }

                if (_modelSelectData == null)
                {
                    _modelSelectData = new List<ModelImportData>();
                }

                return _modelSelectData;
            }
        }

        public static List<AnimationImportData> AnimationImportData
        {
            get
            {
                if (_aniSelectData == null)
                {
                    _aniSelectData = EditorTool.LoadJsonData<List<AnimationImportData>>(FormatConfig.AnimationImportPath);
                }

                if (_aniSelectData == null)
                {
                    _aniSelectData = new List<AnimationImportData>();
                }

                return _aniSelectData;
            }
        }

        public static void SaveData()
        {
            if (_texSelectData != null)
            {
                EditorTool.SaveJsonData<List<TextureImportData>>(_texSelectData, FormatConfig.TextureImportPath);
            }
            if (_modelSelectData != null)
            {
                EditorTool.SaveJsonData<List<ModelImportData>>(_modelSelectData, FormatConfig.ModelImportPath);
            }
            if (_aniSelectData != null)
            {
                EditorTool.SaveJsonData<List<AnimationImportData>>(_aniSelectData, FormatConfig.AnimationImportPath);
            }
        }
    }
}