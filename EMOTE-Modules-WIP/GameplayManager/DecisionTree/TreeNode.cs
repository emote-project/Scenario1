using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionTree
{
    /// <summary>
    /// Classe que representar� a arvore de decis�o montada;
    /// </summary>
    public class TreeNode
    {
        TreeNodeCollection _children;
        private TreeAttribute _attribute;

        /// <summary>
        /// Inicializa uma nova inst�ncia de TreeNode
        /// </summary>
        /// <param name="attribute">Atributo ao qual o node est� ligado</param>
        public TreeNode(TreeAttribute attribute)
        {
            if (attribute != null && attribute.PossibleValues != null)
            {
                _children = new TreeNodeCollection();                
                for (int i = 0; i < attribute.PossibleValues.Count; i++)
                    _children.Add(null);
            }
            else
            {
                _children = new TreeNodeCollection();
                _children.Add(null);
            }
            _attribute = attribute;
        }

        /// <summary>
        /// Adiciona um TreeNode filho a este treenode no galho de nome indicicado pelo ValueName
        /// </summary>
        /// <param name="treeNode">TreeNode filho a ser adicionado</param>
        /// <param name="ValueName">Nome do galho onde o treeNode � criado</param>
        public void AddTreeNode(TreeNode treeNode, string ValueName)
        {
            int index = _attribute.indexValue(ValueName);
            _children[index] = treeNode;
        }

        /// <summary>
        /// Retorna o nro total de filhos do n�
        /// </summary>
        public int ChildrenCount
        {
            get
            {
                return _children.Count;
            }
        }

        /// <summary>
        /// Retorna o n� filho de um n�
        /// </summary>
        /// <param name="index">Indice do n� filho</param>
        /// <returns>Um objeto da classe TreeNode representando o n�</returns>
        public TreeNode GetChildAt(int index)
        {
            return _children[index];
        }

        /// <summary>
        /// Atributo que est� conectado ao N�
        /// </summary>
        public TreeAttribute Attribute
        {
            get
            {
                return _attribute;
            }
        }

        /// <summary>
        /// Retorna o filho de um n� pelo nome do galho que leva at� ele
        /// </summary>
        /// <param name="branchName">Nome do galho</param>
        /// <returns>O n�</returns>
        public TreeNode GetChildByBranchName(string branchName)
        {
            int index = _attribute.indexValue(branchName);
            return _children[index];
        }
    }
}
