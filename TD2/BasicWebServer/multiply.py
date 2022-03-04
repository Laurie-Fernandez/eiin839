import sys

def main():

	operations = {
		"multiply": lambda a, b: a * b
	}
	
	result = ""
	
	if len(sys.argv) < 4 :
		result = "3 parameters should be given"
	else:
		if sys.argv[1] in operations:
			try:
				result = f"The result of the method is {operations[sys.argv[1]](int(sys.argv[2]), int(sys.argv[3]))}"
			except:
				result = "Error of format"
		else:
			result = "You have to give a defined method in parameter (multiply)"
			
	print(f"<!DOCTYPE html><html><body>{result}</body></html>")
	
	
main()