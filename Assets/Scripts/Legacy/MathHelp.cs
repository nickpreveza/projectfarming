using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelp
{
    /// <summary>
    /// this creates a random angle and finds a random point on the Circumference of the Circle.
    /// </summary>
    /// <param name="centerPoint"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector2 GetRandomPointOnCircleCircumference(float radius)
    {
        Vector2 point = new Vector2();
        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        point.x = Mathf.Cos(rad) * radius;
        point.y = Mathf.Sin(rad) * radius;
        return point;
    }
    /// <summary>
    /// this finds a point on the Circumference of the Circle with set angle.
    /// </summary>
    /// <param name="centerPoint"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector2 GetPointOnCircleCircumference(float radius, float angle)
    {
        Vector2 point = new Vector2();
        float rad = angle * Mathf.Deg2Rad;
        point.x = Mathf.Cos(rad) * radius;
        point.y = Mathf.Sin(rad) * radius;
        return point;
    }
    /// <summary>
    /// Get an angle between two points
    /// </summary>
    /// <param name="centerPoint"></param>
    /// <param name="otherPoint"></param>
    /// <returns>Returns an angle between 0-360, it returns -1 if the points are on same position</returns>
    public static float GetAngle(Vector2 centerPoint, Vector2 otherPoint)
    {
        Vector2 relativePoint = (otherPoint - centerPoint).normalized;
        if (relativePoint.x == 0 && relativePoint.y == 0)
        {
            return -1; // no angle, points are on same position
        }
        if (relativePoint.x >= 0 && relativePoint.y >= 0) //first quadrant x > 0 and y > 0
        {
            return Mathf.Asin(relativePoint.y) * Mathf.Rad2Deg;
        }
        else if(relativePoint.x < 0 && relativePoint.y >= 0) //second quadrant if x < 0 and y > 0
        {
            return Mathf.Acos(relativePoint.x) * Mathf.Rad2Deg;
        }
        else if(relativePoint.x < 0 && relativePoint.y < 0) //Third quadrant if x < 0 and y < 0
        {
            return 180 + (180 - (Mathf.Acos(relativePoint.x) * Mathf.Rad2Deg));
        }
        else if(relativePoint.x >= 0 && relativePoint.y < 0)  //Fourth quadrant if x > 0 and y < 0
        {
            //360 degrees + the negative angle will give the correct positive angle. 360 + (-angle)
            return 360 + (Mathf.Asin(relativePoint.y) * Mathf.Rad2Deg);
        }

        return 0;
    }

    /// <summary>
    /// get an angle with positive or negative effect
    /// Example 90 degrees angle with 45 degree angleChange will result in either 135 or 45 angle being returned
    /// </summary>
    /// <param name="angle">Start angle</param>
    /// <param name="angleChange">The size of the changing angle</param>
    /// <returns>Returns an angle + or - the angleChange in 0-360 degrees</returns>
    public static float GetManipulatedAngle(float angle, float angleChange, bool clockwise)
    {
        if (clockwise == false)
        {
            angle += angleChange;
            if (angle >= 360)
                angle -= 360;
            return angle;
        }
        else
        {
            angle -= angleChange;
            if (angle < 0)
                 angle += 360;
            return angle;
        }
    }
    public static Vector2 GetDistanceVector(Vector2 currentPos, Vector2 targetPos, float distance)
    {
        Vector2 direction = targetPos - currentPos;
        return direction.normalized * distance;
    }

    public static Vector2 GetDirectionVector(Vector2 currentPos, Vector2 targetPos)
    {
        Vector2 direction = targetPos - currentPos;
        return direction.normalized;
    }
}
