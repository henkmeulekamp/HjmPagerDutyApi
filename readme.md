# Pagerduty API Client#

Simple C# dotnet client for pagerduty.

I needed some code to have services and systems directly report production incidents to Pagerduty.com. Their email integration works fine, but this API client will have the added ability to set problems to resolved state when system finds out that incident is done.

There is a project already on nuget, but that one has a dependency on json.net and restsharp, specific versions which dont play well with other parts in our system.


## Dependencies ##

It has the following nuget dependencies:

- servicestack.text version v4.* (should be easy to replace with any other json library)

## Future plans ##

.. if time permits ..

- PCL version
- publish to nuget
- implement full api, not only creating and resolving incidents
- get exisint incidents and add new problems as logs to those instead of opening new incidents
- async http calls
