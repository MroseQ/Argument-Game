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
        if (storyProgress.storyProgress <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
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
        rotation = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0/*Random.Range(-1f, 1f)*/);
    }

    // Update is called once per frame
    private void Update()
    {
        GetComponent<Transform>().transform.position += destination * Time.deltaTime * speed;
        GetComponent<Transform>().transform.Rotate(rotation * Time.deltaTime * rotationSpeed);
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSecondsRealtime(6);
        gameObject.SetActive(false);
    }
}