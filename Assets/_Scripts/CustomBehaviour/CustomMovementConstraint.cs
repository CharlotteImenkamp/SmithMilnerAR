using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

public class CustomMovementConstraint : TransformConstraint
{
    #region Properties

    /// <summary>
    /// Enable Y-Axis Movement Constraint 
    /// </summary>

    [SerializeField]
    [Tooltip("Apply Constraint or not. Use for Debugging")]
    private bool useConstraint = true;

    /// <summary>
    ///  Referenced Object is used for Movement Boundaries
    /// </summary>
    public Transform ReferenceTransform { get => referenceTransform; }

    [Tooltip("Use Objects Transformation to calculate MovementBoundaries ")]
    private Transform referenceTransform;

    /// <summary>
    /// Constrain movement along an axis
    /// </summary>
    /// 
    [SerializeField]
    [EnumFlags]
    [Tooltip("Constrain movement along an axis")]
    private AxisFlags constraintOnMovement = 0;

    public AxisFlags ConstraintOnMovement
    {
        get => constraintOnMovement;
        set => constraintOnMovement = value;
    }

    public override TransformFlags ConstraintType => TransformFlags.Move;
    private float minYAxis;

    #endregion Properties


    #region Public Methods

    public override void Initialize(MixedRealityTransform worldPose)
    {
        base.Initialize(worldPose);
        GetLowerBorder();
    }

    public override void ApplyConstraint(ref MixedRealityTransform transform)
    {
        if (useConstraint)
        {
            Vector3 position = transform.Position;

            // Apply constraints on y-Axis if neccessary
            if (constraintOnMovement.HasFlag(AxisFlags.YAxis))
            {
                if (transform.Position.y <= minYAxis)
                {
                    position.y = worldPoseOnManipulationStart.Position.y;
                    Debug.Log("Position Constraint");
                }
            }
            transform.Position = position;
        }
    }
    #endregion Public Methods

    #region Private Methods
    private void GetLowerBorder()
    {
        if (referenceTransform == null)
        {
            referenceTransform = GameObject.FindGameObjectWithTag("MovementConstraint").transform;
        }
        minYAxis = referenceTransform.position.y; 
    }
    #endregion Private Methods
}
