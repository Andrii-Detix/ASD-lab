using System.Collections.Generic;

namespace Drawing.classes
{
    internal static class GraphCharacteristics
    {
        public static string ShowMainInfo(DirectedGraph graph)
        {
            string str = string.Empty;
            str += "Степені вершин: ";
            int[] arr = VertexDegree(graph);
            for (int i = 0; i < graph.Length; i++)
            {
                str += $"deg({i + 1}) = {arr[i]}; ";
            }

            if (graph.IsDirected)
            {
                str += "\r\nПозитивні напівстепені: ";
                arr = PosVertexHalfDegree(graph);
                for (int i = 0; i < graph.Length; i++)
                {
                    str += $"deg+({i + 1}) = {arr[i]}; ";
                }

                str += "\r\nНегативні напівстепені: ";
                arr = NegVertexHalfDegree(graph);
                for (int i = 0; i < graph.Length; i++)
                {
                    str += $"deg-({i + 1}) = {arr[i]}; ";
                }
            }

            str += "\r\nСтепінь однорідності: ";
            int regulDeg = CheckGraphRegularity(graph);
            str += (regulDeg == -1 ? $"-\r\n" : $"{regulDeg}\r\n");
            arr = FindHangingVertices(graph);
            str += "Висячі вершини: ";
            int arrLength = arr.Length;
            if (arr.Length == 0)
                str += "-\r\n";
            for (int i = 0; i < arrLength; i++)
            {
                str += $"{arr[i] + 1}";
                str += (i == arrLength - 1 ? ";\r\n" : ", ");
            }

            arr = FindIsolatedVertices(graph);
            str += "Ізольовані вершини: \n";
            arrLength = arr.Length;
            if (arr.Length == 0)
                str += "-\r\n";
            for (int i = 0; i < arrLength; i++)
            {
                str += $"{arr[i] + 1}";
                str += (i == arrLength - 1 ? ";\r\n" : ", ");
            }

            return str;
        }

        public static string ShowWays(DirectedGraph graph, int length)
        {
            string str = $"Шляхи довжиною {length}: ";
            int[][][] ways = length == 1 ? GetOneWays(graph) : FindWays(graph, length);
            for (int i = 0; i < ways.Length; i++)
            {
                for (int j = 0; j < ways[i].Length; j++)
                {
                    str += "[";
                    for (int k = 0; k < ways[i][j].Length; k++)
                    {
                        str += $"{ways[i][j][k] + 1}";
                        if (k != ways[i][j].Length - 1)
                            str += "-";
                    }

                    str += "]; ";
                }
            }

            return str;
        }

        public static string ShowConnectComponents(DirectedGraph graph)
        {
            string str = $"Компоненти зв'язоності: \r\n";
            int[][] components = GetConnectComponents(graph);
            for (int i = 0; i < components.Length; i++)
            {
                str += $"Компонента {i + 1}: ";
                for (int j = 0; j < components[i].Length; j++)
                {
                    str += $"{components[i][j] + 1}";
                    str += (j == components[i].Length - 1 ? ";\r\n" : ", ");
                }
            }

            return str;
        }

        public static int[] VertexDegree(DirectedGraph graph)
        {
            int[] result = new int[graph.Length];
            int index = 0;
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    if (!graph.IsDirected && graph.Matrix[j, i] == 1 && j < index)
                        continue;
                    if (graph.Matrix[i, j] == 1)
                    {
                        result[i]++;
                        result[j]++;
                    }
                }

                if (!graph.IsDirected)
                    index++;
            }

