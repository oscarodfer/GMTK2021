using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform camTransform;
    [SerializeField] GameObject playerObject;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;

    [SerializeField] float offsetAmount;
    [SerializeField] float smoothTimerOffset;
    [SerializeField] Transform limitUp, limitDown, limitLeft, limitRight;


    Vector3 originalPos;
    float inititialAmount;

    private GameObject objectToFollow;
    private Camera myCamera;

    void Start()
    {
        myCamera = GetComponent<Camera>();

        objectToFollow = playerObject;

    }
    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }
    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        

        Vector3 goalPos = objectToFollow.transform.position;
        float posX = Mathf.Clamp(goalPos.x, limitLeft.position.x + myCamera.orthographicSize * myCamera.aspect, limitRight.position.x - myCamera.orthographicSize * myCamera.aspect);
        float posY = Mathf.Clamp(goalPos.y, limitDown.position.y + myCamera.orthographicSize, limitUp.position.y - myCamera.orthographicSize);
        goalPos = new Vector3( posX,posY , goalPos.z);
        //goalPos.y = transform.position.y;
        goalPos.z = -10;

       

        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
        originalPos = transform.position;
       

    }




    public void SetObjectToFollow(GameObject obj)
    {
        objectToFollow = obj;
    }

    public void FollowPlayer()
    {
        objectToFollow = playerObject;
    }

}
