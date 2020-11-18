using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;


/*
    TODO: 
    -Fix ViewObstruction bug
    -Create a technique to zoom in player instead of hidding the walls
    -Zoom in and out with the camera by pressing the scroll button
*/
public class ThirdPersonCameraController : MonoBehaviour
{
    public float rotationSpeed = 1;
    public Transform target, player;
    float mouseX, mouseY;

    public Transform obstruction;
    float zoomSpeed = 2f;
    public float maxDistance = 6f;

    // Start is called before the first frame update
    void Start()
    {
        obstruction = target;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CamControl();
        ViewObstruction();
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        transform.LookAt(target);
        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        player.rotation = Quaternion.Euler(0, mouseX, 0);
    }

    //This function makes the walls disappear in order to see the player...BUG: sometimes the camera moves a little forward so the walls remain hidden 
    void ViewObstruction()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, maxDistance))
        {
            if(hit.collider.gameObject.tag != "Player")
            {
                obstruction = hit.transform;
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                if(Vector3.Distance(obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, target.position) >= 1.5f)
                {
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
                }
            }
            else
            {
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if (Vector3.Distance(transform.position, target.position) < 4.5f) //Change 4.5f to maxDistance??
                {
                    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                }
            }
        }
    }

    //Create a technique to zoom in player instead of hidding the walls
    void ZoomInViewObstruction()
    {

    }
}
