using Unity.Mathematics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform pointSpawn;

    public Vector2 direction = Vector2.right;
    // Start is called before the first frame update
    private void Start()
    {
        // SpawnObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnObject()
    {
        var obj = Instantiate(prefab, pointSpawn.position, quaternion.identity);
        var bullet = obj.GetComponent<Bullet>();

        bullet.direction = direction;
        bullet.transform.localScale = new Vector3(direction.x, transform.localScale.y, transform.localScale.z);
    }
}
