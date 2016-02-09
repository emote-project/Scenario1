using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using Thalamus;

namespace UtteranceManager
{

    //subscribe
    public interface IUtteranceActions : EmoteCommonMessages.IFMLSpeech, Thalamus.BML.IAnimationActions
    { }

    //announce publishing
    public interface IUtterancePublisher : IThalamusPublisher,
        IUtteranceActions
    { }

    public class UtteranceClient : ThalamusClient //nao subscrebe a nada
    {
        public IUtterancePublisher UtterancePublisher;

        public UtteranceLibrary Library { get; private set; }

        public class UtteranceClientPublisher : IUtterancePublisher
        {
            dynamic publisher;
            public UtteranceClientPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void PlayAnimation(string id, string animation)
            {
                publisher.PlayAnimation(id, animation);
            }

            public void PlayAnimationQueued(string id, string animation)
            {
                publisher.PlayAnimationQueued(id, animation);
            }

            public void StopAnimation(string id)
            {
                publisher.StopAnimation(id);
            }


            public void ReplaceTagsAndPerform(string utterance, string category)
            {
                publisher.ReplaceTagsAndPerform(utterance, category);
            }

            public void CancelUtterance(string id)
            {
                publisher.CancelUtterance(id);
            }

            public void PerformUtterance(string id, string utterance, string category)
            {
                publisher.PerformUtterance(id, utterance, category);
            }

            public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
            {
                publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            }
        }

        public UtteranceClient(string character)
            : base("UtteranceClient", character)
        {
            SetPublisher<IUtterancePublisher>();
            UtterancePublisher = new UtteranceClientPublisher(Publisher);

            Library = new UtteranceLibrary();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
