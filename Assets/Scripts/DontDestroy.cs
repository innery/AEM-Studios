using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        // Eğer sahne geçişinde bu nesne zaten varsa, yenisini yok et
        if (GameObject.Find(gameObject.name) != gameObject)
        {
            Destroy(gameObject);
            return;
        }

        // Bu nesneyi sahneler arasında yok etme
        DontDestroyOnLoad(gameObject);
    }
}