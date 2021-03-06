using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float playerSpeed;
	[SerializeField] private Camera mainCamera;
	[SerializeField] private UnityEvent deathSound;
	[SerializeField] private GameObject finishScreen;
	private Vector2 _velocityMovement;
	private Vector2 _mousePosition;
	private float _playerRotation;
	private Transform _playerTransform;
	private Rigidbody2D _playerRb;
	private bool _canPlaySound;
	private bool _soundIsPlayed;

	private void Awake()
	{
		_playerRb = GetComponent<Rigidbody2D>();
		_playerTransform = GetComponent<Transform>();
		_playerRb.gravityScale = 0;
	}

	private void FixedUpdate()
	{
		if (_canPlaySound && !_soundIsPlayed)
		{
			deathSound?.Invoke();
			_canPlaySound = false;
			_soundIsPlayed = true;
		}
		if (!GameManager.Instance.PlayerAlive)
		{
			_canPlaySound = true;
			return;
		}

		_velocityMovement.x = Input.GetAxis("Horizontal");
		_velocityMovement.y = Input.GetAxis("Vertical");

		_playerRb.velocity = _velocityMovement * (playerSpeed * Time.fixedDeltaTime);
		_mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		var position = _playerTransform.position;
		_playerRotation = Mathf.Atan2(_mousePosition.y - position.y, _mousePosition.x - position.x) * Mathf.Rad2Deg + 90;
		_playerRb.transform.eulerAngles = Vector3.forward * _playerRotation;
	}

	private void OnDestroy()
	{
		deathSound?.Invoke();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Finish"))
		{
			GameManager.Instance.PlayVictoryOrDefeatSound(true);
			finishScreen.SetActive(true);
			GameManager.Instance.PlayerAlive = false;
		}
	}
}
