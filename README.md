# basics
This is a c# 8.0 system using azure cosmos db. Just replace the connection string in the appsettings json with yours and it'll handle the rest, you dont need to do any manual sync
# How auth works and how to modify
Auth is a homebrewed solution because i couldnt be asked to work out how anyone elses system. Uses bcrypt to encrypt the passwords, middleware/AuthenticationFilter, basic auth string setup, i'm sure you can work out how to re-create it on the client side tom. 
# How to setup models
To make a model able to be saved into the database, all you have to do is inheriet from basedatabasemodel, and then we'll be using system.text.json so, use those libraries if you want to change the json structure of the stuff like json name and shit.
# How db layer works
Db is setup with a basic filter to automatically filter out inactive, partition key is the key that's used to help distinguish the data, you want this in relatively big chunks, but not too big.
# How controllers works 
To make a basic crud controller, all you have to do is create a new controller, setup the route attribute and the api controller attribute, and inheriet from BaseCrudController<withTheTypeBeingTheTypeYOuWnatToSave>. If you want to have good swagger responses, do what i did in player controller to just call the base. To make something admin add [admin] to the controller/property. The rest is prettyself evident, database structure gives you a lot of control, feel free to grab me if you get stuck tom
