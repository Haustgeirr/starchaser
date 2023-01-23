using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public float acceleration = 10f;
    public float rotationSpeed = 100f;
    public float maxSpeed = 100f;

    public Vector3 velocity;
    public float fullStopTime = 0.5f;
    public float deadStopSpeed = 0.25f;

    public delegate void OnUpdateHullUI(float t, float timerBeforeHullLoss);
    public static OnUpdateHullUI onUpdateHullUI;

    public delegate void OnStoppingStart();
    public static OnStoppingStart onStoppingStart;


    public delegate void OnStoppingStop();
    public static OnStoppingStop onStoppingStop;

    [Header("Hull")]
    public float timeBeforeHullLoss = 1.5f;
    private float hullIntegrityTimer = 0f;
    private bool isLosingHullIntegrity = false;

    private float stoppingTime = 0f;
    private Vector3 stoppingInitialVelocity;

    private ShipResources ship;
    private Detector detector;

    public void Init()
    {
        isLosingHullIntegrity = false;
        onStoppingStop();
        onUpdateHullUI(0, timeBeforeHullLoss);

        ship.Init();
        detector.Init();
        GameManager.instance.PlayDialogue(5);
    }

    public void TakeFatalDamage(string message)
    {
        onUpdateHullUI(timeBeforeHullLoss, timeBeforeHullLoss);
        GameManager.instance.GameOver(message);
    }

    public void StopTakeDamage()
    {
        if (isLosingHullIntegrity)
        {
            isLosingHullIntegrity = false;
            hullIntegrityTimer = 0f;
            onUpdateHullUI(hullIntegrityTimer, timeBeforeHullLoss);
            // timeOfHullLoss = Mathf.Infinity;
        }
    }

    public void TakeDamage()
    {
        if (!isLosingHullIntegrity)
        {
            isLosingHullIntegrity = true;
            hullIntegrityTimer = 0.0001f;
            onUpdateHullUI(hullIntegrityTimer, timeBeforeHullLoss);
            // timeOfHullLoss = Time.time + timeBeforeHullLoss;
        }
    }

    void GetVelocityFromInput()
    {
        if (Input.GetKeyUp("space"))
        {
            onStoppingStop();
        }

        if (Input.GetKeyDown("space"))
        {
            stoppingInitialVelocity = velocity;
            onStoppingStart();
        }

        if (Input.GetKey("space"))
        {
            Vector3 prevVelocity = velocity;

            if (Vector3.SqrMagnitude(velocity) < deadStopSpeed * deadStopSpeed)
            {
                velocity = Vector3.zero;
                onStoppingStop();
                return;
            }

            Vector3 newVelocity = Vector3.Lerp(stoppingInitialVelocity, Vector3.zero, Util.CubicEaseOut(stoppingTime, fullStopTime));
            stoppingTime += Time.deltaTime;

            float powerConsumption = (prevVelocity.sqrMagnitude - newVelocity.sqrMagnitude) * Time.deltaTime;

            if (ship.PowerAmount() < powerConsumption)
            {
                // UI Ping
                ship.ConsumePower(ship.PowerAmount());
                return;
            }

            ship.ConsumePower(powerConsumption);
            velocity = newVelocity;
            return;
        }

        stoppingTime = 0;

        float speed = Mathf.Clamp01(Input.GetAxisRaw("Vertical")) * acceleration;
        speed *= Time.deltaTime;

        if (ship.FuelAmount() < speed)
        {
            ship.ConsumeFuel(ship.FuelAmount());
            return;
        }

        ship.ConsumeFuel(speed);
        velocity += speed * transform.forward;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }


    void Start()
    {
        ship = GetComponent<ShipResources>();
        detector = GetComponent<Detector>();
    }

    void Update()
    {
        if (!GameManager.instance.IsPlaying())
            return;


        if (isLosingHullIntegrity)
        {
            onUpdateHullUI(hullIntegrityTimer, timeBeforeHullLoss);
            if (hullIntegrityTimer >= timeBeforeHullLoss)
            {
                TakeFatalDamage("The molten ship breifly shone in flattery of it's neighbour.\nBut then turned to ash.");
            }

            hullIntegrityTimer += Time.deltaTime;
        }

        GetVelocityFromInput();
        transform.position += velocity * Time.deltaTime;

        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }
}
