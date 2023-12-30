using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    protected Slider healthbar;
    private IDamageable damageable;
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = this.GetComponentInChildren<Slider>();
        damageable = this.GetComponentInParent<IDamageable>();
        canvas = this.GetComponentInParent<Canvas>();

        healthbar.minValue = 0;
        healthbar.maxValue = damageable.GetHP();
    }

    // Update is called once per frame
    public void Update()
    {
        if (damageable != null && canvas != null && healthbar != null && !damageable.IsDead)
        {
            canvas.transform.LookAt(this.transform.root.position + Camera.main.transform.forward);
            healthbar.value = damageable.GetHP();
        }
    }

    public void HealthUpdate()
    {
        if (damageable != null && healthbar != null && !damageable.IsDead)
        {
            healthbar.value = damageable.GetHP();
        }
    }
}
