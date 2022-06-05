using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCursor : MonoBehaviour
{
    [SerializeField]
    GameObject cursorChildObject;
    [SerializeField]
    GameObject objectToPlace;
    [SerializeField]
    ARRaycastManager raycastManager;

    [SerializeField]
    GameObject rosAnchor;

    [SerializeField]
    Vector3 rosAnchorOffset = new Vector3(0.0f, -0.64f, 0.0f);

    public bool useCursor = true;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        cursorChildObject.SetActive(useCursor);
    }

    // Update is called once per frame
    void Update()
    {
        if (useCursor)
        {
            UpdateCursor();
        }
        
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        
        if (touch.phase != TouchPhase.Began)
            return;

        
        if (useCursor && objectToPlace!=null)
        {
            GameObject.Instantiate(objectToPlace, transform.position, transform.rotation);
        }
        else if (objectToPlace != null)
        {
            raycastManager.Raycast(touch.position, s_Hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);
            if (s_Hits.Count > 0)
            {
                GameObject.Instantiate(objectToPlace, s_Hits[0].pose.position, s_Hits[0].pose.rotation);
            }
        }
    }

    public void PlaceTable()
    {
        rosAnchor.SetActive(false);
        rosAnchor.transform.position = transform.position + rosAnchorOffset;
        rosAnchor.SetActive(true);
    }

    void UpdateCursor()
    {
        var screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));

        raycastManager.Raycast(screenPosition, s_Hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        if (s_Hits.Count > 0)
        {
            transform.position = s_Hits[0].pose.position;
            transform.rotation = s_Hits[0].pose.rotation;
        }
    }
}
