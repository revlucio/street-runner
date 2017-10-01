# TODO

- build a simple web host that renders the SVG X
    - extend this so you can choose osm files / gpx files

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