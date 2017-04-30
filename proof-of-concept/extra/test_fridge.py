import requests
import time
import sys

# Response 204 means that it doesn't need to justify us with a response
# Response 400 means that our request was bad
# Response 415 means that our response wasn't even the correct type of data.
url = "http://localhost:51186/"

init_request = requests.get(url + "api/Device/Request").json()

password = init_request['password']
id = init_request['id']

print 'id:' + str(id) + '\npassword:' + str(password)

for i in xrange(50):
	
	payload = '{"individual":2, "happened":2, "caloriesEaten":2000, "id":{"password":"' + password + '", "id":' + str(id) + ',"data": "test ' + str(i) + '"}}'
	print payload
	headers = {'content-type': 'application/json'}
	r = requests.post(url + "api/Fridge/AddFridgeDay", data=payload, headers = headers)
	print r
	time.sleep(5)
