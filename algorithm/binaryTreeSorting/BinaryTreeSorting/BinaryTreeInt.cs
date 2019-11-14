// https://www.cnblogs.com/songwenjie/p/8955856.html

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata.Ecma335;
namespace BinaryTreeSorting
{
    public class BinaryTreeInt
    {

        public TreeNode<int> Root;
        public TreeNode<int> Insert(int value)
        {
            TreeNode<int> current = Root;
            if (current == null)
            {
                Root = new TreeNode<int>(value);
                return Root;
            }
            while (current != null)
            {
                if (value < current.Data)
                {
                    if (current.LeftChildNode == null)
                    {
                        return current.LeftChildNode = new TreeNode<int>(value);
                    }
                    else
                    {
                        current = current.LeftChildNode;
                    }
                }
                else if (value > current.Data)
                {
                    if (current.RightChildNode == null)
                    {
                        return current.RightChildNode = new TreeNode<int>(value);
                    }
                    else
                    {
                        return current = current.LeftChildNode;
                    }
                }
            }
            return current;
        }
        public static void PreOrderRecur(TreeNode<int> treeNode)
        {
            if (treeNode == null)
            {
                return;
            }
            Console.Write(treeNode.Data + " ");
            PreOrderRecur(treeNode.LeftChildNode);
            PreOrderRecur(treeNode.RightChildNode);
        }

        public static void InOrderRecur(TreeNode<int> treeNode)
        {
            if (treeNode != null)
            {
                InOrderRecur(treeNode.LeftChildNode);
                Console.Write(treeNode.Data + " ");
                InOrderRecur(treeNode.RightChildNode);
            }
        }

        public static void LastOrderRecur(TreeNode<int> treeNode)
        {
            if (treeNode != null)
            {
                LastOrderRecur(treeNode.LeftChildNode);
                LastOrderRecur(treeNode.RightChildNode);
                Console.Write(treeNode.Data + " ");
            }
        }

        public static void LayerOrderRecur(TreeNode<int> treeNode)
        {
            if (treeNode == null)
            {
                return;
            }
            Queue<TreeNode<int>> queue = new Queue<TreeNode<int>>();
            queue.Enqueue(treeNode);

            while (queue.Any())
            {
                var node = queue.Dequeue();
                Console.Write(node.Data + " ");

                if(node.LeftChildNode != null){
                    queue.Enqueue(node.LeftChildNode);
                }
                if(node.RightChildNode != null){
                    queue.Enqueue(node.RightChildNode);
                }
            }
        }
    }
}