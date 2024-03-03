using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

//Implement this 2 compress/rebalance methods
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

    // Method to deep copy this node and its subtree
    public Node DeepCopy() {
        Node newNode = new Node(this.stringCharacters);
        newNode.Length = this.Length; // Copy the length
        if (this.Left != null) {
            newNode.Left = this.Left.DeepCopy(); // Recursively copy the left child
        }
        if (this.Right != null) {
            newNode.Right = this.Right.DeepCopy(); // Recursively copy the right child
        }
        return newNode; // Return the new deep-copied node
    }
}

public class Rope {
    const int maxLeafStringLength = 10; //Final length
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
        Node tempRoot = root;

        tempRoot = Split(tempRoot, i);
        string iString = tempRoot.stringCharacters;
        Console.WriteLine(iString);

        //newRoot = Split(root, j);
        //string jString = newRoot.stringCharacters;
        //Console.WriteLine(jString);

        //string returnString = iString.Remove();

        //return returnString;
        return "-1";
    }

    //Return the index of the first occurrence of S; -1 otherwise(9 marks).
    public int Find(string S) {
        //Keep a backup of the Root

        //Call substring until you go through the whole Rope


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

        //root = Split(root, index);

        Node rightTree = Split(root, index);
        //Rebalance(); //Rebalance the left tree (root) after splitting

        Console.WriteLine("");
        Console.WriteLine("After split: Right");
        PrintRope(rightTree, 0);

        //Console.WriteLine("");
        //Console.WriteLine("After split: Left");
        //PrintRope(root, 0);
    }

    //Tests the split() method b/c it's private
    public void TestRebalance(int index) {
        Console.WriteLine("------------------------------");
        Console.WriteLine("");
        Console.WriteLine("Index to split is: " + index);
        Console.WriteLine("Before split");
        PrintRope();

        //root = Split(root, index);

        Node rightTree = Split(root, index);
        //Rebalance(); //Rebalance the left tree (root) after splitting

        Console.WriteLine("");
        Console.WriteLine("After split: Right");
        PrintRope(rightTree, 0);

        Console.WriteLine("");
        Console.WriteLine("After split: Left");
        PrintRope(root, 0);
    }

    // Method to compare two trees
    public static bool AreEqual(Node node1, Node node2) {
        // Check if both nodes are null (implies trees are the same up to this point)
        if (node1 == null && node2 == null) {
            return true;
        }

        // If one is null and the other is not, trees are not the same
        if (node1 == null || node2 == null) {
            return false;
        }

        // Check the current node's properties
        bool areCurrentNodesEqual = node1.stringCharacters == node2.stringCharacters
                                    && node1.Length == node2.Length;

        // Recursively check left and right subtrees
        bool areLeftSubtreesEqual = AreEqual(node1.Left, node2.Left);
        bool areRightSubtreesEqual = AreEqual(node1.Right, node2.Right);

        // Current nodes and their subtrees must be equal
        return areCurrentNodesEqual && areLeftSubtreesEqual && areRightSubtreesEqual;
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
        // Recursively traverse the rope, inserting leaves and subtrees into the sequence based on their length
        void FillSequence(Node node, List<Node> sequence) {
            if (node.Left != null) { 
                //Go left
                FillSequence(node.Left, sequence);
            }
            if (node.Right != null) {
                //Go Right
                FillSequence(node.Right, sequence);
            }
            if (node.Left == null && node.Right == null) { 
                //At leaf
                sequence.Add(node);
            }
        }

        Node ConcatenateSequence(List<Node> sequence) {
            // Concatenate all nodes in the sequence from smallest to largest
            Node result = null;
            foreach (var node in sequence) {
                result = Concatenate(result, node); // Implement Concatenate to merge two ropes
            }
            return result;
        }

        List<Node> sequence = new List<Node>(); // To store ropes of different sizes
        FillSequence(root, sequence);

        /*Console.WriteLine("Sequence leafs: ");
        for (int i = 0; i < sequence.Count; i++) {
            Console.WriteLine(sequence[i].stringCharacters);
        }*/

        //return ConcatenateSequence(sequence);


        return new Node("-1");
    }

    //Time complexity for figuring out this method O(too got damn long x3) 
    //Split the rope with root p at index i and return the root of the right subtree (9 marks). DONE
    /*private Node Split(Node p, int i) {
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

        //Checks if a node's right child is on the right side of the split
        bool inRightSplitArea(Node rightChild, int i) {
            //Console.WriteLine(i);
            if (i == 0) {
                //Right node could be on the right side
                return true;
            } 
            else {
                return false;
            }
        }

        //Handles building the right subtree going back up the recursive calls for the internal nodes
        Node SplitUp(ref Node p, Node previousRightTree, int i) {
            Node rightTree = previousRightTree;

            //is p (current node) on the right side of the split
            if (i == 0) {
                //Right of split
                Console.WriteLine("current is right");
                rightTree = p.DeepCopy();
                p = new Node("");
                return rightTree;
            } 
            else {
                //Left of split
                Console.WriteLine("-------current (p) left of split--------");
                Console.WriteLine("i: " + i);
                Console.WriteLine("p (current node): ");
                PrintRope(p, 0);
                Console.WriteLine("");
                Console.WriteLine("rightTree");
                PrintRope(rightTree, 0);
                Console.WriteLine("");
                //is p.Right on the right or left side of the split
                //if ((i <= ((p.Length - 1) - p.Right.Length))) {
                if (inRightSplitArea(p.Right, i - p.Left.Length)) {
                    //Right of split
                    Console.WriteLine("p.right right of split");

                    //is my right child different from what is currently in the right tree node
                    if (!AreEqual(p.Right, rightTree)) {
                        Console.WriteLine("p.right diferent from rightTree");
                        //yes
                        if (rightTree != null) {
                            //concatenate the rightTree and p.Right's child to form a new rightTree,
                            Console.WriteLine("Concat current rightTree and p.Right");
                            rightTree = Concatenate(rightTree, p.Right);
                            PrintRope(rightTree, 0);
                        } else {
                            rightTree = p.Right.DeepCopy();
                        }
                        
                    } 
                    else {
                        //no
                        //Console.WriteLine("p.right same as righTree");
                    }
                    p.Right = null;
                    return rightTree;
                }   
                else {
                    //Left of split
                    //just pass along what is already in the right tree
                    Console.WriteLine("p.right left of split");
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

                rightSubtree = p.Right.DeepCopy(); ;
                rightSubtree.Length = rightSubtree.stringCharacters.Length;

                p.Right = null;
                p.Length = p.Left.Length;

                //p = p.Right;
            } 
            else {
                //Case 2: i refers to the left or right most char in the string
                //Console.WriteLine("Don't split");

                //Determine what to send back
                if (i == 0) {
                    //Console.WriteLine("leaf right of split");
                    rightSubtree = p.DeepCopy();
                    rightSubtree.Length = rightSubtree.stringCharacters.Length;
                    p = null;
                } 
                else {
                    //Console.WriteLine("leaf left of split");
                    rightSubtree = null;
                }
            }
            //Console.WriteLine("leaf node");
            //Console.WriteLine("p:");
            //PrintRope(p, 0);
            //Console.WriteLine("rightTree:");
            //PrintRope(rightSubtree, 0);
            return rightSubtree;
        } 
        else if (i < p.Left.Length) {
            //Go Left
            //Console.WriteLine("Go left down");
            rightSubtree = Split(p.Left, i);
            //Console.WriteLine("right tree after split()");
            //PrintRope(rightSubtree, 0);
            
            //Going up
            //Console.WriteLine("Going up");
            //Console.WriteLine("index: " + i);
            i = i + p.Left.Length;
            rightSubtree = SplitUp(ref p, rightSubtree, i);
        } 
        else if (i >= p.Left.Length) {
            //Go right
            i = i - p.Left.Length;
            //Console.WriteLine("Go right down");
            rightSubtree = Split(p.Right, i);
            //Console.WriteLine("right tree after split()");
            //PrintRope(rightSubtree, 0);

            //Going up
            //Console.WriteLine("Going up");
            i = i + p.Left.Length;
            //Console.WriteLine("index: " + i);
            rightSubtree = SplitUp(ref p, rightSubtree, i);
        }

        root = p;
        return rightSubtree;
    }*/
    /*private Node Split(Node root, int i) {
        Node rightSubtreeRoot = null;

        // Inner function to traverse and split the tree
        Node traverse(Node current, int index, out bool shouldDetach) {
            shouldDetach = false;

            if (current == null) return null;

            // If at a leaf node and index matches
            if (index < current.Length && current.stringCharacters != "") {
                string leftString = current.stringCharacters.Substring(0, index);
                string rightString = current.stringCharacters.Substring(index);

                current.stringCharacters = leftString; // Update current node's string
                current.Length = leftString.Length; // Update length

                // Create a new node for the right part of the split
                Node rightPart = new Node(rightString) {
                    Length = rightString.Length
                };

                if (index > 0) {
                    rightSubtreeRoot = rightPart; // This node becomes the root of the right subtree
                    shouldDetach = true; // Indicate that this subtree should be detached
                }
                return rightPart;
            }

            // Traversal logic
            Node childResult = null;
            if (index < current.Left?.Length) {
                childResult = traverse(current.Left, index, out shouldDetach);
                if (shouldDetach) current.Left = null; // Detach if needed
            } else {
                childResult = traverse(current.Right, index - (current.Left?.Length ?? 0), out shouldDetach);
                if (shouldDetach) current.Right = null; // Detach if needed
            }

            return childResult;
        }

        bool shouldDetach; // This will be used to determine if the current subtree needs to be detached
        traverse(root, i, out shouldDetach);
        return rightSubtreeRoot;
    }*/
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

        Node BuildRightTree(Stack<Node> nodes) {
            Node rightTree = null;

            if (nodes.Count == 0) {
                //Stack is empty
                rightTree = null;
                return rightTree;
            } else if (nodes.Count == 1) {
                //Stack contains one element
                rightTree = nodes.Pop();
                return rightTree;
            } else {
                //Stack contains many elements
                rightTree = nodes.Pop();    //Removes having to use an if/else in the while loop
                while (nodes.Count > 0) {
                    //Concatenate this with the previous tree
                    Console.WriteLine("Top node on stack");
                    Console.WriteLine(nodes.Peek().stringCharacters);
                    rightTree = Concatenate(rightTree, nodes.Pop());
                    //Thread.Sleep(5000);
                }
                return rightTree;
            }
        }

        Node SplitPrivate(Node p, int i, Stack<Node> nodesToConcatenate) {
            Node rightTree = null;
            Console.WriteLine("p: " + p.Length);
            //Thread.Sleep(4000);
            if (p.Left == null && p.Right == null) {
                //At the leaf
                Console.WriteLine("Leaf");
                if (i == (p.Length - 1)) {
                    //Case 1: no split occurs
                    rightTree = null;
                } else {
                    //Case 2: Split occurs
                    //Split the node
                    p = SplitNode(p, i);

                    //Add the right side to the stack
                    nodesToConcatenate.Push(p.Right);

                    //Remove the right side from the left tree
                    p.Right = null;

                    //Update length
                    p.Length = p.Left.Length;
                }
                Console.WriteLine("bottom");
                //Concatenate all of the nodes in the stack together to form the right tree (and the left tree should already by finished)
                rightTree = BuildRightTree(nodesToConcatenate);
                Console.WriteLine("Built right Tree");
                PrintRope(rightTree, 0);

                Console.WriteLine("left tree");
                PrintRope(root, 0);
                return rightTree;

            } else if (i < p.Left.Length) {
                //Go left
                //Console.WriteLine("Left");
                nodesToConcatenate.Push(p.Right);
                p.Right = null;
                rightTree = SplitPrivate(p.Left, i, nodesToConcatenate);
            } else {
                //Go right
                //Console.WriteLine("right");
                i = i - p.Left.Length;
                rightTree = SplitPrivate(p.Right, i, nodesToConcatenate);
            }

            //Adjust length of left tree coming back up
            int leftLen = 0;
            int rightLen = 0;
            if (p.Left != null) {
                leftLen = p.Left.Length;
            }
            if (p.Right != null) {
                rightLen = p.Right.Length;
            }
            p.Length = leftLen + rightLen;

            return rightTree;
        }

        
        Node rightTree = null;
        Stack<Node> nodesToConcatenate = new Stack<Node>(); //Will hold the nodes of the right tree as we go down to the split
        rightTree = SplitPrivate(root, i, nodesToConcatenate);

        return rightTree;
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
        mySplitRope.TestSplit(2);*/



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


        //Testing rebalance on a normal rope
        Rope myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(7);    //
        /*
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(7);    //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(8);    //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(15);    //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(16);    //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(60);    //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(63);    //*/

    }
}
