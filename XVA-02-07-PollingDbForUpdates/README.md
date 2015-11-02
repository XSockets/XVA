##XVA-02-07 Polling Legacy Database



### Installed Nuget Packages

- XSockets
- XSockets.JsApi

Other resources
- Wijmo
- Knockoutjs
- jQuery
- Ninject
- Bootstrap
- SpongeBob

### Description

Polling is ugly, but if you do not control a database or service your self you may have to do it... It is better to have one resource doing polling and then notify users when anything has happend instead of having hundreds or thousands of clients polling the same resource.

Note that example https://github.com/XSockets/XVA/tree/master/XVA-02-04-DataSyncBasic2 is a good alternative or possible to combine with this technique.




A realtime communication demo based on XSockets.NET, Wijmo, KnockoutJS, Bootstrap and EntityFramework CodeFirst MSSQL as DataSource 



This sample was built a back in January 2013 as an answer to a question on SO, but was updated recently to XSockets 4
Stackoverflow: http://stackoverflow.com/questions/14319758/signalr-polling-database-for-updates/14326800#14326800


The project was built as a proof of concept on what you can do with XSockets.NET
as a realtime backend.


##Screenshot


![DbPolling Sample](https://raw.githubusercontent.com/XSockets/XVA/master/XVA-02-07-PollingDbForUpdates/screenshot.PNG