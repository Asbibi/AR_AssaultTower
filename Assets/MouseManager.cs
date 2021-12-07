using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Vector3 mousePosition;
    private Plane plane;
    private Hero movedHero = null;
    private Vector3 offset;
    [SerializeField] private LayerMask virtualLayerMask;
    [SerializeField] private Camera c;


    void Start()
    {
        SetPlane(0);
    }
    private void SetPlane(float offsetY)
    {
        plane = new Plane(Vector3.up, Vector3.up * offsetY);
    }


    void Update()
    {
        // Mouse is released ?
        if (Input.GetMouseButtonUp(0))
            OnReleaseMouse();


        // Not in preparation phase ? => deny selection/Grabbing
        if (GameManager.GetState() != GameManager.GameState.Preparation)
            return;


        // Mouse is pressed ?
        if (Input.GetMouseButtonDown(0))
            OnPressMouse();


        // Move the selected Hero if there is one
        if (movedHero == null)
            return;

        ComputeMousePosition();
        movedHero.transform.position = mousePosition + offset;
    }

    public Vector3 getMousePos()
    {
        return mousePosition;
    }
    private void ComputeMousePosition()
    {
        Ray ray = c.ScreenPointToRay(Input.mousePosition);  //Camera.main.ScreenPointToRay(Input.mousePosition);

        float enter = 0.0f;
        if (plane.Raycast(ray, out enter))
        {
            mousePosition = ray.GetPoint(enter);
        }
    }

    private void OnPressMouse()
    {
        RaycastHit hit;
        Ray ray = c.ScreenPointToRay(Input.mousePosition);  //Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 3, virtualLayerMask))
        {
            movedHero = hit.transform.GetComponent<Hero>();
            ComputeMousePosition();
            float y = hit.point.y;
            SetPlane(y);
            offset = new Vector3(0, -y, 0);//movedHero.transform.position - mousePosition;
        }
    }
    private void OnReleaseMouse()
    {
        movedHero = null;
    }
}
