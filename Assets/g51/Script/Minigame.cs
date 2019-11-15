using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public float progress;
    public RectTransform point;
    public Vector2 pointAncoredPosition;
    
    public float force;
    public float dampeng;

    void PointUpdate()
    {
        pointAncoredPosition += Velocity(true);
        point.anchoredPosition = pointAncoredPosition;
    }

    Vector2 velocity;
    Vector2 Velocity(bool calculateNew)
    {
        Vector2 returned = velocity;
        if (calculateNew)
            velocity -= velocity * dampeng * Time.deltaTime;
        return returned;
    }
    public void AddVelocity(Vector2 add)
    {
        velocity += add;
    }

    void Input()
    {
        float x = UnityEngine.Input.GetAxis("Horizontal");
        float y = UnityEngine.Input.GetAxis("Vertical");
        Vector2 inputVel = RemapRectToCircle(new Vector2(x, y));
        AddVelocity(inputVel * force * Time.deltaTime);
    }


    public TickTimeForce tickTimeForce = new TickTimeForce();
    [System.Serializable]
    public class TickTimeForce
    {
        public Vector2 tickRange;
        private float timer;
        public float force;
        public void PointLive(Minigame minigame)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                minigame.AddVelocity(RemapRectToCircle(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))) * force);
                timer = Random.Range(tickRange.x, tickRange.y);
            }
        }
    }

    
    private void Update()
    {
        PointUpdate();
        Input();
        tickTimeForce.PointLive(this);
    }

    static Vector2 RemapRectToCircle(Vector2 input)
    {
        return new Vector2(
            input.x * Mathf.Sqrt(1 - input.y * input.y * 0.5f),
            input.y * Mathf.Sqrt(1 - input.x * input.x * 0.5f));
    }
}
