using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;

namespace DecisionTree
{


    /// <summary>
    /// Classe que implementa uma árvore de Decisão usando o algoritmo ID3
    /// </summary>
    public class DecisionTree
    {
        private DataTable _sampleData;
        private int mTotalPositives = 0;
        private int mTotal = 0;
        private string mTargetAttribute = "StrategyMay";
        private double mEntropySet = 0.0;

        ///
        private int mTotalSG = 0;
        private int mTotalS1 = 0;
        private int mTotalS2 = 0;
        private int mTotalS3 = 0;

        /// <summary>
        /// The total positive count in the source data
        /// </summary>
        /// <param name="samples">DataTable com as amostras</param>
        /// <returns>O nro total de amostras positivas</returns>
        private int countTotalPositives(DataTable samples)
        {
            int result = 0;

            foreach (DataRow aRow in samples.Rows)
            {
                if (aRow[mTargetAttribute].ToString().ToUpper().Trim() == "TRUE")
                    result++;
            }

            return result;
        }
        /// <summary>
        /// EDITADO!!!!!!!!
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        /// Counts the total of Global Strategies (SG)
        private int countTotalSG(DataTable samples)
        {
            int result = 0;

            foreach (DataRow aRow in samples.Rows)
            {
                if (aRow[mTargetAttribute].ToString().ToUpper().Trim() == "SG")
                    result++;
            }

            return result;
        }
        /// Counts the total of Strategies that support the Evironmentalist (S1)
        private int countTotalS1(DataTable samples)
        {
            int result = 0;

            foreach (DataRow aRow in samples.Rows)
            {
                if (aRow[mTargetAttribute].ToString().ToUpper().Trim() == "S1")
                    result++;
            }

            return result;
        }
        /// Counts the total of strategies that support the Economist (S2)
        private int countTotalS2(DataTable samples)
        {
            int result = 0;

            foreach (DataRow aRow in samples.Rows)
            {
                if (aRow[mTargetAttribute].ToString().ToUpper().Trim() == "S2")
                    result++;
            }

            return result;
        }
        /// Counts the total pf strategies that support the President (S3)
        private int countTotalS3(DataTable samples)
        {
            int result = 0;

            foreach (DataRow aRow in samples.Rows)
            {
                if (aRow[mTargetAttribute].ToString().ToUpper().Trim() == "S3")
                    result++;
            }

            return result;
        }
        ///EDITADO!!!!!!!!!

        /// <summary>
        /// Calcula a entropia dada a seguinte fórmula
        /// -p+log2p+ - p-log2p-
        /// 
        /// onde: p+ é a proporção de valores positivos
        ///		  p- é a proporção de valores negativos
        /// </summary>
        /// <param name="positives">Quantidade de valores positivos</param>
        /// <param name="negatives">Quantidade de valores negativos</param>
        /// <returns>Retorna o valor da Entropia</returns>
        private double getCalculatedEntropy(int positives, int negatives)
        {
            int total = positives + negatives;
            double ratioPositive = (double)positives / total;
            double ratioNegative = (double)negatives / total;

            if (ratioPositive != 0)
                ratioPositive = -(ratioPositive) * System.Math.Log(ratioPositive, 2);
            if (ratioNegative != 0)
                ratioNegative = -(ratioNegative) * System.Math.Log(ratioNegative, 2);

            double result = ratioPositive + ratioNegative;

            return result;
        }
        /// EDITADO!!!!!
        private double getCalculatedEntropyV2(int SGs, int S1s, int S2s, int S3s)
        {
            int total = SGs + S1s + S2s + S3s;

            if (total == 0)
                return 0;

            double ratioSG = (double)SGs / total;
            double ratioS1 = (double)S1s / total;
            double ratioS2 = (double)S2s / total;
            double ratioS3 = (double)S3s / total;

            if (ratioSG != 0)
                ratioSG = -(ratioSG) * System.Math.Log(ratioSG, 2);
            if (ratioS1 != 0)
                ratioS1 = -(ratioS1) * System.Math.Log(ratioS1, 2);
            if (ratioS2 != 0)
                ratioS2 = -(ratioS2) * System.Math.Log(ratioS2, 2);
            if (ratioS3 != 0)
                ratioS3 = -(ratioS3) * System.Math.Log(ratioS3, 2);

            double result = ratioSG + ratioS1 + ratioS2 + ratioS3;

            return result;
        }


