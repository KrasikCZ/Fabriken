using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blok : MonoBehaviour
{
    public string type;
    private void OnMouseOver()
    {
        this.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        this.gameObject.SetActive(false);
    }
}
