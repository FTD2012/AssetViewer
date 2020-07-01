using EditorCommon;
using System.Collections.Generic;
using CommonComponent;
using System;
using UnityEngine;

namespace ResourceFormat
{
    public struct Health
    {
        public enum HealthEnum
        {
            NONE = 0, // Only have tip
            INFO = 1,
            WARNING = 2,
            ERROR = 3
        }

        public HealthEnum State;
        public string Desc;
        public int Threshold;
        public int Value;

        public Health(HealthEnum state, string desc, int threshold, int value)
        {
            State = state;
            Desc = desc;
            Threshold = threshold;
            Value = value;
        }
    }

    public class HealthInfoManager : Singleton<HealthInfoManager>
    {
        public struct HealthInfoMeta
        {
            public bool EnableCondition;
            public List<object> ConditionList;
            public string Tip;
            public int Threshold;
            public HealthInfoMeta(bool enableCondition,string tip, int threshold, List<object> conditionList)
            {
                EnableCondition = enableCondition;
                ConditionList = conditionList;
                Tip = tip;
                Threshold = threshold;
            }
        }

        protected Dictionary<string, HealthInfoMeta> healthInfoMetaDic;

        public HealthInfoManager()
        {
            healthInfoMetaDic = new Dictionary<string, HealthInfoMeta>();
        }

        public void AddHealthInfo(string modeEnum, bool enableConditon, string tip, int threshold, List<object> conditionList)
        {
            if (healthInfoMetaDic.ContainsKey(modeEnum))
            {
                Debug.LogWarning("Append same healdinfo.");
            }

            healthInfoMetaDic[modeEnum] = new HealthInfoMeta(enableConditon, tip, threshold, conditionList);
        }

        public void Clear()
        {
            healthInfoMetaDic.Clear();
        }

        public bool GetEnableCondition(string modeEnum)
        {
            if (!healthInfoMetaDic.ContainsKey(modeEnum))
            {
                return false;
            }
            return healthInfoMetaDic[modeEnum].EnableCondition;
        }

        public string GetTip(string modeEnum)
        {
            if (!healthInfoMetaDic.ContainsKey(modeEnum))
            {
                return "Empty describle";
            }
            return healthInfoMetaDic[modeEnum].Tip;
        }

        public int GetThreshold(string modeEnum)
        {
            if (!healthInfoMetaDic.ContainsKey(modeEnum))
            {
                return 0;
            }
            return healthInfoMetaDic[modeEnum].Threshold;
        }

        public List<object> GetConditionList(string modeEnum)
        {
            if (!healthInfoMetaDic.ContainsKey(modeEnum))
            {
                return null;
            }
            return healthInfoMetaDic[modeEnum].ConditionList;
        }
    }


    public class OverviewData
    {
        public int Count;

        protected List<object> _object = new List<object>();

        public virtual bool IsMatch(BaseInfo texInfo)
        {
            throw new NotImplementedException();
        }

        public virtual int GetMatchHealthCount(object obj)
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
