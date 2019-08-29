using System;

namespace BinaryTreeSorting
{
    class Program
    {
        static void Main(string[] args)
        {
            TreeNode<int> e = new TreeNode<int>(1);
            TreeNode<int> g = new TreeNode<int>(2);
            TreeNode<int> h = new TreeNode<int>(3);
            TreeNode<int> i = new TreeNode<int>(4);
            TreeNode<int> d = new TreeNode<int>(5, null, g);
            TreeNode<int> f = new TreeNode<int>(6, h, i);
            TreeNode<int> b = new TreeNode<int>(7, d, e);
            TreeNode<int> c = new TreeNode<int>(8, f, null);
            TreeNode<int> root = new TreeNode<int>(9, b, c);

            Console.Write("开始先序遍历：");
            BinaryTreeInt.PreOrderRecur(root);
            Console.WriteLine("");
            Console.Write("开始中序遍历：");
            BinaryTreeInt.InOrderRecur(root);
            Console.WriteLine("");
            Console.Write("开始后序遍历：");
            BinaryTreeInt.LastOrderRecur(root);
            Console.WriteLine("");
            Console.Write("开始层序遍历：");
            BinaryTreeInt.LayerOrderRecur(root);
        }
    }
}
