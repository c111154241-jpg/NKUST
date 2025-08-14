using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // 確保導入 UI 命名空間

public class Elevator : MonoBehaviour
{
    [SerializeField] private IntuGameManage intuGameManage;

    [Header("顯示器")]
    [SerializeField] private Animator monitorAnimator;

    [Header("門")]
    [SerializeField] private GameObject elevatorRight;
    [SerializeField] private GameObject elevatorLeft;
    [SerializeField] private float doorMoveDistance = 2.0f; // 門開啟的距離
    [SerializeField] private float doorOpenSpeed = 1.0f; // 門開啟速度
    [SerializeField] private Vector3 deviation;
    private Vector3 rightDoorStartPos;
    private Vector3 leftDoorStartPos;

    void Start()
    {
        rightDoorStartPos = elevatorRight.transform.position;
        leftDoorStartPos = elevatorLeft.transform.position;
    }
    void Update()
    {
        if (intuGameManage == null) 
        {
            intuGameManage = FindObjectOfType<IntuGameManage>();
        }
        if (intuGameManage != null && intuGameManage.endFlag3)
        {
            monitorAnimator.enabled = true;
            IsAnimationFinished();
        }
    }
    void IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = monitorAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1 && !monitorAnimator.IsInTransition(0))
        {
            OpenDoors();
        }
    }
    void OpenDoors()
    {
        // 門分別向相反方向移動
        StartCoroutine(MoveDoor(elevatorRight, leftDoorStartPos + Vector3.left * doorMoveDistance + deviation));
        StartCoroutine(MoveDoor(elevatorLeft, rightDoorStartPos + Vector3.right * doorMoveDistance - deviation));
    }

    IEnumerator MoveDoor(GameObject door, Vector3 targetPos)
    {
        float elapsedTime = 0f;
        Vector3 startPos = door.transform.position;

        while (elapsedTime < doorOpenSpeed)
        {
            door.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / doorOpenSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.position = targetPos; // 確保最終位置正確
    }
}
