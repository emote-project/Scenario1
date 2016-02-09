using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DecisionTree
{
    /// <summary>
    /// Classe que representa um atributo utilizado na classe de decis�o
    /// </summary>
    public class TreeAttribute
    {
        protected PossibleValueCollection _possibleValues;
        protected string _name;
        protected object _label;

        /// <summary>
        /// Inicializa uma nova inst�ncia de uma classe Atribute
        /// </summary>
        /// <param name="name">Indica o nome do atributo</param>
        /// <param name="values">Indica os valores poss�veis para o atributo</param>
        public TreeAttribute(string name, PossibleValueCollection possibleValues)
        {
            _name = name;

            if (possibleValues == null)
                _possibleValues = null;
            else
            {
                _possibleValues = possibleValues;
                _possibleValues.Sort();
            }
        }



        /// <summary>
        /// Indica o nome do atributo
        /// </summary>
        public string AttributeName
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Retorna um array com os valores do atributo
        /// </summary>
        public PossibleValueCollection PossibleValues
        {
            get
            {
                return _possibleValues;
            }
        }

        /// <summary>
        /// Indica se um valor � permitido para este atributo
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool isValidValue(string value)
        {
            return indexValue(value) >= 0;
        }

        /// <summary>
        /// Retorna o �ndice de um valor
        /// </summary>
        /// <param name="value">Valor a ser retornado</param>
        /// <returns>O valor do �ndice na qual a posi��o do valor se encontra</returns>
        public int indexValue(string value)
        {
            if (_possibleValues != null)
                return _possibleValues.BinarySearch(value);
            else
                return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_name != string.Empty)
            {
                return _name;
            }
            else
            {
                return _label.ToString();
            }
        }
    }
}
