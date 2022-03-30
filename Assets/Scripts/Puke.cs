using System;
using UnityEngine;

public class Puke : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LineRenderer line;
    [SerializeField] Transform pukeStart;
    [SerializeField] Transform origin;


    void Start() => TogglePuke(false);

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TogglePuke(true);

        if (Input.GetMouseButton(0))
            UpdatePuke();

        if (Input.GetMouseButtonUp(0))
            TogglePuke(false);

        AimToMouse();
    }

    void AimToMouse()
    {
        Vector2 direction = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        origin.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    void TogglePuke(bool state)
    {
        line.enabled = state;
    }

    void UpdatePuke()
    {
        var mousePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        line.SetPosition(0, pukeStart.position);
        line.SetPosition(1, mousePos);

        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(origin.transform.position, direction);

        if(hit)
        {
            line.SetPosition(1, hit.point);
            Colorable colorable = hit.collider.GetComponent<Colorable>();
            if(colorable)
                colorable.AddColor();
        }
    }
}
