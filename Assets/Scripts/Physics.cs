using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{
    // real seconds per ingame second
    public const int timeUnit = (int) 3.154E6;
    // Meters per ingame unit
    public const int distanceUnit = (int) 1E9;
    // Gravitational Constant in m^3/(kg*s^2)
    public const float G = 6.6743E-11f;
    public List<PhysicalObject> registered = new List<PhysicalObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(PhysicalObject po in registered)
        {
            // calculate resulting force
            Vector3 res = Vector3.zero;
            foreach(PhysicalObject grav in registered)
            {
                // Skip if it's the same object
                if (grav == po)
                        continue;
                // Add Force acted upon po to resulting Force in simulated units (N)
                res += gravityBetween(grav, po);
            }
            // calculate acceleration (Newton's second law) in simulated units (m/s^2)
            Vector3 a = res / (float) po.mass;
            // accelerate velocity in simulated units (m/s)
            po.velocity += floatRealToSimTime(Time.deltaTime) * a;
            // apply velocity
            po.transform.position += vectorSimToRealLength(floatRealToSimTime(Time.deltaTime) * po.velocity);
            // draw vectors
            Debug.DrawRay(po.transform.position, vectorSimToRealVelocity(po.velocity), Color.green, Time.deltaTime);
            Debug.DrawRay(po.transform.position, vectorSimToRealVelocity(a)*timeUnit, Color.red, Time.deltaTime);
        }
    }

    // returns a Vector spanning from a to b in game coordinates
    Vector3 distance(PhysicalObject a, PhysicalObject b)
    {
        return b.transform.position - a.transform.position;
    }

    // returns the gravity force exerted on the recipient due to the influence of the emitter in Newtons
    Vector3 gravityBetween(PhysicalObject emitter, PhysicalObject recipient)
    {
        // distance between objects in Meters
        Vector3 d = distance(recipient, emitter)*distanceUnit;
        // Newtons gravitational equation returns Force acted upon the recipient in Newtons
        double mag = G * emitter.mass * recipient.mass / Mathf.Pow(d.magnitude, 2);
        return d.normalized * (float) mag;
    }

    // returns the value of a real dimensioned distance Vector3 in simulated dimensions
    Vector3 vectorRealToSimLength(Vector3 realIn)
    {
        return realIn * (float)distanceUnit;
    }

    // returns the value of a simulated dimensioned distance Vector3 in real dimensions
    Vector3 vectorSimToRealLength(Vector3 simIn)
    {
        return simIn / (float)distanceUnit;
    }

    // returns the value of a real dimensioned time float in simulated dimensions
    float floatRealToSimTime(float realIn)
    {
        return realIn * (float)timeUnit;
    }

    // returns the value of a simulated dimensioned time float in real dimensions
    float floatSimToRealTime(float simIn)
    {
        return simIn / (float)timeUnit;
    }

    // returns the value of a real dimensioned velocity Vector3 in simulated dimensions
    Vector3 vectorRealToSimVelocity(Vector3 realIn)
    {
        return vectorRealToSimLength(realIn) / floatRealToSimTime(1);
    }

    // returns the value of a simulated dimensioned velocity Vector3 in real dimensions
    Vector3 vectorSimToRealVelocity(Vector3 simIn)
    {
        return vectorSimToRealLength(simIn) / floatSimToRealTime(1);
    }
}
