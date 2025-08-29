using System;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    private Dictionary<string, object> data = new Dictionary<string, object>();

    /// <summary>
    /// 设置一个键值对，支持任意类型
    /// </summary>
    public void Set<T>(string key, T value)
    {
        if (data.ContainsKey(key))
        {
            data[key] = value;
        }
        else
        {
            data.Add(key, value);
        }

#if UNITY_EDITOR
        Debug.Log($"[Blackboard] Set: {key} = {value} ({typeof(T)})");
#endif
    }

    /// <summary>
    /// 获取指定键的值，若不存在则返回默认值
    /// </summary>
    public T Get<T>(string key, T defaultValue = default)
    {
        if (data.TryGetValue(key, out object value))
        {
            if (value is T typedValue)
            {
                return typedValue;
            }
            else
            {
                Debug.LogWarning($"[Blackboard] 类型不匹配: {key} 期望类型 {typeof(T)}, 实际类型 {value.GetType()}");
                return defaultValue;
            }
        }

        return defaultValue;
    }

    /// <summary>
    /// 是否包含指定键
    /// </summary>
    public bool HasKey(string key)
    {
        return data.ContainsKey(key);
    }

    /// <summary>
    /// 移除指定键
    /// </summary>
    public void Remove(string key)
    {
        if (data.ContainsKey(key))
        {
            data.Remove(key);
        }
    }

    /// <summary>
    /// 清空所有数据
    /// </summary>
    public void Clear()
    {
        data.Clear();
    }

    /// <summary>
    /// 获取所有键（用于调试）
    /// </summary>
    public IEnumerable<string> Keys => data.Keys;
}

public class BlackboardTest : MonoBehaviour
{
    private Blackboard blackboard = new Blackboard();

    void Start()
    {
        blackboard.Set("PlayerHealth", 100);
        blackboard.Set("IsAlive", true);
        blackboard.Set("PlayerName", "勇者");
        blackboard.Set("Position", new Vector3(1, 2, 3));

        int health = blackboard.Get<int>("PlayerHealth");
        bool isAlive = blackboard.Get<bool>("IsAlive");
        string name = blackboard.Get<string>("PlayerName");
        Vector3 pos = blackboard.Get<Vector3>("Position");

        Debug.Log($"玩家 {name} 血量: {health}, 存活: {isAlive}, 位置: {pos}");
    }
}