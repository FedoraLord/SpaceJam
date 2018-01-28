using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {

    public float scaleSpeed;
    public float maxScale;

    private void FixedUpdate()
    {
        transform.localScale = new Vector2(transform.localScale.x + scaleSpeed / 100, transform.localScale.y + scaleSpeed / 100);
        if (transform.localScale.x > maxScale)
            Destroy(gameObject);
    }
}
