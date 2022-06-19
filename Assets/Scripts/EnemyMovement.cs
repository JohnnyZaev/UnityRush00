using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float enemySpeed;
    [SerializeField] private float baseRange;
    private float _enemyRotation;
    private Transform _enemyTransform;
    private Rigidbody2D _enemyRb;
    private Transform _target;
    private Vector2 _lastPosition;
    private float _followTimer;
    private Vector2 _startPosition;
    private Quaternion _startRotation;
    private float _chaseTime;
    private const float MaxChaseTime = 4f;
    private const double posDelta = 0.2f;

    private void Awake()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        _enemyTransform = GetComponent<Transform>();
        _enemyRb.gravityScale = 0;
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _target = transform;
    }

    private void Update()
    {
        Vector2 _goto = _target == transform ? _startPosition : _target.position;
        FollowTarget(_goto);
        Vector2 _lookat = _target == transform ? _startPosition : _target.position;
        LookAtTarget(_lookat);
        CheckTime();
    }

    private void CheckTime()
    {
        if (_target != transform)
            _chaseTime += Time.deltaTime;
        if (_chaseTime > MaxChaseTime)
            ReturnToSpawn();
    }

    private void FollowTarget(Vector2 pos)
    {
        if (Vector2.Distance(transform.position, pos) > baseRange || GoingToSpawn())
            ApproachTarget(pos);
        else
        {
            _enemyRb.velocity = Vector2.zero;
            if (_target == transform)
                transform.rotation = _startRotation;
        }
    }

    private bool GoingToSpawn()
    {
        if (transform == _target)
            if (Vector2.Distance(transform.position, _startPosition) > posDelta)
                return true;
            else
            {
                transform.rotation = _startRotation;
                _enemyRb.velocity = Vector2.zero;
            }
        return false;
    }

    private void ApproachTarget(Vector2 pos)
    {
        var dir = (pos - (Vector2)transform.position).normalized;
        _enemyRb.velocity = new Vector2(dir.x, dir.y) * enemySpeed;
    }
    
    private void LookAtTarget(Vector2 pos)
    {
        if (transform == _target && !GoingToSpawn())
            return;
        var position = _enemyTransform.position;
        var target = pos;
        _enemyRotation = Mathf.Atan2(target.y - position.y,
            target.x - position.x) * Mathf.Rad2Deg + 90;
        _enemyRb.transform.eulerAngles = Vector3.forward * _enemyRotation;
    }

    public void GoAfter(Transform target)
    {
        _target = target;
        _chaseTime = 0;
        //start shooting here
    }

    public void ReturnToSpawn()
    {
        _target = transform;
        _chaseTime = 0;
        //end shooting here
    }
}
