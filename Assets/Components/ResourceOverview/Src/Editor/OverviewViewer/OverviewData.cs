using EditorCommon;
using System.Collections.Generic;
using CommonComponent;
using System;

namespace ResourceFormat
{
    public class OverviewData
    {
        protected List<object> _object = new List<object>();

        public virtual bool IsMatch(BaseInfo texInfo)
        {
            throw new NotImplementedException();
        }
        public virtual void AddObject(BaseInfo texInfo)
        {
            throw new NotImplementedException();
        }
        public List<object> GetObject()
        {
            return _object;
        }
    }

    public class OverviewModeManager : Singleton<OverviewModeManager>
    {
        public virtual string[] GetMode()
        {
            throw new NotImplementedException();
        }

        public virtual ColumnType[] GetDataTable(string overviewMode)
        {
            throw new NotImplementedException();
        }

        public virtual ColumnType[] GetShowTable(string overviewMode)
        {
            throw new NotImplementedException();
        }
    }


}
