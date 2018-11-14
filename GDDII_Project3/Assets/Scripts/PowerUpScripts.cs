using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScripts : MonoBehaviour
{
    // Vars
    private float num = -2.25f;

    // Update
    void FixedUpdate()
    {
        num += Time.deltaTime;
        float numAbs = Mathf.Abs(num);

        if (num < -1.5)
        {
            gameObject.transform.localScale = new Vector3(numAbs, numAbs, numAbs);
        }
        else if (num < 1.5)
        {
            num = 1.5f;
        }
        else if (num < 2.25)
        {
            gameObject.transform.localScale = new Vector3(numAbs, numAbs, numAbs);
        }
        else if (num >= 2.25)
        {
            num = -2.25f;
        }
    }

    // Delete
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Destroying power up");
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Destroying power up");
            Destroy(gameObject);
        }
    }
}
