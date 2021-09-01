using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public GameObject fX;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var fxPrefab = Instantiate(fX, transform.position, Quaternion.identity);
            Destroy(fxPrefab, 1f);
            other.GetComponent<PlayerHealth>().AddHealth(1);
            Destroy(gameObject);
        }
    }
}
