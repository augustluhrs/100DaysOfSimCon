using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeMover : MonoBehaviour
{
    public HingeJoint frontRight;
    public HingeJoint frontLeft;
    public HingeJoint backRight;
    public HingeJoint backLeft;

    void Start()
    {
        JointSpring fL = frontLeft.spring;
        fL.spring = 15;
        fL.targetPosition = 70;
        frontLeft.spring = fL;
        frontLeft.useSpring = true;

        JointSpring fR = frontRight.spring;
        fR.spring = 5;
        fR.targetPosition = 70;
        frontRight.spring = fR;
        frontRight.useSpring = true;

        JointSpring bL = backLeft.spring;
        bL.spring = 5;
        bL.targetPosition = -70;
        backLeft.spring = bL;
        backLeft.useSpring = true;

        JointSpring bR = backRight.spring;
        bR.spring = 15;
        bR.targetPosition = -70;
        backRight.spring = bR;
        backRight.useSpring = true;

    }

    // Update is called once per frame
    void Update()
    {
        /*
        JointSpring fL = frontLeft.spring;
        fL.spring = 15;
        fL.targetPosition = 70;
        frontLeft.spring = fL;
        frontLeft.useSpring = true;

        JointSpring fR = frontRight.spring;
        fR.spring = 5;
        fR.targetPosition = 70;
        frontRight.spring = fR;
        frontRight.useSpring = true;

        JointSpring bL = backLeft.spring;
        bL.spring = 5;
        bL.targetPosition = -70;
        backLeft.spring = bL;
        backLeft.useSpring = true;

        JointSpring bR = backRight.spring;
        bR.spring = 15;
        bR.targetPosition = -70;
        backRight.spring = bR;
        backRight.useSpring = true;
        */
    }
}
