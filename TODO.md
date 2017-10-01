# TODO

GOAL:
    - given an area of london, how much have i covered in my runs?
    - load east london
    - load a run
    - output % of streets covered

- need a better UI for looking into the files
    - 

- crop the streets to just ones within the run box

- calculate which streets are covered by the run
    - basic unit tests with calculation X
    - highlight the roads with colours in svg X
    - improve complexity of 'covered' calculation

- BAM thats the MVP!

- add street names to the svg
- highlight streets as you hover
- hook into strava apis to download historical runs
- start to optimise this
- put it onto heroku

## performance

Currently to render 1 run in east london (with cached min/max) is 151s

ideas:
    - cache the min/max
    - cache the scaling of points that are rendered
    - when calculating covered streets exclude points that are far away
    - run things async !

probably need to put in some unit tests around perf...

## design

domain:
    - StreetMap
        give it an osm file and a list of gpx files
        can output stats on how many streets are covered
        can output svg file to visualise this
    - StreetMapBuilder
        give it filenames and it returns svg
    - OsmToStreetsConverter
    - GpxToRunsConverter
