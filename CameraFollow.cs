using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public LayerMask occlusionBlockLayers = ~0;
    public Transform player;            
    public float distance = 10f;        
    public float mouseSensitivity = 4f; 
    public int i;                       
    public float MouseScrollWheel = 2f;
    private Vector3 idealPosition;
    private float currentRotX = 0f;     
    private float currentRotY = 0f;    
    public float smoothBackSpeed = 20f;
    private Vector3 lookPosition;
    public GameObject GameUI;

    public void Start()
    {
        occlusionBlockLayers = ~(1 << LayerMask.NameToLayer("Player"));
        currentRotX = transform.eulerAngles.x;
        currentRotY = transform.eulerAngles.y;
        CalcIdealPosition();
        OcclusionHandle();
    }

    public void LateUpdate()
    {
        if (GameUI.activeInHierarchy) {
         HandleMouseInput();
         CalcIdealPosition();
         OcclusionHandle();
        }
           
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(0))
        {            
            currentRotY -= Input.GetAxis("Mouse X") * mouseSensitivity;            
            currentRotX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            currentRotX = Mathf.Clamp(currentRotX, -89f, 89f);
        }
        distance += Input.GetAxis("Mouse ScrollWheel") * MouseScrollWheel;
        distance = Mathf.Clamp(distance, 1f, 50f);
    }

    private void CalcIdealPosition()
    {
        float radX = currentRotX * Mathf.Deg2Rad;
        float radY = currentRotY * Mathf.Deg2Rad;
        float x = distance * Mathf.Sin(radX) * Mathf.Cos(radY);
        float y = distance * Mathf.Cos(radX);
        float z = distance * Mathf.Sin(radX) * Mathf.Sin(radY);
        idealPosition = player.position + new Vector3(x, y, z);
    }
    private void OcclusionHandle()
    {
        lookPosition = new Vector3(player.position.x , player.position.y + i , player.position.z);
        Vector3 dirToCam = (idealPosition - lookPosition).normalized;
        float maxDist = Vector3.Distance(lookPosition, idealPosition);
        RaycastHit[] hits = Physics.RaycastAll(lookPosition, dirToCam, maxDist,
                             occlusionBlockLayers,
                             QueryTriggerInteraction.Ignore);


        if (hits.Length == 0)
        {
            transform.position = Vector3.Lerp(transform.position,
                                              idealPosition,
                                              smoothBackSpeed * Time.deltaTime);

            transform.LookAt(lookPosition);
        }
        else
        {
            float minDist = Mathf.Infinity;
            Vector3 closest = idealPosition;     
            foreach (var h in hits)            
            {
                float d = Vector3.Distance(lookPosition, h.point);
                if (d < minDist)                
                {
                    minDist = d;
                    closest = h.point;
                }
            }


            Vector3 hitNormal = (lookPosition - closest).normalized;
            Vector3 finalPos = closest + hitNormal * 0.1f;


            transform.position = Vector3.Lerp(transform.position,
                                              finalPos,
                                              smoothBackSpeed * Time.deltaTime);

            transform.LookAt(lookPosition);

        }


    }
}

