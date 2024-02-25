using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UIManager;

public class GMCmd
{
    [MenuItem("GMCmd/读取表格")]
    public static void ReadTable()
    {
        PackageTable packageTable = Resources.Load<PackageTable>
            ("TableData/PackageTable");
        foreach(PackageTableItem packageItem in packageTable.DataList)
        {
            Debug.Log(string.Format("【id】:{0},【name】:{1},【Type】:{2}",packageItem.id,packageItem.name,packageItem.type));
        }
    }

    [MenuItem("GMCmd/创建背包数据测试")]
    public static void CreateLocalPackageData()
    {
        PackageLocalData.Instance.items = new List<PackageLocalItem> ();
        for (int i = 1; i < 6; i++) 
        {
            PackageLocalItem packageLocalItme = new()
            {
                uid=Guid.NewGuid().ToString(),
                id=i,
                num=i,
                level=i,
                isNew=i % 2 == 1
            };
            PackageLocalData.Instance.items.Add(packageLocalItme);
        }
        PackageLocalData.Instance.SavePackage();
    }

    [MenuItem("GMCmd/读取背包数据测试")]
    public static void ReadLocalPackageData()
    {
        List<PackageLocalItem> readItems = PackageLocalData.Instance.LoadPackage();
        foreach (PackageLocalItem itme in readItems)
        {
            Debug.Log(itme);
        }
    }

    [MenuItem("GMCmd/打开背包测试")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.BagMenu);
    }

    [MenuItem("GMCmd/关闭背包测试")]
    public static void ClosePackagePanel()
    {
        UIManager.Instance.ClosePanel(UIConst.BagMenu);
    }
}
