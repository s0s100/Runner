using System.Collections.Generic;
using UnityEngine;

public struct WireSegment
{
    public Vector2 posNew;
    public Vector2 posOld;

    public WireSegment(Vector2 startingPos)
    {
        this.posNew = startingPos;
        this.posOld = startingPos;
    }
}

public class Wire : MonoBehaviour
{
    private const float DESTRUCTION_TIME = 5.0f;

    private LineRenderer lineRenderer;
    private List<WireSegment> wireSegments = new List<WireSegment>();

    [SerializeField]
    private Color wireColorStart = Color.black;
    [SerializeField]
    private Color wireColorEnd = Color.blue;
    [SerializeField]
    private float segmentLength = 0.5f;
    [SerializeField]
    private int numOfSegments = 20;
    [SerializeField]
    private float wireWidth = 0.1f;
    [SerializeField]
    private int constrainIterNumber = 20;

    [SerializeField]
    private Vector2 connectedObjShift = Vector2.zero;

    [SerializeField]
    private GameObject connectedObj;

    private void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        SetWire();

        // Set line renderer
        lineRenderer.startColor = wireColorStart;
        lineRenderer.endColor = wireColorEnd;

        lineRenderer.startWidth = wireWidth;
        lineRenderer.endWidth = wireWidth;

        lineRenderer.positionCount = numOfSegments;
    }

    private void SetWire()
    {
        Vector3 wireStartPoint = this.transform.position;
        Vector3 wireEndPoint = connectedObj.transform.position;
        Vector3 difference = (wireStartPoint - wireEndPoint) / numOfSegments;

        for (int i = 0; i < numOfSegments; i++)
        {
            WireSegment newWireSegment = new WireSegment(wireStartPoint);
            wireSegments.Add(newWireSegment);
            wireStartPoint -= difference;
        }
    }

    private void Update()
    {
        DrawWire();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    private void Simulate()
    {
        ApplyVelocity();

        for (int i = 0; i < constrainIterNumber; i++)
        {
            ApplyConstraint();
        }
    }

    private void ApplyVelocity()
    {
        Vector2 gravityDirection = new Vector2(0.0f, -1.0f);

        for (int i = 0; i < numOfSegments; i++)
        {
            WireSegment segment = wireSegments[i];
            Vector2 velocity = segment.posNew - segment.posOld;

            segment.posOld = segment.posNew;

            segment.posNew += velocity;
            segment.posNew += gravityDirection * Time.deltaTime;

            wireSegments[i] = segment;
        }
    }

    private void ApplyConstraint()
    {
        SetFirstPosition();
        SetLastPosition();

        for (int i = 0; i < numOfSegments - 1; i++)
        {
            WireSegment firstSeg = wireSegments[i];
            WireSegment nextSeg = wireSegments[i + 1];

            float dist = (firstSeg.posNew - nextSeg.posNew).magnitude;
            float error = Mathf.Abs(dist - segmentLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > segmentLength)
            {
                changeDir = (firstSeg.posNew - nextSeg.posNew).normalized;
            }
            else if (dist < segmentLength)
            {
                changeDir = (nextSeg.posNew - firstSeg.posNew).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNew -= changeAmount * 0.5f;
                wireSegments[i] = firstSeg;
                nextSeg.posNew += changeAmount * 0.5f;
                wireSegments[i + 1] = nextSeg;
            }
            else
            {
                nextSeg.posNew += changeAmount;
                wireSegments[i + 1] = nextSeg;
            }
        }
    }

    private void DrawWire()
    {
        Vector3[] wirePositions = new Vector3[numOfSegments];
        for (int i = 0; i < numOfSegments; i++)
        {
            wirePositions[i] = wireSegments[i].posNew;
        }

        lineRenderer.SetPositions(wirePositions);
    }

    private void SetFirstPosition()
    {
        WireSegment firstSegment = wireSegments[0];
        firstSegment.posNew = this.transform.position;
        wireSegments[0] = firstSegment;
    }

    private void SetLastPosition()
    {
        WireSegment lastSegment = wireSegments[numOfSegments - 1];
        if (connectedObj != null)
        {
            lastSegment.posNew = connectedObj.transform.position;
        }
        wireSegments[numOfSegments - 1] = lastSegment;
    }

    public void UnattachAndDestroyWire()
    {
        connectedObj = null;

        Destroy(this.gameObject, DESTRUCTION_TIME);
    }
}
