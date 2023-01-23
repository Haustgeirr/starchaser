using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public SpriteRenderer damageFill;
    public SpriteRenderer damageBorder;
    public ParticleSystem engineParticles;
    public ParticleSystem gatherParticles;
    public ParticleSystem stoppingParticles;

    private float hullTimer = 0f;

    void OnStoppingStart() { stoppingParticles.Play(); }
    void OnStoppingStop() { stoppingParticles.Stop(); }

    void OnGatherStart()
    {
        gatherParticles.Play();
    }

    void OnGatherStop()
    {
        gatherParticles.Stop();
    }

    void UpdateHullUI(float time, float timeBeforeHullLoss)
    {
        if (time <= 0)
        {
            damageFill.color = new Color(damageFill.color.r, damageFill.color.g, damageFill.color.b, 0);
            damageFill.transform.localScale = Vector3.zero;
            damageBorder.color = new Color(damageBorder.color.r, damageBorder.color.g, damageBorder.color.b, 0);
            return;
        }

        damageBorder.color = new Color(damageBorder.color.r, damageBorder.color.g, damageBorder.color.b, 1);
        var lerpAmount = time / timeBeforeHullLoss;
        var opacity = Mathf.Lerp(0, 1, lerpAmount);
        var scale = Vector3.Lerp(Vector3.zero, Vector3.one, lerpAmount);

        damageFill.color = new Color(damageFill.color.r, damageFill.color.g, damageFill.color.b, opacity);
        damageFill.transform.localScale = scale;
    }

    private void OnEnable()
    {
        Player.onUpdateHullUI += UpdateHullUI;
        Player.onStoppingStart += OnStoppingStart;
        Player.onStoppingStop += OnStoppingStop;
        ShipResources.onGatherStart += OnGatherStart;
        ShipResources.onGatherStop += OnGatherStop;
    }

    private void OnDisable()
    {
        Player.onUpdateHullUI -= UpdateHullUI;
        Player.onStoppingStart -= OnStoppingStart;
        Player.onStoppingStop -= OnStoppingStop;
        ShipResources.onGatherStart -= OnGatherStart;
        ShipResources.onGatherStop -= OnGatherStop;
    }

    void Update()
    {
        if (Input.GetKeyDown("w")) { engineParticles.Play(); }
        if (Input.GetKeyUp("w")) { engineParticles.Stop(); }
    }
}
