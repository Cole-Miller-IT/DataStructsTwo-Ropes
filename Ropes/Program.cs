using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

//Implement these 2 compress/rebalance methods
//Just take the string of the left tree after the split and call the build method on it 
//1.After a Split, compress the path back to the root to ensure that binary tree is full, i.e. each non-leaf
//node has two non - empty children(4 marks).

//  10          10 (5 + 5) <-- this is the leaf now with the string chars
// 5  5 -->  null null
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

    //Insert string S at index i(5 marks). DONE
    public void Insert(string S, int i) {
        int maxIndex = root.Length - 1;
        Console.WriteLine("");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Insert string: " + S + " at index " + i);
        //Check if the index is out of range
        if (i < 0 || i > maxIndex) {
            //Out of range
            Console.WriteLine("Index out of range Insert()");
        } else {
            //build a Rope for the string
            Rope sRope = new Rope(S);

            //split the root where you want to insert
            Node rightTree = null;
            if (i == 0) {
                //don't split, insert infront of the tree
            } else {
                rightTree = Split(root, i);
            }

            //concat left side with the Rope for the string you want to insert
            Node leftTree = null;
            if (i == 0) {
                //insert string before the left tree
                leftTree = Concatenate(sRope.root, root);
            } else {
                //insert string after the left tree
                leftTree = Concatenate(root, sRope.root);
            }
            //Console.WriteLine("left tree concat with sRope");
            //PrintRope(leftTree, 0);

            //concat new Rope with the right side
            if (i == 0 || i == maxIndex) {
                //don't concat, right tree is null
                root = leftTree;
            } else {
                root = Concatenate(leftTree, rightTree);
                //Console.WriteLine("new root after concating everything");
                //PrintRope();
            }

            //balance the new tree
            root = Rebalance();

            Console.WriteLine("Final tree after insert");
            PrintRope();
            Console.WriteLine("Final string: ");
            Console.WriteLine(ToString());
        }    
    }

    //Delete the substring S[i, j](5 marks). DONE
    public void Delete(int i, int j) {
        Console.WriteLine("");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Delete substring at index range: [" + i + "," + j + "]");
        int maxIndex = root.Length - 1;

        //Check index
        if ((i < 0) || (j < 0) || (i > maxIndex) || (j > maxIndex)) {
            Console.WriteLine("Index out of range Delete()");
        }  else if (j < i) {
            Console.WriteLine("Index ranges are invalid Delete()");
        }
        else {
            Node rightTree = null;
            Node leftTree = null;
            //split tree a i, keep left side
            if (i == 0) {
                //Don't split, first char up to j is being removed
            } else {
                //left tree will be store in the root
                rightTree = Split(root, i - 1); //i - 1 b/c if we did the split at i it would keep the first value that we want to remove
            }
            leftTree = root.DeepCopy();


            //split the tree at j, keep right side
            if (j == maxIndex) {
                //removing from i to the end of the tree
                //so we can just take the left tree generated above
            } else if (i == 0) {
                //splitting from the start of the tree to j
                rightTree = Split(root, j);
            } else {
                //j ends before the last index so a right tree will exist
                j = j - i;  //update the index to work with the split tree
                root = rightTree;
                rightTree = Split(root, j);
            }


            //Determine how the tree was split
            if (i == 0) {
                //Start of the tree was removed
                //Determine if the whole tree was removed
                if (j == maxIndex) {
                    //Yes
                    root = new Node("");
                } else {
                    //No
                    root = rightTree;
                }
            } else if (j == maxIndex) {
                //Removed i up to the end of the tree
                root = leftTree;
            } else {
                //middle
                root = Concatenate(leftTree, rightTree);
            }


            root = Rebalance();
            Console.WriteLine("final tree");
            PrintRope();
        }
    }

    //Return the substring S[i, j](6 marks). DONE
    public string Substring(int i, int j) {
        int maxIndex = root.Length - 1;
        Node rootBackup = root.DeepCopy();

        Console.WriteLine("");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Retrieving substring at index range [" + i + "," + j + "]");
        //Console.WriteLine("Current Rope: ");
        //PrintRope();


        //Check index
        if ((i < 0) || (j < 0) || (i > maxIndex) || (j > maxIndex)) {
            Console.WriteLine("Index out of range substring()");
            return "-1";
        } else if (j < i) {
            Console.WriteLine("Index ranges are invalid substring()");
            return "-1";
        } else {
            //Get substring

            //
            Node rightTree = null;
            rightTree = Split(root, j); //root/left tree contains the start of the tree to j now
            //Might need to rebalance here
            //Console.WriteLine("root first split");
            //PrintRope(root, 0);

            if (i == 0) {
                //i is at the start
                //Do nothing, the tree already contains the substring
            } else if (i == j) {
                //i is at the end i
                rightTree = Split(root, i - 1);
                root = rightTree;
            } else {
                // i is > 0 and < j
                rightTree = Split(root, i);
                root = rightTree;
            }

            //Console.WriteLine("root after");
            //PrintRope(root, 0);
            string returnString = ToString();

            //Restore root
            root = rootBackup;

            Console.WriteLine("Substring: " + returnString);
            return ToString();
        }    
    }

    //Return the index of the first occurrence of S; -1 otherwise(9 marks).
    public int Find(string S) {
        //Not efficient but I don't have time to make this Find() method better
        //Search through the tree for all characters that start with the first letter of string S, return as a list or something

        //For all potential areas where the substring could be located
            //get backup of root

            //substring()

            //if substring matches string S
                //return index
            //else
                //continue

        //If you go through all possibilites and did not find it, return -1
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
                //Console.WriteLine("Leaf");
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

        Node rightTree = Split(root, index);

        Console.WriteLine("");
        Console.WriteLine("After split: Right");
        PrintRope(rightTree, 0);

        Console.WriteLine("");
        Console.WriteLine("After split: Left");
        PrintRope(root, 0);
    }

    //Tests the rebalance() method b/c it's private
    public void TestRebalance(int index) {
        Console.WriteLine("------------------------------");
        Console.WriteLine("");
        Console.WriteLine("Index to split is: " + index);
        Console.WriteLine("Before split");
        PrintRope();

        Node rightTree = Split(root, index);

        Console.WriteLine("");
        Console.WriteLine("After split: Right");
        PrintRope(rightTree, 0);

        Console.WriteLine("");
        Console.WriteLine("After split: Left");
        PrintRope(root, 0);

        root = Rebalance(); //Rebalance the left tree (root) after splitting
        Console.WriteLine("");
        Console.WriteLine("After rebalance (Left)");
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

    //Rebalance the rope using the algorithm found on pages 1319 - 1320 of Boehm et al. (9 marks). DONE
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

        //Builds a balanced tree from the nodes in the sequence
        Node ConcatenateSequence(List<Node> sequence) {
            // Concatenate all nodes in the sequence from smallest to largest
            Node result = null;

            //Determine how to build the tree
            if (sequence == null) {
                //No leaf nodes
                return null;
            } else if (sequence.Count == 1) {
                //Single leaf node
                return sequence[0];

            } else {
                //Many leaf nodes
                //Do some fibonacci magic

                //Generate a dictionary to construct a balanced tree. key(fibonacci #) --> node it stores
                //Return the nth Fibbonacci number
                int GetNthFibonacciNum(int n) {
                    if ((n == 0) || (n == 1)) {
                        return n;
                    } else
                        return GetNthFibonacciNum(n - 1) + GetNthFibonacciNum(n - 2);
                }

                //Generates a dictionary that holds the fibonacci numbers and Nodes to later rebalance the Rope
                Dictionary<int, Node> generateFibonacciDictionary() {
                    Dictionary<int, Node> fibDict = new Dictionary<int, Node>();
                    int fibNumsToGenerate = (root.Length / maxLeafStringLength) + 3;
                    int fibNum = -1;

                    //Console.WriteLine("Fib nums to generate: " + fibNumsToGenerate);
                    for (int i = 0; i < fibNumsToGenerate; i++) {
                        fibNum = GetNthFibonacciNum(i + 2);
                        //Console.Write(fibNum + ", ");
                        fibDict.Add(fibNum, null);
                    }
                    //Console.WriteLine("");


                    //Print fib dict
                    /*Console.WriteLine("Generated dict");
                    foreach (var element in fibDict)
                        Console.WriteLine("Key: {0}, Value: {1}", element.Key, element.Value);*/
                    return fibDict;
                }

                //Takes the leaf nodes from our sequence and balances them into a dictionary using the algorithm info found on pages 1319 - 1320 of Boehm et al.
                void balanceLeafNodes(Dictionary<int, Node> fibDict) {
                    //Go through all of leaf nodes stored in the sequence array
                    foreach (Node leafNode in sequence) {
                        //Go through the dictionary until you find where to put it
                        //insert going down the fibonacci dict until you find an open space that satisfies the depth requirment
                        bool searching = true;
                        int depth = 1; //NOTE: If I start at depth 0 I'll get 1,1,2,3,5 ... for my keys which is fine, but the diagram they show starts at 1,2,3,5,... so I start at depth 1
                        int key = GetNthFibonacciNum(depth);
                        Node nodeToInsert = leafNode;
                        while (searching) {
                            //Console.WriteLine("Depth: " + depth);
                            //Console.WriteLine("Key: " + key);
                            if (fibDict[key] == null) {
                                //insert
                                fibDict[key] = nodeToInsert;
                                break;
                            } else {
                                //concat with what is already there
                                nodeToInsert = Concatenate(fibDict[key], nodeToInsert);

                                //remove value from previous spot (the node that was in the dict that you concatenated with)
                                fibDict[key] = null;

                                //Check the next spot
                                depth = depth + 1;
                                key = GetNthFibonacciNum(depth + 2);
                            }
                        }
                        //Console.WriteLine("End of inserting leaf node: " + leafNode.stringCharacters);
                    }

                    //Print fib dict
                    /*
                    Console.WriteLine("");
                    Console.WriteLine("After inserting leaf nodes");
                    int count = 0;
                    foreach (var element in fibDict) {
                        Console.WriteLine("Key: {0}, Value: {1}", element.Key, element.Value);
                        if (element.Value != null) {
                            count = count + 1;
                            PrintRope(element.Value, 0);
                        }
                    }*/
                }

                //Concatenates all of the balanced sub Ropes in the fibonacci dictionary together
                Node finalRebalance(Dictionary<int, Node> fibDict) {
                    //Final concatenation over the whole dict to build the final Rope
                    //Start at the first key, moving up
                    Node finalBalancedTree = null;
                    foreach (var element in fibDict) {
                        //if the current key value isn't null
                        if (element.Value == null) {
                            //skip
                        } else {
                            //Console.WriteLine(element.Value.Length);
                            //am I storing a rope already
                            if (finalBalancedTree == null) {
                                //No
                                //store it
                                finalBalancedTree = element.Value;
                            } else {
                                //yes, concate and store new Rope
                                finalBalancedTree = Concatenate(element.Value, finalBalancedTree);
                            }
                        }
                    }
                    //Return final balanced tree
                    return finalBalancedTree;
                }


                //Start of else {
                //Generate fibonnaci #'s, balance the leaf nodes into sub trees, then rebalance all of the subtrees for a final rebalanced Rope
                Dictionary<int, Node> fibDict = new Dictionary<int, Node>();
                fibDict = generateFibonacciDictionary();

                balanceLeafNodes(fibDict);

                return finalRebalance(fibDict);
            }
        }

        //Start of Rebalance()
        List<Node> sequence = new List<Node>(); // To store ropes of different sizes

        //Gather all of the leaf nodes
        FillSequence(root, sequence);
        //Print for debugging
        /*Console.WriteLine("Sequence leafs collected: ");
        for (int i = 0; i < sequence.Count; i++) {
            Console.WriteLine(sequence[i].stringCharacters);
        }*/

        //Build a balanced tree from the collected leaves
        //Console.WriteLine("Tree after concatenateSequence()");
        Node rebalanceTree = ConcatenateSequence(sequence);

        //PrintRope(rebalanceTree, 0);
        return rebalanceTree;
    }

    //Time complexity for figuring out this method O(too got damn long x4) 
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

        //Builds the Right tree by removing the nodes accumulated while traversing down the tree
        Node BuildRightTree(Stack<Node> nodes) {
            Node rightTree = null;

            //Determine what to do with the stack
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
                    rightTree = Concatenate(rightTree, nodes.Pop());
                }
                return rightTree;
            }
        }

        //Recursively go down the tree, splits the node if required, and builds the left and right trees after the split.
        Node SplitPrivate(Node p, int i, Stack<Node> nodesToConcatenate) {
            Node rightTree = null;
            //PrintRope(p, 0);

            //Traverse down the tree
            if (p.Left == null && p.Right == null) {
                //At the leaf
                //Console.WriteLine("Leaf");
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

                //Concatenate all of the nodes in the stack together to form the right tree (and the left tree should already by finished)
                rightTree = BuildRightTree(nodesToConcatenate);

                return rightTree;

            } else if (i < p.Left.Length) {
                //Go left
                //Console.WriteLine("Left");
                //Console.WriteLine("i: " + i);
                nodesToConcatenate.Push(p.Right);
                p.Right = null;
                rightTree = SplitPrivate(p.Left, i, nodesToConcatenate);
            } else {
                //Go right
                //Console.WriteLine("right");
                i = i - p.Left.Length;
                //Console.WriteLine("i: " + i);
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
        
        
        Stack<Node> nodesToConcatenate = new Stack<Node>(); //Will hold the nodes of the right tree as we go down to the leaf node
        if (i > p.Length || i < 0) {
            Console.WriteLine("Index out of range for Split(). Split() not performed.");
            return null;
        }
        
        return SplitPrivate(p, i, nodesToConcatenate);
        /*if (i > root.Length || i < 0) {
            Console.WriteLine("Index out of range for Split(). Split() not performed.");
            return null;
        }

        return SplitPrivate(root, i, nodesToConcatenate);*/
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
        /*Rope myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestSplit(0);            //Test left most element
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestSplit(7);        //Test right side of left most element
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestSplit(8);        //Test random indexes
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestSplit(15);       //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestSplit(16);       //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestSplit(60);       //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestSplit(63);       //Test right most element
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestSplit(-1);           //Test incorrect index
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestSplit(999);*/          //Test incorrect index

        /*
        Rope myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(0);            //Test left most element
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(7);            //Test right side of left most element
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(8);            //Test random indexes
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(55);            //Test random indexes
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(15);           //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(16);           //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(60);           //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(3);           //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(24);           //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(32);           //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(31);           //
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(63);           //Test right most element
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(-1);           //Test incorrect index
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.TestRebalance(999);          //Test incorrect index
        */

        //Test inserting a string < maxLengthofString characters
        /*Rope myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("Hello", 0);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("Hello", 7);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("Hello", 22);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("Hello", 63);
        //Test inserting a string == maxLengthofString characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld", 0);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld", 7);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld", 22);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld", 63);
        //Test inserting a string > maxLengthofString characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld!", 0);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld!", 7);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld!", 22);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld!", 63);

        //Test inserting an out of range index
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld!", -1);
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Insert("HelloWorld!", 999);*/

        /*
        Rope myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(-1, -1);      //Test inserting an out of range index
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(999, 999);   //Test inserting an out of range index
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(23, 1);       //Test inserting an out of order index
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(0, 0);        //Test deleting the first character
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(63, 63);      //Test deleting the last character
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(0, 10);       //Test deleting front characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(23, 45);      //Test deleting middle characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(59, 63);      //Test deleting i to the end of the tree characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(2, 34);       //Test random characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Delete(32, 63);      //Test random characters*/

        Rope myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(-1, -1);      //Test an out of range index
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(999, 999);   //Test an out of range index
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(23, 1);       //Test an out of order index
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(0, 0);        //Test the first character
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(63, 63);      //Test the last character
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(0, 10);       //Test front characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(23, 45);      //Test middle characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(59, 63);      //Test i to the end of the tree characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(2, 34);       //Test random characters
        myRope = new Rope("aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhh");
        myRope.Substring(32, 63);      //Test random characters
    }
}
