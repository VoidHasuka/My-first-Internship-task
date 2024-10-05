using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IPoolable
{
    void OnObjectReuse();    // ����������ʹ��ʱ����״̬
    void OnObjectReturn();   // �����󱻻��յ���ʱִ�еĲ���
}

public static class ObjectPoolManager
{
    //�о����ϣ��������֮��
    private static Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private static Dictionary<string, int> maxPoolSize = new Dictionary<string, int>();

    



    // ���ö��������С
    public static void SetPoolSize(GameObject prefab, int size)
    {
        string key = prefab.name;

        if (!maxPoolSize.ContainsKey(key))
        {
            maxPoolSize[key] = size;
        }
    }

    // �Ӷ���ػ�ȡ����
    public static GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string key = prefab.name;

        if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
        {
            GameObject obj = poolDictionary[key].Dequeue();
            

            // ���ö��������״̬
            IPoolable poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.OnObjectReuse();
            }

            //֮�������λ�ýǶ�
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            return obj;
        }

        // �������Ϊ����Instantiate
        GameObject newObject = Object.Instantiate(prefab, position, rotation);
        newObject.name = key;  // ȷ�������ʶ����ͬ��key
        return newObject;
    }

    // ���ն��󵽶����
    public static void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);  // ���ض���
        string key = obj.name;

        // ���ö��������״̬
        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnObjectReturn();
        }

        // ����������û���������ĳ��ӣ�����һ��
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        // ��̬�ش�С�����������ش�Сʱ���ٶ���
        if (poolDictionary[key].Count < maxPoolSize[key])
        {
            poolDictionary[key].Enqueue(obj);
        }
        else
        {
            Object.Destroy(obj);  // ����������������ٶ���
        }
    }

    // ��ն���أ���Ȼ�͵����ټ��س����ı����
    public static void ClearPool()
    {
        foreach (var key in poolDictionary.Keys)
        {
            while (poolDictionary[key].Count > 0)
            {
                Object.Destroy(poolDictionary[key].Dequeue());
            }
        }
        poolDictionary.Clear(); // ��ճ�
        maxPoolSize.Clear();
    }

    //��Ȼ�ҶԼ����¼������е��Ժ�
    static ObjectPoolManager()
    {
        // ע�᳡�������¼�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ȡ���¼�
    private static void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearPool();  // ��ÿ�γ�������ʱ��ն����
    }
}
