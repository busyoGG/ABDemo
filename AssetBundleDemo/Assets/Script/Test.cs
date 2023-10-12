using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("初始化");
        //加载热更代码
#if !UNITY_EDITOR
        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdate.dll.bytes"));
#else
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif
        Type hotUpdate = hotUpdateAss.GetType("Hello");
        hotUpdate.GetMethod("Run").Invoke(null, null);

        //加载模型
        Loading<Texture2D>("tex_face");
        Loading<Material>("mat_face");
        Loading<GameObject>("pre_face");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Loading<T>(string name)
    {
        AsyncOperationHandle<T> handle;
        handle = Addressables.LoadAssetAsync<T>(name);
        handle.Completed += OnLoadingEnd;
    }

    private void OnLoadingEnd<T>(AsyncOperationHandle<T> obj)
    {
        if(obj.Status == AsyncOperationStatus.Succeeded)
        {
            if(obj.Result is GameObject)
            {
                GameObject gameObject = Instantiate(obj.Result as GameObject);
                //gameObject.transform.parent = 
            }
        }
        else
        {
            Debug.LogError("加载失败");
        }
    }
}
