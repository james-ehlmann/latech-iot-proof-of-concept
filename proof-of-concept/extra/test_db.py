from sqlalchemy import Column, DateTime, String, Integer, Float, ForeignKey, func
from sqlalchemy.orm import relationship, backref
from sqlalchemy.orm.collections import InstrumentedList
from sqlalchemy.ext.declarative import declarative_base
from datetime import datetime, timedelta
import json

import requests
import time
import sys
import os.path
import random

if '-h' in sys.argv or '--help' in sys.argv or len(sys.argv) < 2:
	print """
Usage: pythonbot.py URL_OF_WEBSERVICE
requires: requests, json, sqlalchemy
	pip install sqlalchemy
	pip install requests

creates these files: [fbData.sqlite, password.json] (for program memory*)
You can change the database that it is connecting to and how it connects to it, read the sqlalchemy documentation.
This is meant purely as an example, it is not designed for production code. However it can be extended to make production code

Should be set as cron using crontab -e with expression 0 0 0 1/1 * ? *
	"""
	sys.exit(0)
	
# Set up our global variables
url = sys.argv[1]

if(url.endswith("/")):
	pass
else:
	url += "/"
epoch = datetime.utcfromtimestamp(0)

print 'using url: ' + url

# load up our password and our id. 
# password can be stored on local device. 
password = ''
id = 0
if os.path.isfile('password.json'):
	pw_obj = json.load(open('password.json', 'r'))
	password = pw_obj['password']
	id = pw_obj['id']
else:
	init_request = requests.get(url + "api/Device/Request").json()
	password = init_request['password']
	id = init_request['id']
	json.dump(init_request, open('password.json', 'w'))

# Needed for sqlalchemy
Base = declarative_base()

# pretty good ways of making json objects for ourselves. 
# Actually I didn't use these, because it was complex =/
def to_json(obj):
	dic = {}
	manual_l = {}
	for attr, var in obj.__dict__.iteritems():
		if not attr.startswith('_'):
			if type(var) is datetime:
				dic[attr] = int((var - epoch).total_seconds())
			elif type(var) is InstrumentedList:
				manual_l[attr] = var
			else:
				dic[attr] = var
	ret = json.dumps(dic)
	for i in manual_l:
		manual_l[i] = to_json_l(manual_l[i])
	ret = ret[:-1] + ","
	for i in manual_l:
		ret += '"' + i + '":' + manual_l[i] + ","
	
	ret = ret[:-1] + "}"
	return ret 

def to_json_l(obj):
	ret = '['
	for i in obj:
		ret += to_json(i) + ","
	ret = ret[:-1] + ']'
	return ret

# Adds our device token to any jsonObjs we need to send to the server..
# Essentially functions like an API key. 
def addToken(jsonObj):
	jsonObj = jsonObj[:-1]
	jsonObj += ', "id": {"password":"' + password + '", "id":' + str(id) + '}}'
	return jsonObj

def sendRequest(payload, apiEnd):
	headers = {'content-type':'application/json'}
	return (url + apiEnd, requests.post(url + apiEnd, data=payload, headers = headers))


###############################################################################

# These tables should be imported from somewhere else
# That means another file, so that they don't clutter our codebase here. 
# However for this example purpose, they are here.
class user(Base):
	__tablename__ = "user"
	id = Column(Integer, primary_key=True)
	name = Column(String)
	fridgedays = relationship("fridgeDay", back_populates="user")
	fitbitdays = relationship("fbDay", back_populates="user")
	
	
class fridgeDay(Base):
	__tablename__ = 'fridge_day'
	id = Column(Integer, primary_key=True)
	user_id = Column(Integer, ForeignKey('user.id'))
	date = Column(DateTime, default=func.now())
	# class name
	# secondary = __tablename__
	foods = relationship(
		'food',
		secondary='food_fridge_link') 
	user = relationship(user, back_populates="fridgedays")
	def caloriesEaten(self):
		ret = 0
		for food in self.foods:
			ret += food.calories
		return ret
	
	def toJson(self):
		ret = "{"
		ret += '"individual":' + str(self.user_id) + ','
		ret += '"happened":' + str(int((self.date - epoch).total_seconds())) + ','
		ret += '"caloriesEaten":' + str(self.caloriesEaten()) + "}"
		return ret
		
