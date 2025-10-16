using UnityEngine;

public class Fragment : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FragmentManager.Instance.AddFragment();
            Destroy(gameObject);
        }
    }
}