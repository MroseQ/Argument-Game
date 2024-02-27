using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool start = true;
    void Update()
    {
        if (start && !GetComponent<TextLoader>().pauseGame)
        {
            start = false;
            var positionX = Random.Range(-0.01f, 0.01f);
            var rotateZ = Random.Range(-0.03f, 0.03f);
            Vector2 direction = new Vector2(positionX, 0f);
            Vector3 rotation = new Vector3(0, 0, rotateZ);
            if (transform.localPosition.x > 0.1f)
            {
                direction.x = -Mathf.Abs(direction.x);
            }
            else if (transform.localPosition.x < -0.1f)
            {
                direction.x = Mathf.Abs(direction.x);
            }
            if (transform.rotation.z > 0.005f)
            {
                rotation.z = -Mathf.Abs(rotation.z);
            }
            else if (transform.rotation.z < -0.005f)
            {
                rotation.z = Mathf.Abs(rotation.z);
            }
            transform.Translate(direction);
            transform.Rotate(rotation);
            StartCoroutine(Waiting());
        }
    }

    public IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.04f);
        start = true;
    }
}
