using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	public float speed = 0.2f;
	Vector2 destinationPosition = Vector2.zero;
	Vector2 currentDirection = Vector2.zero;
	Vector2 nextDirection = Vector2.zero;

	void Start()
	{
		destinationPosition = transform.position;
	}
		
	void FixedUpdate()
	{
		ReadInputAndMove();
		Animate();
	}

	void Animate()
	{
		//set the pacman animation as per direction
		Vector2 dir = destinationPosition - (Vector2)transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);
	}

	bool IsValidDirectionToMove(Vector2 direction)
	{
		
		// Ratcast a ray to check the valid diection to move for pacman
		Vector2 pos = transform.position;
		direction += new Vector2(direction.x * 0.45f, direction.y * 0.45f);
		RaycastHit2D hit = Physics2D.Linecast(pos + direction, pos);
		return hit.collider.name == "CollectibleDot" || (hit.collider == GetComponent<Collider2D>());
	}

	public void ResetDestinationPosition()
	{
		destinationPosition = new Vector2(15f, 11f);
		GetComponent<Animator>().SetFloat("DirX", 1);
		GetComponent<Animator>().SetFloat("DirY", 0);
	}

	void ReadInputAndMove()
	{
		// moving to the destinationposition
		Vector2 p = Vector2.MoveTowards(transform.position, destinationPosition, speed);
		GetComponent<Rigidbody2D>().MovePosition(p);

		if (Input.GetAxis("Horizontal") > 0) 
			nextDirection = Vector2.right;
		if (Input.GetAxis("Horizontal") < 0) 
			nextDirection = -Vector2.right;
		if (Input.GetAxis("Vertical") > 0) 
			nextDirection = Vector2.up;
		if (Input.GetAxis("Vertical") < 0) 
			nextDirection = -Vector2.up;

		// when pacman is at center of current tile.
		if (Vector2.Distance(destinationPosition, transform.position) < 0.00001f)
		{
			if (IsValidDirectionToMove(nextDirection))
			{
				destinationPosition = (Vector2)transform.position + nextDirection;
				currentDirection = nextDirection;
			}
			else
			{
				if (IsValidDirectionToMove(currentDirection)) 
					destinationPosition = (Vector2)transform.position + currentDirection;
			}
		}
	}
}
