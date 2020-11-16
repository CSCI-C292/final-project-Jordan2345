using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelTicker : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float _tickerSpeed = 3f;
    [SerializeField] GameObject gate;
    [SerializeField] GameObject ticker;
    [SerializeField] RuntimeData _runtimeData;

    int waypointIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name.Equals("Ticker"))
            //based off of - https://www.youtube.com/watch?v=ExRQAEm4jPg
            transform.position = waypoints[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.name.Equals("Ticker"))
            Move();
    }
    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, _tickerSpeed * Time.deltaTime);
        if(transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex = waypointIndex == 1 ? 0 : 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.name.Equals("Ticker"))
        {
            Debug.Log("Ticker Crossed");
            float height = (transform.position.y - waypoints[0].transform.position.y) * 6.25f;
            int score = (int) height;
            Debug.Log(score);
            Destroy(gameObject);
            _runtimeData._gateScore += score;
        }
        else if(gameObject.name.Equals("Gate"))
        {
            Debug.Log("Missed Ticker");
        }
        AudioManager.AudioInstance.StopSound();
        AudioManager.AudioInstance.PlaySound("Level Clear");
        Destroy(gate);
        Destroy(ticker);

    }
}
