using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;

[LuaCallCSharp]
public class Test : MonoBehaviour
{
    private int _loadingCount = 3;

    [CSharpCallLua]
    public delegate int Add(int a,int b);
    [CSharpCallLua]
    public delegate int gen(int arg);

    // Start is called before the first frame update
    void Start()
    {
        XLuaManager.Instance().Init();
        ABManager.Instance().Loading<TextAsset[]>("lua", StartGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartGame(object res)
    {
        //加载热更脚本
        TextAsset[] tas = (TextAsset[])res;
        foreach (TextAsset ta in tas)
        {
            XLuaManager.Instance().AddXLuaDic(ta.name, ta);
            XLuaManager.Instance().Require(ta.name);
        }

        

        ITest test = XLuaManager.Instance().Get<ITest>("test");
        test.Run();

        ABManager.Instance().Loading<Texture2D>("tex_face", LoadingCallback);
        ABManager.Instance().Loading<Material>("mat_face", LoadingCallback);
        ABManager.Instance().Loading<GameObject>("pre_face", LoadingCallback);

        //        XLuaManager.Instance().DoString(@"xlua.hotfix(CS.ITest, 'Run', function(self)
        //    print('hotfix')
        //end)");

        TestFunc();

        Add add = XLuaManager.Instance().Get<Add>("Add");

        int result = add(1, 2);
        Debug.Log("结果" + result);

        gen g = XLuaManager.Instance().Get<gen>("GenericTest");

        g(2);
    }

    private void LoadingCallback(object res)
    {
        _loadingCount--;
        if (_loadingCount <= 0)
        {
            InitScene();
        }
    }

    private void InitScene()
    {
        Addressables.InstantiateAsync("pre_face");
    }

    public void TestFunc()
    {

    }

    public static void generic<T>(T arg)
    {
        Debug.Log("泛型测试" + arg);
    }
}
