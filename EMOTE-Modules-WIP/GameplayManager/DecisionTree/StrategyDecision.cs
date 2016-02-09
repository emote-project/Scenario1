using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class StrategyDecision
    {
        private TreeNode _decisionTree;
        private Dictionary<string, Categorization> _categories = new Dictionary<string, Categorization>();

        public StrategyDecision(TreeNode tree, Dictionary<string, Categorization> categories)
        {
            _decisionTree = tree;
            _categories = categories;
        }

        public string getStrategy(DataRow row)
        {
            return Search(row, _decisionTree);
        }

        public string Search(DataRow row, TreeNode node)
        {
            string strategy = String.Empty; //declare the final strategy
            string attribute = node.Attribute.AttributeName; //name of the best attribute
            int newvalue;
            int value = Convert.ToInt32(row[attribute].ToString()); //raw value of the given attribute, not categorized
            // categorize to all columns except "Level"
            if (!(attribute.Equals("Level")))
            {
                Categorization cat = _categories[attribute];
                newvalue = cat.NewValue(value);
            }
            else
                newvalue = value;
            TreeNode son = node.GetChildByBranchName(Convert.ToString(newvalue));

            if (son.Attribute is OutcomeTreeAttribute)
            {
                Console.WriteLine(attribute + " value: " + value + " newvalue: " + newvalue + " " + son.Attribute);
                strategy += son.Attribute;
            }
            else
            {
                Console.WriteLine(attribute + " value: " + value + " newvalue: " + newvalue + " " + son.Attribute);
                strategy = Search(row, son);
            }
            return strategy;
        }
    }
}
