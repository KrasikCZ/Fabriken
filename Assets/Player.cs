 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Clickable selected;
    public Slider progressBar;
    public static bool breaking = false;
    public float breakSpeed = 1f;
    private float breakNeed;
    private float breakNow = 0;
    public GameObject inventar;
    public static bool canMove = true;
    public static float cas = 0;
    public GameObject night;
    public static float health = 100;
    public Slider healthBar;
    public static bool isBuilding = false;
    public static int diff;
    public static int den = 0;
    public GameObject pauseMenu;
    public static bool isPaused = false;
    public static bool isInInv = false;
    public InventoryManager manager;
    public GameObject barUnload;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 mouseDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Ray2D ray2d = new Ray2D(transform.position, mouseDirection);
        RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction, 20f);
        RaycastHit hit;
        if (hit2d.collider != null && !isInInv)
        {
            Oznaceni.Instance.CursorMoved(hit2d.collider.gameObject);
        }
        else if (Physics.Raycast(ray, out hit) && !isInInv && hit.collider.tag != "Dropped") {
            Oznaceni.Instance.CursorMoved(hit.collider.gameObject);
        } 
        else { Oznaceni.Instance.CursorMoved(null); }
        if (Input.GetKeyDown(KeyCode.H)) health-=5;
        if (health < 100)
        {
            healthBar.value = health / 100;
            healthBar.gameObject.SetActive(true);
            health += Time.deltaTime*2;
        }
        else healthBar.gameObject.SetActive(false);
        if (Input.GetMouseButtonDown(0) && Oznaceni.Instance.selected != null && !breaking && !isBuilding && !isInInv)
        {
            //left click
            selected = Oznaceni.Instance.selected;
            breakNeed = selected.breakSpeed;
            progressBar.maxValue = breakNeed;
            progressBar.gameObject.SetActive(true);
            breaking = true;
        }
        if (Input.GetMouseButtonDown(0) && Oznaceni.Instance.selected != null && !breaking && isBuilding && !isInInv)
        {
            selected = Oznaceni.Instance.selected;
            selected.Clicked();
        }
        if (Input.GetMouseButtonDown(1) && Oznaceni.Instance.selected != null && !breaking && !isBuilding && !isInInv)
        {
            //right click
            selected = Oznaceni.Instance.selected;
            selected.Clicked();
        }
        if (Input.GetMouseButtonDown(1) && Oznaceni.Instance.selected != null && !breaking && isBuilding && !isInInv)
        {
            selected.GetComponent<Building>().StopBuilding();
        }
        if(Input.GetKeyDown(KeyCode.R) && Oznaceni.Instance.selected != null && !breaking && !isBuilding && !isInInv && Oznaceni.Instance.selected.GetComponent<Building>() && !Oznaceni.Instance.selected.GetComponent<Mover>())
        {
            selected.GetComponent<Building>().Rotate();
        }
        if (breaking)
        {
            breakNow += breakSpeed * Time.deltaTime;
            progressBar.value = breakNow;
            if(breakNow >= breakNeed)
            {
                breakNow = 0;
                selected.DoneBreaking();
                breaking = false;
                //Oznaceni.Instance.CursorMoved(null);
                progressBar.gameObject.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowInv();
            isInInv = inventar.activeInHierarchy;
        }
        cas += Time.deltaTime;
        if(cas > 360 && !night.activeInHierarchy)
        {
            night.SetActive(true);
        }
        if(cas > 600)
        {
            night.SetActive(false);
            cas = 0;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(true);
            isPaused = !isPaused;
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
    }
    public void ShowInv()
    {
        canMove = !canMove;
        inventar.SetActive(!inventar.activeInHierarchy);
        foreach(Transform tr in pauseMenu.transform.parent.Find("BuildInvents"))
        {
            tr.localPosition = new Vector3(1200, 0);
        }
        barUnload.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Dropped")
        {
            manager.AddItem(other.GetComponent<InventoryItem>().item, 1);
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Dropped")
        {
            manager.AddItem(collision.GetComponent<InventoryItem>().item, 1);
            Destroy(collision.gameObject);
        }
    }
}
