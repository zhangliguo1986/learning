using System;

namespace BinaryTreeSorting
{
    public class Node
    {
        public int value;
        public Node left;
        public Node right;

        public Node() { }

        public Node(int value)
        {
            this.value = value;
        }
        public Node(int value, Node left, Node right)
        {
            this.value = value;
            this.left = left;
            this.right = right;
        }
    }
}
