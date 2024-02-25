using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager _instance;
    private Dictionary<string, string> pathDict;
    private Dictionary<string, GameObject> prefabDict;
    public Dictionary<string, BasePanel> panelDict;
    private Transform _uiRoot;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    public Transform UIRoot
    {
        get
        {
            if ( _uiRoot == null)
            {
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }

    private UIManager()
    {
        InitDicts();
    }

    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();
        pathDict = new Dictionary<string, string>()
        {
            { UIConst.BagMenu,"BagMenu"},
            { UIConst.LotteryPanel,"LotteryPanel"},
        };
    }

    public BasePanel GetPanel(string name)
    {
        BasePanel panel = null;
        // 检查是否已打开
        if (panelDict.TryGetValue(name, out panel))
        {
            return panel;
        }
        return null;
    }


    public BasePanel OpenPanel(string name)
    {
        BasePanel panel;
        if(panelDict.TryGetValue(name,out panel))
        {
            Debug.LogError("界面已打开:" + name);
            return null;
        }

        string path = "";
        if(!pathDict.TryGetValue(name,out path))
        {
            Debug.LogError("界面名称错误，或未配置:" + name);
            return null;
        }

        GameObject panelPrefab = null;
        if(!prefabDict.TryGetValue(name,out panelPrefab))
        {
            string realPath = "Prefab/Panle/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
            if (panelPrefab == null)
            {
                Debug.Log(name+realPath);
            }
        }
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        panel.OpenPanel(name);
        return panel;
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel;
        if(!panelDict.TryGetValue(name,out panel))
        {
            Debug.LogError("界面未打开:" + name);
            return false;
        }

        panel.ClosePanel();
        return true;
    }

    public class UIConst
    {
        public const string BagMenu = "BagMenu";
        public const string LotteryPanel = "LotteryPanel";
    }
}
