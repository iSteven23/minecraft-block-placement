using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlocks : MonoBehaviour
{

    public Material blockMaterial; //brick blocks
    public Material templateMaterial;
    public Material templateYellow;
    public Material templateBlue;
    public bool buildModeOn = false;
    public bool building = false;
    private Vector3 buildPosition;
    private Vector3 point;
    public float distance = 0;

    private GameObject currentTemplateBlock;
    private GameObject newBlock;

    [SerializeField]
    private GameObject blockTemplatePrefab;
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private LayerMask buildableSurfacesLayer;
    
    // Use this for initialization
    void Start()
    {
        buildModeOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] allBlocks;
        allBlocks = GameObject.FindGameObjectsWithTag("PlacedBlock");
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 20, buildableSurfacesLayer);
        

        if (Input.GetKeyDown("b"))
        {
            buildModeOn = !buildModeOn;
        }

        if (buildModeOn)
        {
            if (hit)
            {
                point = hitInfo.point;
                buildPosition = new Vector3(Mathf.Round(point.x), Mathf.Round(point.y) + (0.5f), Mathf.Round(point.z));
                building = true;

            }
            else
            {
                if(currentTemplateBlock != null)
                Destroy(currentTemplateBlock.gameObject);
                building = false;
            }

        }

        if (!buildModeOn && currentTemplateBlock != null)
        {
            Destroy(currentTemplateBlock.gameObject);
            building = false;
        }

        if (building && currentTemplateBlock == null)
        {           
            
            currentTemplateBlock = Instantiate(blockTemplatePrefab, buildPosition, Quaternion.identity);
            currentTemplateBlock.GetComponent<MeshRenderer>().material = templateMaterial;
              
        }
        if (building && currentTemplateBlock != null)
        {
            currentTemplateBlock.transform.position = buildPosition;

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBlock();
            }
           for (int i = 0; i < allBlocks.Length; i++)
            {
                distance = Vector3.Distance(allBlocks[i].gameObject.transform.position, currentTemplateBlock.transform.position);
                if (distance == 1 || distance - 1.414214f < 0.03)
                {
                    Debug.Log("Block and temp block next to each other");
                    currentTemplateBlock.GetComponent<MeshRenderer>().material = templateBlue;
                    break;
                }
                else
                    currentTemplateBlock.GetComponent<MeshRenderer>().material = templateYellow;
            }
        }
        if(Input.GetMouseButtonUp(1))
        {
            if(allBlocks.Length != 0)
                Destroy(allBlocks[allBlocks.Length - 1].gameObject);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }
    private void PlaceBlock()
    {
        Debug.Log("Placing block");
        newBlock = Instantiate(blockPrefab, buildPosition, Quaternion.identity);
        newBlock.GetComponent<MeshRenderer>().material = blockMaterial;
        
        
    }
}
