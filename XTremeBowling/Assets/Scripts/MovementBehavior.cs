using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    public AdjustPosition playerEffects;

    public GameObject particleFireball;
    private Color previousFireballColor;
    private GameObject particleFire;
    private Color previousFireColor;
    private GameObject particleSmoke;
    private Color previousSmokeColor;
    private GameObject particleCenterGlow;
    private Color previousCenterGlowColor;
    private GameObject particleGlow;
    private Color previousGlowColor;
    private GameObject particleSparks;
    private Color previousSparksColor;

    //Camera Reference
    public Transform cammeraOne;
    private Vector3 camF;
    private Vector3 camR;
    private Vector3 intent; //intention of movement

    //Pivot Point and Rope Prefabs
    public GameObject pivotObject;
    public GameObject ropeRender;
    private GameObject currentPivotObject = null;
    private GameObject currentRopeObject = null;    
    private ConfigurableJoint tempJoint; //Configurable Joint had free-er rotation plus rope-like physics (instead of hinge)

    private Vector3 ropeDestination;
    private bool jointIsOn = false;

    private float jointLimit = 3.0f;
    private float jointLimitSpring = 0.0f;
    private float jointLimitDamper = 20.0f;

    private float distanceToTheGround = 0.0f;
    private bool isGrounded = false;

    public float speed = 800.0f;
    private float speedLimit = 45.0f;
    private float jumpPower = 15.0f;

    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2.0f;

    private Rigidbody rigidBody;

    private bool boostOn;
    private float magnitude;

    private bool playerHasControl = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        distanceToTheGround = GetComponent<Collider>().bounds.extents.y + 2.0f;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        //effects
        particleFire = particleFireball.transform.Find("Fire").gameObject;
        particleSmoke = particleFireball.transform.Find("Smoke 2 ").gameObject;
        particleCenterGlow = particleFireball.transform.Find("Center Glow").gameObject;
        particleGlow = particleFireball.transform.Find("Glow ").gameObject;
        particleSparks = particleFireball.transform.Find("Sparks 2").gameObject;

        previousFireballColor = particleFireball.GetComponent<Renderer>().material.GetColor("_EmissionColor");
        previousFireColor = particleFire.GetComponent<Renderer>().material.GetColor("_EmissionColor");
        previousSmokeColor = particleSmoke.GetComponent<Renderer>().material.GetColor("_EmissionColor");
        previousCenterGlowColor = particleCenterGlow.GetComponent<Renderer>().material.GetColor("_EmissionColor");
        previousGlowColor = particleGlow.GetComponent<Renderer>().material.GetColor("_EmissionColor");
        previousSparksColor = particleSparks.GetComponent<Renderer>().material.GetColor("_EmissionColor");

        StartCoroutine(countdownToStart());


    }

    private IEnumerator countdownToStart()
    {
        yield return new WaitForSeconds(5.0f);
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        rigidBody.AddForce(new Vector3(0.0f, jumpPower, -70.0f), ForceMode.Impulse);
        playerHasControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToTheGround + 0.1f);

        //make movement relevant to camera
        CalculateCamera();
        intent = camR * h + camF * v;
        playerEffects.SetRotation(intent);
        intent.y = 0.0f;

        if (playerHasControl)
        {
            Move();
            CheckForNitro();
            Jump();
            if (jointIsOn)
            {
                HandleSwing();
            }
            CheckReleaseOfRope();
            ChangeFlameColor();
        }
        //Debug.Log(rigidBody.velocity.magnitude);

        magnitude = rigidBody.velocity.magnitude;

        CheckForDeath();
    }

    void CheckForDeath()
    {
        if (transform.position.y <= -500.0f)
        {
            DoDeathSequence();
        }
    }

    void DoDeathSequence()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void CalculateCamera()
    {
        camF = cammeraOne.forward;
        camR = cammeraOne.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;
    }

    void Move()
    {
        if (rigidBody.velocity.magnitude > speedLimit)
        {
            float antiVelocity = rigidBody.velocity.magnitude - speedLimit;
            Vector3 oppositeDirection = -rigidBody.velocity;
            rigidBody.AddForce(oppositeDirection * Time.deltaTime);
        }
        else
        {
            rigidBody.AddForce(intent * speed * Time.deltaTime, ForceMode.Acceleration);
        }        
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidBody.AddForce(new Vector3(0.0f, jumpPower, 0.0f), ForceMode.Impulse);
            playerEffects.PlayJumpEffect();
        }

        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1.0f) * Time.deltaTime;
        }
        else if (rigidBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1.0f) * Time.deltaTime;
        }
    }

    void CheckForNitro()
    {
        if (boostOn && isGrounded)
        {
            playerEffects.PlayNitroEffect();
            rigidBody.velocity *= 2.0f;
            boostOn = false;
        }
    }

    void CheckReleaseOfRope()
    {
        if (Input.GetMouseButtonUp(1) && currentPivotObject != null)
        {
            speedLimit = 45.0f;
            jointIsOn = false;
            Destroy(GetComponent<ConfigurableJoint>());
            Destroy(currentPivotObject);
            currentPivotObject = null;
            Destroy(currentRopeObject);
            currentRopeObject = null;
        }
    }

    void HandleSwing()
    {
        speedLimit = 65.0f;
        tempJoint.axis = rigidBody.velocity;

        //effects
        Vector3[] positions = { transform.position, ropeDestination };
        currentRopeObject.GetComponent<LineRenderer>().SetPositions(positions);

        tempJoint.targetVelocity = rigidBody.velocity;
        if (tempJoint.targetVelocity.magnitude > 45.0f)
        {
            float antiVelocity = rigidBody.velocity.magnitude - 45.0f;
            Vector3 oppositeDirection = -tempJoint.targetVelocity;
            tempJoint.targetVelocity += oppositeDirection * Time.deltaTime;
        }
       
    }

    public void SetIntendedPivotPointPosition (Vector3 intendedPivotPoint)
    {
        Vector3 direction = intendedPivotPoint - transform.position;
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 50.0f, layerMask))
        {
            //Draw Rope
            currentRopeObject = Instantiate(ropeRender, transform.position, Quaternion.identity);
            ropeDestination = hit.point;
            Vector3[] positions = { transform.position, ropeDestination };
            currentRopeObject.GetComponent<LineRenderer>().SetPositions(positions);

            //Create Pivot Point
            currentPivotObject = Instantiate(pivotObject, hit.point, Quaternion.identity);
            tempJoint = gameObject.AddComponent<ConfigurableJoint>();
            tempJoint.connectedBody = currentPivotObject.GetComponent<Rigidbody>();
            Vector3 realPoint = transform.InverseTransformPoint(hit.point);
            tempJoint.anchor = realPoint;

            //Configure Joint
            tempJoint.xMotion = ConfigurableJointMotion.Limited;
            tempJoint.yMotion = ConfigurableJointMotion.Limited;
            tempJoint.zMotion = ConfigurableJointMotion.Limited;
            tempJoint.angularXMotion = ConfigurableJointMotion.Free;
            tempJoint.angularYMotion = ConfigurableJointMotion.Free;
            tempJoint.angularZMotion = ConfigurableJointMotion.Free;
            SoftJointLimit limits = tempJoint.linearLimit;
            SoftJointLimitSpring sLimits = tempJoint.linearLimitSpring;


            limits.limit = jointLimit;
            sLimits.spring = jointLimitSpring;
            sLimits.damper = jointLimitDamper;
            tempJoint.linearLimit = limits;
            tempJoint.linearLimitSpring = sLimits;
            //Activate Swing Checks in Update
            jointIsOn = true;
        }
    }

    void ChangeFlameColor()
    {
        Material fireballMat = particleFireball.GetComponent<Renderer>().material;
        Material fireMat = particleFire.GetComponent<Renderer>().material;
        Material smokeMat = particleSmoke.GetComponent<Renderer>().material;
        Material centerGlowMat = particleCenterGlow.GetComponent<Renderer>().material;
        Material glowMat = particleGlow.GetComponent<Renderer>().material;
        Material sparksMat = particleSparks.GetComponent<Renderer>().material;

        if (rigidBody.velocity.magnitude > 50.0f)
        {
            fireballMat.SetColor("_EmissionColor", new Color(0.0f, 77.0f, 255.0f) / 80.0f);
            fireMat.SetColor("_EmissionColor", new Color(0.0f, 77.0f, 255.0f) / 80.0f);
            smokeMat.SetColor("_EmissionColor", new Color(0.0f, 77.0f, 255.0f) / 80.0f);
            centerGlowMat.SetColor("_EmissionColor", new Color(0.0f, 77.0f, 255.0f) / 80.0f);
            glowMat.SetColor("_EmissionColor", new Color(0.0f, 77.0f, 255.0f) / 80.0f);
            sparksMat.SetColor("_EmissionColor", new Color(0.0f, 77.0f, 255.0f) / 80.0f);
        }
        else
        {            
            fireballMat.SetColor("_EmissionColor", previousFireballColor);
            fireMat.SetColor("_EmissionColor", previousFireColor);
            smokeMat.SetColor("_EmissionColor", previousSmokeColor);
            centerGlowMat.SetColor("_EmissionColor", previousCenterGlowColor);
            glowMat.SetColor("_EmissionColor", previousGlowColor);
            sparksMat.SetColor("_EmissionColor", previousSparksColor);
            
        }
    }

    private void OnTriggerEnter(Collider colr){
        if (colr.gameObject.CompareTag("Boost")){
            boostOn = true;
        }       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("WallOpener"))
        {
            if (rigidBody.velocity.magnitude >= 50.0f && jointIsOn)
            {
                other.gameObject.GetComponent<MovingDoorBehavior>().openTheDoor();
            }
        }
    }

    private void OnCollisionEnter(Collision coln){
        if (coln.gameObject.CompareTag("BreakableWall") && magnitude > 50.0f){
            //coln.gameObject.active = false;
            coln.gameObject.GetComponent<BreakableWallBehaviour>().IncreaseBreakingState();
        }
    }
}
