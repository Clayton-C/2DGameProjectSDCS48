using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Tower currentPlaceBuffer;
    private Camera camera;
    private bool isPlacing;
    private Tower selectedTower;
    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -30;
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.zero);
            if (hit.collider != null)
            {
                Tower t = hit.collider.gameObject.GetComponent<Tower>();
                if (t != null)
                {
                    selectTower(hit.collider.gameObject.GetComponent<Tower>());
                    return;
                }
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Tower"))
                {
                    unselectTower();
                }

            }
        }
    }

    private void unselectTower()
    {
        if (selectedTower != null)
        {
            UIManager.instance.deselectTower();
            selectedTower.selected(false);
            selectedTower = null;
        }
    }

    private void selectTower(Tower t)
    {
        selectedTower = t;
        UIManager.instance.selectTower(t);
        selectedTower.selected(true);
    }

    private void placeTower(Vector3 position)
    {
        position.z = -5;
        Instantiate(currentPlaceBuffer.gameObject, position, Quaternion.identity);
    }

    public void startPlace(Tower t)
    {
        currentPlaceBuffer = t;
        if (GameManager.instance.coins < currentPlaceBuffer.cost)
        {
            return;
        }
        isPlacing = true;
        StartCoroutine(place());
    }


    IEnumerator place()
    {
        GameObject mockTower = Instantiate(currentPlaceBuffer.mesh, Vector3.zero, Quaternion.identity);
        GameObject radius = Instantiate(currentPlaceBuffer.radiusDisplay, Vector3.zero, Quaternion.identity);
        radius.transform.localScale = new Vector2(currentPlaceBuffer.radius * 2, currentPlaceBuffer.radius * 2);
        while (isPlacing)
        {
            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            mockTower.transform.position = mousePos;
            radius.transform.position = mousePos;

            //if left click
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.instance.coins >= currentPlaceBuffer.cost)
                {
                    placeTower(mousePos);
                    GameManager.instance.coins -= currentPlaceBuffer.cost;
                    currentPlaceBuffer = null;
                    isPlacing = false;

                    Destroy(mockTower);
                    Destroy(radius);
                }
                else
                {
                    currentPlaceBuffer = null;
                    isPlacing = false;
                    Destroy(mockTower);
                    Destroy(radius);
                    break;
                }
            }
            yield return null;
        }
    }
}
