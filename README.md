# LATech IOT azure cloud secure storage proof of concept

I am sorry if you are picking up this project from a linux / mac only environment.

## Technologies employed asp.net, transact sql, azure, python, restful api

the controllers for logic are in /Controllers/

asp.net this web application is deployed via iis, and I used visual studio to make this code

transact sql as the backend, ssms is what I used to connect to it.
To create the database and drop it easily look in the /db/ folder

Azure this web application was developed with the azure platform in mind, however this can be used in an entirely local manner for development purposes, just remember to change the database on azure when you change it locally.

python is used to test the webservice located in /extra/
pip install requests will be required to use any of the python there.

all configuration information is stored in Web.config, Web.Debug.config, and Web.Release.Config, however .Debug.config is essentially unused as it was giving me problems
