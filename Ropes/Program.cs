using System;
using System.Reflection;
using System.Reflection.PortableExecutable;

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
    //Create a balanced rope from a given string S(5 marks). DONE
    public Rope(string S) {
        root = Build(S, 0, S.Length);

        //Console.WriteLine("");
        //PrintRope();
        //Console.WriteLine("");
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

    //Return the character at index i(3 marks). DONE
    public char CharAt(int i) {
        var current = root;
        bool searching = true;

        //If the index is outside of the Rope's length
        if ((i > root.Length) || (i < 0)) {
            return '\0';
        }

        //Search for the index
        while (searching) {
            //if we are at an internal node
            if (current.Left != null) {
                if (i < current.Left.Length) {
                    //Go left
                    current = current.Left;
                    //Console.WriteLine("Moved left");
                } else {
                    //Go right
                    i = i - current.Left.Length;
                    current = current.Right;
                    //Console.WriteLine("Moved right");
                }
            }
            //else, we are at a leaf node where the index should
            else {
                return current.stringCharacters[i];
            }
        }
        //Didn't find the index, shouldn't ever get here
        return '\0';
    }

    //Return the index of the first occurrence of character c (4 marks). DONE
    public int IndexOf(char c) {
        //Recursive search algorithm for the IndexOf() function
        (int, int) IndexOfSearch(char c, Node current, int foundIndex, int searchingIndex) {
            //Explore the left subtrees first, going left-to-right along the leaf nodes

            //If the left subtree is not empty and char c's index has not been found
            if (current.Left != null && foundIndex == -1) {
                //Explore the left subtree
                (searchingIndex, foundIndex) = IndexOfSearch(c, current.Left, foundIndex, searchingIndex);
            }

            //If the right subtree is not empty and char c's index has not been found
            if (current.Right != null && foundIndex == -1) {
                //Explore the right subtree
                (searchingIndex, foundIndex) = IndexOfSearch(c, current.Right, foundIndex, searchingIndex);
            }

            //if at a leaf node (both children are null) and char c's index has not been found
            if (((current.Left == null) && (current.Right == null)) && foundIndex == -1) {
                //Check all characters in the string at the current leaf node
                for (int i = 0; i < current.stringCharacters.Length; i++) {
                    if (c == current.stringCharacters[i]) {
                        //Found first occurence of the character
                        searchingIndex = searchingIndex + i;
                        //Console.WriteLine("Found character " + c + " at index: " + searchingIndex);
                        foundIndex = searchingIndex;

                        return (searchingIndex, foundIndex);
                    }
                }
            }

            //Determine what to send back going up the recursive stack
            if (foundIndex != -1) {
                //Send back the found index
                return (searchingIndex, foundIndex);
            } else {
                //If the character wasn't found here then return the sum of the length of trees of the parent
                searchingIndex = searchingIndex + current.stringCharacters.Length;
                return (searchingIndex, -1);
            }
        }

        int foundIndex = -1;
        int searchingIndex = 0;
        (searchingIndex, foundIndex) = IndexOfSearch(c, root, foundIndex, searchingIndex);

        return foundIndex;
    }

    //Reverse the string represented by the current rope(5 marks). DONE
    public void Reverse() {
        static string ReverseString(string s) {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        void ReverseTraversal(Node current) {
            if (current != null) {
                //Check if current has a left and right subtree
                if ((current.Left != null && current.Right != null)) {
                    //Swap them (left becomes right and vice versa)
                    Node temp = current.Left;
                    current.Left = current.Right;
                    current.Right = temp;

                    //if the swapped nodes are leaf nodes
                    if (current.Left.Left == null) {
                        //Reverse thier strings
                        current.Left.stringCharacters = ReverseString(current.Left.stringCharacters);
                        current.Right.stringCharacters = ReverseString(current.Right.stringCharacters);
                    }
                }

                //Special Case: Only the root node exists
                if (current == root) {
                    //Reverse the roots string
                    current.stringCharacters = ReverseString(current.stringCharacters);
                }

                //Move down the tree
                if (current.Left != null) {
                    ReverseTraversal(current.Left);
                }
                if (current.Right != null) {
                    ReverseTraversal(current.Right);
                }
            }
        }


        ReverseTraversal(root);
    }

    //Return the length of the string (1 mark). DONE
    public int Length() {
        if (root == null) {
            return -1;
        }

        return root.Length;
    }

    //Return the string represented by the current rope (4 marks). DONE
    override public string ToString() {
        string ToStringPrivate(Node current) {
            string leftString = null;
            string rightString = null;
            if (current.Left != null) {
                //Go left
                //Console.WriteLine("left");
                leftString = ToStringPrivate(current.Left);
            }
            if (current.Right != null) {
                //Go right
                //Console.WriteLine("right");
                rightString = ToStringPrivate(current.Right);
            }
            if ((current.Left == null) && (current.Right == null)) {
                //At a leaf node
                //Console.WriteLine("LEaf");
                //Console.WriteLine(current.stringCharacters);
                return current.stringCharacters;
            }

            string finalString = leftString + rightString;
            //Console.WriteLine(finalString);
            return finalString;
        }

        //PrintRope(root, 0);
        return ToStringPrivate(root);
    }

    //Print the augmented binary tree of the current rope (4 marks). DONE
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

    //Tests the split() method b/c it's private
    public void TestSplit(int index) {
        Console.WriteLine("------------------------------");
        Console.WriteLine("");
        Console.WriteLine("Index to split is: " + index);
        Console.WriteLine("Before split");
        PrintRope();

        root = Split(root, index);

        Console.WriteLine("");
        Console.WriteLine("After split");
        PrintRope();
    }

    ////////////////////////Private Methods////////////////////////////////

    //Recursively build a balanced rope for S[i, j] and return its root (part of the constructor). DONE
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

    //Return the root of the rope constructed by concatenating two ropes with roots p and q (3 marks). DONE
    private Node Concatenate(Node p, Node q) {
        Node newRoot = new Node("");

        //Connect the two subtrees (p and q) to newRoot
        newRoot.Left = p;
        newRoot.Right = q;
        newRoot.Length = p.Length + q.Length;

        return newRoot;
    }

    //Rebalance the rope using the algorithm found on pages 1319 - 1320 of Boehm et al. (9 marks).
    private Node Rebalance() {
        return new Node("-1");
    }

    //Time complexity for figuring out this method O(too got damn long x2) 
    //Split the rope with root p at index i and return the root of the right subtree (9 marks). DONE
    private Node Split(Node p, int i) {
        //Split a node's string into 2 parts, creates 2 new nodes to hold the left and right piece, then update p to refer to the new nodes
        Node SplitNode(Node p, int i) {
            string leftString = p.stringCharacters.Substring(0, i + 1);
            string rightString = p.stringCharacters.Substring(i + 1);

            //Create new subtrees to hold the new strings
            Node leftChild = new Node(leftString);
            leftChild.Length = leftString.Length;

            Node rightChild = new Node(rightString);
            rightChild.Length = rightString.Length;

            //link the new nodes to p
            p.Left = leftChild;
            p.Right = rightChild;
            p.Length = p.Left.Length + p.Right.Length;
            p.stringCharacters = "";

            return p;
        }

        //Handles building the right subtree going back up the recursive calls for the internal nodes
        Node SplitUp(Node p, Node previousRightTree, int i) {
            Node rightTree = previousRightTree;

            //is p (current node) on the right side of the split
            if (i == 0) {
                //Right of split
                rightTree = p;
                return rightTree;
            } 
            else {
                //Left of split
                //is p.Right on the right or left side of the split
                if (i <= ((p.Length - 1) - p.Right.Length)) {
                    //Right of split
                    //Console.WriteLine("rights");

                    //is my right child different from what is currently in the right tree node
                    //This might need to be changed, the comparison
                    if (p.Right != rightTree) {
                        //yes, concatenate the rightTree and p.Right's child to form a new rightTree,
                        rightTree = Concatenate(rightTree, p.Right);
                        return rightTree;
                    } 
                    else {
                        //no, do nothing
                        return rightTree;
                    }
                }   
                else {
                    //Left of split
                    //just pass along what is already in the right tree
                    //Console.WriteLine("leftss");
                    return rightTree;
                }
            }
        }

        //Console.WriteLine("current node: " + p.Length);
        //Console.WriteLine("current index: " + i);
        Node rightSubtree = new Node("");
        
        if (p == null) {
            //No split
            return p;
        }

        if (i < 0 || i > root.Length) {
            //index out of range
            return p;
        }

        //Navigate down the tree 
        if (p.Left == null && p.Right == null) {
            //Found the node that contains our index
            //Console.WriteLine("At a leaf: " + p.stringCharacters);

            //Determine if a split is performed on this node
            if ((i != 0) && (i != p.Length - 1)) {
                //Console.WriteLine("Split");
                //Case 1: i refers to a char somewhere in the middle of the string
                p = SplitNode(p, i);

                //But we just want the node right of the split so
                p = p.Right;
            } 
            else {
                //Case 2: i refers to the left or right most char in the string
                //Console.WriteLine("Don't split");

                //Determine what to send back
                if (i == 0) {
                    //Console.WriteLine("left of split");
                    //do nothing
                } 
                else {
                    //Console.WriteLine("right of split");
                    return new Node("");
                }
            }
            //Console.WriteLine("leaf node");
            //PrintRope(p, 0);
            return p;
        } 
        else if (i < p.Left.Length) {
            //Go Left
            //Console.WriteLine("Go left down");
            rightSubtree = Split(p.Left, i);
            
            //Going up
            //Console.WriteLine("Going up");
            //Console.WriteLine("index: " + i);
            rightSubtree = SplitUp(p, rightSubtree, i);
        } 
        else if (i >= p.Left.Length) {
            //Go right
            i = i - p.Left.Length;
            //Console.WriteLine("Go right down");
            rightSubtree = Split(p.Right, i);

            //Going up
            //Console.WriteLine("Going up");
            i = i + p.Left.Length;
            //Console.WriteLine("index: " + i);
            rightSubtree = SplitUp(p, rightSubtree, i);
        }
        return rightSubtree;
    }
}




class Program {
    static void Main(string[] args) {
        //Console.WriteLine("Hello World!");

        //Rope myRope = new Rope("H");                  //Test one character
        //Rope myRope = new Rope("Hello Worl");         //Test 10 characters
        //Rope myRope = new Rope("abcdefghijklomnpqrstuvwxyz ABCDEFGa");         //Test more than 10 characters
        /*string myString = "A string, by definition, is a linear sequence of characters representing a word, sentence, or body of text. Not surprisingly, strings are an integral and convenient part of most high - level programming languages. In C#, strings are supported by the String and StringBuilder library classes which include standard methods to concatenate two strings, return a substring, find the character at a given index, find the index of a given character, among others. Intuitively, each string is implemented as a linear array of characters which for the most part works reasonably well.For methods that retrieve characters or substrings, the linear array is ideal.But as a length of the string grows considerably longer as in the case of text, methods that require a wholesale shift or copy of characters are slowed by their linear time complexity.The question then arises: Can we do better overall for very long strings ?";
        Rope myRope = new Rope(myString);             //Test a paragraph

        Console.WriteLine(myRope.Length());

        Console.WriteLine(myRope.CharAt(-1));           //Test invalid index
        Console.WriteLine(myRope.CharAt(999));          //Test invalid index
        Console.WriteLine(myRope.CharAt(0));            //Test going left
        Console.WriteLine(myRope.CharAt(33));           //Test going right
        Console.WriteLine(myRope.CharAt(9));            //Test going left then right
        Console.WriteLine(myRope.CharAt(17));           //Test going right then left

        Console.WriteLine("location of c at index: " + myRope.IndexOf('c'));         //Test valid character
        Console.WriteLine("location of k at index: " + myRope.IndexOf('k'));         //Test valid character
        Console.WriteLine("location of y at index: " + myRope.IndexOf('y'));         //Test valid character
        Console.WriteLine("location of _ at index: " + myRope.IndexOf(' '));         //Test valid character
        Console.WriteLine("location of P at index: " + myRope.IndexOf('P'));         //Test invalid character
        Console.WriteLine("location of a at index: " + myRope.IndexOf('a'));         //Test valid character with two chars in array


        Rope myReverseRope = new Rope("abcdefg");                       //Testing Reverse() with only the root node       
        myReverseRope.Reverse();
        myReverseRope.PrintRope();

        myReverseRope = new Rope("Hello World! Goodbye World!");        //Testing Reverse() with multiple leaf nodes     
        myReverseRope.Reverse();
        myReverseRope.PrintRope();
        Console.WriteLine("");
        */

        /*
        //Testing split on a normal rope
        Rope mySplitRope = new Rope("abcdefghijklmnop");          
        mySplitRope.TestSplit(0);       //Test Left, right side split

        mySplitRope = new Rope("abcdefghijklmnop");
        mySplitRope.TestSplit(7);       //Test Left, left side split

        mySplitRope = new Rope("abcdefghijklmnop");
        mySplitRope.TestSplit(8);       //Test right, right side split

        mySplitRope = new Rope("abcdefghijklmnop");
        mySplitRope.TestSplit(15);      //Test right, left side split

        mySplitRope = new Rope("abcdefghijklmnop");
        mySplitRope.TestSplit(3);       //Test left, split in the middle

        mySplitRope = new Rope("abcdefghijklmnop");
        mySplitRope.TestSplit(14);      //Test right, split in the middle

        mySplitRope = new Rope("abcdefghijklmnop");
        mySplitRope.TestSplit(-1);          //Test incorrect index
        mySplitRope.TestSplit(999);         //Test incorrect index

        //Testing split on a bigger Rope (say 30 ft. from your dungeoneering pouch)
        mySplitRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        mySplitRope.TestSplit(0);

        mySplitRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        mySplitRope.TestSplit(9);

        mySplitRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        mySplitRope.TestSplit(20);

        mySplitRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        mySplitRope.TestSplit(24);
        
        mySplitRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        mySplitRope.TestSplit(16);

        mySplitRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        mySplitRope.TestSplit(45);

        mySplitRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        mySplitRope.TestSplit(60);

        mySplitRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        mySplitRope.TestSplit(34);

        mySplitRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        mySplitRope.TestSplit(52);

        mySplitRope = new Rope("");
        mySplitRope.TestSplit(0);
        
        mySplitRope = new Rope("Hello ");
        mySplitRope.TestSplit(2);
        */


        //Testing ToString()
        /*
        Rope myRope = new Rope("");         //Test empty
        Console.WriteLine(myRope.ToString());

        myRope = new Rope("H");     //Test 1 char
        Console.WriteLine(myRope.ToString());

        myRope = new Rope("Hello World!"); //Test a sentence
        Console.WriteLine(myRope.ToString());

        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");  //Test a bigger Rope
        Console.WriteLine(myRope.ToString());

        myRope = new Rope("aaaaaaaaaabbbbbbbbbb"); //Test an evenly split rope
        Console.WriteLine(myRope.ToString());

        myRope = new Rope("aaaaaaaaaabbbbbbbbbbc"); //Test an unevenly split rope
        Console.WriteLine(myRope.ToString());*/
    }
}
