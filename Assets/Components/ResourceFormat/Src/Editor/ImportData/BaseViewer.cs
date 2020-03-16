using UnityEngine;
using EditorCommon;

namespace ResourceFormat
{
    public class BaseViewer
    {
        protected TableView _dataTable;
        protected TableView _showTable;

        public virtual void OnLeave() { }
        public virtual void OnEnter() { }
        public virtual void Draw(Rect view) { }
    }
}