using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using System.Reflection;

namespace JavaExampleProxy
{
	#region Delegates for Java

	public delegate void PerceptionClickHandler(int x, int y);
    public delegate void PerceptionWaypointCreatedHandler(string name, int x, int y);
    public delegate void PerceptionSetZoomLevelFinishedHandler();
    public delegate void PerceptionPanMapToPointFinishedHandler();

	#endregion


	public interface IJavaTestEvents : Thalamus.IPerception
	{
		//this defines a collection of perceptions known as IJavaTestEvents
		void JavaTestEvent1(string paramA, int paramB);
		void JavaTestEvent2(string paramA, int paramB);
	}


	public interface IJavaExampleProxyClient :
		/*This interface defines what your client subscribes to. 
	Your client will automatically subscribe to all the perceptions and actions defined in the interfaces you inherit from
	As a "rule of thumb", I call interfaces of perceptions as "ISomethingEvents", and interfaces of actions as "ISomethingActions"
	They work exactly the same. They are only different so that it is easier for us to define and think about them.
	In this case your client subscribes to 7 perceptions.
	Those are:
		ThalamusInterface.IJavaTestEvents.JavaTestEvent1
		ThalamusInterface.IJavaTestEvents.JavaTestEvent2
		ExampleClientsInterface.IMapEvents.Click
		ExampleClientsInterface.IMapEvents.Zoom
		ExampleClientsInterface.IMapEvents.CreatedWayPoint
		ExampleClientsInterface.IMapEvents.ZoomStarted
		ExampleClientsInterface.IMapEvents.ZoomFinished
	The first two are from the interface we defined in this file
	The other five are defined in the ExampleClientsInterface, you can just go to the definition to find them
	It is useful to define common interfaces in a different project so that different clients can reference that project and use the same messages
	I actually took the ExampleClientsInterface directly from the MapExample in the Thalamus/ExampleClients folder
	*/
		IJavaTestEvents,
		ExampleClientsInterface.IMapEvents
	{}


	public interface IJavaExampleProxyPublisher : IThalamusPublisher,
	/* In order to publish events, you also need to define another interface that inherits from IThalamusPublisher and also all the interfaces that contain the actions you want to publish
	 * In this case, your client publishes all the actions from the ExampleClientsInterface.IMapActions and the Thalamus.BML.ISpeakActions interfaces
	 * If you are calling actions for expression of the character you should use the already defined interfaces that are in the Thalamus.BML namespace
	 * To access the Thalamus.BML namespace, you must add a reference to the ThalamusBMLInterfaces.dll assembly that you can find in Thalamus/Binaries/Release
	 * */
		ExampleClientsInterface.IMapActions,
		Thalamus.BML.ISpeakActions
	{}


	//Just add the IJavaInterfaceClient interface to this class (the one that contains the subscribed messages).
	//Then you just need to right click on IJavaInterfaceClient and select Refactor->Implement Interface Explicit
	//The IDE will automatically create all the functions that are called whenever you receive each of the subscribed events
    public class JavaExampleProxyClient : ThalamusClient, IJavaExampleProxyClient
    {

		public class JavaExampleProxyPublisher: IJavaExampleProxyPublisher 
		{
			//This class is used to make it easier to publish events.
			//You can do it without using this class, but it is better practice if you define it, so just assume that you must.
			// Please refer to the MapExample/ExampleMapClient for explanation on this class
			dynamic publisher;
			public JavaExampleProxyPublisher(dynamic publisher) {
				this.publisher = publisher;
			}

			//This class just acts as a proxy.
			//It created a definition of the functions that call each of the actions you will be publishing, so that it is easier to call the actions
			//Each function just calls "itself", in the "publisher" object, which is a "dynamic" object.
			//I won't be explaining dynamic objects now because it is not necessary in order to use the classes
			#region ISpeakActions implementation

			public void Speak (string id, string text)
			{
				publisher.Speak (id, text);
			}

			public void SpeakBookmarks (string id, string[] text, string[] bookmarks)
			{
				publisher.SpeakBookmarks (id, text, bookmarks);
			}

			#endregion

			#region IMapActions implementation

			public void Zoom (double zoomFactor)
			{
				publisher.Zoom (zoomFactor);
			}

			public void CreateWaypoint (string id, int x, int y)
			{
				publisher.CreateWaypoint (id, x, y);
			}

			#endregion
		}

		//this object will be used to call action in other modules
		//In java you just need to call for example ActionPublisher.Speak("","Hello world") to call the Speak action.
		//Just try it out, I can't try it here.
		public JavaExampleProxyPublisher ActionPublisher;

		public JavaExampleProxyClient()
            : base("JavaExampleProxy")
        {
			//This is generic and must be done in every client that uses a publisher.
			SetPublisher<IJavaExampleProxyPublisher> ();
			ActionPublisher = new JavaExampleProxyPublisher (Publisher);
        }

		#region Events used to notify java of the incomming events

        public event PerceptionClickHandler PerceptionClick;
        public void NotifyPerceptionClick(int x, int y)
        {
            if (PerceptionClick != null)
                PerceptionClick(x, y);
        }

        public event PerceptionWaypointCreatedHandler PerceptionWaypointCreated;
        public void NotifyPerceptionWaypointCreated(string name, int x, int y)
        {
            if (PerceptionWaypointCreated != null)
                PerceptionWaypointCreated(name, x, y);
        }

        public event PerceptionSetZoomLevelFinishedHandler PerceptionSetZoomLevelFinished;
        public void NotifyPerceptionSetZoomLevelFinished()
        {
            if (PerceptionSetZoomLevelFinished != null)
                PerceptionSetZoomLevelFinished();
        }

		public event PerceptionPanMapToPointFinishedHandler PerceptionPanMapToPointFinished;
		public void NotifyPerceptionPanMapToPointFinished()
		{
			if (PerceptionPanMapToPointFinished != null)
				PerceptionPanMapToPointFinished();
		}
		#endregion

		#region IMapEvents implementation

		void ExampleClientsInterface.IMapEvents.Click (int x, int y)
		{
			//perceptionstring = "Click X " + perception.Parameters["x"].GetInt().ToString() + " Y " + perception.Parameters["y"].GetInt().ToString();
			//Console.WriteLine("i got click on thalamus: " + perceptionstring);
			NotifyPerceptionClick(x, y);
		}

		void ExampleClientsInterface.IMapEvents.Zoom (double zoomFactor)
		{
			//do things (in this case you would just call the Event)
		}

		void ExampleClientsInterface.IMapEvents.CreatedWayPoint (string id, int x, int y)
		{
			NotifyPerceptionWaypointCreated(id, x, y);

			//just to show how to use the publisher
			ActionPublisher.Speak ("", "Hey that's a great place for a waypoint!");
		}

		void ExampleClientsInterface.IMapEvents.ZoomStarted ()
		{
			//do things (in this case you would just call the Event)
		}

		void ExampleClientsInterface.IMapEvents.ZoomFinished ()
		{
			NotifyPerceptionSetZoomLevelFinished();
		}

		#endregion

		#region IJavaTestEvents implementation

		void IJavaTestEvents.JavaTestEvent1 (string paramA, int paramB)
		{
			//do things
		}

		void IJavaTestEvents.JavaTestEvent2 (string paramA, int paramB)
		{
			//do things
		}

		#endregion

    }
}


