using ResourceFormat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPreviewData : OverviewData
{
    public int Time;
    public int PassPercent;
    public string RootPath;

    public HealthPreviewData()
    {
        Time = 0;
        PassPercent = 50;
        RootPath = "ddddd/dddd";
    }

}
