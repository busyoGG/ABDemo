using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class XLuaManager
{
    private static XLuaManager instance;

    private LuaEnv _luaEnv;

    private LuaTable _global;

    Dictionary<string, TextAsset> _luaDic;

    public static XLuaManager Instance()
    {
        if (instance == null)
        {
            instance = new XLuaManager();
        }
        return instance;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        _luaEnv = new LuaEnv();
        _luaDic = new Dictionary<string, TextAsset>();
        _luaEnv.AddLoader(CustomLoader);

        _global = _luaEnv.Global;
    }

    private byte[] CustomLoader(ref string filename)
    {
        TextAsset lua = _luaDic[filename];
        return lua.bytes;
    }

    public void AddXLuaDic(string key,TextAsset value)
    {
        _luaDic.Add(key, value);
    }

    /// <summary>
    /// 获得热更代码中的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T Get<T>(string key)
    {
        T res = _global.Get<T>(key);
        return res;
    }

    public void Require(string fileName)
    {
        _luaEnv.DoString($"require '{fileName}'");
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
        _luaEnv.Dispose();
    }
}
