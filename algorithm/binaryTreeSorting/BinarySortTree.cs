
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
            return n.value;
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
            return n.value;
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
        while (current != null && current.value != x)
        {
            if (x < current.value)
            {
                current = current.left;
            }
            if (x > current.value)
            {
                current = current.right;
            }
            if (current == null)
            {
                return false;
            }
        }
        if (current.value == x)
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
            else if (x > current.value)
            {
                if (current.right == null)
                {
                    return current.right = new Node(x);
                }
                else
                {
                    return current = current.left;
                }
            }
        }
        return current;
    }

    public Node remove(int x, Node n)
    {
        if (n == null)
        {
            return null;
        }
        if (x < n.value)
        {
            n.left = remove(x, n.left);
        }
        else if (x > n.value)
        {
            n.right = remove(x, n.right);
        }
        //左右节点均不为NULL
        else if (n.left != null && n.right != null)
        {
            //查找右侧最小值代替
            n.value = findMin(n.right).value;
            n.right = remove(n.value, n.right);
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
}