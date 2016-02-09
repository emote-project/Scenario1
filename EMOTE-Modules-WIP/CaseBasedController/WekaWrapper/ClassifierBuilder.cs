using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java;
using weka;
using weka.classifiers;
using weka.core;
using weka.core.converters;
using ikvm.extensions;

namespace WekaWrapper
{
    /// <summary>
    /// Builds weka classifiers
    /// </summary>
    public class WClassifier
    {
        Classifier _c;
        Instances _insts;

        /// <summary>
        /// Given an example arff file and a model, returns a classifier based on the model and on the attributes described in the arff file
        /// </summary>
        /// <param name="arffPath"></param>
        /// <param name="modelPath"></param>
        /// <returns></returns>
        public WClassifier(string arffPath, string modelPath)
        {
            if (!System.IO.File.Exists(modelPath))
                throw new System.IO.FileNotFoundException("Can't find file: " + modelPath);
            if (!System.IO.File.Exists(arffPath))
                throw new System.IO.FileNotFoundException("Can't find file: " + arffPath);

            java.io.InputStream f = new java.io.FileInputStream(modelPath);
            _c = (Classifier)SerializationHelper.read(f);

            var loader = new ArffLoader();
            loader.setFile(new java.io.File(arffPath));
            _insts = loader.getStructure();
            _insts.setClassIndex(_insts.numAttributes() - 1);
        }

        /// <summary>
        /// Classifies a features vector, returning the estimated class label 
        /// </summary>
        /// <param name="featuresVector"></param>
        /// <param name="featuresNames"></param>
        /// <returns></returns>
        public string Classify(string[] featuresVector, List<string> featuresNames)
        {
            if (_c != null)
            {
                Instance inst = new Instance(featuresVector.Length);   // -2 because i'm excluding the last value in the vector which is the class
                inst.setDataset(_insts);
                for (int i = 0; i < featuresVector.Length; i++ )
                {
                    var att = _insts.attribute(i);
                    if (att.isNumeric())
                    {
                        double val = 0;
                        Double.TryParse(featuresVector[i], out val);
                        inst.setValue(att, val);
                    }
                    else
                    {
                        inst.setValue(att, featuresVector[i]);
                    }
                }
                double labelID =  _c.classifyInstance(inst);
                string label = inst.classAttribute().value((int)labelID);
                return label;
            }
            else
            {
                throw new Exception("The classifier wasn't Initialized!");
            }
        }
    }
}