class food(Base):
	__tablename__ = 'food'
	id = Column(Integer, primary_key=True)
	calories = Column(Integer)
	name = Column(String)
	def __init__(self, name, calories):
		self.name = name
		self.calories = calories
	def toStr(self):
		return self.name + " " + str(self.calories)
	
class foodFridgeLink(Base):
	__tablename__ = "food_fridge_link"
	id = Column(Integer, primary_key = True)
	food_id = Column(Integer, ForeignKey('food.id'))
	fridge_id = Column(Integer, ForeignKey('fridge_day.id'))
	
# This class defines our simple database table, god I love Object Relational Mappers.
# This is a fit_bit day.  
class fbDay(Base):
	__tablename__ = 'fit_bit_data'
	id = Column(Integer, primary_key=True)
	user_id = Column(Integer, ForeignKey('user.id'))
	user = relationship(user, back_populates="fitbitdays")
	date = Column(DateTime, default=func.now())
	steps = Column(Integer)
	caloriesOut = Column(Integer)
	totalDistance = Column(Float)
	elevation = Column(Integer)
	averageHR = Column(Float)
	totalMinutesAsleep = Column(Integer)
	totalSleepRecords = Column(Integer)
	totalTimeInBed = Column(Integer)
 
# this will create a sqlite database at fbData.sqlite on the hard drive, if one does not already exist
from sqlalchemy import create_engine
engine = create_engine('sqlite:///fbData.sqlite')

from sqlalchemy.orm import sessionmaker
session = sessionmaker()
session.configure(bind=engine)
Base.metadata.create_all(engine)

###################################################################################


# needed
s = session()

# Set up some random foods if they don't already exist.
if s.query(food).count() < 5:
	l = [food('Banana', 50), food('Apple', 100), food('Chicken', 300), food('Spaghetti', 500), food('Vodka', 100), food('Milk', 100)]
	for i in l:
		s.add(i)
	s.commit()
	
food_count = s.query(food).count()

# spawn some new data.
if s.query(user).count() < 1:
	john = user()
	john.name = 'John'
	s.add(john)
	s.commit()
	
john = s.query(user).filter(user.id == 1)[0]
f = fridgeDay()
f.foods += [s.query(food).filter(food.id == random.randint(1, food_count))[0] for i in range(0, 3)]
print [i.toStr() for i in f.foods]
john.fridgedays.append(f)

fb = fbDay()
fb.steps = random.randint(0, 20000)
fb.caloriesOut = random.randint(1500, 4000)
fb.totalDistance = random.randint(3, 15)
fb.elevation = random.randint(0, 10000)
fb.averageHR = random.randint(50, 120)
fb.totalMinutesAsleep = random.randint(0, 350)
fb.totalSleepRecords = random.randint(0, 10)
fb.totalTimeInBed = random.randint(0, 24 * 60)

john.fitbitdays.append(fb)

# will not recreate john if he already exists. 
# because we changed his fridgedays, it will also make sure that exists in the DB. 
s.add(john) 
s.commit()

# get everything that was created in the last day. 
fbDaysToPush = s.query(fbDay).filter(fbDay.date > datetime.now() - timedelta(days = 1)).all()
fridgeDaysToPush = s.query(fridgeDay).filter(fridgeDay.date > datetime.now() - timedelta(days = 1)).all()

for i in fbDaysToPush:
	# because it is mapped as individual on server side.
	# I do this the lazy way
	# The extra information hurts nothing. 
	i.individual = i.user_id
	x = to_json(i)
	x = addToken(x)
	print sendRequest(x, 'api/FitBit/AddFitBitDay')
	
for i in fridgeDaysToPush:
	i.individual = i.user_id
	x = i.toJson()
	x = addToken(x)
	print x
	print sendRequest(x, 'api/Fridge/AddFridgeDay')
