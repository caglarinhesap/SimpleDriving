using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float acceleration = 0.3f;
    [SerializeField] private float turningSpeed = 200f;

    private int steerValue; // Positive for turning right, negative for turning left.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed += acceleration * Time.deltaTime;
        transform.Rotate(0f, steerValue * turningSpeed * Time.deltaTime, 0f);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Steer(int value)
    {
        steerValue = value;
    }


}
