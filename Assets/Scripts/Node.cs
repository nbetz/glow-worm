using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: System.IEquatable<Node>
{
    public float f = 0f;
    public float g = 0f;
    public float h = 0f;
    public Node parent;
    public Vector3 pos;

    public Node(Vector3 position)
    {
        parent = null;
        pos = position;
    }

    public Node(Node parentNode, Vector3 position)
    {
        parent = parentNode;
        pos = position;
    }

    public bool Equals(Node other)
    {
        return pos == other.pos;
    }
}
