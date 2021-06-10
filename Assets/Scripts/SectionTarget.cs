using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTarget : MonoBehaviour
{
    // Start is called before the first frame update
   // public Color selectedColor;
   TacticController controller;
    Transform selectedPos = null;
    GameObject selectedObject = null;
    public Prompter prompt;
   // Material originMaterial = null;

    void Start()
    {
        controller = gameObject.GetComponent<TacticController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {

                selectedPos = raycastHit.transform;

                if (selectedPos.CompareTag("CompareTarget"))
                {

                  selectedObject = Instantiate(selectedPos.gameObject, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.x, 10f)), Quaternion.identity);


                }
                else if (selectedPos.CompareTag("UncompareTarget"))
                {
                    string _name = selectedPos.gameObject.name;
                    prompt._Name.text = _name;
                    prompt._Density.text = "밀도 : " + controller.GetRelativeDensity(_name).ToString();
                    prompt._Frequency.text = "빈도 : " + controller.GetRelativeFrequency(_name).ToString();
                    prompt._Coverage.text = "피도 : " + controller.GetRelativeCover(_name).ToString();
                }


            }

        }
        else if (Input.GetMouseButton(0))
        {

            if (selectedObject != null)
            {
                //originMaterial = selectedObject.GetComponentInChildren<Material>();
                // selectedObject.GetComponentInChildren<Material>().color = selectedColor;
                Vector3 targetXZPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                selectedObject.transform.position = new Vector3(targetXZPos.x, 1.2f, targetXZPos.z);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {

            if (selectedObject != null)
            {
                // selectedObject.GetComponentInChildren<Material>().color = originMaterial.color;
                Vector3 targetXZPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                selectedObject.transform.position = new Vector3(targetXZPos.x, 1.0f, targetXZPos.z);

                if (controller.targetObj != null)
                {
                    if (controller.targetObj == selectedObject)
                    {
                        selectedObject.tag = "UncompareTarget";
                        controller.Add(selectedObject);
                        selectedObject = null;
                    }
                    else
                    {
                        Destroy(selectedObject);
                        selectedObject = null;
                    }


                }
                else
                {
                    Destroy(selectedObject);
                    selectedObject = null;
                }


            }
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                Debug.Log(raycastHit.transform.gameObject.name);
                if (raycastHit.transform.CompareTag("UncompareTarget"))
                {
                    Debug.Log("삭제 대상 삭제");
                    controller.Delete(raycastHit.transform.gameObject);
                    Destroy(raycastHit.transform.gameObject);

                }

            }

        }
    }
}
