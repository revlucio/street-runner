# TODO

need to think of the actual UX and UI...
want to hook into strava and then show a map with all the streets you have covered
can start with east london map
go through run by run, persisting the list of streets covered
as a street is covered remove the points from the map to speed it up
can be run in parallel to speed it up too
want to list street names and plan routes in the future
for now get my local area and start running all the streets in the area to get a nice map

FEATURES:
- persisting the runs for speed
    - this will need to happen to scale, so can work on this
    - probably just use the OSM format and annotate it
    - setup a pipeline for OSM:
        - strip out what we dont need
        - run by run, add in streets covered

- browsing the api
    - root should be the stava auth / map view
    - list all runs in a folder, click a button to process each one
    - results are persisted and can be cleared (but maybe dont have to persist on disk if fast enough...)
    - but if there are hundreds of runs will def have to persist, might as well
- strava auth
- handling multiple runs


## MVP TO SHOW ASIER

- website hosted somewhere
- enter strava details
(curl -G https://www.strava.com/api/v3/activities/1144313347 -H "Authorization: Bearer 93944545f252e152f5aeb0128fcca26760eadd01")
start with hard-coded request

curl -G https://www.strava.com/api/v3/activities/1144313347/streams/latlng -H "Authorization: Bearer 93944545f252e152f5aeb0128fcca26760eadd01"

- shows map of london and the streets youve covered

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

