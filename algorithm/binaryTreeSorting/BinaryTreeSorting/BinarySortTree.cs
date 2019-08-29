using System;

namespace BinaryTreeSorting
{
    public class BinarySortTree
    {
        public Node root;

        public BinarySortTree()
        {
            root = null;
        }

        public void Clear()
        {
            root = null;
        }

        public bool IsEntity()
        {
            return root == null;
        }

        public Node findMin(Node n)
        {
            if (n == null)
            {
                return null;
            }
            if (n.left == null)
            {
                return n;
            }
            else
            {
                return findMin(n.left);
            }
        }

        public Node findMax(Node n)
        {
            if (n == null)
            {
                return null;
            }
            if (n.right == null)
            {
                return n;
            }
            else
            {
                return findMax(n.right);
            }
        }

        public bool IsContains(int value)
        {
            Node current = root;
            if (current == null)
            {
                return false;
            }
            while (current != null && current.value != value)
            {
                if (value < current.value)
                {
                    current = current.left;
                }
                if (value > current.value)
                {
                    current = current.right;
                }
                if (current == null)
                {
                    return false;
                }
            }
            if (current.value == value)
            {
                return true;
            }
            return false;
        }

        public Node Insert(int value)
        {
            Node current = root;
            if (current == null)
            {
                root = new Node(value);
                return root;
            }
            while (current != null)
            {
                if (value < current.value)
                {
                    if (current.left == null)
                    {
                        return current.left = new Node(value);
                    }
                    else
                    {
                        current = current.left;
                    }
                }
                else if (value > current.value)
                {
                    if (current.right == null)
                    {
                        return current.right = new Node(value);
                    }
                    else
                    {
                        return current = current.left;
                    }
                }
            }
            return current;
        }

        public Node Remove(int value, Node n)
        {
            if (n == null)
            {
                return null;
            }
            if (value < n.value)
            {
                n.left = Remove(value, n.left);
            }
            else if (value > n.value)
            {
                n.right = Remove(value, n.right);
            }
            //左右节点均不为NULL
            else if (n.left != null && n.right != null)
            {
                //查找右侧最小值代替
                n.value = findMin(n.right).value;
                n.right = Remove(n.value, n.right);
            }
            else
            {
                //左右单空或者左右都NULL
                if (n.left == null && n.right == null)
                {
                    n = null;
                }
                else if (n.right != null)
                {
                    n = n.right;
                }
                else if (n.left != null)
                {
                    n = n.left;
                }
                return n;
            }
            return n;
        }

        public  void Print(Node node)
        {
            if (node == null)
            {
                Console.WriteLine("Node 为 null");
            }
            Console.WriteLine(node.value);
            Print(node.left);
            Print(node.right);
        }
    }
}