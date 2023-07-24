using UnityEngine;

public class GrapplingGun : MonoBehaviour
{

    //private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public PlayerMovementGrappling pm;

    public Transform gunTip, camera, player;
    private float maxDistance = 30;
    private SpringJoint joint;

    [Header("OdmGear")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCanstRadius;
    public Transform predictionPoint;


    void Awake()
    {
        //lr = GetComponent<LineRenderer>();
    }
    private void OdmGearMovement()
    {
        if (Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);

        if (Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);

        if (Input.GetKey(KeyCode.W)) rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 disrectionToPoint = grapplePoint - transform.position;
            rb.AddForce(disrectionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, grapplePoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
        

    }
    private void ChackForSwingPoinsts()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(camera.position, predictionSphereCanstRadius, camera.forward,
                            out sphereCastHit, maxDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(camera.position, camera.forward,
                                out raycastHit, maxDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;

        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;

        else
            realHitPoint = Vector3.zero;
        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
            StopGrapple();
        }
        ChackForSwingPoinsts();

        if (joint != null) OdmGearMovement();


    }
    

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {  
        // return if predictionHit not found
        if (predictionHit.point == Vector3.zero) return;

        pm.swinging = true;
        AudioManager.instance.GrapplingGusSFX();
        grapplePoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        // customize values as you like
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        //lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    /// 
    

    void StopGrapple()
    {
        
        pm.swinging = false;

        pm.sliding = true;
        //lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        //lr.SetPosition(0, gunTip.position);
        //lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
