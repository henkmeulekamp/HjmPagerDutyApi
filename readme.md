# Pagerduty API Client#

Simple C# dotnet client for pagerduty.

I needed some code to have services and systems directly report production incidents to Pagerduty.com. Their email integration works fine, but this API client will have the added ability to set problems to resolved state when system finds out that incident is done.

There is a project already on nuget, but that one has a dependency on json.net and restsharp, specific versions which dont play well with other parts in our system.

## New Cli interface ##

For integration with a monitoring tool with limited options to call external APIs I added a CLI tool. With this you can resolve, acknowledge and open incidents using command line. See PdIncidents project. Commandline options:  
 - c: config, config gile
 - s: subject, subject of incident
 - m: message, message/details of incident
 - a: action, Raise, Acknowledge, Resolve

Example:
`PdIncidents.exe -c c:\config\pagerduty.config -a Raise -s "New IP Monitoring Incident" -m "company.com down" `

Then to resolve it:
`PdIncidents.exe -c c:\config\pagerduty.config -a Resolve -s "New IP Monitoring Incident"`


## Dependencies ##

It has the following nuget dependencies:

- servicestack.text version v3.9.71 (should be easy to replace with any other json library)

## Future plans ##

.. if time permits ..

- PCL version
- publish to nuget
- implement full api, not only creating and resolving incidents
- get exising open incidents and add new problems of the same type as logs to those instead of opening new incidents
- async http calls
