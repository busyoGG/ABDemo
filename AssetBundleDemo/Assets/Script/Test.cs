using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using XLua;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Test : MonoBehaviour
{
    LuaEnv _luaEnv;
    private bool _isLuaLoaded = false;
    Dictionary<string,TextAsset> _luaDic = new Dictionary<string,TextAsset>();

    // Start is called before the first frame update
    void Start()
    {
        //加载lua文件
        StartCoroutine(LuaLoading());
        //启动lua
        StartCoroutine(LuaStart());

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

    private IEnumerator LuaLoading()
    {
        AsyncOperationHandle<TextAsset> handle;
        handle = Addressables.LoadAssetAsync<TextAsset>("hello");
        handle.Completed += (AsyncOperationHandle<TextAsset> obj) =>
        {
            TextAsset res = obj.Result;
            _luaDic.Add(res.name, res);
        };
        yield return handle;
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("添加客制化loader");
            _luaEnv = new XLua.LuaEnv();
            _luaEnv.AddLoader(CustomLoader);
            _isLuaLoaded = true;
            StopCoroutine(LuaLoading());
        }
        else
        {
            Debug.LogError("Lua资源加载失败");
        }
    }

    private IEnumerator LuaStart()
    {
        while(_isLuaLoaded == false)
        {
            yield return _isLuaLoaded == false;
        }
        Debug.Log("执行Lua");

        //加载xLua
        _luaEnv.DoString("require 'hello.lua'");
        ITest test = _luaEnv.Global.Get<ITest>("test");
        Debug.Log(test);
        test.Run();
        //释放
        _luaEnv.Dispose();
        //test.Run();

    }

    private byte[] CustomLoader(ref string filename)
    {
        //string path = Application.persistentDataPath + "/" + filename;
        //return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(path));
        //Debug.Log("客制化loader" + _luaDic);
        TextAsset lua = _luaDic[filename];
        return lua.bytes;
    }
}
