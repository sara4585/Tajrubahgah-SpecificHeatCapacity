using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropHandler : Moderator
{
    public static DragDropHandler DragDropHandlerInstance;

    bool isMouseDragging;
    bool InventoryIsClicked;

    Vector3 offset;
    Vector3 screenPosition;
    GameObject target;

    public GameObject[] gameObjectsList;           //same index as inventory has 

    public GameObject MassCalSetup;                     //parents of instantiate prefabs
    public GameObject HeatingSetup;
    public GameObject PSetup;

    [SerializeField] UIInventoryHandler UI;
    [SerializeField] GameObjectsManager GM;

    [HideInInspector]
    public int InventoryApparatusIndex;

    Rigidbody rb;

    public float Speed = 3f;
    float Step;

    private void Awake()
    {
        DragDropHandlerInstance = this;
    }

    private void Start()
    {
        isMouseDragging = false;
        InventoryApparatusIndex = 0;
        InventoryIsClicked = false;
        rb = null;
    }

    void Update()               ////getting 
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (InventoryIsClicked && UI.CanIInstantiate(InventoryApparatusIndex))
            //must check, invntory is clicked
            {
                target = Instantiate(gameObjectsList[InventoryApparatusIndex]);
                //  Debug.Log("instantiated");

                SetParent();
                //bob texture and material set

                UI.ActivateFrontPanel(InventoryApparatusIndex);

                GM.SetGameObject(InventoryApparatusIndex, target);

                SfxHandler sfx = SfxHandler.SfxIns;
                if (sfx != null)
                    sfx.PlaySound("inventory_s");

            }

            else
            {
                RaycastHit hitInfo;
                target = ReturnClickedObject(out hitInfo);

            }

            if (target != null)
            {
                //Converting world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);

                rb = target.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }

                isMouseDragging = true;
            }
        }

        if (target && isMouseDragging)
        {
            if (target.tag == "BobObj")
            {
                target.GetComponent<BobRotationHandler>().StopRotation();
                target.transform.rotation = Quaternion.identity;
            }

            //tracking mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

            //converting screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace);

            target.transform.position = currentPosition;
            //It will update target gameobject's current postion.   
        }

        if (target && Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
            InventoryIsClicked = false;
        }
    }

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject targetObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            targetObject = hit.collider.gameObject;
        }
        return targetObject;
    }

    public void ClickOnApparatus(int index)
    {
        InventoryApparatusIndex = index;
        InventoryIsClicked = true;
        Debug.Log("appratus is clicked");
    }


    public void SetCupCapParent(GameObject GO)
    {
        GO.transform.SetParent(PSetup.transform);
        InventoryApparatusIndex = 12;
    }
    public void SetParent()
    {
        switch (InventoryApparatusIndex)
        {
            case 0:
            case 1:
            case 2:
                target.transform.SetParent(MassCalSetup.transform);
                break;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 11:
                target.transform.SetParent(HeatingSetup.transform);
                break;
            case 8:
            case 9:
            case 10:
            case 12:
                target.transform.SetParent(PSetup.transform);
                break;
        }
    }
}