        /// <summary>
        /// Varre tabela de amostras verificando um atributo e se o resultado é positivo ou negativo
        /// </summary>
        /// <param name="samples">DataTable com as amostras</param>
        /// <param name="attribute">Atributo a ser pesquisado</param>
        /// <param name="value">valor permitido para o atributo</param>
        /// <param name="positives">Conterá o nro de todos os atributos com o valor determinado com resultado positivo</param>
        /// <param name="negatives">Conterá o nro de todos os atributos com o valor determinado com resultado negativo</param>

        private void getValuesToAttributeV2(DataTable samples, TreeAttribute attribute, string value, out int SGs, out int S1s, out int S2s, out int S3s)
        {
            SGs = 0;
            S1s = 0;
            S2s = 0;
            S3s = 0;

            foreach (DataRow aRow in samples.Rows)
            {
                ///To do:   Figure out if this is correct - it looks bad
                if ((aRow[attribute.AttributeName].ToString() == value))
                {
                    if (aRow[mTargetAttribute].ToString().Trim().ToUpper() == "SG")
                        SGs++;
                    if (aRow[mTargetAttribute].ToString().Trim().ToUpper() == "S1")
                        S1s++;
                    if (aRow[mTargetAttribute].ToString().Trim().ToUpper() == "S2")
                        S2s++;
                    if (aRow[mTargetAttribute].ToString().Trim().ToUpper() == "S3")
                        S3s++;
                }
            }
        }

        /// <summary>
        /// Calcula o ganho de um atributo
        /// </summary>
        /// <param name="attribute">Atributo a ser calculado</param>
        /// <returns>O ganho do atributo</returns>
        private double gainV2(DataTable samples, TreeAttribute attribute)
        {
            PossibleValueCollection values = attribute.PossibleValues; /// gets the possible values taken by an attribute
            double sum = 0.0;

            for (int i = 0; i < values.Count; i++)
            {
                int SGs, S1s, S2s, S3s;

                SGs = S1s = S2s = S3s = 0;

                getValuesToAttributeV2(samples, attribute, values[i], out SGs, out S1s, out S2s, out S3s);
                ///Calculates the information gain
                double entropy = getCalculatedEntropyV2(SGs, S1s, S2s, S3s);
                if(entropy != 0)
                    sum += -(double)(SGs + S1s + S2s + S3s) / mTotal * entropy;
            }
            return mEntropySet + sum;
        }

        /// <summary>
        /// Retorna o melhor atributo.
        /// </summary>
        /// <param name="attributes">Um vetor com os atributos</param>
        /// <returns>Retorna o que tiver maior ganho</returns>
        private TreeAttribute getBestAttribute(DataTable samples, TreeAttributeCollection attributes)
        {
            double maxGain = 0.0;
            TreeAttribute result = null;

            foreach (TreeAttribute attribute in attributes)
            {
                double aux = gainV2(samples, attribute);
                if (aux > maxGain)
                {
                    maxGain = aux;
                    result = attribute;
                }
            }
            return result;
        }

