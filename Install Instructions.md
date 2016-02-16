Installing EMOTE modules on a new PC

This document describes the process of installing the full EMOTE system on a new Windows PC. Note that the computer should be running Windows 7 or higher, and should ideally have sufficient memory (8GB or more is best)

Software to install
Tortoise HG (from http://tortoisehg.bitbucket.org/)
Microsoft Visual Studio 2012
An appropriate version of Python Tools for Visual Studio (http://pytools.codeplex.com/)
Java 1.7

Only for Scenario 1
Spring Tool Suite (from http://spring.io/tools/sts/all)
GitHub client for Windows (from https://github.com/) 
Apache Tomcat 7.0 from http://tomcat.apache.org/download-70.cgi
MySQL Community Server (http://dev.mysql.com/downloads/)  

Compiling
Building Thalamus
Open Thalamus\Thalamus.sln in Visual Studio
Right click on the top-level project and choose “Configuration Manager”, then set the active configuration to “Release”
Right click on the “GBML” project and choose “Build”
Open the “Thalamus” project and the “References” item, and remove the (broken) GBML reference
Right click on References and choose “Add Reference …”
Browse to GBML\obj\x64\Release and choose GBML.dll
Right click on the top-level Thalamus object and choose “Rebuild solution”
Right click on the top-level project and choose “Configuration Manager”, then set the active configuration to “Release”
Right click on the top-level Thalamus object and choose “Rebuild solution”

Building C# modules
All of the necessary modules are included in EMOTE-Modules-WIP. In general, to build each, you need to open the folder, open the .sln file in Visual Studio, change the configuration to Release with the Configuration Manager, and then choose Rebuild solution. Any deviations from this process are noted below. Note that in general the ordering does not matter, but EmoteEvents must always be rebuilt first.
EmoteEvents
Building Java modules
First, download and install SpringTools (link is above) and extract the zip somewhere. Then:
Open STS and tick the box for default workspace
Import the following three projects into the workspace (using “File - Import - General - Existing projects into workspace”):
…\EMOTE\EmoteMaps
…\EMOTE-Modules-WIP\interaction-manager-java
…\EMOTE\XSDM\XSDM
Make the following modifications to the EmoteMaps project:
Project > Properties > builders, remove the builder with red cross and the two “Validation” builders
Project > properties > java build path > select the "source" tab 

Select and click remove on the item.
Add folders:
/src/main/java
/src/main/resources
In the libraries tab, add the following jar to the buildpath.
EmoteMaps\src\main\webapp\WEB-INF\lib\jnaoqi-1.14.3.jar
EmoteMaps\src\main\webapp\WEB-INF\lib\xmlrpc-1.1.1.jar
EmoteMaps\src\main\webapp\WEB-INF\lib\xmlrpc-client-1.1.1.jar
EmoteMaps\src\main\webapp\WEB-INF\lib\ctat.jar
Right click on project > Maven > Update project
Right click on project > Maven > Download sources
Right click the “interaction-manager-java” project and run “Clean”


