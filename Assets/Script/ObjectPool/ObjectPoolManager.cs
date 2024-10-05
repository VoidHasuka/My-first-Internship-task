using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IPoolable
{
    void OnObjectReuse();    // 当对象被重新使用时重置状态
    void OnObjectReturn();   // 当对象被回收到池时执行的操作
}

public static class ObjectPoolManager
{
    //感觉与哈希表有相像之处
    private static Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private static Dictionary<string, int> maxPoolSize = new Dictionary<string, int>();

    



    // 设置对象池最大大小
    public static void SetPoolSize(GameObject prefab, int size)
    {
        string key = prefab.name;

        if (!maxPoolSize.ContainsKey(key))
        {
            maxPoolSize[key] = size;
        }
    }

    // 从对象池获取对象
    public static GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string key = prefab.name;

        if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
        {
            GameObject obj = poolDictionary[key].Dequeue();
            

            // 调用对象的重置状态
            IPoolable poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.OnObjectReuse();
            }

            //之后才设置位置角度
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            return obj;
        }

        // 如果池子为空则Instantiate
        GameObject newObject = Object.Instantiate(prefab, position, rotation);
        newObject.name = key;  // 确保对象池识别相同的key
        return newObject;
    }

    // 回收对象到对象池
    public static void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);  // 隐藏对象
        string key = obj.name;

        // 调用对象的清理状态
        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnObjectReturn();
        }

        // 如果对象池中没有这个对象的池子，创建一个
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        // 动态池大小管理，超过最大池大小时销毁对象
        if (poolDictionary[key].Count < maxPoolSize[key])
        {
            poolDictionary[key].Enqueue(obj);
        }
        else
        {
            Object.Destroy(obj);  // 如果池子已满，销毁对象
        }
    }

    // 清空对象池，不然就等着再加载场景的报错吧
    public static void ClearPool()
    {
        foreach (var key in poolDictionary.Keys)
        {
            while (poolDictionary[key].Count > 0)
            {
                Object.Destroy(poolDictionary[key].Dequeue());
            }
        }
        poolDictionary.Clear(); // 清空池
        maxPoolSize.Clear();
    }

    //虽然我对监听事件还是有点迷糊
    static ObjectPoolManager()
    {
        // 注册场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 取消事件
    private static void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearPool();  // 在每次场景加载时清空对象池
    }
}
