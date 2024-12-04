using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For UI components

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public Slider sprintBar; // Reference to the Sprint Bar UI Slider

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    private float walkSpeed;
    private float sprintSpeed;
    private float movementSpeed;
    public float sprintTime;
    private bool isSprintRecharging;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        walkSpeed = 1f;
        sprintSpeed = 3f; // Sprinting speed
        movementSpeed = walkSpeed;
        isSprintRecharging = false;

        // Initialize the sprint bar to match sprintTime
        sprintBar.maxValue = 4f; // Max sprint time
        sprintBar.minValue = 0f; // Min sprint time
        sprintBar.value = sprintTime; // Current sprint time
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        Sprint();
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * movementSpeed);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    void Sprint()
    {
        // Update the Sprint Bar UI to reflect sprintTime
        sprintBar.value = sprintTime;

        // If Left Shift is held and sprintTime is greater than 0, allow sprinting
        if (Input.GetKey(KeyCode.LeftShift) && sprintTime > 0f)
        {
            sprintTime -= Time.deltaTime; // Decrease sprint time
            movementSpeed = sprintSpeed; // Set speed to sprint
        }
        else
        {
            movementSpeed = walkSpeed; // Return to walking speed if not sprinting
        }

        // Regenerate sprintTime when not holding Left Shift, but only if not recharging
        if (!Input.GetKey(KeyCode.LeftShift) && sprintTime < 4f && !isSprintRecharging)
        {
            sprintTime += Time.deltaTime * 0.5f; // Regenerate sprint time at half speed
        }

        // Start recharge coroutine if sprintTime exceeds the max value
        if (sprintTime >= 4f && !isSprintRecharging)
        {
            StartCoroutine(SprintRecharge());
        }
    }

    IEnumerator SprintRecharge()
    {
        isSprintRecharging = true;
        movementSpeed = walkSpeed; // Ensure the player is walking during recharge
        Debug.Log("Charging sprint");

        while (sprintTime < 4f)
        {
            sprintTime += Time.deltaTime; // Gradually increase sprintTime
            sprintBar.value = sprintTime; // Update the Sprint Bar
            yield return null; // Wait for the next frame
        }

        sprintTime = 4f; // Cap sprintTime at its maximum value
        isSprintRecharging = false;
        Debug.Log("Sprint fully recharged");
    }
}
