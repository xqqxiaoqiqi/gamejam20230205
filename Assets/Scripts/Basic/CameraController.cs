using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// 相机当前是否可交互
    /// </summary>
    public bool IsInteracte = true;

    public float moveSpeed = 0.1f;// 相机移动速度

    /// <summary>
    /// 记录鼠标滚轮当前值
    /// </summary>
    private float mouseWheel;
    /// <summary>
    /// 鼠标移动到屏幕边缘的差值
    /// </summary>
    private float mouseOffset = 0.01f;

    private bool mouseIsDrag = false;
    private Vector3 mouseEndPos;
    private Camera cam;
    private Transform myCamera;
    
    private void Awake()
    {
        cam = GetComponent<Camera>();
        myCamera = cam.transform;
        SetCameraInitPos(Vector3.zero);
    }

    private void Update()
    {
        if (IsInteracte)
        {
            if (!mouseIsDrag)
            {
                mouseInScreenBound();
            }
        }
        else
        {
            if (mouseIsDrag) mouseIsDrag = false;
        }
    }

    /// <summary>
    /// 设置相机位置
    /// </summary>
    public void SetCameraInitPos(Vector3 pos)
    {
        myCamera.position = new Vector3(pos.x, pos.y, myCamera.position.z);
    }
    /// <summary>
    /// 鼠标在屏幕边缘时，移动相机
    /// </summary>
    private void mouseInScreenBound()
    {
        Vector3 v1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //上移
        if (v1.y >= 1 - mouseOffset)
        {
            myCamera.Translate(Vector2.up * moveSpeed);
        }
        //下移
        if (v1.y <= mouseOffset)
        {
            myCamera.Translate(Vector2.down * moveSpeed);
        }
        //左移
        if (v1.x <= mouseOffset)
        {
            myCamera.Translate(Vector2.left * moveSpeed);
        }
        //右移
        if (v1.x >= 1 - mouseOffset)
        {
            myCamera.Translate(Vector2.right * moveSpeed);
        }
    }
}