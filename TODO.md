# TODO

## FEATURES

- take into account the length of roads in the covered %
    - streets are made up of segments
    - each segment has a calculated length
    - then the % covered can be calculated

- loading in a map file and trimming it down via the api
- add a page to view all runs, the dates, and if theyre in london
- add a timeline showing % going up over time
- make a single page with SVG and stats that uses multiple endpoints¡
- offline mode
    - start by finding out where docker stores files
    - map to filesystem so we can cache outside of the app
    
## BUGS

- unathorised isnt handled properly


## PERFORMANCE

- start caching the map object
- add caching headers
- process the runs in the background, add a spinner
- progressively add the runs as discovered
- get e2e tests working against a docker instance
- get a bigger map file for london
- trim down the map file

- maybe convert the lat/lon into x/y to make svg and distance calc easier?
  prob dont need to take into account the curvature of the earth if its just london
  can then cache these x,y points instead of lat lon
  problem is what does x y mean? what offset? and when to convert?
