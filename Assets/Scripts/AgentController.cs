using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    private bool isPlay;
    private float moveSpeed = 0.5f;
    private float dashSpeed = 10.0f;
    private float dashCooldown = 0.0f;
    private bool isDash = false;
    private float dashDuration = 0.0f;
    private float rotateSpeed = 300f;
    private float jumpingTime;
    private float fallingForce = 50f;
    private Vector3 jumpTargetPosition;
    private Backpack backpack;

    private Collider[] hitGroundColliders;

    private Rigidbody agentRigidbody;

    public float JumpingTime { get => jumpingTime; set => jumpingTime = value; }
    public Rigidbody AgentRigidbody { get => agentRigidbody; set => agentRigidbody = value; }
    public float FallingForce { get => fallingForce; set => fallingForce = value; }
    public float MoveSpeed { get => moveSpeed; }
    public float RotateSpeed { get => rotateSpeed; }
    public Backpack Backpack { get => backpack; set => backpack = value; }
    public bool IsDash { get => isDash; set => isDash = value; }
    public float DashCooldown { get => dashCooldown; set => dashCooldown = value; }
    public float DashDuration { get => dashDuration; set => dashDuration = value; }

    private void Start()
    {
        AgentRigidbody = GetComponent<Rigidbody>();
        Backpack = new Backpack();
    }

    private void Update()
    {
        if (DashCooldown > 0f)
        {
            DashCooldown -= Time.deltaTime;
        }
        if (DashDuration > 0f)
        {
            DashDuration -= Time.deltaTime;
        }
        else
        {
            IsDash = false;
        }
    }

    private void FixedUpdate()
    {
        if (isPlay)
        {
            MoveAgent();
        }
    }

    public void MoveAgent()
    {
        Vector3 direction = Vector3.zero;
        Vector3 rotateDirection = Vector3.zero;

        // Move forward / backward
        if (Input.GetKey(KeyCode.W))
        {
            direction = (CheckOnGround() ? 1f : 0.5f) * 1f * transform.forward;
        }else if (Input.GetKey(KeyCode.S)) 
        {
            direction = (CheckOnGround() ? 1f : 0.5f) * -1f * transform.forward;
        }

        // Rotate left / right
        if (Input.GetKey(KeyCode.A))
        {
            rotateDirection = -0.6f * transform.up; 
        }else if (Input.GetKey(KeyCode.D))
        {
            rotateDirection = 0.6f * transform.up;
        }

        // Jumping
        if (Input.GetKey(KeyCode.Space))
        {
            if ((JumpingTime <= 0f) && CanJump())
            {
                JumpingTime = 0.2f;
            }
        }

        // Dash
        if (Input.GetKey(KeyCode.F))
        {
            if (DashCooldown <= 0f)
            {
                direction = transform.forward;
                AgentRigidbody.AddForce(direction * dashSpeed * CheckOnFieldType(), ForceMode.VelocityChange);
                DashCooldown = 2.0f;
                DashDuration = 0.5f;
                IsDash = true;
            }
        }

        transform.Rotate(rotateDirection, Time.fixedDeltaTime * RotateSpeed);
        AgentRigidbody.AddForce(direction * MoveSpeed * CheckOnFieldType(), ForceMode.VelocityChange);
        if (JumpingTime > 0f)
        {
            jumpTargetPosition = new Vector3(AgentRigidbody.position.x, AgentRigidbody.position.y + 1f, AgentRigidbody.position.z) + direction;
            MoveTowards(jumpTargetPosition, AgentRigidbody, 300, 5);
        }

        if (!(JumpingTime > 0f) && !CheckOnGround())
        {
            AgentRigidbody.AddForce(Vector3.down * FallingForce, ForceMode.Acceleration);
        }
        JumpingTime -= Time.fixedDeltaTime;
    }

    public bool CanJump()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + new Vector3(0, -0.05f, 0), -Vector3.up, out hit, 1f);

        if (hit.collider != null && CheckHitColliderCompareTag(hit) && hit.normal.y > 0.95f)
        {
            return true;
        }

        return false;
    }

    public bool CheckOnGround()
    {
        hitGroundColliders = new Collider[8];
        Physics.OverlapBoxNonAlloc(transform.localPosition,
                new Vector3(0.95f / 2f, 0.5f, 0.95f / 2f),
                hitGroundColliders,
                transform.rotation);
        bool grounded = false;
        foreach (Collider collider in hitGroundColliders)
        {
            if (collider != null && collider.transform != transform && CheckColliderCompareTag(collider))
            {
                grounded = true;
                break;
            }
        }
        return grounded;
    }

    public float CheckOnFieldType()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + new Vector3(0, -0.05f, 0), -Vector3.up, out hit, 1f);

        if (hit.collider != null && hit.collider.transform != transform)
        {
            if (hit.collider.CompareTag("pond"))
            {
                return 0f;
            }else if (hit.collider.CompareTag("ice"))
            {
                return 2f;
            }else if (hit.collider.CompareTag("desert"))
            {
                return 0.5f;
            }
        }
        return 1f;
    }

    public void MoveTowards(Vector3 targetPosition, Rigidbody rb, float targetVelocity, float maxVelocity)
    {
        Vector3 moveToPosition = targetPosition - rb.worldCenterOfMass;
        var velocityTarget = Time.fixedDeltaTime * targetVelocity * moveToPosition;
        if (float.IsNaN(velocityTarget.x) == false)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, velocityTarget, maxVelocity);
        }
    }

    public bool CheckColliderCompareTag(Collider collider)
    {
        return (collider.CompareTag("wall") 
            || collider.CompareTag("ground") 
            || collider.CompareTag("box")
            || collider.CompareTag("elevator")
            || collider.CompareTag("pond")
            || collider.CompareTag("ice")
            || collider.CompareTag("desert")
            || collider.CompareTag("destination"));
    }

    public bool CheckHitColliderCompareTag(RaycastHit hit)
    {
        return (hit.collider.CompareTag("wall") 
            || hit.collider.CompareTag("ground") 
            || hit.collider.CompareTag("box")
            || hit.collider.CompareTag("elevator")
            || hit.collider.CompareTag("pond")
            || hit.collider.CompareTag("ice")
            || hit.collider.CompareTag("desert")
            || hit.collider.CompareTag("destination"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Item>(out Item item))
        {
            other.isTrigger = true;
            if (!Backpack.isBackpackFull())
            {
                other.transform.SetParent(transform);
                other.transform.position = transform.position + Vector3.up * Backpack.CountItems();
                Backpack.CollectItem(item);
            }else{
                other.isTrigger = false;
                Debug.Log("Can't get more");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("destination"))
        {
            if (Backpack.CountItems() > 0)
            {
                Backpack.DropItem();
                foreach (Item item in GetComponentsInChildren<Item>())
                {
                    item.transform.SetParent(collision.transform);
                    item.transform.position = collision.transform.position + Vector3.up * (collision.transform.GetComponentsInChildren<Item>().Length + 0.5f);
                    item.GetComponent<SphereCollider>().isTrigger = false;
                }
            }
        }


        if (collision.collider.CompareTag("agent"))
        {
            if (collision.collider.TryGetComponent<AgentController>(out AgentController agent))
            {
                if (agent.IsDash)
                {
                    Debug.Log("Ouch!!");
                }
            }
        }
    }
}
