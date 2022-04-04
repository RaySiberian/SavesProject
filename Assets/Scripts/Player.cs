using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action Hit;
    public int Health;
    [SerializeField] private LayerMask layerMask;
    public Inventory Inventory;

    private void Awake()
    {
        Inventory = new Inventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !CheckBorder(Vector2.up))
        {
            transform.position += Vector3.up;
        }

        if (Input.GetKeyDown(KeyCode.S) && !CheckBorder(-Vector2.up))
        {
            transform.position -= Vector3.up;
        }

        if (Input.GetKeyDown(KeyCode.D) && !CheckBorder(Vector2.right))
        {
            transform.position += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.A) && !CheckBorder(Vector2.left))
        {
            transform.position -= Vector3.right;
        }
    }

    private bool CheckBorder(Vector2 direction)
    {
        var hit = Physics2D.Raycast(transform.position, direction, 1f, layerMask);
        return hit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Damage"))
        {
            Health--;
            if (Health == 0)
            {
                Destroy(gameObject);
            }
        }
        else if (col.CompareTag("Resource"))
        {
            var resource = col.GetComponent<Resource>();
            resource.SerializableResource.IsOnScene = false;
            Inventory.AddItem(resource.Type);
            col.gameObject.SetActive(false);
        }
        Hit?.Invoke();
    }
}