using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UIManager;

public class GMCmd
{
    [MenuItem("GMCmd/��ȡ���")]
    public static void ReadTable()
    {
        PackageTable packageTable = Resources.Load<PackageTable>
            ("TableData/PackageTable");
        foreach(PackageTableItem packageItem in packageTable.DataList)
        {
            Debug.Log(string.Format("��id��:{0},��name��:{1},��Type��:{2}",packageItem.id,packageItem.name,packageItem.type));
        }
    }

    [MenuItem("GMCmd/�����������ݲ���")]
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

    [MenuItem("GMCmd/��ȡ�������ݲ���")]
    public static void ReadLocalPackageData()
    {
        List<PackageLocalItem> readItems = PackageLocalData.Instance.LoadPackage();
        foreach (PackageLocalItem itme in readItems)
        {
            Debug.Log(itme);
        }
    }

    [MenuItem("GMCmd/�򿪱�������")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.BagMenu);
    }

    [MenuItem("GMCmd/�رձ�������")]
    public static void ClosePackagePanel()
    {
        UIManager.Instance.ClosePanel(UIConst.BagMenu);
    }
}
