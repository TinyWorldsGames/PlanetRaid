using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }




    [SerializeField] Transform mainCamera;
    [SerializeField] Transform buildCamera;

    bool isBuildCamera = false;
    bool isDotweening = false;
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isBuildCamera)
            {
                SwitchToPlayerCamera();
            }
            else
            {
                SwitchToBuildCamera();
            }
        }


    }

    public void SwitchToBuildCamera()
    {
        buildCamera.position = mainCamera.position;
        buildCamera.rotation = mainCamera.rotation;
        mainCamera.GetComponent<Camera>().enabled = true;
        buildCamera.gameObject.SetActive(true);
        mainCamera.gameObject.tag = "Untagged";
        isDotweening = true;
        buildCamera.transform.DOMove(transform.position, 1f);
        buildCamera.transform.DORotate(transform.rotation.eulerAngles, 1f).OnComplete(() => isDotweening = false);
        isBuildCamera = true;

    }

    public void SwitchToPlayerCamera()
    {
        isBuildCamera = false;
        buildCamera.transform.DOMove(mainCamera.position, 1f);
        buildCamera.transform.DORotate(mainCamera.rotation.eulerAngles, 1f).OnComplete(() =>
        {
            mainCamera.gameObject.tag = "MainCamera";
            mainCamera.GetComponent<Camera>().enabled = true;
            buildCamera.gameObject.SetActive(false);

        });


    }


    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (isBuildCamera && !isDotweening)
        {
            buildCamera.transform.position = transform.position;
        }

    }
}