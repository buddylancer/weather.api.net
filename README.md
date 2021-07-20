# weather.api.net
Simple Web API for weather requests

Following solutions are included:

**ServiceSolution.sln**

WcfService - Web API application, will launch service on fixed port 59813.
Weather API key & Google API key must be provided in config settings.

**TestSolution.sln**

ConsoleApplication - test console application, type country & postcode manually.

WebApplication - test WebForms application, enter country & postcode into form fields.

WinApplication - test WinForms application, enter country & postcode into form fields.

TestApplication - simple unit testing application, pass through a set of negative and positive test cases.

All applications will consume the service launched prevously on port 59813.

