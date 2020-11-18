using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMovement : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float _movementSpeed = 3f;
    [SerializeField] RuntimeData _runtimeData;

    private string currentLevel;
    private string currAnim = "";
    private bool flag = false;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = _runtimeData._currentLevel != null ? _runtimeData._currentLevel : waypoints[0]+"";
        gameObject.transform.position = FindWaypoint(currentLevel);

        _runtimeData._currentLevel = currentLevel;
        Debug.Log(waypoints[0].name);
        AudioManager.AudioInstance.PlaySound("Yoshi's Island Music");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Entering " + currentLevel);
            LoadScenes.SceneInstance.LoadScene(currentLevel);

        }
        if(Input.GetKeyDown(KeyCode.UpArrow) && !ChooseLevel.ChooseLevelInstance.CanGoUp(currentLevel).Equals(""))
        {
            currentLevel = ChooseLevel.ChooseLevelInstance.CanGoUp(currentLevel);
            animator.SetBool("goingUp", true);
            currAnim = "goingUp";
            flag = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !ChooseLevel.ChooseLevelInstance.CanGoDown(currentLevel).Equals(""))
        {
            currentLevel = ChooseLevel.ChooseLevelInstance.CanGoDown(currentLevel);
            animator.SetBool("goingDown", true);
            currAnim = "goingDown";
            flag = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !ChooseLevel.ChooseLevelInstance.CanGoRight(currentLevel).Equals(""))
        {
            currentLevel = ChooseLevel.ChooseLevelInstance.CanGoRight(currentLevel);
            animator.SetBool("goingRight", true);
            currAnim = "goingRight";
            GetComponent<SpriteRenderer>().flipX = false;
            flag = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !ChooseLevel.ChooseLevelInstance.CanGoLeft(currentLevel).Equals(""))
        {
            currentLevel = ChooseLevel.ChooseLevelInstance.CanGoLeft(currentLevel);
            animator.SetBool("goingRight", true);
            currAnim = "goingRight";
            GetComponent<SpriteRenderer>().flipX = true;
            flag = true;
        }
    }
    private void Move()
    {
        if (!flag)
            CheckPlayerInput();
        if(flag)
        {
            transform.position = Vector2.MoveTowards(transform.position, FindWaypoint(currentLevel), _movementSpeed * Time.deltaTime);
            if (transform.position == FindWaypoint(currentLevel))
            {
                flag = false;
                animator.SetBool(currAnim, false);
                _runtimeData._currentLevel = currentLevel;
                AudioManager.AudioInstance.PlaySound("Select Level");
            }
        }
    }
    private Vector3 FindWaypoint(string level)
    {
        foreach(Transform pos in waypoints)
        {
            if (pos.name.Equals(level))
                return pos.position;
        }
        //assuming level is not found, return home
        return waypoints[0].position;
    }
}
