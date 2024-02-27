using System;




//1.After a Split, compress the path back to the root to ensure that binary tree is full, i.e. each non-leaf
//node has two non - empty children(4 marks).

//2. Combine left and right siblings into one node whose total string length is 5 or less(4 marks).

//node class
public class Node {
    public string stringCharacters { get; set; }
    public int Length { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }

    // Node constructor
    public Node(string item) {
        stringCharacters = item;
        Length = 0;
        Left = Right = null;
    }
}

public class Rope {
    const int maxLeafStringLength = 10; //Final length
    //const int maxLeafStringLength = 3;   //Debugging
    Node root = null;

    ///////////////////////Public methods////////////////////////////////
    //Create a balanced rope from a given string S(5 marks).
    public Rope(string S) {
        root = Build(S, 0, S.Length);

        PrintRope();
    }

    //Insert string S at index i(5 marks).
    public void Insert(string S, int i) {

    }

    //Delete the substring S[i, j](5 marks).
    public void Delete(int i, int j) {

    }

    //Return the substring S[i, j](6 marks).
    public string Substring(int i, int j) {
        return "-1";
    }

    //Return the index of the first occurrence of S; -1 otherwise(9 marks).
    public int Find(string S) {
        return -1;
    }

    //Return the character at index i(3 marks).
    public char CharAt(int i) {
        return 'z';
    }

    //Return the index of the first occurrence of character c(4 marks).
    public int IndexOf(char c) {
        return -1;
    }

    //Reverse the string represented by the current rope(5 marks).
    public void Reverse() {

    }

    //Return the length of the string (1 mark).
    public int Length() {
        return -1;
    }

    //Return the string represented by the current rope(4 marks).
    public string ToString() {
        return "-1";
    }

    //Print the augmented binary tree of the current rope(4 marks).
    public void PrintRope() {
        PrintRope(this.root, 0);
    }

    // Helper method to print the rope recursively
    private void PrintRope(Node node, int depth) {
        if (node == null)
            return;

        // Print the right subtree first, with increased indentation
        PrintRope(node.Right, depth + 1);

        // Print the current node: indent based on its depth
        Console.Write(new string(' ', depth * 4)); // 4 spaces per depth level
        Console.WriteLine($"{node.stringCharacters} ({node.Length})");

        // Print the left subtree with increased indentation
        PrintRope(node.Left, depth + 1);
    }

    ////////////////////////Private Methods////////////////////////////////

    //Recursively build a balanced rope for S[i, j] and return its root (part of the constructor).
    private Node Build(string s, int i, int j) {
        // Base case: when the substring length is less than or equal to maxLeafStringLength
        if ((j - i) <= maxLeafStringLength) {
            // Create a leaf node with the substring
            return new Node(s.Substring(i, j - i)) { Length = j - i };

        } else {
            // Calculate the midpoint to split the string
            int mid = i + (j - i) / 2;

            // Recursively build the left and right subtrees
            Node left = Build(s, i, mid);
            Node right = Build(s, mid, j);

            // Create a new parent node with the left and right children
            Node parent = new Node("") {
                Left = left,
                Right = right,
                // Set the length of the parent node as the sum of lengths of its children
                Length = left.Length + right.Length
            };
            return parent;
        }
    }

    //Return the root of the rope constructed by concatenating two ropes with roots p and q(3 marks).
    private Node Concatenate(Node p, Node q) {
        return new Node("-1");
    }

    //Split the rope with root p at index i and return the root of the right subtree (9 marks).
    private Node Split(Node p, int i) {
        return new Node("-1");
    }

    //Rebalance the rope using the algorithm found on pages 1319 - 1320 of Boehm et al. (9 marks).
    private Node Rebalance() {
        return new Node("-1");
    }
}




class Program {
    static void Main(string[] args) {
        //Console.WriteLine("Hello World!");

        //Rope myRope = new Rope("H");                  //Test one character
        //Rope myRope = new Rope("Hello Worl");           //Test 10 characters
        Rope myRope = new Rope("Hello World!");       //Test more than 10 characters
        //string myString = "A string, by definition, is a linear sequence of characters representing a word, sentence, or body of text. Not surprisingly, strings are an integral and convenient part of most high - level programming languages. In C#, strings are supported by the String and StringBuilder library classes which include standard methods to concatenate two strings, return a substring, find the character at a given index, find the index of a given character, among others. Intuitively, each string is implemented as a linear array of characters which for the most part works reasonably well.For methods that retrieve characters or substrings, the linear array is ideal.But as a length of the string grows considerably longer as in the case of text, methods that require a wholesale shift or copy of characters are slowed by their linear time complexity.The question then arises: Can we do better overall for very long strings ?";
        //Rope myRope = new Rope(myString);       //Test a paragraph
    }
}
