using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    private GameObject carriedBox;
    public float movementSpeed;
    private SpriteRenderer spriteRenderer;
    Vector3 facing;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float modifiedSpeed = movementSpeed;
        if (carriedBox != null)
            modifiedSpeed /= 2;

        facing = new Vector2();
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, modifiedSpeed);
            facing.y = 1;
        }
        else if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -modifiedSpeed);
            facing.y = -1;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-modifiedSpeed, 0);
            facing.x = -1;
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(modifiedSpeed, 0);
            facing.x = 1;
            spriteRenderer.flipX = true;
        }

        //transform.LookAt(transform.position + facing);

        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.LeftControl) ||
            Input.GetKeyDown(KeyCode.RightControl))
        {
            if (carriedBox != null)
                Drop();
            else
                PickUp();
        }
    }

    private void Drop()
    {
        var rawPos = transform.position + facing;
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
        var raycastPos = transform.position + facing;
        RaycastHit2D hit = Physics2D.Raycast(raycastPos, transform.forward, 0.2f);
        if (!IsCarrying() && hit.collider != null && hit.collider.gameObject.tag == "Box")
            {
                carriedBox = hit.collider.gameObject;
                carriedBox.GetComponent<BoxCollider2D>().enabled = false;
                carriedBox.transform.position = transform.position;
                carriedBox.transform.parent = transform;
        }
    }

    public void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bed" && !IsCarrying())
        {
            gm.PlayerWentToBed();
        }
    }

    private bool IsCarrying()
    {
        return carriedBox != null;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MissionPoint")
        {
            gm.PlayerReachedMissionPoint();
        }
    }
}
