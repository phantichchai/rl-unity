using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Actuators;


public class AgentController : MonoBehaviour
{
    [SerializeField]
    private bool isPlay;
    [SerializeField]
    private Position position;
    [SerializeField]
    private Transform parentTranform;
    [SerializeField]
    private ParticleSystem StunedEffect;
    private float moveSpeed = 0.5f;
    private float dashSpeed = 10.0f;
    private float dashCooldown = 0.0f;
    private bool isDash = false;
    private float dashDuration = 0.0f;
    private float rotateSpeed = 300f;
    private float jumpingTime;
    private float fallingForce = 50f;
    private float stunDuration = 0.0f;
    private bool isStun = false;
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
    public float StunDuration { get => stunDuration; set => stunDuration = value; }
    public bool IsStun { 
        get => isStun;
        set {
            if (value)
            {
                StunedEffect.Play();
            }
            else
            {
                StunedEffect.Stop();
            }
            isStun = value; 
        }
    }
    public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public Position Position { get => position; set => position = value; }

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

        if (StunDuration > 0f)
        {
            StunDuration -= Time.deltaTime;
        }
        else
        {
            IsStun = false;
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        Vector3 direction = Vector3.zero;
        Vector3 rotateDirection = Vector3.zero;
        int directionForwardAction = act[0];
        int rotateDirectionAction = act[1];
        int directionSideAction = act[2];
        int jumpAction = act[3];
        int dashAction = act[4];

        // Move forward / backward
        if (directionForwardAction == 1)
        {
            direction = (CheckOnGround() ? 1f : 0.5f) * 1f * transform.forward;
        }else if (directionForwardAction == 2) 
        {
            direction = (CheckOnGround() ? 1f : 0.5f) * -1f * transform.forward;
        }

        if (rotateDirectionAction == 1)
        {
            rotateDirection = transform.up * -1f;
        }
        else if (rotateDirectionAction == 2)
        {
            rotateDirection = transform.up * 1f;
        }

        // Rotate left / right
        if (directionSideAction == 1)
        {
            direction = -0.6f * transform.right; 
        }else if (directionSideAction == 2)
        {
            direction = 0.6f * transform.right;
        }

        // Jumping
        if (jumpAction == 1)
        {
            if ((JumpingTime <= 0f) && CanJump())
            {
                JumpingTime = 0.2f;
            }
        }

        // Dash
        if (dashAction == 1)
        {
            if (DashCooldown <= 0f)
            {
                direction = transform.forward;
                AgentRigidbody.AddForce(direction * DashSpeed * CheckOnFieldType(), ForceMode.VelocityChange);
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
            || collider.CompareTag("destination")
            || collider.CompareTag("collectorAgent")
            || collider.CompareTag("disruptorAgent"));
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
            || hit.collider.CompareTag("destination")
            || hit.collider.CompareTag("collectorAgent")
            || hit.collider.CompareTag("disruptorAgent"));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Item>(out Item item1))
        {
            Collider other = collision.collider;
            if (!Backpack.isBackpackFull())
            {
                other.transform.SetParent(transform);
                other.transform.position = transform.position + Vector3.up * (Backpack.CountItems() + 1);
                Backpack.CollectItem(item1);
                other.isTrigger = true;
            }
            else
            {
                Debug.Log("Can't get more");
            }
        }

        if (collision.collider.CompareTag("destination") && position == Position.Collector)
        {
            if (Backpack.CountItems() > 0)
            {
                Backpack.DropItem();
                foreach (Item item in GetComponentsInChildren<Item>())
                {
                    item.transform.SetParent(collision.transform);
                    item.transform.position = collision.transform.position + Vector3.up * (collision.transform.GetComponentsInChildren<Item>().Length + 0.525f);
                    item.GetComponent<SphereCollider>().isTrigger = false;
                }
            }
        }

        if (collision.collider.CompareTag("collectorAgent") || collision.collider.CompareTag("disruptorAgent"))
        {
            if (collision.collider.TryGetComponent<AgentController>(out AgentController agent))
            {
                if (IsDash)
                {
                    Debug.Log(position + ": Stun!!");
                    agent.StunDuration = 2.0f;
                    agent.IsStun = true;

                    if (agent.GetComponentInChildren<Item>() != null)
                    {
                        Item item = agent.GetComponentInChildren<Item>();
                        item.transform.SetParent(parentTranform);
                        agent.backpack.DropItem();
                        item.transform.position = agent.transform.position + -1.5f * agent.transform.forward;
                        item.GetComponent<SphereCollider>().isTrigger = false;
                    }
                }   

                if (agent.IsDash)
                {
                    Debug.Log(position + ": Ouch!!");
                    StunDuration = 2.0f;
                    IsStun = true;

                    if (GetComponentInChildren<Item>() != null)
                    {
                        Item item = GetComponentInChildren<Item>();
                        item.transform.SetParent(parentTranform);
                        backpack.DropItem();
                        item.transform.position = transform.position + -1.5f * transform.forward;
                        item.GetComponent<SphereCollider>().isTrigger = false;
                    }
                }
            }
        }
    }
}
