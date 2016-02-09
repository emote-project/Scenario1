using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using System.Threading;
using EmoteEvents;

namespace FakeGameEvents
{
    internal interface IFakeGameEventsPublisher : IThalamusPublisher,
        EmoteEnercitiesMessages.IEnercitiesGameStateEvents
    { }

    public class FakeGameEventsClient : Thalamus.ThalamusClient
    {
        private class FakeGameEventsPublisher : IFakeGameEventsPublisher
        {
            dynamic publisher;
            public FakeGameEventsPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void EndGameNoOil(int totalScore)
            {
                throw new NotImplementedException();
            }

            public void EndGameSuccessfull(int totalScore)
            {
                throw new NotImplementedException();
            }

            public void EndGameTimeOut(int totalScore)
            {
                throw new NotImplementedException();
            }

            public void GameStarted(string player1name, string player1role, string player2name, string player2role)
            {
                throw new NotImplementedException();
            }

            public void ReachedNewLevel(int level)
            {
                throw new NotImplementedException();
            }

            public void TurnChanged(string serializedGameState)
            {
                publisher.TurnChanged(serializedGameState);
            }


            public void ResumeGame(string player1name, string player1role, string player2name, string player2role, string serializedGameState)
            {
                publisher.ResumeGame(player1name, player1role, player2name, player2role, serializedGameState);
            }

            public void StrategyGameMoves(string environmentalistMove, string economistMove, string mayorMove, string globalMove)
            {
                publisher.StrategyGameMoves(environmentalistMove, economistMove, mayorMove, globalMove);
            }


            public void PlayersGender(EmoteEnercitiesMessages.Gender player1gender, EmoteEnercitiesMessages.Gender player2gender)
            {
                publisher.PlayersGender(player1gender, player2gender);
            }


            public void PlayersGenderString(string player1gender, string player2gender)
            {
                publisher.PlayersGenderString(player1gender, player2gender);
            }
        }

        internal IFakeGameEventsPublisher ThalamusPublisher;

        public FakeGameEventsClient(string character = "")
            : base("FakeGameEventsClient", character)
        {

            SetPublisher<IFakeGameEventsPublisher>();
            ThalamusPublisher = new FakeGameEventsPublisher(Publisher);

            var p1 = EmoteEnercitiesMessages.EnercitiesRole.Environmentalist;
            var p2 = EmoteEnercitiesMessages.EnercitiesRole.Economist;
            var p3 = EmoteEnercitiesMessages.EnercitiesRole.Mayor;
            EmoteEnercitiesMessages.EnercitiesRole currentRole = p1;
            while (true)
            {
                Thread.Sleep(3000);
                EmoteEvents.EnercitiesGameInfo gstate = new EnercitiesGameInfo();
                gstate.CurrentRole = (currentRole == p1 ? p2 : currentRole == p2 ? p3 : p1);
                gstate.EconomyScore = new Random().Next(100);
                gstate.EnvironmentScore = new Random().Next(100);
                gstate.WellbeingScore = new Random().Next(100);
                gstate.Oil = new Random().Next(100);
                gstate.PowerProduction = new Random().Next(100);
                gstate.PowerConsumption = new Random().Next(100);
                gstate.Population = new Random().Next(100);
                Console.WriteLine("Sending: " + gstate.SerializeToJson());
                //ThalamusPublisher.TurnChanged(gstate.SerializeToJson());
                
            }
        }
    }

}
