# TODO

- tdd a json api for loading map and run files
- the api will load up stats

StatsEndPoint
-number of streets
-number of streets covered

- can all be done in-memory using the owin testserver
- unit test the actual domain for the algorithm parts

- display all of london (define london)
- cut london into parts (borough, then postcode)
- display it nicely (openstreetmap?)
- load runs from strava and calculate area covered
- osm files have many things besides roads. should first clean them when read in
- improve covered algorithm accuracy

- host it on heroku and show asier
- have all of london loaded up and connect to strava
- need to persist run results to disk or a nosql store


## end goal
- given an area of london, how much have i covered in my runs?
- load east london
- load a run
- output % of streets covered

## stats

- on this map there are # of streets
- you have run on # of streets (%)
- cut up streets by postcode ... then zone ...

- add street names to the svg
- highlight streets as you hover
- hook into strava apis to download historical runs
- start to optimise this
- put it onto heroku

=============

Map -> can come from OSD or persisted... should we persist OSD files? sounds like a plan!
Map has a list of covered streets... because this is the slowest part, could persist this. have a file which lists covered streets, build the map from this. could hash the runs and only process new ones...

