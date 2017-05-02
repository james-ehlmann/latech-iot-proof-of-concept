# LATech IOT azure cloud secure storage proof of concept

I am sorry if you are picking up this project from a linux / mac only environment.

## Technologies employed 
* asp.net 
* transact sql 
* azure 
* python
* restful api (json)

the controllers for logic are in /Controllers/

asp.net this web application is deployed via iis, and I used visual studio to make this code

transact sql as the backend, ssms is what I used to connect to it.
To create the database and drop it easily look in the /db/ folder

Azure this web application was developed with the azure platform in mind, however this can be used in an entirely local manner for development purposes, just remember to change the database on azure when you change it locally.

python is used to test the webservice located in /extra/
pip install requests will be required to use any of the python there.
pip install sqlalchemy will be required if you are going to push anything from a local database

all configuration information is stored in Web.config, Web.Debug.config, and Web.Release.Config, however .Debug.config is essentially unused as it was giving me problems

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
* The endpoint is configured to only accept json, if you want to disable this go to: / 

## Pushing to Azure / Setting up a new cloud instance
