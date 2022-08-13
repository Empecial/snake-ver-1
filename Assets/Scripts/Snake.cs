using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private Vector2 Direction = Vector2.right;
    private List<Transform> Segments = new List<Transform>();
    int bananas = 0;

    [SerializeField]
    private Text bananaPoints;

    [SerializeField]
    private Transform segmentPrefab;

    [SerializeField]
    private int initialSize = 0;

    [SerializeField]
    private AudioClip monkey;

    [SerializeField]
    private bool GoingLeft, GoingRight;

    // Start is called before the first frame update
    void Start()
    {
        ResetState();
        bananaPoints.text = bananas.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Direction = Vector2.up;
            GoingLeft= false;
            GoingRight =false;
        }
        else if (Input.GetKeyDown(KeyCode.A) && !GoingRight)
        {
            Direction = Vector2.left;
            GoingLeft = true;
            GoingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Direction = Vector2.down;
            GoingLeft = false;
            GoingRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !GoingLeft)
        {
            Direction = Vector2.right;
            GoingRight = true;
            GoingLeft = false;
        }
    }
    private void FixedUpdate()
    {
        for (int i = Segments.Count-1; i > 0; i--)
        {
            Segments[i].position = Segments[i - 1].position;
        }

        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) 
            + Direction.x, Mathf.Round(this.transform.position.y) + Direction.y, 0.0f);
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);

        //will get the last object in the Segments list and set the new segment here
        //thus growing the body
        segment.position = Segments[Segments.Count - 1].position;

        Segments.Add(segment);
    }


    void ResetState()
    {
        for (int i = 1; i < Segments.Count; i++)
        {
            Destroy(Segments[i].gameObject);
            bananaPoints.text = "0";
        }

        Segments.Clear();
        Segments.Add(this.transform);

        for (int i = 1; i < this.initialSize; i++)
        {
            Segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
            AddPoint();
            Debug.Log("Mmmm.... banana");

            AudioSource audio = gameObject.GetComponent<AudioSource>();
            monkey = gameObject.GetComponent<AudioClip>();
            audio.Play();


        }
        else if (other.CompareTag("Obstacle"))
        {
            ResetState();
        }
    }

    private void AddPoint()
    {
        bananas = bananas + 1;
        bananaPoints.text = bananas.ToString();
    }

}
