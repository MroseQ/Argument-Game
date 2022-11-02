using System.Collections;
using UnityEngine;

public class ShardMovement : MonoBehaviour
{
    public float speed;
    public int rotationSpeed;
    public Vector3 rotation;
    public Vector3 destination;
    public StoryProgress storyProgress;

    private void Start()
    {
        // If on a start of the game, moving shards are disabled.
        if (storyProgress.storyProgress <= 0)
        {
            gameObject.SetActive(false);
        }


        speed = Random.Range(5f, 8f);
        rotationSpeed = Random.Range(40, 50);
        int random = Random.Range(1, 3);
        if (random == 1)
        {
            destination = new Vector3(Random.Range(-1f, -0.5f), Random.Range(-1f, 1f));
        }
        else
        {
            destination = new Vector3(Random.Range(0.5f, 1f), Random.Range(-1f, 1f));
        }
        rotation = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
    }

    private void LateUpdate()
    {
        // Shard movements made every frame, destroying the objects after 6 seconds.
        transform.position += destination * Time.deltaTime * speed;
        transform.Rotate(rotation * Time.deltaTime * rotationSpeed);
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSecondsRealtime(6);
        Destroy(gameObject);
    }
}