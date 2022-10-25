using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [Header("Movement")]
    [SerializeField] private float _lookSpeed;
    [SerializeField] private float _movementSpeed;

    public Entity Target { get; set; }
    public float Health { get; set; } = 100f;
    public float Damage { get; set; } = 10f;
    public bool CanMove { get; set; }

    private Transform m_transform;

    private MaterialPropertyBlock m_block;
    private void Awake()
    {
        m_transform = transform;

        m_block = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(m_block);
        m_block.SetColor("_Color", Random.ColorHSV());
        _renderer.SetPropertyBlock(m_block);
    }

    public void SetCharacteristic(float health, float damage)
    {
        Health = health;
        Damage = damage;
    }

    private void FixedUpdate()
    {
        if (Target == null || !CanMove) return;

        m_transform.SetPositionAndRotation(
            Vector3.Lerp(m_transform.position, Target.transform.position, Time.fixedDeltaTime * _movementSpeed),
            Quaternion.Lerp(m_transform.rotation, Quaternion.LookRotation(Target.transform.position - m_transform.position), Time.fixedDeltaTime * _lookSpeed));

    }
}