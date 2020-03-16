using EditorCommon;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
    public class ResourceFormatWin : EditorWindow
    {
        private ModeManager _modeManager;

        [MenuItem("UComponents/ResourceFormatWin " + EditorHotkeys.Ctrl_ + "F")]
        static void Create()
        {
            ResourceFormatWin resourceFormatWin = GetWindow<ResourceFormatWin>();
            resourceFormatWin.minSize = new Vector2(1000, 800);
            resourceFormatWin.titleContent = new GUIContent("ResFormat", (Texture)Resources.Load("icon1"));    /// TODO: ljm >>> refractor load meathod
        }

        void OnEnable()
        {
            if (_modeManager == null)
            {
                _modeManager = new ModeManager(this);
            }
        }

        void OnGUI()
        {
            _modeManager.OnGUI(position);
        }
    }
}