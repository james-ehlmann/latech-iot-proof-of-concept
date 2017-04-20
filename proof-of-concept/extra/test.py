import requests
import time
import sys

if '-h' in sys.argv or '--help' in sys.argv or len(sys.argv) < 2:
	print """
Usage: pythonbot.py URL_OF_WEBSERVICE
	"""
	sys.exit(0)
url = sys.argv[1]
if(url.endswith("/")):
	pass
else:
	url += "/"

init_request = requests.get(url + "api/Device/Request").json()

password = init_request['password']
id = init_request['id']

print 'id:' + str(id) + '\npassword:' + str(password)

for i in xrange(50):
	
	payload = '{"password":"' + password + '", "id":' + str(id) + ',"data": "test ' + str(i) + '"}'
	print payload
	headers = {'content-type': 'application/json'}
	r = requests.post(url + "api/Entry/AddData", data=payload, headers = headers)
	print r
	time.sleep(5)