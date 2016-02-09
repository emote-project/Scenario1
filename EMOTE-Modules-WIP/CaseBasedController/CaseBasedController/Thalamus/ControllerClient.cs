using Thalamus;

namespace CaseBasedController.Thalamus
{
    public partial class ControllerClient : ThalamusClient
    {
        public const string CLIENT_NAME = "EnercitiesCaseController";
        public const string CASE_POOL_FILE = "casepool.json";
        public const string DEFAULT_CHARACTER_NAME = "";

        public ControllerClient(string character = DEFAULT_CHARACTER_NAME)
            : base(CLIENT_NAME, character)
        {
            this.SetPublisher<IAllActionPublisher>();
            this.ControllerPublisher = new ControllerPublisher(this.Publisher);

            this.showDebugErrors = false;
        }

        /// <summary>
        ///     The <see cref="IThalamusPublisher" /> used by this client to publish actions to other modules.
        /// </summary>
        public IAllActionPublisher ControllerPublisher { get; private set; }

    }
}