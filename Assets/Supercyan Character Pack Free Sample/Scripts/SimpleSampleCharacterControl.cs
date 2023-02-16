using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class SimpleSampleCharacterControl : MonoBehaviour
{
    private enum ControlMode
    {
        /// <summary>
        /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
        /// </summary>
        Tank,
        /// <summary>
        /// Character freely moves in the chosen direction from the perspective of the camera
        /// </summary>
        Direct
    }

    private float startSpeed;
    [SerializeField] public float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;

    [SerializeField] public Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private float m_backwardsWalkScale = 0.16f;
    private float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
    private bool m_jumpInput = false;

    private bool m_isGrounded;

    private List<Collider> m_collisions = new List<Collider>();

    public Joystick joystick;

    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }

        startSpeed = m_moveSpeed;
        m_animator.SetFloat("MoveSpeed", 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }

        
    }
    private Vector3 startRotation = Vector3.zero;
    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
   
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }

    }

    IEnumerator resetRotation(Vector3 target){
        while(transform.eulerAngles != target){
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, target, 1f);
            yield return new WaitForEndOfFrame();
        }
    }

    private void Update()
    {
        if (!m_jumpInput && Input.GetKey(KeyCode.Space))
        {
            m_jumpInput = true;
        }
    }


    public Vector3 startTargetPos, endTargetPos;
    int startedMoveToFinish = -1;

    private void FixedUpdate()
    {
        
        if(PlayerLive.alive == 1){
            m_animator.SetBool("Grounded", true);

            
        }
        else if(PlayerLive.alive == 2){
            m_animator.SetBool("Grounded", true);

            if(!IsInvoking(nameof(comeIn))){
                Invoke(nameof(comeIn), 2f);
            }
            
        }

        DirectUpdate();

        // switch (m_controlMode)
        // {
        //     case ControlMode.Direct:
        //         DirectUpdate();
        //         break;

        //     case ControlMode.Tank:
        //         TankUpdate();
        //         break;

        //     default:
        //         Debug.LogError("Unsupported state");
        //         break;
        // }

        m_wasGrounded = m_isGrounded;
        m_jumpInput = false;
    }

    private void TankUpdate()
    {
        float v = 1f;
        float h = 0;

        bool walk = true;

        if (v < 0)
        {
            if (walk) { v *= m_backwardsWalkScale*4; }
            else { v *= m_backwardRunScale*4; }
        }
        else if (walk)
        {
            v *= m_walkScale*2;
        }

        m_currentV = Mathf.Lerp(m_currentV, v,  m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h,  m_interpolation);
        
        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        m_animator.SetFloat("MoveSpeed", m_currentV);

        JumpingAndLanding();
    }

    private void DirectUpdate()
    {

        float v = 0;
        float h = 0;
        
        if(PlayerLive.alive == 1){
            v = joystick.Vertical;
            h = joystick.Horizontal;
        }
        else if(startedMoveToFinish == 1 && PlayerLive.alive == 2){
            v = 1;
            h = 0;
        }
        else{
            m_animator.SetFloat("MoveSpeed", 0);
        }
        
        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            v *= m_walkScale;
            h *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }

        JumpingAndLanding();
    }

    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && m_jumpInput)
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag=="Player"){
            m_moveSpeed*=3;

            //transform.eulerAngles = other.gameObject.transform.eulerAngles;
        }
        else{
            if(!other.gameObject.name.ToLower().Contains("plane")){
                if(startRotation==Vector3.zero){
                    startRotation=transform.eulerAngles;
                }
                Debug.Log("Now rotate!");
                m_moveSpeed*=1.01f;
                transform.Rotate(0,1,0);
            }
        }
    }
    

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag=="Player"){
            m_moveSpeed/=3;
        }
    }

    void comeIn(){
        if(startedMoveToFinish == -1){
            transform.position = new Vector3(startTargetPos.x, transform.position.y, startTargetPos.z);
            startedMoveToFinish = 1;
        }

        StartCoroutine(coming());
        
    }

    IEnumerator coming(){
        while(true){
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(endTargetPos.x, transform.position.y, endTargetPos.z), 0.1f);
            yield return new WaitForEndOfFrame();
        }
    }
}
