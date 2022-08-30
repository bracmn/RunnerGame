using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollusionController : MonoBehaviour
{
    [SerializeField] private Animator GemAnimator;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            GameObject gemVfx = ObjectPooler.Instance.SpawnFromPool(PoolObjects.GemVFX, new Vector3(0, 0, 0), Quaternion.identity);
            gemVfx.transform.parent = transform;
            gemVfx.transform.localPosition = new Vector3(0, 0, 0);
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.parent = collision.transform;
            GemAnimator.Play("CollectedGem");
        }
    }
}
