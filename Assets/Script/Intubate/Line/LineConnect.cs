using UnityEngine;

public class LineConnectWithFixedLength : MonoBehaviour
{
    [Header("目標")]
    public GameObject ball;
    public GameObject disk;
    private FixedJoint fixedJointBall;
    private FixedJoint fixedJointDisk;
    [Header("線")]
    public int segmentCount = 20;  // 繩子分段數量，您可以自由設定
    public float segmentLength = 0.5f;  // 每段的長度，您可以自行調整
    public float radius = 0.05f;  // 繩子的粗細
    public Material lineMaterial;  // 繩子的材質
    public GameObject segmentPrefab;  // 每個段落節點的預製物件
    private LineRenderer lineRenderer;
    private GameObject[] segments;  // 保存繩子的段落節點

    [Header("其他")]
    [SerializeField] bool flag;
    public LayerMask layerMask;

    void Start()
    {
        Line();
    }
    public void Line()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // 如果沒有 LineRenderer，就創建一個新的
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = segmentCount;
        lineRenderer.startWidth = radius;
        lineRenderer.endWidth = radius;
        lineRenderer.material = lineMaterial;

        // 創建繩子段落節點
        segments = new GameObject[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = Instantiate(segmentPrefab);

            // 設置段落的位置
            float t = i / (float)(segmentCount - 1);
            Vector3 position = Vector3.Lerp(ball.transform.position, disk.transform.position, t);
            segment.transform.position = position;

            // 將 segment 設為當前物件的子物件
            segment.transform.SetParent(this.transform);

            // 添加剛體和碰撞器
            // 檢查並添加 Rigidbody
            Rigidbody rb = segment.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = segment.AddComponent<Rigidbody>();
                rb.mass = .0005f;  // 減少質量以減少抖動
                rb.drag = 10f;  // 增加線性阻力
                rb.angularDrag = 10f;  // 增加角阻力
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.excludeLayers = layerMask;
            }

            if (i < segmentCount - 1 && i != 0)
            {
                SphereCollider collider = segment.AddComponent<SphereCollider>();
                collider.radius = radius;
            }
            // 連接到前一個節點
            if (i > 0)
            {
                ConfigurableJoint joint = segment.AddComponent<ConfigurableJoint>();
                joint.connectedBody = segments[i - 1].GetComponent<Rigidbody>();

                // 設置線性運動的限制，確保每段的長度固定
                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;
                // 設置線性限制，將距離範圍設為 segmentLength
                SoftJointLimit limit = new SoftJointLimit();
                limit.limit = segmentLength;
                limit.contactDistance = .05f;
                joint.linearLimit = limit;

                // 設置關節的阻尼和彈性
                JointDrive drive = new JointDrive
                {
                    positionSpring = 0f,  // 設置彈性為 0，避免彈跳
                    positionDamper = 100f,  // 增加阻尼以減少震動
                    maximumForce = Mathf.Infinity
                };
                joint.xDrive = drive;
                joint.yDrive = drive;
                joint.zDrive = drive;

                joint.autoConfigureConnectedAnchor = false;
                joint.anchor = Vector3.zero;
                joint.connectedAnchor = Vector3.zero;
            }

            segments[i] = segment;
        }


        // 將第一個段落節點固定到球，最後一個段落節點固定到圓盤
        AttachToTransform(segments[0], ball.transform, 0);
        AttachToTransform(segments[segmentCount - 1], disk.transform, 1);
        Rigidbody lastSegmentRb = segments[segmentCount - 1].GetComponent<Rigidbody>();
        Rigidbody fristSegmentRb = segments[0].GetComponent<Rigidbody>();
        if (flag)
        {
            fristSegmentRb.constraints = RigidbodyConstraints.FreezeAll;
            lastSegmentRb.constraints = RigidbodyConstraints.FreezeAll;

            fixedJointBall = segments[0].AddComponent<FixedJoint>();
            fixedJointBall.connectedBody = ball.GetComponent<Rigidbody>();
            fixedJointBall.breakForce = Mathf.Infinity;
            fixedJointBall.breakTorque = Mathf.Infinity;

            fixedJointDisk = segments[segmentCount - 1].AddComponent<FixedJoint>();
            fixedJointDisk.connectedBody = disk.GetComponent<Rigidbody>();
            fixedJointDisk.breakForce = Mathf.Infinity;
            fixedJointDisk.breakTorque = Mathf.Infinity;
        }
    }
    public void DeleteAllSegments()
    {
        // 確保 segments 陣列存在且有元素
        if (segments != null && segmentCount > 0)
        {
            for (int i = 0; i < segmentCount; i++)
            {
                GameObject segment = segments[i];

                // 檢查 segment 是否為 null，避免錯誤
                if (segment != null)
                {
                    // 釋放節點的 ConfigurableJoint
                    ConfigurableJoint joint = segment.GetComponent<ConfigurableJoint>();
                    if (joint != null)
                    {
                        Destroy(joint);
                    }

                    // 釋放節點的 Rigidbody
                    Rigidbody rb = segment.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Destroy(rb);
                    }

                    // 刪除節點的 GameObject
                    Destroy(segment);
                }
                else
                {
                    Debug.LogWarning("Segment " + i + " is null, skipping...");
                }
            }

            // 重置 segments 陣列，避免再次使用已被銷毀的物件
            segments = null;
        }
        else
        {
            Debug.LogWarning("No segments to delete or segmentCount is zero.");
        }
    }

    // 將更新移動到 FixedUpdate 來確保與物理系統同步
    void FixedUpdate()
    {
        if (segments != null && segments.Length > 0)
        {
            // 確保 segments[0] 和 segments[segmentCount - 1] 都存在且有效
            if (segments[0] != null && segments[segmentCount - 1] != null)
            {
                // 更新繩子的端點位置
                segments[0].transform.position = ball.transform.position;
                segments[segmentCount - 1].transform.position = disk.transform.position;

                // 更新 LineRenderer 的每個節點
                for (int i = 0; i < segmentCount; i++)
                {
                    // 確保每個節點有效
                    if (segments[i] != null)
                    {
                        lineRenderer.SetPosition(i, segments[i].transform.position);
                    }
                }
            }
        }
    }
    // 自定義方法：將節點固定到目標物體
    void AttachToTransform(GameObject segment, Transform target, int i)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();

        if (targetRb == null)
        {
            targetRb = target.gameObject.AddComponent<Rigidbody>();
            targetRb.isKinematic = true;  // 確保目標是靜止的，不受物理影響
        }

        ConfigurableJoint joint = segment.AddComponent<ConfigurableJoint>();
        joint.connectedBody = targetRb;

        // 鎖定位置，確保節點與目標固定
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
    }
}
