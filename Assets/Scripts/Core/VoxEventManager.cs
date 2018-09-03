using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 노티피케이션 델리게이트(Parameter)
/// </summary>
/// <param name="sender"></param>
public delegate void Notification(object sender = null);

public class VoxEventManager : MonoBehaviour
{

    static private VoxEventManager _instance = null;
    static public VoxEventManager Instance
    {
        private set
        {
            _instance = value;
        }

        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<VoxEventManager>();
                _instance.Init();

                if (_instance == null)
                {
                    Debug.LogError("VoxEventManager를 Hierarchy 탭에서 찾지 못했습니다.");
                }
            }

            return _instance;
        }
    }

    private Dictionary<string, List<Notification>> notifyDictionary = null;

    private void Init()
    {
        notifyDictionary = new Dictionary<string, List<Notification>>();
    }

    // ----------------------------------------------------- Event Methods -----------------------------------------------------

    /// <summary>
    /// Notifycation할 이벤트 추가.
    /// 해당 이벤트가 실행될 시, 등록된 함수 실행
    /// </summary>
    /// <param name="key"></param>
    /// <param name="notify"></param>
    public void AddObserver(string key, Notification notify)
    {
        List<Notification> notifyList;
        if (!notifyDictionary.TryGetValue(key, out notifyList))
        {
            List<Notification> newNotify = new List<Notification>();
            newNotify.Add(notify);
            notifyDictionary.Add(key, newNotify);
        }
        else
        {
            notifyList.Add(notify);
        }
    }


    /// <summary>
    /// key에 등록되어있는 함수 실행.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="parameter"> 이벤트 실행 시 전달해야 할 argument가 있으면 여기로 전달. 없으면 null을 대입하면 됨. </param>
    public void PostNotifycation(string key, object parameter)
    {
        List<Notification> notifyList;
        if (notifyDictionary.TryGetValue(key, out notifyList))
        {
            if (notifyList == null)
            {
                Debug.LogError("notifyList is null");
                return;
            }
            foreach (Notification notifyMethod in notifyList)
            {
                notifyMethod(parameter);
            }
        }
    }
}
