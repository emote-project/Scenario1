using CaseBasedController.Forms;
using CaseBasedController.Simulation;
using CaseBasedController.Thalamus;
using PS.Utilities.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WekaWrapper;
using CaseBasedController.Classifier;

namespace CaseBasedController
{
    public class MainController
    {
        static List<Form> openedForms = new List<Form>();

        ControllerClient _client;
        CasePool _casePool;
        ClassifierController _classifierController;
        FeaturesCollector _fc;
        string _loadedCasePoolPath;

        public static bool IsClassifierEnabled { get; private set; }
        


        public MainController(string character)
        {
            // ====================================== KEEP UPDATED THE CASE POOL FILES
#if DEBUG
            Programs.CasePoolCodingProgram.EnercitiesDemo().SerializeToJson(@"..\..\..\Tests\EnercitiesDemo.json");
            Programs.CasePoolCodingProgram.EnercitiesDemoEmpathic().SerializeToJson(@"..\..\..\Tests\EnercitiesDemoEmpathic.json");
            Programs.CasePoolCodingProgram.MLPool().SerializeToJson(@"..\..\..\Tests\MLPool.json");
            Programs.CasePoolCodingProgram.TestPool().SerializeToJson(@"..\..\..\Tests\Test.json");
#endif

            // ====================================== INITIALIZES THALAMUS CLIENT
            _client = new ControllerClient(character);

            // ====================================== INITIALIZES TIMER
            MyTimer.UseWithRealTime();

            // ====================================== INITIALIZES THE CASEPOOL 
            string lastLoadedCasePoolPath = Properties.Settings.Default.LastLoadedCasePoolPath;
            if (lastLoadedCasePoolPath != null && System.IO.File.Exists(lastLoadedCasePoolPath))
                _casePool = LoadCasePool(lastLoadedCasePoolPath);
            //Task.Run( () => {
            //    Task.Delay(1000);
            //    LoadClassifierAsync(@"..\..\..\Tests\clean-data.arff", @"..\..\..\Tests\MLPool.json");
            //});

            // ====================================== INITIALIZES THE FEATURES COLLECTOR
            _fc = CreateFeaturesCollector(_casePool);

            

            // ====================================== SHOWS GUI
            //if (showGUI)
            //{
            //ExcelUtil.EnableGraphics = true;
            //    Application.EnableVisualStyles();
            //    Application.SetCompatibleTextRenderingDefault(false);
            //    MainForm mainForm = new MainForm(this);
            //    openedForms.Add(mainForm);
            //    Application.Run(mainForm);
            //}
        }

        /// <summary>
        /// Constructor used for batch executions. It executes a simulation and creates an ARFF based on the casePool and the logFile
        /// </summary>
        /// <param name="character"></param>
        /// <param name="casePool"></param>
        /// <param name="logFilePath"></param>
        public MainController(string character, string casePoolPath, string logFilePath)
        {
            MyTimer.UseWithSimulationTime();
            if (!System.IO.File.Exists(casePoolPath))
            {
                Console.WriteLine("File not found! " + casePoolPath);
                return;
            }
            if (!System.IO.File.Exists(logFilePath))
            {
                Console.WriteLine("File not found! " + logFilePath);
                return;
            }
            CasePool casePool = LoadCasePool(casePoolPath);
            System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
        }


        public void ShowMainGUI()
        {
            MainForm mainForm = new MainForm(this);
            openedForms.Add(mainForm);
            mainForm.Show();
        }

        public void Dispose()
        {
            _client.Dispose();
            foreach (Form f in openedForms) f.Close();
        }



        public CasePool LoadCasePool(string path)
        {
            CasePool casePool;
            var mainForm = ((MainForm)openedForms.Find(f => f is MainForm));
            try
            {
                casePool = CasePool.DeserializeFromJson(path);
                casePool.Init(_client, _client.ControllerPublisher);
                if (mainForm != null) mainForm.UpdateCasePool();
                _loadedCasePoolPath = path;

                Properties.Settings.Default.LastLoadedCasePoolPath = path;
                Properties.Settings.Default.Save();

                return casePool;
            }
            catch (System.IO.FileNotFoundException e)
            {
                MessageBox.Show("Can't load file " + System.IO.Path.GetFullPath(path), "Error loading CasePool file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        public CasePool ReLoadCasePool()
        {
            if (_loadedCasePoolPath != null)
            {
                return LoadCasePool(_loadedCasePoolPath);
            }
            else
            {
                throw new Exception("No case pool previously loaded");
            }
        }

        public async Task<bool> LoadClassifierAsync(string classifierPath, string casePoolPath)
        {
            _classifierController = new ClassifierController(_client, classifierPath, casePoolPath);
            var ret = await _classifierController.LoadAsync();

            ClassifierForm form = openedForms.Find(x => x is ClassifierForm) as ClassifierForm;
            if (form == null)
            {
                form = new ClassifierForm();
                openedForms.Add(form);
            }
            form.Show();
            form.Init(_classifierController);
            return ret;
        }



        #region Getter and Setters

        public void SetCasePool(CasePool casePool)
        {
            if (_casePool != null)
                _casePool.Dispose();
            _casePool = casePool;
            var mainForm = ((MainForm)openedForms.Find(f => f is MainForm));
            if (mainForm != null) mainForm.UpdateCasePool();
        }

        public CasePool GetCasePool()
        {
            return _casePool;
        }
        public void InitCasePool()
        {
            _casePool.Init(_client, _client.ControllerPublisher);
        }

        public ControllerClient GetClient()
        {
            return _client;
        }


        #endregion

        #region Helpers

        static public FeaturesCollector CreateFeaturesCollector(CasePool cp)
        {
            if (cp == null) return null;
            List<string> featuresNames = new List<string>();
            foreach (Case c in cp)
            {
                featuresNames.Add(c.Description);
            }
            featuresNames.Add(FeaturesCollector.CLASS_TO_LEARN_NAME);
            var fc = new FeaturesCollector(featuresNames);
            cp.FeaturesCollector = fc;
            return fc;
        }

        /// <summary>
        /// when the classifier is enabled, its results will be resolved into actions
        /// </summary>
        /// <param name="val">the "enabled" state of the classifier</param>
        public static void UseClassifier(bool val)
        {
            IsClassifierEnabled = val;
            ClassifierForm cf = openedForms.Find(f => f is ClassifierForm) as ClassifierForm;
            if (cf != null)
            {
                cf.Invoke(new Action(() =>
                {
                    cf.UpdateState();
                }));
            }
        }

        #endregion


        public void EnableMainForm(bool enabled)
        {
            MainForm mf = openedForms.Find(f => f is MainForm) as MainForm;
            mf.Invoke(new Action(() => {
                mf.Enabled = enabled;
            }));
        }

        public static void Exit()
        {
            Environment.Exit(0);
        }

    }
}
