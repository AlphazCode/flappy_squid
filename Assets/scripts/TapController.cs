
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {
	
	public delegate void PlayerDelegate();
	public static event PlayerDelegate OnPlayerDied;
	public static event PlayerDelegate OnPlayerScored;
	public Animator animator;
	public float tapForce = 10;
	public float tiltSmooth = 5;
	public Vector3 startPos;
	public AudioSource tapSound;
	public AudioSource scoreSound;
	public AudioSource dieSound;
	public ParticleSystem particle;
	Rigidbody2D rigidBody;
	Quaternion downRotation;
	Quaternion forwardRotation;
	public bool isDead;
	GameManager game;
	public void set_isDead(bool value)
    {
		isDead = value;
    }
	void Start() {
		rigidBody = GetComponent<Rigidbody2D>();
		downRotation = Quaternion.Euler(0, 0 ,-100);
		forwardRotation = Quaternion.Euler(0, 0, 40);
		game = GameManager.Instance;
		rigidBody.simulated = false;
		animator = gameObject.GetComponent<Animator>();
		set_isDead(false);
	}

	void OnEnable() {
		GameManager.OnGameStarted += OnGameStarted;
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	void OnDisable() {
		GameManager.OnGameStarted -= OnGameStarted;
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	void OnGameStarted() {
		rigidBody.velocity = Vector3.zero;
		rigidBody.simulated = true;

	}

	void OnGameOverConfirmed() {
		transform.localPosition = startPos;
		transform.rotation = Quaternion.identity;

	}

	void Update()
	{
		animator.SetBool("isDead", isDead);
		if (game.GameOver) return; 
		if (Input.GetMouseButtonDown(0)) {
			rigidBody.velocity = Vector2.zero;
			transform.rotation = forwardRotation;
			rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
			particle.Play();
			animator.SetTrigger("Jump");
			tapSound.Play();
		}
		if (!isDead) animator.SetTrigger("Idle");
		else animator.Play("Die");
		transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "ScoreZone") {
			OnPlayerScored();
			scoreSound.Play();
		}
		if (col.gameObject.tag == "DeadZone") {
			rigidBody.simulated = false;
			OnPlayerDied();
			set_isDead(true);
			animator.StopPlayback();
			animator.SetBool("isDead", isDead);
			dieSound.Play();
		}
	}

}
