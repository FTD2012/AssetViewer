using System.Collections.Generic;

namespace ResourceFormat
{
    public class ImportData
    {
        public string RootPath = "";
        public string FileNameMatch = "*.*";
        public int Index = -1;
        public int TotalCount = 0;
        public int TotalMemuse = 0;
        public bool PreBuild = true;

        protected List<object> _object = new List<object>();
        protected List<object> _unFortmatObject = new List<object>();

        public virtual bool IsMatch(string path)
        {
            return ImportRegex.IsMatch(this, path);
        }

        public virtual void CopyData(ImportData data)
        {
            RootPath = data.RootPath;
            FileNameMatch = data.FileNameMatch;
            PreBuild = data.PreBuild;
        }
        public virtual List<object> GetObject()
        {
            return _object;
        }

        public virtual void ClearObject()
        {
            _object.Clear();
            _unFortmatObject.Clear();
            TotalCount = 0;
            TotalMemuse = 0;
        }
    }
}