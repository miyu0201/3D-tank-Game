using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;   //starting health value       
    public Slider m_Slider;     //life slider                   
    public Image m_FillImage;    //fill theh slider                  
    public Color m_FullHealthColor = Color.green;  
    public Color m_ZeroHealthColor = Color.red;    //low health red color
    public GameObject m_ExplosionPrefab;
    
    
    private AudioSource m_ExplosionAudio;   //audio   
    private ParticleSystem m_ExplosionParticles;   //explosion effect
    private float m_CurrentHealth;  
    private bool m_Dead;   //check if dead         


    private void Awake()
    {
        //prepare example of explosion effect at awake (initial)
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
        //set inactive
        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }
    
    //call from outside when tank is demaged

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        m_CurrentHealth -= amount;

        SetHealthUI();

        //if tank currentHealth <0, is dead
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = m_CurrentHealth;

        //change fill color
        //Color.Lerp Linear interpolation of colors, interpolating between colors 1 and 2 through the third parameter.
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;

        //set explosion location as tank dead location
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        //play explosion effect and sound
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        gameObject.SetActive(false);

    }
}