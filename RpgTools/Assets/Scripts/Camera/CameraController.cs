using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    //private Vector2 _previousPlayerPosition;

    //private Transform _boundaryX;
    //private Transform _boundaryY;

    //private Rigidbody _rigidbody;
    public static ClampPositionRange rangeBound;
    void Start()
    {
        //_rigidbody = GetComponent<Rigidbody>();
        //_previousPlayerPosition = player.transform.position;
        //_boundaryX = null;
        //_boundaryY = null;
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = rangeBound.Clamp(player.position, transform.position.z);
    }

    private void FixedUpdate()
    {
        //Vector2 velocity = player.linearVelocity;
       
        //if(_boundaryX != null)
        //{
        //    if(_boundaryX.position.x > transform.position.x && player.transform.position.x < transform.position.x
        //        || _boundaryX.position.x < transform.position.x && player.transform.position.x > transform.position.x)
        //    {
        //        _boundaryX = null;
        //    }
        //    else velocity.x = 0;
        //}
        //if (_boundaryY != null) 
        //{
        //    if (_boundaryY.position.y > transform.position.y && player.transform.position.y < transform.position.y
        //       || _boundaryY.position.y < transform.position.y && player.transform.position.y > transform.position.y)
        //    {
        //        _boundaryY = null;
        //    }
        //    else velocity.y = 0;
        //}
        
        //if(_previousPlayerPosition != (Vector2)player.transform.position)_rigidbody.linearVelocity = velocity;
        //if(_boundaryX == null) transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        //if(_boundaryY == null) transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);

        //_previousPlayerPosition = player.transform.position;

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag != "CameraBoundary") return;
    //    if (player.linearVelocity.x != 0)
    //    {
    //        _boundaryX = collision.transform;
    //    }
    //    else if (player.linearVelocity.y != 0) 
    //    {
    //        _boundaryY = collision.transform;
    //    }
    //}
}

public struct ClampPositionRange
{
    public float xMin, xMax;

    public float yMin, yMax;

    public ClampPositionRange(float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;
    }

    public Vector3 Clamp(Vector3 position, float z)
    {
        return new Vector3(Mathf.Clamp(position.x, xMin, xMax), Mathf.Clamp(position.y, yMin, yMax), z);
    }
}