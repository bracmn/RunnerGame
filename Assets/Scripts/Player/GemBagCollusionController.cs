using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBagCollusionController : MonoBehaviour
{
    [SerializeField] public Transform GemFolder;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Gem1" || collision.collider.tag == "Gem2")
        {
            foreach (Transform child in collision.transform)
            {
                child.gameObject.SetActive(false);
                child.parent = null;
            }
            collision.gameObject.SetActive(false);
            collision.transform.parent = GemFolder;
            collision.transform.GetComponent<MeshRenderer>().enabled = true;
            if(collision.collider.tag == "Gem1")
                StartCoroutine(GameManager.Instance.AddScore(10));
            else if (collision.collider.tag == "Gem2")
                StartCoroutine(GameManager.Instance.AddScore(50));
        }
    }
}
