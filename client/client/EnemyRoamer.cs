using UnityEngine;
using System.Collections;


public class RoamerEnemyData : BaseEnemyData
{
    public Direction direction;
}

public class EnemyRoamer : EnemyBase {

    private RoamerEnemyData _roamerData;
    private Direction _targetDirection;
    private Direction _direction;
    private Direction _direccionAnterior;
    private State _state =State.Falling;
    private bool[] _arrayBlocks = new bool[8];
    private int i_anterior = 0;

    //Rays
    private Ray2D _downRay;
    private Ray2D _downRightRay;
    private Ray2D _rightRay;
    private Ray2D _rightUpRay;
    private Ray2D _upRay;
    private Ray2D _upLeftRay;
    private Ray2D _leftRay;
    private Ray2D _leftDownRay;
    

    //Accel
    private const float _minAccSpeed = 1f;
    private float _accSpeed = _minAccSpeed;
    private const float _incrSpeed = 0.1f;

	// Use this for initialization
	void Start () {
        base.Start();

        _downRay = new Ray2D();
        _downRay.direction = Vector2.down;

        _downRightRay = new Ray2D();
        _downRightRay.direction = new Vector2(1f, -1f);

        _rightRay = new Ray2D();
        _rightRay.direction = Vector2.right;

        _rightUpRay = new Ray2D();
        _rightUpRay.direction = new Vector2(1f, 1f);

        _upRay = new Ray2D();
        _upRay.direction = Vector2.up;

        _upLeftRay = new Ray2D();
        _upLeftRay.direction = new Vector2(-1f, 1f);

        _leftRay = new Ray2D();
        _leftRay.direction = Vector2.left;

        _leftDownRay = new Ray2D();
        _leftDownRay.direction = new Vector2(-1f, -1f);

        
	}
	
	// Update is called once per frame
	void Update () {
        CheckPlayerCollisions();
        CheckBlockCollisions();
        AdjustDirectionAndState();
        AdjustVelocity();
        MovingEnemy();
	}

    private void CheckBlockCollisions()
    {
        //Set the origins ofthe rays
        _downRay.origin = transform.position + new Vector3(0f, -0.61f, 0f);
        _downRightRay.origin = transform.position + new Vector3(0.61f, -0.61f, 0f);

        _rightRay.origin = transform.position + new Vector3(0.61f, 0f, 0f);
        _rightUpRay.origin = transform.position + new Vector3(0.61f, 0.61f, 0f);

        _upRay.origin = transform.position + new Vector3(0f, 0.61f, 0f);
        _upLeftRay.origin = transform.position + new Vector3(-0.61f, 0.61f, 0f);

        _leftRay.origin = transform.position + new Vector3(-0.61f, 0f, 0f);
        _leftDownRay.origin = transform.position + new Vector3(-0.61f, -0.61f, 0f);

        //Do the raycasts
        RaycastHit2D hit_down = Physics2D.Raycast(_downRay.origin, _downRay.direction, 0.1f);
        RaycastHit2D hit_downRight = Physics2D.Raycast(_downRightRay.origin, _downRightRay.direction, 0.1f);

        RaycastHit2D hit_right = Physics2D.Raycast(_rightRay.origin, _rightRay.direction, 0.1f);
        RaycastHit2D hit_rightUp = Physics2D.Raycast(_rightUpRay.origin, _rightUpRay.direction, 0.1f);

        RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.1f);
        RaycastHit2D hit_upLeft = Physics2D.Raycast(_upLeftRay.origin, _upLeftRay.direction, 0.1f);

        RaycastHit2D hit_left = Physics2D.Raycast(_leftRay.origin, _leftRay.direction, 0.1f);
        RaycastHit2D hit_leftDown = Physics2D.Raycast(_leftDownRay.origin, _leftDownRay.direction, 0.1f);

        if (hit_down.collider != null)
        {
            Debug.Log(hit_down.collider.gameObject.name);
        }