        /// <summary>
        /// Retorna true caso todos os exemplos da amostragem são positivos
        /// </summary>
        /// <param name="samples">DataTable com as amostras</param>
        /// <param name="targetAttribute">Atributo (coluna) da tabela a qual será verificado</param>
        /// <returns>True caso todos os exemplos da amostragem são positivos</returns>
        private bool allSamplesAreSG(DataTable samples, string targetAttribute)
        {
            foreach (DataRow row in samples.Rows)
            {
                if (row[targetAttribute].ToString().ToUpper().Trim() != "SG")
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Retorna true caso todos os exemplos da amostragem são negativos
        /// </summary>
        /// <param name="samples">DataTable com as amostras</param>
        /// <param name="targetAttribute">Atributo (coluna) da tabela a qual será verificado</param>
        /// <returns>True caso todos os exemplos da amostragem são negativos</returns>
        private bool allSamplesAreS1(DataTable samples, string targetAttribute)
        {
            foreach (DataRow row in samples.Rows)
            {
                if (row[targetAttribute].ToString().ToUpper().Trim() != "S1")
                    return false;
            }

            return true;
        }
        private bool allSamplesAreS2(DataTable samples, string targetAttribute)
        {
            foreach (DataRow row in samples.Rows)
            {
                if (row[targetAttribute].ToString().ToUpper().Trim() != "S2")
                    return false;
            }

            return true;
        }
        private bool allSamplesAreS3(DataTable samples, string targetAttribute)
        {
            foreach (DataRow row in samples.Rows)
            {
                if (row[targetAttribute].ToString().ToUpper().Trim() != "S3")
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Retorna uma lista com todos os valores distintos de uma tabela de amostragem
        /// </summary>
        /// <param name="samples">DataTable com as amostras</param>
        /// <param name="targetAttribute">Atributo (coluna) da tabela a qual será verificado</param>
        /// <returns>Um ArrayList com os valores distintos</returns>
        private ArrayList getDistinctValues(DataTable samples, string targetAttribute)
        {
            ArrayList distinctValues = new ArrayList(samples.Rows.Count);

            foreach (DataRow row in samples.Rows)
            {
                if (distinctValues.IndexOf(row[targetAttribute]) == -1)
                    distinctValues.Add(row[targetAttribute]);
            }

            return distinctValues;
        }

        /// <summary>
        /// Retorna o valor mais comum dentro de uma amostragem
        /// </summary>
        /// <param name="samples">DataTable com as amostras</param>
        /// <param name="targetAttribute">Atributo (coluna) da tabela a qual será verificado</param>
        /// <returns>Retorna o objeto com maior incidência dentro da tabela de amostras</returns>
        private object getMostCommonValue(DataTable samples, string targetAttribute)
        {
            ArrayList distinctValues = getDistinctValues(samples, targetAttribute);
            int[] count = new int[distinctValues.Count];

            foreach (DataRow row in samples.Rows)
            {
                int index = distinctValues.IndexOf(row[targetAttribute]);
                count[index]++;
            }

            int MaxIndex = 0;
            int MaxCount = 0;

            for (int i = 0; i < count.Length; i++)
            {
                if (count[i] > MaxCount)
                {
                    MaxCount = count[i];
                    MaxIndex = i;
                }
            }

                   
            return distinctValues[MaxIndex];
        }

        /// <summary>
        /// Monta uma árvore de decisão baseado nas amostragens apresentadas
        /// </summary>
        /// <param name="samples">Tabela com as amostragens que serão apresentadas para a montagem da árvore</param>
        /// <param name="targetAttribute">Nome da coluna da tabela que possue o valor true ou false para 
        /// validar ou não uma amostragem</param>
        /// <returns>A raiz da árvore de decisão montada</returns></returns?>
        private TreeNode internalMountTree(DataTable samples, string targetAttribute, TreeAttributeCollection attributes)
        {
            if (samples.Rows.Count == 0)
                return new TreeNode(new OutcomeTreeAttribute("null"));

           if (allSamplesAreSG(samples, targetAttribute) == true)
              return new TreeNode(new OutcomeTreeAttribute("SG"));

            if (allSamplesAreS1(samples, targetAttribute) == true)
                return new TreeNode(new OutcomeTreeAttribute("S1"));

            if (allSamplesAreS2(samples, targetAttribute) == true)
                return new TreeNode(new OutcomeTreeAttribute("S2"));

            if (allSamplesAreS3(samples, targetAttribute) == true)
                return new TreeNode(new OutcomeTreeAttribute("S3"));

            
            if (attributes.Count == 0)
                return new TreeNode(new OutcomeTreeAttribute(getMostCommonValue(samples, targetAttribute)));

            
            mTotal = samples.Rows.Count;
            mTargetAttribute = targetAttribute;
            //mTotalPositives = countTotalPositives(samples);

            ///editado
            mTotalSG = countTotalSG(samples);
            mTotalS1 = countTotalS1(samples);
            mTotalS2 = countTotalS2(samples);
            mTotalS3 = countTotalS3(samples);
            ///

            mEntropySet = getCalculatedEntropyV2(mTotalSG, mTotalS1, mTotalS2, mTotalS3);

            TreeAttribute bestAttribute = getBestAttribute(samples, attributes);

            if(bestAttribute == null)
                return new TreeNode(new OutcomeTreeAttribute(getMostCommonValue(samples, targetAttribute)));

            TreeNode root = new TreeNode(bestAttribute);

            if (bestAttribute == null)
                return root;

            DataTable aSample =  new DataTable();
            aSample.Merge(samples);

            foreach (string value in bestAttribute.PossibleValues)
            {
                // Seleciona todas os elementos com o valor deste atributo				
                aSample.Rows.Clear();

                DataRow[] rows = samples.Select(bestAttribute.AttributeName + " = " + "'" + value + "'");

                if (rows.Length == 0)
                {
                    string att = bestAttribute.AttributeName;
                    root.AddTreeNode(new TreeNode(new OutcomeTreeAttribute(getMostCommonValue(samples, targetAttribute))), value);
                }

                else
                {
                    foreach (DataRow row in rows)
                    {
                        aSample.Rows.Add(row.ItemArray);
                    }


                    // Seleciona todas os elementos com o valor deste atributo				

                    // Cria uma nova lista de atributos menos o atributo corrente que é o melhor atributo		
                    TreeAttributeCollection aAttributes = new TreeAttributeCollection();
                    //ArrayList aAttributes = new ArrayList(attributes.Count - 1);
                    for (int i = 0; i < attributes.Count; i++)
                    {
                        if (attributes[i].AttributeName != bestAttribute.AttributeName)
                            aAttributes.Add(attributes[i]);
                    }
                    // Cria uma nova lista de atributos menos o atributo corrente que é o melhor atributo

                    // if (aSample.Rows.Count == 0)
                    //{
                    //  return new TreeNode(new OutcomeTreeAttribute(getMostCommonValue(aSample, targetAttribute)));
                    //}
                    //else
                    //{
                    DecisionTree dc3 = new DecisionTree();
                    TreeNode ChildNode = dc3.mountTree(aSample, targetAttribute, aAttributes);
                    root.AddTreeNode(ChildNode, value);
                  }
                }
            //}

            return root;
        }


        /// <summary>
        /// Monta uma árvore de decisão baseado nas amostragens apresentadas
        /// </summary>
        /// <param name="samples">Tabela com as amostragens que serão apresentadas para a montagem da árvore</param>
        /// <param name="targetAttribute">Nome da coluna da tabela que possue o valor true ou false para 
        /// validar ou não uma amostragem</param>
        /// <returns>A raiz da árvore de decisão montada</returns></returns?>
        public TreeNode mountTree(DataTable samples, string targetAttribute, TreeAttributeCollection attributes)
        {
            _sampleData = samples;
            mTargetAttribute = targetAttribute;
            return internalMountTree(_sampleData, targetAttribute, attributes);
        }
    }

    /// <summary>
    /// Classe que exemplifica a utilização do ID3
    /// </summary>
    public class DecisionTreeImplementation
    {

        string _sourceFile;

        public TreeNode GetTree(DataTable table)
        {
           // _sourceFile = sourceFile;
            RawDataSource samples = new RawDataSource(table);

            TreeAttributeCollection attributes = samples.GetValidAttributeCollection();

            DecisionTree id3 = new DecisionTree();
            TreeNode root = id3.mountTree(samples, "StrategyMay", attributes);



            System.IO.StreamWriter file = new System.IO.StreamWriter("TreeOutput2.txt");

            string treeout = PrintNode(root, "");
            file.Write(treeout);
            file.Close();

            return root;

        }
        public string PrintNode(TreeNode root, string tabs)
        {
            string returnString = String.Empty;
            string prefix = "Best Attribute: ";

            if (tabs != String.Empty)
                prefix = " -> Likely Outcome: ";
            returnString += (tabs + prefix + root.Attribute) + Environment.NewLine;

            if (root != null && root.Attribute != null && root.Attribute.PossibleValues != null)
            {
                for (int i = 0; i < root.Attribute.PossibleValues.Count; i++)
                {
                    returnString += (Environment.NewLine + tabs + "\t" + "Input:  " + root.Attribute.PossibleValues[i]) + Environment.NewLine;
                    TreeNode childNode = root.GetChildByBranchName(root.Attribute.PossibleValues[i]);
                    returnString += PrintNode(childNode, "\t" + tabs);
                }
            }

            return returnString;
        }

    }
}
