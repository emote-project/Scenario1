import cli.System.Console;
import cli.JavaExampleProxy.*;
//import cli.SimpleProject.*;

public class JavaExampleClient {

	static JavaExampleProxyClient proxy;
    public static void main(String[] args) {
	    proxy = new JavaExampleProxyClient();
	    //MyClass client = new MyClass();
	    cli.System.Console.WriteLine("Greetings from Java to .NET world!");
		
		proxy.add_PerceptionClick(new PerceptionClickHandler( new PerceptionClickHandler.Method() {
           public void Invoke(int x, int y) { PerceptionClick(x, y);}
        }));

    }
	
	public static void PerceptionClick(int x, int y) {
		System.out.format("Got a Perception Click in Java with parameters (x:%d, y:%d)", x, y);
		System.out.println(proxy);
		proxy.ActionPublisher.Speak("", "Speaking from Java");
	}
}

