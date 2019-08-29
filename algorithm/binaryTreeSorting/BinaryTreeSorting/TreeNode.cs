
using System.Globalization;
using System.Runtime.CompilerServices;
using System;

namespace BinaryTreeSorting
{
    public class TreeNode<T>
    {
        public T Data { get; set; }

        public TreeNode<T> LeftChildNode { get; set; }

        public TreeNode<T> RightChildNode { get; set; }

        public TreeNode()
        {
            this.Data = default(T);
            LeftChildNode = null;
            RightChildNode = null;
        }

        public TreeNode(T data)
        {
            this.Data = data;
            LeftChildNode = null;
            RightChildNode = null;
        }

        public TreeNode(TreeNode<T> left, TreeNode<T> right)
        {
            this.Data = default(T);
            this.LeftChildNode = left;
            this.RightChildNode = right;
        }

        public TreeNode(T data, TreeNode<T> left, TreeNode<T> right)
        {
            this.Data = data;
            this.LeftChildNode = left;
            this.RightChildNode = right;
        }

        public virtual void CreateTree(TreeNode<T> node)
        {

        }
    }
}