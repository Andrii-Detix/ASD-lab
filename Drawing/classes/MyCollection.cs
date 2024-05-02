using System;
using System.Collections.Generic;

namespace Drawing.classes
{
    public class MyLinkedList<T>
    {
        public MyLinkedList(T value)
        {
            Head = new Node<T>(value);
        }

        public MyLinkedList()
        {
        }

        protected Node<T> Head = null;


        public int Count()
        {
            Node<T> node = Head;
            int count = 0;
            while (node != null)
            {
                count++;
                node = node.next;
            }

            return count;
        }

        public Node<T> GetFirst() => Head;


        public Node<T> GetLast()
        {
            Node<T> node = Head;
            while (node.next != null)
            {
                node = node.next;
            }

            return node;
        }


        public void AddFirst(T value)
        {
            Node<T> node = new Node<T>(value, Head);
            Head = node;
        }

        public void AddLast(T value)
        {
            if (Head == null)
            {
                AddFirst(value);
            }
            else
            {
                Node<T> node = Head;
                Node<T> added = new Node<T>(value);
                while (node.next != null)
                {
                    node = node.next;
                }

                node.next = added;
            }
        }

        public T DeleteFirst()
        {
            T value = Head.value;
            Head = Head.next;
            return value;
        }

        public T DeleteLast()
        {
            if (Head.next == null)
                return DeleteFirst();
            else
            {
                Node<T> curNode = Head;
                Node<T> prevNode = null;
                while (curNode.next != null)
                {
                    prevNode = curNode;
                    curNode = curNode.next;
                }

                prevNode.next = null;
                return curNode.value;
            }
        }

        public void DeleteElement(T value)
        {
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            if (c.Equals(Head.value, value))
                DeleteFirst();
            else
            {

                Node<T> curNode = Head;
                Node<T> prevNode = null;
                while (!c.Equals(curNode.value, value))
                {
                    prevNode = curNode;
                    curNode = curNode.next;
                }

                prevNode.next = curNode.next;
            }
        }

        public bool Include(T value)
        {
            Node<T> node = Head;
            bool include = false;
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            while (node != null)
            {
                if (c.Equals(node.value, value))
                {
                    include = true;
                    break;
                }

                node = node.next;
            }

            return include;
        }

        public Node<T> Find(T value)
        {
            Node<T> node = Head;
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            while (node != null)
            {
                if (c.Equals(node.value, value))
                    break;
                node = node.next;
            }

            return node;
        }

        public virtual void WriteAll()
        {
            Node<T> node = Head;
            while (node != null)
            {
                Console.Write($"{node.value}\r\n");
                node = node.next;
            }
        }

        public void Clear()
        {
            Head = null;
        }
    }


    public class Node<T>
    {
        public Node(T value)
        {
            this.value = value;
        }

        public Node(T value, Node<T> nextNode)
        {
            this.value = value;
            this.next = nextNode;
        }

        public T value;
        public Node<T> next = null;
    }


    public class EdgeLinkedList : MyLinkedList<Edge>
    {
        public EdgeLinkedList(Edge edge) : base(edge)
        {
        }

        public EdgeLinkedList() : base()
        {
        }

        public void Sort()
        {
            Node<Edge> curNode = Head.next;
            Node<Edge> prevNode = Head;
            Node<Edge> compNode;
            Node<Edge> prevAdded;

            while (curNode != null)
            {
                bool changed = false;
                compNode = Head;
                prevAdded = null;
                while (compNode != curNode)
                {
                    if (curNode.value.Weight < compNode.value.Weight)
                    {
                        prevNode.next = curNode.next;
                        curNode.next = compNode;
                        if (compNode == Head)
                        {
                            Head = curNode;
                        }
                        else
                        {
                            prevAdded.next = curNode;
                        }

                        curNode = prevNode.next;
                        changed = true;
                        break;
                    }

                    prevAdded = compNode;
                    compNode = compNode.next;
                }

                if (!changed)
                {
                    prevNode = curNode;
                    curNode = curNode.next;
                }
            }
        }

        public override void WriteAll()
        {
            Node<Edge> node = Head;

            while (node != null)
            {
                Console.Write(
                    $"\r\nVertex 1: {node.value.Vertex1 + 1}\tVertex 2 : {node.value.Vertex2 + 1}\tValue: {node.value.Weight}");
                node = node.next;
            }
        }

        public int TotalWeight()
        {
            Node<Edge> node = Head;
            int total = 0;
            while (node != null)
            {
                total += node.value.Weight;
                node = node.next;
            }

            return total;
        }
    }


    public class MyStack<T>
    {
        public MyStack(T value)
        {
            Head = new Node<T>(value);
        }

        public MyStack()
        {
        }

        private Node<T> Head = null;


        public T Peek()
        {
            return Head.value;
        }

        public T Pop()
        {
            T value = Head.value;
            Head = Head.next;
            return value;
        }

        public void Push(T value)
        {
            Node<T> node = new Node<T>(value, Head);
            Head = node;
        }

        public bool isEmpty()
        {
            return Head == null;
        }

        public void Clear()
        {
            Head = null;
        }
    }
}