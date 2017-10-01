# TODO - and notes

## end goal
    - given an area of london, how much have i covered in my runs?
    - load east london
    - load a run
    - output % of streets covered

## areas to focus on

- converting from osm/gpx to Map
- outputting a nice svg
- calculating what streets are covered by the runs (perf)
- outputting statistics



- add street names to the svg
- highlight streets as you hover
- hook into strava apis to download historical runs
- start to optimise this
- put it onto heroku

## performance

small-east-london.osm:
start - 4.6s
AsParallel() - 3.4s
ToList in FromOsm() so xml is not read multiple times - 2.2s
using a dict for lookup - .1s !!!

Xml reading is 99% of the time! dont need to AsParallel anymore!

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

need to design a json api that is fully tested

need to have tighter control on the boundaries