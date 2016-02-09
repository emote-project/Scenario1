using EmoteEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnercitiesThermometer
{
    class GameQualityEvaluator
    {
        public EnercitiesThermometer.EnercitiesThermometerClient enercitiesThermometerClient;

        public GameQualityEvaluator(EnercitiesThermometer.EnercitiesThermometerClient enercitiesThermometerClient)
        {
            this.enercitiesThermometerClient = enercitiesThermometerClient;
        }


        public event EventHandler<NegativeScoreAlertEventArgs> AlertNegativeScores;
        public class NegativeScoreAlertEventArgs : EventArgs
        {
            public EmoteEnercitiesMessages.EnercitiesRole role;
            public double score;
            public NegativeScoreAlertEventArgs(EmoteEnercitiesMessages.EnercitiesRole role, double score){
                this.role = role;
                this.score = score;
            }
        }

        const double MAX_SD_VALUE = 20;
        const double INITIAL_OIL_AMOUNT = 1800;
        private List<EnercitiesGameInfo> gameStates = new List<EnercitiesGameInfo>();

        double negativeScoresPenality;
        double resourcesPenality;
        double scoresDifferencePenality;
        double totalQuality;

        /// <summary>
        /// Returns the evaluation about the quality of the game related to the current and past game states.
        /// </summary>
        /// <param name="serializedGamestate">current game state</param>
        /// <returns>a value ranging from 0 to 100 where 0 is really bad and 100 is really well</returns>
        public double GetNewEvaluation(string serializedGamestate)
        {
            EnercitiesGameInfo lastState = EnercitiesGameInfo.DeserializeFromJson(serializedGamestate);
            gameStates.Add(lastState);

            // Standan deviation of the 3 scores
            double av = (lastState.WellbeingScore+lastState.EconomyScore+lastState.EnvironmentScore)/3;
            double sd = Math.Sqrt(Math.Pow(lastState.WellbeingScore - av, 2) + Math.Pow(lastState.EconomyScore - av, 2) + Math.Pow(lastState.EnvironmentScore - av, 2) / 3);
            scoresDifferencePenality = (100 * sd / MAX_SD_VALUE);

            // Negative scores
            negativeScoresPenality = lastState.WellbeingScore < 0 ? lastState.WellbeingScore : 0;
            negativeScoresPenality += lastState.EnvironmentScore < 0 ? lastState.EnvironmentScore : 0;
            negativeScoresPenality += lastState.EconomyScore < 0 ? lastState.EconomyScore : 0;
            negativeScoresPenality += negativeScoresPenality > 0 ? 50 : 0;

            // Evaluation of the resources
            resourcesPenality = inExp(INITIAL_OIL_AMOUNT - lastState.Oil, INITIAL_OIL_AMOUNT);  // Oil starts at 1800. The penality grows exponentially so that it will penalize more when a big amount of oil got consumed
            resourcesPenality += Math.Abs(lastState.PowerProduction-lastState.PowerConsumption);      // The energy penality is due by the difference between prodction or consuption. The penality is high if either we produce a lot more or a lot less than we need.
            resourcesPenality += lastState.Money < 0 ? 50+lastState.Money : 0;                        // The money penality simply equals the negative amount of money the city has, if any.

          
           
            totalQuality = 100 - (scoresDifferencePenality + negativeScoresPenality + resourcesPenality);

             LearnerStateInfo lsi = new LearnerStateInfo(1, 1, 1, 2, 1, true, "000");

             if (totalQuality > 90)
             {
                 lsi.mCompetencyItems.Add(new LearnerStateInfo.CompetencyItem("balance", true, "", "", 0, 0, 0, 0, EmoteCommonMessages.CompetencyType.impact, EmoteCommonMessages.Impact.Positive));
             }
             else
             {
                 lsi.mCompetencyItems.Add(new LearnerStateInfo.CompetencyItem("balance", true, "", "", 0, 0, 0, 0, EmoteCommonMessages.CompetencyType.impact, EmoteCommonMessages.Impact.Negative));
            
             }
            String LearnerStateInfo_learnerState = lsi.SerializeToJson();
            enercitiesThermometerClient.ETPublisher.learnerModelUpdate(LearnerStateInfo_learnerState);



            AlertsCheck();

            Console.WriteLine("Global Score: " + lastState.GlobalScore + "  ScoreDifferencePen: " + scoresDifferencePenality + "  resourcesPen: " + resourcesPenality + "  quality: " + totalQuality);
            if (totalQuality < 0) totalQuality = 0;
            if (totalQuality > 100) totalQuality = 100;
            return totalQuality;
        }

        /// <summary>
        /// Resets the evaluator to a clean state (useful when restarting the game)
        /// </summary>
        public void Reset(){
            gameStates.Clear();
        }

        double inExp(double x, double max = 100, double min = 0)
        {
            if (x < min) return min;
            if (x > max) return max;
            return Math.Pow(2.0d, 10.0d * (x - max));
        }

        private void AlertsCheck()
        {
            if (AlertNegativeScores!=null) {
                if (negativeScoresPenality > 50)
                {
                    EmoteEnercitiesMessages.EnercitiesRole role = EmoteEnercitiesMessages.EnercitiesRole.Mayor;

                    var economyScore = gameStates[gameStates.Count-1].EconomyScore;
                    if (economyScore<0) AlertNegativeScores(this, new NegativeScoreAlertEventArgs(EmoteEnercitiesMessages.EnercitiesRole.Economist,economyScore));

                    var environmentScore = gameStates[gameStates.Count-1].EnvironmentScore;
                    if (environmentScore<0) AlertNegativeScores(this, new NegativeScoreAlertEventArgs(EmoteEnercitiesMessages.EnercitiesRole.Environmentalist,environmentScore));

                    var wellbeingScore = gameStates[gameStates.Count-1].WellbeingScore;
                    if (wellbeingScore<0) AlertNegativeScores(this, new NegativeScoreAlertEventArgs(EmoteEnercitiesMessages.EnercitiesRole.Mayor,wellbeingScore));
                }
            }

            if (resourcesPenality > 50)
            {

            }
            if (scoresDifferencePenality > 50)
            {

            }
        }
    }
}
