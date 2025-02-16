using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideSaloonTrigger : MonoBehaviour
{
    public GameObject floorCollider;
    public GameObject trapdoor;

    private void Start()
    {
        floorCollider.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            floorCollider.SetActive(true);
            trapdoor.tag = "Decoration";

        }
    }

}
