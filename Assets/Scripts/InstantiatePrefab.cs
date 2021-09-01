using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    public GameObject prefab;
    public Transform point;
    public float livingTime;

    public void Instantiate()
    {
        var instantiatedObj = Instantiate(prefab, point.position, Quaternion.identity);

        if (livingTime > 0f)
        {
            Destroy(instantiatedObj, livingTime);
        }
    }
}
