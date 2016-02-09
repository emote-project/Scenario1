using CaseBasedController.Detection;
using CaseBasedController.Thalamus;

namespace CaseBasedController.Behavior
{
    public abstract class BaseBehavior : IBehavior
    {
        protected readonly object locker = new object();
        protected IAllActionPublisher actionPublisher;
        protected IAllPerceptionClient perceptionClient;

        #region IBehavior Members

        public int Priority { get; set; }

        public event ExecutedEventHandler ExecutionFinished;

        public abstract void Dispose();
        public abstract void Execute(IFeatureDetector detector);
        public abstract void Cancel();

        public virtual void Init(IAllActionPublisher publisher, IAllPerceptionClient client)
        {
            this.perceptionClient = client;
            this.actionPublisher = publisher;
            AttachPerceptionEvents();
        }

        #endregion

        protected abstract void AttachPerceptionEvents();

        protected void RaiseFinishedEvent(IFeatureDetector detector = null)
        {
            if (this.ExecutionFinished != null)
                this.ExecutionFinished(this, detector);
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}