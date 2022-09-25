namespace FreeIDE.Common.Utils
{
    public static class TreeNodeHelper
    {
        public static bool ContainsNode(System.Windows.Forms.TreeNode node1, System.Windows.Forms.TreeNode node2)
        {
            if (node2 == null) return false;
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;
            return ContainsNode(node1, node2.Parent);
        }

        public static bool ContainsNodeInName(this System.Windows.Forms.TreeNode node1, System.Windows.Forms.TreeNode node2)
        {
            if (node1 == null || node2 == null) return false;
            if (node1.Nodes.Contains(node2)) return true;

            foreach (System.Windows.Forms.TreeNode node in node1.Nodes)
                if (node.Text == node2.Text || node.Name == node2.Name) return true;

            return false;
        }

        public static System.Windows.Forms.TreeNode[] GetFileNodes(string nodePath)
        {
            System.Windows.Forms.TreeNode[] treeFiles = new System.Windows.Forms.TreeNode[System.IO.Directory.GetFiles(nodePath).Length + System.IO.Directory.GetDirectories(nodePath).Length];

            int numNode = 0;
            foreach (string path in System.IO.Directory.GetDirectories(nodePath))
            {
                treeFiles[numNode] = new System.Windows.Forms.TreeNode(new System.IO.DirectoryInfo(path).Name);
                treeFiles[numNode].Name = new System.IO.FileInfo(path).Name;
                treeFiles[numNode].Nodes.Add(new System.Windows.Forms.TreeNode());
                treeFiles[numNode].Tag = path;
                treeFiles[numNode].ImageIndex = 1;
                treeFiles[numNode].SelectedImageIndex = 1;
                numNode++;
            }
            foreach (string path in System.IO.Directory.GetFiles(nodePath))
            {
                treeFiles[numNode] = new System.Windows.Forms.TreeNode(new System.IO.FileInfo(path).Name);
                treeFiles[numNode].Name = new System.IO.FileInfo(path).Name;
                treeFiles[numNode].Tag = path;
                treeFiles[numNode].ImageIndex = IconsUtil.GetImageIndexMini(new System.IO.FileInfo(path).Extension);
                treeFiles[numNode].SelectedImageIndex = IconsUtil.GetImageIndexMini(new System.IO.FileInfo(path).Extension);
                numNode++;
            }

            return treeFiles;
        }

        public static System.Collections.Generic.Queue<System.Windows.Forms.TreeNode> GetOpenNodes(System.Windows.Forms.TreeNodeCollection workTreeNode)
        {
            System.Collections.Generic.Queue<System.Windows.Forms.TreeNode> openNodes = new System.Collections.Generic.Queue<System.Windows.Forms.TreeNode>();
            System.Collections.Generic.Queue<System.Windows.Forms.TreeNode> staging = new System.Collections.Generic.Queue<System.Windows.Forms.TreeNode>();
            System.Windows.Forms.TreeNode treeNode;

            foreach (System.Windows.Forms.TreeNode node in workTreeNode)
                staging.Enqueue(node);

            //Using a queue to store and process each node in the TreeView
            while (staging.Count > 0)
            {
                treeNode = staging.Dequeue();

                if (treeNode.IsExpanded)
                    openNodes.Enqueue(treeNode);

                foreach (System.Windows.Forms.TreeNode node1 in treeNode.Nodes)
                    staging.Enqueue(node1);
            }

            return openNodes;
        }

        public static bool ContainsTag(this System.Windows.Forms.TreeNodeCollection treeNodeCollection, object Tag)
        {
            foreach (System.Windows.Forms.TreeNode node in treeNodeCollection)
                if (node.Tag == Tag)
                    return true;
            return false;
        }
    }
}
