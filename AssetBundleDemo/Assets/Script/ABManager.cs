using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class ABManager
{
    private static ABManager instance;

    //private int _loadingId = 0;

    private Dictionary<string, Action<object>> _loadingDic = new Dictionary<string, Action<object>>();

    public static ABManager Instance()
    {
        if (instance == null)
        {
            instance = new ABManager();
        }
        return instance;
    }

    public void Loading<T>(string name, Action<object> callback = null)
    {
        AsyncOperationHandle<T> handle;
        handle = Addressables.LoadAssetAsync<T>(name);
        handle.Completed += OnLoadingEnd;

        Debug.Log("加载" + handle.DebugName);
        _loadingDic.Add(handle.DebugName, callback);
    }

    private void OnLoadingEnd<T>(AsyncOperationHandle<T> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log(obj.DebugName + "加载完成");
            if (_loadingDic[obj.DebugName] != null)
            {
                _loadingDic[obj.DebugName](obj.Result);
            }
        }
        else
        {
            Debug.LogError("加载失败");
        }
    }
}
