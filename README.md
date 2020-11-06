# TheNugetPackages

## What is Nuget 

Nuget is a open source package management tool that can help to share/consume developers code.

## What is Nuget Package

Nuget package is a single zip file with .nupkg extension that contain compiled code as dll format. 
Otherway you can say a sharable unit of code.

![architecture](https://github.com/habibsql/TheNugetPackages/blob/master/docs/one.jpg?raw=true)

## Benifits

#### Main benifits are:

* Automatic dependency management/resulation.
* Easy include/exclude component reference to any project.
* Manage central repositories for both public and private host.
* Provide tools for easily create/publish packages.
* Capable to cache packages to the local and leter use from other projects as well.
* Inspire for modular software development.

## Installing Nuget packages

Before installing nuget package, your machine should have nuget CLI installed. If you already installed Visual Studio
it will automatically installed. Otherwise you need to install it first.

* Using CLI 

```
dotnet add package [package-id] -v [package-version]
```
if you not provided package version, it will install latest package version.

* You can also install packages uing visual studio nuget tool

## Private Repository

You can store your packages at your personal repositories so that none other than your company can access those codebases.

![repository](https://github.com/habibsql/TheNugetPackages/blob/master/docs/two.png?raw=true)



## Demonastrate few popular nuget packages that could help us alot to build ,NET core backend services.

#### Important Nuget Packages:

* **AWSSDK (Cloud Storage)**
   - Help to access to aws cloud.
   - Help to Upload/download files/folder to s3 bucket (current scope)

* **Twilio (Message Text)**
   - Help to send sms to the client's mobile number.
   - Help to send message to the whatsapp application.
   
* **Mailkit (Email)**
   - Sending Email
	
* **Charpt.Scirpting (Dynamic code execution)**
   - Execute/Efalute code/expression at runtime.
	
* **FluentFtp (Ftp Server)**
   - Upload/download file/folder from ftp server
	
* **abbitMQ (Message Broker)**
   - Publish/Consume message To/from RabbitMQ message broker
	
* Mongodb driver (NoSql Database)
   - Insert/update/delete/read data from MongoDb nosql database
   
* **NPOI (MS Excel)**
   - Read/Write/Generate MS Excel file.
   
* **Pdf (Pdf)**
   - Generate Pdf file.
	
* **Polly (Resilency)**
   - Retry/Circet braker for distributed service call.

* **Redis (Distributed Cache)**
   - Add/Delete/Read keys from/to Redis server
