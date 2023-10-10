using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
            Debug.LogError("º”‘ÿ ß∞‹");
        }
    }
}
