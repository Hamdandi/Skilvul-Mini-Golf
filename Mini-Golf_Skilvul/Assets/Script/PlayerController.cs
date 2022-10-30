using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] GameObject arrow;
    [SerializeField] Image aim;
    [SerializeField] LineRenderer line;
    [SerializeField] TMP_Text shootCountText;
    [SerializeField] LayerMask ballLayer;
    [SerializeField] LayerMask rayLayer;
    [SerializeField] Transform cameraPivot;
    [SerializeField] Camera cam;
    [SerializeField] Vector2 camSensitivity;
    [SerializeField] float shootForce;

    // [SerializeField] AudioSource hitBall;
    // [SerializeField] AudioManager audioManager;
    [SerializeField] AudioClip hitBallClip;


    Vector3 lastMousePosition;
    float ballDistance;
    bool isShooting;
    Vector3 forceDir;
    float forceFactor;

    Renderer[] arrowRends;
    Color[] arrowOriginalColors;

    int shootCount = 0;

    public int ShootCount { get => shootCount; }

    private void Start()
    {
        ballDistance = Vector3.Distance(
            cam.transform.position, ball.transform.position) + 1;
        arrowRends = arrow.GetComponentsInChildren<Renderer>();
        arrowOriginalColors = new Color[arrowRends.Length];
        for (int i = 0; i < arrowRends.Length; i++)
        {
            arrowOriginalColors[i] = arrowRends[i].material.color;
        }
        arrow.SetActive(false);
    }

    void Update()
    {
        if (ball.IsMoving || ball.IsTeleporting)
            return;

        if (this.transform.position != ball.Position)
        {
            this.transform.position = ball.Position;
            aim.gameObject.SetActive(true);
            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to see if we hit the ball
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, ballDistance, ballLayer))
            {
                isShooting = true;
                arrow.SetActive(true);
                line.enabled = true;


                //sound effect
                // hitBall.Play();
                AudioClip clip = hitBallClip;
                AudioSource.PlayClipAtPoint(clip, transform.position);

            }
        }

        // shooting mode
        if (Input.GetMouseButton(0) == isShooting == true)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // if we hit the ball, draw the line
            if (Physics.Raycast(ray, out hit, ballDistance * 2, rayLayer))
            {
                Debug.DrawLine(ball.Position, hit.point);

                var forceVector = ball.Position - hit.point;
                forceVector = new Vector3(forceVector.x, 0, forceVector.z);
                forceDir = forceVector.normalized;
                var forceMagnitude = forceVector.magnitude;
                forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 5);
                Debug.Log(forceMagnitude);
                forceFactor = forceMagnitude / 5;
            }

            // arrow
            this.transform.LookAt(this.transform.position + forceDir);
            arrow.transform.localScale = new Vector3
            (
                1 + 0.5f * forceFactor,
                1 + 0.5f * forceFactor,
                1 + 2f * forceFactor
            );

            // ubah warna arrow dari putih ke merah
            for (int i = 0; i < arrowRends.Length; i++)
            {
                arrowRends[i].material.color = Color.Lerp(
                    arrowOriginalColors[i],
                    Color.red,
                    forceFactor);
            }

            // aim
            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = Input.mousePosition;

            // line
            var ballScreenPos = cam.WorldToScreenPoint(ball.Position);
            line.SetPositions(new Vector3[] { ballScreenPos, Input.mousePosition });


        }

        // camera Mode
        if (Input.GetMouseButton(0) && isShooting == false)
        {
            var current = cam.ScreenToViewportPoint(Input.mousePosition);
            var last = cam.ScreenToViewportPoint(lastMousePosition);
            var delta = current - last;

            // Rotate the camera around the ball
            // Rotate Horizontal
            cameraPivot.transform.RotateAround
            (
                ball.Position,
                Vector3.up,
                delta.x * camSensitivity.x
            );

            // Rotate Vertical
            cameraPivot.transform.RotateAround
            (
                ball.Position,
                cam.transform.right,
                -delta.y * camSensitivity.y
            );

            var angle = Vector3.SignedAngle
            (
                Vector3.up,
                cam.transform.up,
                cam.transform.right
            );

            if (angle < 3)
            {
                cameraPivot.transform.RotateAround(
                    ball.Position,
                    cam.transform.right,
                    3 - angle);
            }
            else if (angle > 80)
            {
                cameraPivot.transform.RotateAround(
                    ball.Position,
                    cam.transform.right,
                    80 - angle);
            }
        }


        if (Input.GetMouseButtonUp(0) && isShooting)
        {
            ball.AddForce(forceDir * shootForce * forceFactor);
            shootCount += 1;
            shootCountText.text = "Shoot Count: " + shootCount;
            forceFactor = 0;
            forceDir = Vector3.zero;
            isShooting = false;
            arrow.SetActive(false);
            aim.gameObject.SetActive(false);
            line.enabled = false;
        }

        lastMousePosition = Input.mousePosition;
    }
}
