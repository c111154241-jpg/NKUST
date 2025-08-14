using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit.SceneDecorator;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [Header("視角")]
    [SerializeField] private GameObject MyCamera;
    [SerializeField] private float Offset;

    [Header("人")]
    [SerializeField] private Animator humanAnimator;

    [Header("移動")]
    [SerializeField] Speak3 speak;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask layer;
    [SerializeField] LayerMask layerRotate;
    [SerializeField] HandGrabInteractable handGrabInteractable;
    [Header("旋轉")]
    [SerializeField] float rotationSpeed;
    private Quaternion targetRotation;
    private bool isRotating = false; // 紀錄是否已經開始旋轉
    [Header("顯示器:動畫")]
    [SerializeField] private Animator animator;
    private Vector3 cameraOffset; // 存儲攝影機相對 Bed 的偏移
    [Header("結束UI")]
    [SerializeField] private GameObject endUI;
    [SerializeField] private End end;
    void Start()
    {
        cameraOffset = MyCamera.transform.position - transform.position; // 記錄相對位置
        targetRotation = Quaternion.Euler(0f, 90f, 0f);
    }

    void Update()
    {
        MyCamera.transform.position = transform.position * Offset + cameraOffset;
        if (speak == null)
        {
            speak = FindObjectOfType<Speak3>();
        }
        if (transform.localPosition.z >= 2.8f && !isRotating)
        {
            StartCoroutine(RotateBed()); // 啟動旋轉協程
            humanAnimator.enabled = true;
            handGrabInteractable.enabled = false; rb.excludeLayers = layerRotate;

        }
        // if (transform.localPosition.z >= 2.8f)
        // {
        //     rb.excludeLayers = layerRotate;
        // }
        else if (speak != null && speak.count >= 10 && IsAnimationFinished(animator))
        {
            rb.excludeLayers = layer;
        }
        if (speak != null && speak.count >= 10 && IsAnimationFinished(humanAnimator))
        {
            endUI.SetActive(true);
            end.Quest();
        }


    }
    bool IsAnimationFinished(Animator animator)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1 && !animator.IsInTransition(0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    IEnumerator RotateBed()
    {
        isRotating = true; // 設為正在旋轉，避免重複啟動協程

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null; // 等待下一幀
        }

        transform.rotation = targetRotation; // 確保最終角度精確到目標旋轉角度
        isRotating = false; // 旋轉完成後重設
    }
}

