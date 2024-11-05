using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Track : MonoBehaviour
{
    // Array of Nodes that the enemy will follow
    public Transform[] Nodes;

    // Unity editor method to visually draw the path between nodes in the scene view
    private void OnDrawGizmos()
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            //destinations of Enemies
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Nodes[i].position, 0.3f);

            if (i > 0)
            {
                //draw the lines between the previous node and current node to indicate enemy path direction
                Gizmos.DrawLine(Nodes[i - 1].position, Nodes[i].position);
            }

        }
    }

    // Method to retrieve the next position of a node based on the given index
    public Vector2 getNextPosition(int index)
    {
        return Nodes[index].position;
    }

    // Method to calculate the total distance between the current tower's position and the remaining nodes in the path
    public float getTotalDistance(int currentTower, Vector2 position)
    {
        float distance = Vector2.Distance(position, Nodes[currentTower].position);
        for (int i = currentTower; i < Nodes.Length - 1; i++)
        {
            distance += Vector2.Distance(Nodes[i].position, Nodes[i + 1].position);
        }
        return distance;
    }


}