        //Check the blocks the enemy is colliding with
        //    5   4   3
        //    6   -   2
        //    7   0   1
        //Down -> 0
        if (hit_down.collider != null && hit_down.collider.gameObject.GetComponent<Block>() != null)
        {
            //transform.position = new Vector3(transform.position.x, hit_down.transform.position.y + 1,0);
            _arrayBlocks[0] = true;
            //Debug.Log("Down hit");
        }
        else
        {
            _arrayBlocks[0] = false;
            //Debug.Log("Down no-hit");
        }
        //Down-Right -> 1
        if (hit_downRight.collider != null && hit_downRight.collider.gameObject.GetComponent<Block>() != null)
        {
            
            _arrayBlocks[1] = true;
            //Debug.Log("Down-Right hit");
        }
        else
        {
            _arrayBlocks[1] = false;
            //Debug.Log("Down-Right no-hit");
        }
        //Right -> 2
        if (hit_right.collider != null && hit_right.collider.gameObject.GetComponent<Block>() != null)
        {
            //transform.position = new Vector3(hit_right.transform.position.x - 1, transform.position.y, 0);
            _arrayBlocks[2] = true;
            //Debug.Log("Right hit");
        }
        else
        {
            _arrayBlocks[2] = false;
            //Debug.Log("Right no-hit");
        }
        //Right-Up -> 3
        if (hit_rightUp.collider != null && hit_rightUp.collider.gameObject.GetComponent<Block>() != null)
        {
            _arrayBlocks[3] = true;
            //Debug.Log("Right-Up hit");
        }
        else
        {
            _arrayBlocks[3] = false;
            //Debug.Log("Right-Up no-hit");
        }
        //Up -> 4
        if (hit_up.collider != null && hit_up.collider.gameObject.GetComponent<Block>() != null)
        {
            //transform.position = new Vector3(transform.position.x, hit_up.transform.position.y - 1, 0);
            _arrayBlocks[4] = true;
            //Debug.Log("Up hit");
        }
        else
        {
            _arrayBlocks[4] = false;
            //Debug.Log("Up no-hit");
        }
        //Up-Left -> 5
        if (hit_upLeft.collider != null && hit_upLeft.collider.gameObject.GetComponent<Block>() != null)
        {
            _arrayBlocks[5] = true;
            //Debug.Log("Up-Left hit");
        }
        else
        {
            _arrayBlocks[5] = false;
            //Debug.Log("Up-Left no-hit");
        }
        //Left -> 6
        if (hit_left.collider != null && hit_left.collider.gameObject.GetComponent<Block>() != null)
        {
            //transform.position = new Vector3(hit_left.transform.position.x+1, transform.position.y, 0);
            _arrayBlocks[6] = true;
            //Debug.Log("Left hit");
        }
        else
        {
            _arrayBlocks[6] = false;
            //Debug.Log("Left no-hit");
        }
        //Left-Down -> 7
        if (hit_leftDown.collider != null && hit_leftDown.collider.gameObject.GetComponent<Block>() != null)
        {
            _arrayBlocks[7] = true;
            //Debug.Log("Left-Down hit");
        }
        else
        {
            _arrayBlocks[7] = false;
            //Debug.Log("Left-Down no-hit");
        }


    }

    private void AdjustDirectionAndState()
    {
        bool inTheWall = true;

        switch(_targetDirection)
        {
            //Clockwise
            case Direction.Right:
                //Get the position of the zero after the last one
                int i_modulada;
                for (int i = 0; i < 8; i++)
                {

                    inTheWall = true;
                    i_modulada = (i + i_anterior) % 8;
                    if (_arrayBlocks[i_modulada])
                    {
                        if ((i_modulada == 7 && !_arrayBlocks[0]) || ( i_modulada < 7 && !_arrayBlocks[i_modulada + 1]))
                        {
                            //This is the position of the zero after the last one
                            int zeroPosition = i_modulada + 1;
                            if (i_modulada == 7)
                            {
                                zeroPosition = 0;
                            }
                            //Change direction
                            switch (zeroPosition)
                            {
                                case 0:
                                case 7:
                                    Debug.Log("Zero in position 0 or 7 -> Down");
                                    if (_direction != Direction.Down)
                                        _direccionAnterior = _direction;
                                    _direction = Direction.Down;
                                    if (_arrayBlocks[6])
                                    {
                                        _direccionAnterior = Direction.Down;
                                    }
                                    break;

                                case 6:
                                case 5:
                                    Debug.Log("Zero in position 5 or 6 -> Left");
                                    if (_direction != Direction.Left)
                                        _direccionAnterior = _direction;
                                    _direction = Direction.Left;
                                    if (_arrayBlocks[4])
                                    {
                                        _direccionAnterior = Direction.Left;
                                    }
                                    
                                    break;

                                case 4:
                                case 3:
                                    Debug.Log("Zero in position 3 or 4 -> Up");
                                    if(_direction != Direction.Up)
                                        _direccionAnterior = _direction;
                                    _direction = Direction.Up;
                                    if (_arrayBlocks[2])
                                    {
                                        _direccionAnterior = Direction.Up;
                                    }
                                    break;

                                case 2:
                                case 1:
                                    Debug.Log("Zero in position 1 or 2 -> Right");
                                    if (_direction != Direction.Right)
                                        _direccionAnterior = _direction;
                                    _direction = Direction.Right;
                                    if (_arrayBlocks[0])
                                    {
                                        _direccionAnterior = Direction.Right;
                                    }
                                    break;
                            }

                            Debug.Log("Direccion anterior = " + _direccionAnterior.ToString());
                            Debug.Log("i anterior = " + i_anterior.ToString());
                            
                            switch (_direccionAnterior)
                            {
                                case Direction.Up:
                                    i_anterior = 1;
                                    break;

                                case Direction.Right:
                                    i_anterior = 7;
                                    break;

                                case Direction.Down:
                                    i_anterior = 5;
                                    break;

                                case Direction.Left:
                                    i_anterior = 3;
                                    break;

                            }

                            break;
                        }
                    }
                    else
                    {
                        inTheWall = false;
                        Debug.Log("inTheWall = false on iteration: " + i.ToString());
                    }
                }

                break;

            //Counter-Clockwise
            case Direction.Left:

                //

                break;
        }

        Debug.Log(ArrayBoolToString(_arrayBlocks));

        if (inTheWall)
        {
            _state = State.Grounded;
            Debug.Log("Grounded");
        }
        else
        {
            _state = State.Falling;
            Debug.Log("Falling");
        }

    }

    private void AdjustVelocity()
    {
        switch (_state)
        {
            case State.Falling:

                _velocity.y = -_speed;
                _accSpeed += _incrSpeed;
                _velocity.x = 0;

                break;

            case State.Grounded:

                switch (_direction)
                {
                    case Direction.Down:
                        _velocity.x = 0f;
                        _velocity.y = -_speed;
                        break;

                    case Direction.Up:
                        _velocity.x = 0f;
                        _velocity.y = _speed;
                        break;

                    case Direction.Right:
                        _velocity.x = _speed;
                        _velocity.y = 0f;
                        break;

                    case Direction.Left:
                        _velocity.x = -_speed;
                        _velocity.y = 0f;
                        break;

                }

                break;
        }
    }

    private void MovingEnemy()
    {
        Vector3 p = transform.position;

        switch (_state)
        {
            case State.Grounded:
                switch (_direction)
                {
                    case Direction.Down:
                    case Direction.Up:
                        p.x = Mathf.Round(transform.position.x);
                        p.y += Time.deltaTime * _velocity.y;
                        break;

                    case Direction.Right:
                    case Direction.Left:
                        p.x += Time.deltaTime * _velocity.x;
                        p.y = Mathf.Round(transform.position.y);
                        break;
                }
                break;

            case State.Falling:
                p.y += Time.deltaTime * _velocity.y * _accSpeed;

                break;
        }

        transform.position = p;
    }

    public override void SetEnemyData(BaseEnemyData data)
    {
        // Start moving towards P0
        _roamerData = (RoamerEnemyData)data;
        _targetDirection = _roamerData.direction;
        _direction = _roamerData.direction;
        _direccionAnterior = _direction;
    }

    public string ArrayBoolToString(bool[] array)
    {
        string res = "";
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i])
            {
                res += "1";
            }
            else
            {
                res += "0";
            }
        }

        return res;
    }
}
