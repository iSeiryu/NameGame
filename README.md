# The Name Game
Leading scientists have proven, via science, that learning your coworker’s names while starting a new job is useful. This is an API that makes it possible to implement a full-featured game on top of it. The rules are simple: the game will present the client/user with six faces and ask them to identify the listed name.


## Implemented Features
Game modes:
- one name and N faces
- one face and N names

Statistics:
- How long does it take on average to identify the subject? 
- How long does it take on average for a person to identify the subject?
- How many attempts did the user make to solve the last challenge?
- How long did it take to solve the last challenge?


## Setup API locally
.NET Core 2.2 or above is required to build and run the project: https://dotnet.microsoft.com/download

**Clone**
```
cd path/to/repo
git clone https://github.com/iseiryu/NameGame.git
```
**Build**
```
cd path/to/repo/NameGame
dotnet build
```
**Run**
```
dotnet run -p NameGame.API\NameGame.API.csproj --launch-profile NameGame -c Release
```
**Test**
```
dotnet test
```

**Postman**

A Postman collection is provided to make the testing of the API easier: https://github.com/iSeiryu/NameGame/tree/master/Postman

**Swagger**

Swagger is also available to check how the API is designed.

**Hosted version**

The API is hosted here: https://namegame.azurewebsites.net

The Postman collection contains a set of environment variables to switch between localhost and azure endpoints.


## Design Decisions
Since the list of the requirements was not very precise and there is no one available to clarify any details it left a lot of room for the assumptions.

There are 2 game controllers that show different approaches to building APIs:
- GameController that represents a game controller/manager that could run different game related processes;
- ChallengeController that represents the HTTP resource with implemented CRUD using the proper HTTP verbs and HTTP statuses.

The latter one could utilize the same route for all of the actions and accept the game_mode as a parameter. It would require to add the game mode/type to the DB as well. In the real application it would also have mappers to separate the DB objects from the DTO and Business objects. It was chosen to not implement those things in this version for the simplicity of the code.

It's possible to have more than 2 game modes. In that case (given that other requirements do not contradict it) we could create a known list of the modes and have a single endpoint and a ChallengeFactory to generate a challenge for the selected mode.

Providing tests was not a part of the requirements so to save time it was decided to only provide a partial coverage. In the real application would be close to 100% coverage with unit-tests and at least 50% coverage with integration tests. The selected DB framework allows to run the DB in-memory which would help to wrap the project with more realistic tests. The selected HTTP requests framework also allows to mock the external endpoint in-memory.

Most of the constants are stored as static variables. In the real (especially in a multilingual) application all of the strings would come from some kind of (localization) resource and the options (number of faces, duration of cache storage, etc) would come from a configuration (appsettings, DB, API, etc).

The real application most likely would have a more complex logging mechanism. The logging was added to show the importance of it and how to cover the code that uses the logger with unit-tests.

**Performance**

The most performant way to implement the answer check would be to send the correct answer along with the challenge to the client. But the requirements mentioned that the game logic should be a part of the API. Originally to speed things up I created a hashmap to store the list of {name, image} values. But for the statistics purposes it was simpler to keep the correct answers in the DB. To improve game process performance further we could run a daily/weekly/monthly long process to create a large set of challenges upfront and store them in the DB. And then we give the same set of challenges to all users and track the progress of each user separately (a Level-based Progression).
There are other things that could be tweaked to improve the performance of the application but it would require more input about the environment it's going to be hosted at and the usage data.

**Authentication/Authorization**

With the current framework the easiest way to enable it is to add the built-in Authentication middleware and then cover the needed Controllers/Actions with the [Authorize] attribute. It would create all needed user related tables in our DB with login/pass scenario and provide a Cookie based authentication. If the project implemented authentication it would read the userName/userId from the system Identity property which would eliminate the need of passing the userName to the endpoints.

The general approach is:

- to let the user to Sign-Up on your resource with a login/pass pair;
- create a new record for the user in our User table;
- store login/pass as some encrypted value;
- when the user Sings-In get the encrypted value from the entered login/pass and compare to the one in the User table;
- if authentication is successful generate a token/session value and store it on the client's side (e.g. in Cookies);
- if authentication failed return 401.

We can also use an authentication-as-a-service provider: Google, FB, Github, etc. Usually those providers use OAuth2 but there are other ways to accomplish it. In that case we let the user to authenticate against the external provider and then use the provider to verify that the current token/session/etc is still valid. In a more complex scenario we could also use Roles and Claims.