            return result;
        }

        public static int[] PosVertexHalfDegree(DirectedGraph graph)
        {
            int[] result = new int[graph.Length];
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    if (graph.Matrix[i, j] == 1)
                        result[i]++;
                }
            }

            return result;
        }

        public static int[] NegVertexHalfDegree(DirectedGraph graph)
        {
            int[] result = new int[graph.Length];
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    if (graph.Matrix[i, j] == 1)
                        result[j]++;
                }
            }

            return result;
        }

        public static int CheckGraphRegularity(DirectedGraph graph)
        {
            int[] degrees = VertexDegree(graph);
            int degree = degrees[0];
            for (int i = 1; i < graph.Length; i++)
            {
                if (degree != degrees[i])
                {
                    degree = -1;
                    break;
                }
            }

            return degree;
        }

        public static int[] FindHangingVertices(DirectedGraph graph)
        {
            int[] degrees = VertexDegree(graph);
            List<int> vertices = new List<int>();
            for (int i = 0; i < graph.Length; i++)
            {
                if (degrees[i] == 1)
                    vertices.Add(i);
            }

            return vertices.ToArray();
        }

        public static int[] FindIsolatedVertices(DirectedGraph graph)
        {
            int[] degrees = VertexDegree(graph);
            List<int> vertices = new List<int>();
            for (int i = 0; i < graph.Length; i++)
            {
                if (degrees[i] == 0 || degrees[i] == 2 && graph.Matrix[i, i] == 1)
                    vertices.Add(i);
            }

            return vertices.ToArray();
        }

        public static int[][][] FindWays(DirectedGraph graph, int wayLength)
        {
            int[,] degreeMatrix = ActMatrix.PowMatrix(graph.Matrix, 2);
            int[][][] previousWays = GetOneWays(graph);
            int[][][] foundWays = new int[graph.Length][][];
            List<int[]> ways = new List<int[]>();
            int count;
            int max = wayLength - 1;
            for (int counter = 0; counter < max; counter++)
            {
                for (int i = 0; i < graph.Length; i++)
                {
                    for (int j = 0; j < graph.Length; j++)
                    {
                        if (degreeMatrix[i, j] != 0)
                        {
                            count = degreeMatrix[i, j];
                            for (int ind = 0; count != 0 && ind < previousWays[i].Length; ind++)
                            {
                                int[] arr = previousWays[i][ind];
                                int k = arr[arr.Length - 1];
                                if (graph.Matrix[k, j] == 1)
                                {
                                    int limit = arr.Length - 1;
                                    bool repeat = false;
                                    for (int l = 0; l < limit; l++)
                                    {
                                        if (arr[l] == k && arr[l + 1] == j)
                                        {
                                            repeat = true;
                                            break;
                                        }
                                    }

                                    if (!repeat)
                                    {
                                        int[] helper = new int[arr.Length + 1];
                                        for (int l = 0; l < arr.Length; l++)
                                        {
                                            helper[l] = arr[l];
                                        }

                                        helper[arr.Length] = j;
                                        ways.Add(helper);
                                    }

                                    count--;
                                }
                            }
                        }
                    }

                    foundWays[i] = ways.ToArray();
                    ways.Clear();
                }

                if (counter != max - 1)
                {
                    degreeMatrix = ActMatrix.MultMatrix(degreeMatrix, graph.Matrix);
                    previousWays = foundWays;
                    foundWays = new int[graph.Length][][];
                }
            }

            return foundWays;
        }

        public static int[][][] GetOneWays(DirectedGraph graph)
        {
            int[][][] oneWays = new int[graph.Length][][];
            List<int[]> ways = new List<int[]>();

            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    if (graph.Matrix[i, j] == 1)
                        ways.Add(new int[] { i, j });
                }

                oneWays[i] = ways.ToArray();
                ways.Clear();
            }

            return oneWays;
        }

        public static int[,] GetReachabilityMatrix(DirectedGraph graph)
        {
            int[,] degreeMatrix = ActMatrix.CopyMatrix(graph.Matrix);
            int[,] result = ActMatrix.GetUnitMatrix(graph.Length);
            for (int i = 0; i < graph.Length - 1; i++)
            {
                result = ActMatrix.AddMatrix(result, degreeMatrix);
                if (i != graph.Length - 2)
                    degreeMatrix = ActMatrix.MultMatrix(degreeMatrix, graph.Matrix);
            }

            result = ActMatrix.BooleanMapping(result);
            return result;
        }

        public static int[,] GetStrongConnectMatrix(DirectedGraph graph)
        {
            int[,] reach = GetReachabilityMatrix(graph);
            int[,] conMatrix = ActMatrix.TransposeMatrix(reach);
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    conMatrix[i, j] *= reach[i, j];
                }
            }

            return conMatrix;
        }

        public static int[][] GetConnectComponents(DirectedGraph graph)
        {
            int[,] connectMatrix = GetStrongConnectMatrix(graph);
            List<int[]> components = new List<int[]>();
            List<int> compVertices = new List<int>();
            int[] vertices = new int[graph.Length];
            for (int i = 0; i < graph.Length; i++)
            {
                if (vertices[i] == 1)
                    continue;
                for (int j = 0; j < graph.Length; j++)
                {
                    if (connectMatrix[i, j] == 1)
                    {
                        compVertices.Add(j);
                        vertices[j] = 1;
                    }
                }

                components.Add(compVertices.ToArray());
                compVertices.Clear();
            }

            return components.ToArray();
        }

        public static int[,] GetCondensMatrix(DirectedGraph graph)
        {
            int[][] components = GetConnectComponents(graph);
            int length = components.Length;
            int[,] condMatrix = new int[length, length];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (i == j || condMatrix[j, i] == 1)
                        continue;
                    foreach (int first in components[i])
                    {
                        bool connection = false;
                        foreach (int second in components[j])
                        {
                            if (graph.Matrix[first, second] == 1)
                            {
                                condMatrix[i, j] = 1;
                                connection = true;
                                break;
                            }
                        }

                        if (connection)
                            break;
                    }
                }
            }

            return condMatrix;
        }


        public static (int[,] searchMatrix, int[] offsetVertices) BFS(DirectedGraph graph)
        {
            int[] nodeInfo = new int[graph.Length];
            Queue<int> openNodes = new Queue<int>();
            int k = 0;
            int[,] searchMatrix = new int[graph.Length, graph.Length];
            for (int i = 0; i < graph.Length; i++)
            {
                if (nodeInfo[i] == 0)
                {
                    for (int j = 0; j < graph.Length; j++)
                    {
                        if (nodeInfo[j] == 0 && graph.Matrix[i, j] == 1 && i != j)
                        {
                            openNodes.Enqueue(i);
                            k++;
                            nodeInfo[i] = k;
                            break;
                        }
                    }
                }

                while (openNodes.Count != 0)
                {
                    int node = openNodes.Peek();
                    for (int j = 0; j < graph.Length; j++)
                    {
                        if (nodeInfo[j] != 0 || j == node || graph.Matrix[node, j] == 0)
                            continue;

                        k++;
                        nodeInfo[j] = k;
                        searchMatrix[node, j] = 1;
                        openNodes.Enqueue(j);
                    }

                    openNodes.Dequeue();
                }
            }

            SearchRemainingVerteces(graph, nodeInfo, k);
            return (searchMatrix, nodeInfo);
        }




        
        public static (int[,] searchMatrix, int[] offsetVertices) DFS(DirectedGraph graph)
        {
            int[] nodeInfo = new int[graph.Length];
            int k = 0;
            int[,] searchMatrix = new int[graph.Length, graph.Length];
            for (int i = 0; i < graph.Length; i++)
            {
                if (nodeInfo[i] == 0)
                {
                    for (int j = 0; j < graph.Length; j++)
                    {
                        if (nodeInfo[j] == 0 && graph.Matrix[i, j] == 1 && i != j)
                        {
                            k++;
                            nodeInfo[i] = k;
                            k = DepthSearch(graph, i, nodeInfo, k, searchMatrix);
                            break;
                        }
                    }
                }
            }
            SearchRemainingVerteces(graph, nodeInfo,k);
            return (searchMatrix, nodeInfo);
        }

        private static int DepthSearch(DirectedGraph graph, int node, int[] nodeInfo, int k, int[,] searchMatrix)
        {
            
            for (int i = 0; i < graph.Length; i++)
            {
                if(nodeInfo[i] != 0 || i == node || graph.Matrix[node, i] == 0)
                    continue;
                k++;
                nodeInfo[i] = k;
                searchMatrix[node, i] = 1;
                
                k = DepthSearch(graph, i, nodeInfo, k, searchMatrix);
                

            }
            return k;
        }
        
        private static void SearchRemainingVerteces(DirectedGraph graph, int[] nodeInfo, int k)
        {
            for (int i = 0; i < graph.Length; i++)
            {
                if (nodeInfo[i] == 0)
                {
                    k++;
                    nodeInfo[i] = k;
                }
            }
        }

        public static EdgeLinkedList GetMinSkeleton(WeightedGraph graph)
        {
            int numEdges = graph.Length - 1;
            EdgeLinkedList edges = graph.ToLinkedList();
            edges.Sort();
            EdgeLinkedList skeleton = new EdgeLinkedList();

            while (!(skeleton.Count() == numEdges || edges.GetFirst() == null))
            {
                Edge edge = edges.DeleteFirst();
                if(edge.Vertex1 == edge.Vertex2)
                    continue;
                skeleton.AddLast(edge);
                if (HasLoop(skeleton))
                {
                    skeleton.DeleteLast();
                }
            }

            return skeleton;
        }

        
        private static bool HasLoop(EdgeLinkedList edges)
        {
            MyLinkedList<int> foundVert = new MyLinkedList<int>();
            var nodes = new MyStack<Node<Edge>>();
            var prevVertices = new MyStack<int>();
            Node<Edge> curNode = edges.GetFirst();

            while (curNode != null)
            {
                int curVertex = curNode.value.Vertex1;
                if (foundVert.Include(curVertex))
                {
                    curNode = curNode.next;
                    continue;
                }
                
                foundVert.AddFirst(curVertex);
                prevVertices.Push(curVertex);
                nodes.Push(curNode);
                curVertex = curNode.value.Vertex2;
                foundVert.AddFirst(curVertex);
                nodes.Push(curNode);
                prevVertices.Push(curVertex);
                Node<Edge> tempNode = edges.GetFirst();
                while (!prevVertices.isEmpty() )
                {
                    if (tempNode == null)
                    {
                        tempNode = nodes.Pop().next;
                        prevVertices.Pop();
                        if (!prevVertices.isEmpty())
                            curVertex = prevVertices.Peek();

                        continue;
                    }
                    if (tempNode == nodes.Peek())
                    {
                        tempNode = tempNode.next;
                        continue;
                    }

                    int helpVertex;
                    if (tempNode.value.Vertex1 == curVertex)
                        helpVertex = tempNode.value.Vertex2;
                    else if (tempNode.value.Vertex2 == curVertex)
                        helpVertex = tempNode.value.Vertex1;
                    else
                    {
                        tempNode = tempNode.next;
                        continue;
                    }

                    if (foundVert.Include(helpVertex))
                    {
                        return true;
                    }
                    
                    foundVert.AddFirst(helpVertex);
                    prevVertices.Push(helpVertex);
                    curVertex = helpVertex;
                    nodes.Push(tempNode);
                    tempNode = edges.GetFirst();

                }

                curNode =  curNode.next;

            }

            return false;
        }
        
    }
}
