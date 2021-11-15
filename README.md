# Machine-learning-based-automated-quantification-machine-for-virus-plaque-assay-counting

## Getting Started
| Name | Required version(s) | Description |
|------|---------------------| ------------|
| Mvtech_halcon| STEADY EDITION 20.11 | ML-based software for image viral plaque counting.|
| Solitech | Put in C directory | A web app for Using GUI of Automated Quantification|

* Note that for Linux/MacOS the commands are usually python3 and pip3 instead of python and pip respectively.

1. Clone this project to your machine(computers).
``` 
git clone https://github.com/kinkinkinxd/YourFitnessPal.git
```
2. Go to the directory where you clone the project.
```
cd YourFitnessPal
```
3. Install virtualenv.
```
python -m pip install virtualenv
```
4. Generate new virtual enviroment. (For Window OS)
```
virtualenv env
```
(For MacOS/Linux)
```
virtualenv venv
```
5. Activate virtualenv

go to env directory and then activate it

For Window OS
```
cd env
Scripts\activate
```
after you did this section you should see (env) in your terminal
```
(env) C:\user\YourFitnessPal\env>
```
for MacOs and Linux
```
source venv/bin/activate
```
6. Go out from the env directory
```
(env) C:\user\YourFitnessPal\env>cd..	
# we should see terminal like below
(env) C:\user\YourFitnessPal>
```
7. Enter this command to install all require packages.
``` 
pip install -r requirements.txt 
```
8. Enter this command to migrate the database.
``` 
python manage.py migrate 
```
9. Enter this command to initiate initial user accounts.
``` 
python manage.py loaddata users.json
```
10. Enter this command to start running the server.
``` 
python manage.py runserver 
```
11. Login to the web application using this given demo account.

|Username | Password |
|-------------|:----------:|
|  sample | hellosample |
12. Enter this command when you are done with the website

```
deactivate 
```