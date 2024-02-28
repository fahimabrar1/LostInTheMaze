using System;

[Serializable]
public class NodeData
{
    public BasicNode node;
    public NodeDirectionEnum nodeDirection;
}


[Serializable]
public class NodeContainer
{
    public NodeContainer(Node node, NodeDirectionEnum nodeDirection)
    {
        this.node = node;
        this.nodeDirection = nodeDirection;
    }

    public Node node;
    public NodeDirectionEnum nodeDirection;
}