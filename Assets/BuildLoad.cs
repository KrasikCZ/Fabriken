using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildLoad : MonoBehaviour
{
    public List<Building> budovy;
    public GameObject bar;
    public GameObject button;
    public void Clicked()
    {
        bar.SetActive(true);
        foreach(Transform obj in bar.transform)
        {
            Destroy(obj.gameObject);
        }
        foreach(Building b in budovy)
        {
            GameObject klikaci = Instantiate(button, bar.transform);
            klikaci.GetComponent<AppearBuild>().building = b;
            klikaci.AddComponent<Image>();
            klikaci.GetComponent<Image>().sprite = b.GetComponent<Building>().budova;
        }
    }
    public void ClickedOut()
    {
        bar.SetActive(false);
    }
}
