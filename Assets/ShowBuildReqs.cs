using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBuildReqs : MonoBehaviour
{
    public List<BuildReqs> potreby;
    public InventoryManager manager;
    public Font font;
    private void Awake()
    {
        manager = Resources.FindObjectsOfTypeAll<InventoryManager>()[0];
    }
    public void ShowReqs()
    {
        foreach(Transform obj in transform)
        {
            Destroy(obj.gameObject);
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(80, 50 * potreby.Count);
        foreach(BuildReqs potr in potreby)
        {
            GameObject obj = Instantiate(new GameObject(), transform);
            GameObject text = Instantiate(new GameObject(), transform);
            obj.AddComponent<Image>();
            text.AddComponent<Text>();
            obj.GetComponent<Image>().sprite = potr.item.image;
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            text.GetComponent<Text>().transform.position = new Vector3(10, 0, 0);
            text.GetComponent<Text>().text = manager.FindItem(potr.item) + " / " + potr.amount;
            text.GetComponent<Text>().font = font;
            text.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            text.name = "text";
        }
        foreach(GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.name == "New Game Object") Destroy(obj);
        }
    }
}
