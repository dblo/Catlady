using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    private GameObject carriedBox;
    public float movementSpeed;

    void Update()
    {
        float modifiedSpeed = movementSpeed;
        if (carriedBox != null)
            modifiedSpeed /= 2;
        Vector3 facing = new Vector2();

        if (Input.GetKey("w"))
        {
            transform.position += new Vector3(0, modifiedSpeed);
            facing.y = 1;
        }
        else if (Input.GetKey("s"))
        {
            transform.position += new Vector3(0, -modifiedSpeed);
            facing.y = -1;
        }
        if (Input.GetKey("a"))
        {
            transform.position += new Vector3(-modifiedSpeed, 0);
            facing.x = -1;
        }
        else if (Input.GetKey("d"))
        {
            transform.position += new Vector3(modifiedSpeed, 0);
            facing.x = 1;
        }

        transform.LookAt(transform.position + facing);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (carriedBox != null)
                Drop();
            else
                PickUp();
        }
    }

    private void Drop()
    {
        var rawPos = transform.position + transform.forward;
        var dropPosition = new Vector2
        {
            x = (float)Math.Round(rawPos.x),
            y = (float)Math.Round(rawPos.y)
        };

        var coll = Physics2D.OverlapBox(dropPosition, new Vector2(0.3f, 0.3f), 0);
        if (coll == null)
        {
            carriedBox.transform.position = dropPosition;
            carriedBox.transform.rotation = Quaternion.identity;
            carriedBox.transform.parent = null;
            carriedBox.transform.localScale = new Vector3(1, 1, 1);
            carriedBox.GetComponent<BoxCollider2D>().enabled = true;
            carriedBox = null;
        }
    }

    private void PickUp()
    {
        //int layerMask = LayerMask.GetMask("Boxes");
        var raycastPos = transform.position + transform.forward;
        RaycastHit2D hit = Physics2D.Raycast(raycastPos, transform.forward, 0.2f);
        if (hit.collider != null && hit.collider.gameObject.tag == "Box")
        {
            carriedBox = hit.collider.gameObject;
            carriedBox.GetComponent<BoxCollider2D>().enabled = false;
            carriedBox.transform.parent = transform;
            carriedBox.transform.position = transform.position;
        }
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bed")
        {
            gm.PlayerWentToBed();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MissionPoint")
        {
            gm.PlayerReachedMissionPoint();
        }
    }
}
