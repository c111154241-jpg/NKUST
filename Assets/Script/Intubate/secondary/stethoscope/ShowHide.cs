using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHide : MonoBehaviour
{   
    //獲得手的Capsules用來顯示聽診器
    public List<Transform> capsulesChildren = new List<Transform>();
    [SerializeField] private Stethoscope stethoscope;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject cylinder;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform parent;
    public Hand handScript;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        parent = parent.parent;
    }


    // Update is called once per frame
    void Update()
    {
        if (capsulesChildren.Count <= 0)
        {
            foreach (Transform Capsules in parent)
            {
                // Debug.LogError(Capsules.name);
                if (Capsules.name == "Capsules")
                {
                    foreach (Transform child in Capsules)
                    {
                        capsulesChildren.Add(child);
                    }
                    target = capsulesChildren[0].gameObject;
                    if (target.GetComponent<Hand>() == null)
                    {
                        // 动态添加 Hand 脚本
                        target.AddComponent<Hand>();
                        handScript = target.GetComponent<Hand>();
                        handScript.stethoscope = stethoscope;

                        BoxCollider boxCollider = target.AddComponent<BoxCollider>();
                        boxCollider.size = new Vector3((float)0.1, (float)0.1, (float)0.1);  // 设置碰撞体的大小
                        boxCollider.center = new Vector3((float)0.075, 0, 0);  // 设置碰撞体的位置
                    }
                    break;
                }
                else
                {
                    LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                    if (lineRenderer != null)
                        lineRenderer.enabled = false;
                    cylinder.SetActive(false);
                }
            }
        }
        else
        {
            if (!capsulesChildren[0].gameObject.activeSelf)
            {
                LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                    lineRenderer.enabled = false;
                cylinder.SetActive(false);
            }
            else
            {
                LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                    lineRenderer.enabled = true;
                cylinder.SetActive(true);
            }
        }
    }
}

