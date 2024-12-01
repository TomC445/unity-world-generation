using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lookSensitivity = 2f;
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _gravity = -9.81f;
    #endregion

    #region Properties
    private CharacterController _characterController;
    private Vector3 _velocity;
    private Transform _cameraTransform;
    private float _verticalLookRotation = 0f;
    #endregion

    #region Methods
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.tag == "Terrain")
                    hit.transform.GetComponent<Marching>().PlaceTerrain(hit.point);

            }

        }

        if (Input.GetMouseButtonDown(1))
        {

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.tag == "Terrain")
                    hit.transform.GetComponent<Marching>().RemoveTerrain(hit.point);

            }

        }

    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        var move = transform.right * moveX + transform.forward * moveY;

        _characterController.Move(move * _speed * Time.deltaTime);

        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && _characterController.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * _lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _lookSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        _verticalLookRotation -= mouseY;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);
        _cameraTransform.localRotation = Quaternion.Euler(_verticalLookRotation, 0f, 0f);
    }
    #endregion
}
