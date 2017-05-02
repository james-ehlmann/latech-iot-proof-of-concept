# LATech IOT azure cloud secure storage proof of concept

I am sorry if you are picking up this project from a linux / mac only environment.

## Technologies employed 
* asp.net 
* transact sql 
* azure 
* python
* restful api (json)

## Prerequisites
To set this up locally you need to install the following things:
* Visual Studio 2017 for ASP.net / Azure (select those packages when installing) 
* SqlExpress (Microsoft SQL Server for localhost)
* SSMS (Microsoft SQL Server Manager)
* python
* pip
	* pip install requests
	* pip install sqlalchemy

## Running

### Database
1) Start sql express on localhost
2) Connect to it using ssms to make sure it's working
3) create a user iot with password iot
4) create a table called collected\_data and make sure that iot has db access to collected\_data
5) run /db/create.sql in a new query in collected\_data

### Website / API
1) Once you have these things you should use 'open project' in visual studio and select the git clone of this application .sln file
By hitting the run button at the top you can run the web application. 
	* Note this needs to be run in DEBUG mode, or else it will connect to azure

### Testing
* Run any of the python scripts in /extra/ on localhost and you should see the application working
* Any of these python scripts should have a help message displayed if you use --help
* Most of the time the url is specified by using python test.py http://url/to/webapplication/instance

## Developing

### Tests
* To create a test, it is recomended that you copy one of the basic tests in the python scripts

### Controllers / Endpoints
* The controller logic is stored in /Controllers/ this follows the basic api model shown in any of the controllers (look at the code)
* Most of the hard stuff is controlled by asp.net controller, and structs defined in the controllers are the best way to automatically accept the json
* The endpoint is configured to only accept json, if you want to disable this go to:
* The configuration for the databases is stored in the Web.config file and Web.config.release file.  

## Pushing to Azure / Setting up a new cloud instance
* Not written yet. 
