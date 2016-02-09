using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.IO;


namespace DecisionTree
{
    public class Categorization
    {
        private System.Data.DataTable _datatable;
        private int _total;
        private string _name;
        private List<List<int>> _categories;
        private List<int[]> _finalCategories;

        public Categorization(System.Data.DataTable datatable, string name, int div)
        {
            _datatable = datatable;
            _total = getTotal();
            _name = name;
            this.DividingCat(div);
            this.FinalCategories();
        }

        public Categorization(List<List<int>> manualCat, string name)
        {
            _name = name;
            _categories = manualCat;
            this.FinalCategories();

        }

        public Dictionary<string, int> CountDistValues(System.Data.DataTable dataTable, string columnName)
        {
            Dictionary<string, int> distValues = new Dictionary<string, int>();
            // Returns the score of each distinct value in a give column
            var query = dataTable.AsEnumerable().GroupBy(l => l[columnName]).Select(g => new
            {
                Value = g.Key,
                Ct = g.Count()
            });

            foreach (var q in query)
                distValues.Add(q.Value.ToString(), Convert.ToInt32(q.Ct));

            return distValues;
        }

        public string  MaxFrequency(Dictionary<string, int> query)
        {
            int value = 0;
            string max = "";
            foreach (var q in query)
            {
                if (value < q.Value)
                {
                    value = q.Value;
                    max = q.Key;
                }
            }
            return max;
        }

        /*Returns a list with all elements separated into categories, these elements are presented in a list (list of lists)*/
        public void DividingCat(int cat) 
        {
            List<List<int>> categories = new List<List<int>>();
            int valuesCat = _total / cat;
            int rest = _total % cat;
            List<int> interval = new List<int>();
            int c = 0;
            
            for (int i = 0; i < _datatable.Rows.Count; i++)
            {
                int numbervalues = Convert.ToInt32(_datatable.Rows[i]["Ct"]); //number of times of ditinct value i
                    int value = Convert.ToInt32(_datatable.Rows[i]["Value"]);

                /*Verifies if current category isn't full*/
                if (interval.Count < valuesCat)/*first analyzes if there is room for that category*/
                {
                    //tests if there is room for all elements of a number in the current category
                    if ((interval.Count + numbervalues) <= valuesCat)
                    {
                        for (int j = 0; j < numbervalues; j++)
                        {
                            interval.Add(value);
                            c++;
                        }
                    }
                    else //When the current category isn't full but there isn't room for all elements
                    {
                        //use the extra room when "rest" isn't zero
                        if ((interval.Count + numbervalues) < (valuesCat + rest))
                        {
                            for (int x = 1; x <= rest; x++)
                            {
                                int auxValuesCat = valuesCat + x;
                            
                                if ((interval.Count + numbervalues) <= auxValuesCat)
                                {
                                    for (int j = 0; j < numbervalues; j++)
                                    {
                                        interval.Add(value);
                                        c++;
                                    }
                                    rest -= x;
                                    break; //stops when there is enough space
                                }
                            }
                        }
                        else /*when isn't possible to add the elements to the current category, even if it isn't complete*/
                        {
                            List<int> copy = new List<int>(interval);
                            categories.Add(copy);
                            interval = new List<int>(); //creates a new category

                            for (int j = 0; j < numbervalues; j++)
                            {
                                interval.Add(value);
                                c++;
                            }
                        }
                    }

                }
                else /*When the current category is full*/
                {
                    List<int> copy = new List<int>(interval);
                    categories.Add(copy);
                    interval = new List<int>(); //creates a new category

                    for (int j = 0; j < numbervalues; j++)
                    {
                        interval.Add(value);
                        c++;
                    }
                }
                if( i == (_datatable.Rows.Count - 1))
                {
                    if(rest > 0) /*falta testar a consistencia entre os valores*/
                    {

                        if (categories.Last().Count < (valuesCat + rest))
                        {
                            List<int> copy = new List<int>(interval);
                            foreach(int num in copy)
                            {
                                //if(rest > 0)
                                //{
                                    categories.Last().Add(num);
                                    interval.Remove(num);
                                    rest--;
                                //}
                                //else
                                  //  break;
                            }
                        }
                    }
                    if (interval.Any())
                        categories.Add(interval);
                }
            }
            _categories = categories;
        }

        /*Final lis of all categories, returns a list with the intervals of each category*/
        public void FinalCategories() 
        {
            List<int[]> finalCategories = new List<int[]>();

            foreach (List<int> l in _categories)
            {
                //Console.WriteLine(l[0] + "  " + l.Last());
                int[] cat = { l[0], l.Last() };
                finalCategories.Add(cat);
            }

            _finalCategories = finalCategories;
        }

        public int NewValue(int value)
        {
            int cat = 0;
            for(int i = 0; i < _finalCategories.Count; i++)
            {
                int[] aux = _finalCategories[i];
                if (value <= aux[0] || value <= aux[1])
                {
                    cat = i;
                    break;
                }
            }
            return cat;
        }

        public int getTotal() 
        {
            int total = 0;
            foreach (DataRow r in _datatable.Rows) 
            {
                total += Convert.ToInt32(r["Ct"]);
            }

            return total;
        }

        public double valuesPerCat(int cat) 
        {
            return _total / cat;
        }
    }
}
